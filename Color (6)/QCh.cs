using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>QCh</b> (<see cref="CAM02"/>)
/// <para><i>Brightness (Q), Chroma (C), Hue (h)</i></para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="QCh"/></para>
/// </summary>
[Component(100, '%', "Q", "Brightness"), Component(100, '%', "C", "Chroma"), Component(360, '°', "h", "Hue")]
[Serializable]
public class QCh : CAM02
{
    public override double Q
    {
        get => X; set => X = value;
    }

    public override double C
    {
        get => Y; set => Y = value;
    }

    public override double J { get; set; }

    public override double M { get; set; }

    public override double s { get; set; }

    public QCh() : base() { }

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="QCh"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
    {
        var result = From<QCh>(input, profile);
        Value = new(result.Q, result.C, result.h);
    }
}