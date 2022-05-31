using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Imagin.Core.Colors;

public class Histogram : Base
{
    ObservableCollection<Vector2> red = null;
    public ObservableCollection<Vector2> Red
    {
        get => red;
        set => this.Change(ref red, value, () => Red);
    }

    ObservableCollection<Vector2> green = null;
    public ObservableCollection<Vector2> Green
    {
        get => green;
        set => this.Change(ref green, value, () => Green);
    }

    ObservableCollection<Vector2> blue = null;
    public ObservableCollection<Vector2> Blue
    {
        get => blue;
        set => this.Change(ref blue, value, () => Blue);
    }

    ObservableCollection<Vector2> saturation = null;
    public ObservableCollection<Vector2> Saturation
    {
        get => saturation;
        set => this.Change(ref saturation, value, () => Saturation);
    }

    ObservableCollection<Vector2> luminance = null;
    public ObservableCollection<Vector2> Luminance
    {
        get => luminance;
        set => this.Change(ref luminance, value, () => Luminance);
    }

    public Histogram() : base() { }

    static int[] Smooth(int[] input)
    {
        var result = new int[input.Length];
        var mask = new double[]
        {
            0.25, 0.5, 0.25
        };

        for (var i = 1; i < input.Length - 1; i++)
        {
            var value = 0.0;

            for (int j = 0; j < mask.Length; j++)
                value += input[i - 1 + j] * mask[j];

            result[i] = (int)value;
        }
        return result;
    }

    public async Task Refresh(ColorMatrix colors, WorkingProfile profile, bool smooth = true)
    {
        ObservableCollection<Vector2> rX = new(), gX = new(), bX = new(), sX = new(), lX = new();
        IEnumerable<Vector2> rY = null, gY = null, bY = null, sY = null, lY = null;

        await Task.Run(() =>
        {
            Try.Invoke(() =>
            {
                int[] r = new int[256];
                int[] g = new int[256];
                int[] b = new int[256];
                int[] s = new int[256];
                int[] l = new int[256];

                RGB rgb = new();
                HSL hsl = new();

                colors.Each(color =>
                {
                    var dX = M.Denormalize(color.X);
                    var dY = M.Denormalize(color.Y);
                    var dZ = M.Denormalize(color.Z);

                    r[dX]++; g[dY]++; b[dZ]++;
                    rgb = new(dX, dY, dZ);

                    hsl.FromRGB(rgb, profile);

                    s[(int)(hsl.Y / 100 * 255)]++;
                    l[(int)(hsl.Z / 100 * 255)]++;
                    return color;
                });

                rY = GetPoints(r, smooth);
                gY = GetPoints(g, smooth);
                bY = GetPoints(b, smooth);
                sY = GetPoints(s, smooth);
                lY = GetPoints(l, smooth);
            });
        });

        rY?.ForEach(i => rX.Add(i));
        gY?.ForEach(i => gX.Add(i));
        bY?.ForEach(i => bX.Add(i));
        sY?.ForEach(i => sX.Add(i));
        lY?.ForEach(i => lX.Add(i));

        Red
            = rX;
        Green
            = gX;
        Blue
            = bX;
        Saturation
            = sX;
        Luminance
            = lX;
    }

    IEnumerable<Vector2> GetPoints(int[] input, bool smooth = true)
    {
        input = smooth ? Smooth(input) : input;

        var max = input.Max();

        //First point (lower-left corner)
        yield return new Vector2(0, max);

        //Middle points
        for (int i = 0; i < input.Length; i++)
            yield return new Vector2(i, max - input[i]);

        //Last point (lower-right corner)
        yield return new Vector2(input.Length - 1, max);
    }
}