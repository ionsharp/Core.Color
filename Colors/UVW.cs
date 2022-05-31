using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System;
using System.Linq;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🞩) <b>U*, V*, W*</b></para>
/// <para>≡ 56.288%</para>
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
[Component(-134, 224, ' ', "U*")]
[Component(-140, 122, ' ', "V*")]
[Component(0, 100, '%', "W*")]
[Serializable, Unfinished]
public sealed class UVW : XYZVector
{
    public UVW(params double[] input) : base(input) { }

    public static implicit operator UVW(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="UVW"/> > <see cref="XYZ"/></summary>
    public override XYZ ToXYZ(WorkingProfile profile)
    {
        double _u, _v, w, u, v, x, y, z, xn, yn, zn, un, vn;
        u = this[0]; v = this[1]; w = this[2];

        if (w == 0)
            return new(0, 0, 0);

        xn = profile.White[0]; yn = profile.White[1].Single(); zn = profile.White[2].Single();
        un = (4 * xn) / (xn + (15 * yn) + (3 * zn));
        vn = (6 * yn) / (xn + (15 * yn) + (3 * zn));

        y = Pow((w + 17f) / 25f, 3f).Single();

        _u = 13 * w == 0 ? 0 : u / (13 * w) + un;
        _v = 13 * w == 0 ? 0 : v / (13 * w) + vn;

        x = (6 / 4) * y * _u / _v;
        z = y * (2 / _v - 0.5 * _u / _v - 5).Single();

        return new(x / 100, y / 100, z / 100);
    }

    /// <summary><see cref="XYZ"/> > <see cref="UVW"/></summary>
    public override void FromXYZ(XYZ input, WorkingProfile profile)
    {
        double x = input[0], y = input[1], z = input[2], xn, yn, zn, un, vn;

        xn = profile.White[0]; yn = profile.White[1]; zn = profile.White[2];
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
}