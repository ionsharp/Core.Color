using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

using static System.Double;
using static System.Math;

namespace Imagin.Core.Colors;

/// <summary>
/// Specifies an abstract mathematical model that describes the way colors can be represented as tuples of numbers.
/// <para>https://en.wikipedia.org/wiki/Color_model</para>
/// </summary>
[Serializable]
public abstract class ColorVector
{
    static readonly Dictionary<Type, Vector<Component>> Components = new();

    /// <summary>Gets the index (based on the type) corresponding to each <see cref="ColorVector"/> in the shader file (<see cref="FilePath"/>).</summary>
    public static Dictionary<Type, int> Index = new();

    /// <summary>Gets the type (based on the index) corresponding to each <see cref="ColorVector"/> in the shader file (<see cref="FilePath"/>).</summary>
    public static Dictionary<int, Type> Type = new();

    //...

    public Vector Value { get; protected set; }

    public double this[int i]
    {
        get => Value[i];
        set => Value = new(i == 0 ? value : Value[0], i == 1 ? value : Value[1], i == 2 ? value : Value[2]);
    }

    //...

    /// <remarks>Rearrange lines to match indices in relevant shader file.</remarks>
    static ColorVector()
    {
        var index = 0;
        void i<T>()
        {
            var type = typeof(T);
            Type
                .Add(index, type);
            Index
                .Add(typeof(T), index);

            var x = type.GetAttributes<ComponentAttribute>()?.ToArray();
            var y = type.GetAttributes<ComponentAttribute>()?.ToArray();
            Components.Add(type, x?.Length > 0 ? new(x[0].Info, x[1].Info, x[2].Info) : new(y[0].Info, y[1].Info, y[2].Info));

            index++;
        }

        i<RGB>();
        i<CMY>();
        i<HCV>();
        i<HCY>();
        i<HPLuv>();
        i<HSB>();
        i<HSBok>();
        i<HSL>();
        i<HSLok>();
        i<HSLuv>();
        i<HSM>();
        i<HSP>();
        i<HUVab>();
        i<HUVabh>();
        i<HUVuv>();
        i<HzUzVz>();
        i<HWB>();
        i<ICtCp>();
        i<JPEG>();
        i<JzAzBz>();
        i<JzCzHz>();
        i<Lab>();
        i<Labh>();
        i<LCHab>();
        i<LCHabh>();
        i<LCHuv>();
        i<LMS>();
        i<Luv>();
        i<OKLab>();
        i<TSL>();
        i<UCS>();
        i<UVW>();
        i<xvYCC>();
        i<xyY>();
        i<XYZ>();
        i<YCbCr>();
        i<YCoCg>();
        i<YDbDr>();
        i<YES>();
        i<YIQ>();
        i<YPbPr>();
        i<YUV>();
    }

    protected ColorVector(params double[] input) : base() => Value = new Vector(input);

    //...

    public static implicit operator Vector(ColorVector input) => input.Value;

    public static explicit operator double[](ColorVector input) => input.Value;

    //...

    public static Component GetComponent<T>(int index) => GetComponent(typeof(T), index);

    public static Component GetComponent<T>(Components component) => GetComponent(typeof(T), component);

    public static Component GetComponent(Type type, int index) => Components[type][index];

    public static Component GetComponent(Type type, Components component) => Components[type][(int)component];

    public static Vector<Component> GetComponents<T>() => Components[typeof(T)];

    public override string ToString() => Value.ToString();

    //... (Adapt)

    /// <summary><see cref="LMS"/> (0) : <see cref="LMS"/> (1)</summary>
    LMS Adapt(LMS input, WorkingProfile source, WorkingProfile target)
    {
        var sWhite = new LMS();
        sWhite.FromXYZ(source.White, source);

        var tWhite = new LMS();
        tWhite.FromXYZ(target.White, source);
        return new VonKriesChromaticAdaptation().Convert(input, sWhite, tWhite);
    }

    /// <summary><see cref="RGB"/> (0) : <see cref="XYZ"/> (0) : <see cref="LMS"/> (0) : <see cref="LMS"/> (1) : <see cref="XYZ"/> (1) : <see cref="RGB"/> (1)</summary>
    RGB Adapt(RGB input, WorkingProfile source, WorkingProfile target)
    {
        var xyz = new XYZ();
        xyz.FromLrgb(input.ToLrgb(source), source);

        xyz = Adapt(xyz, source, target);
        return xyz.ToLrgb(target).ToRGB(target);
    }

