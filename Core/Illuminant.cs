using Imagin.Core.Numerics;

namespace Imagin.Core.Colors;

#region Illuminant

/// <summary>
/// Represents a theoretical source of visible light with a published profile.
/// </summary>
/// <remarks>
/// <para>https://github.com/tompazourek/Colourful</para>
/// <para>https://en.wikipedia.org/wiki/Standard_illuminant</para>
/// </remarks>
public static class Illuminant
{
    /// <summary>An arbitrary illuminant when none is specified (<see cref="Illuminant2.D65"/>).</summary>
    public static XYZ Default => GetWhite(Illuminant2.D65);

    //...

    /// <summary>Gets the chromacity coordinates of the given <see cref="XYZ"/> color.</summary>
    public static Vector3<double> GetChromacity(XYZ input)
    {
        var XYZ = input[0] + input[1] + input[2];
        var x = input[0] / XYZ; var y = input[1] / XYZ; var z = input[2] / XYZ;
        return new(x, y, z);
    }

    /// <summary>Gets the <see cref="XYZ"/> coordinates (white point) of the given <see cref="Vector2{T}"/> (chromacity).</summary>
    public static XYZ GetWhite(Vector2 input)
    {
        var X = (1 / input.Y) * input.X; var Y = 1; var Z = (1 / input.Y) * (1 - input.X - input.Y);
        return new(X, Y, Z);
    }

    /// <summary> Equal energy (<see cref="Illuminant">5454</see> K).</summary>
    public static Vector2 E => new(0.33333, 0.33333);
}

#endregion

#region Illuminant2

/// <summary>Specifies an <see cref="Illuminant"/> measured with <see cref="Observers.D2"/>.</summary>
public static class Illuminant2
{
    /// <summary>Incandescent / tungsten (<see cref="Illuminant">2856</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 A => new(0.44757, 0.40745);
    /// <summary>Direct sunlight at noon (<see cref="Illuminant">4874</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 B => new(0.34842, 0.35161);
    /// <summary>Average / North sky daylight (<see cref="Illuminant">6774</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 C => new(0.31006, 0.31616);

    ///(Daylight)

    /// <summary> Horizon light (<see cref="Illuminant">5003</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 D50 => new(0.34567, 0.35850);
    /// <summary> Mid-morning / Mid-afternoon daylight (<see cref="Illuminant">5503</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 D55 => new(0.33242, 0.34743);
    /// <summary> Undefined. Used by <see cref="WorkingProfiles.DCIP3"/>. To do: Obtain coordinates! (<see cref="Illuminant">6300</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 D63 => new(0.31271, 0.32902);
    /// <summary> Noon daylight (television/sRGB color space) (<see cref="Illuminant">6504</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 D65 => new(0.31271, 0.32902);
    /// <summary> North sky daylight (<see cref="Illuminant">7504</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 D75 => new(0.29902, 0.31485);
    /// <summary> High-efficiency blue phosphor monitors (BT.2035) (<see cref="Illuminant">9305</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 D93 => new(0.28315, 0.29711);

    ///(Fluorescent)

    /// <summary> Daylight fluorescent (<see cref="Illuminant">6430</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 F1  => new(0.31310, 0.33727);
    /// <summary> Cool white fluorescent (<see cref="Illuminant">4230</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 F2  => new(0.37208, 0.37529);
    /// <summary> White fluorescent (<see cref="Illuminant">3450</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 F3  => new(0.40910, 0.39430);
    /// <summary> Warm white fluorescent (<see cref="Illuminant">2940</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 F4  => new(0.44018, 0.40329);
    /// <summary> Daylight fluorescent (<see cref="Illuminant">6350</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 F5  => new(0.31379, 0.34531);
    /// <summary> Light white fluorescent (<see cref="Illuminant">4150</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 F6  => new(0.37790, 0.38835);
    /// <summary>D65 simulator (daylight simulator) (<see cref="Illuminant">6500</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 F7  => new(0.31292, 0.32933);
    /// <summary>D50 simulator (Sylvania F40 Design 50) (<see cref="Illuminant">5000</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 F8  => new(0.34588, 0.35875);
    /// <summary>Cool white deluxe fluorescent (<see cref="Illuminant">4150</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 F9  => new(0.37417, 0.37281);
    /// <summary>Philips TL85, Ultralume 50 (<see cref="Illuminant">5000</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 F10 => new(0.34609, 0.35986);
    /// <summary>Philips TL84, Ultralume 40 (<see cref="Illuminant">4000</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 F11 => new(0.38052, 0.37713);
    /// <summary>Philips TL83, Ultralume 30 (<see cref="Illuminant">3000</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 F12 => new(0.43695, 0.40441);

    ///(LED-B*)

