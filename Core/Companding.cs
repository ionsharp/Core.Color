using System;

using static Imagin.Core.Numerics.M;

namespace Imagin.Core.Colors;

#region (interface) ICompanding

/// <summary>
/// Functions used for conversion to <see cref="XYZ"/> and back.
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
public interface ICompanding
{
    /// <summary>The companded channel (non linear) is made linear with respect to the energy.</summary>
    /// <remarks>Non Linear (Companded) > Linear (Uncompanded)</remarks>
    double CompandInverse(double channel);

    /// <summary>The uncompanded channel (linear) is made non linear (depends on <see cref="RGB"/> color space).</summary>
    /// <remarks>Linear (Uncompanded) > Non Linear (Companded)</remarks>
    double Compand(double channel);
}

#endregion

#region GammaCompanding

public class GammaCompanding : ICompanding
{
    public double Gamma { get; private set; }

    public GammaCompanding(double gamma) => Gamma = gamma;

    public double Compand(double channel)
    {
        var v = channel;
        var V = Math.Pow(v, 1 / Gamma);
        return V;
    }

    public double CompandInverse(double channel)
    {
        var V = channel;
        var v = Math.Pow(V, Gamma);
        return v;
    }
}

#endregion

#region LCompanding

public class LCompanding : ICompanding
{
    public LCompanding() { }

    public double Compand(double channel)
    {
        var v = channel;
        var V = v <= CIE.Epsilon ? v * CIE.Kappa / 100d : Math.Pow(1.16 * v, 1 / 3d) - 0.16;
        return V;
    }

    public double CompandInverse(double channel)
    {
        var V = channel;
        var v = V <= 0.08 ? 100 * V / CIE.Kappa : Pow3((V + 0.16) / 1.16);
        return v;
    }
}

#endregion

#region Rec601Companding

public class Rec601Companding : ICompanding
{
    public Rec601Companding() { }

    public double CompandInverse(double channel)
    {
        var V = channel;
        var L = V < 0.081 ? V / 4.5 : Math.Pow((V + 0.099) / 1.099, 1 / 0.45);
        return L;
    }

    public double Compand(double channel)
    {
        var L = channel;
        var V = L < 0.018 ? 4500 * L : 1.099 * L - 0.099;
        return V;
    }
}

#endregion

#region Rec709Companding

public class Rec709Companding : ICompanding
{
    public Rec709Companding() { }

    public double CompandInverse(double channel)
    {
        var V = channel;
        var L = V < 0.081 ? V / 4.5 : Math.Pow((V + 0.099) / 1.099, 1 / 0.45);
        return L;
    }

    public double Compand(double channel)
    {
        var L = channel;
        var V = L < 0.018 ? 4500 * L : 1.099 * L - 0.099;
        return V;
    }
}

#endregion

#region Rec2020Companding

public class Rec2020Companding : ICompanding
{
    public Rec2020Companding() { }

    public virtual double CompandInverse(double channel)
    {
        var V = channel;
        var L = V < 0.08145 ? V / 4.5 : Math.Pow((V + 0.0993) / 1.0993, 1 / 0.45);
        return L;
    }

    public virtual double Compand(double channel)
    {
        var L = channel;
        var V = L < 0.0181 ? 4500 * L : 1.0993 * L - 0.0993;
        return V;
    }
}

#endregion

#region Rec2100Companding

public class Rec2100Companding : ICompanding
{
    public Rec2100Companding() { }

    public virtual double CompandInverse(double channel)
    {
        var V = channel;
        var L = V < 0.08145 ? V / 4.5 : Math.Pow((V + 0.0993) / 1.0993, 1 / 0.45);
        return L;
    }

    public virtual double Compand(double channel)
    {
        var L = channel;
        var V = L < 0.0181 ? 4500 * L : 1.0993 * L - 0.0993;
        return V;
    }
}

#endregion

#region sRGBCompanding

public class sRGBCompanding : ICompanding
{
    public sRGBCompanding() { }

    public double CompandInverse(double channel)
    {
        var V = channel;
        var v = V <= 0.04045 ? V / 12.92 : Math.Pow((V + 0.055) / 1.055, 2.4);
        return v;
    }

    public double Compand(double channel)
    {
        var v = channel;
        var V = v <= 0.0031308 ? 12.92 * v : 1.055 * Math.Pow(v, 1 / 2.4d) - 0.055;
        return V;
    }
}

#endregion