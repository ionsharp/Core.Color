using Imagin.Core.Numerics;
using System;

using static Imagin.Core.Numerics.M;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Hue (H), Chroma (C), Luminance (Y)</b>
/// <para>≡ 68.648%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="HCY"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/hcy.js</remarks>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "C", "Chroma")]
[Component(0, 255, ' ', "Y", "Luminance")]
[Serializable]
public sealed class HCY : ColorVector3
{
    public HCY(params double[] input) : base(input) { }

    public static implicit operator HCY(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HCY"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var h = (Value[0] < 0 ? (Value[0] % 360) + 360 : (Value[0] % 360)) * PI / 180;
        var s = Max(0, Min(Value[1], 100)) / 100;
        var i = Max(0, Min(Value[2], 255)) / 255;

        double r, g, b;
        if (h < (2 * PI3))
        {
            b = i * (1 - s);
            r = i * (1 + (s * Cos(h) / Cos(PI3 - h)));
            g = i * (1 + (s * (1 - Cos(h) / Cos(PI3 - h))));
        }
        else if (h < (4 * PI3))
        {
            h = h - 2 * PI3;
            r = i * (1 - s);
            g = i * (1 + (s * Cos(h) / Cos(PI3 - h)));
            b = i * (1 + (s * (1 - Cos(h) / Cos(PI3 - h))));
        }
        else
        {
            h = h - 4 * PI3;
            g = i * (1 - s);
            b = i * (1 + (s * Cos(h) / Cos(PI3 - h)));
            r = i * (1 + (s * (1 - Cos(h) / Cos(PI3 - h))));
        }
        return new(r, g, b);
    }

    /// <summary><see cref="RGB"/> > <see cref="HCY"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var sum = input[0] + input[1] + input[2];

        var r = input[0] / sum;
        var g = input[1] / sum;
        var b = input[2] / sum;

        var h = Acos((0.5 * ((r - g) + (r - b))) / Sqrt((r - g) * (r - g) + (r - b) * (g - b)));

        if (b > g)
            h = 2 * PI - h;

        var s = 1 - 3 * Min(r, Min(g, b));

        var i = sum / 3;
        Value = new(h * 180 / PI, s * 100, i * 255);
    }
}