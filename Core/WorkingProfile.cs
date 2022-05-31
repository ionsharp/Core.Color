using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

#region (enum) WorkingProfiles

[Serializable]
public enum WorkingProfiles
{
    /// <summary>Adobe RGB (1998)</summary>
    AdobeRGB1998,
    /// <summary>Apple sRGB</summary>
    AppleRGB,
    /// <summary>Best RGB</summary>
    BestRGB,
    /// <summary>Beta RGB</summary>
    BetaRGB,
    /// <summary>Bruce RGB.</summary>
    BruceRGB,
    /// <summary>CIE RGB.</summary>
    CIERGB,
    /// <summary>CIE XYZ.</summary>
    CIEXYZ,
    /// <summary>ColorMatch RGB.</summary>
    ColorMatchRGB,
    /// <summary>DCI-P3.</summary>
    DCIP3,
    /// <summary>Display P3.</summary>
    DisplayP3,
    /// <summary>Don RGB 4.</summary>
    DonRGB4,
    /// <summary>ECI RGB v2.</summary>
    ECIRGBv2,
    /// <summary>Ekta Space PS5.</summary>
    EktaSpacePS5,
    /// <summary>HDTV.</summary>
    HDTV,
    /// <summary>MAC.</summary>
    MAC,
    /// <summary>NTSC-FCC.</summary>
    NTSCFCC,
    /// <summary>NTSC-J.</summary>
    NTSCJ,
    /// <summary>NTSC-RGB.</summary>
    NTSCRGB,
    /// <summary>PAL-M.</summary>
    PALM,
    /// <summary>PAL/SECAM RGB.</summary>
    PALSECAMRGB,
    /// <summary>ProPhoto RGB.</summary>
    ProPhotoRGB,
    /// <summary>Rec. 601 (525 lines) (ITU-R Recommendation BT.601).</summary>
    Rec601L525,
    /// <summary>Rec. 601 (625 lines) (ITU-R Recommendation BT.601).</summary>
    Rec601L625,
    /// <summary>Rec. 709 (ITU-R Recommendation BT.709).</summary>
    Rec709,
    /// <summary>Rec. 2020 (ITU-R Recommendation BT.2020).</summary>
    Rec2020,
    /// <summary>Rec. 2100 (ITU-R Recommendation BT.2100).</summary>
    Rec2100,
    /// <summary>RIMM.</summary>
    RIMM,
    /// <summary>SMPTE-C RGB.</summary>
    SMPTECRGB,
    /// <summary>
    /// <para>sRGB is a standard RGB (red, green, blue) color space that HP and Microsoft created cooperatively in 1996 to use on monitors, printers, and the World Wide Web. It was subsequently standardized by the International Electrotechnical Commission (IEC) as IEC 61966-2-1:1999. sRGB is the current defined standard colorspace for the web, and it is usually the assumed colorspace for images that are neither tagged for a colorspace nor have an embedded color profile.</para>
    /// <para>sRGB uses the same color primaries and white point as ITU-R BT.709, the standard for HDTV. However sRGB does not use the BT.709 nonlinear transfer function (sometimes informally referred to as gamma). Instead the sRGB transfer function was created for computer processing convenience, as well as being compatible with the era's CRT displays. An associated viewing environment is designed to match typical home and office viewing conditions. sRGB essentially codifies the display specifications for the computer monitors in use at that time.</para>
    /// <para>An amendment of the IEC 61966-2-1 standard document that defines sRGB includes the definition of a number of variants including sYCC, which is a Y′Cb′Cr′ luma-chroma-chroma color representation of sRGB colors with an extended range of values in the RGB domain (supporting negative values in the RGB domain).</para>
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/SRGB</remarks>
    sRGB,
    /// <summary>scRGB.</summary>
    scRGB,
    /// <summary>UHDTV.</summary>
    UHDTV,
    /// <summary>Wide Gamut RGB.</summary>
    WideGamutRGB,
}

#endregion

#region (struct) WorkingProfile

