using Imagin.Core.Numerics;
using System;

using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Hue (H), Whiteness (W), Blackness (B)</b>
/// <para>≡ 73.105%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="HWB"/></para>
/// </summary>
/// <remarks>https://drafts.csswg.org/css-color/#the-hwb-notation</remarks>
[Component(0, 360, '°', "H", "Hue")]
[Component(0, 100, '%', "W", "Whiteness")]
[Component(0, 100, '%', "B", "Blackness")]
[Serializable]
public sealed class HWB : ColorVector3
{
    public HWB(params double[] input) : base(input) { }

    public static implicit operator HWB(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="HWB"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        var white = Value[1] / 100;
        var black = Value[2] / 100;
        if (white + black >= 1)
        {
            var gray = white / (white + black);
            return new(gray, gray, gray);
        }
        var rgb = new HSL(Value[0], 100, 50).ToLrgb(profile);
        for (var i = 0; i < 3; i++)
        {
            rgb[i] *= (1 - white - black);
            rgb[i] += white;
        }
        return rgb;
    }

    /// <summary><see cref="RGB"/> > <see cref="HWB"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        var hsl = new HSL();
        hsl.FromLrgb(input, profile);

        var white = Min(input[0], Min(input[1], input[2]));
        var black = 1 - Max(input[0], Max(input[1], input[2]));
        Value = new(hsl[0], white * 100, black * 100);
    }
}