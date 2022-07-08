using System;

namespace Imagin.Core.Colors;

/// <summary>Weighting parameters for CMC l:c color difference formula (<see cref="CMCColorDifference"/>).</summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Serializable]
public enum CMCColorDifferenceThreshold
{
    /// <summary>2:1 (l:c).</summary>
    Acceptability,
    /// <summary>1:1 (l:c).</summary>
    Imperceptibility,
}