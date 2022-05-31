using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>Hue (H), Chroma (Ua), Luminance (Vb)</b></para>
/// Similar to, and based on, <see cref="LCHab"/>, an experimental derivative of <see cref="Lab"/> that maps chroma (<see cref="U"/>) to a sphere.
/// <para>≡ 50%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Lab"/> > <see cref="HUVab"/></para>
/// 
/// <i>Author</i>
/// <list type="bullet">
/// <item><see href="https://github.com/imagin-tech">Imagin</see> (2022)</item>
/// </list>
/// </summary>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "Ua", "Chroma")]
[Component(0, 100, '%', "Vb", "Luminance")]
[Serializable]
public sealed class HUVab : LabVector
{
    /// <summary><see cref="HUV.Hue"/></summary>
    public double H => X;

    /// <summary><see cref="HUV.Chroma"/></summary>
    public double U => Y;

    /// <summary><see cref="HUV.Lightness"/></summary>
    public double V => Z;

    public HUVab(params double[] input) : base(input) { }

    public static implicit operator HUVab(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HUVab"/> > <see cref="Lab"/></summary>
    public override Lab ToLAB(WorkingProfile profile) => new HUV(this).To();

    /// <summary><see cref="Lab"/> > <see cref="HUVab"/></summary>
    public override void FromLAB(Lab input, WorkingProfile profile)
    {
        var huv = new HUV();
        huv.From(input);

        Value = huv;
    }
}