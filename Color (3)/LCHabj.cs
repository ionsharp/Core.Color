using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>Lightness (L), Chroma (C), Hue (H)</b></para>
/// 
/// <para>A cylindrical form of <see cref="Labj"/> that is designed to accord with the human perception of color.</para>
/// 
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Labj"/> > <see cref="LCHabj"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>JzCzhz</item>
/// <item>JzCzHz</item>
/// </list>
/// </summary>
/// <remarks>https://observablehq.com/@jrus/jzazbz</remarks>
[Component(1, '%', "L", "Luminance"), Component(1, '%', "C", "Chroma"), Component(360, '°', "H", "Hue")]
[Serializable]
public class LCHabj : Labj, ILCh
{
    public LCHabj(params double[] input) : base(input) { }

    public static implicit operator LCHabj(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="LCHabj"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var result = this.FromLCh();
        return new Labj(result).ToLrgb(profile);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="LCHabj"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var result = new Labj();
        result.FromLrgb(input, profile);
        Value = result.ToLCh();
    }
}