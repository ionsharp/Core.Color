using System;
using System.Collections.Generic;

using static System.Double;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>An intermediate <see cref="ColorVector3"/> that is used to chain conversions between color spaces.</summary>
public abstract class ColorVector3i : ColorVector3
{
    public ColorVector3i(params double[] input) : base(input) { }
}

#region JzAzBzVector 

/// <remarks><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="JzAzBz"/> > 🞶</remarks>
/// <inheritdoc/>
[Serializable]
public abstract class JzAzBzVector : XYZVector
{
    public JzAzBzVector(params double[] input) : base(input) { }

    /// <summary>🞶 > <see cref="JzAzBz"/></summary>
    public abstract JzAzBz ToJzAzBz(WorkingProfile profile);

    /// <summary><see cref="JzAzBz"/> > <see cref="XYZ"/></summary>
    public sealed override XYZ ToXYZ(WorkingProfile profile)
    {
        //* > JzAzBz
        var jzazbz = ToJzAzBz(profile);

        //JzAzBz > XYZ
        return jzazbz.ToXYZ(profile);
    }

    /// <summary><see cref="JzAzBz"/> > 🞶</summary>
    public abstract void FromJzAzBz(JzAzBz input, WorkingProfile profile);

    /// <summary><see cref="XYZ"/> > <see cref="JzAzBz"/></summary>
    public sealed override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        //XYZ > JzAzBz
        var jzazbz = new JzAzBz();
        jzazbz.FromXYZ(input, profile);

        //JzAzBz > *
        FromJzAzBz(jzazbz, profile);
    }
}

#endregion

//...

#region LabVector

/// <remarks><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Lab"/> > 🞶</remarks>
/// <inheritdoc/>
[Serializable]
public abstract class LabVector : XYZVector
{
    public LabVector(params double[] input) : base(input) { }

    /// <summary>🞶 > <see cref="Lab"/></summary>
    public abstract Lab ToLAB(WorkingProfile profile);

    /// <summary><see cref="Lab"/> > <see cref="XYZ"/></summary>
    public sealed override XYZ ToXYZ(WorkingProfile profile)
    {
        //* > LAB
        var lab = ToLAB(profile);

        //LAB > XYZ
        return lab.ToXYZ(profile);
    }

    /// <summary><see cref="Lab"/> > 🞶</summary>
    public abstract void FromLAB(Lab input, WorkingProfile profile);

    /// <summary><see cref="XYZ"/> > <see cref="Lab"/></summary>
    public sealed override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        //XYZ > LAB
        var lab = new Lab();
        lab.FromXYZ(input, profile);

        //LAB > *
        FromLAB(lab, profile);
    }
}

#endregion

#region LabhVector

/// <remarks><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Labh"/> > 🞶</remarks>
/// <inheritdoc/>
[Serializable]
public abstract class LabhVector : XYZVector
{
    public LabhVector(params double[] input) : base(input) { }

    /// <summary>🞶 > <see cref="Labh"/></summary>
    public abstract Labh ToLABh(WorkingProfile profile);

    /// <summary><see cref="Labh"/> > <see cref="XYZ"/></summary>
    public sealed override XYZ ToXYZ(WorkingProfile profile)
    {
        //* > LABh
        var labh = ToLABh(profile);

        //LABh > XYZ
        return labh.ToXYZ(profile);
    }

    /// <summary><see cref="Labh"/> > 🞶</summary>
    public abstract void FromLABh(Labh input, WorkingProfile profile);

    /// <summary><see cref="XYZ"/> > <see cref="Labh"/></summary>
    public sealed override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        //XYZ > LABh
        var labh = new Labh();
        labh.FromXYZ(input, profile);

        //LABh > *
        FromLABh(labh, profile);
    }
}

#endregion

//...

#region LuvVector

/// <remarks><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Luv"/> > 🞶</remarks>
/// <inheritdoc/>
[Serializable]
public abstract class LuvVector : XYZVector
{
    public LuvVector(params double[] input) : base(input) { }

    /// <summary>🞶 > <see cref="Luv"/></summary>
    public abstract Luv ToLUV(WorkingProfile profile);

    /// <summary><see cref="Luv"/> > <see cref="XYZ"/></summary>
    public sealed override XYZ ToXYZ(WorkingProfile profile)
    {
        //* > LUV
        var luv = ToLUV(profile);

        //LUV > XYZ
        return luv.ToXYZ(profile);
    }

    /// <summary><see cref="Luv"/> > 🞶</summary>
    public abstract void FromLUV(Luv input, WorkingProfile profile);

