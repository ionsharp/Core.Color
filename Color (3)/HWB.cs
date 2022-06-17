using System;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Hue (H), Whiteness (W), Blackness (B)</b>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="HSB"/> > <see cref="HWB"/></para>
/// </summary>
/// <remarks>https://drafts.csswg.org/css-color/#the-hwb-notation</remarks>
[Component(360, '°', "H", "Hue"), Component(100, '%', "W", "Whiteness"), Component(100, '%', "B", "Blackness")]
[Serializable]
public class HWB : ColorModel3
{
    public HWB() : base() { }

    /// <summary>(🗸) <see cref="HWB"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
		var white = Y / 100;
		var black = Z / 100;

		if (white + black >= 1)
		{
			var gray = white / (white + black);
			return Colour.New<Lrgb>(gray, gray, gray);
		}

		var hsb = Colour.New<HSB>(X, 100, 100);
		hsb.To(out Lrgb rgb, profile);

		rgb[0] *= (1 - white - black);
		rgb[0] += white;

		rgb[1] *= (1 - white - black);
		rgb[1] += white;

		rgb[2] *= (1 - white - black);
		rgb[2] += white;
		return rgb;
	}

	/// <summary>(🗸) <see cref="Lrgb"/> > <see cref="HWB"/></summary>
	public override void From(Lrgb input, WorkingProfile profile)
    {
		var hsb = new HSB();
		hsb.From(input, profile);

		var white = Min(input[0], Min(input[1], input[2]));
		var black = 1 - Max(input[0], Max(input[1], input[2]));

		Value = new(hsb[0], white * 100, black * 100);
	}
}