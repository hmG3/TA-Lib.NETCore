/*
 * Technical Analysis Library for .NET
 * Copyright (c) 2020-2024 Anatolii Siryi
 *
 * This file is part of Technical Analysis Library for .NET.
 *
 * Technical Analysis Library for .NET is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Technical Analysis Library for .NET is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with Technical Analysis Library for .NET. If not, see <https://www.gnu.org/licenses/>.
 */

using System.Linq;
using System.Reflection;

namespace TALib;

public sealed class IndicatorFunction
{
    private const string LookbackSuffix = "Lookback";
    private const string InPrefix = "in";
    private const string OutPrefix = "out";
    private const string OptInPrefix = "optIn";
    private const string GeneralRealParam = "Real";
    private const string BegIdxParam = "BegIdx";
    private const string NbElementParam = "NbElement";

    internal IndicatorFunction(
        string name,
        string description,
        string group,
        string[] inputs,
        (string displayName, string hint)[] options,
        (string displayName, Core.OutputFlags flags)[] outputs) =>
        (Name, Description, Group, Inputs, Options, Outputs) = (name, description, group, inputs, options, outputs);

    public string Name { get; }

    public string Description { get; }

    public string Group { get; }

    public string[] Inputs { get; }

    public (string displayName, string hint)[] Options { get; }

    public (string displayName, Core.OutputFlags flags)[] Outputs { get; }

    public Core.RetCode Run<T>(T[][] inputs, T[] options, T[][] outputs) where T : IFloatingPointIeee754<T>
    {
        var functionMethod = ReflectMethods<T>(publicOnly: false)
                                 .FirstOrDefault(mi => !mi.Name.EndsWith(LookbackSuffix) && FunctionMethodSelector(mi)) ??
                             throw new MissingMethodException(null, $"{Name}<{typeof(T).Name}>");

        var paramsArray = PrepareFunctionMethodParams(inputs, options, outputs, functionMethod, out var isIntegerOutput);

        var retCode = (Core.RetCode) functionMethod.MakeGenericMethod(typeof(T)).Invoke(null, paramsArray)!;
        if (isIntegerOutput && retCode == Core.RetCode.Success)
        {
            for (var i = 0; i < outputs.Length; i++)
            {
                var integerOutputs = (int[]) paramsArray[inputs.Length + 2 + i];
                for (var j = 0; j < integerOutputs.Length; j++)
                {
                    outputs[i][j] = (T) Convert.ChangeType(integerOutputs[j], typeof(T));
                }
            }
        }

        return retCode;
    }

    public int Lookback<T>(params int[] options) where T : IFloatingPointIeee754<T>
    {
        var lookbackMethod = ReflectMethods<T>(publicOnly: true)
                                 .FirstOrDefault(mi => mi.Name.EndsWith(LookbackSuffix) && LookbackMethodSelector(mi))
                             ?? throw new MissingMethodException(null, LookbackMethodName);

        var optInParameters = lookbackMethod.GetParameters().Where(pi => pi.Name!.StartsWith(OptInPrefix)).ToList();
        var paramsArray = new object[optInParameters.Count];
        Array.Fill(paramsArray, Type.Missing);

        var defOptInParameters = Options.Select(o => NormalizeOptionalParameter(o.displayName)).ToList();
        for (int i = 0, paramsArrayIndex = 0; i < defOptInParameters.Count; i++)
        {
            var optInParameter = optInParameters.SingleOrDefault(p => p.Name == defOptInParameters[i]);
            if (optInParameter is null || i >= options.Length)
            {
                continue;
            }

            if (optInParameter.ParameterType.IsEnum && optInParameter.ParameterType.IsEnumDefined(options[i]))
            {
                paramsArray[paramsArrayIndex++] = Enum.ToObject(optInParameter.ParameterType, options[i]);
            }
            else
            {
                paramsArray[paramsArrayIndex++] = options[i];
            }
        }

        return (int) lookbackMethod.Invoke(null, paramsArray)!;
    }

    public void SetUnstablePeriod(int period)
    {
        if (Enum.TryParse(Name, out Core.UnstableFunc func))
        {
            Core.UnstablePeriodSettings.Set(func, period);
        }
        else
        {
            throw new NotSupportedException($"Function {Name} does not support unstable period settings.");
        }
    }

    public override string ToString() => Name;

