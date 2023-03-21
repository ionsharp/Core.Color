using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using Imagin.Core.Reflection;
using System;

namespace Imagin.Core.Colors;

/// <summary>In colorimetry, this is referred to as an "<see cref="WorkingProfile">RGB Working Space</see>".</summary>
[Image(AssemblyType.Core, "Channels.png"), Serializable]
public partial struct WorkingProfile : IEquatable<WorkingProfile>
{
    enum Category { Chromacity }

    #region Properties

    public static WorkingProfile Default => WorkingProfiles.sRGB;

    ///

    public static ICompress sRGBCompression => new Compression(12 / 5, 1.055, 0.0031308, 12.92, 0.04045);

    public static Primary3 sRGBPrimary => new(new(0.6400, 0.3300), new(0.3000, 0.6000), new(0.1500, 0.0600));

    public static Vector2 sRGBWhite => Illuminant2.D65;

    ///

    /// <summary><see cref="WorkingProfiles.sRGB"/></summary>
    public static ICompress DefaultCompression => sRGBCompression;

    /// <summary><see cref="WorkingProfiles.sRGB"/></summary>
    public static Primary3 DefaultPrimary => sRGBPrimary;

    /// <summary>What should these values be...?</summary>
    public static CAM02.ViewingConditions DefaultViewingConditions => new();

    /// <summary><see cref="WorkingProfiles.sRGB"/></summary>
    public static Vector2 DefaultWhite => sRGBWhite;

    ///

    [Caption("The matrix used to adapt a color when the input and output working profile differ.")]
    [Index(3)]
    public Matrix Adaptation { get; private set; } = ChromaticAdaptationTransform.Default;

    [Caption("The chromacity coordinates in 2D space.")]
    [Horizontal, Index(0)]
    public Vector2 Chromacity { get; private set; } = DefaultWhite;

    [Assign(typeof(Compression), typeof(GammaCompression), typeof(GammaLogCompression), typeof(PQCompression))]
    [Caption("The transfer function used for compression.")]
    [Index(2)]
    public ICompress Compression { get; private set; } = DefaultCompression;

    [Caption("The red, green, and blue primary coordinates.")]
    [Horizontal, Index(1)]
    public Primary3 Primary { get; private set; } = DefaultPrimary;

    [Name("Conditions"), Index(4)]
    public CAM02.ViewingConditions ViewingConditions { get; private set; } = DefaultViewingConditions;

    /// <summary><see cref="White"/> = (<see cref="Vector3"/>)(<see cref="XYZ"/>)(<see cref="xyY"/>)(<see cref="xy"/>)<see cref="Chromacity"/></summary><remarks>Default = <see cref="Vector3.One"/></remarks>
    [Hide]
    public Vector3 White { get; private set; } = Vector3.One;

    #endregion

    #region WorkingProfile

    public WorkingProfile() : this(DefaultPrimary, DefaultWhite, DefaultCompression, ChromaticAdaptationTransform.Default, DefaultViewingConditions) { }

    public WorkingProfile(Vector2 pR, Vector2 pG, Vector2 pB, Vector2 chromacity, ICompress compression, Matrix? adaptation = null, CAM02.ViewingConditions? viewingConditions = null)
    {
        Primary = new(pR, pG, pB); Chromacity = chromacity; Compression = compression; Adaptation = adaptation ?? ChromaticAdaptationTransform.Default; 
        ViewingConditions = viewingConditions ?? DefaultViewingConditions;
        White = (XYZ)(xyY)(xy)Chromacity;
    }

    public WorkingProfile(Primary3 primary, Vector2 chromacity, ICompress compression, Matrix? adaptation = null, CAM02.ViewingConditions? viewingConditions = null)
    {
        Primary = primary; Chromacity = chromacity; Compression = compression; Adaptation = adaptation ?? ChromaticAdaptationTransform.Default; 
        ViewingConditions = viewingConditions ?? DefaultViewingConditions;
        White = (XYZ)(xyY)(xy)Chromacity;
    }

    #endregion

    #region ==

    public static bool operator ==(WorkingProfile left, WorkingProfile right) => left.EqualsOverload(right);

    public static bool operator !=(WorkingProfile left, WorkingProfile right) => !(left == right);

    public bool Equals(WorkingProfile i) => this.Equals<WorkingProfile>(i) && Adaptation == i.Adaptation && Compression == i.Compression && Primary == i.Primary && Chromacity == i.Chromacity;

    public override bool Equals(object i) => i is WorkingProfile j && Equals(j);

    public override int GetHashCode() => XArray.New<object>(Adaptation, Compression, Primary, Chromacity).GetHashCode();

    #endregion
}