/*
 * Technical Analysis Library for .NET
 * Copyright (c) 2020-2025 Anatolii Siryi
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

public partial class Abstract
{
    /// <summary>
    /// Represents a function that can be used to calculate technical indicators.
    /// </summary>
    public sealed class IndicatorFunction
    {
        private const string LookbackSuffix = "Lookback";
        private const string InPrefix = "in";
        private const string OutPrefix = "out";
        private const string OptInPrefix = "optIn";
        private const string RealParam = "Real";
        private const string InRangeParam = "inRange";
        private const string OutRangeParam = "outRange";

        internal IndicatorFunction(
            string name,
            string description,
            string group,
            string[] inputs,
            (string displayName, string hint)[] options,
            (string displayName, Core.OutputDisplayHints displayHint)[] outputs) =>
            (Name, Description, Group, Inputs, Options, Outputs) = (name, description, group, inputs, options, outputs);

        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the function.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the group to which the function belongs.
        /// </summary>
        public string Group { get; }

        /// <summary>
        /// Gets the input parameters of the function.
        /// </summary>
        public string[] Inputs { get; }

        /// <summary>
        /// Gets the options of the function.
        /// </summary>
        public (string displayName, string hint)[] Options { get; }

        /// <summary>
        /// Gets the output parameters of the function.
        /// </summary>
        public (string displayName, Core.OutputDisplayHints displayHint)[] Outputs { get; }

#pragma warning disable S2368
        /// <summary>
        /// Runs the function with the specified inputs, options, and outputs.
        /// </summary>
        /// <param name="inputs">The input data arrays to use for the calculation.</param>
        /// <param name="options">The options to use for the calculation.</param>
        /// <param name="outputs">The output data arrays to store the calculation results.</param>
        /// <param name="inRange">The range of indices that determines the portion of data to be calculated within the input arrays.</param>
        /// <param name="outRange">The range of indices representing the valid data within the output arrays.</param>
        /// <typeparam name="T">
        /// The numeric data type, typically <see langword="float"/> or <see langword="double"/>,
        /// implementing the <see cref="IFloatingPointIeee754{T}"/> interface.
        /// </typeparam>
        /// <returns>
        /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
        /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
        /// </returns>
        /// <exception cref="MissingMethodException">Thrown when the function method is not found.</exception>
        public Core.RetCode Run<T>(
            T[][] inputs,
            T[] options,
            T[][] outputs,
            Range inRange,
            out Range outRange) where T : IFloatingPointIeee754<T>
#pragma warning restore S2368
        {
            var functionMethod = ReflectMethods(publicOnly: false)
                                     .FirstOrDefault(mi =>
                                         !mi.Name.EndsWith(LookbackSuffix, StringComparison.Ordinal) && FunctionMethodSelector(mi)) ??
                                 throw new MissingMethodException(null, $"{Name}<{typeof(T).Name}>");

            var paramsArray = PrepareFunctionMethodParams(inputs, options, outputs, inRange, functionMethod, out var isIntegerOutput);

            var retCode = (Core.RetCode) functionMethod.MakeGenericMethod(typeof(T)).Invoke(null, paramsArray)!;

            outRange = (Range) paramsArray[Inputs.Length + 1 + Outputs.Length];

            if (retCode != Core.RetCode.Success || !isIntegerOutput)
            {
                return retCode;
            }

            for (var i = 0; i < Outputs.Length; i++)
            {
                var outputArray = paramsArray[inputs.Length + 1 + i];
                for (var j = 0; j < outputs[i].Length; j++)
                {
                    outputs[i][j] = (T) Convert.ChangeType(((int[]) outputArray)[j], typeof(T));
                }
            }

            return retCode;
        }

        /// <summary>
        /// Calculates the lookback of the function with the specified options.
        /// </summary>
        /// <param name="options">The options to use for the lookback calculation.</param>
        /// <returns>
        /// A <see cref="Core.RetCode"/> value indicating the success or failure of the calculation.
        /// Returns <see cref="Core.RetCode.Success"/> on successful calculation, or an appropriate error code otherwise.
        /// </returns>
        /// <exception cref="MissingMethodException">Thrown when the lookback method is not found.</exception>
        public int Lookback(params int[] options)
        {
            var lookbackMethod = ReflectMethods(publicOnly: true)
                                     .FirstOrDefault(mi =>
                                         mi.Name.EndsWith(LookbackSuffix, StringComparison.Ordinal) && LookbackMethodSelector(mi))
                                 ?? throw new MissingMethodException(null, LookbackMethodName);

            var optInParameters = lookbackMethod.GetParameters().Where(pi => pi.Name!.StartsWith(OptInPrefix, StringComparison.Ordinal))
                .ToList();
            var paramsArray = new object[optInParameters.Count];
            Array.Fill(paramsArray, Type.Missing);

            var paramsArrayIndex = 0;
            var defOptInParameters = Options.Select(o => NormalizeOptionalParameter(o.displayName)).ToList();
            for (var i = 0; i < defOptInParameters.Count; i++)
            {
                var optInParameter = optInParameters.SingleOrDefault(p => p.Name!.Equals(defOptInParameters[i], StringComparison.Ordinal));
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

        /// <summary>
        /// Sets the unstable period of the function.
        /// </summary>
        /// <param name="period">The unstable period to set.</param>
        /// <exception cref="NotSupportedException">Thrown when the function does not support unstable period settings.</exception>
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

        /// <inheritdoc />
        public override string ToString() => Name;

        private static IEnumerable<MethodInfo> ReflectMethods(bool publicOnly) =>
#pragma warning disable S3011
            typeof(Functions).GetMethods(BindingFlags.Static | (publicOnly ? BindingFlags.Public : BindingFlags.NonPublic))
                .Concat(typeof(Candles).GetMethods(BindingFlags.Static | (publicOnly ? BindingFlags.Public : BindingFlags.NonPublic)));
#pragma warning restore S3011

        private object[] PrepareFunctionMethodParams<T>(
            T[][] inputs,
            T[] options,
            T[][] outputs,
            Range inRange,
            MethodInfo method,
            out bool isIntegerOutput) where T : IFloatingPointIeee754<T>
        {
            var paramsArray = InitializeParamsArray(inputs, inRange);

            isIntegerOutput = method.GetParameters().Count(pi =>
                pi.Name!.StartsWith(OutPrefix, StringComparison.Ordinal) && pi.ParameterType == typeof(int[])) == 1;
            FillOutputs(paramsArray, outputs, isIntegerOutput);

            FillOptionalParameters(method, paramsArray, options);

            return paramsArray;
        }

        private object[] InitializeParamsArray<T>(T[][] inputs, Range inRange)
        {
            var paramsArray = new object[Inputs.Length + 1 + Outputs.Length + 1 + Options.Length];
            for (var i = 0; i < Inputs.Length; i++)
            {
                paramsArray[i] = inputs[i];
            }

            paramsArray[Inputs.Length] = inRange;

            return paramsArray;
        }

        private void FillOutputs<T>(object[] paramsArray, T[][] outputs, bool isIntegerOutput)
        {
            for (var i = 0; i < Outputs.Length; i++)
            {
                paramsArray[Inputs.Length + 1 + i] = isIntegerOutput ? new int[outputs[i].Length] : outputs[i];
            }

            Array.Fill(paramsArray, Type.Missing, Inputs.Length + 1 + Outputs.Length + 1, Options.Length);
        }

        private void FillOptionalParameters<T>(MethodInfo method, object[] paramsArray, T[] options)
        {
            var optInParameters = method.GetParameters().Where(pi => pi.Name!.StartsWith(OptInPrefix, StringComparison.Ordinal)).ToList();
            var defOptInParameters = Options.Select(o => NormalizeOptionalParameter(o.displayName)).ToList();

            for (var i = 0; i < defOptInParameters.Count; i++)
            {
                var optInParameter = optInParameters.SingleOrDefault(p => p.Name!.Equals(defOptInParameters[i], StringComparison.Ordinal));
                if (optInParameter is null || i >= options.Length)
                {
                    continue;
                }

                var paramsArrayIndex = Inputs.Length + 1 + Outputs.Length + i + 1;
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
                    paramsArray[paramsArrayIndex] = options[i]!;
                }
            }
        }

        private bool LookbackMethodSelector(MethodBase methodInfo)
        {
            var optInParameters = methodInfo.GetParameters().Select(pi => pi.Name);
            var defOptInParameters = Options.Select(o => NormalizeOptionalParameter(o.displayName));

            return methodInfo.Name.Equals(LookbackMethodName, StringComparison.Ordinal) &&
                   optInParameters.All(p => defOptInParameters.Contains(p, StringComparer.Ordinal));
        }

        private bool FunctionMethodSelector(MethodBase methodInfo)
        {
            var parameters = methodInfo.GetParameters()
                .Where(pi => !pi.Name!.Equals(InRangeParam, StringComparison.Ordinal) &&
                             !pi.Name.Equals(OutRangeParam, StringComparison.Ordinal))
                .ToList();

            var inParameters = parameters.Where(pi => pi.Name!.StartsWith(InPrefix, StringComparison.Ordinal)).Select(pi => pi.Name);

            var outParameters = parameters.Where(pi => pi.Name!.StartsWith(OutPrefix, StringComparison.Ordinal)).Select(pi => pi.Name);

            var optInParameters = parameters.Where(pi => pi.Name!.StartsWith(OptInPrefix, StringComparison.Ordinal)).Select(pi => pi.Name);

            var defInParameters = Inputs.Length > 1 && Array.TrueForAll(Inputs, p => p == RealParam)
                ? Inputs.Select((p, i) => InPrefix + p + i)
                : Inputs.Select(p => InPrefix + p);

            var defOutParameters = Outputs.Select(o => NormalizeOutputParameter(o.displayName));

            var defOptInParameters = Options.Select(o => NormalizeOptionalParameter(o.displayName));

            return methodInfo.Name.Equals(Name, StringComparison.Ordinal) &&
                   inParameters.SequenceEqual(defInParameters) &&
                   outParameters.SequenceEqual(defOutParameters) &&
                   optInParameters.SequenceEqual(defOptInParameters);
        }

        private string LookbackMethodName => Name + LookbackSuffix;

        private static string NormalizeOutputParameter(string parameter) => OutPrefix + parameter.Replace(" ", String.Empty);

        private static string NormalizeOptionalParameter(string parameter) => OptInPrefix + parameter.Replace(" ", String.Empty);
    }
}
