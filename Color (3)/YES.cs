using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Luminance (Y), E-factor (E), S-factor (S)</b>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YES"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/yes.js</remarks>
[Component(1, '%', "Y", "Luminance"), Component(1, '%', "E", "E-factor"), Component(1, '%', "S", "S-factor")]
[Serializable]
public class YES : ColorVector3
{
    public YES(params double[] input) : base(input) { }

    public static implicit operator YES(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="YES"/> > <see cref="Lrgb"/></summary>
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

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="YES"/></summary>
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