using System;

namespace Imagin.Core.Colors;

/// <summary>Specifies a <see cref="Component"/> of a <see cref="ColorModel4"/>.</summary>
[Flags, Serializable]
public enum Component4
{
    /// <summary>Specifies the first <see cref="Component"/> of a <see cref="ColorModel4"/>.</summary>
    X,
    /// <summary>Specifies the second <see cref="Component"/> of a <see cref="ColorModel4"/>.</summary>
    Y,
    /// <summary>Specifies the third <see cref="Component"/> of a <see cref="ColorModel4"/>.</summary>
    Z,
    /// <summary>Specifies the fourth <see cref="Component"/> of a <see cref="ColorModel4"/>.</summary>
    W
}