using System;

namespace Imagin.Core.Colors;

[DisplayName(name), Index(6)]
public struct sRGBCompression : ICompress
{
    const string name = "sRGB";

    [Index(-1), Label(false), ReadOnly, Visible]
    public string Name => name;

    public sRGBCompression() { }

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