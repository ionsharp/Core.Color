using System;
using static System.Math;

namespace Imagin.Core.Colors;

[DisplayName(name), Index(5)]
public struct Rec709Transfer : ITransfer
{
    const string name = "Rec709";

    [Index(-1), Label(false), ReadOnly, Visible]
    public string Name => name;

    public Rec709Transfer() { }

    public double CompandInverse(double channel)
    {
        var V = channel;
        var L = V < 0.081 ? V / 4.5 : Math.Pow((V + 0.099) / 1.099, 1 / 0.45);
        return L;
    }

    public double Compand(double channel)
    {
        var L = channel;
        var V = L < 0.018 ? 4500 * L : 1.099 * Pow(L, 0.45) - 0.099;
        return V;
    }
}