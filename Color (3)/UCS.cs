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
[Component(0, 100, '%', "U", "U"), Component(0, 100, '%', "C", "V"), Component(0, 100, '%', "S", "W")]
[Serializable]
public class UCS : XYZ
{
    public UCS(params double[] input) : base(input) { }

    public static implicit operator UCS(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="UCS"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double u = this[0], v = this[1], w = this[2];
        return new XYZ(1.5 * u, v, 1.5 * u - 3 * v + 2 * w).ToLrgb(profile);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="UCS"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var xyz = new XYZ();
        xyz.FromLrgb(input, profile);

        double x = xyz[0], y = xyz[1], z = xyz[2];
        Value = new(x * 2 / 3, y, 0.5 * (-x + 3 * y + z));
    }
}