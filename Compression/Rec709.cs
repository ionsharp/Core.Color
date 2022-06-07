using System;

namespace Imagin.Core.Colors;

[DisplayName(name), Index(3)]
public struct Rec709Compression : ICompress
{
    const string name = "Rec709";

    [Index(-1), Label(false), ReadOnly, Visible]
    public string Name => name;

    public Rec709Compression() { }

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