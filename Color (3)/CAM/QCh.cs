namespace Imagin.Core.Colors;

/// <summary>
/// <b>QCh</b> (<see cref="CAM02"/>)
/// <para><i>Brightness (Q), Chroma (C), Hue (h)</i></para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="QCh"/></para>
/// </summary>
public class QCh : CAM02
{
    public QCh() : base() { }

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="QCh"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
    {
        var result = From<QCh>(input, new());
        Value = new(result.Q, result.C, result.h);
    }
}