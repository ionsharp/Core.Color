using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Luminance (L), Chroma (C), Hue (H)</b>
/// <para>A cylindrical form of <see cref="Labh"/> that is designed to accord with the human perception of color.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Labh"/> > <see cref="LCHabh"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Hunter LCHab</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Serializable]
public class LCHabh : LCH<Labh>
{
    public LCHabh() : base() { }
}