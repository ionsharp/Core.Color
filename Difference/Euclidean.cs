using System;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>Euclidean distance between two colors.</summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[DisplayName("Euclidean"), Serializable]
public sealed class EuclideanColorDifference : IColorDifference<ColorModel3>, IColorDifference
{
    public EuclideanColorDifference() { }

    public double ComputeDifference(in ColorModel3 x, in ColorModel3 y)
    {
        var distanceSquared = 0d;
        var vectorSize = Min(x.Length, y.Length);

        for (var i = 0; i < vectorSize; i++)
        {
            var xi = x.Value[i];
            var yi = y.Value[i];

            var xyiDiff = xi - yi;
            distanceSquared += xyiDiff * xyiDiff;
        }

        return Sqrt(distanceSquared);
    }

    double IColorDifference.ComputeDifference(in ColorModel x, in ColorModel y) => ComputeDifference((ColorModel3)x, (ColorModel3)y);
}