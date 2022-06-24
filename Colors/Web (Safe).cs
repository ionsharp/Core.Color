using System.Collections.Generic;

namespace Imagin.Core.Colors;

/// <summary>https://en.wikipedia.org/wiki/Web_colors#Web-safe_colors</summary>
public static class SafeWebColors
{
    public static readonly List<string> Colors = new();

    static SafeWebColors()
    {
        var key = new string[] { "0", "3", "6", "9", "C", "F" };

        for (var x = 0; x < 6; x++)
        {
            for (var y = 0; y < 6; y++)
            {
                for (var z = 0; z < 6; z++)
                    Colors.Add($"{key[x]}{key[y]}{key[z]}");
            }
        }
    }
}