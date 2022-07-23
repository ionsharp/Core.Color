using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Cyan (C), Magenta (M), Yellow (Y)</b>
/// <para>A subtractive model where the secondary colors are added together.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="CMY"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/cmy.js</remarks>
[Component(100, "C", "Cyan"), Component(100, "M", "Magenta"), Component(100, "Y", "Yellow")]
[Category(Class.CMY), Serializable]
[Description("A subtractive model where the secondary colors are added together.")]
public class CMY : ColorModel3
{
    public CMY() : base() { }

    /// <summary>(🗸) <see cref="CMY"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
        => Colour.New<Lrgb>(1 - X / 100, 1 - Y / 100, 1 - Z / 100);

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="CMY"/></summary>
    public override void From(Lrgb input, WorkingProfile profile)
        => Value = new Vector3(1 - input.X, 1 - input.Y, 1 - input.Z) * 100;
}