/// <summary>
/// In colorimetry, this is referred to as an "<see cref="WorkingProfile">RGB Working Space</see>". The default is <see cref="Default.sRGB"/>.
/// </summary>
[Serializable]
public struct WorkingProfile
{
    /// <summary>See <see cref="Default.sRGB"/>.</summary>
    public static Vector3<Vector2> DefaultChromacity
        => new(new(0.6400, 0.3300), new(0.3000, 0.6000), new(0.1500, 0.0600));

    /// <summary>See <see cref="Default.sRGB"/>.</summary>
    public static ICompanding DefaultCompanding 
        => new sRGBCompanding();

    /// <summary>See <see cref="Default.sRGB"/>.</summary>
    public static Vector2 DefaultWhite
        => Illuminant2.D65;

    //...

    public Vector3<Vector2> Chromacity { get; private set; } = DefaultChromacity;

    public ICompanding Companding { get; private set; } = DefaultCompanding;

    public XYZ White { get; private set; } = Illuminant.GetWhite(DefaultWhite);

    public WorkingProfile() { }

    /// <summary>See <see cref="Default"/>.</summary>
    internal WorkingProfile(XYZ white, ICompanding companding, Vector3<Vector2> position)
    {
        Companding = companding; Chromacity = position; White = white;
    }

    /// <summary>See <see cref="Default"/>.</summary>
    internal WorkingProfile(Vector2 pR, Vector2 pG, Vector2 pB, Vector2 white, ICompanding companding)
    {
        Companding = companding; Chromacity = new(pR, pG, pB); White = Illuminant.GetWhite(white);
    }

    /// <summary>Gets the <see cref="RGB"/> &lt; | > <see cref="XYZ"/> matrix.</summary>
    /// <remarks>http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html</remarks>
    public static Matrix GetRxGyBz(Vector3<Vector2> chromacity, XYZ white)
    {
        double
            xr = chromacity.R.X,
            xg = chromacity.G.X,
            xb = chromacity.B.X,
            yr = chromacity.R.Y,
            yg = chromacity.G.Y,
            yb = chromacity.B.Y;

        var Xr = xr / yr;
        const double Yr = 1;
        var Zr = (1 - xr - yr) / yr;

        var Xg = xg / yg;
        const double Yg = 1;
        var Zg = (1 - xg - yg) / yg;

        var Xb = xb / yb;
        const double Yb = 1;
        var Zb = (1 - xb - yb) / yb;

        Matrix S = new double[][]
        {
            new[] { Xr, Xg, Xb },
            new[] { Yr, Yg, Yb },
            new[] { Zr, Zg, Zb },
        };

        var W = white.Value;
        var SW = S.Invert().Value.Multiply(W);

        var Sr = SW[0]; var Sg = SW[1]; var Sb = SW[2];

        Matrix M = new double[][]
        {
            new[] { Sr * Xr, Sg * Xg, Sb * Xb },
            new[] { Sr * Yr, Sg * Yg, Sb * Yb },
            new[] { Sr * Zr, Sg * Zg, Sb * Zb },
        };
        return M;
    }

    #region (class) Default

    /// <summary>
    /// Using the chromaticities and reference whites of common RGB working spaces, I have done the math for you to compute the RGB-to-XYZ and XYZ-to-RGB matrices. Note that these matrices are given relative to their own reference whites. If you examine the matrices for these working spaces found inside ICC profiles (through the redColorantTag, greenColorantTag and blueColorantTag), those matrices will always be relative to D50, and therefore, the colorants have been subjected to a chromatic adaptation transformation if the working space reference white is not also D50.
    /// </summary>
    /// <remarks>http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html</remarks>
    /// <remarks>https://github.com/tompazourek/Colourful</remarks>
    public static class Default
    {
        /// <summary>Adobe RGB (1998)</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        public static WorkingProfile AdobeRGB1998 
            => new(new(0.640, 0.330), new(0.210, 0.710), new(0.150, 0.060), Illuminant2.D65, new GammaCompanding(2.2));

