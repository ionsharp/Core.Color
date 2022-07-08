using System;

namespace Imagin.Core.Colors;

/// <summary>Application area for CIE Delta-E 1994 (<see cref="CIE94ColorDifference"/>).</summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Serializable]
public enum CIE94ColorDifferenceApplication
{
    GraphicArts,
    Textiles,
}