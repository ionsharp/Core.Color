using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System;
using System.Linq;

using static Imagin.Core.Numerics.M;
using static System.Double;
using static System.Math;

namespace Imagin.Core.Colors;

/// <notes>(🗸) Done (🞩) Not done [🞶%] Conversion accuracy</notes>

#region ColorVector3

public interface IColorVector3 { }

/// <summary>A <see cref="ColorVector"/> with three (3) components.</summary>
/// <inheritdoc/>
[Serializable]
public abstract class ColorVector3 : ColorVector, IColorVector3
{
    /// <summary>
    /// The first component.
    /// </summary>
    public double X => Value[0];

    /// <summary>
    /// The second component.
    /// </summary>
    public double Y => Value[1];

    /// <summary>
    /// The third component.
    /// </summary>
    public double Z => Value[2];

    public ColorVector3(params double[] input) : base() => Value = new(input);

    public static implicit operator Vector3(ColorVector3 input) => new(input.X, input.Y, input.Z);
}

#endregion

#region (🗸) [100.00%] (RGB)

/// <summary>
/// <para><b>Red (R), Green (G), Blue (B)</b></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>CIERGB</item>
/// </list>
/// 
/// <para><i>Author</i>: <see cref="CIE"/> (1931)</para>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 1, "R", "Red"), Component(0, 1, "G", "Green"), Component(0, 1, "B", "Blue")]
[Serializable]
public sealed class RGB : ColorVector3
{
    public RGB(params double[] input) : base(input) { }

    public static implicit operator RGB(Vector3 input) => new(input.X, input.Y, input.Z);

    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var oldValue = Value;
        var newValue = oldValue.Transform(i => profile.Companding.CompandInverse(i));
        return new(newValue);
    }

    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var oldValue = input.Value;
        var newValue = oldValue.Transform(i => profile.Companding.Compand(i));
        Value = new(newValue);
    }
}

#endregion

#region (🗸) [100.00%] (Lrgb)

/// <summary>
/// <para><b>Red (R), Green (G), Blue (B)</b></para>
/// <i>Alias</i>
/// <list type="bullet">
/// <item>LRGB</item>
/// <item>Linear RGB</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Hidden, Serializable]
public sealed class Lrgb : ColorVector3
{
    public Lrgb(params double[] input) : base(input) { }

    public static implicit operator Lrgb(Vector3 input) => new(input.X, input.Y, input.Z);

    public override void FromLrgb(Lrgb input, WorkingProfile profile) => Value = input.Value;

    public override Lrgb ToLrgb(WorkingProfile profile) => new(Value);
}

#endregion

#region (🗸) [100.00%] (CMY)

/// <summary>
/// <b>Cyan (C), Magenta (M), Yellow (Y)</b>
/// <para><see cref="RGB"/> in reverse!</para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/cmy.js</remarks>
[Component(0, 1, "C", "Cyan"), Component(0, 1, "M", "Magenta"), Component(0, 1, "Y", "Yellow")]
[Serializable]
public sealed class CMY : ColorVector3
{
    public CMY(params double[] input) : base(input) { }

    public static implicit operator CMY(Vector3 input) => new(input.X, input.Y, input.Z);

    public override Lrgb ToLrgb(WorkingProfile profile)
        => new(1 - Value[0], 1 - Value[1], 1 - Value[2]);

    public override void FromLrgb(Lrgb input, WorkingProfile profile)
        => Value = new(1 - input[0], 1 - input[1], 1 - input[2]);
}

#endregion

#region (🗸) [98.379%] (HCV)

/// <summary><b>Hue (H), Chroma (C), Gray (V)</b></summary>
/// <remarks>https://github.com/helixd2s/hcv-color</remarks>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "C", "Chroma")]
[Component(0, 100, '%', "V", "Gray")]
[Serializable]
public sealed class HCV : ColorVector3
{
    public HCV(params double[] input) : base(input) { }

    public static implicit operator HCV(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HCV"/> > <see cref="Lrgb"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double h = Value[0] / 360, c = Value[1] / 100.0, g = Value[2] / 100.0;

        if (c == 0)
            return new Lrgb(g, g, g);

        var hi = (h % 1.0) * 6.0;
        var v = hi % 1.0;
        var pure = new double[3];
        var w = 1.0 - v;

        switch (Floor(hi))
        {
            case 0:
                pure[0] = 1; pure[1] = v; pure[2] = 0; break;
            case 1:
                pure[0] = w; pure[1] = 1; pure[2] = 0; break;
            case 2:
                pure[0] = 0; pure[1] = 1; pure[2] = v; break;
            case 3:
                pure[0] = 0; pure[1] = w; pure[2] = 1; break;
            case 4:
                pure[0] = v; pure[1] = 0; pure[2] = 1; break;
            default:
                pure[0] = 1; pure[1] = 0; pure[2] = w; break;
        }

        var mg = (1.0 - c) * g;
        return new Lrgb
        (
            c * pure[0] + mg,
            c * pure[1] + mg,
            c * pure[2] + mg
        );
    }

    /// <summary><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="HCV"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        double r = input.Value[0], g = input.Value[1], b = input.Value[2];

        var max = Max(Max(r, g), b);
        var min = Min(Min(r, g), b);

        var chroma = max - min;
        double grayscale = 0;
        double hue;

        if (chroma < 1)
            grayscale = min / (1.0 - chroma);

        if (chroma > 0)
        {
            if (max == r)
            {
                hue = ((g - b) / chroma) % 6;
            }
            else if (max == g)
            {
                hue = 2 + (b - r) / chroma;
            }
            else hue = 4 + (r - g) / chroma;

            hue /= 6;
            hue %= 1;
        }
        else hue = 0;

        Value = new(hue * 360, chroma * 100, grayscale * 100);
    }
}

#endregion

#region (🗸) [68.648%] (HCY)

/// <summary><b>Hue (H), Chroma (C), Luminance (Y)</b></summary>
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

#endregion

#region (🗸) [76.778%] (HSB)

/// <summary>
/// <para><b>Hue (H), Saturation (S), Brightness (B)</b></para>
/// <i>Alias</i>
/// <list type="bullet">
/// <item>HSV</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/hsb.js</remarks>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "S", "Saturation")]
[Component(0, 100, '%', "B", "Brightness")]
[Serializable]
public sealed class HSB : ColorVector3
{
    public HSB(params double[] input) : base(input) { }

