using Imagin.Core.Numerics;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Chroma (x), Chroma (y)</b>
/// <para>A color with two <b>Chroma</b> components.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="xyY"/> > <see cref="xy"/></para>
/// </summary>
[Component(1, "x"), Component(1, "y")]
[Hidden, Serializable]
[SuppressMessage("Style", "IDE1006:Naming Styles")]
public class xy : ColorModel2<xyY>
{
    public xy() : base() { }

    //...

    public static explicit operator xy(Vector2 input) => Colour.New<xy>(input);

    public static explicit operator xy(xyY input) => Colour.New<xy>(input.XY);

    //...

    /// <summary>(🗸) <see cref="xyY"/> > <see cref="xy"/></summary>
    public override void From(xyY input, WorkingProfile profile)
        => Value = input.XY;

    /// <summary>(🗸) <see cref="xy"/> > <see cref="xyY"/></summary>
    public override void To(out xyY result, WorkingProfile profile)
        => result = Colour.New<xyY>(X, Y, 1);
}