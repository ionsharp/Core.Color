using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>JMh</b> (<see cref="CAM02"/>)
/// <para><i>Lightness (J), Colorfulness (M), Hue (h)</i></para>
/// <para>A model specified by 'CAM02' that defines color as having lightness (J), colorfulness (M), and hue (h).</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="JMh"/></para>
/// </summary>
[Component(100, '%', "J", "Lightness"), Component(100, '%', "M", "Colorfulness"), Component(360, '°', "h", "Hue")]
[Category(Class.CAM), Serializable]
[Description("A model specified by 'CAM02' that defines color as having lightness (J), colorfulness (M), and hue (h).")]
public class JMh : CAM02
{
    public override double J
    {
        get => X; set => X = value;
    }

    public override double M
    {
        get => Y; set => Y = value;
    }

    public override double Q { get; set; }

    public override double C { get; set; }

    public override double s { get; set; }

    public JMh() : base() { }

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="JMh"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
    {
        var result = From<JMh>(input, profile);
        Value = new(result.J, result.M, result.h);
    }
}