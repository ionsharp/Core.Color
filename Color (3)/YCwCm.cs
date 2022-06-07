using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🞩) <b>Luminance (Y), Chromatic warmth (Cw), Chromatic mildness (Cm)</b></para>
/// 
/// <para>A color space similar to <see cref="YCbCr"/> and <see cref="YCoCg"/>, but the luminance is more accurate and the "warmth" is useful for aesthetic reasons.</para>
/// 
/// <para>≡ 0%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YCwCm"/></para>
/// </summary>
/// <remarks>https://github.com/tommyettinger/colorful-gdx</remarks>
[Component(0, 1, '%', "Y", "Luminance"), Component(0, 1, '%', "Cw", "Chromatic warmth"), Component(0, 1, '%', "Cm", "Chromatic mildness")]
[Serializable, Unfinished]
public class YCwCm : ColorVector3
{
    public YCwCm(params double[] input) : base(input) { }

    public static implicit operator YCwCm(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🞩) <see cref="YCwCm"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile) => new();

    /// <summary>(🞩) <see cref="Lrgb"/> > <see cref="YCwCm"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile) { }
}