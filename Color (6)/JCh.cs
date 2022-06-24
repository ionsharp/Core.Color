using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>JCh</b> (<see cref="CAM02"/>)
/// <para><i>Lightness (J), Chroma (C), Hue (h)</i></para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="JCh"/></para>
/// </summary>
[Component(100, '%', "J", "Lightness"), Component(100, '%', "C", "Chroma"), Component(360, '°', "h", "Hue")]
[Serializable]
public class JCh : CAM02
{
    public override double J
    {
        get => X; set => X = value;
    }

    public override double C
    {
        get => Y; set => Y = value;
    }

    public override double Q { get; set; }

    public override double M { get; set; }

    public override double s { get; set; }

    public JCh() : base() { }

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="JCh"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
    {
        var result = From<JCh>(input, profile);
        Value = new(result.J, result.C, result.h);
    }
}