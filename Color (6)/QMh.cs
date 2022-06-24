using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>QMh</b> (<see cref="CAM02"/>)
/// <para><i>Brightness (Q), Colorfulness (M), Hue (h)</i></para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="QMh"/></para>
/// </summary>
[Component(100, '%', "Q", "Brightness"), Component(100, '%', "M", "Colorfulness"), Component(360, '°', "h", "Hue")]
[Serializable]
public class QMh : CAM02
{
    public override double Q
    {
        get => X; set => X = value;
    }

    public override double M
    {
        get => Y; set => Y = value;
    }

    public override double J { get; set; }

    public override double C { get; set; }

    public override double s { get; set; }

    public QMh() : base() { }

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="QMh"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
    {
        var result = From<QMh>(input, profile);
        Value = new(result.Q, result.M, result.h);
    }
}