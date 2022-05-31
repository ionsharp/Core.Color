using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🗸) <b>Lightness (Jz), Chroma (Cz), Hue (Hz)</b></para>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="JzAzBz"/> > <see cref="JzCzHz"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>JzCzhz</item>
/// </list>
/// </summary>
/// <remarks>https://observablehq.com/@jrus/jzazbz</remarks>
[Component(0, 1.0, '%', "Jz", "Lightness")]
[Component(0, 1.0, '%', "Cz", "Chroma")]
[Component(0, 360, '%', "Hz", "Hue")]
[Serializable]
public sealed class JzCzHz : JzAzBzVector
{
    public JzCzHz(params double[] input) : base(input) { }

    public static implicit operator JzCzHz(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="JzCzHz"/> > <see cref="JzAzBz"/></summary>
    public override JzAzBz ToJzAzBz(WorkingProfile profile) => new(new LCH(this).To());

    /// <summary><see cref="JzAzBz"/> > <see cref="JzCzHz"/></summary>
    public override void FromJzAzBz(JzAzBz input, WorkingProfile profile)
    {
        var lch = new LCH();
        lch.From(input);

        Value = lch;
    }
}