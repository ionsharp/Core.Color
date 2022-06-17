using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>A <see cref="ColorModel"/> with two (2) components.</summary>
[Serializable]
public abstract class ColorModel2 : ColorModel, IColorModel2
{
    public virtual Vector2 XY
    {
        get => new(X, Y);
        set => Value = new(value.X, value.Y);
    }

    /// <summary>The first component.</summary>
    public virtual double X => this[0];

    /// <summary>The second component.</summary>
    public virtual double Y => this[1];

    public ColorModel2() : base() { }

    public static implicit operator Vector2(ColorModel2 input) => input.XY;

    public Vector2 GetMaximum() { var n = Colour.Maximum(GetType()); return new(n[0], n[1]); }

    public Vector2 GetMinimum() { var n = Colour.Minimum(GetType()); return new(n[0], n[1]); }

    public Vector2 Denormalize() => M.Denormalize(XY, GetMinimum(), GetMaximum());

    public Vector2 Normalize() => M.Normalize(XY, GetMinimum(), GetMaximum());
}