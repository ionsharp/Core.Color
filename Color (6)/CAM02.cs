using Imagin.Core.Numerics;
using System;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>CAM02</b>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="CAM02"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>CIECAM02</item>
/// </list>
/// 
/// <i>Authors</i>
/// <list type="bullet">
/// <item><see cref="CIE"/> (2002)</item>
/// <item><see href="vektor@dumbterm.net">Billy Biggs</see> (2003)</item>
/// </list>
/// 
/// <i>Versions</i>
/// <list type="bullet">
/// <item>( - ) Ported from C++.</item>
/// <item>(0.4) Various revisions by Nathan Moroney of HP Labs.</item>
/// <item>(0.3) Further cleanups and a function to return all of J,C,h,Q,M,s.</item>
/// <item>(0.2) Cleanup, added missing functions.</item>
/// <item>(0.1) Initial release</item>
/// </list>
/// </summary>
/// <remarks>http://scanline.ca/ciecam02/ciecam02.c</remarks>
[Serializable]
public abstract partial class CAM02 : ColorModel3<XYZ>
{
    #region (enum) Correlates

    /// <summary>Dimensions of color appearance as defined by <see cref="CIE"/>.</summary>
    public enum Correlates
    {
        /// <summary><b>Brightness</b> (Q)
        /// <para>The subjective appearance of how bright an object appears given its surroundings and how it is illuminated (also called <b>Luminance</b>).</para>
        /// </summary>
        /// <remarks><b>Q = (4 / c) * Sqrt(1 / 100 * J) * (A[w] + 4) * Pow(F[L], 1 / 4)</b></remarks>
        Brightness,
        /// <summary><b>Lightness</b> (J) 
        /// <para>The subjective appearance of how light a color appears to be.</para>
        /// </summary>
        /// <remarks><b>J = 100 * Pow(A / A[w], cz)</b></remarks>
        Lightness,

        /// <summary><b>Chroma</b> (C) 
        /// <para>The colorfulness relative to the brightness of another color that appears white under similar viewing conditions.</para></summary>
        /// <remarks><b>C = Pow(t, 0.9) * Sqrt(1 / 100 * J) * Pow(1.64 - Pow(0.29, n), 0.73)</b></remarks>
        Chroma,
        /// <summary><b>Saturation</b> (s) 
        /// <para>The colorfulness of a color relative to its own brightness.</para>
        /// </summary>
        /// <remarks><b>s = 100 * Sqrt(M / Q)</b></remarks>
        Saturation,

        /// <summary><b>Colorfulness</b> (M)
        /// <para>The degree of difference between a color and gray.</para></summary>
        /// <remarks><b>M = C * Pow(F[L], 1 / 4)</b></remarks>
        Colorfulness,
        /// <summary><b>Hue</b> (h) 
        /// <para>The degree to which a stimulus can be described as similar to or different from stimuli that are described as red, green, blue, and yellow (the so-called unique hues).</para></summary>
        /// <remarks><b>h = </b></remarks>
        Hue,
    }

    #endregion

    #region (enum) Surrounds

    /// <summary><see cref="CAM02"/> surround(ing)s as defined by <see cref="CIE"/>.</summary>
    public enum Surrounds
    {
        /// <summary>
        /// <b>Viewing surface colors.</b>
        /// <para>Relative luminance > 20% of scene white.</para>
        /// </summary>
        /// <remarks><see cref="ViewingConditions.F"/> = 1, <see cref="ViewingConditions.c"/> = 0.690, <see cref="ViewingConditions.Nc"/> = 1</remarks>
        Average,
        /// <summary>
        /// <b>Viewing television.</b>
        /// <para>Relative luminance 0% of scene white.</para>
        /// </summary>
        /// <remarks><see cref="ViewingConditions.F"/> = 0.9, <see cref="ViewingConditions.c"/> = 0.590, <see cref="ViewingConditions.Nc"/> = 0.95</remarks>
        /// <summary></summary>
        Dim,
        /// <summary>
        /// <b>Using a projector in a dark room.</b>
        /// <para>Relative luminance between 0% and 20% of scene white.</para>
        /// </summary>
        /// <remarks><see cref="ViewingConditions.F"/> = 0.8, <see cref="ViewingConditions.c"/> = 0.525, <see cref="ViewingConditions.Nc"/> = 0.8</remarks>
        /// <summary></summary>
        Dark,
    }

    #endregion

    #region Fields

    public static Matrix Aab_RGB => new double[][]
    {
        new double[] { 0.32787,  0.32145,  0.20527 },
        new double[] { 0.32787, -0.63507, -0.18603 },
        new double[] { 0.32787, -0.15681, -4.49038 }
    };

    public static Matrix CAT02_HPE => new double[][]
    {
        new double[] {  0.7409792,  0.2180250, 0.0410058 },
        new double[] {  0.2853532,  0.6242014, 0.0904454 },
        new double[] { -0.0096280, -0.0056980, 1.0153260 }
    };

