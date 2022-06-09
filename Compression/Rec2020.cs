using System;
using static System.Math;

namespace Imagin.Core.Colors;

[DisplayName(name), Index(4)]
public struct Rec2020Transfer : ITransfer
{
    const string name = "Rec2020";

    [Index(-1), Label(false), ReadOnly, Visible]
    public string Name => name;

    public Rec2020Transfer() { }

    public double CompandInverse(double channel)
    {
        var V = channel;
        var L = V < 0.08145 ? V / 4.5 : Math.Pow((V + 0.0993) / 1.0993, 1 / 0.45);
        return L;
    }

    public double Compand(double channel)
    {
        var L = channel;
        var V = L < 0.0181 ? 4500 * L : 1.0993 * Pow(L, 0.45) - 0.0993;
        return V;
    }
}