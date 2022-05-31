using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Long (L), Medium (M), Short (S)</b>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="LMS"/></para>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(0, 1, '%', "L", "Long")]
[Component(0, 1, '%', "M", "Medium")]
[Component(0, 1, '%', "S", "Short")]
[Serializable]
public sealed class LMS : XYZVector
{
    public LMS(params double[] input) : base(input) { }

    public static implicit operator LMS(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="LMS"/> > <see cref="XYZ"/></summary>
    public override XYZ ToXYZ(WorkingProfile profile)
    {
        var xyz = LMSTransformationMatrix.VonKriesHPEAdjusted.Invert().Value.Multiply(Value);
        return new(xyz[0], xyz[1], xyz[2]);
    }

    /// <summary><see cref="XYZ"/> > <see cref="LMS"/></summary>
    public override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        Value = new(LMSTransformationMatrix.VonKriesHPEAdjusted.Multiply(input.Value));
    }
}