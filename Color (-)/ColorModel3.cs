using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>A <see cref="ColorModel"/> with three (3) components.</summary>
[Serializable]
public abstract class ColorModel3 : ColorModel, IColorModel3
{
    public Vector2 XY
    {
        get => new(X, Y);
        set => Value = new(value.X, value.Y);
    }

    public Vector3 XYZ
    {
        get => new(X, Y, Z);
        set => Value = new(value.X, value.Y, value.Z);
    }

    /// <summary>The first component.</summary>
    public double X => this[0];

    /// <summary>The second component.</summary>
    public double Y => this[1];

    /// <summary>The third component.</summary>
    public double Z => this[2];

    public ColorModel3() : base() { }

    public static implicit operator Vector3(ColorModel3 input) => input.XYZ;

    public void From(Vector3 input) => Value = input;

    public Vector3 GetMaximum() { var n = Colour.Maximum(GetType()); return new(n[0], n[1], n[2]); }

    public Vector3 GetMinimum() { var n = Colour.Minimum(GetType()); return new(n[0], n[1], n[2]); }

    public Vector3 Denormalize() => M.Denormalize(XYZ, GetMinimum(), GetMaximum());

    public Vector3 Normalize() => M.Normalize(XYZ, GetMinimum(), GetMaximum());
}