using System;

namespace Imagin.Core.Colors;

[AttributeUsage(AttributeTargets.Field)]
public class ColorAttribute : Attribute
{
    public readonly string Hexadecimal;

    public ColorAttribute(string hexadecimal) : base() => Hexadecimal = hexadecimal;
}