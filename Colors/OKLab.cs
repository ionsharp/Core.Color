using Imagin.Core.Numerics;
using System;

using static Imagin.Core.Numerics.M;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// (🗸) <b>Perceived lightness (L), Red/green (a), Blue/yellow (b)</b>
/// <para>≡ 100%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="OKLab"/></para>
/// </summary>
/// <remarks>https://colour.readthedocs.io/en/develop/_modules/colour/models/oklab.html</remarks>
[Component(0, 1, '°', "L", "Perceived lightness")]
[Component(0, 1, '%', "a", "Red/green")]
[Component(0, 1, '%', "b", "Blue/yellow")]
[Serializable]
public sealed class OKLab : XYZVector
{
    public OKLab(params double[] input) : base(input) { }

    public static implicit operator OKLab(Vector3 input) => new(input.X, input.Y, input.Z);

    static Matrix XYZ_LMS => new double[][]
    {
        new[] { 0.8189330101, 0.3618667424, -0.1288597137 },
        new[] { 0.0329845436, 0.9293118715,  0.0361456387 },
        new[] { 0.0482003018, 0.2643662691,  0.6338517070 },
    };
    static Matrix LMS_XYZ => XYZ_LMS.Invert3By3();

    static Matrix LMS_LAB => new double[][]
    {
        new[] { 0.2104542553,  0.7936177850, -0.0040720468 },
        new[] { 1.9779984951, -2.4285922050,  0.4505937099 },
        new[] { 0.0259040371,  0.7827717662, -0.8086757660 },
    };
    static Matrix LAB_LMS => LMS_LAB.Invert3By3();

    /// <summary><see cref="OKLab"/> > <see cref="XYZ"/></summary>
    public override XYZ ToXYZ(WorkingProfile profile)
    {
        var lab = Value;

        var lms = LAB_LMS.Multiply(lab);
        var lmsPrime = new Vector(Pow(lms[0], 3), Pow(lms[1], 3), Pow(lms[2], 3));

        var xyz = LMS_XYZ.Multiply(lmsPrime);
        return new(xyz * profile.White);
    }

    /// <summary><see cref="XYZ"/> > <see cref="OKLab"/></summary>
    public override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        XYZ xyz = new(input.Value / profile.White);

        var lms = XYZ_LMS.Multiply(xyz);
        var lmsPrime = new LMS(Cbrt(lms[0]), Cbrt(lms[1]), Cbrt(lms[2]));

        var lab = LMS_LAB.Multiply(lmsPrime);
        Value = new(lab);
    }
}