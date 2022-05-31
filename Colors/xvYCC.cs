using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>x (Y), v (Cb), Y (Cr)</b>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YPbPr"/> > <see cref="xvYCC"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/xvycc.js</remarks>
[Component(0, 255, ' ', "x", "Y")]
[Component(0, 255, ' ', "v", "Cb")]
[Component(0, 255, ' ', "Y", "Cr")]
[Serializable]
public sealed class xvYCC : YPbPrVector
{
    public xvYCC(params double[] input) : base(input) { }

    public static implicit operator xvYCC(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="xvYCC"/> > <see cref="YPbPr"/></summary>
    public override YPbPr ToYPbPr(WorkingProfile profile)
    {
        double y = Value[0], cb = Value[1], cr = Value[2];
        return new((y - 16) / 219, (cb - 128) / 224, (cr - 128) / 224);
    }

    /// <summary><see cref="YPbPr"/> > <see cref="xvYCC"/></summary>
    public override void FromYPbPr(YPbPr input, WorkingProfile profile)
    {
        double y = input[0], pb = input[1], pr = input[2];
        Value = new(16 + 219 * y, 128 + 224 * pb, 128 + 224 * pr);
    }
}