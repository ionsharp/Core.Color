using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Luminance (Y), Cb, Cr</b>
/// <para>A color model based on 'YCbCr' (Rec. 601) where all three components have the full 8-bit range of [0, 255].</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YPbPr"/> > <see cref="YCbCr"/> > <see cref="JPEG"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/jpeg.js</remarks>
[Component(255, "Y", "Luminance"), Component(255, "Cb"), Component(255, "Cr")]
[Category(Class.YUV), Serializable]
[Description("A color model based on 'YCbCr' (Rec. 601) where all three components have the full 8-bit range of [0, 255].")]
public class JPEG : ColorModel3<YCbCr>
{
    public JPEG() : base() { }

    /// <summary>(🗸) <see cref="YCbCr"/> > <see cref="JPEG"/></summary>
    public override void From(YCbCr input, WorkingProfile profile)
    {
        double r = input.X, g = input.Y, b = input.Z;
        Value = new(0.299 * r + 0.587 * g + 0.114 * b, 128 - 0.168736 * r - 0.331264 * g + 0.5 * b, 128 + 0.5 * r - 0.418688 * g - 0.081312 * b);
    }

    /// <summary>(🗸) <see cref="JPEG"/> > <see cref="YCbCr"/></summary>
    public override void To(out YCbCr result, WorkingProfile profile)
    {
        double y = X, cb = Y, cr = Z;
        result = Colour.New<YCbCr>(y + 1.402 * (cr - 128), y - 0.34414 * (cb - 128) - 0.71414 * (cr - 128), y + 1.772 * (cb - 128));
    }
}