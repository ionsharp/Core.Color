using Imagin.Core.Numerics;
using System;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Cyan (C), Magenta (M), Yellow (Y), White (W)</b>
/// <para>A subtractive color model based on <see cref="RGBW"/> where the secondary colors are added with white.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="CMYW"/></para>
/// </summary>
[Component(100, "C", "Cyan"), Component(100, "M", "Magenta"), Component(100, "Y", "Yellow"), Component(100, "W", "White")]
[Serializable]
public sealed class CMYW : ColorModel4
{
    public CMYW() : base() { }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="CMYW"/></summary>
    public override void From(Lrgb input, WorkingProfile profile)
    {
        var r = input.X; var g = input.Y; var b = input.Z;
        var w = Min(r, Min(g, b));

        r -= w; r /= 1 - w;
        g -= w; g /= 1 - w;
        b -= w; b /= 1 - w;
        Value = new Vector4(1 - r, 1 - g, 1 - b, w) * 100;
    }

    /// <summary>(🗸) <see cref="CMYW"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
        double r = (100 - X) / 100;
        double g = (100 - Y) / 100;
        double b = (100 - Z) / 100;
        double w = W / 100;

        r *= (1 - w); r += w;
        g *= (1 - w); g += w;
        b *= (1 - w); b += w;
        return Colour.New<Lrgb>(r, g, b);
    }
}