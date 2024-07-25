using System;
using System.Linq;
using System.Numerics;
using Shouldly;
using TALib.NETCore.Tests.Models;
using Xunit;

namespace TALib.NETCore.Tests;

public sealed class FunctionTests
{
    [SkippableTheory]
    [JsonFileData("DataSets/untest.json", typeof(double), "_")]
    [JsonFileData("DataSets/atoz.json", typeof(double), "_")]
    [JsonFileData("DataSets/extra.json", typeof(double), "_")]
    [JsonFileData("DataSets/spy.json", typeof(double), "_")]
    [JsonFileData("DataSets/unst.json", typeof(double), "_")]
    [JsonFileData("DataSets/cdl.json", typeof(double), "_")]
#pragma warning disable xUnit1026
    public void ShouldCalculateSuccessfullyForDoubleInputAndSeparateOutput(TestDataModel<double> model, string fileName)
#pragma warning restore xUnit1026
    {
        Skip.If(model.Skip, "Test marked as skipped in the dataset.");

        const double equalityTolerance = 0.001d;

        Abstract.All.FunctionsDefinition.ShouldContainKey(model.Name, $"Cannot find definition for '{model.Name}");
        var function = Abstract.All.FunctionsDefinition[model.Name];

        model.Options.Length.ShouldBe(function.Options.Length, "Number of options must match the definition");
        var inputOffset = function.Lookback(Array.ConvertAll(model.Options, d => (int) d));
        model.Inputs.Length.ShouldBe(function.Inputs.Length, "Number of inputs must match the definition");
        var outputLength = model.Inputs[0].Length - inputOffset;
        outputLength.ShouldBePositive("Output array should have the correct length");

        var resultOutput = new double[model.Outputs.Length][];
        resultOutput.Length.ShouldBe(function.Outputs.Length, "Number of outputs must match the definition");
        for (var i = 0; i < resultOutput.Length; i++)
        {
            resultOutput[i] = new double[outputLength];
        }

        if (model.Unstable.HasValue)
        {
            function.SetUnstablePeriod(model.Unstable.Value);
        }

        var returnCode = function.Run(model.Inputs, model.Options, resultOutput, Range.EndAt(model.Inputs[0].Length - 1), out var outRange);
        returnCode.ShouldBe(Core.RetCode.Success, "Function should complete with success status code RetCode.Success(0)");

        for (var i = 0; i < resultOutput.Length; i++)
        {
            resultOutput[i].Length.ShouldBe(model.Outputs[i].Length,
                $"Expected and calculated length of the output values should be equal for output {i + 1}");
            resultOutput[i].ShouldBe(model.Outputs[i], equalityTolerance,
                $"Calculated values should be within expected for output {i + 1}");
        }

        outRange.ShouldBe(model.Range);

        if (model.Unstable.HasValue)
        {
            Core.UnstablePeriodSettings.Set(Core.UnstableFunc.All, 0);
        }
    }

    [SkippableTheory]
    [JsonFileData("DataSets/untest.json", typeof(float), "_")]
    [JsonFileData("DataSets/atoz.json", typeof(float), "_")]
    [JsonFileData("DataSets/extra.json", typeof(float), "_")]
    [JsonFileData("DataSets/spy.json", typeof(float), "_")]
    [JsonFileData("DataSets/unst.json", typeof(float), "_")]
    [JsonFileData("DataSets/cdl.json", typeof(float), "_")]
#pragma warning disable xUnit1026
    public void ShouldCalculateSuccessfullyForFloatInputAndSeparateOutput(TestDataModel<float> model, string fileName)
#pragma warning restore xUnit1026
    {
        Skip.If(model.Skip, "Test marked as skipped in the dataset.");
        Skip.If(fileName == "untest.json" && model.Name is "Ad" or "AdOsc",
            "The precision of floating-point arithmetic is insufficient for calculating accurate results.");

        const float equalityTolerance = 0.1f;

        Abstract.All.FunctionsDefinition.ShouldContainKey(model.Name, $"Cannot find definition for '{model.Name}");
        var function = Abstract.All.FunctionsDefinition[model.Name];

        model.Options.Length.ShouldBe(function.Options.Length, "Number of options must match the definition");
        var inputOffset = function.Lookback(Array.ConvertAll(model.Options, d => (int) d));
        model.Inputs.Length.ShouldBe(function.Inputs.Length, "Number of inputs must match the definition");
        var outputLength = model.Inputs[0].Length - inputOffset;
        outputLength.ShouldBePositive("Output array should have the correct length");

        var resultOutput = new float[model.Outputs.Length][];
        resultOutput.Length.ShouldBe(function.Outputs.Length, "Number of outputs must match the definition");
        for (var i = 0; i < resultOutput.Length; i++)
        {
            resultOutput[i] = new float[outputLength];
        }

        if (model.Unstable.HasValue)
        {
            function.SetUnstablePeriod(model.Unstable.Value);
        }

        var returnCode = function.Run(model.Inputs, model.Options, resultOutput, Range.EndAt(model.Inputs[0].Length - 1), out var outRange);
        returnCode.ShouldBe(Core.RetCode.Success, "Function should complete with success status code RetCode.Success(0)");

        for (var i = 0; i < resultOutput.Length; i++)
        {
            resultOutput[i].Length.ShouldBe(model.Outputs[i].Length,
                $"Expected and calculated length of the output values should be equal for output {i + 1}");
            resultOutput[i].ShouldBe(model.Outputs[i], equalityTolerance,
                $"Calculated values should be within expected for output {i + 1}");
        }

        outRange.ShouldBe(model.Range);

        if (model.Unstable.HasValue)
        {
            Core.UnstablePeriodSettings.Set(Core.UnstableFunc.All, 0);
        }
    }