    /// <summary><see cref="XYZ"/> > <see cref="Luv"/></summary>
    public sealed override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        //XYZ > LUV
        var luv = new Luv();
        luv.FromXYZ(input, profile);

        //LUV > *
        FromLUV(luv, profile);
    }
}

#endregion

#region LCHuvVector

/// <remarks><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="Luv"/> > <see cref="LCHuv"/> > 🞶</remarks>
/// <inheritdoc/>
[Serializable]
public abstract class LCHuvVector : LuvVector
{
    protected static double[][] M = new double[][]
    {
        new double[] {  3.240969941904521, -1.537383177570093, -0.498610760293    },
        new double[] { -0.96924363628087,   1.87596750150772,   0.041555057407175 },
        new double[] {  0.055630079696993, -0.20397695888897,   1.056971514242878 },
    };

    public LCHuvVector(params double[] input) : base(input) { }

    //...

    protected static IList<double[]> GetBounds(double L)
    {
        var result = new List<double[]>();

        double sub1 = Pow(L + 16, 3) / 1560896;
        double sub2 = sub1 > CIE.IEpsilon ? sub1 : L / CIE.IKappa;

        for (int c = 0; c < 3; ++c)
        {
            var m1 = M[c][0];
            var m2 = M[c][1];
            var m3 = M[c][2];

            for (int t = 0; t < 2; ++t)
            {
                var top1 = (284517 * m1 - 94839 * m3) * sub2;
                var top2 = (838422 * m3 + 769860 * m2 + 731718 * m1) * L * sub2 - 769860 * t * L;
                var bottom = (632260 * m3 - 126452 * m2) * sub2 + 126452 * t;

                result.Add(new double[] { top1 / bottom, top2 / bottom });
            }
        }

        return result;
    }

    protected static double GetChroma(double L)
    {
        var bounds = GetBounds(L);
        double min = MaxValue;

        for (int i = 0; i < 2; ++i)
        {
            var m1 = bounds[i][0];
            var b1 = bounds[i][1];
            var line = new double[] { m1, b1 };

            double x = GetIntersection(line, new double[] { -1 / m1, 0 });
            double length = GetDistance(new double[] { x, b1 + x * m1 });

            min = Min(min, length);
        }

        return min;
    }

    protected static double GetChroma(double L, double H)
    {
        double hrad = H / 360 * PI * 2;

        var bounds = GetBounds(L);
        double min = MaxValue;
        foreach (var bound in bounds)
        {
            double length;
            if (GetRayLength(hrad, bound, out length))
                min = Min(min, length);
        }

        return min;
    }

    protected static double GetDistance(IList<double> point)
        => Sqrt(Pow(point[0], 2) + Pow(point[1], 2));

    protected static double GetIntersection(IList<double> lineA, IList<double> lineB)
        => (lineA[1] - lineB[1]) / (lineB[0] - lineA[0]);

    protected static bool GetRayLength(double theta, IList<double> line, out double length)
    {
        length = line[1] / (Sin(theta) - line[0] * Cos(theta));
        return length >= 0;
    }

    //...

    /// <summary>🞶 > <see cref="LCHuv"/></summary>
    public abstract LCHuv ToLCHuv(WorkingProfile profile);

    /// <summary><see cref="LCHuv"/> > <see cref="Luv"/></summary>
    public sealed override Luv ToLUV(WorkingProfile profile)
    {
        //* > LCHuv
        var lchuv = ToLCHuv(profile);

        //LCHuv > LUV
        return lchuv.ToLUV(profile);
    }

    /// <summary><see cref="LCHuv"/> > 🞶</summary>
    public abstract void FromLCHuv(LCHuv input, WorkingProfile profile);

    /// <summary><see cref="Luv"/> > <see cref="LCHuv"/></summary>
    public sealed override void FromLUV(Luv input, WorkingProfile profile)
    {
        //LUV > LCHuv
        var lchuv = new LCHuv();
        lchuv.FromLUV(input, profile);

        //LCHuv > *
        FromLCHuv(lchuv, profile);
    }
}

#endregion

//...

#region OKLabVector

/// <remarks><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="LMS"/> > <see cref="OKLab"/> > 🞶</remarks>
/// <inheritdoc/>
[Serializable]
public abstract class OKLabVector : XYZVector
{
    public OKLabVector(params double[] input) : base(input) { }

    /// <summary>🞶 > <see cref="OKLab"/></summary>
    public abstract OKLab ToOKLab(WorkingProfile profile);

