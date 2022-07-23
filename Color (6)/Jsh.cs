using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Jsh</b> (<see cref="CAM02"/>)
/// <para><i>Lightness (J), Saturation (s), Hue (h)</i></para>
/// <para>A model specified by 'CAM02' that defines color as having lightness (J), saturation (S), and hue (h).</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Jsh"/></para>
/// </summary>
[Component(100, '%', "J", "Lightness"), Component(100, '%', "s", "Saturation"), Component(360, '°', "h", "Hue")]
[Category(Class.CAM), Serializable]
[Description("A model specified by 'CAM02' that defines color as having lightness (J), saturation (S), and hue (h).")]
public class Jsh : CAM02
{
    public override double J
    {
        get => X; set => X = value;
    }

    public override double s
    {
        get => Y; set => Y = value;
    }

    public override double Q { get; set; }

    public override double C { get; set; }

    public override double M { get; set; }

    public Jsh() : base() { }

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="Jsh"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
    {
        var result = From<Jsh>(input, profile);
        Value = new(result.J, result.s, result.h);
    }
}