    /// <summary><see cref="XYZ"/> (0) : <see cref="LMS"/> (0) : <see cref="LMS"/> (1) : <see cref="XYZ"/> (1)</summary>
    XYZ Adapt(XYZ input, WorkingProfile source, WorkingProfile target)
    {
        var lms = new LMS();
        lms.FromLrgb(input.ToLrgb(source), source);

        lms = Adapt(lms, source, target);

        var result = new XYZ();
        result.FromLrgb(lms.ToLrgb(target), target);
        return result;
    }

    /// <summary><see cref="ColorVector">this</see> (0) : <see cref="RGB"/> (0) : <see cref="XYZ"/> (0) : <see cref="LMS"/> (0) : <see cref="LMS"/> (1) : <see cref="XYZ"/> (1) : <see cref="RGB"/> (1) : <see cref="ColorVector">this</see> (1)</summary>
    public ColorVector Adapt(WorkingProfile source, WorkingProfile target)
    {
        //this (0) : Adapt(XYZ) : this (1)
        if (this is RGB rgb)
            goto Default;

        //Adapt(XYZ)
        if (this is XYZ xyz)
            return Adapt(xyz, source, target);

        //this (0) : RGB (0) : Adapt(XYZ) : RGB (1) : this (1)
        rgb = ToLrgb(source).ToRGB(source);
        Default: return Adapt(rgb, source, target);
    }

    //... (Convert)

    public RGB ToRGB(WorkingProfile profile)
    {
        var rgb = new RGB();
        rgb.FromLrgb(ToLrgb(profile), profile);
        return rgb;
    }

    public abstract Lrgb ToLrgb(WorkingProfile profile);

    public void FromRGB(RGB input, WorkingProfile profile)
    {
        var lrgb = input.ToLrgb(profile);
        FromLrgb(lrgb, profile);
    }

    public abstract void FromLrgb(Lrgb input, WorkingProfile profile);

    //... (Equals)

    public bool Equals(ColorVector other) => Value.Equals(other.Value);

    public override bool Equals(object i) => i is ColorVector j && Equals(j);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(ColorVector left, ColorVector right) => Equals(left, right);

    public static bool operator !=(ColorVector left, ColorVector right) => !Equals(left, right);

    //... (Log)

    /// <summary>Converts all <see cref="RGB"/> colors to the given <see cref="ColorVector">color space</see> and back, and prints an estimate of accuracy for converting back and forth.</summary>
    /// <param name="precision">A number in the range [1, ∞].</param>
    public static void LogAccuracy(Type model, WorkingProfile profile, uint precision = 10)
    {
        try
        {
            var xyz = New(model);

            double sA = 0, n = 0;
            for (var r = 0.0; r < precision; r++)
            {
                for (var g = 0.0; g < precision; g++)
                {
                    for (var b = 0.0; b < precision; b++)
                    {
                        //RGB > Lrgb > *
                        var rgb0 = new RGB(r / (precision - 1), g / (precision - 1), b / (precision - 1));
                        xyz.FromRGB(rgb0, profile);

                        //* > Lrgb > RGB
                        var rgb1 = xyz.ToRGB(profile);

                        //Absolute difference
                        var rD = (1 - Abs(rgb0[0] - rgb1[0]).ReplaceNaN(1)).Clamp(MaxValue);
                        var gD = (1 - Abs(rgb0[1] - rgb1[1]).ReplaceNaN(1)).Clamp(MaxValue);
                        var bD = (1 - Abs(rgb0[2] - rgb1[2]).ReplaceNaN(1)).Clamp(MaxValue);

                        //Average of [absolute difference]
                        var dA = (rD + gD + bD) / 3;

                        //Sum of (average of [absolute difference])
                        sA += dA;
                        n++;
                    }
                }
            }
            Analytics.Log.Write<ColorVector>($"{model.Name} (Accuracy): \n{(sA / n * 100).Round(3)}%\n");
        }
        catch (Exception e)
        {
            Analytics.Log.Write<ColorVector>(e);
        }
    }

