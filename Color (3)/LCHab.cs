using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Luminance (L), Chroma (C), Hue (H)</b>
/// 
/// <para>A cylindrical form of <see cref="Lab"/> that is designed to accord with the human perception of color.</para>
/// 
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Lab"/> > <see cref="LCHab"/></para>
/// 
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(100, '%', "L", "Luminance"), Component(100, '%', "C", "Chroma"), Component(360, '°', "H", "Hue")]
[Serializable]
public class LCHab : Lab, ILCh
{
    public LCHab(params double[] input) : base(input) { }

    public static implicit operator LCHab(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="LCHab"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var result = this.FromLCh();
        return new Lab(result).ToLrgb(profile);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="LCHab"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var result = new Lab();
        result.FromLrgb(input, profile);
        Value = result.ToLCh();
    }
}