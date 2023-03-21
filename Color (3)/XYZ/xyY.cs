using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System;
using System.Diagnostics.CodeAnalysis;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Chroma (x), Chroma (y), Luminance (Y)</b>
/// <para>A model that defines color as having chroma (x), chroma (y), and luminance (Y).</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="xyY"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/xyy.js</remarks>
[Component(1, "x", "Chroma"), Component(1, "y", "Chroma"), Component(1, '%', "Y", "Luminance")]
[Category(Class.XYZ), Serializable]
[Description("A model that defines color as having chroma (x), chroma (y), and luminance (Y).")]
[SuppressMessage("Style", "IDE1006:Naming Styles")]
public class xyY : ColorModel3<XYZ>
{
    public xyY() : base() { }

    ///

    public static explicit operator xyY(xy input) => Colour.New<xyY>(input.X, input.Y, 1);

    public static explicit operator xyY(XYZ input)
    {
        var result = new xyY();
        result.From(input, WorkingProfile.Default);
        return result;
    }

    ///

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="xyY"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
        => Value = input.Value.Sum() is double sum && sum == 0 ? new Vector3(0, 0, input.Y) : new(input.X / sum, input.Y / sum, input.Y);

    /// <summary>(🗸) <see cref="xyY"/> > <see cref="XYZ"/></summary>
    public override void To(out XYZ result, WorkingProfile profile)
        => result = Y == 0 ? Colour.New<XYZ>(0) : Colour.New<XYZ>(X * Z / Y, Z, (1 - X - Y) * Z / Y);
}