using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Hue (H), Whiteness (W), Blackness (B)</b>
/// <para>≡ 0%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Labk"/> > <see cref="HSBk"/> > <see cref="HWBsbk"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Okhwb</item>
/// </list>
/// </summary>
/// <remarks>https://colour.readthedocs.io/en/develop/_modules/colour/models/oklab.html</remarks>
[Component(0, 360, '°', "H", "Hue"), Component(0, 100, '%', "W", "Whiteness"), Component(0, 100, '%', "B", "Blackness")]
[Serializable]
public class HWBsbk : HSBk, IHWb
{
    public HWBsbk(params double[] input) : base(input) { }

    public static implicit operator HWBsbk(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="HWBsbk"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var result = this.FromHWb();
        return new HSBk(result).ToLrgb(profile);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="HWBsbk"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var result = new HSBk();
        result.FromLrgb(input, profile);

        Value = result.ToHWb();
    }
}