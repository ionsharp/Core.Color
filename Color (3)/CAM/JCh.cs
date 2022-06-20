namespace Imagin.Core.Colors;

/// <summary>
/// <b>JCh</b> (<see cref="CAM02"/>)
/// <para><i>Lightness (J), Chroma (C), Hue (h)</i></para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="JCh"/></para>
/// </summary>
public class JCh : CAM02
{
    public JCh() : base() { }

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="JCh"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
    {
        var result = From<JCh>(input, new());
        Value = new(result.J, result.C, result.h);
    }
}