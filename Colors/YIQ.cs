using Imagin.Core.Numerics;
using System;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// (🞩) <b>Y, I, Q</b>
/// <para>≡ 99.765%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YIQ"/></para>
/// </summary>
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