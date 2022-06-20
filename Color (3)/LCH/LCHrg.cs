using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Luminance (L), Chroma (C), Hue (H)</b>
/// <para>A cylindrical form of <see cref="rgG"/> that is designed to accord with the human perception of color.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="rgG"/> > <see cref="LCHrg"/></para>
/// </summary>
[Serializable]
public class LCHrg : LCH<rgG>
{
    public LCHrg() : base() { }
}