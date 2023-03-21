using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Hue (H), Saturation (S), Brightness (B)</b>
/// <para>A model derived from 'Labk' that defines color as having hue (H), saturation (S), and brightness (B).</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Labk"/> > <see cref="Labksb"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Okhsb</item>
/// </list>
/// </summary>
/// <remarks>https://colour.readthedocs.io/en/develop/_modules/colour/models/oklab.html</remarks>
[Component(360, '°', "H", "Hue"), Component(100, '%', "S", "Saturation"), Component(100, '%', "B", "Brightness")]
[Category(Class.Labk), Hide, Serializable]
[Description("A model derived from 'Labk' that defines color as having hue (H), saturation (S), and brightness (B).")]
public class Labksb : ColorModel3<Labk>
{
    public Labksb() : base() { }

    /// <summary>(🞩) <see cref="Labk"/> > <see cref="Labksb"/></summary>
    public override void From(Labk input, WorkingProfile profile) { }

    /// <summary>(🞩) <see cref="Labksb"/> > <see cref="Labk"/></summary>
    public override void To(out Labk result, WorkingProfile profile) => result = new();
}