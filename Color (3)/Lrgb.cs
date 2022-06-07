using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

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
public class Lrgb : ColorVector3
{
    public Lrgb(params double[] input) : base(input) { }

    public static implicit operator Lrgb(Vector3 input) => new(input.X, input.Y, input.Z);

    public override void FromLrgb(Lrgb input, WorkingProfile profile) => Value = input.Value;

    public override Lrgb ToLrgb(WorkingProfile profile) => new(Value);
}