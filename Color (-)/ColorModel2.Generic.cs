namespace Imagin.Core.Colors;

public abstract class ColorModel2<T> : ColorModel2, IConvert<T> where T : ColorModel3, new()
{
    public ColorModel2() : base() { }

    public sealed override void From(Lrgb input, WorkingProfile profile)
    {
        var result = new T();
        result.From(input, profile);
        From(result, profile);
    }

    public sealed override Lrgb To(WorkingProfile profile)
    {
        To(out T result, profile);
        return result.To(profile);
    }

    public abstract void From(T input, WorkingProfile profile);

    public abstract void To(out T result, WorkingProfile profile);
}