using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Y, Pb, Pr</b>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YPbPr"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/ypbpr.js</remarks>
[Component(0, 1, '%', "Y", "Luminance")]
[Component(-0.5, 0.5, ' ', "Pb", "")]
[Component(-0.5, 0.5, ' ', "Pr", "")]
[Serializable]
public sealed class YPbPr : ColorVector3
{
    public YPbPr(params double[] input) : base(input) { }

    public static implicit operator YPbPr(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="YPbPr"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double y = Value[0], pb = Value[1], pr = Value[2];

        //ITU-R BT.709
        double kb = 0.0722;
        double kr = 0.2126;

        var r = y + 2 * pr * (1 - kr);
        var b = y + 2 * pb * (1 - kb);
        var g = (y - kr * r - kb * b) / (1 - kr - kb);

        return new(r, g, b);
    }

    /// <summary><see cref="RGB"/> > <see cref="YPbPr"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        double r = input[0], g = input[1], b = input[2];

        //ITU-R BT.709
        double kb = 0.0722;
        double kr = 0.2126;

        var y = kr * r + (1 - kr - kb) * g + kb * b;
        var pb = 0.5 * (b - y) / (1 - kb);
        var pr = 0.5 * (r - y) / (1 - kr);

        Value = new(y, pb, pr);
    }
}