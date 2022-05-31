using System;

namespace Imagin.Core.Colors;

[Flags, Serializable]
public enum Components
{
    /// <summary>Specifies the first <see cref="Component"/> of a <see cref="ColorVector"/>.</summary>
    X,
    /// <summary>Specifies the second <see cref="Component"/> of a <see cref="ColorVector"/>.</summary>
    Y,
    /// <summary>Specifies the third <see cref="Component"/> of a <see cref="ColorVector"/>.</summary>
    Z,
}