using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🞩) <b>Intensity (I), Blue/yellow (Ct), Red/green (Cp)</b></para>
/// 
/// <para>A color representation format specified in the Rec. ITU-R BT.2100 standard that is used as a part of the color image pipeline in video and digital photography systems for high dynamic range (HDR) and wide color gamut (WCG) imagery.</para>
/// 
/// <para>≡ 0%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="ICtCp"/></para>
/// 
/// <para>Requires <see cref="Rec2020Transfer">Rec2100Companding</see>?</para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>ICTCP</item>
/// <item>ITP</item>
/// </list>
/// 
/// <i>Author</i>
/// <list type="bullet">
/// <item>Dolby Laboratories</item>
/// </list>
/// </summary>
/// <remarks></remarks>
[Component(1, '%', "I", "Intensity"), Component(-1, 1, '%', "Ct", "Blue/yellow"), Component(-1, 1, '%', "Cp", "Red/green")]
[Serializable, Unfinished]
public class ICtCp : ColorVector3
{
    public ICtCp(params double[] input) : base(input) { }

    public static implicit operator ICtCp(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary>(🞩) <see cref="ICtCp"/> > <see cref="Lrgb"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile) => new();

    /// <summary>(🞩) <see cref="Lrgb"/> > <see cref="ICtCp"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile) { }
}