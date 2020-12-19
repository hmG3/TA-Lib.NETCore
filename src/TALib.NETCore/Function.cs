using System;
using System.Linq;
using System.Reflection;

namespace TALib
{
    public sealed class Function
    {
        private const string LookbackSuffix = "Lookback";
        private const string InPrefix = "in";
        private const string OutPrefix = "out";
        private const string OptInPrefix = "optIn";
        private const string GeneralRealParam = "Real";
        private const string BegIdxParam = "BegIdx";
        private const string NbElementParam = "NbElement";

        internal Function(string name, string description, string group, string inputs, string options, string outputs)
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

        public int Lookback(params int[] options)
        {
            var method = typeof(Core)
                             .GetMethods(BindingFlags.Static | BindingFlags.Public)
                             .Where(mi => mi.Name.EndsWith(LookbackSuffix))
                             .SingleOrDefault(MethodFinder) ??
                         throw new MissingMethodException(typeof(Core).FullName, LookbackMethodName);

            var optInParameters = method.GetParameters().Where(pi => pi.Name.StartsWith(OptInPrefix)).ToList();
            var paramsArray = new object[optInParameters.Count];
            Array.Fill(paramsArray, Type.Missing);

            var defOptInParameters = Options.Select(NormalizeOptionalParameter).ToList();
            for (int i = 0, paramsArrayIndex = 0; i < defOptInParameters.Count; i++)
            {
                var optInParameter = optInParameters.SingleOrDefault(p => p.Name == defOptInParameters[i]);
                if (optInParameter != null)
                {
                    if (optInParameter.ParameterType.IsEnum && optInParameter.ParameterType.IsEnumDefined(options[i]))
                    {
                        paramsArray[paramsArrayIndex++] = Enum.ToObject(optInParameter.ParameterType, options[i]);
                    }
                    else
                    {
                        paramsArray[paramsArrayIndex++] = options[i];
                    }
                }
            }

            return (int) method.Invoke(null, paramsArray);
        }

        public Core.RetCode Run(double[][] inputs, double[] options, double[][] outputs) => RunInternal(inputs, options, outputs);

        public Core.RetCode Run(decimal[][] inputs, decimal[] options, decimal[][] outputs) => RunInternal(inputs, options, outputs);

        private Core.RetCode RunInternal<T>(T[][] inputs, T[] options, T[][] outputs)
        {
            var method = typeof(Core)
                                 .GetMethods(BindingFlags.Static | BindingFlags.Public)
                                 .Where(mi => !mi.Name.EndsWith(LookbackSuffix))
                                 .SingleOrDefault(mi => MethodFinder(mi, typeof(T[]))) ??
                             throw new MissingMethodException(typeof(Core).FullName, $"{Name}<{typeof(T).Name}>");

            var optInParameters = method.GetParameters().Where(pi => pi.Name.StartsWith(OptInPrefix)).ToList();

            var paramsArray = new object[inputs.Length + 2 + outputs.Length + 2 + optInParameters.Count];
            inputs.CopyTo(paramsArray, 0);
            paramsArray[inputs.Length] = 0;
            paramsArray[inputs.Length + 1] = inputs[0].Length - 1;

            var isIntegerOutput =
                method.GetParameters().Count(pi => pi.Name.StartsWith(OutPrefix) && pi.ParameterType == typeof(int[])) == 1;
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
            if (options.Length == optInParameters.Count)
            {
                var defOptInParameters = Options.Select(NormalizeOptionalParameter).ToList();
                for (var i = 0; i < defOptInParameters.Count; i++)
                {
                    var paramsArrayIndex = new Index(inputs.Length + 2 + outputs.Length + 2 + i);
                    var optInParameter = optInParameters.Single(p => p.Name == defOptInParameters[i]);
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

            var retCode = (Core.RetCode) method.Invoke(null, paramsArray);
            if (isIntegerOutput && retCode == Core.RetCode.Success)
            {
                var integerOutputs = Array.ConvertAll((int[]) paramsArray[inputs.Length + 2], i => (T) Convert.ChangeType(i, typeof(T)));
                Array.Copy(integerOutputs, 0, outputs[0], 0, ((int[]) paramsArray[inputs.Length + 2]).Length);
            }

            return retCode;
        }

        private bool MethodFinder(MethodBase methodInfo)
        {
            var optInParameters = methodInfo.GetParameters().Select(pi => pi.Name);
            var defOptInParameters = Options.Select(NormalizeOptionalParameter);

            return methodInfo.Name == LookbackMethodName && optInParameters.All(p => defOptInParameters.Contains(p));
        }

        private bool MethodFinder(MethodBase methodInfo, Type parameterType)
        {
            var parameters = methodInfo.GetParameters()
                .Where(pi => pi.Name != OutPrefix + BegIdxParam && pi.Name != OutPrefix + NbElementParam)
                .ToList();

            var inParameters = parameters.Where(pi => pi.Name.StartsWith(InPrefix) && pi.ParameterType == parameterType)
                .Select(pi => pi.Name);
            var outParameters = parameters
                .Where(pi => pi.Name.StartsWith(OutPrefix) && (pi.ParameterType == parameterType || pi.ParameterType == typeof(int[])))
                .Select(pi => pi.Name);
            var optInParameters = parameters.Where(pi => pi.Name.StartsWith(OptInPrefix)).Select(pi => pi.Name);

            var defInParameters = Inputs.Length > 1 && Inputs.All(p => p == GeneralRealParam)
                ? Inputs.Select((p, i) => InPrefix + p + i)
                : Inputs.Select(p => InPrefix + p);
            var defOutParameters = Outputs.Select(NormalizeOutputParameter);
            var defOptInParameters = Options.Select(NormalizeOptionalParameter);

            return methodInfo.Name == Name && inParameters.SequenceEqual(defInParameters) &&
                   outParameters.SequenceEqual(defOutParameters) && optInParameters.SequenceEqual(defOptInParameters);
        }

        private string LookbackMethodName => Name + LookbackSuffix;

        private static string NormalizeOutputParameter(string parameter) => OutPrefix + parameter.Replace(" ", String.Empty);

        private static string NormalizeOptionalParameter(string parameter) => OptInPrefix + parameter.Replace(" ", String.Empty);
    }
}
