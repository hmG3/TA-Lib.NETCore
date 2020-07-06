using System;
using System.Collections.Generic;

namespace TALib
{
    public static partial class Core
    {
        private const double TA_EPSILON = 0.00000000000001;

        private static double TA_RealBody(IReadOnlyList<double> close, IReadOnlyList<double> open, int idx)
        {
            return Math.Abs(close[idx] - open[idx]);
        }

        private static decimal TA_RealBody(IReadOnlyList<decimal> close, IReadOnlyList<decimal> open, int idx)
        {
            return Math.Abs(close[idx] - open[idx]);
        }

        private static double TA_UpperShadow(IReadOnlyList<double> high, IReadOnlyList<double> close, IReadOnlyList<double> open, int idx)
        {
            return high[idx] - (close[idx] >= open[idx] ? close[idx] : open[idx]);
        }

        private static decimal TA_UpperShadow(IReadOnlyList<decimal> high, IReadOnlyList<decimal> close, IReadOnlyList<decimal> open,
            int idx)
        {
            return high[idx] - (close[idx] >= open[idx] ? close[idx] : open[idx]);
        }

        private static double TA_LowerShadow(IReadOnlyList<double> close, IReadOnlyList<double> open, IReadOnlyList<double> low, int idx)
        {
            return (close[idx] >= open[idx] ? open[idx] : close[idx]) - low[idx];
        }

        private static decimal TA_LowerShadow(IReadOnlyList<decimal> close, IReadOnlyList<decimal> open, IReadOnlyList<decimal> low,
            int idx)
        {
            return (close[idx] >= open[idx] ? open[idx] : close[idx]) - low[idx];
        }

        private static double TA_HighLowRange(IReadOnlyList<double> high, IReadOnlyList<double> low, int idx)
        {
            return high[idx] - low[idx];
        }

        private static decimal TA_HighLowRange(IReadOnlyList<decimal> high, IReadOnlyList<decimal> low, int idx)
        {
            return high[idx] - low[idx];
        }

        private static bool TA_CandleColor(IReadOnlyList<double> close, IReadOnlyList<double> open, int idx)
        {
            return close[idx] >= open[idx];
        }

        private static bool TA_CandleColor(IReadOnlyList<decimal> close, IReadOnlyList<decimal> open, int idx)
        {
            return close[idx] >= open[idx];
        }

        private static RangeType TA_CandleRangeType(CandleSettingType set)
        {
            return Globals.CandleSettings[(int) set].RangeType;
        }

        private static int TA_CandleAvgPeriod(CandleSettingType set)
        {
            return Globals.CandleSettings[(int) set].AvgPeriod;
        }

        private static double TA_CandleFactor(CandleSettingType set)
        {
            return Globals.CandleSettings[(int) set].Factor;
        }

        private static double TA_CandleRange(IReadOnlyList<double> open, IReadOnlyList<double> high, IReadOnlyList<double> low,
            IReadOnlyList<double> close, CandleSettingType set, int idx)
        {
            return TA_CandleRangeType(set) switch
            {
                RangeType.RealBody => TA_RealBody(close, open, idx),
                RangeType.HighLow => TA_HighLowRange(high, low, idx),
                RangeType.Shadows => TA_UpperShadow(high, close, open, idx) + TA_LowerShadow(close, open, low, idx),
                _ => 0
            };
        }

        private static decimal TA_CandleRange(IReadOnlyList<decimal> open, IReadOnlyList<decimal> high, IReadOnlyList<decimal> low,
            IReadOnlyList<decimal> close, CandleSettingType set, int idx)
        {
            return TA_CandleRangeType(set) switch
            {
                RangeType.RealBody => TA_RealBody(close, open, idx),
                RangeType.HighLow => TA_HighLowRange(high, low, idx),
                RangeType.Shadows => TA_UpperShadow(high, close, open, idx) + TA_LowerShadow(close, open, low, idx),
                _ => 0
            };
        }

        private static double TA_CandleAverage(IReadOnlyList<double> open, IReadOnlyList<double> high, IReadOnlyList<double> low,
            IReadOnlyList<double> close, CandleSettingType set, double sum, int idx)
        {
            return TA_CandleFactor(set) * (TA_CandleAvgPeriod(set) != 0
                ? sum / TA_CandleAvgPeriod(set)
                : TA_CandleRange(open, high, low, close, set, idx)) / (TA_CandleRangeType(set) == RangeType.Shadows ? 2.0 : 1.0);
        }

        private static decimal TA_CandleAverage(IReadOnlyList<decimal> open, IReadOnlyList<decimal> high, IReadOnlyList<decimal> low,
            IReadOnlyList<decimal> close, CandleSettingType set, decimal sum, int idx)
        {
            return (decimal) TA_CandleFactor(set) * (TA_CandleAvgPeriod(set) != 0
                       ? sum / TA_CandleAvgPeriod(set)
                       : TA_CandleRange(open, high, low, close, set, idx)) /
                   (TA_CandleRangeType(set) == RangeType.Shadows ? 2m : Decimal.One);
        }

