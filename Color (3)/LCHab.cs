using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Luminance (L), Chroma (C), Hue (H)</b>
/// <para>A cylindrical form of <see cref="Lab"/> that is designed to accord with the human perception of color.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Lab"/> > <see cref="LCHab"/></para>
/// 
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Serializable]
public class LCHab : LCH<Lab>
{
    public LCHab() : base() { }
}