    public static implicit operator HSB(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HSB"/> > <see cref="RGB"/></summary>
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

    /// <summary><see cref="RGB"/> > <see cref="HSB"/></summary>
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

#endregion

#region (🗸) [73.105%] (HSL)

/// <summary><b>Hue (H), Saturation (S), Lightness (L)</b></summary>
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

#endregion

#region (🞩) [62.752%] (HSM)

/// <summary>
/// <b>Hue (H), Saturation (S), Mixture (M)</b>
/// <para>🞩 <i>Why is only one hue seemingly displayed?</i></para>
/// </summary>
/// <remarks>https://seer.ufrgs.br/rita/article/viewFile/rita_v16_n2_p141/7428</remarks>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "S", "Saturation")]
[Component(0, 255, ' ', "M", "Mixture")]
[Serializable, Unfinished]
public sealed class HSM : ColorVector3
{
    public HSM(params double[] input) : base(input) { }

    public static implicit operator HSM(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HSM"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var max = GetMaximum<HSM>();

        double h = X / max[0], s = Y / max[1], m = Z / max[2];
        double r, g, b;

        var u = Cos(h);
        var v = s * u;
        var w = Sqrt(41);

        r = (3 / 41) * v + m - (4 / 861 * Sqrt(861 * Pow2(s) * (1 - Pow2(u))));
        g = (w * v + (23 * m) - (19 * r)) / 4;
        b = ((11 * r) - (9 * m) - (w * v)) / 2;

        return new(r, g, b);
    }

    /// <summary><see cref="RGB"/> > <see cref="HSM"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var m = ((4 * input.X) + (2 * input.Y) + input.Z) / 7;

        double t, w;

        var j = (3 * (input.X - m) - 4 * (input.Y - m) - 4 * (input.Z - m)) / Sqrt(41);
        var k = Sqrt(Pow2(input.X - m) + Pow2(input.Y - m) + Pow2(input.Z - m));

        t = Acos(j / k);
        w = input.Z <= input.Y ? t : PI2 - t;

        double r = input.X, g = input.Y, b = input.Z;

        double u, v = 0;
        u = Pow2(r - m) + Pow2(g - m) + Pow2(b - m);

        if (AB(m, 0 / 7, 1 / 7))
        {
            v = Pow2(0 - m) + Pow2(0 - m) + Pow2(7 - m);
        }
        else if (aB(m, 1 / 7, 3 / 7))
        {
            v = Pow2(0 - m) + Pow2(((7 * m - 1) / 2) - m) + Pow2(1 - m);
        }
        else if (aB(m, 3 / 7, 1 / 2))
        {
            v = Pow2(((7 * m - 3) / 2) - m) + Pow2(1 - m) + Pow2(1 - m);
        }
        else if (aB(m, 1 / 2, 4 / 7))
        {
            v = Pow2(((7 * m) / 4) - m) + Pow2(0 - m) + Pow2(0 - m);
        }
        else if (aB(m, 4 / 7, 6 / 7))
        {
            v = Pow2(1 - m) + Pow2(((7 * m - 4) / 2) - m) + Pow2(0 - m);
        }
        else if (aB(m, 6 / 7, 7 / 7))
        {
            v = Pow2(1 - m) + Pow2(1 - m) + Pow2((7 * m - 6) - m);
        }

        double h = w / PI2;
        double s = Sqrt(u) / Sqrt(v);

        var max = GetMaximum<HSM>();
        Value = new(h * max[0], s * max[1], m * max[2]);
    }
}

#endregion

#region (🗸) [99.818%] (HSP)

/// <summary><b>Hue (H), Saturation (S), Percieved brightness (P)</b></summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/hsp.js</remarks>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "S", "Saturation")]
[Component(0, 255, ' ', "P", "Percieved brightness")]
[Serializable]
public sealed class HSP : ColorVector3
{
    public HSP(params double[] input) : base(input) { }

    public static implicit operator HSP(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HSP"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
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
        return new Lrgb(new Vector(r.Round() / 255.0, g.Round() / 255.0, b.Round() / 255.0).Coerce(0, 1));
    }

    /// <summary><see cref="RGB"/> > <see cref="HSP"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
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

#endregion

#region (🗸) [73.105%] (HWB) 

/// <summary><b>Hue (H), Whiteness (W), Blackness (B)</b></summary>
/// <remarks>https://drafts.csswg.org/css-color/#the-hwb-notation</remarks>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "W", "Whiteness")]
[Component(0, 100, '%', "B", "Blackness")]
[Serializable]
public sealed class HWB : ColorVector3
{
    public HWB(params double[] input) : base(input) { }

    public static implicit operator HWB(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HWB"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var white = Value[1] / 100;
        var black = Value[2] / 100;
        if (white + black >= 1)
        {
            var gray = white / (white + black);
            return new(gray, gray, gray);
        }
        var rgb = new HSL(Value[0], 100, 50).ToLrgb(profile);
        for (var i = 0; i < 3; i++)
        {
            rgb[i] *= (1 - white - black);
            rgb[i] += white;
        }
        return rgb;
    }

    /// <summary><see cref="RGB"/> > <see cref="HWB"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var hsl = new HSL();
        hsl.FromLrgb(input, profile);

        var white = Min(input[0], Min(input[1], input[2]));
        var black = 1 - Max(input[0], Max(input[1], input[2]));
        Value = new(hsl[0], white * 100, black * 100);
    }
}

#endregion

#region (🞩) [0     %] (ICtCp)

/// <summary>
/// <para><b>Intensity (I), Blue/yellow (Ct), Red/green (Cp)</b></para>
/// 
/// <para>Requires <see cref="Rec2020Companding">Rec2100Companding</see>?</para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>ICTCP</item>
/// <item>ITP</item>
/// </list>
/// </summary>
/// <remarks></remarks>
[Component(0, 1, '%', "I", "Intensity")]
[Component(-1, 1, '%', "Ct", "Blue/yellow")]
[Component(-1, 1, '%', "Cp", "Red/green")]
[Serializable, Unfinished]
public sealed class ICtCp : ColorVector3
{
    public ICtCp(params double[] input) : base(input) { }

    public static implicit operator ICtCp(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="ICtCp"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile) => new();

    /// <summary><see cref="RGB"/> > <see cref="ICtCp"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile) { }
}

#endregion

#region (🗸) [100.00%] (JzAzBz)

/// <summary
/// <para><b>Lightness (Jz), Red/green (Az), Yellow/blue (Bz)</b></para>
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Jzazbz</item>
/// </list>
/// </summary>
/// <remarks>https://observablehq.com/@jrus/jzazbz</remarks>
[Component(.0, 1, '%', "Jz", "Lightness")]
[Component(-1, 1, '%', "Az", "Red/green")]
[Component(-1, 1, '%', "Bz", "Yellow/blue")]
[Serializable]
public sealed class JzAzBz : XYZVector
{
    public JzAzBz(params double[] input) : base(input) { }

    public static implicit operator JzAzBz(Vector3 input) => new(input.X, input.Y, input.Z);

    static double PerceptualQuantizer(double x)
    {
        var xx = Pow(x * 1e-4, 0.1593017578125);
        var result = Pow((0.8359375 + 18.8515625 * xx) / (1 + 18.6875 * xx), y: 134.034375);
        return result;
    }

    static double PerceptualQuantizerInverse(double X)
    {
        var XX = Pow(X, 7.460772656268214e-03);
        var result = 1e4 * Pow((0.8359375 - XX) / (18.6875 * XX - 18.8515625), y: 6.277394636015326);
        return result;
    }

    /// <summary><see cref="JzAzBz"/> > <see cref="XYZ"/></summary>
    public override XYZ ToXYZ(WorkingProfile profile)
    {
        var Jz = Value[0]; var az = Value[1]; var bz = Value[2];

        Jz = Jz + 1.6295499532821566e-11;
        var Iz = Jz / (0.44 + 0.56 * Jz);

        var L = PerceptualQuantizerInverse(Iz + 1.386050432715393e-1 * az + 5.804731615611869e-2 * bz);
        var M = PerceptualQuantizerInverse(Iz - 1.386050432715393e-1 * az - 5.804731615611891e-2 * bz);
        var S = PerceptualQuantizerInverse(Iz - 9.601924202631895e-2 * az - 8.118918960560390e-1 * bz);

        var X = +1.661373055774069e+00 * L - 9.145230923250668e-01 * M + 2.313620767186147e-01 * S;
        var Y = -3.250758740427037e-01 * L + 1.571847038366936e+00 * M - 2.182538318672940e-01 * S;
        var Z = -9.098281098284756e-02 * L - 3.127282905230740e-01 * M + 1.522766561305260e+00 * S;

        return new(X / 10000d, Y / 10000d, Z / 10000d);
    }

