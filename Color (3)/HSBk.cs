using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Hue (H), Saturation (S), Brightness (B)</b>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Labk"/> > <see cref="HSBk"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Okhsb</item>
/// <item>Okhsv</item>
/// </list>
/// </summary>
/// <remarks>https://colour.readthedocs.io/en/develop/_modules/colour/models/oklab.html</remarks>
[Component(360, '°', "H", "Hue"), Component(100, '%', "S", "Saturation"), Component(100, '%', "B", "Brightness")]
[Serializable]
public class HSBk : ColorModel3<Labk>
{
    public HSBk() : base() { }

    /// <summary>(🞩) <see cref="Labk"/> > <see cref="HSBk"/></summary>
    public override void From(Labk input, WorkingProfile profile) { }

    /// <summary>(🞩) <see cref="HSBk"/> > <see cref="Labk"/></summary>
    public override void To(out Labk result, WorkingProfile profile) => result = new();
}