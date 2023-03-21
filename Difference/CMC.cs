using Imagin.Core.Numerics;
using System;
using static Imagin.Core.Colors.CMCColorDifferenceThreshold;
using static Imagin.Core.Numerics.M;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>CMC l:c (1984) color difference formula.</summary>
/// <remarks>
/// <para>https://github.com/tompazourek/Colourful</para>
/// <para>http://www.Zrucelindbloom.com/index.html?Eqn_DeltaE_CMC.html</para>
/// </remarks>
[Name("CMC l:c (1984)"), Serializable]
public class CMCColorDifference : IColorDifference<Lab>, IColorDifference
{
    /// <summary>Chroma.</summary>
    private readonly double _c;

    /// <summary>Lightness.</summary>
    private readonly double _l;

    public CMCColorDifference() { }

    /// <summary>Constructs with given recommended threshold parameters.</summary>
    public CMCColorDifference(in CMCColorDifferenceThreshold threshold) : this(threshold == Acceptability ? 2 : 1, 1) { }

    /// <summary>Constructs with arbitrary threshold parameters.</summary>
    public CMCColorDifference(in double lightness, in double chroma)
    {
        _l = lightness;
        _c = chroma;
    }

    /// <inheritdoc />
    public double ComputeDifference(in Lab x, in Lab y)
    {
        double L1 = x.X, a1 = x.Y, b1 = x.Z;
        double L2 = y.X, a2 = y.Y, b2 = y.Z;

        var dL = L1 - L2;
        var da = a1 - a2;
        var db = b1 - b2;

        var C1 = Sqrt(a1 * a1 + b1 * b1);
        var C2 = Sqrt(a2 * a2 + b2 * b2);
        var dC = C1 - C2;

        var dH_pow2 = da * da + db * db - dC * dC;
        var H1_rad = Atan2(b1, a1);
        var H1 = Angle.NormalizeDegree(Angle.GetDegree(H1_rad));

        var C1_pow4 = Pow4(in C1);
        var F = Sqrt(C1_pow4 / (C1_pow4 + 1900));

        var T = H1 >= 164 && H1 <= 345
            ? 0.56 + Abs(0.2 * CosDeg(H1 + 168))
            : 0.36 + Abs(0.4 * CosDeg(H1 + 35));

        var SC = 0.0638 * C1 / (1 + 0.0131 * C1) + 0.638;
        var SL = L1 < 16
            ? 0.511
            : 0.040975 * L1 / (1 + 0.01765 * L1);

        var SH = SC * (F * T + 1 - F);

        var dE_1 = dL / (_l * SL);
        var dE_2 = dC / (_c * SC);
        var dE_3_pow2 = dH_pow2 / (SH * SH);

        var dE = Sqrt(dE_1 * dE_1 + dE_2 * dE_2 + dE_3_pow2);
        return dE;
    }

    double IColorDifference.ComputeDifference(in ColorModel x, in ColorModel y) => ComputeDifference((Lab)x, (Lab)y);
}