    /// <summary>Phosphor-converted blue (<see cref="Illuminant">2733</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 LED_B1 => new(0.4560, 0.4078);
    /// <summary>Phosphor-converted blue (<see cref="Illuminant">2998</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 LED_B2 => new(0.4357, 0.4012);
    /// <summary>Phosphor-converted blue (<see cref="Illuminant">4103</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 LED_B3 => new(0.3756, 0.3723);
    /// <summary>Phosphor-converted blue (<see cref="Illuminant">5109</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 LED_B4 => new(0.3422, 0.3502);
    /// <summary>Phosphor-converted blue (<see cref="Illuminant">6598</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 LED_B5 => new(0.3118, 0.3236);

    ///(LED-BH*)

    /// <summary>Mixing of phosphor-converted blue LED and red LED (blue-hybrid) (<see cref="Illuminant">2851</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 LED_BH1 => new(0.4474, 0.4066);

    ///(LED-RGB*)

    /// <summary>Mixing of red, green, and blue LEDs (<see cref="Illuminant">2840</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 LED_RGB => new(0.4557, 0.4211);

    ///(LED-V*)

    /// <summary>Phosphor-converted violet (<see cref="Illuminant">2724</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 LED_V1 => new(0.4560, 0.4548);
    /// <summary>Phosphor-converted violet (<see cref="Illuminant">4070</see> K).</summary>
    /// <remarks><see cref="Observers.D2"/></remarks>
    public static Vector2 LED_V2 => new(0.3781, 0.3775);
}

#endregion

#region Illuminant10

/// <summary>Specifies an <see cref="Illuminant"/> measured with <see cref="Observers.D10"/>.</summary>
public static class Illuminant10
{
    /// <summary>Incandescent / tungsten (<see cref="Illuminant">2856</see> K).</summary>
    public static Vector2 A => new(0.45117, 0.40594);
    /// <summary>Direct sunlight at noon (<see cref="Illuminant">4874</see> K).</summary>
    public static Vector2 B => new(0.34980, 0.35270);
    /// <summary>Average / North sky daylight (<see cref="Illuminant">6774</see> K).</summary>
    public static Vector2 C => new(0.31039, 0.31905);

    ///(Daylight)

    /// <summary> Horizon light (<see cref="Illuminant">5003</see> K).</summary>
    public static Vector2 D50 => new(0.34773, 0.35952);
    /// <summary> Mid-morning / Mid-afternoon daylight (<see cref="Illuminant">5503</see> K).</summary>
    public static Vector2 D55 => new(0.33411, 0.34877);
    /// <summary> Noon daylight (television/sRGB color space) (<see cref="Illuminant">6504</see> K).</summary>
    public static Vector2 D65 => new(0.31382, 0.33100);
    /// <summary> North sky daylight (<see cref="Illuminant">7504</see> K).</summary>
    public static Vector2 D75 => new(0.29968, 0.31740);
    /// <summary> High-efficiency blue phosphor monitors (BT.2035) (<see cref="Illuminant">9305</see> K).</summary>
    public static Vector2 D93 => new(0.28327, 0.30043);

    ///(Fluorescent)

    /// <summary> Daylight fluorescent (<see cref="Illuminant">6430</see> K).</summary>
    public static Vector2 F1  => new(0.31811, 0.33559);
    /// <summary> Cool white fluorescent (<see cref="Illuminant">4230</see> K).</summary>
    public static Vector2 F2  => new(0.37925, 0.36733);
    /// <summary> White fluorescent (<see cref="Illuminant">3450</see> K).</summary>
    public static Vector2 F3  => new(0.41761, 0.38324);
    /// <summary> Warm white fluorescent (<see cref="Illuminant">2940</see> K).</summary>
    public static Vector2 F4  => new(0.44920, 0.39074);
    /// <summary> Daylight fluorescent (<see cref="Illuminant">6350</see> K).</summary>
    public static Vector2 F5  => new(0.31975, 0.34246);
    /// <summary> Light white fluorescent (<see cref="Illuminant">4150</see> K).</summary>
    public static Vector2 F6  => new(0.38660, 0.37847);
    /// <summary>D65 simulator (daylight simulator) (<see cref="Illuminant">6500</see> K).</summary>
    public static Vector2 F7  => new(0.31569, 0.32960);
    /// <summary>D50 simulator (Sylvania F40 Design 50) (<see cref="Illuminant">5000</see> K).</summary>
    public static Vector2 F8  => new(0.34902, 0.35939);
    /// <summary>Cool white deluxe fluorescent (<see cref="Illuminant">4150</see> K).</summary>
    public static Vector2 F9  => new(0.37829, 0.37045);
    /// <summary>Philips TL85, Ultralume 50 (<see cref="Illuminant">5000</see> K).</summary>
    public static Vector2 F10 => new(0.35090, 0.35444);
    /// <summary>Philips TL84, Ultralume 40 (<see cref="Illuminant">4000</see> K).</summary>
    public static Vector2 F11 => new(0.38541, 0.37123);
    /// <summary>Philips TL83, Ultralume 30 (<see cref="Illuminant">3000</see> K).</summary>
    public static Vector2 F12 => new(0.44256, 0.39717);
}

#endregion