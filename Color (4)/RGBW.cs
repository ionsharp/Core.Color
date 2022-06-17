using System;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Red (R), Green (G), Blue (B), White (W)</b>
/// <para>An additive color model based on <see cref="RGB"/> where the primary colors are added with white.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="RGBW"/></para>
/// </summary>
/// <remarks>
/// <para>https://andi-siess.de/rgb-to-color-temperature/</para>
/// <para>https://stackoverflow.com/questions/21117842/converting-an-rgbw-color-to-a-standard-rgb-hsb-representation</para>
/// <para>https://stackoverflow.com/questions/40312216/converting-rgb-to-rgbw</para>
/// </remarks>
[Component(255, "R", "Red"), Component(255, "G", "Green"), Component(255, "B", "Blue"), Component(255, "W", "White")]
[Serializable]
public sealed class RGBW : ColorModel4
{
    public RGBW() : base() { }

    /// <summary>(🞩) <see cref="Lrgb"/> > <see cref="RGBW"/></summary>
    public override void From(Lrgb input, WorkingProfile profile)
    {
        //Reference, currently set to 4500k white light:
        const double kWhiteRedChannel = 255;
        const double kWhiteGreenChannel = 219;
        const double kWhiteBlueChannel = 186;

        double r = input.X;
        double g = input.Y;
        double b = input.Z;

        //These values are what the 'white' value would need to be to get the corresponding color value.
        double whiteValueForRed = r * 255.0 / kWhiteRedChannel;
        double whiteValueForGreen = g * 255.0 / kWhiteGreenChannel;
        double whiteValueForBlue = b * 255.0 / kWhiteBlueChannel;

        //Set the white value to the highest it can be for the given color (without over saturating any channel - thus the minimum of them).
        double minWhiteValue = Min(whiteValueForRed, Min(whiteValueForGreen, whiteValueForBlue));
        double Wo = (minWhiteValue <= 255 ? (double)minWhiteValue : 255);

        //The rest of the channels will just be the original value minus the contribution by the white channel.
        double Ro = (double)(r - minWhiteValue * kWhiteRedChannel / 255);
        double Go = (double)(g - minWhiteValue * kWhiteGreenChannel / 255);
        double Bo = (double)(b - minWhiteValue * kWhiteBlueChannel / 255);

        Value = new(Ro, Go, Bo, Wo);
    }

    /// <summary>(🗸) <see cref="RGBW"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
        double r = X / 255;
        double g = Y / 255;
        double b = Z / 255;
        double w = W / 255;

        r *= (1 - w);
        r += w;

        g *= (1 - w);
        g += w;

        b *= (1 - w);
        b += w;

        return Colour.New<Lrgb>(r, g, b);
    }
}