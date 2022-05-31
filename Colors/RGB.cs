using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>Red (R), Green (G), Blue (B)</b></para>
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
public sealed class RGB : ColorVector3
{
    public RGB(params double[] input) : base(input) { }

    public static implicit operator RGB(Vector3 input) => new(input.X, input.Y, input.Z);

    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var oldValue = Value;
        var newValue = oldValue.Transform(i => profile.Companding.CompandInverse(i));
        return new(newValue);
    }

    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var oldValue = input.Value;
        var newValue = oldValue.Transform(i => profile.Companding.Compand(i));
        Value = new(newValue);
    }
}

/// <summary>
/// <para>(🗸) <b>Red (R), Green (G), Blue (B)</b></para>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>LRGB</item>
/// <item>Linear RGB</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Hidden, Serializable]
public sealed class Lrgb : ColorVector3
{
    public Lrgb(params double[] input) : base(input) { }

    public static implicit operator Lrgb(Vector3 input) => new(input.X, input.Y, input.Z);

    public override void FromLrgb(Lrgb input, WorkingProfile profile) => Value = input.Value;

    public override Lrgb ToLrgb(WorkingProfile profile) => new(Value);
}