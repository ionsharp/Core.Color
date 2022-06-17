using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Red (R), Green (G), Blue (B), Black (K)</b>
/// <para>An additive color model based on <see cref="CMYK"/> where the primary colors are added with black.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="RGBK"/></para>
/// </summary>
[Component(255, '%', "R", "Red"), Component(255, '%', "G", "Green"), Component(255, '%', "B", "Blue"), Component(255, '%', "K", "Black")]
[Serializable]
public sealed class RGBK : ColorModel4
{
    public RGBK() : base() { }

    /// <summary>(🞩) <see cref="Lrgb"/> > <see cref="RGBK"/></summary>
    public override void From(Lrgb input, WorkingProfile profile) { }

    /// <summary>(🗸) <see cref="RGBK"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
        var result = Value / 255;
        var r = result[0] * (1.0 - result[3]);
        var g = result[1] * (1.0 - result[3]);
        var b = result[2] * (1.0 - result[3]);
        return Colour.New<Lrgb>(r, g, b);
    }
}