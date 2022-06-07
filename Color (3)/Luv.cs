using Imagin.Core.Numerics;
using System;
using static System.Double;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>Lightness (L*), u*, v*</b></para>
/// 
/// <para>An Adams chromatic valence color space that attempts perceptual uniformity (the successor to UVW).</para>
/// 
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Luv"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>CIELUV</item>
/// <item>L*u*v*</item>
/// </list>
/// 
/// <i>Author</i>
/// <list type="bullet">
/// <item><see cref="CIE"/> (1976)</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 100, '%', "L*", "Lightness"), Component(-134, 224, ' ', "u*"), Component(-140, 122, ' ', "v*")]
[Serializable]
public class Luv : XYZ, ILAb
{
    public Luv(params double[] input) : base(input) { }

    public static implicit operator Luv(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="Luv"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        static double Compute_u0(XYZ input) => 4 * input.X / (input.X + 15 * input.Y + 3 * input.Z);
        static double Compute_v0(XYZ input) => 9 * input.Y / (input.X + 15 * input.Y + 3 * input.Z);

        double L = Value[0], u = Value[1], v = Value[2];

        var u0 = Compute_u0(profile.White);
        var v0 = Compute_v0(profile.White);

        var Y = L > CIE.IKappa * CIE.IEpsilon
            ? Pow((L + 16) / 116, 3)
            : L / CIE.IKappa;

        var a = (52 * L / (u + 13 * L * u0) - 1) / 3;
        var b = -5 * Y;
        var c = -1 / 3d;
        var d = Y * (39 * L / (v + 13 * L * v0) - 5);

        var X = (d - b) / (a - c);
        var Z = X * a + b;

        if (IsNaN(X) || X < 0)
            X = 0;

        if (IsNaN(Y) || Y < 0)
            Y = 0;

        if (IsNaN(Z) || Z < 0)
            Z = 0;

        return new XYZ(X, Y, Z).ToLrgb(profile);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="Luv"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var xyz = new XYZ();
        xyz.FromLrgb(input, profile);

        static double Compute_up(XYZ i) => 4 * i.X / (i.X + 15 * i.Y + 3 * i.Z);
        static double Compute_vp(XYZ i) => 9 * i.Y / (i.X + 15 * i.Y + 3 * i.Z);

        var yr = input.Y / profile.White.Y;
        var up = Compute_up(xyz);
        var vp = Compute_vp(xyz);

        var upr = Compute_up(profile.White);
        var vpr = Compute_vp(profile.White);

        var L = yr > CIE.IEpsilon ? 116 * Pow(yr, 1 / 3d) - 16 : CIE.IKappa * yr;

        if (IsNaN(L) || L < 0)
            L = 0;

        var u = 13 * L * (up - upr);
        var v = 13 * L * (vp - vpr);

        if (IsNaN(u))
            u = 0;

        if (IsNaN(v))
            v = 0;

        Value = new(L, u, v);
    }
}