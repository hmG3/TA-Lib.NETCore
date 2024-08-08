using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using TALib.NETCore.Tests.Models;
using Xunit.Sdk;

namespace TALib.NETCore.Tests;

public sealed class JsonFileDataAttribute : DataAttribute
{
    private readonly string _filePath;
    private readonly Type _targetCollectionType;
    private readonly string? _propertyName;

    /// <summary>
    /// Load data from a JSON file as the data source for a theory
    /// </summary>
    /// <param name="filePath">The absolute or relative path to the JSON file to load</param>
    /// <param name="targetModelType">The type of values for the test model</param>
    /// <param name="propertyName">The name of the property on the JSON file that contains the data for the test</param>
    public JsonFileDataAttribute(string filePath, Type targetModelType, string? propertyName = null)
    {
        _filePath = filePath;
        _targetCollectionType = typeof(IEnumerable<>).MakeGenericType(typeof(TestDataModel<>).MakeGenericType(targetModelType));
        _propertyName = propertyName;
    }

    /// <inheritDoc />
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        ArgumentNullException.ThrowIfNull(testMethod);

        var path = Path.IsPathRooted(_filePath)
            ? _filePath
            : Path.GetRelativePath(Directory.GetCurrentDirectory(), _filePath);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Could not find file at path {path}");
        }

        IEnumerable<object> dataModels;

        using (var fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
        using (var dataDocument = JsonDocument.Parse(fileStream))
        {
            if (!String.IsNullOrEmpty(_propertyName))
            {
                if (!dataDocument.RootElement.TryGetProperty(_propertyName.AsSpan(), out var dataProperty) ||
                    dataProperty.ValueKind != JsonValueKind.Array)
                {
                    throw new JsonException($"Could not find property {_propertyName}");
                }

                dataModels = (dataProperty.ToObject(_targetCollectionType,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) as IEnumerable<object>)!;
            }
            else
            {
                dataModels = (dataDocument.ToObject(_targetCollectionType,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) as IEnumerable<object>)!;
            }
        }

        foreach (var dataModel in dataModels)
        {
            yield return [dataModel, Path.GetFileName(_filePath)];
        }
    }
}
