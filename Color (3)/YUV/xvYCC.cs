using System;
using System.Diagnostics.CodeAnalysis;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>x (Y), v (Cb), Y (Cr)</b>
/// <para>A color space that can be used in television sets to support a gamut 1.8 times as large as that of the sRGB color space.</para>
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
[SuppressMessage("Style", "IDE1006:Naming Styles")]
public class xvYCC : ColorModel3<YPbPr>
{
    public xvYCC() : base() { }

    /// <summary>(🗸) <see cref="YPbPr"/> > <see cref="xvYCC"/></summary>
    public override void From(YPbPr input, WorkingProfile profile)
    {
        double y = input.X, pb = input.Y, pr = input.Z;
        Value = new(16 + 219 * y, 128 + 224 * pb, 128 + 224 * pr);
    }

    /// <summary>(🗸) <see cref="xvYCC"/> > <see cref="YPbPr"/></summary>
    public override void To(out YPbPr result, WorkingProfile profile)
    {
        double y = X, cb = Y, cr = Z;
        result = Colour.New<YPbPr>((y - 16) / 219, (cb - 128) / 224, (cr - 128) / 224);
    }
}