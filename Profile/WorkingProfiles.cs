namespace Imagin.Core.Colors;

/// <summary>
/// <para>http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html</para>
/// <para>https://github.com/tompazourek/Colourful</para>
/// </summary>
public static class WorkingProfiles
{
    enum Category { Adobe, Apple, CIE, DCI, Don, ECI, Fraser, Holmes, Imagin, ITU, Kodak, Lindbloom, Microsoft, NTSC, PAL, Radius }

    #region Adobe

    /// <summary>
    /// <b>Adobe RGB (1998)</b>
    /// <para><see cref="Illuminant2.D65"/></para>
    /// </summary>
    [Category(Category.Adobe), Name("Adobe RGB (1998)")]
    [Description("Designed to encompass most of the colors achievable on CMYK color printers, but by using RGB primary colors on a device such as a computer display.")]
    public static WorkingProfile AdobeRGB1998
        => new(new(0.640, 0.330), new(0.210, 0.710), new(0.150, 0.060), Illuminant2.D65, new GammaCompression(2.2));

    /// <summary>
    /// <b>Wide Gamut RGB</b>
    /// <para><see cref="Illuminant2.D50"/></para>
    /// </summary>
    [Category(Category.Adobe), Name("Wide Gamut RGB")]
    [Description("Offers a large gamut by using pure spectral primary colors.")]
    public static WorkingProfile WideGamutRGB
        => new(new(0.735, 0.265), new(0.115, 0.826), new(0.157, 0.018), Illuminant2.D50, new GammaCompression(2.2));

    #endregion

    #region Apple

    /// <summary>
    /// <b>Apple RGB</b>
    /// <para><see cref="Illuminant2.D65"/></para>
    /// </summary>
    [Category(Category.Apple), Name("Apple RGB")]
    [Description("")]
    public static WorkingProfile AppleRGB
        => new(new(0.625, 0.340), new(0.280, 0.595), new(0.155, 0.070), Illuminant2.D65, new GammaCompression(1.8));

    /// <summary>
    /// <b>Cinema Gamut</b>
    /// <para><see cref="Illuminant2.D65"/></para>
    /// </summary>
    [Category(Category.Apple), Name("Cinema Gamut")]
    [Description("")]
    public static WorkingProfile CinemaGamut
        => new(new(0.740, 0.270), new(0.170, 1.140), new(0.080, -0.100), Illuminant2.D65, new GammaCompression(2.6));

    /// <summary>
    /// <b>P3-D65 (Display)</b>
    /// <para><see cref="Illuminant2.D65"/></para>
    /// </summary>
    [Category(Category.Apple), Name("P3-D65 (Display)")]
    [Description("")]
    public static WorkingProfile Display_P3
        => new(new(0.680, 0.320), new(0.265, 0.690), new(0.150, 0.060), Illuminant2.D65, new Compression(12 / 5, 0.055, 0.0031308, 12.92, 0.04045));

    #endregion

    #region (Bruce) Lindbloom

    /// <summary>
    /// <b>Beta RGB</b>
    /// <para><see cref="Illuminant2.D50"/></para>
    /// <para>An optimized capture, archiving, and editing space for high-end digital imaging applications.</para>
    /// </summary>
    [Category(Category.Lindbloom), Name("Beta RGB")]
    [Description("An optimized capture, archiving, and editing space for high-end digital imaging applications.")]
    public static WorkingProfile BetaRGB
        => new(new(0.6888, 0.3112), new(0.1986, 0.7551), new(0.1265, 0.0352), Illuminant2.D50, new GammaCompression(2.2));

    #endregion

    #region (Bruce) Fraser

    /// <summary>
    /// <b>Bruce RGB</b>
    /// <para><see cref="Illuminant2.D65"/></para>
    /// <para>A conservative-gamut space for dealing with 8-bit imagery that needs heavy editing.</para>
    /// </summary>
    [Category(Category.Fraser), Name("Bruce RGB")]
    [Description("A conservative-gamut space for dealing with 8-bit imagery that needs heavy editing.")]
    public static WorkingProfile BruceRGB
        => new(new(0.640, 0.330), new(0.280, 0.650), new(0.150, 0.060), Illuminant2.D65, new GammaCompression(2.2));

    #endregion

    #region CIE

