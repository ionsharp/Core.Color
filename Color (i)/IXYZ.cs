namespace Imagin.Core.Colors;

/// <summary>A color that converts to and from <see cref="XYZ"/>.</summary>
public interface IXYZ : IColor
{
    XYZ ToXYZ(WorkingProfile profile);

    void FromXYZ(XYZ input, WorkingProfile profile);
}