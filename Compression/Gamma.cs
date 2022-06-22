using static System.Math;

namespace Imagin.Core.Colors;

/// <summary><b>Gamma</b></summary>
[DisplayName(name), Index(0)]
public struct GammaCompression : ICompress
{
    const string name = "Gamma";

    [Index(-1), ReadOnly, Visible]
    public string Name => name;

    public double Gamma { get; private set; } = 2.4;

    public GammaCompression() { }

    public GammaCompression(double gamma) : this() => Gamma = gamma;

    public double Transfer(double channel)
    {
        var v = channel;
        var V = Pow(v, 1 / Gamma);
        return V;
    }

    public double TransferInverse(double channel)
    {
        var V = channel;
        var v = Pow(V, Gamma);
        return v;
    }
}