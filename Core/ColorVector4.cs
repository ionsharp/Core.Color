using Imagin.Core.Numerics;
using System;

using static System.Math;

namespace Imagin.Core.Colors;

#region ColorVector4

public interface IColorVector4 { }

/// <remarks>A <see cref="ColorVector"/> with four (4) components.</remarks>
/// <inheritdoc/>
[Serializable]
public abstract class ColorVector4 : ColorVector, IColorVector4
{
    /// <summary>
    /// The first component.
    /// </summary>
    public double W => Value[0];

    /// <summary>
    /// The second component.
    /// </summary>
    public double X => Value[1];

    /// <summary>
    /// The third component.
    /// </summary>
    public double Y => Value[2];

    /// <summary>
    /// The fourth component.
    /// </summary>
    public double Z => Value[3];

    protected ColorVector4(double w, double x, double y, double z) : base() => Value = new Vector(w, x, y, z);

    public static implicit operator Vector4(ColorVector4 input) => new(input.W, input.X, input.Y, input.Z);
}

#endregion

#region (🞩) [?     %] (CMYK)

/// <inheritdoc/>
[Component(0, 100, '%', "C", "Cyan"), Component(0, 100, '%', "M", "Magenta"), Component(0, 100, '%', "Y", "Yellow"), Component(0, 100, '%', "K", "Black")]
[Serializable]
public sealed class CMYK : ColorVector4
{
    public CMYK(double w, double x, double y, double z) : base(w, x, y, z) { }

    /// <remarks>https://github.com/colorjs/color-space/blob/master/cmyk.js</remarks>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var r = (1.0 - Value[0]) * (1.0 - Value[3]);
        var g = (1.0 - Value[1]) * (1.0 - Value[3]);
        var b = (1.0 - Value[2]) * (1.0 - Value[3]);
        return new Lrgb(r, g, b);
    }

    /// <remarks>https://github.com/colorjs/color-space/blob/master/cmyk.js</remarks>
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

#endregion