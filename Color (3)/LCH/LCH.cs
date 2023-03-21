using Imagin.Core.Numerics;
using static Imagin.Core.Numerics.M;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Luminance (L), Chroma (C), Hue (H)</b>
/// <para>A cylindrical <see cref="ColorModel"/> that is designed to accord with the human perception of color.</para>
/// </summary>
[Component(100, '%', "L", "Luminance"), Component(100, '%', "C", "Chroma"), Component(360, '°', "H", "Hue")]
public abstract class LCH<T> : ColorModel3 where T : ColorModel3, new()
{
    public LCH() : base() { }

    ///

    static Vector3 FromLChx(Vector3 i)
    {
        double u = GetDistance(i.Z / 360, i.Y / 100);
        double v = GetDistance(i.Z / 360, i.X / 100);
        return new Vector3(i.X, i.Y, Clamp(i.Z * u / v, 359));
    }

    static Vector3 FromLChy(Vector3 i)
    {
        double w = GetDistance(i.Y / 100, i.X / 100);
        return new Vector3(i.X, i.Y, i.Z * w);
    }

    static Vector3 FromLChz(Vector3 i)
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

    ///

    /// <summary>(🗸) <see cref="LCH{T}"/> > <see cref="LCH{T}">T</see></summary>
    public virtual Vector3 FromLCh(Vector3 input)
    {
        double c = input.Y, h = input.Z;
        h = Angle.GetRadian(h);

        var a = c * Cos(h);
        var b = c * Sin(h);
        return new(input.X, a, b);
    }

    /// <summary>(🗸) <see cref="LCH{T}">T</see> > <see cref="LCH{T}"/></summary>
    public virtual Vector3 ToLCh(Vector3 input)
    {
        double a = input.Y, b = input.Z;

        var hr = Atan2(b, a);
        var h = Angle.NormalizeDegree(Angle.GetDegree(hr));

        var c = Sqrt(a * a + b * b);
        return new(input.X, c, h);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="LCH{T}">T</see> > <see cref="LCH{T}"/></summary>
    public override void From(Lrgb input, WorkingProfile profile)
    {
        var result = new T();
        result.From(input, profile);

        Value = ToLCh(result);
    }

    /// <summary>(🗸) <see cref="LCH{T}"/> > <see cref="LCH{T}">T</see> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
        T result = new();
        result.From(FromLCh(Value));

        return result.To(profile);
    }
}