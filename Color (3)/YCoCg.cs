using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>Luminance (Y), Co, Cg</b></para>
/// 
/// <para>The color space formed from a simple transformation of an associated RGB color space into a luma value (denoted as Y) and two chroma values called "chrominance green" (Cg) and "chrominance orange" (Co).</para>
/// 
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YCoCg"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>YCgCo</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/ycgco.js</remarks>
[Component(0,    1,   '%', "Y", "Luminance"), Component(-0.5, 0.5, ' ', "Co"), Component(-0.5, 0.5, ' ', "Cg")]
[Serializable]
public class YCoCg : ColorVector3
{
    public YCoCg(params double[] input) : base(input) { }

    public static implicit operator YCoCg(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="YCoCg"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double y = Value[0], cg = Value[1], co = Value[2];

        var c = y - cg;
        return new(c + co, y + cg, c - co);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="YCoCg"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        double r = input[0], g = input[1], b = input[2];
        Value = new(0.25 * r + 0.5 * g + 0.25 * b, -0.25 * r + 0.5 * g - 0.25 * b, 0.5 * r - 0.5 * b);
    }
}