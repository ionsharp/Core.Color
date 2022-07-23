using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Cyan (C), Magenta (M), Yellow (Y), Black (K)</b>
/// <para>A subtractive model where the secondary colors are added with black.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="CMYK"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/cmyk.js</remarks>
[Component(100, '%', "C", "Cyan"), Component(100, '%', "M", "Magenta"), Component(100, '%', "Y", "Yellow"), Component(100, '%', "K", "Black")]
[Category(Class.CMY), Serializable]
[Description("A subtractive model where the secondary colors are added with black.")]
public sealed class CMYK : ColorModel4
{
    public CMYK() : base() { }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="CMYK"/></summary>
    public override void From(Lrgb input, WorkingProfile profile)
    {
        var k0 = 1.0 - Max(input.X, Max(input.Y, input.Z));
        var k1 = 1.0 - k0;

        var c = (1.0 - input.X - k0) / k1;
        var m = (1.0 - input.Y - k0) / k1;
        var y = (1.0 - input.Z - k0) / k1;
        Value = new Vector4(c.NaN(0), m.NaN(0), y.NaN(0), k0) * 100;
    }

    /// <summary>(🗸) <see cref="CMYK"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
        var result = Value / 100;
        var r = (1.0 - result[0]) * (1.0 - result[3]);
        var g = (1.0 - result[1]) * (1.0 - result[3]);
        var b = (1.0 - result[2]) * (1.0 - result[3]);
        return Colour.New<Lrgb>(r, g, b);
    }
}