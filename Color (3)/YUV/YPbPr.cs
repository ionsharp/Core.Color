using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Y, Pb, Pr</b>
/// <para>A gamma-corrected color model designed for use in analog systems that is numerically equivalent to 'YCbCr'.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YPbPr"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Y′PbPr</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/ypbpr.js</remarks>
[Component(1, '%', "Y", "Luminance"), Component(-0.5, 0.5, ' ', "Pb", ""), Component(-0.5, 0.5, ' ', "Pr", "")]
[Category(Class.YUV), Serializable]
[Description("A gamma-corrected color model designed for use in analog systems that is numerically equivalent to 'YCbCr'.")]
public class YPbPr : ColorModel3
{
    public YPbPr() : base() { }

    /// <summary>(🗸) <see cref="YPbPr"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
        double y = X, pb = Y, pr = Z;

        //ITU-R BT.709
        double kb = 0.0722;
        double kr = 0.2126;

        var r = y + 2 * pr * (1 - kr);
        var b = y + 2 * pb * (1 - kb);
        var g = (y - kr * r - kb * b) / (1 - kr - kb);

        return Colour.New<Lrgb>(r, g, b);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="YPbPr"/></summary>
    public override void From(Lrgb input, WorkingProfile profile)
    {
        double r = input.X, g = input.Y, b = input.Z;

        //ITU-R BT.709
        double kb = 0.0722;
        double kr = 0.2126;

        var y = kr * r + (1 - kr - kb) * g + kb * b;
        var pb = 0.5 * (b - y) / (1 - kb);
        var pr = 0.5 * (r - y) / (1 - kr);

        Value = new(y, pb, pr);
    }
}