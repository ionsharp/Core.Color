using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

#region (class) Component

public class Component : Base
{
    public string Name { get; private set; }

    public double Maximum { get; private set; } = 1;

    public double Minimum { get; private set; } = 0;

    public Range<double> Range => new(Minimum, Maximum);

    public string Symbol { get; private set; }

    public char Unit { get; private set; }

    public Component(char unit, string symbol, string name) : base()
    {
        Unit = unit; Symbol = symbol; Name = name?.Length > 0 ? name : symbol;
    }

    public Component(double minimum, double maximum, char unit, string symbol, string name) : this(unit, symbol, name) { Minimum = minimum; Maximum = maximum; }
}

#endregion

#region Component : Attribute

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ComponentAttribute : Attribute
{
    public Component Info { get; private set; }

    public ComponentAttribute(string symbol, string name = "") : this(' ', symbol, name) { }

    public ComponentAttribute(char unit, string symbol, string name = "") : this(default, default, unit, symbol, name) { }

    public ComponentAttribute(double maximum, char unit, string symbol, string name = "") : this(0, maximum, unit, symbol, name) { }

    public ComponentAttribute(double minimum, double maximum, char unit, string symbol, string name = "") : base()
        => Info = new Component(minimum, maximum, unit, symbol, name);

    public ComponentAttribute(double maximum, string symbol, string name = "") : this(0, maximum, symbol, name) { }

    public ComponentAttribute(double minimum, double maximum, string symbol, string name = "") : this(minimum, maximum, ' ', symbol, name) { }
}

#endregion