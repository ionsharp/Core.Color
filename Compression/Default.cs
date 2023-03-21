using System;

namespace Imagin.Core.Colors;

/// <summary><b>Default</b></summary>
[Name(name), Index(0), Serializable]
[Description("")]
public struct Compression : ICompress
{
    const string name = "Default";

    /// <summary>γ</summary>
    public double γ { get; private set; } = 12 / 5;

    /// <summary>α</summary>
    public double α { get; private set; } = 0.055;

    /// <summary>β</summary>
    public double β { get; private set; } = 0.0031308;

    /// <summary>δ</summary>
    public double δ { get; private set; } = 12.92;

    /// <summary>βδ</summary>
    public double βδ { get; private set; } = 0.04045;

    [Index(-1), ReadOnly, Show]
    public string Name => name;

    public Compression() { }

    public Compression(double γ, double α, double β, double δ, double βδ) : this()
    {
        this.γ = γ; this.α = α; this.β = β; this.δ = δ; this.βδ = βδ;
    }

    #region L* (To do: cross-reference and identify discrepancies)

    /*
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
    */

    #endregion

    public double Transfer(double channel)
    {
        var v = channel;
        var V = v <= β ? δ * v : (α + 1) * Math.Pow(v, 1 / γ) - α;
        return V;
    }

    public double TransferInverse(double channel)
    {
        var V = channel;
        var v = V <= βδ ? V / δ : Math.Pow((V + α) / (α + 1), γ);
        return v;
    }

    public override string ToString() => name;
}