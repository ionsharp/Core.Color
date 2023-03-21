using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>r, g, G</b>
/// <para>A chromacity model similar to 'xyY' where a color is directly converted from 'RGB' instead of 'XYZ'.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="rgG"/></para>
/// </summary>
/// <remarks>https://en.wikipedia.org/wiki/Rg_chromaticity</remarks>
[Component(1, "r"), Component(1, "g"), Component(1, '%', "G")]
[Category(Class.RGB), Serializable]
[Description("A chromacity model similar to 'xyY' where a color is directly converted from 'RGB' instead of 'XYZ'.")]
[SuppressMessage("Style", "IDE1006:Naming Styles")]
public class rgG : ColorModel3
{
    public rgG() : base() { }

    ///

    public static explicit operator rgG(rg input) => Colour.New<rgG>(input.X, input.Y, 1);

    ///

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="rgG"/></summary>
    public override void From(Lrgb input, WorkingProfile profile)
        => Value = input.Value.Sum() is double sum && sum == 0 ? new Vector3(0, 0, input.Y) : new(input.X / sum, input.Y / sum, input.Y);

    /// <summary>(🗸) <see cref="rgG"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
        => Y == 0 ? Colour.New<Lrgb>(0) : Colour.New<Lrgb>(X * Z / Y, Z, (1 - X - Y) * Z / Y);
}