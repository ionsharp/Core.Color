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
public abstract class ColorVector : IEquatable<ColorVector>
{
    #region Fields

    static readonly Dictionary<Type, Vector<Component>> Components = new();

    /// <summary>Gets the index (based on the type) corresponding to each <see cref="ColorVector"/> in the shader file (<see cref="FilePath"/>).</summary>
    public static Dictionary<Type, int> Index = new();

    /// <summary>Gets the type (based on the index) corresponding to each <see cref="ColorVector"/> in the shader file (<see cref="FilePath"/>).</summary>
    public static Dictionary<int, Type> Type = new();

    #endregion

    #region Properties

    public Vector Value { get; protected set; }

    public double this[int i]
    {
        get => Value[i];
        set => Value = new(i == 0 ? value : Value[0], i == 1 ? value : Value[1], i == 2 ? value : Value[2]);
    }

    #endregion

    #region ColorVector

    protected ColorVector(params double[] input) : base() => Value = new Vector(input);

    public static implicit operator Vector(ColorVector input) => input.Value;

    public static explicit operator double[](ColorVector input) => input.Value;

    #endregion

    #region ColorVector (static)

    /// <remarks>Rearrange lines to match indices in relevant shader file.</remarks>
    static ColorVector()
    {
        #region i<T>()

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

        #endregion

        i<RGB>();
        i<CMY>();
        i<HCV>(); i<HCY>();
        i<HPLuv>();
        i<HSB>(); i<HSBk>();
        i<HSL>(); i<HSLk>(); i<HSLuv>();
        i<HSM>();
        i<HSP>();
        i<HWBsb>(); i<HWBsbk>();
        i<ICtCp>();
        i<JPEG>();
        i<Lab>(); i<Labh>(); i<Labi>(); i<Labj>(); i<Labk>();
        i<LCHab>(); i<LCHabh>(); i<LCHabj>(); i<LCHuv>();
        i<LMS>(); i<Luv>();
        i<TSL>();
        i<UCS>(); i<UVW>();
        i<xvYCC>(); i<xyY>(); i<xyYC>(); i<XYZ>();
        i<YCbCr>(); i<YCoCg>(); i<YDbDr>(); i<YES>(); i<YIQ>(); i<YPbPr>(); i<YUV>();
    }

    #endregion

    #region Methods

    public override string ToString() => Value.ToString();

    #endregion

    #region Adapt

    /// <summary>(🗸) <see cref="LMS"/> (0) > <see cref="LMS"/> (1)</summary>
    protected LMS Adapt(LMS input, WorkingProfile source, WorkingProfile target)
    {        
        //XYZ (0) > LMS (0)
        var a = new LMS();
        a.FromXYZ(source.White, source);

        //XYZ (1) > LMS (1)
        var b = new LMS();
        b.FromXYZ(target.White, target);

        //LMS (0) > LMS (1)
        return new VonKriesAdaptation().Convert(input, a, b);
    }

    /// <summary>(🗸) <see cref="RGB"/> (0) > <see cref="XYZ"/> (0) > <see cref="LMS"/> (0) > <see cref="LMS"/> (1) > <see cref="XYZ"/> (1) > <see cref="RGB"/> (1)</summary>
    protected RGB Adapt(RGB input, WorkingProfile source, WorkingProfile target)
    {
        //RGB (0) > XYZ (0)
        var xyz = new XYZ();
        xyz.FromRGB(input, source);

        //XYZ (0) > LMS (0) > LMS (1) > XYZ (1)
        xyz = Adapt(xyz, source, target);

        //XYZ (1) > RGB (1)
        return xyz.ToRGB(target);
    }

    /// <summary>(🗸) <see cref="XYZ"/> (0) > <see cref="LMS"/> (0) > <see cref="LMS"/> (1) > <see cref="XYZ"/> (1)</summary>
    protected XYZ Adapt(XYZ input, WorkingProfile source, WorkingProfile target)
    {
        //XYZ (0) > LMS (0)
        var lms = new LMS();
        lms.FromXYZ(input, source);

        //LMS (0) > LMS (1)
        lms = Adapt(lms, source, target);

        //LMS (1) > XYZ (1)
        return lms.ToXYZ(target);
    }

    /// <summary>(🗸) <see cref="ColorVector">this</see> (0) > <see cref="RGB"/> (0) > <see cref="XYZ"/> (0) > <see cref="LMS"/> (0) > <see cref="LMS"/> (1) > <see cref="XYZ"/> (1) > <see cref="RGB"/> (1) > <see cref="ColorVector">this</see> (1)</summary>
    public virtual void Adapt(WorkingProfile source, WorkingProfile target)
    {
        var result = ToRGB(source);
        Value = Adapt(result, source, target);
    }

    #endregion

    #region this > (L)rgb

    /// <summary>(🗸) this > <see cref="Lrgb"/> > <see cref="RGB"/></summary>
    public RGB ToRGB(WorkingProfile profile)
    {
        var a = ToLrgb(profile);

        var b = new RGB();
        b.FromLrgb(a, profile);
        return b;
    }

    /// <summary>(🗸) this > <see cref="Lrgb"/></summary>
    public abstract Lrgb ToLrgb(WorkingProfile profile);

