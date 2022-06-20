﻿using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Hue (H), Whiteness (W), Blackness (B)</b>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Labk"/> > <see cref="Labkwb"/></para>
/// </summary>
/// <remarks>https://colour.readthedocs.io/en/develop/_modules/colour/models/oklab.html</remarks>
[Component(360, '°', "H", "Hue"), Component(100, '%', "W", "Whiteness"), Component(100, '%', "B", "Blackness")]
[Serializable]
public class Labkwb : ColorModel3<Labk>
{
    public Labkwb() : base() { }

    /// <summary>(🞩) <see cref="Labk"/> > <see cref="Labkwb"/></summary>
    public override void From(Labk input, WorkingProfile profile) { }

    /// <summary>(🞩) <see cref="Labkwb"/> > <see cref="Labk"/></summary>
    public override void To(out Labk result, WorkingProfile profile) => result = new();
}