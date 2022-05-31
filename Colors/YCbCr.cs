using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>Luminance (Y), Cb, Cr</b></para>
/// A digital form of <see cref="YCbCr"/> (ITU-R BT.601/709).
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YPbPr"/> > <see cref="YCbCr"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/ycbcr.js</remarks>
[Component(16, 235, ' ', "Y", "Luminance")]
[Component(16, 240, ' ', "Cb")]
[Component(16, 240, ' ', "Cr")]
[Serializable]
public sealed class YCbCr : YPbPrVector
{
    public YCbCr(params double[] input) : base(input) { }

    public static implicit operator YCbCr(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="YCbCr"/> > <see cref="YPbPr"/></summary>
    public override YPbPr ToYPbPr(WorkingProfile profile)
    {
        double y = Value[0], cb = Value[1], cr = Value[2];
        return new((y - 16) / 219, (cb - 128) / 224, (cr - 128) / 224);
    }

    /// <summary><see cref="YPbPr"/> > <see cref="YCbCr"/></summary>
    public override void FromYPbPr(YPbPr input, WorkingProfile profile)
    {
        double y = input[0], pb = input[1], pr = input[2];
        Value = new(16 + 219 * y, 128 + 224 * pb, 128 + 224 * pr);
    }
}