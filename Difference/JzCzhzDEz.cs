using Imagin.Core.Numerics;
using System;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>Delta Ez color difference for <see cref="LCHabj"/>.</summary>
/// <remarks>
/// <para>https://github.com/tompazourek/Colourful</para>
/// <para>https://observablehq.com/@jrus/jzazbz</para>
/// </remarks>
[Name("Delta Ez"), Serializable]
public sealed class JzCzhzDEzColorDifference : IColorDifference<LCHabj>, IColorDifference
{
    public JzCzhzDEzColorDifference() { }

    /// <inheritdoc />
    public double ComputeDifference(in LCHabj x, in LCHabj y)
    {
        // conversion algorithm from: 

        var dJz = y.X - x.X;
        var dCz = y.Y - x.Y;
        var dhz = Angle.GetRadian(y.Z) - Angle.GetRadian(x.Z);
        var dHz2 = 2 * x.Y * y.Y * (1 - Cos(dhz));
        return Sqrt(dJz * dJz + dCz * dCz + dHz2);
    }

    double IColorDifference.ComputeDifference(in ColorModel x, in ColorModel y) => ComputeDifference((LCHabj)x, (LCHabj)y);
}