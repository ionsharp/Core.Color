using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Colors;

[Serializable]
public class ColorMatrix : Matrix<Vector4>
{
    public ColorMatrix(uint rows, uint columns) : base(rows, columns) { }

    public ColorMatrix(Vector4[][] input) : base(input) { }

    public ColorMatrix(Vector4[,] input) : base(input) { }

    public ColorMatrix(Matrix<Vector4> input) : base(input) { }
}