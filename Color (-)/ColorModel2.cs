using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>A <see cref="ColorModel"/> with two (2) components.</summary>
[Serializable]
public abstract class ColorModel2 : ColorModel<Vector2>, IColorModel2
{
    public override int Length => 2;

    /// <summary>The first component.</summary>
    public double X
    {
        get => Value.X;
        set => Value = new(value, Value.Y);
    }

    /// <summary>The second component.</summary>
    public double Y
    {
        get => Value.Y;
        set => Value = new(Value.X, value);
    }

    public virtual Vector2 XY => new(X, Y);

    public ColorModel2() : base() { }

    public static implicit operator Vector2(ColorModel2 input) => input.XY;

    public Vector2 GetMaximum() { var n = Colour.Maximum(GetType()); return new(n[0], n[1]); }

    public Vector2 GetMinimum() { var n = Colour.Minimum(GetType()); return new(n[0], n[1]); }

    public Vector2 Denormalize() => M.Denormalize(XY, GetMinimum(), GetMaximum());

    public Vector2 Normalize() => M.Normalize(XY, GetMinimum(), GetMaximum());
}