using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System;
using static System.Double;
using static System.Math;

namespace Imagin.Core.Colors;

public static partial class Colour
{
    public static class Analysis
    {
        #region Output

        #region [6/14/2022] Accuracy

        /*
        (🗸) [0] RGB
        100%

        (🞩) [1] RYB
        59.122%

        (🗸) [2] CMY
        100%

        (🞩) [3] HCV
        98.379%

        (🞩) [4] HCY
        99.3%

        (🞩) [5] HPLuv
        69.233%

        (🞩) [6] HSB
        76.778%

        (🞩) [7] HSBk
        0%

        (🗸) [8] HSL
        100%

        (🞩) [9] HSLk
        0%

        (🞩) [10] HSLuv
        69.233%

        (🞩) [11] HSM
        62.752%

        (🞩) [12] HSP
        99.815%

        (🞩) [13] HWB
        76.778%

        (🞩) [14] IPT
        74.537%

        (🗸) [15] JPEG
        100%

        (🗸) [16] Lab
        100%

        (🗸) [17] Labh
        100%

        (🞩) [18] Labi
        50%

        (🗸) [19] Labj
        100%

        (🞩) [20] Labk
        0%

        (🗸) [21] LCHab
        100%

        (🗸) [22] LCHabh
        100%

        (🗸) [23] LCHabj
        100%

        (🞩) [24] LCHuv
        67.628%

        (🗸) [25] LCHxy
        100%

        (🗸) [26] LMS
        100%

        (🞩) [27] Luv
        67.628%

        (🞩) [28] TSL
        36.884%

        (🗸) [29] UCS
        100%

        (🞩) [30] UVW
        56.288%

        (🗸) [31] xvYCC
        100%

        (🗸) [32] xyY
        100%

        (🞩) [33] xyYC
        0%

        (🗸) [34] XYZ
        100%

        (🗸) [35] YCbCr
        100%

        (🗸) [36] YCoCg
        100%

        (🗸) [37] YDbDr
        100%

        (🗸) [38] YES
        100%

        (🞩) [39] YIQ
        99.765%

        (🗸) [40] YPbPr
        100%

        (🞩) [41] YUV
        99.886%

        (🗸) [42] CMYK
        100%

        (🞩) [43] RGBW
        51.8%
        */

        #endregion

        #region [6/13/2022] Range

