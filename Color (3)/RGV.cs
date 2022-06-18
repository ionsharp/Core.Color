using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para><b>Orange (R), Spring green (G), Violet (V)</b></para>
/// <para>A variation of <see cref="RGB"/> in which the <b>Orange</b> (red/yellow), <b>Spring green</b> (green/cyan), and <b>Violet</b> (blue/magenta) <i>tertiary</i> colors are added together.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="RGV"/></para>
///
/// <i>Author</i>
/// <list type="bullet">
/// <item>Imagin (2022)</item>
/// </list>
/// </summary>
[Component(255, "R", "Orange"), Component(255, "G", "Spring green"), Component(255, "V", "Violet")]
[Serializable]
public class RGV : ColorModel3
{
    public RGV() : base() { }

	/// <summary>(🞩) <see cref="Lrgb"/> > <see cref="RGV"/></summary>
	public override void From(Lrgb input, WorkingProfile profile) { }

    /// <summary>(🗸) <see cref="RGV"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
		//(100%) Orange
		//> R = 75%, G = 25%

		//(100%) Spring green
		//> G = 75%, B = 25%

		//(100%) Violet
		//> B = 75%, R = 25%

		var result = XYZ / 255;
		double r = result.X, g = result.Y, v = result.Z;
		return Colour.New<Lrgb>(0.75 * r + 0.25 * v, 0.75 * g + 0.25 * r, 0.75 * v + 0.25 * g);
	}
}