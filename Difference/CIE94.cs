using System;
using static Imagin.Core.Colors.CIE94ColorDifferenceApplication;
using static Imagin.Core.Numerics.M;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>CIE Delta-E 1994 color difference formula.</summary>
/// <remarks>
/// <para>https://github.com/tompazourek/Colourful</para>
/// <para>http://www.Zrucelindbloom.com/Eqn_DeltaE_CIE94.html</para>
/// </remarks>
[DisplayName("CIE Delta-E 1994"), Serializable]
public class CIE94ColorDifference : IColorDifference<Lab>, IColorDifference
{
    private const double KH = 1;
    private const double KC = 1;

    private readonly double K1;
    private readonly double K2;
    private readonly double KL;

    /// <summary>Construct using weighting factors for <see cref="CIE94ColorDifferenceApplication.GraphicArts" />.</summary>
    public CIE94ColorDifference() : this(GraphicArts) { }

    /// <summary>Construct using weighting factors for given application of color difference.</summary>
    /// <param name="application">A <see cref="CIE94ColorDifferenceApplication" /> value specifying the application area. Different weighting factors are used in the computation depending on the application.</param>
    public CIE94ColorDifference(in CIE94ColorDifferenceApplication application)
    {
        if (application == Textiles)
        {
            KL = 2;
            K1 = 0.048;
            K2 = 0.014;
        }
        else
        {
            // GraphicArts
            KL = 1;
            K1 = 0.045;
            K2 = 0.015;
        }
    }

    /// <param name="x">Reference color.</param>
    /// <param name="y">Sample color.</param>
    /// <returns>Delta-E (1994) color difference.</returns>
    public double ComputeDifference(in Lab x, in Lab y)
    {
        var da = x.Y - y.Y;
        var db = x.Z - y.Z;
        var dL = x.X - y.X;
        var C1 = Sqrt(x.Y * x.Y + x.Z * x.Z);
        var C2 = Sqrt(y.Y * y.Y + y.Z * y.Z);
        var dC = C1 - C2;
        var dH_sq = da * da + db * db - dC * dC; // dH ^ 2
        const double SL = 1;
        var SC = 1 + K1 * C1;
        var SH = 1 + K2 * C1;
        var dE94 = Sqrt(Pow2(dL / (KL * SL)) + Pow2(dC / (KC * SC)) + dH_sq / Pow2(KH * SH));
        return dE94;
    }

    double IColorDifference.ComputeDifference(in ColorModel x, in ColorModel y) => ComputeDifference((Lab)x, (Lab)y);
}