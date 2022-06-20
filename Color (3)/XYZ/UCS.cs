using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para><b>U, C (V), S (W)</b></para>
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
[Component(100, '%', "U", "U"), Component(100, '%', "C", "V"), Component(100, '%', "S", "W")]
[Serializable]
public class UCS : ColorModel3<XYZ>
{
    public UCS() : base() { }

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="UCS"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
    {
        double x = input.X, y = input.Y, z = input.Z;
        Value = new(x * 2 / 3, y, 0.5 * (-x + 3 * y + z));
    }

    /// <summary>(🗸) <see cref="UCS"/> > <see cref="XYZ"/></summary>
    public override void To(out XYZ result, WorkingProfile profile)
    {
        double u = X, v = Y, w = Z;
        result = Colour.New<XYZ>(1.5 * u, v, 1.5 * u - 3 * v + 2 * w);
    }
}