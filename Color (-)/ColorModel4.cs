using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

public interface IColorModel4 { }

/// <remarks>A <see cref="ColorModel"/> with four (4) components.</remarks>
[Serializable]
public abstract class ColorModel4 : ColorModel, IColorModel4
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

    public Vector4 XYZW
    {
        get => new(X, Y, Z, W);
        set => Value = new(value.X, value.Y, value.Z, value.W);
    }
    
    /// <summary>The first component.</summary>
    public double X => this[0];

    /// <summary>The second component.</summary>
    public double Y => this[1];

    /// <summary>The third component.</summary>
    public double Z => this[2];

    /// <summary>The fourth component.</summary>
    public double W => this[3];

    protected ColorModel4() : base() { }

    public static implicit operator Vector4(ColorModel4 input) => input.XYZW;

    public Vector4 GetMaximum() { var i = Colour.Maximum(GetType()); return new(i[0], i[1], i[2], i[3]); }

    public Vector4 GetMinimum() { var i = Colour.Minimum(GetType()); return new(i[0], i[1], i[2], i[3]); }

    public Vector4 Denormalize() => M.Denormalize(XYZW, GetMinimum(), GetMaximum());

    public Vector4 Normalize() => M.Normalize(XYZW, GetMinimum(), GetMaximum());
}