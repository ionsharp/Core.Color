using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>Hue (H), Chroma (Ua), Luminance (Vb)</b></para>
/// <para>Similar to, and based on, <see cref="LCHabh"/>, an experimental derivative of <see cref="Labh"/> that maps chroma (<see cref="U"/>) to a sphere.</para>
/// <para>≡ 50%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Labh"/> > <see cref="HUVabh"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>Hunter HUVab</item>
/// </list>
/// <para><i>Author</i>: <see href="https://github.com/imagin-tech">Imagin</see> (2022)</para>
/// </summary>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "Ua", "Chroma")]
[Component(0, 100, '%', "Vb", "Luminance")]
[Serializable]
public sealed class HUVabh : LabhVector
{
    /// <summary><see cref="HUV.Hue"/></summary>
    public double H => X;

    /// <summary><see cref="HUV.Chroma"/></summary>
    public double U => Y;

    /// <summary><see cref="HUV.Lightness"/></summary>
    public double V => Z;

    public HUVabh(params double[] input) : base(input) { }

    public static implicit operator HUVabh(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HUVabh"/> > <see cref="Labh"/></summary>
    public override Labh ToLABh(WorkingProfile profile) => new HUV(this).To();

    /// <summary><see cref="Labh"/> > <see cref="HUVabh"/></summary>
    public override void FromLABh(Labh input, WorkingProfile profile)
    {
        var huv = new HUV();
        huv.From(input);

        Value = huv;
    }
}