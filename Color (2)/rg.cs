using Imagin.Core.Numerics;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>rg</b>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="rg"/></para>
/// </summary>
/// <remarks>https://en.wikipedia.org/wiki/Rg_chromaticity</remarks>
[Component(1, "r"), Component(1, "g")]
[Hidden, Serializable]
[SuppressMessage("Style", "IDE1006:Naming Styles")]
public class rg : ColorModel2<rgG>
{
    public rg() : base() { }

    //...

    public static explicit operator rg(Vector2 input) => Colour.New<rg>(input);

    public static explicit operator rg(rgG input) => Colour.New<rg>(input.XY);

    //...

    /// <summary>(🗸) <see cref="rgG"/> > <see cref="rg"/></summary>
    public override void From(rgG input, WorkingProfile profile)
        => Value = input.XY;

    /// <summary>(🗸) <see cref="rg"/> > <see cref="rgG"/></summary>
    public override void To(out rgG result, WorkingProfile profile)
        => result = Colour.New<rgG>(X, Y, 1);
}