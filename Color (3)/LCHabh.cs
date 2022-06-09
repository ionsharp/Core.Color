using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Luminance (L), Chroma (C), Hue (H)</b>
/// 
/// <para>A cylindrical form of <see cref="Labh"/> that is designed to accord with the human perception of color.</para>
/// 
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Labh"/> > <see cref="LCHabh"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Hunter LCHab</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(100, '%', "L", "Luminance"), Component(100, '%', "C", "Chroma"), Component(360, '°', "H", "Hue")]
[Serializable]
public class LCHabh : Labh, ILCh
{
    public LCHabh(params double[] input) : base(input) { }

    public static implicit operator LCHabh(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="LCHabh"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var result = this.FromLCh();
        return new Labh(result).ToLrgb(profile);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="LCHabh"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var result = new Labh();
        result.FromLrgb(input, profile);
        Value = result.ToLCh();
    }
}