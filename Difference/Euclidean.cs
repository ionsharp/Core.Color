using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>Euclidean distance between two colors.</summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
public sealed class EuclideanDistanceColorDifference<T> : IColorDifference<T> where T : ColorModel3
{
    public double ComputeDifference(in T x, in T y)
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
}