        /*
        (0) RGB
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 255, y = 255, z = 255


        (1) CMY
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 1, y = 1, z = 1


        (2) HCV
        Minimum = x = -60, y = 0, z = 0, Maximum = x = 285.936, y = 100, z = 100


        (3) HCY
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 359.409, y = 100, z = 255


        (4) HPLuv
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 359.845, y = 60832.301, z = 100


        (5) HSB
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 359.29, y = 100, z = 100


        (6) HSBk
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 0, y = 0, z = 0


        (7) HSL
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 359.29, y = 100, z = 100


        (8) HSLk
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 0, y = 0, z = 0


        (9) HSLuv
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 359.845, y = 35166.492, z = 100


        (10) HSM
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 359.466, y = 100, z = 255


        (11) HSP
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 360, y = 100, z = 255


        (12) HWB
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 359.29, y = 100, z = 100


        (13) IPT
        Minimum = x = 0, y = -1.349, z = -0.911, Maximum = x = 1.004, y = 1.929, z = 0.687


        (14) JPEG
        Minimum = x = 79.068, y = 53.3, z = 33.848, Maximum = x = 175.437, y = 203.544, z = 219.652


        (15) Lab
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 0, y = 0, z = 0


        (16) Labh
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 0, y = 0, z = 0


        (17) Labi
        Minimum = x = -5.835, y = -4.577, z = -3.89, Maximum = x = -5.835, y = 2.838, z = 2.606


        (18) Labj
        Minimum = x = 0, y = -0.179, z = -0.323, Maximum = x = 0.989, y = 0.225, z = 0.223


        (19) Labk
        Minimum = x = 0, y = -0.22, z = -0.295, Maximum = x = 1.003, y = 0.307, z = 0.202


        (20) LCHab
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 0, y = 0, z = 0


        (21) LCHabh
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 0, y = 0, z = 0


        (22) LCHabj
        Minimum = x = 0, y = 0, z = 0.528, Maximum = x = 0.989, y = 0.34, z = 359.725


        (23) LCHuv
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 152.029, y = 283.472, z = 359.845


        (24) LCHxy
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 0.64, y = 1.056, z = 71.788


        (25) LMS
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 0.941, y = 1.04, z = 1.09


        (26) Luv
        Minimum = x = 0, y = -127.119, z = -219.808, Maximum = x = 152.029, y = 277.097, z = 164.364


        (27) TSL
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 0.248, y = 1, z = 1


        (28) UCS
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 0.634, y = 1, z = 1.569


        (29) UVW
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 0, y = 0, z = 0


        (30) xvYCC
        Minimum = x = 16, y = 16, z = 16, Maximum = x = 235, y = 240, z = 240


        (31) xyY
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 0.64, y = 0.6, z = 1


        (32) xyYC
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 0, y = 0, z = 0


        (33) XYZ
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 0.95, y = 1, z = 1.089


        (34) YCbCr
        Minimum = x = 16, y = 16, z = 16, Maximum = x = 235, y = 240, z = 240


        (35) YCoCg
        Minimum = x = 0, y = -0.5, z = -0.5, Maximum = x = 1, y = 0.5, z = 0.5


        (36) YDbDr
        Minimum = x = 0, y = -1.333, z = -1.333, Maximum = x = 1, y = 1.333, z = 1.333


        (37) YES
        Minimum = x = 0, y = -0.5, z = -0.5, Maximum = x = 1, y = 0.5, z = 0.5


        (38) YIQ
        Minimum = x = 0, y = -0.596, z = -0.528, Maximum = x = 1, y = 0.596, z = 0.523


        (39) YPbPr
        Minimum = x = 0, y = -0.5, z = -0.5, Maximum = x = 1, y = 0.5, z = 0.5


        (40) YUV
        Minimum = x = 0, y = -0.436, z = -0.615, Maximum = x = 1, y = 0.436, z = 0.615


        (41) RYB
        Minimum = x = 0, y = 0, z = 0, Maximum = x = 1, y = 1, z = 1
        */

        #endregion

        #endregion

        #region Methods

        /// <summary>Converts all <see cref="RGB"/> colors to the given <see cref="ColorModel">color space</see> and back, and gets an estimate of accuracy for converting back and forth.</summary>
        /// <param name="depth">A number in the range [1, ∞].</param>
        public static double GetAccuracy(Type model, WorkingProfile profile, uint depth = 10, int precision = 3, bool log = false)
        {
            try
            {
                var color = New(model);

                double sA = 0, n = 0;
                for (double r = 0; r < depth; r++)
                {
                    for (double g = 0; g < depth; g++)
                    {
                        for (double b = 0; b < depth; b++)
                        {
                            //RGB > Lrgb > *
                            var x = New<RGB>(r / (depth - 1) * 255, g / (depth - 1) * 255, b / (depth - 1) * 255);
                            color.From(x, profile);

                            //* > Lrgb > RGB
                            color.To(out RGB y, profile);

                            //Normalize
                            x.XYZ /= 255; y.XYZ /= 255;

                            //Absolute difference
                            var rD = M.Clamp(1 - Abs(x[0] - y[0]).NaN(1), MaxValue);
                            var gD = M.Clamp(1 - Abs(x[1] - y[1]).NaN(1), MaxValue);
                            var bD = M.Clamp(1 - Abs(x[2] - y[2]).NaN(1), MaxValue);

                            //Average of [absolute difference]
                            var dA = (rD + gD + bD) / 3;

                            //Sum of (average of [absolute difference])
                            sA += dA;
                            n++;
                        }
                    }
                }
                return (sA / n * 100).Round(precision);
            }
            catch (Exception e)
            {
                log.If(true, () => Analytics.Log.Write<ColorModel>(e));
                return 0;
            }
        }

        /// <summary>Converts all <see cref="RGB"/> colors to the given <see cref="ColorModel">color space</see> and back, and gets an estimate of accuracy for converting back and forth.</summary>
        /// <param name="depth">A number in the range [1, ∞].</param>
        public static double GetAccuracy<T>(WorkingProfile profile, uint depth = 10, int precision = 3, bool log = false) where T : ColorModel3 => GetAccuracy(typeof(T), profile, depth, precision, log);

