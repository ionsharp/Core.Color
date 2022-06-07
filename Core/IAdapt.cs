namespace Imagin.Core.Colors;

/// <summary>
/// Specifies a model for converting from <see cref="LMS"/> with white point (a) to <see cref="LMS"/> with white point (b). If white points are equal, no conversion is performed.
/// <para>Note: <see cref="IAdapt">Colourful</see> defines source and target white point as <see cref="LMS"/> (instead of <see cref="Vector"/>).</para>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
public interface IAdapt
{
    LMS Convert(LMS input, LMS sWhite, LMS tWhite);
}