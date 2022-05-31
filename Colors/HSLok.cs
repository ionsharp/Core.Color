using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🞩) <b>Hue (H), Saturation (S), Lightness (L)</b>
/// <para>≡ 0%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="OKLab"/> > <see cref="HSLok"/></para>
/// </summary>
/// <remarks>https://colour.readthedocs.io/en/develop/_modules/colour/models/oklab.html</remarks>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "S", "Saturation")]
[Component(0, 100, '%', "L", "Lightness")]
[Serializable, Unfinished]
public sealed class HSLok : OKLabVector
{
    public HSLok(params double[] input) : base(input) { }

    public static implicit operator HSLok(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HSLok"/> > <see cref="OKLab"/></summary>
    public override OKLab ToOKLab(WorkingProfile profile) => new();

    /// <summary><see cref="OKLab"/> > <see cref="HSLok"/></summary>
    public override void FromOKLab(OKLab input, WorkingProfile profile) { }
}