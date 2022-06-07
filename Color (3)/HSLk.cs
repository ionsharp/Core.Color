using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🞩) <b>Hue (H), Saturation (S), Lightness (L)</b>
/// <para>≡ 0%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Labk"/> > <see cref="HSLk"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Okhsd</item>
/// <item>Okhsi</item>
/// <item>Okhsl</item>
/// </list>
/// </summary>
/// <remarks>https://colour.readthedocs.io/en/develop/_modules/colour/models/oklab.html</remarks>
[Component(0, 360, '°', "H", "Hue"), Component(0, 100, '%', "S", "Saturation"), Component(0, 100, '%', "L", "Lightness")]
[Serializable, Unfinished]
public class HSLk : Labk, IHs
{
    public HSLk(params double[] input) : base(input) { }

    public static implicit operator HSLk(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🞩) <see cref="HSLk"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var result = Vector3.Zero;
        return new Labk(result).ToLrgb(profile);
    }

    /// <summary>(🞩) <see cref="Lrgb"/> > <see cref="HSLk"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var result = new Labk();
        result.FromLrgb(input, profile);
    }
}