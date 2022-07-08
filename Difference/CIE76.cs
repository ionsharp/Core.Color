using System;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>CIE Delta-E 1976 color difference formula.</summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[DisplayName("CIE Delta-E 1976"), Serializable]
public class CIE76ColorDifference : IColorDifference<Lab>, IColorDifference
{
    public CIE76ColorDifference() { }

    /// <param name="a">Reference color.</param>
    /// <param name="b">Sample color.</param>
    /// <returns>Delta-E (1976) color difference.</returns>
    public double ComputeDifference(in Lab a, in Lab b)
    {
        var distance = Sqrt
        (
            (a.X - b.X) * (a.X - b.X) +
            (a.Y - b.Y) * (a.Y - b.Y) +
            (a.Z - b.Z) * (a.Z - b.Z)
        );
        return distance;
    }

    double IColorDifference.ComputeDifference(in ColorModel x, in ColorModel y) => ComputeDifference((Lab)x, (Lab)y);
}