    [SkippableTheory]
    [JsonFileData("DataSets/untest.json", typeof(double), "_")]
    [JsonFileData("DataSets/atoz.json", typeof(double), "_")]
    [JsonFileData("DataSets/extra.json", typeof(double), "_")]
    [JsonFileData("DataSets/spy.json", typeof(double), "_")]
    [JsonFileData("DataSets/unst.json", typeof(double), "_")]
    [JsonFileData("DataSets/same.json", typeof(double), "_")]
    [JsonFileData("DataSets/same.json", typeof(double), "__")]
    [JsonFileData("DataSets/same.json", typeof(double), "___")]
    [JsonFileData("DataSets/cdl.json", typeof(double), "_")]
#pragma warning disable xUnit1026
    public void ShouldCalculateSuccessfullyForDoubleInputSameOutput(TestDataModel<double> model, string fileName)
#pragma warning restore xUnit1026
    {
        Skip.If(model.Skip, "Test marked as skipped in the dataset.");

        const double equalityTolerance = 0.001d;

        Abstract.All.FunctionsDefinition.ShouldContainKey(model.Name, $"Cannot find definition for '{model.Name}");
        var function = Abstract.All.FunctionsDefinition[model.Name];

        model.Options.Length.ShouldBe(function.Options.Length, "Number of options must match the definition");
        var inputOffset = function.Lookback(Array.ConvertAll(model.Options, d => (int) d));
        model.Inputs.Count(input => input.Any(i => i != 0)).ShouldBe(function.Inputs.Length, "Number of inputs must match the definition");
        var outputLength = model.Inputs[0].Length - inputOffset;
        outputLength.ShouldBePositive("Output array should have the correct length");

        var resultOutput = new double[model.Outputs.Length][];
        resultOutput.Length.ShouldBe(function.Outputs.Length, "Number of outputs must match the definition");
        for (var i = 0; i < resultOutput.Length; i++)
        {
            resultOutput[i] = new double[outputLength];
        }

        if (model.Unstable.HasValue)
        {
            function.SetUnstablePeriod(model.Unstable.Value);
        }

        if (model.Inputs.Length < function.Outputs.Length)
        {
            model.Inputs = ResizeArray(model.Inputs, function.Outputs.Length);
        }

        double[][] inputs;
        double[][] outputs;
        if (model.Inputs.Any(input => input.All(i => i == 0)))
        {
            var inputArrayIndex = Array.FindIndex(model.Inputs, input => input.Any(i => i != 0));
            inputs = ReorderArray(model.Inputs, inputArrayIndex);
            outputs = model.Inputs;
        }
        else
        {
            inputs = outputs = model.Inputs;
        }

        var returnCode = function.Run(inputs, model.Options, outputs, Range.EndAt(model.Inputs[0].Length - 1), out var outRange);
        returnCode.ShouldBe(Core.RetCode.Success, "Function should complete with success status code RetCode.Success(0)");

        for (var i = 0; i < model.Outputs.Length; i++)
        {
            if (!model.Unstable.HasValue)
            {
                outputs[i][..model.Outputs[i].Length].ShouldBe(model.Outputs[i], equalityTolerance,
                    $"Calculated values should be within expected for output {i + 1}");
            }
            else
            {
                var valuableOutputLength = model.Outputs[i].Length - model.Unstable.Value;
                outputs[i][..valuableOutputLength].ShouldBe(model.Outputs[i][..valuableOutputLength], equalityTolerance,
                    $"Calculated values should be within expected for output {i + 1}");
            }
        }

        outRange.ShouldBe(model.Range);

        if (model.Unstable.HasValue)
        {
            Core.UnstablePeriodSettings.Set(Core.UnstableFunc.All, 0);
        }
    }

