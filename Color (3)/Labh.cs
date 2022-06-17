using System;
using Imagin.Core.Numerics;

using static System.Double;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <para><b>Lightness (L), a, b</b></para>
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
[Component(100, '%', "L", "Lightness"), Component(-100, 100, ' ', "a"), Component(-100, 100, ' ', "b")]
[Serializable]
public class Labh : ColorModel3<XYZ>
{
    public Labh() : base() { }

    /// <summary>Computes the <b>Ka</b> parameter.</summary>
    public static double ComputeKa(Vector3 whitePoint)
    {
        if (whitePoint == (XYZ)(xyY)(xy)Illuminant2.C)
            return 175;

        var Ka = 100 * (175 / 198.04) * (whitePoint.X + whitePoint.Y);
        return Ka;
    }

    /// <summary>Computes the <b>Kb</b> parameter.</summary>
    public static double ComputeKb(Vector3 whitePoint)
    {
        if (whitePoint == (XYZ)(xyY)(xy)Illuminant2.C)
            return 70;

        var Ka = 100 * (70 / 218.11) * (whitePoint.Y + whitePoint.Z);
        return Ka;
    }

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="Labh"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
    {
        double X = input.X, Y = input.Y, Z = input.Z;
        double Xn = profile.White.X, Yn = profile.White.Y, Zn = profile.White.Z;

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

    /// <summary>(🗸) <see cref="Labh"/> > <see cref="XYZ"/></summary>
    public override void To(out XYZ result, WorkingProfile profile)
    {
        double L = X, a = Y, b = Z;
        double Xn = profile.White.X, Yn = profile.White.Y, Zn = profile.White.Z;

        var Ka = ComputeKa(profile.White);
        var Kb = ComputeKb(profile.White);

        var y = Pow(L / 100.0, 2) * Yn;
        var x = (a / Ka * Sqrt(y / Yn) + y / Yn) * Xn;
        var z = (b / Kb * Sqrt(y / Yn) - y / Yn) * -Zn;
        result = Colour.New<XYZ>(x, y, z);
    }
}