using System;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <para><b>Luminance (Y), U, V</b></para>
/// <para>A model that defines color as having luminance (Y), chroma (U), and chroma (V).</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YUV"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>EBU</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/yuv.js</remarks>
[Component(1, '%', "Y", "Luminance"), Component(-0.5, 0.5, ' ', "U"), Component(-0.5, 0.5, ' ', "V")]
[Category(Class.YUV), Serializable]
[Description("A model that defines color as having luminance (Y), chroma (U), and chroma (V).")]
public class YUV : ColorModel3
{
    public YUV() : base() { }

    /// <summary>(🗸) <see cref="YUV"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
        double
            y = X,
            u = Y,
            v = Z,
            r, g, b;

        r = (y * 1)
            + (u * 0)
            + (v * 1.13983);
        g = (y * 1)
            + (u * -0.39465)
            + (v * -0.58060);
        b = (y * 1)
            + (u * 2.02311)
            + (v * 0);

        r = Min(Max(0, r), 1);
        g = Min(Max(0, g), 1);
        b = Min(Max(0, b), 1);

        return Colour.New<Lrgb>(r, g, b);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="YUV"/></summary>
    public override void From(Lrgb input, WorkingProfile profile)
    {
        double
            r = input.X,
            g = input.Y,
            b = input.Z;

        var y = (r * 0.299)
            + (g * 0.587)
            + (b * 0.114);
        var u = (r * -0.14713)
            + (g * -0.28886)
            + (b * 0.436);
        var v = (r * 0.615)
            + (g * -0.51499)
            + (b * -0.10001);

        Value = new(y, u, v);
    }
}