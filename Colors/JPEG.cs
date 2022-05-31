using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Luminance (Y), Cb, Cr</b>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YPbPr"/> > <see cref="YCbCr"/> > <see cref="JPEG"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/jpeg.js</remarks>
[Component(0, 255, "Y", "Luminance")]
[Component(0, 255, "Cb")]
[Component(0, 255, "Cr")]
[Serializable]
public sealed class JPEG : YCbCrVector
{
    public JPEG(params double[] input) : base(input) { }

    public static implicit operator JPEG(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="JPEG"/> > <see cref="YCbCr"/></summary>
    public override YCbCr ToYCbCr(WorkingProfile profile)
    {
        double y = Value[0], cb = Value[1], cr = Value[2];
        return new(y + 1.402 * (cr - 128), y - 0.34414 * (cb - 128) - 0.71414 * (cr - 128), y + 1.772 * (cb - 128));
    }

    /// <summary><see cref="YCbCr"/> > <see cref="JPEG"/></summary>
    public override void FromYCbCr(YCbCr input, WorkingProfile profile)
    {
        double r = input[0], g = input[1], b = input[2];
        Value = new(0.299 * r + 0.587 * g + 0.114 * b, 128 - 0.168736 * r - 0.331264 * g + 0.5 * b, 128 + 0.5 * r - 0.418688 * g - 0.081312 * b);
    }
}