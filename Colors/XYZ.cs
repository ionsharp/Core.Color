using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>X, Y, Z</b></para>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>CIEXYZ</item>
/// </list>
/// 
/// <i>Author</i>
/// <list type="bullet">
/// <item><see cref="CIE"/> (1931)</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 1, "X"), Component(0, 1, "Y"), Component(0, 1, "Z")]
[Serializable]
public sealed class XYZ : ColorVector3
{
    public XYZ(params double[] input) : base(input) { }

    public static implicit operator XYZ(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="XYZ"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var p = profile.Chromacity; var w = profile.White;
        return new(WorkingProfile.GetRxGyBz(p, w).Invert().Value.Multiply(Value));
    }

    /// <summary><see cref="RGB"/> > <see cref="XYZ"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var s = input.Value;
        var p = profile.Chromacity; var w = profile.White;
        var m = WorkingProfile.GetRxGyBz(p, w);
        Value = m.Multiply(s);
    }
}