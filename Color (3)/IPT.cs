using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🞩) <b>Intensity (I), Cyan/red (P), Blue/yellow (T)</b></para>
/// 
/// <para>Similar to <see cref="YCwCm"/>, but with smoother transitions between hues. <see cref="P"/> stands for protanopia (or red-green colorblindness) and <see cref="T"/> stands for tritanopia (another form of colorblindness).</para>
/// 
/// <para>≡ 0%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="IPT"/></para>
/// 
/// <i>Author</i>
/// <list type="bullet">
/// <item>Ebner/Fairchild (1998)</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/tommyettinger/colorful-gdx</remarks>
[Component(1, '%', "I", "Intensity"), Component(1, '%', "P", "Cyan/red"), Component(1, '%', "T", "Blue/yellow")]
[Serializable, Unfinished]
public class IPT : ColorVector3
{
    public double Intensity => X;

    public double P => Y;

    public double T => Z;

    public IPT(params double[] input) : base(input) { }

    public static implicit operator IPT(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🞩) <see cref="IPT"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile) => new();

    /// <summary>(🞩) <see cref="Lrgb"/> > <see cref="IPT"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile) { }
}