    /// <summary><see cref="XYZ"/> > <see cref="JzAzBz"/></summary>
    public override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        var X = input.X * 10000d; var Y = input.Y * 10000d; var Z = input.Z * 10000d;

        var Lp = PerceptualQuantizer(0.674207838 * X + 0.382799340 * Y - 0.047570458 * Z);
        var Mp = PerceptualQuantizer(0.149284160 * X + 0.739628340 * Y + 0.083327300 * Z);
        var Sp = PerceptualQuantizer(0.070941080 * X + 0.174768000 * Y + 0.670970020 * Z);

        var Iz = 0.5 * (Lp + Mp);

        var az = 3.524000 * Lp - 4.066708 * Mp + 0.542708 * Sp;
        var bz = 0.199076 * Lp + 1.096799 * Mp - 1.295875 * Sp;
        var Jz = 0.44 * Iz / (1 - 0.56 * Iz) - 1.6295499532821566e-11;

        Value = new(Jz, az, bz);
    }
}

#endregion

#region (🗸) [50.000%] (JzAzBz) > (HzUzVz)

/// <summary>
/// <para><b>Hue (Hz), Chroma (Uz), Luminance (Vz)</b></para>
/// Similar to, and based on, <see cref="JzCzHz"/>, an experimental derivative of <see cref="JzAzBz"/> that maps chroma (<see cref="Uz"/>) to a sphere.
/// <para><i>Author</i>: <see href="https://github.com/imagin-tech">Imagin</see> (2022)</para>
/// </summary>
[Component(0, 360, '°', "Hz", "Hue")]
[Component(0, 100, '%', "Uz", "Chroma")]
[Component(0, 100, '%', "Vz", "Luminance")]
[Serializable]
public sealed class HzUzVz : JzAzBzVector
{
    /// <summary><see cref="HUV.Hue"/></summary>
    public double Hz => X;

    /// <summary><see cref="HUV.Chroma"/></summary>
    public double Uz => Y;

    /// <summary><see cref="HUV.Lightness"/></summary>
    public double Vz => Z;

    public HzUzVz(params double[] input) : base(input) { }

    public static implicit operator HzUzVz(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HzUzVz"/> > <see cref="JzAzBz"/></summary>
    public override JzAzBz ToJzAzBz(WorkingProfile profile) => new HUV(this).To() / 100;

    /// <summary><see cref="JzAzBz"/> > <see cref="HzUzVz"/></summary>
    public override void FromJzAzBz(JzAzBz input, WorkingProfile profile)
    {
        var huv = new HUV();
        huv.From((Vector3)input * 100);

        Value = huv;
    }
}

#endregion

#region (🗸) [100.00%] (JzAzBz) > (JzCzHz)

/// <summary>
/// <para><b>Lightness (Jz), Chroma (Cz), Hue (Hz)</b></para>
/// <i>Alias</i>
/// <list type="bullet">
/// <item>JzCzhz</item>
/// </list>
/// </summary>
/// <remarks>https://observablehq.com/@jrus/jzazbz</remarks>
[Component(0, 1.0, '%', "Jz", "Lightness")]
[Component(0, 1.0, '%', "Cz", "Chroma")]
[Component(0, 360, '%', "Hz", "Hue")]
[Serializable]
public sealed class JzCzHz : JzAzBzVector
{
    public JzCzHz(params double[] input) : base(input) { }

    public static implicit operator JzCzHz(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="JzCzHz"/> > <see cref="JzAzBz"/></summary>
    public override JzAzBz ToJzAzBz(WorkingProfile profile) => new(new LCH(this).To());

    /// <summary><see cref="JzAzBz"/> > <see cref="JzCzHz"/></summary>
    public override void FromJzAzBz(JzAzBz input, WorkingProfile profile)
    {
        var lch = new LCH();
        lch.From(input);

        Value = lch;
    }
}

#endregion

#region (🞩) [100.00%] (Lab)

/// <summary>
/// <para><b>Lightness (L*), a*, b*</b></para>
/// <para>🞩 <i>Why is only one hue seemingly displayed?</i></para>
/// <i>Alias</i>
/// <list type="bullet">
/// <item>CIELAB</item>
/// <item>L*a*b*</item>
/// </list>
/// <para><i>Author</i>: <see cref="CIE"/> (1976)</para>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 100, '%', "L*", "Lightness")]
[Component(-100, 100, ' ', "a*")]
[Component(-100, 100, ' ', "b*")]
[Serializable]
public sealed class Lab : XYZVector
{
    public Lab(params double[] input) : base(input) { }

    public static implicit operator Lab(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="Lab"/> > <see cref="XYZ"/></summary>
    public override XYZ ToXYZ(WorkingProfile profile)
    {
        double L = Value[0], a = Value[1], b = Value[2];

        var fy = (L + 16) / 116d;
        var fx = a / 500d + fy;
        var fz = fy - b / 200d;

        var fx3 = Pow(fx, 3);
        var fz3 = Pow(fz, 3);

        var xr = fx3 > CIE.IEpsilon ? fx3 : (116 * fx - 16) / CIE.IKappa;
        var yr = L > CIE.IKappa * CIE.IEpsilon ? Pow((L + 16) / 116d, 3) : L / CIE.IKappa;
        var zr = fz3 > CIE.IEpsilon ? fz3 : (116 * fz - 16) / CIE.IKappa;

        return new(xr * profile.White[0], yr * profile.White[1], zr * profile.White[2]);
    }

    /// <summary><see cref="XYZ"/> > <see cref="Lab"/></summary>
    public override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        double Xr = profile.White[0], Yr = profile.White[1], Zr = profile.White[2];

        double xr = input[0] / Xr, yr = input[1] / Yr, zr = input[2] / Zr;

        static double f(double cr)
            => cr > CIE.IEpsilon
            ? Pow(cr, 1 / 3d)
            : (CIE.IKappa * cr + 16) / 116d;

        var fx = f(xr); var fy = f(yr); var fz = f(zr);

        var l = 116 * fy - 16;
        var a = 500 * (fx - fy);
        var b = 200 * (fy - fz);
        Value = new(l, a, b);
    }
}

#endregion

#region (🗸) [50.000%] (Lab) > (HUVab)

/// <summary>
/// <para><b>Hue (H), Chroma (Ua), Luminance (Vb)</b></para>
/// Similar to, and based on, <see cref="LCHab"/>, an experimental derivative of <see cref="Lab"/> that maps chroma (<see cref="U"/>) to a sphere.
/// <para><i>Author</i>: <see href="https://github.com/imagin-tech">Imagin</see> (2022)</para>
/// </summary>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "Ua", "Chroma")]
[Component(0, 100, '%', "Vb", "Luminance")]
[Serializable]
public sealed class HUVab : LabVector
{
    /// <summary><see cref="HUV.Hue"/></summary>
    public double H => X;

    /// <summary><see cref="HUV.Chroma"/></summary>
    public double U => Y;

    /// <summary><see cref="HUV.Lightness"/></summary>
    public double V => Z;

    public HUVab(params double[] input) : base(input) { }

    public static implicit operator HUVab(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HUVab"/> > <see cref="Lab"/></summary>
    public override Lab ToLAB(WorkingProfile profile) => new HUV(this).To();

    /// <summary><see cref="Lab"/> > <see cref="HUVab"/></summary>
    public override void FromLAB(Lab input, WorkingProfile profile)
    {
        var huv = new HUV();
        huv.From(input);

        Value = huv;
    }
}

#endregion

#region (🗸) [100.00%] (Lab) > (LCHab)

/// <summary><b>Luminance (L), Chroma (Ca), Hue (Hb)</b></summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 100, '%', "L", "Luminance")]
[Component(0, 100, '%', "Ca", "Chroma")]
[Component(0, 360, '°', "Hb", "Hue")]
[Serializable]
public sealed class LCHab : LabVector
{
    public LCHab(params double[] input) : base(input) { }

