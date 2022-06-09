using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Luminance (Y), Cb, Cr</b>
/// 
/// <para>A color space based on <see cref="YCbCr"/> (Rec. 601) where all three components have the full 8-bit range of [0, 255].</para>
/// 
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YPbPr"/> > <see cref="YCbCr"/> > <see cref="JPEG"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/jpeg.js</remarks>
[Component(255, "Y", "Luminance"), Component(255, "Cb"), Component(255, "Cr")]
[Serializable]
public class JPEG : YCbCr
{
    public JPEG(params double[] input) : base(input) { }

    public static implicit operator JPEG(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="JPEG"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double y = this[0], cb = this[1], cr = this[2];
        return new YCbCr(y + 1.402 * (cr - 128), y - 0.34414 * (cb - 128) - 0.71414 * (cr - 128), y + 1.772 * (cb - 128)).ToLrgb(profile);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="JPEG"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var result = new YCbCr();
        result.FromLrgb(input, profile);

        double r = result[0], g = result[1], b = result[2];
        Value = new(0.299 * r + 0.587 * g + 0.114 * b, 128 - 0.168736 * r - 0.331264 * g + 0.5 * b, 128 + 0.5 * r - 0.418688 * g - 0.081312 * b);
    }
}