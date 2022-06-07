namespace Imagin.Core.Colors;

/// <summary>Functions used for conversion to <see cref="XYZ"/> and back.</summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
public interface ICompress
{
    /// <summary>The companded channel (non linear) is made linear with respect to the energy.</summary>
    /// <remarks>Non Linear (Companded) > Linear (Uncompanded)</remarks>
    double CompandInverse(double channel);

    /// <summary>The uncompanded channel (linear) is made non linear (depends on <see cref="RGB"/> color space).</summary>
    /// <remarks>Linear (Uncompanded) > Non Linear (Companded)</remarks>
    double Compand(double channel);
}