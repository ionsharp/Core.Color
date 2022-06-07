using System;

using static Imagin.Core.Numerics.M;

namespace Imagin.Core.Colors;

[DisplayName(name), Index(1)]
public struct LCompression : ICompress
{
    const string name = "L";

    [Index(-1), Label(false), ReadOnly, Visible]
    public string Name => name;

    public LCompression() { }

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