        /// <summary>Apple RGB</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        public static WorkingProfile AppleRGB 
            => new(new(0.625, 0.340), new(0.280, 0.595), new(0.155, 0.070), Illuminant2.D65, new GammaCompanding(1.8));

        /// <summary>Best RGB</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        public static WorkingProfile BestRGB 
            => new(new(0.7347, 0.2653), new(0.2150, 0.7750), new(0.1300, 0.0350), Illuminant2.D50, new GammaCompanding(2.2));

        /// <summary>Beta RGB</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        public static WorkingProfile BetaRGB
            => new(new(0.6888, 0.3112), new(0.1986, 0.7551), new(0.1265, 0.0352), Illuminant2.D50, new GammaCompanding(2.2));

        /// <summary>Bruce RGB</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        public static WorkingProfile BruceRGB 
            => new(new(0.640, 0.330), new(0.280, 0.650), new(0.150, 0.060), Illuminant2.D65, new GammaCompanding(2.2));

        /// <summary>CIE-RGB</summary><remarks><see cref="Illuminant.E"/></remarks>
        public static WorkingProfile CIERGB 
            => new(new(0.735, 0.265), new(0.274, 0.717), new(0.167, 0.009), Illuminant.E,  new GammaCompanding(2.2));

        /// <summary>CIE-XYZ</summary><remarks><see cref="Illuminant.E"/></remarks>
        [Unfinished]
        public static WorkingProfile CIEXYZ
            => new(new(1.000, 0.000), new(0.000, 1.000), new(0.000, 0.000), Illuminant.E, new GammaCompanding(2.2));

        /// <summary>ColorMatch RGB</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        public static WorkingProfile ColorMatchRGB
            => new(new(0.630, 0.340), new(0.295, 0.605), new(0.150, 0.075), Illuminant2.D50, new GammaCompanding(1.8));

        /// <summary>DCI-P3</summary><remarks><see cref="Illuminant2.D63"/></remarks>
        public static WorkingProfile DCIP3
            => new(new(0.680, 0.320), new(0.265, 0.690), new(0.150, 0.060), Illuminant2.D63, new GammaCompanding(2.6));

        /// <summary>Display P3</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        public static WorkingProfile DisplayP3
            => new(new(0.680, 0.320), new(0.265, 0.690), new(0.150, 0.060), Illuminant2.D65, new GammaCompanding(2.2));

        /// <summary>Don RGB 4</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        public static WorkingProfile DonRGB4 
            => new(new(0.696, 0.300), new(0.215, 0.765), new(0.130, 0.035), Illuminant2.D50, new GammaCompanding(2.2));

        /// <summary>ECI RGB</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        public static WorkingProfile ECIRGBv2 
            => new(new(0.670, 0.330), new(0.210, 0.710), new(0.140, 0.080), Illuminant2.D50, new LCompanding());

        /// <summary>Ekta Space PS5</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        public static WorkingProfile EktaSpacePS5 
            => new(new(0.695, 0.305), new(0.260, 0.700), new(0.110, 0.005), Illuminant2.D50, new GammaCompanding(2.2));

        /// <summary>HDTV</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        public static WorkingProfile HDTV
            => new(new(0.640, 0.330), new(0.300, 0.600), new(0.150, 0.060), Illuminant2.D65, new GammaCompanding(2.4));

        /// <summary>MAC</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        public static WorkingProfile MAC
            => new(new(0.670, 0.330), new(0.210, 0.710), new(0.140, 0.080), Illuminant2.D65, new GammaCompanding(2.8));

        /// <summary>NTSC-J</summary><remarks><see cref="Illuminant2.D93"/></remarks>
        [Unfinished]
        public static WorkingProfile NTSCJ
            => new(new(0.630, 0.340), new(0.310, 0.595), new(0.155, 0.070), Illuminant2.D93, DefaultCompanding);

