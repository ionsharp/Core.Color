using Imagin.Core.Numerics;
using System;
using static Imagin.Core.Numerics.M;
using static System.Math;
using static System.MidpointRounding;

namespace Imagin.Core.Colors;

/// <summary>
/// <para><b>Red (R), Green (G), Blue (B)</b></para>
/// 
/// <para>An additive color model in which the <b>Red</b>, <b>Green</b>, and <b>Blue</b> <i>primary</i> colors are added together.</para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>CIERGB</item>
/// </list>
/// 
/// <i>Author</i>
/// <list type="bullet">
/// <item><see cref="CIE"/> (1931)</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(255, "R", "Red"), Component(255, "G", "Green"), Component(255, "B", "Blue")]
[Description("An additive color model in which the <b>Red</b>, <b>Green</b>, and <b>Blue</b> <i>primary</i> colors are added together.")]
[Category(Class.RGB), Serializable]
public class RGB : ColorModel3
{
    public RGB() : base() { }

    //...

    /// <summary>(🗸) <see cref="RGB"/> (0) > <see cref="XYZ"/> (0) > <see cref="LMS"/> (0) > <see cref="LMS"/> (1) > <see cref="XYZ"/> (1) > <see cref="RGB"/> (1)</summary>
    public override void Adapt(WorkingProfile source, WorkingProfile target) => Value = Adapt(this, source, target);

    //...

    /// <summary>Gets <see cref="RGB"/> from 8-bit channels [0, 255].</summary>
    public static RGB From8Bit(in byte r, in byte g, in byte b) => Colour.New<RGB>(r, g, b);

    /// <summary>Gets <see cref="RGB"/> with all channels equal.</summary>
    public static RGB FromGray(in double value) => Colour.New<RGB>(value);

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="RGB"/></summary>
    public override void From(Lrgb input, WorkingProfile profile)
    {
        var result = input.XYZ.Transform((i, j) => profile.Compression.Transfer(j));
        Value = M.Denormalize(result, new(0), new(255));
    }

    /// <summary>(🗸) <see cref="RGB"/> > <see cref="RGB"/></summary>
    public override void From(RGB input, WorkingProfile profile) => Value = input.XYZ;

    //...

    /// <summary>Gets channel values as 8-bit values [0, 255].</summary>
    public void To8Bit(out byte r, out byte g, out byte b)
    {
        r = (byte)Clamp(Round(X, AwayFromZero), 255);
        g = (byte)Clamp(Round(Y, AwayFromZero), 255);
        b = (byte)Clamp(Round(Z, AwayFromZero), 255);
    }

    /// <summary>(🗸) <see cref="RGB"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
        var oldValue = M.Normalize(XYZ, new(0), new(255));
        return Colour.New<Lrgb>(oldValue.Transform((i, j) => profile.Compression.TransferInverse(j)));
    }

    /// <summary>(🗸) <see cref="RGB"/> > <see cref="RGB"/></summary>
    public override void To(out RGB result, WorkingProfile profile) => result = Colour.New<RGB>(XYZ);
}