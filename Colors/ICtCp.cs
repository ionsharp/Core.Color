using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

/// <summary>
/// <para>(🞩) <b>Intensity (I), Blue/yellow (Ct), Red/green (Cp)</b></para>
/// <para>≡ 0%</para>
/// <para><see cref="RGB"/> > <see cref="Lrgb"/> > <see cref="ICtCp"/></para>
/// 
/// <para>Requires <see cref="Rec2020Companding">Rec2100Companding</see>?</para>
/// 
/// <i>Alias</i>
/// <list type="bullet">
/// <item>ICTCP</item>
/// <item>ITP</item>
/// </list>
/// </summary>
/// <remarks></remarks>
[Component(0, 1, '%', "I", "Intensity")]
[Component(-1, 1, '%', "Ct", "Blue/yellow")]
[Component(-1, 1, '%', "Cp", "Red/green")]
[Serializable, Unfinished]
public sealed class ICtCp : ColorVector3
{
    public ICtCp(params double[] input) : base(input) { }

    public static implicit operator ICtCp(Vector3 input) => new(input.X, input.Y, input.Z);

    /// <summary><see cref="ICtCp"/> > <see cref="RGB"/></summary>
    public override Lrgb ToLrgb(WorkingProfile profile) => new();

    /// <summary><see cref="RGB"/> > <see cref="ICtCp"/></summary>
    public override void FromLrgb(Lrgb input, WorkingProfile profile) { }
}