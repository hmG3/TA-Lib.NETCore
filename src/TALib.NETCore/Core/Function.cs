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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TALib;

public sealed class Function
{
    private const string LookbackSuffix = "Lookback";
    private const string InPrefix = "in";
    private const string OutPrefix = "out";
    private const string OptInPrefix = "optIn";
    private const string GeneralRealParam = "Real";
    private const string BegIdxParam = "BegIdx";
    private const string NbElementParam = "NbElement";

    internal Function(
        string name,
        string description,
        string group,
        string inputs,
        string options,
        string outputs)
    {
        Name = name;
        Description = description;
        Group = group;
        Inputs = inputs.Split('|');
        Options = !String.IsNullOrEmpty(options) ? options.Split('|') : Array.Empty<string>();
        Outputs = outputs.Split('|');
    }

    public string Name { get; }

    public string Description { get; }

    public string Group { get; }

    public string[] Inputs { get; }

    public string[] Options { get; }

    public string[] Outputs { get; }

    public Core.RetCode Run<T>(T[][] inputs, T[] options, T[][] outputs) where T : IFloatingPointIeee754<T>
    {
        var functionMethod = ReflectMethods<T>()
                                 .Where(mi => !mi.Name.EndsWith(LookbackSuffix))
                                 .FirstOrDefault(FunctionMethodSelector) ??
                             throw new MissingMethodException(null, $"{Name}<{typeof(T).Name}>");

        var paramsArray = PrepareFunctionMethodParamsInternal(inputs, options, outputs, functionMethod, out var isIntegerOutput);

        var retCode = (Core.RetCode) functionMethod.Invoke(null, paramsArray)!;
        if (isIntegerOutput && retCode == Core.RetCode.Success)
        {
            var integerOutputs = Array.ConvertAll((int[]) paramsArray[inputs.Length + 2], i => (T) Convert.ChangeType(i, typeof(T)));
            Array.Copy(integerOutputs, 0, outputs[0], 0, ((int[]) paramsArray[inputs.Length + 2]).Length);
        }

        return retCode;
    }

    public int Lookback<T>(params int[] options) where T : IFloatingPointIeee754<T>
    {
        var lookbackMethod = ReflectMethods<T>()
                                 .Where(mi => mi.Name.EndsWith(LookbackSuffix))
                                 .FirstOrDefault(LookbackMethodSelector) ??
                             throw new MissingMethodException(null, LookbackMethodName);

        var optInParameters = lookbackMethod.GetParameters().Where(pi => pi.Name!.StartsWith(OptInPrefix)).ToList();
        var paramsArray = new object[optInParameters.Count];
        Array.Fill(paramsArray, Type.Missing);

        var defOptInParameters = Options.Select(NormalizeOptionalParameter).ToList();
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
        if (Enum.TryParse<Core.UnstableFunc>(Name, out var func))
        {
            Core.UnstablePeriodSettings.Set(func, period);
        }
        else
        {
            throw new NotSupportedException($"Function {Name} does not support unstable period settings.");
        }
    }

    public override string ToString() => Name;

    private static IEnumerable<MethodInfo> ReflectMethods<T>() where T : IFloatingPointIeee754<T> =>
        typeof(Functions<>).MakeGenericType(typeof(T)).GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Concat(typeof(Candles<>).MakeGenericType(typeof(T)).GetMethods(BindingFlags.Static | BindingFlags.Public));

    internal object[] PrepareFunctionMethodParamsInternal<T>(
        T[][] inputs,
        T[] options,
        T[][] outputs,
        MethodInfo method,
        out bool isIntegerOutput) where T : IFloatingPointIeee754<T>
    {
        var optInParameters = method.GetParameters().Where(pi => pi.Name!.StartsWith(OptInPrefix)).ToList();

        var paramsArray = new object[inputs.Length + 2 + outputs.Length + 2 + optInParameters.Count];
        inputs.CopyTo(paramsArray, 0);
        paramsArray[inputs.Length] = 0;
        paramsArray[inputs.Length + 1] = inputs[0].Length - 1;

        isIntegerOutput = method.GetParameters().Count(pi => pi.Name!.StartsWith(OutPrefix) && pi.ParameterType == typeof(int[])) == 1;
        if (isIntegerOutput)
        {
            var integerOutputs = outputs.Select(ta => ta.Select(t => (int) Convert.ChangeType(t, typeof(int))).ToArray()).ToArray();
            integerOutputs.CopyTo(paramsArray, inputs.Length + 2);
        }
        else
        {
            outputs.CopyTo(paramsArray, inputs.Length + 2);
        }

        Array.Fill(paramsArray, Type.Missing, inputs.Length + 2 + outputs.Length + 2, optInParameters.Count);

        var defOptInParameters = Options.Select(NormalizeOptionalParameter).ToList();
        for (var i = 0; i < defOptInParameters.Count; i++)
        {
            var optInParameter = optInParameters.SingleOrDefault(p => p.Name == defOptInParameters[i]);
            if (optInParameter is null || i >= options.Length)
            {
                continue;
            }

            var paramsArrayIndex = new Index(inputs.Length + 2 + outputs.Length + 2 + i);
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
        var defOptInParameters = Options.Select(NormalizeOptionalParameter);

        return methodInfo.Name == LookbackMethodName && optInParameters.All(p => defOptInParameters.Contains(p));
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

        var defOutParameters = Outputs.Select(NormalizeOutputParameter);

        var defOptInParameters = Options.Select(NormalizeOptionalParameter);

        return methodInfo.Name == Name &&
               inParameters.SequenceEqual(defInParameters) &&
               outParameters.SequenceEqual(defOutParameters) &&
               optInParameters.SequenceEqual(defOptInParameters);
    }

    private string LookbackMethodName => Name + LookbackSuffix;

    private static string NormalizeOutputParameter(string parameter) => OutPrefix + parameter.Replace(" ", String.Empty);

    private static string NormalizeOptionalParameter(string parameter) => OptInPrefix + parameter.Replace(" ", String.Empty);
}
