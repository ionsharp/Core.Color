using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Hue (H), Whiteness (W), Blackness (B)</b>
/// 
/// <para>≡ 73.105%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="HSB"/> > <see cref="HWBsb"/></para>
/// </summary>
/// <remarks>https://drafts.csswg.org/css-color/#the-hwb-notation</remarks>
[Component(360, '°', "H", "Hue"), Component(100, '%', "W", "Whiteness"), Component(100, '%', "B", "Blackness")]
[Serializable]
public class HWBsb : HSB, IHWb
{
    public HWBsb(params double[] input) : base(input) { }

    public static implicit operator HWBsb(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="HWBsb"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var result = this.FromHWb();
        return new HSB(result).ToLrgb(profile);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="HWBsb"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var result = new HSB();
        result.FromLrgb(input, profile);

        Value = result.ToHWb();
    }
}