    public static implicit operator LCHab(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="LCHab"/> > <see cref="Lab"/></summary>
    public override Lab ToLAB(WorkingProfile profile)
        => new(new LCH(this).To());

    /// <summary><see cref="Lab"/> > <see cref="LCHab"/></summary>
    public override void FromLAB(Lab input, WorkingProfile profile)
    {
        var lch = new LCH();
        lch.From(input);

        Value = lch;
    }
}

#endregion

#region (🗸) [100.00%] (Labh)

/// <summary>
/// <para><b>Lightness (L), a, b</b></para>
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Hunter Lab</item>
/// </list>
/// <para><i>Author</i>: Richard S. Hunter (1948)</para>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 100, '%', "L", "Lightness")]
[Component(-100, 100, ' ', "a")]
[Component(-100, 100, ' ', "b")]
[Serializable]
public sealed class Labh : XYZVector
{
    public Labh(params double[] input) : base(input) { }

    public static implicit operator Labh(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>Computes the Ka parameter.</summary>
    public static double ComputeKa(XYZ whitePoint)
    {
        if (whitePoint == Illuminant.GetWhite(Illuminant2.C))
            return 175;

        var Ka = 100 * (175 / 198.04) * (whitePoint.X + whitePoint.Y);
        return Ka;
    }

    /// <summary>Computes the Kb parameter.</summary>
    public static double ComputeKb(XYZ whitePoint)
    {
        if (whitePoint == Illuminant.GetWhite(Illuminant2.C))
            return 70;

        var Ka = 100 * (70 / 218.11) * (whitePoint.Y + whitePoint.Z);
        return Ka;
    }

    /// <summary><see cref="Labh"/> > <see cref="XYZ"/></summary>
    public override XYZ ToXYZ(WorkingProfile profile)
    {
        double L = Value[0], a = Value[1], b = Value[2];
        double Xn = profile.White.X, Yn = profile.White.Y, Zn = profile.White.Z;

        var Ka = ComputeKa(profile.White);
        var Kb = ComputeKb(profile.White);

        var Y = Pow(L / 100d, 2) * Yn;
        var X = (a / Ka * Sqrt(Y / Yn) + Y / Yn) * Xn;
        var Z = (b / Kb * Sqrt(Y / Yn) - Y / Yn) * -Zn;
        return new(X, Y, Z);
    }

    /// <summary><see cref="XYZ"/> > <see cref="Labh"/></summary>
    public override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        double X = input.X, Y = input.Y, Z = input.Z;
        double Xn = profile.White[0], Yn = profile.White[1], Zn = profile.White[2];

        var Ka = ComputeKa(profile.White);
        var Kb = ComputeKb(profile.White);

        var L = 100 * Sqrt(Y / Yn);
        var a = Ka * ((X / Xn - Y / Yn) / Sqrt(Y / Yn));
        var b = Kb * ((Y / Yn - Z / Zn) / Sqrt(Y / Yn));

        if (IsNaN(a))
            a = 0;

        if (IsNaN(b))
            b = 0;

        Value = new(L, a, b);
    }
}

#endregion

#region (🗸) [50.000%] (Labh) > (HUVabh)

/// <summary>
/// <para><b>Hue (H), Chroma (Ua), Luminance (Vb)</b></para>
/// <para>Similar to, and based on, <see cref="LCHabh"/>, an experimental derivative of <see cref="Labh"/> that maps chroma (<see cref="U"/>) to a sphere.</para>
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Hunter HUVab</item>
/// </list>
/// <para><i>Author</i>: <see href="https://github.com/imagin-tech">Imagin</see> (2022)</para>
/// </summary>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "Ua", "Chroma")]
[Component(0, 100, '%', "Vb", "Luminance")]
[Serializable]
public sealed class HUVabh : LabhVector
{
    /// <summary><see cref="HUV.Hue"/></summary>
    public double H => X;

    /// <summary><see cref="HUV.Chroma"/></summary>
    public double U => Y;

    /// <summary><see cref="HUV.Lightness"/></summary>
    public double V => Z;

    public HUVabh(params double[] input) : base(input) { }

    public static implicit operator HUVabh(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HUVabh"/> > <see cref="Labh"/></summary>
    public override Labh ToLABh(WorkingProfile profile) => new HUV(this).To();

    /// <summary><see cref="Labh"/> > <see cref="HUVabh"/></summary>
    public override void FromLABh(Labh input, WorkingProfile profile)
    {
        var huv = new HUV();
        huv.From(input);

        Value = huv;
    }
}

#endregion

#region (🗸) [100.00%] (Labh) > (LCHabh)

/// <summary>
/// <b>Luminance (L), Chroma (Ca), Hue (Hb)</b>
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Hunter LCHab</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 100, '%', "L", "Luminance")]
[Component(0, 100, '%', "Ca", "Chroma")]
[Component(0, 360, '°', "Hb", "Hue")]
[Serializable]
public sealed class LCHabh : LabhVector
{
    public LCHabh(params double[] input) : base(input) { }

    public static implicit operator LCHabh(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="LCHabh"/> > <see cref="Labh"/></summary>
    public override Labh ToLABh(WorkingProfile profile)
        => new(new LCH(this).To());

    /// <summary><see cref="Labh"/> > <see cref="LCHabh"/></summary>
    public override void FromLABh(Labh input, WorkingProfile profile)
    {
        var lch = new LCH();
        lch.From(input);

        Value = lch;
    }
}

#endregion

#region (🗸) [100.00%] (LMS)

/// <summary><b>Long (L), Medium (M), Short (S)</b></summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 1, '%', "L", "Long")]
[Component(0, 1, '%', "M", "Medium")]
[Component(0, 1, '%', "S", "Short")]
[Serializable]
public sealed class LMS : XYZVector
{
    public LMS(params double[] input) : base(input) { }

    public static implicit operator LMS(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="LMS"/> > <see cref="XYZ"/></summary>
    public override XYZ ToXYZ(WorkingProfile profile)
    {
        var xyz = LMSTransformationMatrix.VonKriesHPEAdjusted.Invert().Value.Multiply(Value);
        return new(xyz[0], xyz[1], xyz[2]);
    }

    /// <summary><see cref="XYZ"/> > <see cref="LMS"/></summary>
    public override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        Value = new(LMSTransformationMatrix.VonKriesHPEAdjusted.Multiply(input.Value));
    }
}

#endregion

#region (🗸) [100.00%] (Luv)

/// <summary>
/// <para><b>Lightness (L*), u*, v*</b></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>CIELUV</item>
/// <item>L*u*v*</item>
/// </list>
/// 
/// <para><i>Author</i>: <see cref="CIE"/> (1976)</para>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 100, '%', "L*", "Lightness")]
[Component(-134, 224, ' ', "u*")]
[Component(-140, 122, ' ', "v*")]
[Serializable]
public sealed class Luv : XYZVector
{
    public Luv(params double[] input) : base(input) { }