    /// <summary>Converts all <see cref="RGB"/> colors to the given <see cref="ColorVector">color space</see> and back, and prints an estimate of accuracy for converting back and forth.</summary>
    /// <param name="accuracy">A number in the range [1, ∞].</param>
    public static void LogAccuracy<T>(WorkingProfile profile, uint accuracy = 10) where T : ColorVector3 => LogAccuracy(typeof(T), profile, accuracy);

    /// <summary>
    /// Converts all <see cref="RGB"/> colors to the given <see cref="ColorVector">color space</see> and prints the estimated range of that <see cref="ColorVector">color space</see>.
    /// </summary>
    /// <param name="precision">A number in the range [1, ∞].</param>
    public static void LogRange(Type model, WorkingProfile profile, bool reverse = false, uint precision = 10)
    {
        Try.Invoke(() =>
        {
            var rgb = new RGB();
            var xyz = New(model);

            double minA = MaxValue, minB = MaxValue, minC = MaxValue, maxA = MinValue, maxB = MinValue, maxC = MinValue;
            for (var r = 0.0; r < precision; r++)
            {
                for (var g = 0.0; g < precision; g++)
                {
                    for (var b = 0.0; b < precision; b++)
                    {
                        //[0, 1]
                        var x = r / (precision - 1);
                        //[0, 1]
                        var y = g / (precision - 1);
                        //[0, 1]
                        var z = b / (precision - 1);

                        if (reverse)
                        {
                            var nRange = new DoubleRange(0, 1);

                            Vector min = GetMinimum(model); Vector max = GetMaximum(model);
                            x = nRange.Convert(min[0], max[0], x);
                            y = nRange.Convert(min[1], max[1], y);
                            z = nRange.Convert(min[2], max[2], z);

                            xyz = New(model, new Vector3<double>(x, y, z));
                            rgb = xyz.ToRGB(profile);

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
                            rgb = new RGB(x, y, z);
                            xyz.FromRGB(rgb, profile);

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
            Analytics.Log.Write<ColorVector>($"{model.Name} (Range): \n([{minA.Round(3)}, {minB.Round(3)}, {minC.Round(3)}], [{maxA.Round(3)}, {maxB.Round(3)}, {maxC.Round(3)}])\n");
        },
        e => Analytics.Log.Write<ColorVector>(e));
    }

    /// <summary>
    /// Converts all <see cref="RGB"/> colors to the given <see cref="ColorVector">color space</see> and prints the estimated range of that <see cref="ColorVector">color space</see>.
    /// </summary>
    /// <param name="accuracy">A number in the range [1, ∞].</param>
    public static void LogRange<T>(WorkingProfile profile, bool reverse, uint accuracy = 10) => LogRange(typeof(T), profile, reverse, accuracy);

    //... (New)

    public static ColorVector3 New(Type type) => type.Create<ColorVector3>();

    public static ColorVector3 New<T>() where T : ColorVector3 => New(typeof(T));

    public static ColorVector3 New(Type type, Vector3<double> values)
    {
        var result = New(type);
        result.Value = new(values);
        return result;
    }

    public static ColorVector3 New<T>(Vector3<double> values) => New(typeof(T), values);

    public static ColorVector3 New(Type type, RGB rgb, WorkingProfile profile)
    {
        var result = New(type);
        result.FromRGB(rgb, profile);
        return result;
    }

    public static ColorVector3 New<T>(RGB rgb, WorkingProfile profile) where T : ColorVector3 => New(typeof(T), rgb, profile);

    //... (Range)

    public static Vector GetMaximum(Type model) 
        => new(Components[model][0].Maximum, Components[model][1].Maximum, Components[model][2].Maximum);

    public static Vector GetMaximum<T>() 
        => GetMaximum(typeof(T));

    public static Vector GetMinimum(Type model) 
        => new(Components[model][0].Minimum, Components[model][1].Minimum, Components[model][2].Minimum);

    public static Vector GetMinimum<T>() 
        => GetMinimum(typeof(T));

    public static Range<Vector> GetRange(Type model) 
        => new(GetMinimum(model), GetMaximum(model));

    public static Range<Vector> GetRange<T>() 
        => GetRange(typeof(T));
}