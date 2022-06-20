namespace Imagin.Core.Colors;

/// <summary>
/// <b>JMh</b> (<see cref="CAM02"/>)
/// <para><i>Lightness (J), Colorfulness (M), Hue (h)</i></para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="JMh"/></para>
/// </summary>
public class JMh : CAM02
{
    public JMh() : base() { }

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="JMh"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
    {
        var result = From<JMh>(input, new());
        Value = new(result.J, result.M, result.h);
    }
}