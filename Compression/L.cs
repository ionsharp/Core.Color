using System;

using static Imagin.Core.Numerics.M;

namespace Imagin.Core.Colors;

/// <summary><b>L*</b></summary>
[DisplayName(name), Index(1)]
public struct LCompression : ICompress
{
    const string name = "L*";

    [Index(-1), ReadOnly, Visible]
    public string Name => name;

    public LCompression() { }

    public double TransferInverse(double channel)
    {
        var V = channel;
        var v = V <= 0.08 ? 100 * V / CIE.IKappa : Pow3((V + 0.16) / 1.16);
        return v;
    }

    public double Transfer(double channel)
    {
        var v = channel;
        var V = v <= CIE.IEpsilon ? v * CIE.IKappa / 100d : 1.16 * Math.Pow(v, 1.0 / 3.0) - 0.16;
        return V;
    }
}