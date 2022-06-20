using System;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Hue (H), Chroma (C), Luminance (Y)</b>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="HCY"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/hcy.js</remarks>
[Component(360, '°', "H", "Hue"), Component(100, '%', "C", "Chroma"), Component(255, ' ', "Y", "Luminance")]
[Serializable]
public class HCY : ColorModel3
{
    public HCY() : base() { }

    /// <summary>(🗸) <see cref="HCY"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
        var h = (Value[0] < 0 ? (Value[0] % 360) + 360 : (Value[0] % 360)) * PI / 180;
        var c = Value[1] / 100;
        var y = Value[2] / 255;

        var pi3 = PI / 3; //Do not confuse with PI * 3!

        double r, g, b;
        if (h < (2 * pi3))
        {
            b = y * (1 - c);
            r = y * (1 + (c * Cos(h) / Cos(pi3 - h)));
            g = y * (1 + (c * (1 - Cos(h) / Cos(pi3 - h))));
        }
        else if (h < (4 * pi3))
        {
            h = h - 2 * pi3;
            r = y * (1 - c);
            g = y * (1 + (c * Cos(h) / Cos(pi3 - h)));
            b = y * (1 + (c * (1 - Cos(h) / Cos(pi3 - h))));
        }
        else
        {
            h = h - 4 * pi3;
            g = y * (1 - c);
            b = y * (1 + (c * Cos(h) / Cos(pi3 - h)));
            r = y * (1 + (c * (1 - Cos(h) / Cos(pi3 - h))));
        }
        return Colour.New<Lrgb>(r, g, b);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="HCY"/></summary>
    public override void From(Lrgb input, WorkingProfile profile)
    {
        var sum = input[0] + input[1] + input[2];

        var r = input[0] / sum;
        var g = input[1] / sum;
        var b = input[2] / sum;

        var h = Acos((0.5 * ((r - g) + (r - b))) / Sqrt((r - g) * (r - g) + (r - b) * (g - b)));

        if (b > g)
            h = 2 * PI - h;

        var c = 1 - 3 * Min(r, Min(g, b));

        var y = sum / 3;
        Value = new(h * 180 / PI, c * 100, y * 255);
    }
}