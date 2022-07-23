using Imagin.Core.Numerics;
using static System.Math;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para><b>Intensity (I), Cyan/red (P), Blue/yellow (T)</b></para>
/// <para>A preceder to 'ICtCp' that is similar to 'YCwCm', but has smoother transitions between hues. 'P' stands for protanopia (or red-green colorblindness) and 'T' stands for tritanopia (another form of colorblindness).</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="LMS"/> > <see cref="IPT"/></para>
/// 
/// <i>Author</i>
/// <list type="bullet">
/// <item>Ebner/Fairchild (1998)</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/tommyettinger/colorful-gdx</remarks>
[Component(1, '%', "I", "Intensity"), Component(-1, 1, '%', "P", "Cyan/red"), Component(-1, 1, '%', "T", "Blue/yellow")]
[Category(Class.XYZ), Serializable]
[Description("A preceder to 'ICtCp' that is similar to 'YCwCm', but has smoother transitions between hues. 'P' stands for protanopia (or red-green colorblindness) and 'T' stands for tritanopia (another form of colorblindness).")]
public class IPT : ColorModel3
{
    public static Matrix M = new Matrix
    (
        new double[][]
        {
            new[] { 0.4000,  0.4000,  0.2000 },
            new[] { 4.4550, -4.8510,  0.3960 },
            new[] { 0.8056,  0.3572, -1.1628 }
        }
    );

    public IPT() : base() { }

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="IPT"/></summary>
    public override void From(Lrgb input, WorkingProfile profile) 
    {
        var lms = new LMS();
        lms.From(input, profile);

        var l = lms.X >= 0 ? Pow(lms.X, 0.43) : -Pow(-lms.X, 0.43);
        var m = lms.Y >= 0 ? Pow(lms.Y, 0.43) : -Pow(-lms.Y, 0.43);
        var s = lms.Z >= 0 ? Pow(lms.Z, 0.43) : -Pow(-lms.Z, 0.43);

        Value = M * new Vector3(l, m, s);
    }

    /// <summary>(🞩) <see cref="IPT"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
        //(1) IPT > LMS
        var m = M.Invert3By3() * Value;

        var lms = Colour.New<LMS>(m[0], m[1], m[2]);

        //(2) ?

        //(3) LMS > Lrgb
        return lms.To(profile);
    }
}