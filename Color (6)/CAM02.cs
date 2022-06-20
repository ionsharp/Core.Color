using Imagin.Core.Numerics;
using System;
using static Imagin.Core.Numerics.M;
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
public abstract class CAM02 : ColorModel3<XYZ>
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
        /// <summary>Viewing surface colors.</summary>
        Average,
        /// <summary>Using a projector in a dark room.</summary>
        Dark,
        /// <summary>Viewing television.</summary>
        Dim
    }

    #endregion

    #region (struct) ViewingConditions

    /// <summary><see cref="CAM02"/> viewing conditions.</summary>
    public struct ViewingConditions
    {
        public double xw, yw, zw, aw;

        public double la, yb;

        public int surround;

        public double n, z, f, c, nbb, nc, ncb, fl, d;
    };

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

    /// <summary>
    /// Theoretically, <b>D</b> ranges from
    /// 
    /// <para>0 = <b>No adaptation to the adopted white point.</b></para>
    /// <para>1 = <b>Complete adaptation to the adopted white point.</b></para>
    /// 
    /// <para>In practice, the minimum <b>D</b> value will not be less than 0.65 for <see cref="Surrounds.Dark"/> and exponentially converges to 1 for <see cref="Surrounds.Average"/> with increasingly large values of L[A].</para>
    /// 
    /// <para>L[A] is the luminance of the adapting field in cd/m^2.</para>
    /// </summary>
    static double Compute_d(ViewingConditions conditions)
        => (conditions.f * (1.0 - ((1.0 / 3.6) * Exp((-conditions.la - 42.0) / 92.0))));

    static double Compute_fl(ViewingConditions conditions)
    {
        double k, fl;
        k = 1.0 / ((5.0 * conditions.la) + 1.0);
        fl = 0.2 * Pow(k, 4.0) * (5.0 * conditions.la) + 0.1 * (Pow((1.0 - Pow(k, 4.0)), 2.0)) * (Pow((5.0 * conditions.la), (1.0 / 3.0)));
        return (fl);
    }

    static double Compute_fl_from_la_cam02(double la)
    {
        double la5 = la * 5.0;
        double k = 1.0 / (la5 + 1.0);

        k = Pow4(k);
        return (0.2 * k * la5) + (0.1 * (1.0 - k) * (1.0 - k) * Pow(la5, 1.0 / 3.0));
    }

    static double Compute_n(ViewingConditions conditions) 
        => conditions.yb / conditions.yw;

    static double Compute_z(ViewingConditions conditions)
        => 1.48 + Pow(conditions.n, 0.5);

    static double Compute_nbb(ViewingConditions conditions)
        => 0.725 * Pow(1.0 / conditions.n, 0.2);

    //...

    static double nonlinear_adaptation(double c, double fl)
    {
        double p = Pow((fl * c) / 100.0, 0.42);
        return ((400.0 * p) / (27.13 + p)) + 0.1;
    }

    static double inverse_nonlinear_adaptation(double c, double fl)
        => (100.0 / fl) * Pow((27.13 * Abs(c - 0.1)) / (400.0 - Abs(c - 0.1)), 1.0 / 0.42);

    //...

    static double achromatic_response_to_white(ViewingConditions conditions)
    {
        double r = 0, g = 0, b = 0;
        double rc, gc, bc;
        double rp = 0, gp = 0, bp = 0;
        double rpa, gpa, bpa;

        var rgb = LMS.Transform.CAT02 * new Vector(conditions.xw, conditions.yw, conditions.zw);
        r = rgb[0]; g = rgb[1]; b = rgb[2];
        
        rc = r * (((conditions.yw * conditions.d) / r) + (1.0 - conditions.d));
        gc = g * (((conditions.yw * conditions.d) / g) + (1.0 - conditions.d));
        bc = b * (((conditions.yw * conditions.d) / b) + (1.0 - conditions.d));

        var rgbp = CAT02_HPE * new Vector(rc, gc, bc);
        rp = rgbp[0]; gp = rgbp[1]; bp = rgbp[2];

        rpa = nonlinear_adaptation(rp, conditions.fl);
        gpa = nonlinear_adaptation(gp, conditions.fl);
        bpa = nonlinear_adaptation(bp, conditions.fl);

        return ((2.0 * rpa) + gpa + ((1.0 / 20.0) * bpa) - 0.305) * conditions.nbb;
    }

    //...

    /// <summary>(🞩) <see cref="XYZ"/> > <see cref="CAM02"/></summary>
    protected T From<T>(XYZ input, ViewingConditions conditions) where T : CAM02, new()
    {
        double r = 0, g = 0, b = 0;
        double rw = 0, gw = 0, bw = 0;
        double rc, gc, bc;
        double rp = 0, gp = 0, bp = 0;
        double rpa, gpa, bpa;
        double a, ca, cb;
        double et, t, temp;

        var rgb = LMS.Transform.CAT02 * new Vector(input.X, input.Y, input.Z);
        r = rgb[0]; g = rgb[1]; b = rgb[2];

        var rgbw = LMS.Transform.CAT02 * new Vector(conditions.xw, conditions.yw, conditions.zw);
        rw = rgbw[0]; gw = rgbw[1]; bw = rgbw[2];

        rc = r * (((conditions.yw * conditions.d) / rw) + (1.0 - conditions.d));
        gc = g * (((conditions.yw * conditions.d) / gw) + (1.0 - conditions.d));
        bc = b * (((conditions.yw * conditions.d) / bw) + (1.0 - conditions.d));

        var rgbp = CAT02_HPE * new Vector(rc, gc, bc);
        rp = rgbp[0]; gp = rgbp[1]; bp = rgbp[2];

        rpa = nonlinear_adaptation(rp, conditions.fl);
        gpa = nonlinear_adaptation(gp, conditions.fl);
        bpa = nonlinear_adaptation(bp, conditions.fl);

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

        a = ((2.0 * rpa) + gpa + ((1.0 / 20.0) * bpa) - 0.305) * conditions.nbb;

        i.J = 100.0 * Pow(a / conditions.aw, conditions.c * conditions.z);

        et = (1.0 / 4.0) * (Cos(((i.h * PI) / 180.0) + 2.0) + 3.8);
        t = ((50000.0 / 13.0) * conditions.nc * conditions.ncb * et * Sqrt((ca * ca) + (cb * cb))) / (rpa + gpa + (21.0 / 20.0) * bpa);

        i.C = Pow(t, 0.9) * Sqrt(i.J / 100.0) * Pow(1.64 - Pow(0.29, conditions.n), 0.73);

        i.Q = (4.0 / conditions.c) * Sqrt(i.J / 100.0) * (conditions.aw + 4.0) * Pow(conditions.fl, 0.25);

        i.M = i.C * Pow(conditions.fl, 0.25);

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

        ViewingConditions conditions = new();

        double r, g, b;
        double rw = 0, gw = 0, bw = 0;
        double rc = 0, gc = 0, bc = 0;
        double rp, gp, bp;
        double rpa = 0, gpa = 0, bpa = 0;
        double a, ca, cb;
        double et, t;
        double p1, p2, p3, p4, p5, hr;
        double tx = 0, ty = 0, tz = 0;

        var rgbw = LMS.Transform.CAT02 * new Vector(conditions.xw, conditions.yw, conditions.zw);
        rw = rgbw[0]; gw = rgbw[1]; bw = rgbw[2];

        t = Pow(input.C / (Sqrt(input.J / 100.0) * Pow(1.64 - Pow(0.29, conditions.n), 0.73)), (1.0 / 0.9));
        et = (1.0 / 4.0) * (Cos(((input.h * PI) / 180.0) + 2.0) + 3.8);

        a = Pow(input.J / 100.0, 1.0 / (conditions.c * conditions.z)) * conditions.aw;

        p1 = ((50000.0 / 13.0) * conditions.nc * conditions.ncb) * et / t;
        p2 = (a / conditions.nbb) + 0.305;
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

        var rgbpa = Aab_RGB * new Vector((a / conditions.nbb) + 0.305, ca, cb);
        rpa = rgbpa[0]; gpa = rgbpa[1]; bpa = rgbpa[2];

        rp = inverse_nonlinear_adaptation(rpa, conditions.fl);
        gp = inverse_nonlinear_adaptation(gpa, conditions.fl);
        bp = inverse_nonlinear_adaptation(bpa, conditions.fl);

        var xyzt = HPE_XYZ * new Vector(rp, gp, bp);
        tx = xyzt[0]; ty = xyzt[1]; tz = xyzt[2];

        var rgbc = LMS.Transform.CAT02 * new Vector(tx, ty, tz);
        rc = rgbc[0]; gc = rgbc[1]; bc = rgbc[2];

        r = rc / (((conditions.yw * conditions.d) / rw) + (1.0 - conditions.d));
        g = gc / (((conditions.yw * conditions.d) / gw) + (1.0 - conditions.d));
        b = bc / (((conditions.yw * conditions.d) / bw) + (1.0 - conditions.d));

        var xyz = CAT02_XYZ * new Vector(r, g, b);
        result = Colour.New<XYZ>(xyz);
    }

    /*
    int main (int argc, char** argv) 
    {
        int mode, verbose, setD, i, samples;
        char temp_char;

        struct CIECAM02vc myVC;
        struct CIECAM02color myColor;

        FILE *myViewingConditions, *myInput, *myOutput;

        if (argc != 7) {
        printf ("\n ciecam02 mode verbose setD in.vc in.dat out.dat\n\n");
        printf ("   mode - 0 for forward and 1 for inverse.\n");
        printf ("   verbose - 0 for off and 1 for on.\n");
        printf ("   setD - 0 for compute and 1 to force to 1.\n");
        printf ("   in.vc - Xw, Yw, Zw, La, Yb and surround.\n");
        printf ("     surrounds are 1 - average, 2 - dim, and 3 - dark.\n\n");
        exit(1);
        }
        else {
        mode    = atoi(argv[1]);
        verbose = atoi(argv[2]);
        setD    = atoi(argv[3]);
        }

        if ( ((myViewingConditions  = fopen(argv[4], "r") ) == NULL) ||
            ((myInput              = fopen(argv[5], "r") ) == NULL) ||
            ((myOutput             = fopen(argv[6], "w") ) == NULL) ) {
            printf ("\n\n Cant open one of the data files. Bailing...\n\n");
        exit(1);
        }

        //Read in and compute the parameters associated with the viewing conditions.

        fscanf(myViewingConditions, "%lf", &myVC.xw);
        fscanf(myViewingConditions, "%lf", &myVC.yw);
        fscanf(myViewingConditions, "%lf", &myVC.zw);
        fscanf(myViewingConditions, "%lf", &myVC.la);
        fscanf(myViewingConditions, "%lf", &myVC.yb);
        fscanf(myViewingConditions, "%d", &myVC.surround);

        if (myVC.surround == 1) {
        //Average
        myVC.f  = 1.00;
        myVC.c  = 0.69;
        myVC.nc = 1.00;
        }
        else if (myVC.surround == 2) {
        //Dim
        myVC.f  = 0.90;
        myVC.c  = 0.59;
        myVC.nc = 0.90;
        }
        else if (myVC.surround == 3) {
        //Dark
        myVC.f  = 0.800;
        myVC.c  = 0.525;
        myVC.nc = 0.800;
        }
        else {
        printf ("\n Invalid value for the surround. Exiting.\n\n");
        exit (1);
        }

        myVC.n   = compute_n(myVC);
        myVC.z   = compute_z(myVC);
        myVC.fl  = compute_fl(myVC);
        myVC.nbb = compute_nbb(myVC);
        myVC.ncb = myVC.nbb;
        myVC.d   = compute_d(myVC);
        myVC.aw  = achromatic_response_to_white(myVC);


        if (verbose == 1) {
        fprintf (myOutput, "xw=%lf yw=%lf zw=%lf\n", myVC.xw, myVC.yw, myVC.zw);
        fprintf (myOutput, "la=%lf\n", myVC.la);
        fprintf (myOutput, "yb=%lf\n", myVC.yb);
        fprintf (myOutput, "n=%lf\n", myVC.n);
        fprintf (myOutput, "z=%lf\n", myVC.z);
        fprintf (myOutput, "fl=%lf\n", myVC.fl);
        fprintf (myOutput, "nbb=%lf\n", myVC.nbb);
        fprintf (myOutput, "ncb=%lf\n", myVC.ncb);
        fprintf (myOutput, "surround=%d  f=%lf  c=%lf  nc=%lf\n", myVC.surround, myVC.f, myVC.c, myVC.nc);
        fprintf (myOutput, "d=%lf\n", myVC.d);
        fprintf (myOutput, "aw=%lf\n", myVC.aw);
        }

        samples = 0;
        while ( (temp_char = getc(myInput)) != EOF ) {
        if (temp_char == '\n') samples++;
        }
        fseek(myInput, 0, SEEK_SET);

        if (mode == 0) {
        for (i = 0; i < samples; i++) {
            fscanf (myInput, "%lf", &myColor.x);
            fscanf (myInput, "%lf", &myColor.y);
            fscanf (myInput, "%lf", &myColor.z);

            if(verbose == 1) fprintf (myOutput, "x=%lf y=%lf z=%lf\n", myColor.x, myColor.y, myColor.z);

            myColor = forwardCIECAM02(myColor, myVC, verbose, myOutput);

            fprintf (myOutput, "%lf %lf %lf\n", myColor.J, myColor.C, myColor.h);
        }
        }
        else if (mode == 1) {
        for (i = 0; i < samples; i++) {
            fscanf (myInput, "%lf", &myColor.J);
            fscanf (myInput, "%lf", &myColor.C);
            fscanf (myInput, "%lf", &myColor.h);

            if(verbose == 1) fprintf (myOutput, "J=%lf C=%lf h=%lf\n", myColor.J, myColor.C, myColor.h);

            myColor = inverseCIECAM02(myColor, myVC, verbose, myOutput);

            fprintf (myOutput, "%lf %lf %lf\n", myColor.x, myColor.y, myColor.z);
        }
        }

        fclose(myViewingConditions);
        fclose(myInput);
        fclose(myOutput);

        return (0);
    } 
    */

    #endregion
}