using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Hue (H), Saturation (P), Lightness (L)</b>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Luv"/> > <see cref="LCHuv"/> > <see cref="HPLuv"/></para>
/// </summary>
/// <remarks>https://github.com/hsluv/hsluv-csharp</remarks>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "P", "Saturation")]
[Component(0, 100, '%', "L", "Lightness")]
[Serializable]
public sealed class HPLuv : LCHuvVector
{
    public HPLuv(params double[] input) : base(input) { }

    public static implicit operator HPLuv(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HPLuv"/> > <see cref="LCHuv"/></summary>
    public override LCHuv ToLCHuv(WorkingProfile profile)
    {
        double H = Value[0], S = Value[1], L = Value[2];

        if (L > 99.9999999)
            return new(100, 0, H);

        if (L < 0.00000001)
            return new(0, 0, H);

        double max = GetChroma(L);
        double C = max / 100 * S;

        return new(L, C, H);
    }

    /// <summary><see cref="LCHuv"/> > <see cref="HPLuv"/></summary>
    public override void FromLCHuv(LCHuv input, WorkingProfile profile)
    {
        double L = input[0], C = input[1], H = input[2];

        if (L > 99.9999999)
        {
            Value = new(H, 0, 100);
            return;
        }

        if (L < 0.00000001)
        {
            Value = new(H, 0, 0);
            return;
        }

        double max = GetChroma(L);
        double S = C / max * 100;

        Value = new(H, S, L);
    }
}