using System;

namespace Imagin.Core.Colors;

[Flags, Serializable]
public enum RedGreenBlue
{
    [Hidden]
    None = 0,
    Red = 1,
    Green = 2,
    Blue = 4,
    [Hidden]
    All = Red | Green | Blue
}