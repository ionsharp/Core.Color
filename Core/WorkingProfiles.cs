namespace Imagin.Core.Colors;

    /// <summary>
    /// Using the chromaticities and reference whites of common RGB working spaces, I have done the math for you to compute the RGB-to-XYZ and XYZ-to-RGB matrices. Note that these matrices are given relative to their own reference whites. If you examine the matrices for these working spaces found inside ICC profiles (through the redColorantTag, greenColorantTag and blueColorantTag), those matrices will always be relative to D50, and therefore, the colorants have been subjected to a chromatic adaptation transformation if the working space reference white is not also D50.
    /// </summary>
    /// <remarks>http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html</remarks>
    /// <remarks>https://github.com/tompazourek/Colourful</remarks>
    public static class WorkingProfiles
    {
        /// <summary>Adobe RGB (1998)</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [DisplayName("Adobe RGB (1998)")]
        public static WorkingProfile AdobeRGB1998
            => new(new(0.640, 0.330), new(0.210, 0.710), new(0.150, 0.060), Illuminant2.D65, new GammaCompression(2.2));

        /// <summary>Apple RGB</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [DisplayName("Apple RGB")]
        public static WorkingProfile AppleRGB
            => new(new(0.625, 0.340), new(0.280, 0.595), new(0.155, 0.070), Illuminant2.D65, new GammaCompression(1.8));

        /// <summary>Best RGB</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        [DisplayName("Best RGB")]
        public static WorkingProfile BestRGB
            => new(new(0.7347, 0.2653), new(0.2150, 0.7750), new(0.1300, 0.0350), Illuminant2.D50, new GammaCompression(2.2));

        /// <summary>Beta RGB</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        [DisplayName("Beta RGB")]
        public static WorkingProfile BetaRGB
            => new(new(0.6888, 0.3112), new(0.1986, 0.7551), new(0.1265, 0.0352), Illuminant2.D50, new GammaCompression(2.2));

        /// <summary>Bruce RGB</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [DisplayName("Bruce RGB")]
        public static WorkingProfile BruceRGB
            => new(new(0.640, 0.330), new(0.280, 0.650), new(0.150, 0.060), Illuminant2.D65, new GammaCompression(2.2));

        /// <summary>CIE-RGB</summary><remarks><see cref="Illuminant.E"/></remarks>
        [DisplayName("CIE-RGB")]
        public static WorkingProfile CIERGB
            => new(new(0.735, 0.265), new(0.274, 0.717), new(0.167, 0.009), Illuminant.E, new GammaCompression(2.2));

        /// <summary>CIE-XYZ</summary><remarks><see cref="Illuminant.E"/></remarks>
        [DisplayName("CIE-XYZ"), Incomplete]
        public static WorkingProfile CIEXYZ
            => new(new(1.000, 0.000), new(0.000, 1.000), new(0.000, 0.000), Illuminant.E, new GammaCompression(2.2));

        /// <summary>ColorMatch RGB</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        [DisplayName("ColorMatch RGB")]
        public static WorkingProfile ColorMatchRGB
            => new(new(0.630, 0.340), new(0.295, 0.605), new(0.150, 0.075), Illuminant2.D50, new GammaCompression(1.8));

        /// <summary>DCI-P3</summary><remarks><see cref="Illuminant2.D63"/></remarks>
        [DisplayName("DCI-P3")]
        public static WorkingProfile DCIP3
            => new(new(0.680, 0.320), new(0.265, 0.690), new(0.150, 0.060), Illuminant2.D63, new GammaCompression(2.6));

        /// <summary>Display P3</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [DisplayName("Display P3")]
        public static WorkingProfile DisplayP3
            => new(new(0.680, 0.320), new(0.265, 0.690), new(0.150, 0.060), Illuminant2.D65, new GammaCompression(2.2));

        /// <summary>Don RGB 4</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        [DisplayName("Don RGB 4")]
        public static WorkingProfile DonRGB4
            => new(new(0.696, 0.300), new(0.215, 0.765), new(0.130, 0.035), Illuminant2.D50, new GammaCompression(2.2));

        /// <summary>ECI RGB</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        [DisplayName("ECI RGB")]
        public static WorkingProfile ECIRGBv2
            => new(new(0.670, 0.330), new(0.210, 0.710), new(0.140, 0.080), Illuminant2.D50, new LCompression());

        /// <summary>Ekta Space PS5</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        [DisplayName("Ekta Space PS5")]
        public static WorkingProfile EktaSpacePS5
            => new(new(0.695, 0.305), new(0.260, 0.700), new(0.110, 0.005), Illuminant2.D50, new GammaCompression(2.2));

        /// <summary>HDTV</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [DisplayName("HDTV")]
        public static WorkingProfile HDTV
            => new(new(0.640, 0.330), new(0.300, 0.600), new(0.150, 0.060), Illuminant2.D65, new GammaCompression(2.4));

        /// <summary>MAC</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [DisplayName("MAC")]
        public static WorkingProfile MAC
            => new(new(0.670, 0.330), new(0.210, 0.710), new(0.140, 0.080), Illuminant2.D65, new GammaCompression(2.8));

        /// <summary>NTSC-J</summary><remarks><see cref="Illuminant2.D93"/></remarks>
        [DisplayName("NTSC-J"), Incomplete]
        public static WorkingProfile NTSCJ
            => new(new(0.630, 0.340), new(0.310, 0.595), new(0.155, 0.070), Illuminant2.D93, new sRGBCompression());

        /// <summary>NTSC-FCC</summary><remarks><see cref="Illuminant2.C"/></remarks>
        [DisplayName("NTSC-FCC")]
        public static WorkingProfile NTSCFCC
            => new(new(0.670, 0.330), new(0.210, 0.710), new(0.140, 0.080), Illuminant2.C, new GammaCompression(2.5));

        /// <summary>NTSC-RGB</summary><remarks><see cref="Illuminant2.C"/></remarks>
        [DisplayName("NTSC-RGB")]
        public static WorkingProfile NTSCRGB
            => new(new(0.670, 0.330), new(0.210, 0.710), new(0.140, 0.080), Illuminant2.C, new GammaCompression(2.2));

        /// <summary>PAL-M</summary><remarks><see cref="Illuminant2.C"/></remarks>
        [DisplayName("PAL-M")]
        public static WorkingProfile PALM
            => new(new(0.670, 0.330), new(0.210, 0.710), new(0.140, 0.080), Illuminant2.C, new GammaCompression(2.2));

        /// <summary>PAL/SECAM RGB</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [DisplayName("PAL/SECAM RGB")]
        public static WorkingProfile PALSECAMRGB
            => new(new(0.640, 0.330), new(0.290, 0.600), new(0.150, 0.060), Illuminant2.D65, new GammaCompression(2.2));

        /// <summary>ProPhoto RGB</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        [DisplayName("ProPhoto RGB")]
        public static WorkingProfile ProPhotoRGB
            => new(new(0.7347, 0.2653), new(0.1596, 0.8404), new(0.0366, 0.0001), Illuminant2.D50, new GammaCompression(1.8));

        /// <summary>Rec. 601 (525 lines)</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [DisplayName("Rec. 601 (525 lines)")]
        public static WorkingProfile Rec601L5
            => new(new(0.630, 0.340), new(0.310, 0.595), new(0.155, 0.070), Illuminant2.D65, new Rec709Compression());

        /// <summary>Rec. 601 (625 lines)</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [DisplayName("Rec. 601 (625 lines)")]
        public static WorkingProfile Rec601L6
            => new(new(0.640, 0.330), new(0.290, 0.600), new(0.150, 0.060), Illuminant2.D65, new Rec709Compression());

        /// <summary>Rec. 709</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [DisplayName("Rec. 709")]
        public static WorkingProfile Rec709
            => new(new(0.640, 0.330), new(0.300, 0.600), new(0.150, 0.060), Illuminant2.D65, new Rec709Compression());

        /// <summary>Rec. 2020</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [DisplayName("Rec. 2020")]
        public static WorkingProfile Rec2020
            => new(new(0.708, 0.292), new(0.170, 0.797), new(0.131, 0.046), Illuminant2.D65, new Rec2020Compression());

        /// <summary>Rec. 2100</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [DisplayName("Rec. 2100")]
        public static WorkingProfile Rec2100
            => new(new(0.708, 0.292), new(0.170, 0.797), new(0.131, 0.046), Illuminant2.D65, new PQCompression());

        /// <summary>RIMM</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        [DisplayName("RIMM")]
        public static WorkingProfile RIMM
            => new(new(0.7347, 0.2653), new(0.1596, 0.8404), new(0.0366, 0.0001), Illuminant2.D50, new GammaCompression(2.222));

        /// <summary>scRGB</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [DisplayName("scRGB"), Incomplete]
        public static WorkingProfile scRGB
            => new(new(0.640, 0.330), new(0.300, 0.600), new(0.1500, 0.060), Illuminant2.D65, new sRGBCompression());

        /// <summary>SMPTE-C-RGB</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [DisplayName("SMPTE-C-RGB")]
        public static WorkingProfile SMPTECRGB
            => new(new(0.630, 0.340), new(0.310, 0.595), new(0.155, 0.070), Illuminant2.D65, new GammaCompression(2.2));

        /// <summary>sRGB</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [DisplayName("sRGB")]
        public static WorkingProfile sRGB
            => new(WorkingProfile.sRGBPrimary, WorkingProfile.sRGBWhite, new sRGBCompression());

        /// <summary>UHDTV</summary><remarks><see cref="Illuminant2.D65"/></remarks>
        [DisplayName("UHDTV")]
        public static WorkingProfile UHDTV
            => new(new(0.708, 0.292), new(0.170, 0.797), new(0.131, 0.046), Illuminant2.D65, new GammaCompression(2.4));

        /// <summary>Wide Gamut RGB</summary><remarks><see cref="Illuminant2.D50"/></remarks>
        [DisplayName("Wide Gamut RGB")]
        public static WorkingProfile WideGamutRGB
            => new(new(0.735, 0.265), new(0.115, 0.826), new(0.157, 0.018), Illuminant2.D50, new GammaCompression(2.2));
    }