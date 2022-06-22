using static System.Math;

namespace Imagin.Core.Colors;

/// <summary><b>Rec. 709</b></summary>
[DisplayName(name), Index(5)]
public struct Rec709Compression : ICompress
{
    const string name = "Rec. 709";

    [Index(-1), ReadOnly, Visible]
    public string Name => name;

    public Rec709Compression() { }

    public double TransferInverse(double channel)
    {
        var V = channel;
        var L = V < 0.081 ? V / 4.5 : Pow((V + 0.099) / 1.099, 1 / 0.45);
        return L;
    }

    public double Transfer(double channel)
    {
        var L = channel;
        var V = L < 0.018 ? 4.5 * L : 1.099 * Pow(L, 0.45) - 0.099;
        return V;
    }
}