    /// <summary><see cref="OKLab"/> > <see cref="XYZ"/></summary>
    public sealed override XYZ ToXYZ(WorkingProfile profile)
    {
        //* > OKLab
        var oklab = ToOKLab(profile);

        //OKLab > XYZ
        return oklab.ToXYZ(profile);
    }

    /// <summary><see cref="OKLab"/> > 🞶</summary>
    public abstract void FromOKLab(OKLab input, WorkingProfile profile);

    /// <summary><see cref="XYZ"/> > <see cref="OKLab"/></summary>
    public sealed override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        //XYZ > OKLab
        var oklab = new OKLab();
        oklab.FromXYZ(input, profile);

        //OKLab > *
        FromOKLab(oklab, profile);
    }
}

#endregion

//...

#region XYZVector

/// <remarks><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > 🞶</remarks>
/// <inheritdoc/>
[Serializable]
public abstract class XYZVector : ColorVector3i
{
    public XYZVector(params double[] input) : base(input) { }

    /// <summary>🞶 > <see cref="XYZ"/></summary>
    public abstract XYZ ToXYZ(WorkingProfile profile);

    /// <summary><see cref="XYZ"/> > <see cref="Lrgb"/></summary>
    public sealed override Lrgb ToLrgb(WorkingProfile profile)
    {
        //* > XYZ
        var xyz = ToXYZ(profile);

        //XYZ > Lrgb
        return xyz.ToLrgb(profile);
    }

    /// <summary><see cref="XYZ"/> > 🞶</summary>
    public abstract void FromXYZ(XYZ input, WorkingProfile profile);

    /// <summary><see cref="Lrgb"/> > <see cref="XYZ"/></summary>
    public sealed override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        //Lrgb > XYZ
        var xyz = new XYZ();
        xyz.FromLrgb(input, profile);

        //XYZ > *
        FromXYZ(xyz, profile);
    }
}

#endregion

#region YPbPrVector

/// <remarks><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YPbPr"/> > 🞶</remarks>
/// <inheritdoc/>
[Serializable]
public abstract class YPbPrVector : ColorVector3i
{
    public YPbPrVector(params double[] input) : base(input) { }

    /// <summary>🞶 > <see cref="YPbPr"/></summary>
    public abstract YPbPr ToYPbPr(WorkingProfile profile);

    /// <summary><see cref="YPbPr"/> > <see cref="Lrgb"/></summary>
    public sealed override Lrgb ToLrgb(WorkingProfile profile)
    {
        //* > YPbPr
        var ypbpr = ToYPbPr(profile);

        //YPbPr > Lrgb
        return ypbpr.ToLrgb(profile);
    }

    /// <summary><see cref="YPbPr"/> > 🞶</summary>
    public abstract void FromYPbPr(YPbPr input, WorkingProfile profile);

    /// <summary><see cref="Lrgb"/> > <see cref="YPbPr"/></summary>
    public sealed override void FromLrgb(Lrgb input, WorkingProfile profile)
    {
        //Lrgb > YPbPr
        var ypbpr = new YPbPr();
        ypbpr.FromLrgb(input, profile);

        //YPbPr > *
        FromYPbPr(ypbpr, profile);
    }
}

#endregion

#region YCbCrVector

/// <remarks><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="YPbPr"/> > <see cref="YCbCr"/> > 🞶</remarks>
/// <inheritdoc/>
[Serializable]
public abstract class YCbCrVector : YPbPrVector
{
    public YCbCrVector(params double[] input) : base(input) { }

    /// <summary>🞶 > <see cref="YCbCr"/></summary>
    public abstract YCbCr ToYCbCr(WorkingProfile profile);

    /// <summary><see cref="YCbCr"/> > <see cref="YPbPr"/></summary>
    public sealed override YPbPr ToYPbPr(WorkingProfile profile)
    {
        //* > YCbCr
        var ycbcr = ToYCbCr(profile);

        //YCbCr > Lrgb
        return ycbcr.ToYPbPr(profile);
    }

    /// <summary><see cref="YCbCr"/> > 🞶</summary>
    public abstract void FromYCbCr(YCbCr input, WorkingProfile profile);

    /// <summary><see cref="YPbPr"/> > <see cref="YCbCr"/></summary>
    public sealed override void FromYPbPr(YPbPr input, WorkingProfile profile)
    {
        //Lrgb > YCbCr
        var ycbcr = new YCbCr();
        ycbcr.FromYPbPr(input, profile);

        //YCbCr > *
        FromYCbCr(ycbcr, profile);
    }
}

#endregion