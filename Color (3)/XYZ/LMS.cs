using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Long (L/ρ), Medium (M/γ), Short (S/β)</b>
/// <para>A model that defines color based on the response of the three types of cones of the human eye, named for their responsivity (sensitivity) peaks at long, medium, and short wavelengths.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="LMS"/></para>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(1, '%', "L", "Long"), Component(1, '%', "M", "Medium"), Component(1, '%', "S", "Short")]
[Category(Class.XYZ), Serializable]
[Description("A model that defines color based on the response of the three types of cones of the human eye, named for their responsivity (sensitivity) peaks at long, medium, and short wavelengths.")]
public class LMS : ColorModel3<XYZ>
{    
    public LMS() : base() { }

    ///

    /// <summary>(🗸) <see cref="LMS"/> (0) > <see cref="LMS"/> (1)</summary>
    public override void Adapt(WorkingProfile source, WorkingProfile target) => Value = Adapt(this, source, target);

    ///

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="LMS"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
        => Value = profile.Adaptation * input.Value;

    /// <summary>(🗸) <see cref="LMS"/> > <see cref="XYZ"/></summary>
    public override void To(out XYZ result, WorkingProfile profile)
    {
        var v = profile.Adaptation.Invert3By3().Multiply(Value);
        result = Colour.New<XYZ>(v[0], v[1], v[2]);
    }

    ///

    /// <summary>Gets the matrix used to convert between <see cref="LMS"/> and <see cref="XYZ"/>.</summary>
    /// <remarks>
    /// <para>Normalized to <see cref="Illuminant2.D65"/>.</para>
    /// <para>https://github.com/tompazourek/Colourful</para>
    /// </remarks>
    public static Matrix GetMatrix() => ChromaticAdaptationTransform.Default;
}