using Imagin.Core.Numerics;
using System;

using static Imagin.Core.Numerics.M;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// (🞩) <b>Hue (H), Saturation (S), Mixture (M)</b>
/// <para>🞩 <i>Only one hue seemingly displayed.</i></para>
/// <para>≡ 62.752%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="HSM"/></para>
/// </summary>
/// <remarks>https://seer.ufrgs.br/rita/article/viewFile/rita_v16_n2_p141/7428</remarks>
[Component(0, 360, '°', "H", "Hue"), Component(0, 100, '%', "S", "Saturation"), Component(0, 255, ' ', "M", "Mixture")]
[Serializable, Unfinished]
public class HSM : ColorVector3, IHs
{
    public HSM(params double[] input) : base(input) { }

    public static implicit operator HSM(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🞩) <see cref="HSM"/> > <see cref="Lrgb"/></summary>
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

    /// <summary>(🞩) <see cref="Lrgb"/> > <see cref="HSM"/></summary>
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