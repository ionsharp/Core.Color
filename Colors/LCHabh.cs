using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Luminance (L), Chroma (Ca), Hue (Hb)</b>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Labh"/> > <see cref="LCHabh"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Hunter LCHab</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 100, '%', "L", "Luminance")]
[Component(0, 100, '%', "Ca", "Chroma")]
[Component(0, 360, '°', "Hb", "Hue")]
[Serializable]
public sealed class LCHabh : LabhVector
{
    public LCHabh(params double[] input) : base(input) { }

    public static implicit operator LCHabh(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="LCHabh"/> > <see cref="Labh"/></summary>
    public override Labh ToLABh(WorkingProfile profile)
        => new(new LCH(this).To());

    /// <summary><see cref="Labh"/> > <see cref="LCHabh"/></summary>
    public override void FromLABh(Labh input, WorkingProfile profile)
    {
        var lch = new LCH();
        lch.From(input);

        Value = lch;
    }
}