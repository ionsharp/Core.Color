using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Cyan (C), Magenta (M), Yellow (Y)</b> 
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="CMY"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/cmy.js</remarks>
[Component(0, 1, "C", "Cyan"), Component(0, 1, "M", "Magenta"), Component(0, 1, "Y", "Yellow")]
[Serializable]
public sealed class CMY : ColorVector3
{
    public CMY(params double[] input) : base(input) { }

    public static implicit operator CMY(Vector3 input) => new(input.X, input.Y, input.Z);

    public override Lrgb ToLrgb(WorkingProfile profile)
        => new(1 - Value[0], 1 - Value[1], 1 - Value[2]);

    public override void FromLrgb(Lrgb input, WorkingProfile profile)
        => Value = new(1 - input[0], 1 - input[1], 1 - input[2]);
}