    public static implicit operator Luv(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="Luv"/> > <see cref="XYZ"/></summary>
    public override XYZ ToXYZ(WorkingProfile profile)
    {
        static double Compute_u0(XYZ input) => 4 * input.X / (input.X + 15 * input.Y + 3 * input.Z);
        static double Compute_v0(XYZ input) => 9 * input.Y / (input.X + 15 * input.Y + 3 * input.Z);

        double L = Value[0], u = Value[1], v = Value[2];

        var u0 = Compute_u0(profile.White);
        var v0 = Compute_v0(profile.White);

        var Y = L > CIE.IKappa * CIE.IEpsilon
            ? Pow((L + 16) / 116, 3)
            : L / CIE.IKappa;

        var a = (52 * L / (u + 13 * L * u0) - 1) / 3;
        var b = -5 * Y;
        var c = -1 / 3d;
        var d = Y * (39 * L / (v + 13 * L * v0) - 5);

        var X = (d - b) / (a - c);
        var Z = X * a + b;

        if (IsNaN(X) || X < 0)
            X = 0;

        if (IsNaN(Y) || Y < 0)
            Y = 0;

        if (IsNaN(Z) || Z < 0)
            Z = 0;

        return new(X, Y, Z);
    }

    /// <summary><see cref="XYZ"/> > <see cref="Luv"/></summary>
    public override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        static double Compute_up(XYZ i) => 4 * i.X / (i.X + 15 * i.Y + 3 * i.Z);
        static double Compute_vp(XYZ i) => 9 * i.Y / (i.X + 15 * i.Y + 3 * i.Z);

        var yr = input.Y / profile.White.Y;
        var up = Compute_up(input);
        var vp = Compute_vp(input);

        var upr = Compute_up(profile.White);
        var vpr = Compute_vp(profile.White);

        var L = yr > CIE.IEpsilon ? 116 * Pow(yr, 1 / 3d) - 16 : CIE.IKappa * yr;

        if (IsNaN(L) || L < 0)
            L = 0;

        var u = 13 * L * (up - upr);
        var v = 13 * L * (vp - vpr);

        if (IsNaN(u))
            u = 0;

        if (IsNaN(v))
            v = 0;

        Value = new(L, u, v);
    }
}

#endregion

#region (🗸) [50.000%] (Luv) > (HUVuv)

/// <summary>
/// <para><b>Hue (H), Chroma (Uu), Luminance (Vv)</b></para>
/// Similar to, and based on, <see cref="LCHuv"/>, an experimental derivative of <see cref="Luv"/> that maps chroma (<see cref="U"/>) to a sphere.
/// <para><i>Author</i>: <see href="https://github.com/imagin-tech">Imagin</see> (2022)</para>
/// </summary>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "Uu", "Chroma")]
[Component(0, 100, '%', "Vv", "Luminance")]
[Serializable]
public sealed class HUVuv : LuvVector
{
    /// <summary><see cref="HUV.Hue"/></summary>
    public double H => X;

    /// <summary><see cref="HUV.Chroma"/></summary>
    public double U => Y;

    /// <summary><see cref="HUV.Lightness"/></summary>
    public double V => Z;

    public HUVuv(params double[] input) : base(input) { }

    public static implicit operator HUVuv(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HUVuv"/> > <see cref="Luv"/></summary>
    public override Luv ToLUV(WorkingProfile profile) => new HUV(this).To();

    /// <summary><see cref="Luv"/> > <see cref="HUVuv"/></summary>
    public override void FromLUV(Luv input, WorkingProfile profile)
    {
        var huv = new HUV();
        huv.From(input);

        Value = huv;
    }
}

#endregion

#region (🗸) [100.00%] (Luv) > (LCHuv)

/// <summary><b>Luminance (L), Chroma (Cu), Hue (Hv)</b></summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 100, '%', "L", "Luminance")]
[Component(0, 100, '%', "Cu", "Chroma")]
[Component(0, 360, '°', "Hv", "Hue")]
[Serializable]
public sealed class LCHuv : LuvVector
{
    public LCHuv(params double[] input) : base(input) { }

    public static implicit operator LCHuv(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="LCHuv"/> > <see cref="Luv"/></summary>
    public override Luv ToLUV(WorkingProfile profile)
        => new(new LCH(this).To());

    /// <summary><see cref="Luv"/> > <see cref="LCHuv"/></summary>
    public override void FromLUV(Luv input, WorkingProfile profile)
    {
        var lch = new LCH();
        lch.From(input);

        Value = lch;
    }
}

#endregion

#region (🗸) [100.00%] (Luv) > (LCHuv) > (HPLuv)

/// <summary><b>Hue (H), Saturation (P), Lightness (L)</b></summary>
/// <remarks>https://github.com/hsluv/hsluv-csharp</remarks>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "P", "Saturation")]
[Component(0, 100, '%', "L", "Lightness")]
[Serializable]
public sealed class HPLuv : LCHuvVector
{
    public HPLuv(params double[] input) : base(input) { }

    public static implicit operator HPLuv(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HPLuv"/> > <see cref="LCHuv"/></summary>
    public override LCHuv ToLCHuv(WorkingProfile profile)
    {
        double H = Value[0], S = Value[1], L = Value[2];

        if (L > 99.9999999)
            return new(100, 0, H);

        if (L < 0.00000001)
            return new(0, 0, H);

        double max = GetChroma(L);
        double C = max / 100 * S;

        return new(L, C, H);
    }

    /// <summary><see cref="LCHuv"/> > <see cref="HPLuv"/></summary>
    public override void FromLCHuv(LCHuv input, WorkingProfile profile)
    {
        double L = input[0], C = input[1], H = input[2];

        if (L > 99.9999999)
        {
            Value = new(H, 0, 100);
            return;
        }

        if (L < 0.00000001)
        {
            Value = new(H, 0, 0);
            return;
        }

        double max = GetChroma(L);
        double S = C / max * 100;

        Value = new(H, S, L);
    }
}

#endregion

#region (🗸) [100.00%] (Luv) > (LCHuv) > (HSLuv)

/// <summary><b>Hue (H), Saturation (S), Lightness (L)</b></summary>
/// <remarks>https://github.com/hsluv/hsluv-csharp</remarks>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "S", "Saturation")]
[Component(0, 100, '%', "L", "Lightness")]
[Serializable]
public sealed class HSLuv : LCHuvVector
{
    public HSLuv(params double[] input) : base(input) { }

    public static implicit operator HSLuv(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HSLuv"/> > <see cref="LCHuv"/></summary>
    public override LCHuv ToLCHuv(WorkingProfile profile)
    {
        double H = Value[0], S = Value[1], L = Value[2];

        if (L > 99.9999999)
            return new(100, 0, H);

        if (L < 0.00000001)
            return new(0, 0, H);

        double max = GetChroma(L, H);
        double C = max / 100 * S;

        return new(L, C, H);
    }

    /// <summary><see cref="LCHuv"/> > <see cref="HSLuv"/></summary>
    public override void FromLCHuv(LCHuv input, WorkingProfile profile)
    {
        double L = input[0], C = input[1], H = input[2];

        if (L > 99.9999999)
        {
            Value = new(H, 0, 100);
            return;
        }

        if (L < 0.00000001)
        {
            Value = new(H, 0, 0);
            return;
        }

        double max = GetChroma(L, H);
        double S = C / max * 100;

        Value = new(H, S, L);
    }
}

#endregion

#region (🞩) [36.884%] (TSL)

/// <summary>
/// <b>Tint (T), Saturation (S), Lightness (L)</b>
/// <para>🞩 <i>Conversion accuracy is very low.</i></para>
/// <para>🞩 <i>The color space repeats unless <see cref="T"/> / 4. Is this expected?</i></para>
/// </summary>
/// <remarks>https://en.wikipedia.org/wiki/TSL_color_space#Conversion_between_RGB_and_TSL</remarks>
[Component(0, 1, '%', "T", "Tint")]
[Component(0, 1, '%', "S", "Saturation")]
[Component(0, 1, '%', "L", "Lightness")]
[Serializable, Unfinished]
public sealed class TSL : ColorVector3
{
    public double T => X;

