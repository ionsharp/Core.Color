using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

public interface IColorVector3 { }

/// <summary>A <see cref="ColorModel"/> with three (3) components.</summary>
/// <inheritdoc/>
[Serializable]
public abstract class ColorVector3 : ColorModel, IColorVector3
{
    public Vector3 Value3
    {
        get => new(Value[0], Value[1], Value[2]);
        set => Value = new(value.X, value.Y, value.Z);
    }

    /// <summary>The first component.</summary>
    public double X => Value[0];

    /// <summary>The second component.</summary>
    public double Y => Value[1];

    /// <summary>The third component.</summary>
    public double Z => Value[2];

    public ColorVector3(params double[] input) : base() => Value = new(input);

    public static implicit operator Vector3(ColorVector3 input) => new(input.X, input.Y, input.Z);
}