    public static Matrix CAT02_XYZ => new double[][]
    {
        new double[] {  1.096124, -0.278869, 0.182745 },
        new double[] {  0.454369,  0.473533, 0.072098 },
        new double[] { -0.009628, -0.005698, 1.015326 }
    };

    public static Matrix HPE_XYZ => new double[][]
    {
        new double[] { 1.910197, -1.112124,  0.201908 },
        new double[] { 0.370950,  0.629054, -0.000008 },
        new double[] { 0,         0,         1        }
    };

    #endregion

    #region Properties

    public double aC { get; private set; }
    public double bC { get; private set; }

    public double aS { get; private set; }
    public double bS { get; private set; }

    public double aM { get; private set; }
    public double bM { get; private set; }

    //...

    public double H { get; private set; }

    //...

    public abstract double J { get; set; }

    public abstract double Q { get; set; }

    public abstract double C { get; set; }

    public abstract double M { get; set; }

    public abstract double s { get; set; }

    public double h
    {
        get => Z; set => Z = value;
    }

    #endregion

    #region CAM02

    public CAM02() : base() { }

    #endregion

    #region Methods

    static double NonlinearAdaptation(double c, double FL)
    {
        double p = Pow((FL * c) / 100.0, 0.42);
        return ((400.0 * p) / (27.13 + p)) + 0.1;
    }

    static double NonlinearAdaptationInverse(double c, double FL)
        => (100.0 / FL) * Pow((27.13 * Abs(c - 0.1)) / (400.0 - Abs(c - 0.1)), 1.0 / 0.42);

    //...

    /// <summary>(🞩) <see cref="XYZ"/> > <see cref="CAM02"/></summary>
    protected T From<T>(XYZ input, WorkingProfile profile) where T : CAM02, new()
    {
        input.Value *= new Vector3(100);

        var vc = profile.ViewingConditions;

        double r = 0, g = 0, b = 0;
        double rw = 0, gw = 0, bw = 0;
        double rc, gc, bc;
        double rp = 0, gp = 0, bp = 0;
        double rpa, gpa, bpa;
        double a, ca, cb;
        double et, t, temp;

        var rgb = LMS.Transform.CAT02 * new Vector(input.X, input.Y, input.Z);
        r = rgb[0]; g = rgb[1]; b = rgb[2];

        var rgbw = LMS.Transform.CAT02 * new Vector(profile.White.X * 100, profile.White.Y * 100, profile.White.Z * 100);
        rw = rgbw[0]; gw = rgbw[1]; bw = rgbw[2];

        rc = r * (((profile.White.Y * 100 * vc.D) / rw) + (1.0 - vc.D));
        gc = g * (((profile.White.Y * 100 * vc.D) / gw) + (1.0 - vc.D));
        bc = b * (((profile.White.Y * 100 * vc.D) / bw) + (1.0 - vc.D));

        var rgbp = CAT02_HPE * new Vector(rc, gc, bc);
        rp = rgbp[0]; gp = rgbp[1]; bp = rgbp[2];

        rpa = NonlinearAdaptation(rp, vc.FL);
        gpa = NonlinearAdaptation(gp, vc.FL);
        bpa = NonlinearAdaptation(bp, vc.FL);

        ca = rpa - ((12.0 * gpa) / 11.0) + (bpa / 11.0);
        cb = (1.0 / 9.0) * (rpa + gpa - (2.0 * bpa));

        T i = new();
        i.h = (180.0 / PI) * Atan2(cb, ca);
        if (i.h < 0.0) i.h += 360.0;

        if (i.h < 20.14)
        {
            temp = ((i.h + 122.47) / 1.2) + ((20.14 - i.h) / 0.8);
            i.H = 300 + (100 * ((i.h + 122.47) / 1.2)) / temp;
        }
        else if (i.h < 90.0)
        {
            temp = ((i.h - 20.14) / 0.8) + ((90.00 - i.h) / 0.7);
            i.H = (100 * ((i.h - 20.14) / 0.8)) / temp;
        }
        else if (i.h < 164.25)
        {
            temp = ((i.h - 90.00) / 0.7) + ((164.25 - i.h) / 1.0);
            i.H = 100 + ((100 * ((i.h - 90.00) / 0.7)) / temp);
        }
        else if (i.h < 237.53)
        {
            temp = ((i.h - 164.25) / 1.0) + ((237.53 - i.h) / 1.2);
            i.H = 200 + ((100 * ((i.h - 164.25) / 1.0)) / temp);
        }
        else
        {
            temp = ((i.h - 237.53) / 1.2) + ((360 - i.h + 20.14) / 0.8);
            i.H = 300 + ((100 * ((i.h - 237.53) / 1.2)) / temp);
        }

        a = ((2.0 * rpa) + gpa + ((1.0 / 20.0) * bpa) - 0.305) * vc.Nbb;

        i.J = 100.0 * Pow(a / vc.Aw, vc.c * vc.z);

        et = (1.0 / 4.0) * (Cos(((i.h * PI) / 180.0) + 2.0) + 3.8);
        t = ((50000.0 / 13.0) * vc.Nc * vc.Ncb * et * Sqrt((ca * ca) + (cb * cb))) / (rpa + gpa + (21.0 / 20.0) * bpa);

        i.C = Pow(t, 0.9) * Sqrt(i.J / 100.0) * Pow(1.64 - Pow(0.29, vc.n), 0.73);

        i.Q = (4.0 / vc.c) * Sqrt(i.J / 100.0) * (vc.Aw + 4.0) * Pow(vc.FL, 0.25);

        i.M = i.C * Pow(vc.FL, 0.25);

        i.s = 100.0 * Sqrt(i.M / i.Q);

        i.aC = i.C * Cos(i.h * PI / 180.0);
        i.bC = i.C * Sin(i.h * PI / 180.0);

        i.aM = i.M * Cos(i.h * PI / 180.0);
        i.bM = i.M * Sin(i.h * PI / 180.0);

        i.aS = i.s * Cos(i.h * PI / 180.0);
        i.bS = i.s * Sin(i.h * PI / 180.0);
        return (i);
    }

