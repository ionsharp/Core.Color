using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>Luminance (Y), Cb, Cr</b></para>
/// 
/// <para>A color space used as a part of the color image pipeline in digital systems where Y′ is a luma component, and CB and CR are the blue-difference and red-difference chroma components, respectively.</para>
/// 
/// <para>≡ 100%</para>
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
public class YCbCr : YPbPr
{
    public YCbCr(params double[] input) : base(input) { }

    public static implicit operator YCbCr(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="YCbCr"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double y = this[0], cb = this[1], cr = this[2];
        return new YPbPr((y - 16) / 219, (cb - 128) / 224, (cr - 128) / 224).ToLrgb(profile);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="YCbCr"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var result = new YPbPr();
        result.FromLrgb(input, profile);

        double y = result[0], pb = result[1], pr = result[2];
        Value = new(16 + 219 * y, 128 + 224 * pb, 128 + 224 * pr);
    }
}