using Imagin.Core.Numerics;
using System;

using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>Hue (H), Saturation (S), Brightness (B)</b></para>
/// 
/// <para>A color space that defines color as having a hue (color), saturation (colorfulness), and brightness (color intensity).</para>
/// 
/// <para>≡ 76.778%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="HSB"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>HSV</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/hsb.js</remarks>
[Component(360, '°', "H", "Hue"), Component(100, '%', "S", "Saturation"), Component(100, '%', "B", "Brightness")]
[Serializable]
public class HSB : ColorVector3, IHs
{
    public HSB(params double[] input) : base(input) { }

    public static implicit operator HSB(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="HSB"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var hsb = new Vector(Value[0] / 360, Value[1] / 100, Value[2] / 100);
        if (hsb[1] == 0)
            return new(hsb[2], hsb[2], hsb[2]);

        var h = hsb[0] * 6;
        if (h == 6)
            h = 0;

        var i = Floor(h);
        var x = hsb[2] * (1 - hsb[1]);
        var y = hsb[2] * (1 - hsb[1] * (h - i));
        var z = hsb[2] * (1 - hsb[1] * (1 - (h - i)));

        double r, g, b;

        if (i == 0)
        { r = hsb[2]; g = z; b = x; }
        else if (i == 1)
        { r = y; g = hsb[2]; b = x; }
        else if (i == 2)
        { r = x; g = hsb[2]; b = z; }
        else if (i == 3)
        { r = x; g = y; b = hsb[2]; }
        else if (i == 4)
        { r = z; g = x; b = hsb[2]; }
        else
        { r = hsb[2]; g = x; b = y; }

        return new(r, g, b);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="HSB"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var max = GetMaximum<HSB>();

        var r = input[0]; var g = input[1]; var b = input[2];

        var m = Max(r, Max(g, b));
        var n = Min(r, Min(g, b));

        var chroma = m - n;

        double h = 0, s, v = m;
        if (chroma == 0)
        {
            h = 0; s = 0;
        }
        else
        {
            s = chroma / m;

            var dR = (((m - r) / 6) + (chroma / 2)) / chroma;
            var dG = (((m - g) / 6) + (chroma / 2)) / chroma;
            var dB = (((m - b) / 6) + (chroma / 2)) / chroma;

            if (r == m)
                h = dB - dG;

            else if (g == m)
                h = (1 / 3) + dR - dB;

            else if (b == m)
                h = (2 / 3) + dG - dR;

            if (h < 0)
                h += 1;

            if (h > 1)
                h -= 1;
        }

        Value = new Vector(h, s, v) * max;
    }
}