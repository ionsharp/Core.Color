using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Hue (H), Saturation (S), Lightness (L)</b>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Labk"/> > <see cref="Labksl"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Okhsd</item>
/// <item>Okhsi</item>
/// </list>
/// </summary>
/// <remarks>https://colour.readthedocs.io/en/develop/_modules/colour/models/oklab.html</remarks>
[Component(360, '°', "H", "Hue"), Component(100, '%', "S", "Saturation"), Component(100, '%', "L", "Lightness")]
[Serializable]
public class Labksl : ColorModel3<Labk>
{
    public Labksl() : base() { }

    /// <summary>(🞩) <see cref="Labk"/> > <see cref="Labksl"/></summary>
    public override void From(Labk input, WorkingProfile profile) { }

    /// <summary>(🞩) <see cref="Labksl"/> > <see cref="Labk"/></summary>
    public override void To(out Labk result, WorkingProfile profile) => result = new();
}