namespace Imagin.Core.Colors;

/// <summary>
/// <b>Jsh</b> (<see cref="CAM02"/>)
/// <para><i>Lightness (J), Saturation (s), Hue (h)</i></para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Jsh"/></para>
/// </summary>
public class Jsh : CAM02
{
    public Jsh() : base() { }

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="Jsh"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
    {
        var result = From<Jsh>(input, new());
        Value = new(result.J, result.s, result.h);
    }
}