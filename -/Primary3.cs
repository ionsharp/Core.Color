using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

[Serializable]
public struct Primary3 : IEquatable<Primary3>
{
    #region Fields

    public Vector2 R { get; private set; } = Vector2.Zero;

    public Vector2 G { get; private set; } = Vector2.Zero;

    public Vector2 B { get; private set; } = Vector2.Zero;

    #endregion

    #region Primary3

    public Primary3() { }

    public Primary3(Vector2 r, Vector2 g, Vector2 b)
    {
        R = r; G = g; B = b;
    }

    public Primary3(double rX, double rY, double gX, double gY, double bX, double bY) 
    {
        R = new(rX, rY); G = new(gX, gY); B = new(bX, bY);
    }

    #endregion

    #region Methods

    public override string ToString()
        => $"R = {R}, G = {G}, B = {B}";

    #endregion

    #region ==

    public static bool operator ==(Primary3 a, Primary3 b)
        => a.EqualsOverload(b);

    public static bool operator !=(Primary3 a, Primary3 b)
        => !(a == b);

    public bool Equals(Primary3 i)
        => this.Equals<Primary3>(i) && R.Equals(i.R) && G.Equals(i.G) && B.Equals(i.B);

    public override bool Equals(object i)
        => i is Primary3 j && Equals(j);

    public override int GetHashCode()
        => XArray.New(R, G, B).GetHashCode();

    #endregion
}