    public double S => Y;

    public double L => Z;

    public TSL(params double[] input) : base(input) { }

    public static implicit operator TSL(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="TSL"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double T = this[0] / 4, S = this[1], L = this[2];

        var y = 2 * PI * T;
        var x = -(Cos(y) / Sin(y));

        var bP = 5 / (9 * (Pow(x, 2) + 1));
        var gP = T > 0.5 ? -Sqrt(bP) * S : T < 0.5 ? Sqrt(bP) * S : 0;
        var rP = T == 0 ? Sqrt(5) / 3 * S : x * gP;

        var r = rP + (1 / 3);
        var g = gP + (1 / 3);

        var k = L / (r * 0.185 + g * 0.473 + 0.114);

        var R = k * r; var G = k * g; var B = k * (1 - r - g);
        return new(R, G, B);
    }

    /// <summary><see cref="RGB"/> > <see cref="TSL"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var R = input[0]; var G = input[1]; var B = input[2];

        var sum = R + G + B;
        var r = R / sum;
        var g = G / sum;

        double rP = r - (1 / 3), gP = g - (1 / 3), bP = 1 / (2 * PI);

        var T = gP > 0
            ? bP * Atan((rP / gP) + (1 / 4))
            : gP < 0
            ? bP * Atan((rP / gP) + (3 / 4))
            : 0;

        var S = Sqrt(9 / 5 * (Pow2(rP) + Pow2(gP)));
        var L = (R * 0.299) + (G * 0.587) + (B * 0.114);
        Value = new(T, S, L);
    }
}

#endregion

#region (🗸) [100.00%] (XYZ)

/// <summary>
/// <para><b>X, Y, Z</b></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>CIEXYZ</item>
/// </list>
/// 
/// <para><i>Author</i>: <see cref="CIE"/> (1931)</para>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 1, "X"), Component(0, 1, "Y"), Component(0, 1, "Z")]
[Serializable]
public sealed class XYZ : ColorVector3
{
    public XYZ(params double[] input) : base(input) { }

    public static implicit operator XYZ(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="XYZ"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var p = profile.Chromacity; var w = profile.White;
        return new(WorkingProfile.GetRxGyBz(p, w).Invert().Value.Multiply(Value));
    }

    /// <summary><see cref="RGB"/> > <see cref="XYZ"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var s = input.Value;
        var p = profile.Chromacity; var w = profile.White;
        var m = WorkingProfile.GetRxGyBz(p, w);
        Value = m.Multiply(s);
    }
}

#endregion

#region (🗸) [100.00%] (XYZ) > (OKLab)

/// <summary><b>Perceived lightness (L), Red/green (a), Blue/yellow (b)</b></summary>
/// <remarks>https://colour.readthedocs.io/en/develop/_modules/colour/models/oklab.html</remarks>
[Component(0, 1, '°', "L", "Perceived lightness")]
[Component(0, 1, '%', "a", "Red/green")]
[Component(0, 1, '%', "b", "Blue/yellow")]
[Serializable]
public sealed class OKLab : XYZVector
{
    public OKLab(params double[] input) : base(input) { }

    public static implicit operator OKLab(Vector3 input) => new(input.X, input.Y, input.Z);

    static Matrix XYZ_LMS => new double[][]
    {
        new[] { 0.8189330101, 0.3618667424, -0.1288597137 },
        new[] { 0.0329845436, 0.9293118715,  0.0361456387 },
        new[] { 0.0482003018, 0.2643662691,  0.6338517070 },
    };
    static Matrix LMS_XYZ => XYZ_LMS.Invert3By3();

    static Matrix LMS_LAB => new double[][]
    {
        new[] { 0.2104542553,  0.7936177850, -0.0040720468 },
        new[] { 1.9779984951, -2.4285922050,  0.4505937099 },
        new[] { 0.0259040371,  0.7827717662, -0.8086757660 },
    };
    static Matrix LAB_LMS => LMS_LAB.Invert3By3();

    /// <summary><see cref="OKLab"/> > <see cref="XYZ"/></summary>
    public override XYZ ToXYZ(WorkingProfile profile)
    {
        var lab = Value;

        var lms = LAB_LMS.Multiply(lab);
        var lmsPrime = new Vector(Pow(lms[0], 3), Pow(lms[1], 3), Pow(lms[2], 3));

        var xyz = LMS_XYZ.Multiply(lmsPrime);
        return new(xyz * profile.White);
    }

    /// <summary><see cref="XYZ"/> > <see cref="OKLab"/></summary>
    public override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        XYZ xyz = new(input.Value / profile.White);

        var lms = XYZ_LMS.Multiply(xyz);
        var lmsPrime = new LMS(Cbrt(lms[0]), Cbrt(lms[1]), Cbrt(lms[2]));

        var lab = LMS_LAB.Multiply(lmsPrime);
        Value = new(lab);
    }
}

#endregion

#region (🞩) [0     %] (XYZ) > (OKLab) > (HSBok)

/// <summary><b>Hue (H), Saturation (S), Brightness (B)</b></summary>
/// <remarks>https://colour.readthedocs.io/en/develop/_modules/colour/models/oklab.html</remarks>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "S", "Saturation")]
[Component(0, 100, '%', "B", "Brightness")]
[Serializable, Unfinished]
public sealed class HSBok : OKLabVector
{
    public HSBok(params double[] input) : base(input) { }

    public static implicit operator HSBok(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HSBok"/> > <see cref="OKLab"/></summary>
    public override OKLab ToOKLab(WorkingProfile profile) => new();

    /// <summary><see cref="OKLab"/> > <see cref="HSBok"/></summary>
    public override void FromOKLab(OKLab input, WorkingProfile profile) { }
}

#endregion

#region (🞩) [0     %] (XYZ) > (OKLab) > (HSLok)

/// <summary><b>Hue (H), Saturation (S), Lightness (L)</b></summary>
/// <remarks>https://colour.readthedocs.io/en/develop/_modules/colour/models/oklab.html</remarks>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "S", "Saturation")]
[Component(0, 100, '%', "L", "Lightness")]
[Serializable, Unfinished]
public sealed class HSLok : OKLabVector
{
    public HSLok(params double[] input) : base(input) { }

    public static implicit operator HSLok(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HSLok"/> > <see cref="OKLab"/></summary>
    public override OKLab ToOKLab(WorkingProfile profile) => new();

    /// <summary><see cref="OKLab"/> > <see cref="HSLok"/></summary>
    public override void FromOKLab(OKLab input, WorkingProfile profile) { }
}

#endregion

#region (🗸) [100.00%] (XYZ) > (UCS)

/// <summary>
/// <para><b>U, C (V), S (W)</b></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>CIEUCS</item>
/// <item>Uniform Color Space</item>
/// <item>Uniform Color Scale</item>
/// <item>Uniform Chromaticity Scale</item>
/// <item>Uniform Chromaticity Space</item>
/// </list>
/// <para><i>Author</i>: <see cref="CIE"/>/David MacAdam (1960)</para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/ucs.js</remarks>
[Component(0, 100, '%', "U", "U")]
[Component(0, 100, '%', "C", "V")]
[Component(0, 100, '%', "S", "W")]
[Serializable]
public sealed class UCS : XYZVector
{
    public UCS(params double[] input) : base(input) { }

