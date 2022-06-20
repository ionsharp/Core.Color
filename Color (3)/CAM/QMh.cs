namespace Imagin.Core.Colors;

/// <summary>
/// <b>QMh</b> (<see cref="CAM02"/>)
/// <para><i>Brightness (Q), Colorfulness (M), Hue (h)</i></para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="QMh"/></para>
/// </summary>
public class QMh : CAM02
{
    public QMh() : base() { }

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="QMh"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
    {
        var result = From<QMh>(input, new());
        Value = new(result.Q, result.M, result.h);
    }
}