using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para><b>Rose (R), Chartreuse (C), Azure (A)</b></para>
/// <para>A variation of <see cref="RGB"/> in which the <b>Rose</b> (red/magenta), <b>Chartreuse</b> (green/yellow), and <b>Azure</b> (blue/cyan) <i>tertiary</i> colors are added together.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="RCA"/></para>
///
/// <i>Author</i>
/// <list type="bullet">
/// <item>Imagin (2022)</item>
/// </list>
/// </summary>
[Component(255, "R", "Rose"), Component(255, "C", "Chartreuse"), Component(255, "A", "Azure")]
[Serializable]
public class RCA : ColorModel3
{
    public RCA() : base() { }

	/// <summary>(🞩) <see cref="Lrgb"/> > <see cref="RCA"/></summary>
	public override void From(Lrgb input, WorkingProfile profile) 
	{
	}

	/// <summary>(🗸) <see cref="RCA"/> > <see cref="Lrgb"/></summary>
	public override Lrgb To(WorkingProfile profile)
	{
		//(100%) Rose
		//> R = 75%, B = 25%

		//(100%) Chartreuse
		//> G = 75%, R = 25%

		//(100%) Azure
		//> B = 75%, G = 25%

		var result = XYZ / 255;
		double r = result.X, c = result.Y, a = result.Z;
		return Colour.New<Lrgb>(0.75 * r + 0.25 * c, 0.75 * c + 0.25 * a, 0.75 * a + 0.25 * r);
	}
}