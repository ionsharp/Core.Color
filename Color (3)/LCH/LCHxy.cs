using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Luminance (L), Chroma (C), Hue (H)</b>
/// <para>A cylindrical form of <see cref="xyY"/> that is designed to accord with the human perception of color.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="xyY"/> > <see cref="LCHxy"/></para>
/// </summary>
[Serializable]
public class LCHxy : LCH<xyY>
{
    public LCHxy() : base() { }
}