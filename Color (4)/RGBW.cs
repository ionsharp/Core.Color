using Imagin.Core.Numerics;
using System;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Red (R), Green (G), Blue (B), White (W)</b>
/// <para>An additive color model based on <see cref="RGB"/> where the primary colors are added with white.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="RGBW"/></para>
/// </summary>
/// <remarks>
/// <para>https://andi-siess.de/rgb-to-color-temperature/</para>
/// <para>https://stackoverflow.com/questions/21117842/converting-an-rgbw-color-to-a-standard-rgb-hsb-representation</para>
/// <para>https://stackoverflow.com/questions/40312216/converting-rgb-to-rgbw</para>
/// </remarks>
[Component(255, "R", "Red"), Component(255, "G", "Green"), Component(255, "B", "Blue"), Component(255, "W", "White")]
[Serializable]
public sealed class RGBW : ColorModel4
{
    public RGBW() : base() { }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="RGBW"/></summary>
    public override void From(Lrgb input, WorkingProfile profile)
    {
        var r = input.X; var g = input.Y; var b = input.Z;
        var w = Min(r, Min(g, b));

        r -= w; r /= 1 - w;
        g -= w; g /= 1 - w;
        b -= w; b /= 1 - w;
        Value = new Vector4(r, g, b, w) * 255;
    }

    /// <summary>(🗸) <see cref="RGBW"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
        double r = X / 255;
        double g = Y / 255;
        double b = Z / 255;
        double w = W / 255;

        r *= (1 - w); r += w;
        g *= (1 - w); g += w;
        b *= (1 - w); b += w;
        return Colour.New<Lrgb>(r, g, b);
    }
}