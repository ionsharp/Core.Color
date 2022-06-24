using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System;
using System.Linq;
using System.Collections.Generic;
using static System.Double;
using static System.Math;

namespace Imagin.Core.Colors;

public static partial class Colour
{
    public static class Analysis
    {
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
                            x.Value /= 255; y.Value /= 255;

                            //Absolute difference
                            var rD = M.Clamp(1 - Abs(x.X - y.X).NaN(1), MaxValue);
                            var gD = M.Clamp(1 - Abs(x.Y - y.Y).NaN(1), MaxValue);
                            var bD = M.Clamp(1 - Abs(x.Z - y.Z).NaN(1), MaxValue);

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

        static void Compare(ColorModel m, int length, double[] minimum, double[] maximum)
        {
            for (var i = 0; i < length; i++)
            {
                if (m[i] < minimum[i])
                    minimum[i] = m[i];

                if (m[i] > maximum[i])
                    maximum[i] = m[i];
            }
        }

        static void Clean(int length, List<ColorModel> values, double[] minimum, double[] maximum)
        {
            var marked = new List<int>();
            for (var i = 0; i < length; i++)
            {
                var hell = values.Select(j => j[i]);

                double avg = hell.Average();
                double std = Sqrt(hell.Average(j => Pow(j - avg, 2)));

                for (var j = values.Count - 1; j >= 0; j--)
                {
                    if ((Abs(values[j][i] - avg)) > (2 * std))
                        marked.Add(j);
                }
            }

            for (var i = marked.Count - 1; i >= 0; i--)
                values.RemoveAt(marked[i]);

            foreach (var i in values)
                Compare(i, length, minimum, maximum);
        }

        static double[] GetArray(int length, double input)
        {
            var result = new double[length];
            for (var i = 0; i < length; i++)
                result[i] = input;

            return result;
        }

        /// <summary>
        /// Converts all <see cref="RGB"/> colors to the given <see cref="ColorModel">color space</see> and gets the estimated range of that <see cref="ColorModel">color space</see>.
        /// </summary>
        /// <param name="depth">A number in the range [1, ∞].</param>
        public static Range<Vector> GetRange(Type model, WorkingProfile profile, out double accuracy, bool reverse = false, uint depth = 10, int precision = 3, bool log = false)
        {
            var rgb = new RGB();
            var xyz = New(model);

            var dimension = xyz is ColorModel2 ? 2 : xyz is ColorModel3 ? 3 : xyz is ColorModel4 ? 4 : 0;

            var length = reverse ? 3 : xyz.Length;

            double[] minimum = new double[length], maximum = new double[length];
            for (var i = 0; i < length; i++)
            {
                minimum[i] = MaxValue; maximum[i] = MinValue;
            }

            var normalRange = new DoubleRange(0, 1);
            Vector min = Minimum(model), max = Maximum(model);

            List<ColorModel> values = new();

            void f(ColorModel m)
            {
                Compare(m, length, minimum, maximum);
                //values.Add(m);
            }

            try
            {
                if (reverse == true)
                {
                    double x, y, z, w;
                    for (var r = 0.0; r < depth; r++)
                    {
                        for (var g = 0.0; g < depth; g++)
                        {
                            if (dimension == 2)
                            {
                                x = r / (depth - 1); y = g / (depth - 1);

                                x = normalRange.Convert(min[0], max[0], x);
                                y = normalRange.Convert(min[1], max[1], y);

                                xyz = New(model, x, y);
                                xyz.To(out rgb, profile);
                                f(rgb);
                                continue;
                            }
                            for (var b = 0.0; b < depth; b++)
                            {
                                if (dimension == 3)
                                {
                                    x = r / (depth - 1); y = g / (depth - 1); z = b / (depth - 1);

                                    x = normalRange.Convert(min[0], max[0], x);
                                    y = normalRange.Convert(min[1], max[1], y);
                                    z = normalRange.Convert(min[2], max[2], z);

                                    xyz = New(model, x, y, z);
                                    xyz.To(out rgb, profile);
                                    f(rgb);
                                    continue;
                                }
                                for (var a = 0.0; a < depth; a++)
                                {
                                    x = r / (depth - 1); y = g / (depth - 1); z = b / (depth - 1); w = a / (depth - 1);

                                    x = normalRange.Convert(min[0], max[0], x);
                                    y = normalRange.Convert(min[1], max[1], y);
                                    z = normalRange.Convert(min[2], max[2], z);
                                    w = normalRange.Convert(min[3], max[3], w);

                                    xyz = New(model, x, y, z, w);
                                    xyz.To(out rgb, profile);
                                    f(rgb);
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (var r = 0.0; r < depth; r++)
                    {
                        for (var g = 0.0; g < depth; g++)
                        {
                            for (var b = 0.0; b < depth; b++)
                            {
                                //[0, 1]
                                double x = r / (depth - 1), y = g / (depth - 1), z = b / (depth - 1);

                                rgb = New<RGB>(x * 255, y * 255, z * 255);
                                xyz.From(rgb, profile);
                                f(xyz);
                            }
                        }
                    }
                }

                //Clean(length, values, minimum, maximum);

                var aV = new double[length];
                for (var i = 0; i < length; i++)
                {
                    var s = Abs(maximum[i]) + Abs(minimum[i]);
                    var t = reverse ? 255 : max[i] + Abs(min[i]);
                    aV[i] = ((s > t ? t / s : s / t) * 100).Round(precision);

                    minimum[i] = M.Clamp(minimum[i], 999, -999).Round(precision);
                    maximum[i] = M.Clamp(maximum[i], 999, -999).Round(precision);
                }

                accuracy = new Vector(aV).Sum() / length.Double();
                return new(minimum, maximum);
            }
            catch (Exception e)
            {
                log.If(true, () => Analytics.Log.Write<ColorModel>(e));

                accuracy = 0;
                return new(Vector3.Zero, Vector3.Zero);
            }
        }

        /// <summary>
        /// Converts all <see cref="RGB"/> colors to the given <see cref="ColorModel">color space</see> and gets the estimated range of that <see cref="ColorModel">color space</see>.
        /// </summary>
        /// <param name="depth">A number in the range [1, ∞].</param>
        public static Range<Vector> GetRange<T>(WorkingProfile profile, bool reverse, uint depth = 10, int precision = 3, bool log = false) where T : ColorModel 
            => GetRange(typeof(T), profile, out double _, reverse, depth, precision, log);
    }
}