    /// <summary>
    /// <b>CIE-RGB</b>
    /// <para><see cref="Illuminant.E"/></para>
    /// </summary>
    [Category(Category.CIE), Name("CIE-RGB")]
    [Description("")]
    public static WorkingProfile CIE_RGB
        => new(new(0.735, 0.265), new(0.274, 0.717), new(0.167, 0.009), Illuminant.E, new GammaCompression(2.2));

    /// <summary>
    /// <b>CIE-XYZ</b>
    /// <para><see cref="Illuminant.E"/></para>
    /// </summary>
    [Category(Category.CIE), Name("CIE-XYZ"), Incomplete]
    [Description("")]
    public static WorkingProfile CIE_XYZ
        => new(new(1.000, 0.000), new(0.000, 1.000), new(0.000, 0.000), Illuminant.E, new GammaCompression(2.2));

    #endregion

    #region DCI

    /// <summary>
    /// <b>P3-D60 (ACES Cinema)</b>
    /// <para><see cref="Illuminant2.D60"/></para>
    /// </summary>
    [Category(Category.DCI), Name("P3-D60 (ACES Cinema)")]
    [Description("")]
    public static WorkingProfile Cinema_P3
        => new(new(0.680, 0.320), new(0.265, 0.690), new(0.150, 0.060), Illuminant2.D60, new GammaCompression(2.6));

    /// <summary>
    /// <b>DCI-P3+</b>
    /// <para><see cref="Illuminant2.D63"/></para>
    /// </summary>
    [Category(Category.DCI), Name("DCI-P3+")]
    [Description("")]
    public static WorkingProfile DCI_P3Plus
        => new(new(0.740, 0.270), new(0.220, 0.780), new(0.090, -0.090), Illuminant2.D63, new GammaCompression(2.6));

    /// <summary>
    /// <b>P3-DCI (Theater)</b>
    /// <para><see cref="Illuminant2.D63"/></para>
    /// </summary>
    [Category(Category.DCI), Name("P3-DCI (Theater)")]
    [Description("")]
    public static WorkingProfile Theater_P3
        => new(new(0.680, 0.320), new(0.265, 0.690), new(0.150, 0.060), Illuminant2.D63, new GammaCompression(2.6));

    #endregion

    #region Don (Hutcheson)

    /// <summary>
    /// <b>Best RGB</b>
    /// <para><see cref="Illuminant2.D50"/></para>
    /// </summary>
    [Category(Category.Don), Name("Best RGB")]
    [Description("")]
    public static WorkingProfile BestRGB
        => new(new(0.7347, 0.2653), new(0.2150, 0.7750), new(0.1300, 0.0350), Illuminant2.D50, new GammaCompression(2.2));

    /// <summary>
    /// <b>Don RGB 4</b>
    /// <para><see cref="Illuminant2.D50"/></para>
    /// </summary>
    [Category(Category.Don), Name("Don RGB 4")]
    [Description("")]
    public static WorkingProfile DonRGB4
        => new(new(0.696, 0.300), new(0.215, 0.765), new(0.130, 0.035), Illuminant2.D50, new GammaCompression(2.2));

    #endregion

    #region ECI

    /// <summary>
    /// <b>eciRGB</b>
    /// <para><see cref="Illuminant2.D50"/></para>
    /// </summary>
    [Category(Category.ECI), Name("eciRGB")]
    [Description("")]
    public static WorkingProfile eciRGB
        => new(new(0.670, 0.330), new(0.210, 0.710), new(0.140, 0.080), Illuminant2.D50, new Compression(3, 1.16, 0.008856, 9.033, 0.08));

    #endregion

    #region Imagin

    /// <summary>
    /// <b>sRGB-HLG</b>
    /// <para><see cref="Illuminant2.D65"/></para>
    /// <para><see cref="sRGB"/> with hybrid log-gamma (HLG) compression.</para>
    /// </summary>
    [Category(Category.Imagin), Name("sRGB-HLG")]
    [Description("sRGB with hybrid log-gamma (HLG) compression.")]
    public static WorkingProfile sRGB_HLG
        => new(WorkingProfile.sRGBPrimary, WorkingProfile.sRGBWhite, new GammaLogCompression());

    /// <summary>
    /// <b>sRGB-PQ</b>
    /// <para><see cref="Illuminant2.D65"/></para>
    /// <para><see cref="sRGB"/> with perceptual quantization (PQ) compression.</para>
    /// </summary>
    [Category(Category.Imagin), Name("sRGB-PQ")]
    [Description("sRGB with perceptual quantization (PQ) compression.")]
    public static WorkingProfile sRGB_PQ
        => new(WorkingProfile.sRGBPrimary, WorkingProfile.sRGBWhite, new PQCompression());

