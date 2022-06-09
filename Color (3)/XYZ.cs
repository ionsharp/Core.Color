using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>X, Y, Z</b></para>
/// 
/// A color space based on <see cref="LMS"/> where the Z value corresponds to the short (S) cone response of the human eye, the Y value is a mix of long (L) and medium (M) cone responses, and the X value is a mix of all three.
/// 
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
[Component(1, "X"), Component(1, "Y"), Component(1, "Z")]
[Serializable]
public class XYZ : ColorVector3
{
    public XYZ(params double[] input) : base(input) { }

    public static implicit operator XYZ(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="XYZ"/> (0) > <see cref="LMS"/> (0) > <see cref="LMS"/> (1) > <see cref="XYZ"/> (1)</summary>
    public override void Adapt(WorkingProfile source, WorkingProfile target) => Value = Adapt(this, source, target);

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var p = profile.Primary; var w = profile.White;
        return new(WorkingProfile.GetRxGyBz(p, w).Invert().Value.Multiply(Value));
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="XYZ"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var s = input.Value;
        var p = profile.Primary; var w = profile.White;
        var m = WorkingProfile.GetRxGyBz(p, w);
        Value = m.Multiply(s);
    }
}