namespace Imagin.Core.Colors;

/// <summary>Computes distance between two colors.</summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
public interface IColorDifference<T> where T : ColorModel
{
    /// <summary>Computes distance between two colors.</summary>
    double ComputeDifference(in T x, in T y);
}

/// <summary>Computes distance between two colors.</summary>
public interface IColorDifference
{
    /// <summary>Computes distance between two colors.</summary>
    double ComputeDifference(in ColorModel x, in ColorModel y);
}