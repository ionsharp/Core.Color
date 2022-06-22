using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Qsh</b> (<see cref="CAM02"/>)
/// <para><i>Brightness (Q), Saturation (s), Hue (h)</i></para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Qsh"/></para>
/// </summary>
[Component(100, '%', "Q", "Brightness"), Component(100, '%', "s", "Saturation"), Component(360, '°', "h", "Hue")]
[Serializable]
public class Qsh : CAM02
{
    public override double Q
    {
        get => X; set => X = value;
    }

    public override double s
    {
        get => Y; set => Y = value;
    }

    public override double J { get; set; }

    public override double C { get; set; }

    public override double M { get; set; }

    public Qsh() : base() { }

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="Qsh"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
    {
        var result = From<Qsh>(input, new(), profile);
        Value = new(result.Q, result.s, result.h);
    }
}