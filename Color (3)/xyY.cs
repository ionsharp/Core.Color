using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>x, y, Y</b>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="xyY"/></para>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/xyy.js</remarks>
[Component(1, "x"), Component(1, "y"), Component(1, '%', "Y")]
[Serializable]
public class xyY : XYZ
{
    public xyY(params double[] input) : base(input) { }

    public static implicit operator xyY(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🗸) <see cref="xyY"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile)
    {
        double x = this[0], y = this[1], Y = this[2];
        if (y == 0)
            return new(0, 0, 0);

        return new XYZ(x * Y / y, Y, (1 - x - y) * Y / y).ToLrgb(profile);
	}

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="xyY"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile)
	{
		var result = new XYZ();
		result.FromLrgb(input, profile);

        double sum, X, Y, Z;
        X = input[0]; Y = input[1]; Z = input[2];

        sum = X + Y + Z;
        if (sum == 0)
        {
            Value = new(.0, 0, Y);
            return;
        }
        Value = new(X / sum, Y / sum, Y);
    }
}