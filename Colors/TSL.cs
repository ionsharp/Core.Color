using Imagin.Core.Numerics;
using System;

using static Imagin.Core.Numerics.M;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// (🞩) <b>Tint (T), Saturation (S), Lightness (L)</b>
/// <para>≡ 36.884%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="TSL"/></para>
/// 
/// <para>🞩 <i>Conversion accuracy is very low.</i></para>
/// <para>🞩 <i>The color space repeats unless <see cref="T"/> / 4. Is this expected?</i></para>
/// </summary>
/// <remarks>https://en.wikipedia.org/wiki/TSL_color_space#Conversion_between_RGB_and_TSL</remarks>
[Component(0, 1, '%', "T", "Tint")]
[Component(0, 1, '%', "S", "Saturation")]
[Component(0, 1, '%', "L", "Lightness")]
[Serializable, Unfinished]
public sealed class TSL : ColorVector3
{
    public double T => X;

    public double S => Y;

    public double L => Z;

    public TSL(params double[] input) : base(input) { }

    public static implicit operator TSL(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="TSL"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double T = this[0] / 4, S = this[1], L = this[2];

        var y = 2 * PI * T;
        var x = -(Cos(y) / Sin(y));

        var bP = 5 / (9 * (Pow(x, 2) + 1));
        var gP = T > 0.5 ? -Sqrt(bP) * S : T < 0.5 ? Sqrt(bP) * S : 0;
        var rP = T == 0 ? Sqrt(5) / 3 * S : x * gP;

        var r = rP + (1 / 3);
        var g = gP + (1 / 3);

        var k = L / (r * 0.185 + g * 0.473 + 0.114);

        var R = k * r; var G = k * g; var B = k * (1 - r - g);
        return new(R, G, B);
    }

    /// <summary><see cref="RGB"/> > <see cref="TSL"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var R = input[0]; var G = input[1]; var B = input[2];

        var sum = R + G + B;
        var r = R / sum;
        var g = G / sum;

        double rP = r - (1 / 3), gP = g - (1 / 3), bP = 1 / (2 * PI);

        var T = gP > 0
            ? bP * Atan((rP / gP) + (1 / 4))
            : gP < 0
            ? bP * Atan((rP / gP) + (3 / 4))
            : 0;

        var S = Sqrt(9 / 5 * (Pow2(rP) + Pow2(gP)));
        var L = (R * 0.299) + (G * 0.587) + (B * 0.114);
        Value = new(T, S, L);
    }
}