        /// <summary>NTSC-FCC</summary><remarks><see cref="Illuminant2.C"/></remarks>
        public static WorkingProfile NTSCFCC
            => new(new(0.670, 0.330), new(0.210, 0.710), new(0.140, 0.080), Illuminant2.C, new GammaCompanding(2.5));

        /// <summary>NTSC-RGB</summary><remarks><see cref="Illuminant2.C"/></remarks>
        public static WorkingProfile NTSCRGB 
            => new(new(0.670, 0.330), new(0.210, 0.710), new(0.140, 0.080), Illuminant2.C,   new GammaCompanding(2.2));

        /// <summary>PAL-M</summary><remarks><see cref="Illuminant2.C"/></remarks>
        public static WorkingProfile PALM
            => new(new(0.670, 0.330), new(0.210, 0.710), new(0.140, 0.080), Illuminant2.C, new GammaCompanding(2.2));

        /// <summary>PAL/SECAM RGB</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        public static WorkingProfile PALSECAMRGB 
            => new(new(0.640, 0.330), new(0.290, 0.600), new(0.150, 0.060), Illuminant2.D65, new GammaCompanding(2.2));

        /// <summary>ProPhoto RGB</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        public static WorkingProfile ProPhotoRGB 
            => new(new(0.7347, 0.2653), new(0.1596, 0.8404), new(0.0366, 0.0001), Illuminant2.D50, new GammaCompanding(1.8));

        /// <summary>Rec. 601 (525 lines)</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        public static WorkingProfile Rec601L5
            => new(new(0.630, 0.340), new(0.310, 0.595), new(0.155, 0.070), Illuminant2.D65, new Rec601Companding());

        /// <summary>Rec. 601 (625 lines)</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        public static WorkingProfile Rec601L6
            => new(new(0.640, 0.330), new(0.290, 0.600), new(0.150, 0.060), Illuminant2.D65, new Rec601Companding());

        /// <summary>Rec. 709</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        public static WorkingProfile Rec709
            => new(new(0.640, 0.330), new(0.300, 0.600), new(0.150, 0.060), Illuminant2.D65, new Rec709Companding());

        /// <summary>Rec. 2020</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        public static WorkingProfile Rec2020
            => new(new(0.708, 0.292), new(0.170, 0.797), new(0.131, 0.046), Illuminant2.D65, new Rec2020Companding());

        /// <summary>Rec. 2100</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        public static WorkingProfile Rec2100
            => new(new(0.708, 0.292), new(0.170, 0.797), new(0.131, 0.046), Illuminant2.D65, new Rec2100Companding());

        /// <summary>RIMM</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        public static WorkingProfile RIMM
            => new(new(0.7347, 0.2653), new(0.1596, 0.8404), new(0.0366, 0.0001), Illuminant2.D50, new GammaCompanding(2.222));

        /// <summary>scRGB</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [Unfinished]
        public static WorkingProfile scRGB
            => new(new(0.640, 0.330), new(0.300, 0.600), new(0.1500, 0.060), Illuminant2.D65, DefaultCompanding);

        /// <summary>SMPTE-C-RGB</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        public static WorkingProfile SMPTECRGB 
            => new(new(0.630, 0.340), new(0.310, 0.595), new(0.155, 0.070), Illuminant2.D65, new GammaCompanding(2.2));

        /// <summary>sRGB</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        public static WorkingProfile sRGB 
            => new(DefaultChromacity.X, DefaultChromacity.Y, DefaultChromacity.Z, DefaultWhite, DefaultCompanding);

        /// <summary>UHDTV</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        public static WorkingProfile UHDTV
            => new(new(0.708, 0.292), new(0.170, 0.797), new(0.131, 0.046), Illuminant2.D65, new GammaCompanding(2.4));

        /// <summary>Wide Gamut RGB</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        public static WorkingProfile WideGamutRGB 
            => new(new(0.735, 0.265), new(0.115, 0.826), new(0.157, 0.018), Illuminant2.D50, new GammaCompanding(2.2));
    }

    #endregion
}

#endregion