using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🞩) <b>Hue (H), Saturation (S), Brightness (B)</b>
/// <para>≡ 0%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Labk"/> > <see cref="HSBk"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Okhsb</item>
/// <item>Okhsv</item>
/// </list>
/// </summary>
/// <remarks>https://colour.readthedocs.io/en/develop/_modules/colour/models/oklab.html</remarks>
[Component(0, 360, '°', "H", "Hue"), Component(0, 100, '%', "S", "Saturation"), Component(0, 100, '%', "B", "Brightness")]
[Serializable, Unfinished]
public class HSBk : Labk, IHs
{
    public HSBk(params double[] input) : base(input) { }

    public static implicit operator HSBk(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🞩) <see cref="HSBk"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var result = Vector3.Zero;
        return new Labk(result).ToLrgb(profile);
    }

    /// <summary>(🞩) <see cref="Lrgb"/> > <see cref="HSBk"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var result = new Labk();
        result.FromLrgb(input, profile);
    }
}