using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Luminance (L), Chroma (Ca), Hue (Hb)</b>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Lab"/> > <see cref="LCHab"/></para>
/// 
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 100, '%', "L", "Luminance")]
[Component(0, 100, '%', "Ca", "Chroma")]
[Component(0, 360, '°', "Hb", "Hue")]
[Serializable]
public sealed class LCHab : LabVector
{
    public LCHab(params double[] input) : base(input) { }

    public static implicit operator LCHab(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="LCHab"/> > <see cref="Lab"/></summary>
    public override Lab ToLAB(WorkingProfile profile)
        => new(new LCH(this).To());

    /// <summary><see cref="Lab"/> > <see cref="LCHab"/></summary>
    public override void FromLAB(Lab input, WorkingProfile profile)
    {
        var lch = new LCH();
        lch.From(input);

        Value = lch;
    }
}