using static System.Math;

namespace Imagin.Core.Colors;

/// <summary><b>Rec. 2020</b></summary>
[DisplayName(name), Index(4)]
public struct Rec2020Compression : ICompress
{
    const string name = "Rec. 2020";

    [Index(-1), ReadOnly, Visible]
    public string Name => name;

    public Rec2020Compression() { }

    public double Transfer(double channel)
    {
        var L = channel;
        var V = L < CIE.Beta ? 4.5 * L : CIE.Alpha * Pow(L, y: 0.45) - (CIE.Alpha - 1.0);
        return V;
    }

    public double TransferInverse(double channel)
    {
        var V = channel;
        var L = V < CIE.BetaInverse ? V / 4.5 : Pow((V + CIE.Alpha - 1.0) / CIE.Alpha, 1 / 0.45);
        return L;
    }
}