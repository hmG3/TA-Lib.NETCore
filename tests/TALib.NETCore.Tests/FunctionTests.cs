using System;
using System.Buffers;
using System.Runtime.InteropServices;
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
    public void Should_Calculate_CorrectOutput_With_SuccessStatus_For_DoubleInput(TestDataModel<double> model, string fileName)
#pragma warning restore xUnit1026
    {
        Skip.If(model.Skip, "Test marked as skipped in the dataset.");

        const double equalityTolerance = 0.001d;

        Abstract.FunctionsDefinition.ShouldContainKey(model.Name, $"Cannot find definition for '{model.Name}");
        var function = Abstract.FunctionsDefinition[model.Name];

        model.Options.Length.ShouldBe(function.Options.Length, "Number of options must match the definition");
        var inputOffset = function.Lookback<double>(Array.ConvertAll(model.Options, d => (int) d));
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

        var returnCode = function.Run<double>(model.Inputs, model.Options, resultOutput);
        returnCode.ShouldBe(Core.RetCode.Success, "Function should complete with success status code RetCode.Success(0)");

        for (var i = 0; i < resultOutput.Length; i++)
        {
            resultOutput[i].Length.ShouldBe(model.Outputs[i].Length,
                $"Expected and calculated length of the output values should be equal for output {i + 1}");
            resultOutput[i].ShouldBe(model.Outputs[i], equalityTolerance,
                $"Calculated values should be within expected for output {i + 1}");
        }

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
    public void Should_Calculate_CorrectOutput_With_SuccessStatus_For_FloatInput(TestDataModel<float> model, string fileName)
#pragma warning restore xUnit1026
    {
        Skip.If(model.Skip, "Test marked as skipped in the dataset.");
        Skip.If(fileName == "untest.json" && model.Name is "Ad" or "AdOsc",
            "The precision of floating-point arithmetic is insufficient for calculating accurate results.");

        const float equalityTolerance = 0.1f;

        Abstract.FunctionsDefinition.ShouldContainKey(model.Name, $"Cannot find definition for '{model.Name}");
        var function = Abstract.FunctionsDefinition[model.Name];

        model.Options.Length.ShouldBe(function.Options.Length, "Number of options must match the definition");
        var inputOffset = function.Lookback<float>(Array.ConvertAll(model.Options, d => (int) d));
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

        var returnCode = function.Run<float>(model.Inputs, model.Options, resultOutput);
        returnCode.ShouldBe(Core.RetCode.Success, "Function should complete with success status code RetCode.Success(0)");

        for (var i = 0; i < resultOutput.Length; i++)
        {
            resultOutput[i].Length.ShouldBe(model.Outputs[i].Length,
                $"Expected and calculated length of the output values should be equal for output {i + 1}");
            resultOutput[i].ShouldBe(model.Outputs[i], equalityTolerance,
                $"Calculated values should be within expected for output {i + 1}");
        }

        if (model.Unstable.HasValue)
        {
            Core.UnstablePeriodSettings.Set(Core.UnstableFunc.All, 0);
        }
    }
}
