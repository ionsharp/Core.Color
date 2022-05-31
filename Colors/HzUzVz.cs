using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>Hue (Hz), Chroma (Uz), Luminance (Vz)</b></para>
/// Similar to, and based on, <see cref="JzCzHz"/>, an experimental derivative of <see cref="JzAzBz"/> that maps chroma (<see cref="Uz"/>) to a sphere.
/// <para>≡ 50%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="JzAzBz"/> > <see cref="HzUzVz"/></para>
/// 
/// <i>Author</i>
/// <list type="bullet">
/// <item><see href="https://github.com/imagin-tech">Imagin</see> (2022)</item>
/// </list>
/// </summary>
[Component(0, 360, '°', "Hz", "Hue")]
[Component(0, 100, '%', "Uz", "Chroma")]
[Component(0, 100, '%', "Vz", "Luminance")]
[Serializable]
public sealed class HzUzVz : JzAzBzVector
{
    /// <summary><see cref="HUV.Hue"/></summary>
    public double Hz => X;

    /// <summary><see cref="HUV.Chroma"/></summary>
    public double Uz => Y;

    /// <summary><see cref="HUV.Lightness"/></summary>
    public double Vz => Z;

    public HzUzVz(params double[] input) : base(input) { }

    public static implicit operator HzUzVz(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HzUzVz"/> > <see cref="JzAzBz"/></summary>
    public override JzAzBz ToJzAzBz(WorkingProfile profile) => new HUV(this).To() / 100;

    /// <summary><see cref="JzAzBz"/> > <see cref="HzUzVz"/></summary>
    public override void FromJzAzBz(JzAzBz input, WorkingProfile profile)
    {
        var huv = new HUV();
        huv.From((Vector3)input * 100);

        Value = huv;
    }
}