namespace Imagin.Core.Colors;

/// <summary>Basic implementation of the von Kries chromatic adaptation model.</summary>
/// <remarks>http://www.brucelindbloom.com/index.html?Eqn_ChromAdapt.html</remarks>
[DisplayName(name)]
public struct VonKriesAdaptation : IAdapt
{
    const string name = "VonKries";

    [Index(-1), Label(false), ReadOnly, Visible]
    public string Name => name;

    public VonKriesAdaptation() { }

    public LMS Convert(LMS input, LMS sWhite, LMS tWhite)
    {
        var matrix = Matrix.Diagonal(tWhite[0] / sWhite[0], tWhite[1] / sWhite[1], tWhite[2] / sWhite[2]);
        var result = matrix.Multiply(input.Value);
        return new LMS(result);
    }
}