    [SkippableTheory]
    [JsonFileData("DataSets/untest.json", typeof(float), "_")]
    [JsonFileData("DataSets/atoz.json", typeof(float), "_")]
    [JsonFileData("DataSets/extra.json", typeof(float), "_")]
    [JsonFileData("DataSets/spy.json", typeof(float), "_")]
    [JsonFileData("DataSets/unst.json", typeof(float), "_")]
    [JsonFileData("DataSets/same.json", typeof(float), "_")]
    [JsonFileData("DataSets/same.json", typeof(float), "__")]
    [JsonFileData("DataSets/same.json", typeof(float), "___")]
    [JsonFileData("DataSets/cdl.json", typeof(float), "_")]
#pragma warning disable xUnit1026
    public void ShouldCalculateSuccessfullyForFloatInputSameOutput(TestDataModel<float> model, string fileName)
#pragma warning restore xUnit1026
    {
        Skip.If(model.Skip, "Test marked as skipped in the dataset.");
        Skip.If(fileName == "untest.json" && model.Name is "Ad" or "AdOsc",
            "The precision of floating-point arithmetic is insufficient for calculating accurate results.");

        const float equalityTolerance = 0.1f;

        Abstract.All.FunctionsDefinition.ShouldContainKey(model.Name, $"Cannot find definition for '{model.Name}");
        var function = Abstract.All.FunctionsDefinition[model.Name];

        model.Options.Length.ShouldBe(function.Options.Length, "Number of options must match the definition");
        var inputOffset = function.Lookback(Array.ConvertAll(model.Options, d => (int) d));
        model.Inputs.Count(input => input.Any(i => i != 0)).ShouldBe(function.Inputs.Length, "Number of inputs must match the definition");
        var outputLength = model.Inputs[0].Length - inputOffset;
        outputLength.ShouldBePositive("Output array should have the correct length");

        var resultOutput = new float[model.Outputs.Length][];
        resultOutput.Length.ShouldBe(function.Outputs.Length, "Number of outputs must match the definition");
        for (var i = 0; i < resultOutput.Length; i++)
        {
            resultOutput[i] = new float[outputLength];
        }

        if (model.Unstable.HasValue)
        {
            function.SetUnstablePeriod(model.Unstable.Value);
        }

        if (model.Inputs.Length < function.Outputs.Length)
        {
            model.Inputs = ResizeArray(model.Inputs, function.Outputs.Length);
        }

        float[][] inputs;
        float[][] outputs;
        if (model.Inputs.Any(input => input.All(i => i == 0)))
        {
            var inputArrayIndex = Array.FindIndex(model.Inputs, input => input.Any(i => i != 0));
            inputs = ReorderArray(model.Inputs, inputArrayIndex);
            outputs = model.Inputs;
        }
        else
        {
            inputs = outputs = model.Inputs;
        }

        var returnCode = function.Run(inputs, model.Options, outputs, Range.EndAt(model.Inputs[0].Length - 1), out var outRange);
        returnCode.ShouldBe(Core.RetCode.Success, "Function should complete with success status code RetCode.Success(0)");

        for (var i = 0; i < model.Outputs.Length; i++)
        {
            if (!model.Unstable.HasValue)
            {
                outputs[i][..model.Outputs[i].Length].ShouldBe(model.Outputs[i], equalityTolerance,
                    $"Calculated values should be within expected for output {i + 1}");
            }
            else
            {
                var valuableOutputLength = model.Outputs[i].Length - model.Unstable.Value;
                outputs[i][..valuableOutputLength].ShouldBe(model.Outputs[i][..valuableOutputLength], equalityTolerance,
                    $"Calculated values should be within expected for output {i + 1}");
            }
        }

        outRange.ShouldBe(model.Range);

        if (model.Unstable.HasValue)
        {
            Core.UnstablePeriodSettings.Set(Core.UnstableFunc.All, 0);
        }
    }

    private static T[][] ReorderArray<T>(T[][] inputArray, int indexToMoveToTop) where T : IFloatingPointIeee754<T>
    {
        if (indexToMoveToTop <= 0 || indexToMoveToTop >= inputArray.Length)
        {
            return inputArray;
        }

        var newArray = new T[inputArray.Length][];
        newArray[0] = inputArray[indexToMoveToTop];
        inputArray.AsSpan(0, indexToMoveToTop).CopyTo(newArray.AsSpan(1));
        inputArray.AsSpan(indexToMoveToTop + 1, inputArray.Length - indexToMoveToTop - 1).CopyTo(newArray.AsSpan(indexToMoveToTop + 1));

        return newArray;
    }

    private static T[][] ResizeArray<T>(T[][] inputArray, int newSize) where T : IFloatingPointIeee754<T>
    {
        if (newSize <= 0)
        {
            return inputArray;
        }

        var newArray = new T[newSize][];
        for (var i = 0; i < newSize; i++)
        {
            newArray[i] = i < inputArray.Length ? inputArray[i] : new T[inputArray[0].Length];
        }

        return newArray;
    }
}
