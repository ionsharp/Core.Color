using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>U, C (V), S (W)</b></para>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="UCS"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>CIEUCS</item>
/// <item>Uniform Color Space</item>
/// <item>Uniform Color Scale</item>
/// <item>Uniform Chromaticity Scale</item>
/// <item>Uniform Chromaticity Space</item>
/// </list>
/// 
/// <i>Author</i>
/// <list type="bullet">
/// <item><see cref="CIE"/> (1960)</item>
/// </list>
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