using System;
using static Imagin.Core.Numerics.Angle;
using static Imagin.Core.Numerics.M;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Hue (H), Saturation (S), Mixture (M)</b>
/// <para>A model that defines color as having hue (H), saturation (S), and mixture (M).</para>
/// <para>🞩 <i>Only one hue seemingly displayed.</i></para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="HSM"/></para>
/// </summary>
/// <remarks>https://seer.ufrgs.br/rita/article/viewFile/rita_v16_n2_p141/7428</remarks>
[Component(360, '°', "H", "Hue"), Component(100, '%', "S", "Saturation"), Component(255, ' ', "M", "Mixture")]
[Category(Class.HS), Serializable]
[Description("A model that defines color as having hue (H), saturation (S), and mixture (M).")]
public class HSM : ColorModel3
{
    public HSM() : base() { }

    /// <summary>(🗸) <see cref="HSM"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
        var max = Colour.Maximum<HSM>();

        double h = Cos(GetRadian(X)), s = Y / max[1] /*(100)*/, m = Z / max[2] /*(255)*/;
        double r, g, b;

        double i = h * s;
        double j = i * Sqrt(41);

        double x = 4 / 861;
        double y = 861 * Pow2(s);
        double z = 1 - Pow2(h);

        r = (3 / 41 * i) + m - (x * Sqrt(y * z));
        g = (j + (23 * m) - (19 * r)) / 4;
        b = ((11 * r) - (9 * m) - j) / 2;

        return Colour.New<Lrgb>(r, g, b);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="HSM"/></summary>
    public override void From(Lrgb input, WorkingProfile profile)
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

        var max = Colour.Maximum<HSM>();
        Value = new(h * max[0], s * max[1], m * max[2]);
    }
}