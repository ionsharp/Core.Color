using System;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <para><b>Red (R), Yellow (Y), Blue (B)</b></para>
/// <para>A variation of <see cref="RGB"/> in which the <b>Red</b> and <b>Blue</b> <i>primary</i> colors are added with the <b>Yellow</b> <i>secondary</i> color.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="RYB"/></para>
/// </summary>
/// <remarks>http://www.deathbysoftware.com/colors/index.html</remarks>
[Component(255, "R", "Red"), Component(255, "Y", "Yellow"), Component(255, "B", "Blue")]
[Serializable]
public class RYB : ColorModel3
{
    public RYB() : base() { }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="RYB"/></summary>
    public override void From(Lrgb input, WorkingProfile profile)
	{
		var r = input.X; var g = input.Y; var b = input.Z;

		//Remove the white from the color
		var white = Min(r, Min(g, b));

		r -= white;
		g -= white;
		b -= white;

		var mG = Max(r, Max(g, b));

		//Get the yellow out of the red/green

		var y = Min(r, g);

		r -= y;
		g -= y;

		//If blue and green, cut each in half to preserve maximum range
		if (b > 0 && g > 0)
		{
			b /= 2;
			g /= 2;
		}

		//Redistribute the remaining green
		y += g;
		b += g;

		//Normalize
		var mY = Max(r, Max(y, b));

		if (mY > 0)
		{
			var mN = mG / mY;

			r *= mN;
			y *= mN;
			b *= mN;
		}

		//Add the white back in
		r += white;
		y += white;
		b += white;

		Value = new(Floor(r), Floor(y), Floor(b));
	}

    /// <summary>(🗸) <see cref="RYB"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
		var r = X; var y = Y; var b = Z;

		//Remove the whiteness
		var white = Min(r, Min(y, b));
		r -= white; y -= white; b -= white;

		var mY = Max(r, Max(y, b));

		//Get the green out of the yellow/blue
		var g = Min(y, b);
		y -= g; b -= g;

		if (b > 0 && g > 0)
		{
			b *= 2.0; g *= 2.0;
		}

		//Redistribute the remaining yellow
		r += y; g += y;

		//Normalize
		var mG = Max(r, Max(g, b));
		if (mG > 0)
		{
			var mN = mY / mG;
			r *= mN; g *= mN; b *= mN;
		}

		//Add the white back in
		r += white; g += white; b += white;
		return Colour.New<Lrgb>(Floor(r), Floor(g), Floor(b));
	}
}