        private static bool TA_RealBodyGapUp(IReadOnlyList<double> open, IReadOnlyList<double> close, int idx2, int idx1)
        {
            return Math.Min(open[idx2], close[idx2]) > Math.Max(open[idx1], close[idx1]);
        }

        private static bool TA_RealBodyGapUp(IReadOnlyList<decimal> open, IReadOnlyList<decimal> close, int idx2, int idx1)
        {
            return Math.Min(open[idx2], close[idx2]) > Math.Max(open[idx1], close[idx1]);
        }

        private static bool TA_RealBodyGapDown(IReadOnlyList<double> open, IReadOnlyList<double> close, int idx2, int idx1)
        {
            return Math.Max(open[idx2], close[idx2]) < Math.Min(open[idx1], close[idx1]);
        }

        private static bool TA_RealBodyGapDown(IReadOnlyList<decimal> open, IReadOnlyList<decimal> close, int idx2, int idx1)
        {
            return Math.Max(open[idx2], close[idx2]) < Math.Min(open[idx1], close[idx1]);
        }

        private static bool TA_CandleGapUp(IReadOnlyList<double> low, IReadOnlyList<double> high, int idx2, int idx1)
        {
            return low[idx2] > high[idx1];
        }

        private static bool TA_CandleGapUp(IReadOnlyList<decimal> low, IReadOnlyList<decimal> high, int idx2, int idx1)
        {
            return low[idx2] > high[idx1];
        }

        private static bool TA_CandleGapDown(IReadOnlyList<double> low, IReadOnlyList<double> high, int idx2, int idx1)
        {
            return high[idx2] < low[idx1];
        }

        private static bool TA_CandleGapDown(IReadOnlyList<decimal> low, IReadOnlyList<decimal> high, int idx2, int idx1)
        {
            return high[idx2] < low[idx1];
        }

        private static bool TA_IsZero(double v) => -TA_EPSILON < v && v < TA_EPSILON;

        private static bool TA_IsZero(decimal v) => -(decimal) TA_EPSILON < v && v < (decimal) TA_EPSILON;

        private static bool TA_IsZeroOrNeg(double v) => v < TA_EPSILON;

        private static bool TA_IsZeroOrNeg(decimal v) => v < (decimal) TA_EPSILON;

        private static void TrueRange(double th, double tl, double yc, out double @out)
        {
            @out = th - tl;
            double tempDouble = Math.Abs(th - yc);
            if (tempDouble > @out)
            {
                @out = tempDouble;
            }

            tempDouble = Math.Abs(tl - yc);
            if (tempDouble > @out)
            {
                @out = tempDouble;
            }
        }

        private static void TrueRange(decimal th, decimal tl, decimal yc, out decimal @out)
        {
            @out = th - tl;
            decimal tempDecimal = Math.Abs(th - yc);
            if (tempDecimal > @out)
            {
                @out = tempDecimal;
            }

            tempDecimal = Math.Abs(tl - yc);
            if (tempDecimal > @out)
            {
                @out = tempDecimal;
            }
        }

        private static void DoPriceWma(double[] real, ref int idx, ref double periodWMASub, ref double periodWMASum,
            ref double trailingWMAValue, double varNewPrice, out double varToStoreSmoothedValue)
        {
            periodWMASub += varNewPrice;
            periodWMASub -= trailingWMAValue;
            periodWMASum += varNewPrice * 4.0;
            trailingWMAValue = real[idx++];
            varToStoreSmoothedValue = periodWMASum * 0.1;
            periodWMASum -= periodWMASub;
        }

        private static void DoPriceWma(decimal[] real, ref int idx, ref decimal periodWMASub, ref decimal periodWMASum,
            ref decimal trailingWMAValue, decimal varNewPrice, out decimal varToStoreSmoothedValue)
        {
            periodWMASub += varNewPrice;
            periodWMASub -= trailingWMAValue;
            periodWMASum += varNewPrice * 4m;
            trailingWMAValue = real[idx++];
            varToStoreSmoothedValue = periodWMASum * 0.1m;
            periodWMASum -= periodWMASub;
        }

        private static IDictionary<string, T> InitHilbertVariables<T>() where T : struct, IComparable<T>
        {
            var variables = new Dictionary<string, T>(4 * 11);

            new List<string> { "detrender", "q1", "jI", "jQ" }.ForEach(varName =>
            {
                variables.Add($"{varName}Odd0", default);
                variables.Add($"{varName}Odd1", default);
                variables.Add($"{varName}Odd2", default);
                variables.Add($"{varName}Even0", default);
                variables.Add($"{varName}Even1", default);
                variables.Add($"{varName}Even2", default);
                variables.Add(varName, default);
                variables.Add($"prev{varName}Odd", default);
                variables.Add($"prev{varName}Even", default);
                variables.Add($"prev{varName}InputOdd", default);
                variables.Add($"prev{varName}InputEven", default);
            });

            return variables;
        }

