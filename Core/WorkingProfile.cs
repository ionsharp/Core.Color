using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>In colorimetry, this is referred to as an "<see cref="WorkingProfile">RGB Working Space</see>".</summary>
[Serializable]
public partial struct WorkingProfile : IEquatable<WorkingProfile>
{
    #region Properties

    public static WorkingProfile Default => WorkingProfiles.sRGB;

    //...

    public static Primary3 sRGBPrimary => new(new(0.6400, 0.3300), new(0.3000, 0.6000), new(0.1500, 0.0600));

    public static Vector2 sRGBWhite => Illuminant2.D65;

    //...

    /// <summary><see cref="WorkingProfiles.sRGB"/></summary>
    public static ICompress DefaultCompression => new sRGBCompression();

    /// <summary><see cref="WorkingProfiles.sRGB"/></summary>
    public static Primary3 DefaultPrimary => sRGBPrimary;

    /// <summary><see cref="WorkingProfiles.sRGB"/></summary>
    public static Vector2 DefaultWhite => sRGBWhite;

    //...

    public Matrix Adapt { get; private set; } = LMS.Transform.Default;

    public Vector2 Chromacity { get; private set; } = DefaultWhite;

    public ICompress Compress { get; private set; } = DefaultCompression;

    public Primary3 Primary { get; private set; } = DefaultPrimary;

    /// <summary><see cref="White"/> = (<see cref="Vector3"/>)(<see cref="XYZ"/>)(<see cref="xyY"/>)(<see cref="xy"/>)<see cref="Chromacity"/></summary><remarks>Default = <see cref="Vector3.One"/></remarks>
    public readonly Vector3 White = Vector3.One;

    #endregion

    #region WorkingProfile

    public WorkingProfile() { }

    public WorkingProfile(Vector2 pR, Vector2 pG, Vector2 pB, Vector2 chromacity, ICompress compress, Matrix? adapt = null)
    {
        Primary = new(pR, pG, pB); Chromacity = chromacity; Compress = compress; Adapt = adapt ?? LMS.Transform.Default;
        White = (XYZ)(xyY)(xy)Chromacity;
    }

    public WorkingProfile(Primary3 primary, Vector2 chromacity, ICompress compress, Matrix? adapt = null)
    {
        Primary = primary; Chromacity = chromacity; Compress = compress; Adapt = adapt ?? LMS.Transform.Default;
        White = (XYZ)(xyY)(xy)Chromacity;
    }

    #endregion

    #region ==

    public static bool operator ==(WorkingProfile left, WorkingProfile right) => left.EqualsOverload(right);

    public static bool operator !=(WorkingProfile left, WorkingProfile right) => !(left == right);

    public bool Equals(WorkingProfile i) => this.Equals<WorkingProfile>(i) && Adapt == i.Adapt && Compress == i.Compress && Primary == i.Primary && Chromacity == i.Chromacity;

    public override bool Equals(object i) => i is WorkingProfile j && Equals(j);

    public override int GetHashCode() => XArray.New<object>(Adapt, Compress, Primary, Chromacity).GetHashCode();

    #endregion
}