    #endregion

    #region (L)rgb > this

    /// <summary>(🗸) <see cref="RGB"/> > <see cref="Lrgb"/> > this</summary>
    public void FromRGB(RGB input, WorkingProfile profile)
    {
        var result = input.ToLrgb(profile);
        FromLrgb(result, profile);
    }

    /// <summary>(🗸) <see cref="Lrgb"/> > this</summary>
    public abstract void FromLrgb(Lrgb input, WorkingProfile profile);

    #endregion

    #region ==

    public bool Equals(ColorVector other) => Value.Equals(other.Value);

    public override bool Equals(object i) => i is ColorVector j && Equals(j);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(ColorVector left, ColorVector right) => Equals(left, right);

    public static bool operator !=(ColorVector left, ColorVector right) => !Equals(left, right);

    #endregion

    #region New

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

    #endregion

    #region Static

    /// <summary>Converts all <see cref="RGB"/> colors to the given <see cref="ColorVector">color space</see> and back, and gets an estimate of accuracy for converting back and forth.</summary>
    /// <param name="depth">A number in the range [1, ∞].</param>
    public static double GetAccuracy(Type model, WorkingProfile profile, uint depth = 10, int precision = 3)
    {
        try
        {
            var xyz = New(model);

            double sA = 0, n = 0;
            for (var r = 0.0; r < depth; r++)
            {
                for (var g = 0.0; g < depth; g++)
                {
                    for (var b = 0.0; b < depth; b++)
                    {
                        //RGB > Lrgb > *
                        var rgb0 = new RGB(r / (depth - 1), g / (depth - 1), b / (depth - 1));
                        xyz.FromRGB(rgb0, profile);

                        //* > Lrgb > RGB
                        var rgb1 = xyz.ToRGB(profile);

                        //Absolute difference
                        var rD = M.Clamp(1 - Abs(rgb0[0] - rgb1[0]).NaN(1), MaxValue);
                        var gD = M.Clamp(1 - Abs(rgb0[1] - rgb1[1]).NaN(1), MaxValue);
                        var bD = M.Clamp(1 - Abs(rgb0[2] - rgb1[2]).NaN(1), MaxValue);

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
            Analytics.Log.Write<ColorVector>(e);
            return 0;
        }
    }

    /// <summary>Converts all <see cref="RGB"/> colors to the given <see cref="ColorVector">color space</see> and back, and gets an estimate of accuracy for converting back and forth.</summary>
    /// <param name="depth">A number in the range [1, ∞].</param>
    public static double GetAccuracy<T>(WorkingProfile profile, uint depth = 10, int precision = 3) where T : ColorVector3 => GetAccuracy(typeof(T), profile, depth, precision);

    //...

    public static Component GetComponent<T>(int index) => GetComponent(typeof(T), index);

    public static Component GetComponent<T>(Components component) => GetComponent(typeof(T), component);

    public static Component GetComponent(Type type, int index) => Components[type][index];

    public static Component GetComponent(Type type, Components component) => Components[type][(int)component];

    public static Vector<Component> GetComponents<T>() => Components[typeof(T)];

    //...

    public static Vector GetMaximum(Type model) 
        => new(Components[model][0].Maximum, Components[model][1].Maximum, Components[model][2].Maximum);

    public static Vector GetMaximum<T>() 
        => GetMaximum(typeof(T));

    public static Vector GetMinimum(Type model) 
        => new(Components[model][0].Minimum, Components[model][1].Minimum, Components[model][2].Minimum);

    public static Vector GetMinimum<T>() 
        => GetMinimum(typeof(T));

    //...

    public static Range<Vector> GetRange(Type model) 
        => new(GetMinimum(model), GetMaximum(model));

    public static Range<Vector> GetRange<T>() 
        => GetRange(typeof(T));

    //...

    /// <summary>
    /// Converts all <see cref="RGB"/> colors to the given <see cref="ColorVector">color space</see> and gets the estimated range of that <see cref="ColorVector">color space</see>.
    /// </summary>
    /// <param name="depth">A number in the range [1, ∞].</param>
    public static Range<Vector3> GetRange(Type model, WorkingProfile profile, bool reverse = false, uint depth = 10, int precision = 3)
    {
        try
        {
            var rgb = new RGB();
            var xyz = New(model);

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
            return new(new(minA.Round(precision), minB.Round(precision), minC.Round(precision)), new(maxA.Round(precision), maxB.Round(precision), maxC.Round(precision)));
        }
        catch (Exception e)
        {
            Analytics.Log.Write<ColorVector>(e);
            return new(Vector3.Zero, Vector3.Zero);
        }
    }

    /// <summary>
    /// Converts all <see cref="RGB"/> colors to the given <see cref="ColorVector">color space</see> and gets the estimated range of that <see cref="ColorVector">color space</see>.
    /// </summary>
    /// <param name="depth">A number in the range [1, ∞].</param>
    public static Range<Vector3> GetRange<T>(WorkingProfile profile, bool reverse, uint depth = 10, int precision = 3) => GetRange(typeof(T), profile, reverse, depth, precision);

    #endregion
}