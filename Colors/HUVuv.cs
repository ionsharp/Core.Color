using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>Hue (H), Chroma (Uu), Luminance (Vv)</b></para>
/// Similar to, and based on, <see cref="LCHuv"/>, an experimental derivative of <see cref="Luv"/> that maps chroma (<see cref="U"/>) to a sphere.
/// <para>≡ 50%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Luv"/> > <see cref="HUVuv"/></para>
/// 
/// <i>Author</i>
/// <list type="bullet">
/// <item><see href="https://github.com/imagin-tech">Imagin</see> (2022)</item>
/// </list>
/// </summary>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "Uu", "Chroma")]
[Component(0, 100, '%', "Vv", "Luminance")]
[Serializable]
public sealed class HUVuv : LuvVector
{
    /// <summary><see cref="HUV.Hue"/></summary>
    public double H => X;

    /// <summary><see cref="HUV.Chroma"/></summary>
    public double U => Y;

    /// <summary><see cref="HUV.Lightness"/></summary>
    public double V => Z;

    public HUVuv(params double[] input) : base(input) { }

    public static implicit operator HUVuv(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HUVuv"/> > <see cref="Luv"/></summary>
    public override Luv ToLUV(WorkingProfile profile) => new HUV(this).To();

    /// <summary><see cref="Luv"/> > <see cref="HUVuv"/></summary>
    public override void FromLUV(Luv input, WorkingProfile profile)
    {
        var huv = new HUV();
        huv.From(input);

        Value = huv;
    }
}