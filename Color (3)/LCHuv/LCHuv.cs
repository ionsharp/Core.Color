using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Luminance (L), Chroma (C), Hue (H)</b>
/// <para>A cylindrical form of 'Luv' that is designed to accord with the human perception of color.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Luv"/> > <see cref="LCHuv"/></para>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Category(Class.LCHuv), Serializable]
[Description("A cylindrical form of 'Luv' that is designed to accord with the human perception of color.")]
public class LCHuv : LCH<Luv>
{
    public LCHuv() : base() { }
}