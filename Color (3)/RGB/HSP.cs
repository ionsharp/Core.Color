using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System;

using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Hue (H), Saturation (S), Percieved brightness (P)</b>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="HSP"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/hsp.js</remarks>
[Component(360, '°', "H", "Hue"), Component(100, '%', "S", "Saturation"), Component(255, ' ', "P", "Percieved brightness")]
[Serializable]
public class HSP : ColorModel3
{
    public HSP() : base() { }

    /// <summary>(🗸) <see cref="HSP"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
        const double Pr = 0.299;
        const double Pg = 0.587;
        const double Pb = 0.114;

        double h = Value[0] / 360.0, s = Value[1] / 100.0, p = Value[2];

        double r, g, b, part, minOverMax = 1.0 - s;

        if (minOverMax > 0.0)
        {
            // R > G > B
            if (h < 1.0 / 6.0)
            {
                h = 6.0 * (h - 0.0 / 6.0);
                part = 1.0 + h * (1.0 / minOverMax - 1.0);
                b = p / Sqrt(Pr / minOverMax / minOverMax + Pg * part * part + Pb);
                r = (b) / minOverMax;
                g = (b) + h * ((r) - (b));
            }
            // G > R > B
            else if (h < 2.0 / 6.0)
            {
                h = 6.0 * (-h + 2.0 / 6.0);
                part = 1.0 + h * (1.0 / minOverMax - 1.0);
                b = p / Sqrt(Pg / minOverMax / minOverMax + Pr * part * part + Pb);
                g = (b) / minOverMax;
                r = (b) + h * ((g) - (b));
            }
            // G > B > R
            else if (h < 3.0 / 6.0)
            {
                h = 6.0 * (h - 2.0 / 6.0);
                part = 1.0 + h * (1.0 / minOverMax - 1.0);
                r = p / Sqrt(Pg / minOverMax / minOverMax + Pb * part * part + Pr);
                g = (r) / minOverMax;
                b = (r) + h * ((g) - (r));
            }
            // B > G > R
            else if (h < 4.0 / 6.0)
            {
                h = 6.0 * (-h + 4.0 / 6.0);
                part = 1.0 + h * (1.0 / minOverMax - 1.0);
                r = p / Sqrt(Pb / minOverMax / minOverMax + Pg * part * part + Pr);
                b = (r) / minOverMax;
                g = (r) + h * ((b) - (r));
            }
            // B > R > G
            else if (h < 5.0 / 6.0)
            {
                h = 6.0 * (h - 4.0 / 6.0);
                part = 1.0 + h * (1.0 / minOverMax - 1.0);
                g = p / Sqrt(Pb / minOverMax / minOverMax + Pr * part * part + Pg);
                b = (g) / minOverMax;
                r = (g) + h * ((b) - (g));
            }
            // R > B > G
            else
            {
                h = 6.0 * (-h + 6.0 / 6.0);
                part = 1.0 + h * (1.0 / minOverMax - 1.0);
                g = p / Sqrt(Pr / minOverMax / minOverMax + Pb * part * part + Pg);
                r = (g) / minOverMax;
                b = (g) + h * ((r) - (g));
            }
        }
        else
        {
            // R > G > B
            if (h < 1.0 / 6.0)
            {
                h = 6.0 * (h - 0.0 / 6.0);
                r = Sqrt(p * p / (Pr + Pg * h * h));
                g = (r) * h;
                b = 0.0;
            }
            // G > R > B
            else if (h < 2.0 / 6.0)
            {
                h = 6.0 * (-h + 2.0 / 6.0);
                g = Sqrt(p * p / (Pg + Pr * h * h));
                r = (g) * h;
                b = 0.0;
            }
            // G > B > R
            else if (h < 3.0 / 6.0)
            {
                h = 6.0 * (h - 2.0 / 6.0);
                g = Sqrt(p * p / (Pg + Pb * h * h));
                b = (g) * h;
                r = 0.0;
            }
            // B > G > R
            else if (h < 4.0 / 6.0)
            {
                h = 6.0 * (-h + 4.0 / 6.0);
                b = Sqrt(p * p / (Pb + Pg * h * h));
                g = (b) * h;
                r = 0.0;
            }
            // B > R > G
            else if (h < 5.0 / 6.0)
            {
                h = 6.0 * (h - 4.0 / 6.0);
                b = Sqrt(p * p / (Pb + Pr * h * h));
                r = (b) * h;
                g = 0.0;
            }
            // R > B > G
            else
            {
                h = 6.0 * (-h + 6.0 / 6.0);
                r = Sqrt(p * p / (Pr + Pb * h * h));
                b = (r) * h;
                g = 0.0;
            }
        }
        return Colour.New<Lrgb>(r.Round() / 255.0, g.Round() / 255.0, b.Round() / 255.0);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="HSP"/></summary>
    public override void From(Lrgb input, WorkingProfile profile)
    {
        const double Pr = 0.299;
        const double Pg = 0.587;
        const double Pb = 0.114;

        var _input = input.Value * 255;
        double r = _input[0], g = _input[1], b = _input[2];
        double h = 0, s = 0, p = 0;

        p = Sqrt(r * r * Pr + g * g * Pg + b * b * Pb);

        if (r == g && r == b)
        {
            h = 0.0;
            s = 0.0;
        }
        else
        {
            //R is largest
            if (r >= g && r >= b)
            {
                if (b >= g)
                {
                    h = 6.0 / 6.0 - 1.0 / 6.0 * (b - g) / (r - g);
                    s = 1.0 - g / r;
                }
                else
                {
                    h = 0.0 / 6.0 + 1.0 / 6.0 * (g - b) / (r - b);
                    s = 1.0 - b / r;
                }
            }

            //G is largest
            if (g >= r && g >= b)
            {
                if (r >= b)
                {
                    h = 2.0 / 6.0 - 1.0 / 6.0 * (r - b) / (g - b);
                    s = 1 - b / g;
                }
                else
                {
                    h = 2.0 / 6.0 + 1.0 / 6.0 * (b - r) / (g - r);
                    s = 1.0 - r / g;
                }
            }

            //B is largest
            if (b >= r && b >= g)
            {
                if (g >= r)
                {
                    h = 4.0 / 6.0 - 1.0 / 6.0 * (g - r) / (b - r);
                    s = 1.0 - r / b;
                }
                else
                {
                    h = 4.0 / 6.0 + 1.0 / 6.0 * (r - g) / (b - g);
                    s = 1.0 - g / b;
                }
            }
        }
        Value = new((h * 360.0).Round(), s * 100.0, p.Round());
    }
}