    /// <summary>(🞩) <see cref="CAM02"/> > <see cref="XYZ"/></summary>
    public override void To(out XYZ result, WorkingProfile profile)
    {
        var input = this;

        var vc = profile.ViewingConditions;

        double r, g, b;
        double rw = 0, gw = 0, bw = 0;
        double rc = 0, gc = 0, bc = 0;
        double rp, gp, bp;
        double rpa = 0, gpa = 0, bpa = 0;
        double a, ca, cb;
        double et, t;
        double p1, p2, p3, p4, p5, hr;
        double tx = 0, ty = 0, tz = 0;

        var rgbw = LMS.Transform.CAT02 * new Vector(profile.White.X * 100, profile.White.Y * 100, profile.White.Z * 100);
        rw = rgbw[0]; gw = rgbw[1]; bw = rgbw[2];

        t = Pow(input.C / (Sqrt(input.J / 100.0) * Pow(1.64 - Pow(0.29, vc.n), 0.73)), (1.0 / 0.9));
        et = (1.0 / 4.0) * (Cos(((input.h * PI) / 180.0) + 2.0) + 3.8);

        a = Pow(input.J / 100.0, 1.0 / (vc.c * vc.z)) * vc.Aw;

        p1 = ((50000.0 / 13.0) * vc.Nc * vc.Ncb) * et / t;
        p2 = (a / vc.Nbb) + 0.305;
        p3 = 21.0 / 20.0;

        hr = (input.h * PI) / 180.0;

        if (Abs(Sin(hr)) >= Abs(Cos(hr)))
        {
            p4 = p1 / Sin(hr);
            cb = (p2 * (2.0 + p3) * (460.0 / 1403.0)) /
                (p4 + (2.0 + p3) * (220.0 / 1403.0) *
                (Cos(hr) / Sin(hr)) - (27.0 / 1403.0) +
                p3 * (6300.0 / 1403.0));
            ca = cb * (Cos(hr) / Sin(hr));
        }
        else
        {
            p5 = p1 / Cos(hr);
            ca = (p2 * (2.0 + p3) * (460.0 / 1403.0)) /
                    (p5 + (2.0 + p3) * (220.0 / 1403.0) -
                    ((27.0 / 1403.0) - p3 * (6300.0 / 1403.0)) *
                    (Sin(hr) / Cos(hr)));
            cb = ca * (Sin(hr) / Cos(hr));
        }

        var rgbpa = Aab_RGB * new Vector((a / vc.Nbb) + 0.305, ca, cb);
        rpa = rgbpa[0]; gpa = rgbpa[1]; bpa = rgbpa[2];

        rp = NonlinearAdaptationInverse(rpa, vc.FL);
        gp = NonlinearAdaptationInverse(gpa, vc.FL);
        bp = NonlinearAdaptationInverse(bpa, vc.FL);

        var xyzt = HPE_XYZ * new Vector(rp, gp, bp);
        tx = xyzt[0]; ty = xyzt[1]; tz = xyzt[2];

        var rgbc = LMS.Transform.CAT02 * new Vector(tx, ty, tz);
        rc = rgbc[0]; gc = rgbc[1]; bc = rgbc[2];

        r = rc / (((profile.White.Y * 100 * vc.D) / rw) + (1.0 - vc.D));
        g = gc / (((profile.White.Y * 100 * vc.D) / gw) + (1.0 - vc.D));
        b = bc / (((profile.White.Y * 100 * vc.D) / bw) + (1.0 - vc.D));

        var xyz = CAT02_XYZ * new Vector(r, g, b);
        result = Colour.New<XYZ>(xyz);
    }

    #endregion
}