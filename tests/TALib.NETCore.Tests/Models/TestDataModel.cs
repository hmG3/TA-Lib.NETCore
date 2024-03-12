using System;

namespace TALib.NETCore.Tests.Models;

public class TestDataModel<T> where T : struct
{
    public string Name { get; set; } = null!;

    public T[][] Inputs { get; set; } = null!;

    public T[] Options { get; set; } = Array.Empty<T>();

    public T[][] Outputs { get; set; } = null!;

    public bool Skip { get; set; }

    public override string ToString() => Name;
}
