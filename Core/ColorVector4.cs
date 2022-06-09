using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

public interface IColorVector4 { }

/// <remarks>A <see cref="ColorModel"/> with four (4) components.</remarks>
/// <inheritdoc/>
[Serializable]
public abstract class ColorVector4 : ColorModel, IColorVector4
{
    public Vector4 Value4
    {
        get => new(Value[0], Value[1], Value[2], Value[3]);
        set => Value = new(value.W, value.X, value.Y, value.Z);
    }

    /// <summary>The first component.</summary>
    public double W => Value[0];

    /// <summary>The second component.</summary>
    public double X => Value[1];

    /// <summary>The third component.</summary>
    public double Y => Value[2];

    /// <summary>The fourth component.</summary>
    public double Z => Value[3];

    protected ColorVector4(double w, double x, double y, double z) : base() => Value = new Vector(w, x, y, z);

    public static implicit operator Vector4(ColorVector4 input) => new(input.W, input.X, input.Y, input.Z);
}