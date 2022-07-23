using System;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Perceptual Quantization (PQ)</b>
/// <para>A transfer function developed by Dolby for HDR and allowing a luminance level of up to 10,000 cd/m2.</para>
/// <para><b>Rec. 2100</b></para>
/// <para><b>SMPTE ST 2084</b></para>
/// </summary>
/// <remarks>https://en.wikipedia.org/wiki/Perceptual_quantizer</remarks>
[DisplayName(name), Index(3), Serializable]
[Description("")]
public struct PQCompression : ICompress
{
    const string name = "PQ";

    [Index(-1), ReadOnly, Visible]
    public string Name => name;

    public PQCompression() { }

    public double Transfer(double E)
    {
        var c2 = 18.8515625;
        var c3 = 18.6875;
        var c1 = c3 - c2 + 1;

        var m2 = 78.84375;
        return 10000 * Max(Pow(E, 1 / m2) - c1, 0) / (c2 - c3 * Pow(E, 1 / m2));
    }

    public double TransferInverse(double Y)
    {
        var c2 = 18.8515625;
        var c3 = 18.6875;
        var c1 = c3 - c2 + 1;

        var m1 = 0.1593017578125;
        var m2 = 78.84375;

        return Pow(c1 + c2 * Pow(Y, m1) / (1 + c3 * Pow(Y, m1)), m2);
    }

    public override string ToString() => name;
}