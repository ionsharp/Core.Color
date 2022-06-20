using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>A <see cref="ColorModel"/> with three (3) components.</summary>
[Serializable]
public abstract class ColorModel3 : ColorModel<Vector3>, IColorModel3
{
    public override int Length => 3;

    /// <summary>The first component.</summary>
    public double X
    {
        get => Value.X;
        set => Value = new(value, Value.Y, Value.Z);
    }

    /// <summary>The second component.</summary>
    public double Y
    {
        get => Value.Y;
        set => Value = new(Value.X, value, Value.Z);
    }

    /// <summary>The third component.</summary>
    public double Z
    {
        get => Value.Z;
        set => Value = new(Value.X, Value.Y, value);
    }

    public Vector2 XY => new(X, Y);

    public Vector3 XYZ => new(X, Y, Z);

    public ColorModel3() : base() { }

    public static implicit operator Vector3(ColorModel3 input) => input.XYZ;

    public void From(Vector3 input) => Value = input;

    public Vector3 GetMaximum() { var n = Colour.Maximum(GetType()); return new(n[0], n[1], n[2]); }

    public Vector3 GetMinimum() { var n = Colour.Minimum(GetType()); return new(n[0], n[1], n[2]); }

    public Vector3 Denormalize() => M.Denormalize(XYZ, GetMinimum(), GetMaximum());

    public Vector3 Normalize() => M.Normalize(XYZ, GetMinimum(), GetMaximum());
}