namespace Imagin.Core.Colors
{
    /// <summary>
    /// Specifies a model for converting from <see cref="LMS"/> with white point (a) to <see cref="LMS"/> with white point (b). If white points are equal, no conversion is performed.
    /// <para>Note: <see cref="ChromaticAdaptation">Colourful</see> defines source and target white point as <see cref="LMS"/> (instead of <see cref="Vector"/>).</para>
    /// </summary>
    /// <remarks>https://github.com/tompazourek/Colourful</remarks>
    public abstract class ChromaticAdaptation
    {
        public abstract LMS Convert(LMS input, LMS sWhite, LMS tWhite);
    }

    /// <summary>
    /// Basic implementation of the von Kries chromatic adaptation model.
    /// </summary>
    /// <remarks>http://www.brucelindbloom.com/index.html?Eqn_ChromAdapt.html</remarks>
    public class VonKriesChromaticAdaptation : ChromaticAdaptation
    {
        public VonKriesChromaticAdaptation() : base() { }

        /// <inheritdoc />
        public override LMS Convert(LMS input, LMS sWhite, LMS tWhite)
        {
            var matrix = Matrix.Diagonal(tWhite[0] / sWhite[0], tWhite[1] / sWhite[1], tWhite[2] / sWhite[2]);
            
            var source = input.Value;
            var target = matrix.Multiply(source);
            return new LMS(target);
        }
    }
}