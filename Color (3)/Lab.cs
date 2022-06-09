using Imagin.Core.Numerics;
using System;

using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>Lightness (L*), a*, b*</b></para>
/// 
/// <para>A perceptually uniform color space that measures perceptual lightness and the four unique colors of human vision (red, green, blue, and yellow) where a given numerical change corresponds to a similar perceived change in color.</para>
/// 
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Lab"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>CIELAB</item>
/// <item>L*a*b*</item>
/// </list>
/// 
/// <i>Author</i>
/// <list type="bullet">
/// <item><see cref="CIE"/> (1976)</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(100, '%', "L*", "Lightness"), Component(-100, 100, ' ', "a*"), Component(-100, 100, ' ', "b*")]
[Serializable]
public class Lab : XYZ, ILAb
{
    public Lab(params double[] input) : base(input) { }

    public static implicit operator Lab(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="Lab"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double L = Value[0], a = Value[1], b = Value[2];

        var fy = (L + 16) / 116d;
        var fx = a / 500d + fy;
        var fz = fy - b / 200d;

        var fx3 = Pow(fx, 3);
        var fz3 = Pow(fz, 3);

        var xr = fx3 > CIE.IEpsilon ? fx3 : (116 * fx - 16) / CIE.IKappa;
        var yr = L > CIE.IKappa * CIE.IEpsilon ? Pow((L + 16) / 116d, 3) : L / CIE.IKappa;
        var zr = fz3 > CIE.IEpsilon ? fz3 : (116 * fz - 16) / CIE.IKappa;

        return new XYZ(xr * profile.White[0], yr * profile.White[1], zr * profile.White[2]).ToLrgb(profile);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="Lab"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var xyz = new XYZ();
        xyz.FromLrgb(input, profile);

        double Xr = profile.White[0], Yr = profile.White[1], Zr = profile.White[2];
        double xr = xyz[0] / Xr, yr = xyz[1] / Yr, zr = xyz[2] / Zr;

        static double f(double cr)
            => cr > CIE.IEpsilon
            ? Pow(cr, 1 / 3d)
            : (CIE.IKappa * cr + 16) / 116d;

        var fx = f(xr); var fy = f(yr); var fz = f(zr);

        var l = 116 * fy - 16;
        var a = 500 * (fx - fy);
        var b = 200 * (fy - fz);
        Value = new(l, a, b);
    }
}