using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Hue (H), Saturation (S), Lightness (L)</b>
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
[Component(360, '°', "H", "Hue"), Component(100, '%', "S", "Saturation"), Component(100, '%', "L", "Lightness")]
[Serializable]
public class HSLk : ColorModel3<Labk>
{
    public HSLk() : base() { }

    /// <summary>(🞩) <see cref="Labk"/> > <see cref="HSLk"/></summary>
    public override void From(Labk input, WorkingProfile profile) { }

    /// <summary>(🞩) <see cref="HSLk"/> > <see cref="Labk"/></summary>
    public override void To(out Labk result, WorkingProfile profile) => result = new();
}