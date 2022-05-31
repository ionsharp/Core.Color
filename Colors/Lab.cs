using Imagin.Core.Numerics;
using System;

using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🞩) <b>Lightness (L*), a*, b*</b></para>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Lab"/></para>
/// 
/// <para>🞩 <i>Why is only one hue seemingly displayed?</i></para>
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
[Component(0, 100, '%', "L*", "Lightness")]
[Component(-100, 100, ' ', "a*")]
[Component(-100, 100, ' ', "b*")]
[Serializable]
public sealed class Lab : XYZVector
{
    public Lab(params double[] input) : base(input) { }

    public static implicit operator Lab(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="Lab"/> > <see cref="XYZ"/></summary>
    public override XYZ ToXYZ(WorkingProfile profile)
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

        return new(xr * profile.White[0], yr * profile.White[1], zr * profile.White[2]);
    }

    /// <summary><see cref="XYZ"/> > <see cref="Lab"/></summary>
    public override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        double Xr = profile.White[0], Yr = profile.White[1], Zr = profile.White[2];

        double xr = input[0] / Xr, yr = input[1] / Yr, zr = input[2] / Zr;

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