    #endregion

    #region ITU

    /// <summary>
    /// <b>HDTV</b>
    /// <para><i>ITU-R BT.709</i> (Rec. 709)</para>
    /// <para><see cref="Illuminant2.D65"/></para>
    /// </summary>
    [Category(Category.ITU), Name("HDTV")]
    [Description("")]
    public static WorkingProfile HDTV
        => new(new(0.640, 0.330), new(0.300, 0.600), new(0.150, 0.060), Illuminant2.D65, new Compression(20 / 9, 0.099, 0.004, 4.5, 0.018));

    /// <summary>
    /// <b>MAC</b>
    /// <para><see cref="Illuminant2.D65"/></para>
    /// </summary>
    [Category(Category.ITU), Name("MAC")]
    [Description("")]
    public static WorkingProfile MAC
        => new(new(0.670, 0.330), new(0.210, 0.710), new(0.140, 0.080), Illuminant2.D65, new GammaCompression(2.8));

    /// <summary>
    /// <b>UHDTV</b> (2020)
    /// <para><i>ITU-R BT.2020</i> (Rec. 2020)</para>
    /// <para><see cref="Illuminant2.D65"/></para>
    /// </summary>
    [Category(Category.ITU), Name("UHDTV (2020)")]
    [Description("")]
    public static WorkingProfile UHDTV_2020
        => new(new(0.708, 0.292), new(0.170, 0.797), new(0.131, 0.046), Illuminant2.D65, new Compression(1 / 0.45, CIE.Alpha - 1, CIE.Beta, 4.5, CIE.BetaInverse));

    /// <summary>
    /// <b>UHDTV</b> (2100)
    /// <para><i>ITU-R BT.2100</i> (Rec. 2100)</para>
    /// <para><see cref="Illuminant2.D65"/></para>
    /// </summary>
    [Category(Category.ITU), Name("UHDTV (2100)")]
    [Description("")]
    public static WorkingProfile UHDTV_2100
        => new(new(0.708, 0.292), new(0.170, 0.797), new(0.131, 0.046), Illuminant2.D65, new PQCompression() /*|| new GammaLogCompression()*/);

    #endregion

    #region (Joseph) Holmes

    /// <summary>Ekta Space PS5</summary><remarks><see cref="Illuminant2.D50"/></remarks>
    [Category(Category.Holmes), Name("Ekta Space PS5")]
    [Description("Developed for high quality storage of image data from scans of transparencies.")]
    public static WorkingProfile EktaSpacePS5
        => new(new(0.695, 0.305), new(0.260, 0.700), new(0.110, 0.005), Illuminant2.D50, new GammaCompression(2.2));

    #endregion

    #region Kodak

    /// <summary>RIMM</summary><remarks><see cref="Illuminant2.D50"/></remarks>
    [Category(Category.Kodak), Name("RIMM")]
    [Description("")]
    public static WorkingProfile RIMM
        => new(new(0.7347, 0.2653), new(0.1596, 0.8404), new(0.0366, 0.0001), Illuminant2.D50, new Compression(20 / 9, 0.099, 0.0018, 5.5, 0.099));

    /// <summary>ROMM (ProPhoto)</summary><remarks><see cref="Illuminant2.D50"/></remarks>
    [Category(Category.Kodak), Name("ROMM (ProPhoto)")]
    [Description("Offers an especially large gamut designed for use with photographic output in mind.")]
    public static WorkingProfile ROMM
        => new(new(0.7347, 0.2653), new(0.1596, 0.8404), new(0.0366, 0.0001), Illuminant2.D50, new Compression(9 / 5, 0, 0.001953125, 16, 0.031248));

    #endregion

    #region NTSC

    /// <summary>
    /// <b>NTSC-J</b>
    /// <para><see cref="Illuminant2.D93"/></para>
    /// </summary>
    [Category(Category.NTSC), Name("NTSC-J"), Incomplete]
    [Description("")]
    public static WorkingProfile NTSC_J
        => new(new(0.630, 0.340), new(0.310, 0.595), new(0.155, 0.070), Illuminant2.D93, WorkingProfile.sRGBCompression);