    public static implicit operator UCS(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="UCS"/> > <see cref="XYZ"/></summary>
    public override XYZ ToXYZ(WorkingProfile profile)
    {
        double u = this[0], v = this[1], w = this[2];
        return new(1.5 * u, v, 1.5 * u - 3 * v + 2 * w);
    }

    /// <summary><see cref="XYZ"/> > <see cref="UCS"/></summary>
    public override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        double x = input[0], y = input[1], z = input[2];

        Value = new
        (
            x * 2 / 3,
            y,
            0.5 * (-x + 3 * y + z)
        );
    }
}

#endregion

#region (🞩) [56.288%] (XYZ) > (UVW)

/// <summary>
/// <para><b>U*, V*, W*</b></para>
/// <para>🞩 <i>Conversion accuracy is very low.</i></para>
/// <para>🞩 <i>The color space is clipped when displayed. Is this expected?</i></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>CIEUVW</item>
/// <item>U*V*W*</item>
/// </list>
/// 
/// <para><i>Author</i>: <see cref="CIE"/> (1964)</para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/uvw.js</remarks>
[Component(-134, 224, ' ', "U*")]
[Component(-140, 122, ' ', "V*")]
[Component(0, 100, '%', "W*")]
[Serializable, Unfinished]
public sealed class UVW : XYZVector
{
    public UVW(params double[] input) : base(input) { }

    public static implicit operator UVW(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="UVW"/> > <see cref="XYZ"/></summary>
    public override XYZ ToXYZ(WorkingProfile profile)
    {
        double _u, _v, w, u, v, x, y, z, xn, yn, zn, un, vn;
        u = this[0]; v = this[1]; w = this[2];

        if (w == 0)
            return new(0, 0, 0);

        xn = profile.White[0]; yn = profile.White[1].Single(); zn = profile.White[2].Single();
        un = (4 * xn) / (xn + (15 * yn) + (3 * zn));
        vn = (6 * yn) / (xn + (15 * yn) + (3 * zn));

        y = Pow((w + 17f) / 25f, 3f).Single();

        _u = 13 * w == 0 ? 0 : u / (13 * w) + un;
        _v = 13 * w == 0 ? 0 : v / (13 * w) + vn;

        x = (6 / 4) * y * _u / _v;
        z = y * (2 / _v - 0.5 * _u / _v - 5).Single();

        return new(x / 100, y / 100, z / 100);
    }

    /// <summary><see cref="XYZ"/> > <see cref="UVW"/></summary>
    public override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        double x = input[0], y = input[1], z = input[2], xn, yn, zn, un, vn;

        xn = profile.White[0]; yn = profile.White[1]; zn = profile.White[2];
        un = (4 * xn) / (xn + (15 * yn) + (3 * zn));
        vn = (6 * yn) / (xn + (15 * yn) + (3 * zn));

        var uv = x + 15 * y + 3 * z;
        var _u = uv == 0 ? 0 : 4 * x / uv;
        var _v = uv == 0 ? 0 : 6 * y / uv;

        var w = 25 * Pow(y, 1 / 3) - 17;
        var u = 13 * w * (_u - un);
        var v = 13 * w * (_v - vn);
        Value = new(u, v, w);
    }
}

#endregion

#region (🗸) [100.00%] (XYZ) > (xyY)

/// <summary><b>x, y, Y</b></summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/xyy.js</remarks>
[Component(0, 1, "x"), Component(0, 1, "y"), Component(0, 1, '%', "Y")]
[Serializable]
public sealed class xyY : XYZVector
{
    public xyY(params double[] input) : base(input) { }

    public static implicit operator xyY(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="xyY"/> > <see cref="XYZ"/></summary>
    public override XYZ ToXYZ(WorkingProfile profile)
    {
        double x = this[0], y = this[1], Y = this[2];
        if (y == 0)
            return new(0, 0, 0);

        return new XYZ(x * Y / y, Y, (1 - x - y) * Y / y);
    }

    /// <summary><see cref="XYZ"/> > <see cref="xyY"/></summary>
    public override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        double sum, X, Y, Z;
        X = input[0]; Y = input[1]; Z = input[2];

        sum = X + Y + Z;
        if (sum == 0)
        {
            Value = new(.0, 0, Y);
            return;
        }
        Value = new(X / sum, Y / sum, Y);
    }
}

#endregion

#region (🗸) [100.00%] (YCoCg)

/// <summary>
/// <para><b>Luminance (Y), Co, Cg</b></para>
/// <i>Alias</i>
/// <list type="bullet">
/// <item>YCgCo</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/ycgco.js</remarks>
[Component(0, 1, '%', "Y", "Luminance")]
[Component(-0.5, 0.5, ' ', "Co")]
[Component(-0.5, 0.5, ' ', "Cg")]
[Serializable]
public sealed class YCoCg : ColorVector3
{
    public YCoCg(params double[] input) : base(input) { }

    public static implicit operator YCoCg(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="YCoCg"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double y = Value[0], cg = Value[1], co = Value[2];

        var c = y - cg;
        return new(c + co, y + cg, c - co);
    }

    /// <summary><see cref="RGB"/> > <see cref="YCoCg"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        double r = input[0], g = input[1], b = input[2];
        Value = new(0.25 * r + 0.5 * g + 0.25 * b, -0.25 * r + 0.5 * g - 0.25 * b, 0.5 * r - 0.5 * b);
    }
}

#endregion

#region (🗸) [100.00%] (YDbDr)

/// <summary><b>Luminance (Y), Db, Dr</b></summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/ydbdr.js</remarks>
[Component(0, 1, '%', "Y", "Luminance")]
[Component(-1.333, 1.333, ' ', "Db")]
[Component(-1.333, 1.333, ' ', "Dr")]
[Serializable]
public sealed class YDbDr : ColorVector3
{
    public YDbDr(params double[] input) : base(input) { }

    public static implicit operator YDbDr(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="YDbDr"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double y = Value[0], db = Value[1], dr = Value[2];

        var r = y + 0.000092303716148 * db - 0.525912630661865 * dr;
        var g = y - 0.129132898890509 * db + 0.267899328207599 * dr;
        var b = y + 0.664679059978955 * db - 0.000079202543533 * dr;
        return new(r, g, b);
    }

    /// <summary><see cref="RGB"/> > <see cref="YDbDr"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        double r = input[0], g = input[1], b = input[2];
        Value = new
        (
             0.299 * r + 0.587 * g + 0.114 * b,
            -0.450 * r - 0.883 * g + 1.333 * b,
            -1.333 * r + 1.116 * g + 0.217 * b
        );
    }
}

#endregion

#region (🗸) [100.00%] (YES)

/// <summary><b>Luminance (Y), E-factor (E), S-factor (S)</b></summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/yes.js</remarks>
[Component(0, 1, '%', "Y", "Luminance")]
[Component(0, 1, '%', "E", "E-factor")]
[Component(0, 1, '%', "S", "S-factor")]
[Serializable]
public sealed class YES : ColorVector3
{
    public YES(params double[] input) : base(input) { }

    public static implicit operator YES(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="YES"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double y = Value[0], e = Value[1], s = Value[2];

        var m = new[]
        {
            1,  1.431,  0.126,
            1, -0.569,  0.126,
            1,  0.431, -1.874
        };

        double
            r = y * m[0] + e * m[1] + s * m[2],
            g = y * m[3] + e * m[4] + s * m[5],
            b = y * m[6] + e * m[7] + s * m[8];

        return new(r, g, b);
    }

    /// <summary><see cref="RGB"/> > <see cref="YES"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        double r = input[0], g = input[1], b = input[2];

        var m = new[]
        {
            0.253,  0.684,  0.063,
            0.500, -0.500,  0,
            0.250,  0.250, -0.500
        };

