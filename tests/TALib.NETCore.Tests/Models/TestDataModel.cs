using System.Numerics;

namespace TALib.NETCore.Tests.Models;

public class TestDataModel<T> where T : IFloatingPointIeee754<T>
{
    public string Name { get; set; } = null!;

    public T[][] Inputs { get; set; } = null!;

    public T[] Options { get; set; } = [];

    public int? Unstable { get; set; }

    public T[][] Outputs { get; set; } = null!;

    public bool Skip { get; set; }

    public override string ToString() => Name;
}
