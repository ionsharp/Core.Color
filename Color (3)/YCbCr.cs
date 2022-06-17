using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para><b>Luminance (Y), Cb, Cr</b></para>
/// <para>A color space used as a part of the color image pipeline in digital systems where Y′ is a luma component, and CB and CR are the blue-difference and red-difference chroma components, respectively.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YPbPr"/> > <see cref="YCbCr"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Y′CbCr</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/ycbcr.js</remarks>
[Component(16, 235, ' ', "Y", "Luminance"), Component(16, 240, ' ', "Cb"), Component(16, 240, ' ', "Cr")]
[Serializable]
public class YCbCr : ColorModel3<YPbPr>
{
    public YCbCr() : base() { }

    /// <summary>(🗸) <see cref="YPbPr"/> > <see cref="YCbCr"/></summary>
    public override void From(YPbPr input, WorkingProfile profile)
    {
        double y = input[0], pb = input[1], pr = input[2];
        Value = new(16 + 219 * y, 128 + 224 * pb, 128 + 224 * pr);
    }

    /// <summary>(🗸) <see cref="YCbCr"/> > <see cref="YPbPr"/></summary>
    public override void To(out YPbPr result, WorkingProfile profile)
    {
        double y = this[0], cb = this[1], cr = this[2];
        result = Colour.New<YPbPr>((y - 16) / 219, (cb - 128) / 224, (cr - 128) / 224);
    }
}