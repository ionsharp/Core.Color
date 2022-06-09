using Imagin.Core.Numerics;
using System;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>Luminance (Y), U, V</b></para>
/// <para>≡ 99.886%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YUV"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>EBU</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/yuv.js</remarks>
[Component(1, '%', "Y", "Luminance"), Component(-0.5, 0.5, ' ', "U"), Component(-0.5, 0.5, ' ', "V")]
[Serializable]
public class YUV : ColorVector3
{
    public YUV(params double[] input) : base(input) { }

    public static implicit operator YUV(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="YUV"/> > <see cref="Lrgb"/></summary>
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

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="YUV"/></summary>
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