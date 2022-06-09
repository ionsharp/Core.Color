using System;

using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// (🞩) <b>Cyan (C), Magenta (M), Yellow (Y), Black (K)</b>
/// 
/// <para>A subtractive color model based on <see cref="CMY"/> that is used in color printing.</para>
/// 
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="CMYK"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/cmyk.js</remarks>
[Component(100, '%', "C", "Cyan"), Component(100, '%', "M", "Magenta"), Component(100, '%', "Y", "Yellow"), Component(100, '%', "K", "Black")]
[Serializable]
public sealed class CMYK : ColorVector4
{
    public CMYK(double w, double x, double y, double z) : base(w, x, y, z) { }

    /// <summary>(🞩) <see cref="CMYK"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var r = (1.0 - Value[0]) * (1.0 - Value[3]);
        var g = (1.0 - Value[1]) * (1.0 - Value[3]);
        var b = (1.0 - Value[2]) * (1.0 - Value[3]);
        return new Lrgb(r, g, b);
    }

    /// <summary>(🞩) <see cref="Lrgb"/> > <see cref="CMYK"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var k0 = 1.0 - Max(input[0], Max(input[1], input[2]));
        var k1 = 1.0 - k0;

        var c = (1.0 - input[0] - k0) / k1;
        var m = (1.0 - input[1] - k0) / k1;
        var y = (1.0 - input[2] - k0) / k1;

        c = double.IsNaN(c) ? 0 : c;
        m = double.IsNaN(m) ? 0 : m;
        y = double.IsNaN(y) ? 0 : y;

        Value = new(c, m, y, k0);
    }
}