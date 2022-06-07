using System;

namespace Imagin.Core.Colors;

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
    /// <summary>sRGB is a standard RGB (red, green, blue) color space that HP and Microsoft created cooperatively in 1996 to use on monitors, printers, and the World Wide Web.<para>https://en.wikipedia.org/wiki/SRGB</para></summary>
    sRGB,
    /// <summary>scRGB.</summary>
    scRGB,
    /// <summary>UHDTV.</summary>
    UHDTV,
    /// <summary>Wide Gamut RGB.</summary>
    WideGamutRGB,
}