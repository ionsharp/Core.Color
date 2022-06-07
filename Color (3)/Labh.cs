using Imagin.Core.Numerics;
using System;

using static System.Double;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>Lightness (L), a, b</b></para>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Labh"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Hunter Lab</item>
/// </list>
/// 
/// <i>Author</i>
/// <list type="bullet">
/// <item>Richard S. Hunter (1948)</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 100, '%', "L", "Lightness"), Component(-100, 100, ' ', "a"), Component(-100, 100, ' ', "b")]
[Serializable]
public class Labh : XYZ, ILAb
{
    public Labh(params double[] input) : base(input) { }

    public static implicit operator Labh(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>Computes the Ka parameter.</summary>
    public static double ComputeKa(XYZ whitePoint)
    {
        if (whitePoint == Illuminant.GetWhite(Illuminant2.C))
            return 175;

        var Ka = 100 * (175 / 198.04) * (whitePoint.X + whitePoint.Y);
        return Ka;
    }

    /// <summary>Computes the Kb parameter.</summary>
    public static double ComputeKb(XYZ whitePoint)
    {
        if (whitePoint == Illuminant.GetWhite(Illuminant2.C))
            return 70;

        var Ka = 100 * (70 / 218.11) * (whitePoint.Y + whitePoint.Z);
        return Ka;
    }

    /// <summary>(🗸) <see cref="Labh"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double L = Value[0], a = Value[1], b = Value[2];
        double Xn = profile.White.X, Yn = profile.White.Y, Zn = profile.White.Z;

        var Ka = ComputeKa(profile.White);
        var Kb = ComputeKb(profile.White);

        var Y = Pow(L / 100d, 2) * Yn;
        var X = (a / Ka * Sqrt(Y / Yn) + Y / Yn) * Xn;
        var Z = (b / Kb * Sqrt(Y / Yn) - Y / Yn) * -Zn;
        return new XYZ(X, Y, Z).ToLrgb(profile);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="Labh"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var xyz = new XYZ();
        xyz.FromLrgb(input, profile);

        double X = xyz.X, Y = xyz.Y, Z = xyz.Z;
        double Xn = profile.White[0], Yn = profile.White[1], Zn = profile.White[2];

        var Ka = ComputeKa(profile.White);
        var Kb = ComputeKb(profile.White);

        var L = 100 * Sqrt(Y / Yn);
        var a = Ka * ((X / Xn - Y / Yn) / Sqrt(Y / Yn));
        var b = Kb * ((Y / Yn - Z / Zn) / Sqrt(Y / Yn));

        if (IsNaN(a))
            a = 0;

        if (IsNaN(b))
            b = 0;

        Value = new(L, a, b);
    }
}