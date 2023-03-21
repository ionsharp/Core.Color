using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para><b>X, Y, Z</b></para>
/// A color model based on 'LMS' where 'Z' corresponds to the short (S) cone response of the human eye, 'Y' is a mix of long (L) and medium (M) cone responses, and 'X' is a mix of all three.
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>CIEXYZ</item>
/// </list>
/// 
/// <i>Author</i>
/// <list type="bullet">
/// <item><see cref="CIE"/> (1931)</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/tompazourek/Colourful</remarks>
[Component(1, "X"), Component(1, "Y"), Component(1, "Z")]
[Category(Class.XYZ), Serializable]
[Description("A color model based on 'LMS' where 'Z' corresponds to the short (S) cone response of the human eye, 'Y' is a mix of long (L) and medium (M) cone responses, and 'X' is a mix of all three.")]
public class XYZ : ColorModel3
{
    public XYZ() : base() { }

    ///

    public static explicit operator XYZ(Vector3 input) => Colour.New<XYZ>(input);

    public static explicit operator XYZ(xy input) => (XYZ)(xyY)input;

    public static explicit operator XYZ(xyY input)
    {
        input.To(out XYZ result, default);
        return result;
    }

    ///

    /// <summary>(🗸) <see cref="XYZ"/> (0) > <see cref="LMS"/> (0) > <see cref="LMS"/> (1) > <see cref="XYZ"/> (1)</summary>
    public override void Adapt(WorkingProfile source, WorkingProfile target) => Value = Adapt(this, source, target);

    ///

    /// <summary>(🗸) <see cref="Lrgb"/> > <see cref="XYZ"/></summary>
    public override void From(Lrgb input, WorkingProfile profile)
    {
        var m = GetMatrix(profile.Primary, profile.White);
        Value = m * input.Value;
    }

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="Lrgb"/></summary>
    public override Lrgb To(WorkingProfile profile)
    {
        var result = GetMatrix(profile.Primary, profile.White).Invert3By3() * Value;
        return Colour.New<Lrgb>(result[0], result[1], result[2]);
    }

    ///

    /// <summary>Gets the matrix used to convert between <see cref="RGB"/> and <see cref="XYZ"/>.</summary>
    /// <remarks>http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html</remarks>
    public static Matrix GetMatrix(Primary3 primary, Vector3 white)
    {
        double
            xr = primary.R.X,
            xg = primary.G.X,
            xb = primary.B.X,
            yr = primary.R.Y,
            yg = primary.G.Y,
            yb = primary.B.Y;

        var Xr = xr / yr;
        const double Yr = 1;
        var Zr = (1 - xr - yr) / yr;

        var Xg = xg / yg;
        const double Yg = 1;
        var Zg = (1 - xg - yg) / yg;

        var Xb = xb / yb;
        const double Yb = 1;
        var Zb = (1 - xb - yb) / yb;

        Matrix S = new double[][]
        {
            new[] { Xr, Xg, Xb },
            new[] { Yr, Yg, Yb },
            new[] { Zr, Zg, Zb },
        };

        var W = white;
        var SW = S.Invert3By3().Multiply(W);

        var Sr = SW[0]; var Sg = SW[1]; var Sb = SW[2];

        Matrix M = new double[][]
        {
            new[] { Sr * Xr, Sg * Xg, Sb * Xb },
            new[] { Sr * Yr, Sg * Yg, Sb * Yb },
            new[] { Sr * Zr, Sg * Zg, Sb * Zb },
        };
        return M;
    }
}