namespace Imagin.Core.Colors;

public interface IConvert<T> where T : ColorModel3
{
    void From(T input, WorkingProfile profile);

    void To(out T result, WorkingProfile profile);
}