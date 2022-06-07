using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>Red (R), Green (G), Blue (B)</b></para>
/// 
/// <para>An additive color model in which the red, green, and blue primary colors are added together.</para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>CIERGB</item>
/// </list>
/// 
/// <i>Author</i>
/// <list type="bullet">
/// <item><see cref="CIE"/> (1931)</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 1, "R", "Red"), Component(0, 1, "G", "Green"), Component(0, 1, "B", "Blue")]
[Serializable]
public class RGB : ColorVector3
{
    public RGB(params double[] input) : base(input) { }

    public static implicit operator RGB(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="RGB"/> (0) > <see cref="XYZ"/> (0) > <see cref="LMS"/> (0) > <see cref="LMS"/> (1) > <see cref="XYZ"/> (1) > <see cref="RGB"/> (1)</summary>
    public override void Adapt(WorkingProfile source, WorkingProfile target) => Value = Adapt(this, source, target);

    /// <summary>(🗸) <see cref="RGB"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var oldValue = Value;
        var newValue = oldValue.Transform(i => profile.Compression.CompandInverse(i));
        return new(newValue);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="RGB"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var oldValue = input.Value;
        var newValue = oldValue.Transform(i => profile.Compression.Compand(i));
        Value = new(newValue);
    }
}