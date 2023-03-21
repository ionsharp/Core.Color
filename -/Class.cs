using System;

namespace Imagin.Core.Colors;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ClassAttribute : Attribute
{
    public Class Class { get; set; }

    public ClassAttribute(Class @class) : base() => Class = @class;
}

[Flags]
public enum Class
{
    None = 0,
    CAM = 1, CMY = 2, H = 4, HC = 8, HS = 16, Lab = 32, Labk = 64, LCH = 128, LCHuv = 256, RGB = 512, XYZ = 1024, YUV = 2048
}