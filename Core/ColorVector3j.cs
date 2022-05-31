using Imagin.Core.Numerics;

using static System.Double;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>A transformative <see cref="ColorVector3"/> that is used as an endpoint in forming a derivative color space.</summary>
/// <inheritdoc/>
public abstract class ColorVector3j
{
    public Vector3 Value { get; protected set; }

    public ColorVector3j(Vector3 input = default) : base() => Value = input;

    public static implicit operator Vector(ColorVector3j input) => input.Value;

    public static implicit operator Vector3(ColorVector3j input) => input.Value;

    public abstract void From(Vector3 input);

    public abstract Vector3 To();
}

#region (🗸) [100.0%] LCH

/// <summary>Lightness (L), Chroma (C), Hue (H)</summary>
/// <remarks><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > 🞶 > <see cref="LCH"/></remarks>
public sealed class LCH : ColorVector3j
{
    public LCH(Vector3 input = default) : base(input) { }

    /// <summary>Returns saturation of the color (chroma normalized by lightness).</summary>
    public static double GetSaturation(double L, double C)
    {
        var result = 100 * (C / L);

        if (IsNaN(result))
            return 0;

        return result;
    }

    /// <summary>Gets chroma from saturation and lightness.</summary>
    public static double GetChroma(double saturation, double L) => L * (saturation / 100);

    //...

    /// <remarks>[🞶] > [<see cref="LCH"/>]</remarks>
    public override void From(Vector3 input)
    {
        double a = input.Y, b = input.Z;

        var hr = Atan2(b, a);
        var h = Angle.NormalizeDegree(Angle.GetDegree(hr));

        var c = Sqrt(a * a + b * b);
        Value = new(input.X, c, h);
    }

    /// <remarks>[<see cref="LCH"/>] > [🞶]</remarks>
    public override Vector3 To()
    {
        double c = Value.Y, h = Value.Z;
        h = Angle.GetRadian(h);

        var a = c * Cos(h);
        var b = c * Sin(h);
        return new(Value.X, a, b);
    }
}

#endregion

#region (🞩) [50.00%] HUV

/// <summary>
/// Hue (H), Chroma (U), Lightness (V)
/// <para><i>Author</i>: <see href="https://github.com/imagin-tech">Imagin</see> (2022)</para>
/// </summary>
/// <remarks><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > 🞶 > <see cref="HUV"/></remarks>
[Unfinished]
public sealed class HUV : ColorVector3j
{
    public double Hue => Value.X;

    public double Chroma => Value.Y;

    public double Lightness => Value.Z;

    public HUV(Vector3 input = default) : base(input) { }

    public static double GetDistance(double x1, double y1, double x2, double y2)
        => Sqrt(Pow(Abs(x1 - x2), 2) + Pow(Abs(y1 - y2), 2));

    /// <remarks>[🞶] > [<see cref="HUV"/>]</remarks><returns>[0, 100]</returns>
    [Unfinished]
    public override void From(Vector3 input)
    {
        double t = Atan2(input.Z, input.Y);

        double m = GetDistance(0.5, 0.5, 1, 1);
        double n = t / 2 / PI;

        double h = (Sqrt(Pow((n * m) / PI * 2, 2) - Pow(Abs(0.5 - input.X), 2)) - 0.5) / -1;
        double u = Sqrt(input.Y * input.Y + input.Z * input.Z);
        double v = input.X;
        Value = new(h, u, v);
    }

    /// <remarks>[<see cref="HUV"/>] > [🞶]</remarks><returns>[-100, 100]</returns>
    public override Vector3 To()
    {
        double m = GetDistance(0.5, 0.5, 1, 1);
        double n = Min(GetDistance(0.5, 0.5, Value.X / 360, Value.Z / 100) / m, 1);
        double t = n * (PI * 2);

        double a = Value.Y * Cos(t);
        double b = Value.Y * Sin(t);
        return new(Value.Z, a, b);
    }
}

#endregion