    private static IEnumerable<MethodInfo> ReflectMethods<T>(bool publicOnly) where T : IFloatingPointIeee754<T> =>
        typeof(Functions).GetMethods(BindingFlags.Static | (publicOnly ? BindingFlags.Public : BindingFlags.NonPublic))
            .Concat(typeof(Candles).GetMethods(BindingFlags.Static | (publicOnly ? BindingFlags.Public : BindingFlags.NonPublic)));

    private object[] PrepareFunctionMethodParams<T>(
        T[][] inputs,
        T[] options,
        T[][] outputs,
        MethodInfo method,
        out bool isIntegerOutput) where T : IFloatingPointIeee754<T>
    {
        var optInParameters = method.GetParameters().Where(pi => pi.Name!.StartsWith(OptInPrefix)).ToList();

        var paramsArray = new object[inputs.Length + 2 + outputs.Length + optInParameters.Count];
        for (var i = 0; i < inputs.Length; i++)
        {
            paramsArray[i] = inputs[i];
        }

        paramsArray[inputs.Length] = 0;
        paramsArray[inputs.Length + 1] = inputs[0].Length - 1;

        isIntegerOutput = method.GetParameters().Count(pi => pi.Name!.StartsWith(OutPrefix) && pi.ParameterType == typeof(int[])) == 1;
        for (var i = 0; i < outputs.Length; i++)
        {
            paramsArray[inputs.Length + 2 + i] = isIntegerOutput ? new int[outputs[i].Length] : outputs[i];
        }

        Array.Fill(paramsArray, Type.Missing, inputs.Length + 2 + outputs.Length, optInParameters.Count);

        var defOptInParameters = Options.Select(o => NormalizeOptionalParameter(o.displayName)).ToList();
        for (var i = 0; i < defOptInParameters.Count; i++)
        {
            var optInParameter = optInParameters.SingleOrDefault(p => p.Name == defOptInParameters[i]);
            if (optInParameter is null || i >= options.Length)
            {
                continue;
            }

            var paramsArrayIndex = inputs.Length + 2 + outputs.Length + i;
            if (optInParameter.ParameterType == typeof(int) || optInParameter.ParameterType.IsEnum)
            {
                var intOption = Convert.ToInt32(options[i]);
                if (optInParameter.ParameterType.IsEnum && optInParameter.ParameterType.IsEnumDefined(intOption))
                {
                    paramsArray[paramsArrayIndex] = Enum.ToObject(optInParameter.ParameterType, intOption);
                }
                else
                {
                    paramsArray[paramsArrayIndex] = intOption;
                }
            }
            else
            {
                paramsArray[paramsArrayIndex] = options[i];
            }
        }

        return paramsArray;
    }

    private bool LookbackMethodSelector(MethodBase methodInfo)
    {
        var optInParameters = methodInfo.GetParameters().Select(pi => pi.Name);
        var defOptInParameters = Options.Select(o => NormalizeOptionalParameter(o.displayName));

        return methodInfo.Name == LookbackMethodName && optInParameters.All(defOptInParameters.Contains);
    }

    private bool FunctionMethodSelector(MethodBase methodInfo)
    {
        var parameters = methodInfo.GetParameters()
            .Where(pi => pi.Name != OutPrefix + BegIdxParam && pi.Name != OutPrefix + NbElementParam)
            .ToList();

        var inParameters = parameters.Where(pi => pi.Name!.StartsWith(InPrefix)).Select(pi => pi.Name);

        var outParameters = parameters.Where(pi => pi.Name!.StartsWith(OutPrefix)).Select(pi => pi.Name);

        var optInParameters = parameters.Where(pi => pi.Name!.StartsWith(OptInPrefix)).Select(pi => pi.Name);

        var defInParameters = Inputs.Length > 1 && Inputs.All(p => p == GeneralRealParam)
            ? Inputs.Select((p, i) => InPrefix + p + i)
            : Inputs.Select(p => InPrefix + p);

        var defOutParameters = Outputs.Select(o => NormalizeOutputParameter(o.displayName));

        var defOptInParameters = Options.Select(o => NormalizeOptionalParameter(o.displayName));

        return methodInfo.Name == Name &&
               inParameters.SequenceEqual(defInParameters) &&
               outParameters.SequenceEqual(defOutParameters) &&
               optInParameters.SequenceEqual(defOptInParameters);
    }

    private string LookbackMethodName => Name + LookbackSuffix;

    private static string NormalizeOutputParameter(string parameter) => OutPrefix + parameter.Replace(" ", String.Empty);

    private static string NormalizeOptionalParameter(string parameter) => OptInPrefix + parameter.Replace(" ", String.Empty);
}