        Value = new(r * m[0] + g * m[1] + b * m[2], r * m[3] + g * m[4] + b * m[5], r * m[6] + g * m[7] + b * m[8]);
    }
}

#endregion

#region (🗸) [99.765%] (YIQ)

/// <summary><b>Y, I, Q</b></summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/yiq.js</remarks>
[Component(0, 1, '%', "Y", "Luminance")]
[Component(-0.5957, 0.5957, ' ', "I")]
[Component(-0.5226, 0.5226, ' ', "Q")]
[Serializable]
public sealed class YIQ : ColorVector3
{
    public YIQ(params double[] input) : base(input) { }

    public static implicit operator YIQ(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="YIQ"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double y = Value[0], i = Value[1], q = Value[2], r, g, b;
        r = (y * 1) + (i * 0.956) + (q * 0.621);
        g = (y * 1) + (i * -0.272) + (q * -0.647);
        b = (y * 1) + (i * -1.108) + (q * 1.705);

        r = Min(Max(0, r), 1);
        g = Min(Max(0, g), 1);
        b = Min(Max(0, b), 1);
        return new(r, g, b);
    }

    /// <summary><see cref="RGB"/> > <see cref="YIQ"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        double r = input[0], g = input[1], b = input[2];

        var y = (r * 0.299) + (g * 0.587) + (b * 0.114);
        double i = 0, q = 0;

        if (r != g || g != b)
        {
            i = (r * 0.596) + (g * -0.275) + (b * -0.321);
            q = (r * 0.212) + (g * -0.528) + (b * 0.311);
        }
        Value = new(y, i, q);
    }
}

#endregion

#region (🗸) [100.00%] (YPbPr)

/// <summary><b>Y, Pb, Pr</b></summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/ypbpr.js</remarks>
[Component(0, 1, '%', "Y", "Luminance")]
[Component(-0.5, 0.5, ' ', "Pb", "")]
[Component(-0.5, 0.5, ' ', "Pr", "")]
[Serializable]
public sealed class YPbPr : ColorVector3
{
    public YPbPr(params double[] input) : base(input) { }

    public static implicit operator YPbPr(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="YPbPr"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double y = Value[0], pb = Value[1], pr = Value[2];

        //ITU-R BT.709
        double kb = 0.0722;
        double kr = 0.2126;

        var r = y + 2 * pr * (1 - kr);
        var b = y + 2 * pb * (1 - kb);
        var g = (y - kr * r - kb * b) / (1 - kr - kb);

        return new(r, g, b);
    }

    /// <summary><see cref="RGB"/> > <see cref="YPbPr"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        double r = input[0], g = input[1], b = input[2];

        //ITU-R BT.709
        double kb = 0.0722;
        double kr = 0.2126;

        var y = kr * r + (1 - kr - kb) * g + kb * b;
        var pb = 0.5 * (b - y) / (1 - kb);
        var pr = 0.5 * (r - y) / (1 - kr);

        Value = new(y, pb, pr);
    }
}

#endregion

#region (🗸) [100.00%] (YPbPr) > (xvYCC)

/// <summary><b>x (Y), v (Cb), Y (Cr)</b></summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/xvycc.js</remarks>
[Component(0, 255, ' ', "x", "Y")]
[Component(0, 255, ' ', "v", "Cb")]
[Component(0, 255, ' ', "Y", "Cr")]
[Serializable]
public sealed class xvYCC : YPbPrVector
{
    public xvYCC(params double[] input) : base(input) { }

    public static implicit operator xvYCC(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="xvYCC"/> > <see cref="YPbPr"/></summary>
    public override YPbPr ToYPbPr(WorkingProfile profile)
    {
        double y = Value[0], cb = Value[1], cr = Value[2];
        return new((y - 16) / 219, (cb - 128) / 224, (cr - 128) / 224);
    }

    /// <summary><see cref="YPbPr"/> > <see cref="xvYCC"/></summary>
    public override void FromYPbPr(YPbPr input, WorkingProfile profile)
    {
        double y = input[0], pb = input[1], pr = input[2];
        Value = new(16 + 219 * y, 128 + 224 * pb, 128 + 224 * pr);
    }
}

#endregion

#region (🗸) [100.00%] (YPbPr) > (YCbCr)

/// <summary>
/// <para><b>Luminance (Y), Cb, Cr</b></para>
/// A digital form of <see cref="YCbCr"/> (ITU-R BT.601/709).</summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/ycbcr.js</remarks>
[Component(16, 235, ' ', "Y", "Luminance")]
[Component(16, 240, ' ', "Cb")]
[Component(16, 240, ' ', "Cr")]
[Serializable]
public sealed class YCbCr : YPbPrVector
{
    public YCbCr(params double[] input) : base(input) { }

    public static implicit operator YCbCr(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="YCbCr"/> > <see cref="YPbPr"/></summary>
    public override YPbPr ToYPbPr(WorkingProfile profile)
    {
        double y = Value[0], cb = Value[1], cr = Value[2];
        return new((y - 16) / 219, (cb - 128) / 224, (cr - 128) / 224);
    }

    /// <summary><see cref="YPbPr"/> > <see cref="YCbCr"/></summary>
    public override void FromYPbPr(YPbPr input, WorkingProfile profile)
    {
        double y = input[0], pb = input[1], pr = input[2];
        Value = new(16 + 219 * y, 128 + 224 * pb, 128 + 224 * pr);
    }
}

#endregion

#region (🗸) [100.00%] (YPbPr) > (YCbCr) > (JPEG)

/// <summary><b>Luminance (Y), Cb, Cr</b></summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/jpeg.js</remarks>
[Component(0, 255, "Y", "Luminance")]
[Component(0, 255, "Cb")]
[Component(0, 255, "Cr")]
[Serializable]
public sealed class JPEG : YCbCrVector
{
    public JPEG(params double[] input) : base(input) { }

    public static implicit operator JPEG(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="JPEG"/> > <see cref="YCbCr"/></summary>
    public override YCbCr ToYCbCr(WorkingProfile profile)
    {
        double y = Value[0], cb = Value[1], cr = Value[2];
        return new(y + 1.402 * (cr - 128), y - 0.34414 * (cb - 128) - 0.71414 * (cr - 128), y + 1.772 * (cb - 128));
    }

    /// <summary><see cref="YCbCr"/> > <see cref="JPEG"/></summary>
    public override void FromYCbCr(YCbCr input, WorkingProfile profile)
    {
        double r = input[0], g = input[1], b = input[2];
        Value = new(0.299 * r + 0.587 * g + 0.114 * b, 128 - 0.168736 * r - 0.331264 * g + 0.5 * b, 128 + 0.5 * r - 0.418688 * g - 0.081312 * b);
    }
}

#endregion

#region (🗸) [99.886%] (YUV)

/// <summary>
/// <para><b>Luminance (Y), U, V</b></para>
/// <i>Alias</i>
/// <list type="bullet">
/// <item>EBU</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/yuv.js</remarks>
[Component(0, 1, '%', "Y", "Luminance")]
[Component(-0.5, 0.5, ' ', "U")]
[Component(-0.5, 0.5, ' ', "V")]
[Serializable]
public sealed class YUV : ColorVector3
{
    public YUV(params double[] input) : base(input) { }

    public static implicit operator YUV(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="YUV"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double
            y = Value[0],
            u = Value[1],
            v = Value[2],
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

        return new(r, g, b);
    }

    /// <summary><see cref="RGB"/> > <see cref="YUV"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        double
            r = input[0],
            g = input[1],
            b = input[2];

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

#endregion