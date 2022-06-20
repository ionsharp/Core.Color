namespace Imagin.Core.Colors;

/// <summary>
/// <b>Qsh</b> (<see cref="CAM02"/>)
/// <para><i>Brightness (Q), Saturation (s), Hue (h)</i></para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Qsh"/></para>
/// </summary>
public class Qsh : CAM02
{
    public Qsh() : base() { }

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="Qsh"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
    {
        var result = From<Qsh>(input, new());
        Value = new(result.Q, result.s, result.h);
    }
}