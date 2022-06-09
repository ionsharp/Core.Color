using System;

namespace Imagin.Core.Colors;

[DisplayName(name), Index(0)]
public struct GammaTransfer : ITransfer
{
    const string name = "Gamma";

    [Index(-1), Label(false), ReadOnly, Visible]
    public string Name => name;

    public double Gamma { get; private set; } = 2.4;

    public GammaTransfer() { }

    public GammaTransfer(double gamma) : this() => Gamma = gamma;

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