        private static void DoHilbertTransform(IDictionary<string, double> variables, string varName, double input, string oddOrEvenId,
            int hilbertIdx, double adjustedPrevPeriod)
        {
            const double a = 0.0962;
            const double b = 0.5769;

            double hilbertTempDouble = a * input;
            variables[varName] = -variables[$"{varName}{oddOrEvenId}{hilbertIdx}"];
            variables[$"{varName}{oddOrEvenId}{hilbertIdx}"] = hilbertTempDouble;
            variables[varName] += hilbertTempDouble;
            variables[varName] -= variables[$"prev{varName}{oddOrEvenId}"];
            variables[$"prev{varName}{oddOrEvenId}"] = b * variables[$"prev{varName}Input{oddOrEvenId}"];
            variables[varName] += variables[$"prev{varName}{oddOrEvenId}"];
            variables[$"prev{varName}Input{oddOrEvenId}"] = input;
            variables[varName] *= adjustedPrevPeriod;
        }

        private static void DoHilbertTransform(IDictionary<string, decimal> variables, string varName, decimal input, string oddOrEvenId,
            int hilbertIdx, decimal adjustedPrevPeriod)
        {
            const decimal a = 0.0962m;
            const decimal b = 0.5769m;

            decimal hilbertTempDecimal = a * input;
            variables[varName] = -variables[$"{varName}{oddOrEvenId}{hilbertIdx}"];
            variables[$"{varName}{oddOrEvenId}{hilbertIdx}"] = hilbertTempDecimal;
            variables[varName] += hilbertTempDecimal;
            variables[varName] -= variables[$"prev{varName}{oddOrEvenId}"];
            variables[$"prev{varName}{oddOrEvenId}"] = b * variables[$"prev{varName}Input{oddOrEvenId}"];
            variables[varName] += variables[$"prev{varName}{oddOrEvenId}"];
            variables[$"prev{varName}Input{oddOrEvenId}"] = input;
            variables[varName] *= adjustedPrevPeriod;
        }

        private static void DoHilbertOdd(IDictionary<string, double> variables, string varName, double input, int hilbertIdx,
            double adjustedPrevPeriod)
        {
            DoHilbertTransform(variables, varName, input, "Odd", hilbertIdx, adjustedPrevPeriod);
        }

        private static void DoHilbertOdd(IDictionary<string, decimal> variables, string varName, decimal input, int hilbertIdx,
            decimal adjustedPrevPeriod)
        {
            DoHilbertTransform(variables, varName, input, "Odd", hilbertIdx, adjustedPrevPeriod);
        }

        private static void DoHilbertEven(IDictionary<string, double> variables, string varName, double input, int hilbertIdx,
            double adjustedPrevPeriod)
        {
            DoHilbertTransform(variables, varName, input, "Even", hilbertIdx, adjustedPrevPeriod);
        }

        private static void DoHilbertEven(IDictionary<string, decimal> variables, string varName, decimal input, int hilbertIdx,
            decimal adjustedPrevPeriod)
        {
            DoHilbertTransform(variables, varName, input, "Even", hilbertIdx, adjustedPrevPeriod);
        }

        private static void CalcTerms(IReadOnlyList<double> inLow, IReadOnlyList<double> inHigh, IReadOnlyList<double> inClose, int day,
            out double trueRange, out double closeMinusTrueLow)
        {
            double tempLT = inLow[day];
            double tempHT = inHigh[day];
            double tempCY = inClose[day - 1];
            double trueLow = Math.Min(tempLT, tempCY);
            closeMinusTrueLow = inClose[day] - trueLow;
            trueRange = tempHT - tempLT;
            double tempDouble = Math.Abs(tempCY - tempHT);
            if (tempDouble > trueRange)
            {
                trueRange = tempDouble;
            }

            tempDouble = Math.Abs(tempCY - tempLT);
            if (tempDouble > trueRange)
            {
                trueRange = tempDouble;
            }
        }

        private static void CalcTerms(IReadOnlyList<decimal> inLow, IReadOnlyList<decimal> inHigh, IReadOnlyList<decimal> inClose, int day,
            out decimal trueRange, out decimal closeMinusTrueLow)
        {
            decimal tempLT = inLow[day];
            decimal tempHT = inHigh[day];
            decimal tempCY = inClose[day - 1];
            decimal trueLow = Math.Min(tempLT, tempCY);
            closeMinusTrueLow = inClose[day] - trueLow;
            trueRange = tempHT - tempLT;
            decimal tempDecimal = Math.Abs(tempCY - tempHT);
            if (tempDecimal > trueRange)
            {
                trueRange = tempDecimal;
            }

            tempDecimal = Math.Abs(tempCY - tempLT);
            if (tempDecimal > trueRange)
            {
                trueRange = tempDecimal;
            }
        }
    }
}
