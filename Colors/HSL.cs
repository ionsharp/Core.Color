using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System;

using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Hue (H), Saturation (S), Lightness (L)</b>
/// <para>≡ 73.105</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="HSL"/> (%)</para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/hsl.js</remarks>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "S", "Saturation")]
[Component(0, 100, '%', "L", "Lightness")]
[Serializable]
public sealed class HSL : ColorVector3
{
    public HSL(params double[] input) : base(input) { }

    public static implicit operator HSL(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HSL"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var max = GetMaximum<HSL>();

        double h = Value[0] / 60.0, s = Value[1] / max[1], l = Value[2] / max[2];

        double r = l, g = l, b = l;

        if (s > 0)
        {
            var chroma = (1.0 - (2.0 * l - 1.0).Absolute()) * s;
            var x = chroma * (1.0 - ((h % 2.0) - 1).Absolute());

            var result = new Vector(0.0, 0, 0);

            if (0 <= h && h <= 1)
            {
                result = new Vector(chroma, x, 0);
            }
            else if (1 <= h && h <= 2)
            {
                result = new Vector(x, chroma, 0);
            }
            else if (2 <= h && h <= 3)
            {
                result = new Vector(0.0, chroma, x);
            }
            else if (3 <= h && h <= 4)
            {
                result = new Vector(0.0, x, chroma);
            }
            else if (4 <= h && h <= 5)
            {
                result = new Vector(x, 0, chroma);
            }
            else if (5 <= h && h <= 6)
                result = new Vector(chroma, 0, x);

            var m = l - (0.5 * chroma);

            r = result[0] + m;
            g = result[1] + m;
            b = result[2] + m;
        }

        return new(r, g, b);
    }

    /// <summary><see cref="RGB"/> > <see cref="HSL"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var max = GetMaximum<HSL>();

        var m = Max(Max(input.Value[0], input.Value[1]), input.Value[2]);
        var n = Min(Min(input.Value[0], input.Value[1]), input.Value[2]);

        var chroma = m - n;

        double h = 0, s = 0, l = (m + n) / 2.0;

        if (chroma != 0)
        {
            s
                = l < 0.5
                ? chroma / (2.0 * l)
                : chroma / (2.0 - 2.0 * l);

            if (input.Value[0] == m)
            {
                h = (input.Value[1] - input.Value[2]) / chroma;
                h = input.Value[1] < input.Value[2]
                ? h + 6.0
                : h;
            }
            else if (input.Value[2] == m)
            {
                h = 4.0 + ((input.Value[0] - input.Value[1]) / chroma);
            }
            else if (input.Value[1] == m)
                h = 2.0 + ((input.Value[2] - input.Value[0]) / chroma);

            h *= 60;
        }

        Value = new(h, s * max[1], l * max[2]);
    }
}