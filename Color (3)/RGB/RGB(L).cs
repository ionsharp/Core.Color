using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para><b>Red (R), Green (G), Blue (B)</b></para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>LRGB</item>
/// <item>Linear RGB</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(1, '%', "R", "Red"), Component(1, '%', "G", "Green"), Component(1, '%', "B", "Blue")]
[Hidden, Serializable]
public class Lrgb : ColorModel3
{
    public Lrgb() : base() { }

    public override void From(Lrgb input, WorkingProfile profile) => Value = input.XYZ;

    public override Lrgb To(WorkingProfile profile) => Colour.New<Lrgb>(XYZ);
}