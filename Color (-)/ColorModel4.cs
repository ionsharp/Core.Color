using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <remarks>A <see cref="ColorModel"/> with four (4) components.</remarks>
[Serializable]
public abstract class ColorModel4 : ColorModel<Vector4>, IColorModel4
{
    public override int Length => 4;

    /// <summary>The first component.</summary>
    public double X
    {
        get => Value.X;
        set => Value = new(value, Value.Y, Value.Z, Value.W);
    }

    /// <summary>The second component.</summary>
    public double Y
    {
        get => Value.Y;
        set => Value = new(Value.X, value, Value.Z, Value.W);
    }

    /// <summary>The third component.</summary>
    public double Z
    {
        get => Value.Z;
        set => Value = new(Value.X, Value.Y, value, Value.W);
    }

    /// <summary>The fourth component.</summary>
    public double W
    {
        get => Value.W;
        set => Value = new(Value.X, Value.Y, Value.Z, value);
    }

    public Vector2 XY => new(X, Y);

    public Vector3 XYZ => new(X, Y, Z);

    public Vector4 XYZW => new(X, Y, Z, W);

    protected ColorModel4() : base() { }

    public static implicit operator Vector4(ColorModel4 input) => input.XYZW;

    public Vector4 GetMaximum() { var i = Colour.Maximum(GetType()); return new(i[0], i[1], i[2], i[3]); }

    public Vector4 GetMinimum() { var i = Colour.Minimum(GetType()); return new(i[0], i[1], i[2], i[3]); }

    public Vector4 Denormalize() => M.Denormalize(XYZW, GetMinimum(), GetMaximum());

    public Vector4 Normalize() => M.Normalize(XYZW, GetMinimum(), GetMaximum());
}