        //...

        /// <summary>
        /// Converts all <see cref="RGB"/> colors to the given <see cref="ColorModel">color space</see> and gets the estimated range of that <see cref="ColorModel">color space</see>.
        /// </summary>
        /// <param name="depth">A number in the range [1, ∞].</param>
        public static Range<Vector3> GetRange(Type model, WorkingProfile profile, bool reverse = false, uint depth = 10, int precision = 3, bool log = false)
        {
            try
            {
                var rgb = new RGB();
                var xyz = Colour.New(model);

                double minA = MaxValue, minB = MaxValue, minC = MaxValue, maxA = MinValue, maxB = MinValue, maxC = MinValue;
                for (var r = 0.0; r < depth; r++)
                {
                    for (var g = 0.0; g < depth; g++)
                    {
                        for (var b = 0.0; b < depth; b++)
                        {
                            //[0, 1]
                            var x = r / (depth - 1);
                            //[0, 1]
                            var y = g / (depth - 1);
                            //[0, 1]
                            var z = b / (depth - 1);

                            if (reverse)
                            {
                                var nRange = new DoubleRange(0, 1);

                                Vector min = Colour.Minimum(model); Vector max = Colour.Maximum(model);
                                x = nRange.Convert(min[0], max[0], x);
                                y = nRange.Convert(min[1], max[1], y);
                                z = nRange.Convert(min[2], max[2], z);

                                xyz = Colour.New(model, x, y, z);
                                xyz.To(out rgb, profile);

                                if (rgb[0] < minA)
                                    minA = rgb[0];
                                if (rgb[1] < minB)
                                    minB = rgb[1];
                                if (rgb[2] < minC)
                                    minC = rgb[2];

                                if (rgb[0] > maxA)
                                    maxA = rgb[0];
                                if (rgb[1] > maxB)
                                    maxB = rgb[1];
                                if (rgb[2] > maxC)
                                    maxC = rgb[2];
                            }
                            else
                            {
                                rgb = Colour.New<RGB>(x * 255, y * 255, z * 255);
                                xyz.From(rgb, profile);

                                if (xyz[0] < minA)
                                    minA = xyz[0];
                                if (xyz[1] < minB)
                                    minB = xyz[1];
                                if (xyz[2] < minC)
                                    minC = xyz[2];

                                if (xyz[0] > maxA)
                                    maxA = xyz[0];
                                if (xyz[1] > maxB)
                                    maxB = xyz[1];
                                if (xyz[2] > maxC)
                                    maxC = xyz[2];
                            }
                        }
                    }
                }
                return new(new(minA.Round(precision), minB.Round(precision), minC.Round(precision)), new(maxA.Round(precision), maxB.Round(precision), maxC.Round(precision)));
            }
            catch (Exception e)
            {
                log.If(true, () => Analytics.Log.Write<ColorModel>(e));
                return new(Vector3.Zero, Vector3.Zero);
            }
        }

        /// <summary>
        /// Converts all <see cref="RGB"/> colors to the given <see cref="ColorModel">color space</see> and gets the estimated range of that <see cref="ColorModel">color space</see>.
        /// </summary>
        /// <param name="depth">A number in the range [1, ∞].</param>
        public static Range<Vector3> GetRange<T>(WorkingProfile profile, bool reverse, uint depth = 10, int precision = 3, bool log = false) => GetRange(typeof(T), profile, reverse, depth, precision, log);

        //...

        public static void LogAllAccuracy(WorkingProfile profile, uint depth = 10, int precision = 3, bool log = false)
        {
            var j = 0;
            Colour.Types.ForEach(i =>
            {
                var result = GetAccuracy(i, profile, depth, precision, log);
                Analytics.Log.Write<ColorModel>($"({(result == 100 ? "🗸" : "🞩")}) [{j}] {i.Name}\n{result}%\n");
                j++;
            });
        }

        public static void LogAllRange(WorkingProfile profile, bool reverse = false, uint depth = 10, int precision = 3, bool log = false)
        {
            var j = 0;
            Colour.Types.ForEach(i =>
            {
                Analytics.Log.Write<ColorModel>($"({j}) {i.Name}\n{GetRange(i, profile, reverse, depth, precision, log)}\n");
                j++;
            });
        }

        #endregion
    }
}