    /// <summary>
    /// <b>NTSC-FCC</b>
    /// <para><see cref="Illuminant2.C"/></para>
    /// </summary>
    [Category(Category.NTSC), Name("NTSC-FCC")]
    [Description("")]
    public static WorkingProfile NTSC_FCC
        => new(new(0.670, 0.330), new(0.210, 0.710), new(0.140, 0.080), Illuminant2.C, new GammaCompression(2.5));

    /// <summary>
    /// <b>NTSC-RGB</b>
    /// <para><see cref="Illuminant2.C"/></para>
    /// </summary>
    [Category(Category.NTSC), Name("NTSC-RGB")]
    [Description("")]
    public static WorkingProfile NTSC_RGB
        => new(new(0.670, 0.330), new(0.210, 0.710), new(0.140, 0.080), Illuminant2.C, new GammaCompression(2.2));

    /// <summary>
    /// <b>NTSC-SMPTE (C)</b>
    /// <para><i>SMPTE RP 145 (C), 170M, 240M</i> (1987)</para>
    /// <para><see cref="Illuminant2.D65"/></para>
    /// <para>Rec. 601 (525 lines)...?</para>
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/NTSC#SMPTE_C</remarks>
    [Category(Category.NTSC), Name("NTSC-SMPTE (C)")]
    [Description("")]
    public static WorkingProfile NTSC_SMPTE_C
        => new(new(0.630, 0.340), new(0.310, 0.595), new(0.155, 0.070), Illuminant2.D65, new Compression(20 / 9, 0.1115, 0.0057, 4, 0.0228));

    #endregion

    #region PAL

    /// <summary>
    /// <b>PAL-M</b>
    /// <para><i>BT.470-6</i> (1972)</para>
    /// <para><see cref="Illuminant2.C"/></para>
    /// </summary>
    [Category(Category.PAL), Name("PAL-M")]
    [Description("")]
    public static WorkingProfile PAL_M
        => new(new(0.670, 0.330), new(0.210, 0.710), new(0.140, 0.080), Illuminant2.C, new GammaCompression(2.2));

    /// <summary>
    /// <b>PAL/SECAM</b>
    /// <para><i>EBU 3213-E, ITU-R BT.470/601 (B/G)</i> (1970)</para>
    /// <para><see cref="Illuminant2.D65"/></para>
    /// <para>Rec. 601 (625 lines)...?</para>
    /// Gamma: <see href="https://github.com/tompazourek/Colourful/">Colourful</see> = 2.2/<see href="https://en.wikipedia.org/wiki/RGB_color_spaces">Wikipedia</see> = 2.8.
    /// </summary>
    [Category(Category.PAL), Name("PAL/SECAM")]
    [Description("")]
    public static WorkingProfile PAL_SECAM
        => new(new(0.640, 0.330), new(0.290, 0.600), new(0.150, 0.060), Illuminant2.D65, new GammaCompression(2.8));

    #endregion

    #region Microsoft

    /// <summary>
    /// <b>scRGB</b>
    /// <para><see cref="Illuminant2.D65"/></para>
    /// <para>Uses the same color primaries and white/black points as <see cref="sRGB"/>, but allows coordinates below zero and greater than one.</para>
    /// </summary>
    [Category(Category.Microsoft), Name("scRGB"), Incomplete]
    [Description("Uses the same color primaries and white/black points as sRGB, but allows coordinates below zero and greater than one.")]
    public static WorkingProfile scRGB
        => new(WorkingProfile.sRGBPrimary, WorkingProfile.sRGBWhite, WorkingProfile.sRGBCompression);

    /// <summary>
    /// <b>sRGB</b>
    /// <para><see cref="Illuminant2.D65"/></para>
    /// <para>Developed as a color standard designed primarily for office, home, and web users.</para>
    /// </summary>
    [Category(Category.Microsoft), Name("sRGB")]
    [Description("Developed as a color standard designed primarily for office, home, and web users.")]
    public static WorkingProfile sRGB
        => new(WorkingProfile.sRGBPrimary, WorkingProfile.sRGBWhite, WorkingProfile.sRGBCompression);

    #endregion

    #region Radius

    /// <summary>
    /// <b>ColorMatch RGB</b>
    /// <para><see cref="Illuminant2.D50"/></para>
    /// </summary>
    [Category(Category.Radius), Name("ColorMatch RGB")]
    [Description("")]
    public static WorkingProfile ColorMatchRGB
        => new(new(0.630, 0.340), new(0.295, 0.605), new(0.150, 0.075), Illuminant2.D50, new GammaCompression(1.8));

    #endregion
}