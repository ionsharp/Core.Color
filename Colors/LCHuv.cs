using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Luminance (L), Chroma (Cu), Hue (Hv)</b>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Luv"/> > <see cref="LCHuv"/></para>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 100, '%', "L", "Luminance")]
[Component(0, 100, '%', "Cu", "Chroma")]
[Component(0, 360, '°', "Hv", "Hue")]
[Serializable]
public sealed class LCHuv : LuvVector
{
    public LCHuv(params double[] input) : base(input) { }

    public static implicit operator LCHuv(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="LCHuv"/> > <see cref="Luv"/></summary>
    public override Luv ToLUV(WorkingProfile profile)
        => new(new LCH(this).To());

    /// <summary><see cref="Luv"/> > <see cref="LCHuv"/></summary>
    public override void FromLUV(Luv input, WorkingProfile profile)
    {
        var lch = new LCH();
        lch.From(input);

        Value = lch;
    }
}