using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System;
using System.Linq;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <para><b>U*, V*, W*</b></para>
/// <para>A color model based on 'UCS' that was invented to calculate color differences without having to hold the luminance constant.</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="XYZ"/> > <see cref="UVW"/></para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>CIEUVW</item>
/// <item>U*V*W*</item>
/// </list>
/// 
/// <i>Author</i>
/// <list type="bullet">
/// <item><see cref="CIE"/> (1964)</item>
/// </list>
/// </summary>
/// <remarks>https://github.com/colorjs/color-space/blob/master/uvw.js</remarks>
[Component(-134, 224, ' ', "U*"), Component(-140, 122, ' ', "V*"), Component(100, '%', "W*")]
[Category(Class.XYZ), Serializable]
[Description("A color model based on 'UCS' that was invented to calculate color differences without having to hold the luminance constant.")]
public class UVW : ColorModel3<XYZ>
{
    public UVW() : base() { }

    /// <summary>(🗸) <see cref="XYZ"/> > <see cref="UVW"/></summary>
    public override void From(XYZ input, WorkingProfile profile)
    {
        double x = input.X, y = input.Y, z = input.Z, xn, yn, zn, un, vn;

        xn = profile.White.X; yn = profile.White.Y; zn = profile.White.Z;
        un = (4 * xn) / (xn + (15 * yn) + (3 * zn));
        vn = (6 * yn) / (xn + (15 * yn) + (3 * zn));

        var uv = x + 15 * y + 3 * z;
        var _u = uv == 0 ? 0 : 4 * x / uv;
        var _v = uv == 0 ? 0 : 6 * y / uv;

        var w = 25 * Pow(y, 1 / 3) - 17;
        var u = 13 * w * (_u - un);
        var v = 13 * w * (_v - vn);
        Value = new(u, v, w);
    }

    /// <summary>(🗸) <see cref="UVW"/> > <see cref="XYZ"/></summary>
    public override void To(out XYZ result, WorkingProfile profile)
    {
        double _u, _v, w, u, v, x, y, z, xn, yn, zn, un, vn;
        u = X; v = Y; w = Z;

        if (w == 0)
        {
            result = Colour.New<XYZ>(0);
            return;
        }

        xn = profile.White.X; yn = profile.White.Y; zn = profile.White.Z;
        un = (4 * xn) / (xn + (15 * yn) + (3 * zn));
        vn = (6 * yn) / (xn + (15 * yn) + (3 * zn));

        y = Pow((w + 17f) / 25f, 3f).Single();

        _u = 13 * w == 0 ? 0 : u / (13 * w) + un;
        _v = 13 * w == 0 ? 0 : v / (13 * w) + vn;

        x = (6 / 4) * y * _u / _v;
        z = y * (2 / _v - 0.5 * _u / _v - 5).Single();

        result = Colour.New<XYZ>(new Vector3(x, y, z) / 100);
    }
}