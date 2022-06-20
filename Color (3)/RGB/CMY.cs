using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Cyan (C), Magenta (M), Yellow (Y)</b>
/// <para>A subtractive color model in which the cyan, magenta, and yellow secondary colors are added together.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="CMY"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/cmy.js</remarks>
[Component(1, "C", "Cyan"), Component(1, "M", "Magenta"), Component(1, "Y", "Yellow")]
[Serializable]
public class CMY : ColorModel3
{
    public CMY() : base() { }

    /// <summary>(🗸) <see cref="CMY"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
        => Colour.New<Lrgb>(1 - X, 1 - Y, 1 - Z);

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="CMY"/></summary>
    public override void From(Lrgb input, WorkingProfile profile)
        => Value = new(1 - input.X, 1 - input.Y, 1 - input.Z);
}