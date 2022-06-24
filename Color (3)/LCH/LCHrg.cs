using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <b>Luminance (L), Chroma (C), Hue (H)</b>
/// <para>A cylindrical form of <see cref="rgG"/> that is designed to accord with the human perception of color.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="rgG"/> > <see cref="LCHrg"/></para>
/// </summary>
[Serializable]
public class LCHrg : LCH<rgG>
{
    public LCHrg() : base() { }

    /// <inheritdoc/>
    public override Vector3 ToLCh(Vector3 input)
    {
        input = new Vector3(input.Z, input.X, input.Y) * new Vector3(100, 200, 200) - new Vector3(0, 100, 100);
        return base.ToLCh(input);
    }

    /// <inheritdoc/>
    public override Vector3 FromLCh(Vector3 input)
    {
        var result = base.FromLCh(input);
        result = new Vector3(result.Y, result.Z, result.X) + new Vector3(100, 100, 0) / new Vector3(200, 200, 100);
        return result;
    }
}