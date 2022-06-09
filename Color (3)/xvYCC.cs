using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>x (Y), v (Cb), Y (Cr)</b>
/// 
/// <para>A color space that can be used in the video electronics of television sets to support a gamut 1.8 times as large as that of the sRGB color space.</para>
/// 
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YPbPr"/> > <see cref="xvYCC"/></para>
/// 
/// <i>Author</i>
/// <list type="bullet">
/// <item>Sony</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/xvycc.js</remarks>
[Component(255, ' ', "x", "Y"), Component(255, ' ', "v", "Cb"), Component(255, ' ', "Y", "Cr")]
[Serializable]
public class xvYCC : YPbPr
{
    public xvYCC(params double[] input) : base(input) { }

    public static implicit operator xvYCC(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="xvYCC"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double y = this[0], cb = this[1], cr = this[2];
        return new YPbPr((y - 16) / 219, (cb - 128) / 224, (cr - 128) / 224).ToLrgb(profile);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="xvYCC"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var xyz = new YPbPr();
        xyz.FromLrgb(input, profile);

        double y = input[0], pb = input[1], pr = input[2];
        Value = new(16 + 219 * y, 128 + 224 * pb, 128 + 224 * pr);
    }
}