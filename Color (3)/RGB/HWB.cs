using System;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Hue (H), Whiteness (W), Blackness (B)</b>
/// <para>A model that defines color as having hue (H), whiteness (W), and blackness (B).</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="HSB"/> > <see cref="HWB"/></para>
/// </summary>
/// <remarks>https://drafts.csswg.org/css-color/#the-hwb-notation</remarks>
[Component(360, '°', "H", "Hue"), Component(100, '%', "W", "Whiteness"), Component(100, '%', "B", "Blackness")]
[Category(Class.H), Class(Class.H), Serializable]
[Description("A model that defines color as having hue (H), whiteness (W), and blackness (B).")]
public class HWB : ColorModel3
{
    public HWB() : base() { }

	/// <summary>(🗸) <see cref="Lrgb"/> > <see cref="HWB"/></summary>
	public override void From(Lrgb input, WorkingProfile profile)
    {
		var hsb = new HSB();
		hsb.From(input, profile);

		var white = Min(input.X, Min(input.Y, input.Z));
		var black = 1 - Max(input.X, Max(input.Y, input.Z));

		Value = new(hsb.X, white * 100, black * 100);
	}

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

		rgb.X *= (1 - white - black);
		rgb.X += white;

		rgb.Y *= (1 - white - black);
		rgb.Y += white;

		rgb.Z *= (1 - white - black);
		rgb.Z += white;
		return rgb;
	}
}