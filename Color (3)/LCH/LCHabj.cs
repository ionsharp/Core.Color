using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para><b>Lightness (L), Chroma (C), Hue (H)</b></para>
/// <para>A cylindrical form of <see cref="Labj"/> that is designed to accord with the human perception of color.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Labj"/> > <see cref="LCHabj"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>JzCzhz</item>
/// </list>
/// </summary>
/// <remarks>https://observablehq.com/@jrus/jzazbz</remarks>
[Serializable]
public class LCHabj : LCH<Labj>
{
    public LCHabj() : base() { }
}