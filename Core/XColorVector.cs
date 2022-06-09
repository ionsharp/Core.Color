using Imagin.Core.Numerics;
using System;

using static Imagin.Core.Numerics.M;
using static System.Math;

namespace Imagin.Core.Colors;

[Serializable]
public static class XColorVector
{
    public enum Series { X, Y, Z }

    //...

    /// <summary>(🗸) <see cref="ILChi"/> (<see cref="Series.X"/>) > <see cref="ILCh"/></summary>
    static Vector3 FromLChx(ILChi i)
    {
        double u = GetDistance(i.Z / 360, i.Y / 100);
        double v = GetDistance(i.Z / 360, i.X / 100);
        return new Vector3(i.X, i.Y, Clamp(i.Z * u / v, 359));
    }

    /// <summary>(🗸) <see cref="ILChi"/> (<see cref="Series.Y"/>) > <see cref="ILCh"/></summary>
    static Vector3 FromLChy(ILChi i)
    {
        double w = GetDistance(i.Y / 100, i.X / 100);
        return new Vector3(i.X, i.Y, i.Z * w);
    }

    /// <summary>(🗸) <see cref="ILChi"/> (<see cref="Series.Z"/>) > <see cref="ILCh"/></summary>
    static Vector3 FromLChz(ILChi i)
    {
        double l = i.X / 100;
        double c = i.Y / 100;
        double h = i.Z / 360;

        double u = GetDistance(h, c);
        double v = GetDistance(h, l);

        double x = GetDistance(c, c);
        double y = GetDistance(c, l);
        return new Vector3(i.X, Clamp(i.Y * x / y, 100), Clamp(i.Z * u / v, 359));
    }

    //...

    /// <summary>(🞩) <see cref="ILCh"/> > <see cref="ILChi"/> (<see cref="Series.X"/>)</summary>
    static Vector3 ToLChx(ILCh i) => default;

    /// <summary>(🞩) <see cref="ILCh"/> > <see cref="ILChi"/> (<see cref="Series.Y"/>)</summary>
    static Vector3 ToLChy(ILCh i) => default;

    /// <summary>(🞩) <see cref="ILCh"/> > <see cref="ILChi"/> (<see cref="Series.Z"/>)</summary>
    static Vector3 ToLChz(ILCh i) => default;

    //...

    /// <summary>(🗸) <see cref="IHWb"/> > <see cref="IH"/></summary>
    public static Vector3 FromHWb(this IHWb input)
    {
        var s = 1 - (input.Y / (1 - input.Z));
        var b = 1 - input.Z;
        return new(input.X, s, b);
    }

    /// <summary>(🗸) <see cref="ILCh"/> > <see cref="ILAb"/></summary>
    public static Vector3 FromLCh(this ILCh input)
    {
        double c = input.Y, h = input.Z;
        h = Angle.GetRadian(h);

        var a = c * Cos(h);
        var b = c * Sin(h);
        return new(input.X, a, b);
    }

    /// <summary>(🗸) <see cref="ILChi"/> > <see cref="ILCh"/></summary>
    public static Vector3 FromLChi(this ILChi input, Series series)
    {
        Vector3 result = default;
        switch (series)
        {
            case Series.X: result = FromLChx(input); break;
            case Series.Y: result = FromLChy(input); break;
            case Series.Z: result = FromLChz(input); break;
        }
        return result;
    }

    //...

    /// <summary>(🗸) <see cref="IH"/> > <see cref="IHWb"/></summary>
    public static Vector3 ToHWb(this IH input)
    {
        double w = (1 - input.Y) * input.Z;
        double b = 1 - input.Z;
        return new(input.X, w, b);
    }

    /// <summary>(🗸) <see cref="ILAb"/> > <see cref="ILCh"/></summary>
    public static Vector3 ToLCh(this ILAb input)
    {
        double a = input.Y, b = input.Z;

        var hr = Atan2(b, a);
        var h = Angle.NormalizeDegree(Angle.GetDegree(hr));

        var c = Sqrt(a * a + b * b);
        return new(input.X, c, h);
    }

    /// <summary>(🗸) <see cref="ILCh"/> > <see cref="ILChi"/></summary>
    public static Vector3 ToLChi(this ILCh input, Series series)
    {
        Vector3 result = default;
        switch (series)
        {
            case Series.X: result = ToLChx(input); break;
            case Series.Y: result = ToLChy(input); break;
            case Series.Z: result = ToLChz(input); break;
        }
        return result;
    }
}