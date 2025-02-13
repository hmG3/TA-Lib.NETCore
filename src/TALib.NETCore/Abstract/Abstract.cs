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

using System.Collections;
using System.Collections.ObjectModel;
using F = TALib.Abstract.IndicatorFunction;
using OH = TALib.Core.OutputDisplayHints;

namespace TALib;

/// <summary>
/// Provides Abstraction layer for accessing all functions.
/// Abstract API simplifies the usage of individual functions by offering a unified interface for setting and controlling input data,
/// configuring function parameters, and retrieving results.
/// </summary>
public partial class Abstract : IEnumerable<F>
{
    internal readonly ReadOnlyDictionary<string, F> FunctionsDefinition;

    private static readonly Lazy<Abstract> Instance = new(() => new Abstract());

    private Abstract()
    {
        const string highInput = "High";
        const string lowInput = "Low";
        const string closeInput = "Close";
        const string openInput = "Open";
        const string volumeInput = "Volume";
        const string realInput = "Real";

        const string realOutput = "Real";
        const string integerOutput = "Integer";
        const string integerTypeOutput = "IntType";

        const string maTypeOption = "MA Type";
        const string timePeriodOption = "Time Period";
        const string penetrationOption = "Penetration";

        const string prGroup = "Pattern Recognition";
        const string miGroup = "Momentum Indicators";
        const string osGroup = "Overlap Studies";
        const string vmiGroup = "Volume Indicators";
        const string mtGroup = "Math Transform";
        const string moGroup = "Math Operators";
        const string sfGroup = "Statistic Functions";
        const string ptGroup = "Price Transform";
        const string ciGroup = "Cycle Indicators";
        const string vliGroup = "Volatility Indicators";

        string[] inputPriceOHLC = [openInput, highInput, lowInput, closeInput];
        string[] inputPriceHLC = [highInput, lowInput, closeInput];
        string[] inputPriceHLCV = [highInput, lowInput, closeInput, volumeInput];
        string[] inputPriceHL = [highInput, lowInput];
        string[] inputReal = [realInput];
        string[] inputRealUnary = [realInput, realInput];

        (string, OH)[] outputReal = [(realOutput, OH.Line)];
        (string, OH)[] outputIntLine = [(integerOutput, OH.Line)];
        (string, OH)[] outputIntTypePattern = [(integerTypeOutput, OH.PatternBullBear)];
        (string displayName, OH displayHint)[] outputBands =
            [("Real Upper Band", OH.UpperLimit), ("Real Middle Band", OH.Line), ("Real Lower Band", OH.LowerLimit)];
        (string displayName, OH displayHint)[] outputMacd = [("MACD", OH.Line), ("MACD Signal", OH.DashLine), ("MACD Hist", OH.Histo)];
        (string displayName, OH displayHint)[] outputStoch = [("Fast K", OH.Line), ("Fast D", OH.Line)];

        (string, string)[] optionTimePeriod = [(timePeriodOption, "Number of periods")];
        (string, string)[] optionPenetration = [(penetrationOption, "Percentage of penetration of a candle within another candle")];
        (string displayName, string hint) optionMAType = (maTypeOption, "Type of Moving Average");
        var optionFastPeriod = ("Fast Period", "Number of periods for the fast moving average");
        var optionSlowPeriod = ("Slow Period", "Number of periods for the slow moving average");
        var optionSignalPeriod = ("Signal Period", "Smoothing for the signal line (nb of period)");

        var funcDefs = new Dictionary<string, F>(StringComparer.OrdinalIgnoreCase)
        {
            { "ACCBANDS", new F("Accbands", "Acceleration Bands", osGroup, inputPriceHLC, optionTimePeriod, outputBands) },
            { "ACOS", new F("Acos", "Vector Trigonometric ACos", mtGroup, inputReal, [], outputReal) },
            { "AD", new F("Ad", "Chaikin A/D Line", vmiGroup, inputPriceHLCV, [], outputReal) },
            { "ADD", new F("Add", "Vector Arithmetic Addition", moGroup, inputRealUnary, [], outputReal) },
            {
                "ADOSC",
                new F("AdOsc", "Chaikin A/D Oscillator", vmiGroup, inputPriceHLCV, [optionFastPeriod, optionSlowPeriod], outputReal)
            },
            { "ADX", new F("Adx", "Average Directional Movement Index", miGroup, inputPriceHLC, optionTimePeriod, outputReal) },
            { "ADXR", new F("Adxr", "Average Directional Movement Index Rating", miGroup, inputPriceHLC, optionTimePeriod, outputReal) },
            {
                "APO",
                new F("Apo", "Absolute Price Oscillator", miGroup, inputReal, [optionFastPeriod, optionSlowPeriod, optionMAType],
                    outputReal)
            },
            {
                "AROON",
                new F("Aroon", "Aroon", miGroup, inputPriceHL, optionTimePeriod, [("Aroon Down", OH.DashLine), ("Aroon Up", OH.Line)])
            },
            { "AROONOSC", new F("AroonOsc", "Aroon Oscillator", miGroup, inputPriceHL, optionTimePeriod, outputReal) },
            { "ASIN", new F("Asin", "Vector Trigonometric ASin", mtGroup, inputReal, [], outputReal) },
            { "ATAN", new F("Atan", "Vector Trigonometric ATan", mtGroup, inputReal, [], outputReal) },
            {
                "ATR", new F("Atr", "Average True Range", vliGroup, inputPriceHLC, optionTimePeriod, outputReal)
            },
            { "AVGDEV", new F("AvgDev", "Average Deviation", ptGroup, inputReal, optionTimePeriod, outputReal) },
            { "AVGPRICE", new F("AvgPrice", "Average Price", ptGroup, inputPriceOHLC, [], outputReal) },
            {
                "BBANDS",
                new F("Bbands", "Bollinger Bands", osGroup, inputReal, [
                    ..optionTimePeriod, ("Nb Dev Up", "Deviation multiplier for upper band"),
                    ("Nb Dev Dn", "Deviation multiplier for lower band"), optionMAType
                ], outputBands)
            },
            { "BETA", new F("Beta", "Beta", sfGroup, inputRealUnary, optionTimePeriod, outputReal) },
            { "BOP", new F("Bop", "Balance of Power", miGroup, inputPriceOHLC, [], outputReal) },
            { "CCI", new F("Cci", "Commodity Channel Index", miGroup, inputPriceHLC, optionTimePeriod, outputReal) },
            { "CEIL", new F("Ceil", "Vector Ceil", mtGroup, inputReal, [], outputReal) },
            { "CMO", new F("Cmo", "Chande Momentum Oscillator", miGroup, inputReal, optionTimePeriod, outputReal) },
            { "CORREL", new F("Correl", "Pearson's Correlation Coefficient (r)", sfGroup, inputRealUnary, optionTimePeriod, outputReal) },
            { "COS", new F("Cos", "Vector Trigonometric Cos", mtGroup, inputReal, [], outputReal) },
            { "COSH", new F("Cosh", "Vector Trigonometric Cosh", mtGroup, inputReal, [], outputReal) },
            { "DEMA", new F("Dema", "Double Exponential Moving Average", osGroup, inputReal, optionTimePeriod, outputReal) },
            { "DIV", new F("Div", "Vector Arithmetic Division", moGroup, inputRealUnary, [], outputReal) },
            { "DX", new F("Dx", "Directional Movement Index", miGroup, inputPriceHLC, optionTimePeriod, outputReal) },
            { "EMA", new F("Ema", "Exponential Moving Average", osGroup, inputReal, optionTimePeriod, outputReal) },
            { "EXP", new F("Exp", "Vector Arithmetic Exp", mtGroup, inputReal, [], outputReal) },
            { "FLOOR", new F("Floor", "Vector Floor", mtGroup, inputReal, [], outputReal) },
            { "HTDCPERIOD", new F("HtDcPeriod", "Hilbert Transform - Dominant Cycle Period", ciGroup, inputReal, [], outputReal) },
            { "HTDCPHASE", new F("HtDcPhase", "Hilbert Transform - Dominant Cycle Phase", ciGroup, inputReal, [], outputReal) },
            {
                "HTPHASOR",
                new F("HtPhasor", "Hilbert Transform - Phasor Components", ciGroup, inputReal, [],
                    [("In Phase", OH.Line), ("Quadrature", OH.DashLine)])
            },
            {
                "HTSINE",
                new F("HtSine", "Hilbert Transform - SineWave", ciGroup, inputReal, [], [("Sine", OH.Line), ("Lead Sine", OH.DashLine)])
            },
            { "HTTRENDLINE", new F("HtTrendline", "Hilbert Transform - Instantaneous Trendline", osGroup, inputReal, [], outputReal) },
            { "HTTRENDMODE", new F("HtTrendMode", "Hilbert Transform - Trend vs Cycle Mode", ciGroup, inputReal, [], outputIntLine) },
            { "KAMA", new F("Kama", "Kaufman Adaptive Moving Average", osGroup, inputReal, optionTimePeriod, outputReal) },
            { "LINEARREG", new F("LinearReg", "Linear Regression", sfGroup, inputReal, optionTimePeriod, outputReal) },
            { "LINEARREGANGLE", new F("LinearRegAngle", "Linear Regression Angle", sfGroup, inputReal, optionTimePeriod, outputReal) },
            {
                "LINEARREGINTERCEPT",
                new F("LinearRegIntercept", "Linear Regression Intercept", sfGroup, inputReal, optionTimePeriod, outputReal)
            },
            { "LINEARREGSLOPE", new F("LinearRegSlope", "Linear Regression Slope", sfGroup, inputReal, optionTimePeriod, outputReal) },
            { "LN", new F("Ln", "Vector Log Natural", mtGroup, inputReal, [], outputReal) },
            { "LOG10", new F("Log10", "Vector Log10", mtGroup, inputReal, [], outputReal) },
            { "MA", new F("Ma", "Moving Average", osGroup, inputReal, [..optionTimePeriod, optionMAType], outputReal) },
            {
                "MACD",
                new F("Macd", "Moving Average Convergence/Divergence", miGroup, inputReal,
                    [optionFastPeriod, optionSlowPeriod, optionSignalPeriod], outputMacd)
            },
            {
                "MACDEXT",
                new F("MacdExt", "MACD with controllable MA type", miGroup, inputReal,
                    [
                        optionFastPeriod, ($"Fast {maTypeOption}", $"{optionMAType.hint} for fast MA"), optionSlowPeriod,
                        ($"Slow {maTypeOption}", $"{optionMAType.hint} for slow MA"), optionSignalPeriod,
                        ($"Signal {maTypeOption}", $"{optionMAType.hint} for signal line")
                    ],
                    outputMacd)
            },
            {
                "MACDFIX",
                new F("MacdFix", "Moving Average Convergence/Divergence Fix 12/26", miGroup, inputReal, [optionSignalPeriod], outputMacd)
            },
            {
                "MAMA",
                new F("Mama", "MESA Adaptive Moving Average", osGroup, inputReal,
                    [
                        ("Fast Limit", "Upper limit use in the adaptive algorithm"),
                        ("Slow Limit", "Lower limit use in the adaptive algorithm")
                    ],
                    [("MAMA", OH.Line), ("FAMA", OH.DashLine)])
            },
            {
                "MAVP",
                new F("Mavp", "Moving average with variable period", osGroup, [realInput, "Periods"],
                [
                    ("Min Period", "Value less than minimum will be changed to Minimum period"),
                    ("Max Period", "Value higher than maximum will be changed to Maximum period"), optionMAType
                ], outputReal)
            },
            { "MAX", new F("Max", "Highest value over a specified period", moGroup, inputReal, optionTimePeriod, outputReal) },
            {
                "MAXINDEX",
                new F("MaxIndex", "Index of highest value over a specified period", moGroup, inputReal, optionTimePeriod, outputIntLine)
            },
            { "MEDPRICE", new F("MedPrice", "Median Price", ptGroup, inputPriceHL, [], outputReal) },
            { "MFI", new F("Mfi", "Money Flow Index", miGroup, inputPriceHLCV, optionTimePeriod, outputReal) },
            { "MIDPOINT", new F("MidPoint", "MidPoint over period", osGroup, inputReal, optionTimePeriod, outputReal) },
            { "MIDPRICE", new F("MidPrice", "Midpoint Price over period", osGroup, inputPriceHL, optionTimePeriod, outputReal) },
            { "MIN", new F("Min", "Lowest value over a specified period", moGroup, inputReal, optionTimePeriod, outputReal) },
            {
                "MININDEX",
                new F("MinIndex", "Index of lowest value over a specified period", moGroup, inputReal, optionTimePeriod, outputIntLine)
            },
            {
                "MINMAX",
                new F("MinMax", "Lowest and highest values over a specified period", moGroup, inputReal, optionTimePeriod,
                    [("Min", OH.Line), ("Max", OH.Line)])
            },
            {
                "MINMAXINDEX",
                new F("MinMaxIndex", "Indexes of lowest and highest values over a specified period", moGroup, inputReal, optionTimePeriod,
                    [("Min Idx", OH.Line), ("Max Idx", OH.Line)])
            },
            { "MINUSDI", new F("MinusDI", "Minus Directional Indicator", miGroup, inputPriceHLC, optionTimePeriod, outputReal) },
            { "MINUSDM", new F("MinusDM", "Minus Directional Movement", miGroup, inputPriceHL, optionTimePeriod, outputReal) },
            { "MOM", new F("Mom", "Momentum", miGroup, inputReal, optionTimePeriod, outputReal) },
            { "MULT", new F("Mult", "Vector Arithmetic Multiplication", moGroup, inputRealUnary, [], outputReal) },
            { "NATR", new F("Natr", "Normalized Average True Range", vliGroup, inputPriceHLC, optionTimePeriod, outputReal) },
            { "OBV", new F("Obv", "On Balance Volume", vmiGroup, [realInput, volumeInput], [], outputReal) },
            { "PLUSDI", new F("PlusDI", "Plus Directional Indicator", miGroup, inputPriceHLC, optionTimePeriod, outputReal) },
            { "PLUSDM", new F("PlusDM", "Plus Directional Movement", miGroup, inputPriceHL, optionTimePeriod, outputReal) },
            {
                "PPO",
                new F("Ppo", "Percentage Price Oscillator", miGroup, inputReal, [optionFastPeriod, optionSlowPeriod, optionMAType],
                    outputReal)
            },
            { "ROC", new F("Roc", "Rate of change : ((price/prevPrice)-1)*100", miGroup, inputReal, optionTimePeriod, outputReal) },
            {
                "ROCP",
                new F("RocP", "Rate of change Percentage: (price-prevPrice)/prevPrice", miGroup, inputReal, optionTimePeriod, outputReal)
            },
            { "ROCR", new F("RocR", "Rate of change ratio: (price/prevPrice)", miGroup, inputReal, optionTimePeriod, outputReal) },
            {
                "ROCR100",
                new F("RocR100", "Rate of change ratio 100 scale: (price/prevPrice)*100", miGroup, inputReal, optionTimePeriod, outputReal)
            },
            { "RSI", new F("Rsi", "Relative Strength Index", miGroup, inputReal, optionTimePeriod, outputReal) },
            {
                "SAR",
                new F("Sar", "Parabolic SAR", osGroup, inputPriceHL,
                [
                    ("Acceleration", "Acceleration Factor used up to the Maximum value"), ("Maximum", "Acceleration Factor Maximum value")
                ], outputReal)
            },
            {
                "SAREXT",
                new F("SarExt", "Parabolic SAR - Extended", osGroup, inputPriceHL,
                    [
                        ("Start Value", "Start value and direction. 0 for Auto, >0 for Long, <0 for Short"),
                        ("Offset On Reverse", "Percent offset added/removed to initial stop on short/long reversal"),
                        ("Acceleration Init Long", "Acceleration Factor initial value for the Long direction"),
                        ("Acceleration Long", "Acceleration Factor for the Long direction"),
                        ("Acceleration Max Long", "Acceleration Factor maximum value for the Long direction"),
                        ("Acceleration Init Short", "Acceleration Factor initial value for the Short direction"),
                        ("Acceleration Short", "Acceleration Factor for the Short direction"),
                        ("Acceleration Max Short", "Acceleration Factor maximum value for the Short direction")
                    ],
                    outputReal)
            },
            { "SIN", new F("Sin", "Vector Trigonometric Sin", mtGroup, inputReal, [], outputReal) },
            { "SINH", new F("Sinh", "Vector Trigonometric Sinh", mtGroup, inputReal, [], outputReal) },
            { "SMA", new F("Sma", "Simple Moving Average", osGroup, inputReal, optionTimePeriod, outputReal) },
            { "SQRT", new F("Sqrt", "Vector Square Root", mtGroup, inputReal, [], outputReal) },
            {
                "STDDEV",
                new F("StdDev", "Standard Deviation", sfGroup, inputReal, [..optionTimePeriod, ("Nb Dev", "Nb of deviations")], outputReal)
            },
            {
                "STOCH",
                new F("Stoch", "Stochastic", miGroup, inputPriceHLC,
                    [
                        ("Fast K Period", "Time period for building the Fast-K line"),
                        ("Slow K Period", "Smoothing for making the Slow-K line. Usually set to 3"),
                        ($"Slow K {maTypeOption}", $"{optionMAType.hint} for Slow-K"),
                        ("Slow D Period", "Smoothing for making the Slow-D line"),
                        ($"Slow D {maTypeOption}", $"{optionMAType.hint} for Slow-D")
                    ],
                    [("Slow K", OH.DashLine), ("Slow D", OH.DashLine)])
            },
            {
                "STOCHF",
                new F("StochF", "Stochastic Fast", miGroup, inputPriceHLC,
                [
                    ("Fast K Period", "Time period for building the Fast-K line"),
                    ("Fast D Period", "Smoothing for making the Fast-D line. Usually set to 3"),
                    ($"Fast D {maTypeOption}", $"{optionMAType.hint} for Fast-D")
                ], outputStoch)
            },
            {
                "STOCHRSI",
                new F("StochRsi", "Stochastic Relative Strength Index", miGroup, inputReal,
                [
                    ..optionTimePeriod, ("Fast K Period", "Time period for building the Fast-K line"),
                    ("Fast D Period", "Smoothing for making the Fast-D line. Usually set to 3"),
                    ($"Fast D {maTypeOption}", $"{optionMAType.hint} for Fast-D")
                ], outputStoch)
            },
            { "SUB", new F("Sub", "Vector Arithmetic Subtraction", moGroup, inputRealUnary, [], outputReal) },
            { "SUM", new F("Sum", "Summation", moGroup, inputReal, optionTimePeriod, outputReal) },
            { "T3", new F("T3", "T3 Moving Average", osGroup, inputReal, [..optionTimePeriod, ("V Factor", "Volume Factor")], outputReal) },
            { "TAN", new F("Tan", "Vector Trigonometric Tan", mtGroup, inputReal, [], outputReal) },
            { "TANH", new F("Tanh", "Vector Trigonometric Tanh", mtGroup, inputReal, [], outputReal) },
            { "TEMA", new F("Tema", "Triple Exponential Moving Average", osGroup, inputReal, optionTimePeriod, outputReal) },
            { "TRANGE", new F("TRange", "True Range", vliGroup, inputPriceHLC, [], outputReal) },
            { "TRIMA", new F("Trima", "Triangular Moving Average", osGroup, inputReal, optionTimePeriod, outputReal) },
            {
                "TRIX", new F("Trix", "1-day Rate-Of-Change (ROC) of a Triple Smooth EMA", miGroup, inputReal, optionTimePeriod, outputReal)
            },
            { "TSF", new F("Tsf", "Time Series Forecast", sfGroup, inputReal, optionTimePeriod, outputReal) },
            { "TYPPRICE", new F("TypPrice", "Typical Price", ptGroup, inputPriceHLC, [], outputReal) },
            {
                "ULTOSC",
                new F("UltOsc", "Ultimate Oscillator", miGroup, inputPriceHLC,
                [
                    ($"{timePeriodOption} 1", "Number of bars for 1st period"), ($"{timePeriodOption} 2", "Number of bars for 2nd period"),
                    ($"{timePeriodOption} 3", "Number of bars for 3rd period")
                ], outputReal)
            },
            { "VAR", new F("Var", "Variance", sfGroup, inputReal, optionTimePeriod, outputReal) },
            { "WCLPRICE", new F("WclPrice", "Weighted Close Price", ptGroup, inputPriceHLC, [], outputReal) },
            { "WILLR", new F("WillR", "Williams' %R", miGroup, inputPriceHLC, optionTimePeriod, outputReal) },
            { "WMA", new F("Wma", "Weighted Moving Average", osGroup, inputReal, optionTimePeriod, outputReal) },
            { "ABANDONEDBABY", new F("AbandonedBaby", "Abandoned Baby", prGroup, inputPriceOHLC, optionPenetration, outputIntTypePattern) },
            { "ADVANCEBLOCK", new F("AdvanceBlock", "Advance Block", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "BELTHOLD", new F("BeltHold", "Belt-hold", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "BREAKAWAY", new F("Breakaway", "Breakaway", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "CLOSINGMARUBOZU", new F("ClosingMarubozu", "Closing Marubozu", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            {
                "CONCEALINGBABYSWALLOW",
                new F("ConcealingBabySwallow", "Concealing Baby Swallow", prGroup, inputPriceOHLC, [], outputIntTypePattern)
            },
            { "COUNTERATTACK", new F("Counterattack", "Counterattack", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            {
                "DARKCLOUDCOVER",
                new F("DarkCloudCover", "Dark Cloud Cover", prGroup, inputPriceOHLC, optionPenetration, outputIntTypePattern)
            },
            { "DOJI", new F("Doji", "Doji", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "DOJISTAR", new F("DojiStar", "Doji Star", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "DRAGONFLYDOJI", new F("DragonflyDoji", "Dragonfly Doji", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "ENGULFING", new F("Engulfing", "Engulfing Pattern", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            {
                "EVENINGDOJISTAR",
                new F("EveningDojiStar", "Evening Doji Star", prGroup, inputPriceOHLC, optionPenetration, outputIntTypePattern)
            },
            { "EVENINGSTAR", new F("EveningStar", "Evening Star", prGroup, inputPriceOHLC, optionPenetration, outputIntTypePattern) },
            {
                "GAPSIDEBYSIDEWHITELINES",
                new F("GapSideBySideWhiteLines", "Up/Down-gap side-by-side white lines", prGroup, inputPriceOHLC, [], outputIntTypePattern)
            },
            { "GRAVESTONEDOJI", new F("GravestoneDoji", "Gravestone Doji", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "HAMMER", new F("Hammer", "Hammer", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "HANGINGMAN", new F("HangingMan", "Hanging Man", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "HARAMI", new F("Harami", "Harami Pattern", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "HARAMICROSS", new F("HaramiCross", "Harami Cross Pattern", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "HIGHWAVE", new F("HighWave", "High-Wave Candle", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            {
                "HIKKAKE",
                new F("Hikkake", "Hikkake Pattern", prGroup, inputPriceOHLC, [],
                    [(integerTypeOutput, OH.PatternBullBear | OH.PatternStrength)])
            },
            {
                "HIKKAKEMODIFIED",
                new F("HikkakeModified", "Modified Hikkake Pattern", prGroup, inputPriceOHLC, [],
                    [(integerTypeOutput, OH.PatternBullBear | OH.PatternStrength)])
            },
            { "HOMINGPIGEON", new F("HomingPigeon", "Homing Pigeon", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            {
                "IDENTICALTHREECROWS",
                new F("IdenticalThreeCrows", "Identical Three Crows", prGroup, inputPriceOHLC, [], outputIntTypePattern)
            },
            { "INNECK", new F("InNeck", "In-Neck Pattern", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "INVERTEDHAMMER", new F("InvertedHammer", "Inverted Hammer", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "KICKING", new F("Kicking", "Kicking", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            {
                "KICKINGBYLENGTH",
                new F("KickingByLength", "Kicking - bull/bear determined by the longer marubozu", prGroup, inputPriceOHLC, [],
                    outputIntTypePattern)
            },
            { "LADDERBOTTOM", new F("LadderBottom", "Ladder Bottom", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "LONGLEGGEDDOJI", new F("LongLeggedDoji", "Long Legged Doji", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "LONGLINE", new F("LongLine", "Long Line Candle", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "MARUBOZU", new F("Marubozu", "Marubozu", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "MATCHINGLOW", new F("MatchingLow", "Matching Low", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "MATHOLD", new F("MatHold", "Mat Hold", prGroup, inputPriceOHLC, optionPenetration, outputIntTypePattern) },
            {
                "MORNINGDOJISTAR",
                new F("MorningDojiStar", "Morning Doji Star", prGroup, inputPriceOHLC, optionPenetration, outputIntTypePattern)
            },
            { "MORNINGSTAR", new F("MorningStar", "Morning Star", prGroup, inputPriceOHLC, optionPenetration, outputIntTypePattern) },
            { "ONNECK", new F("OnNeck", "On-Neck Pattern", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "PIERCINGLINE", new F("PiercingLine", "Piercing Line", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "RICKSHAWMAN", new F("RickshawMan", "Rickshaw Man", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            {
                "RISINGFALLINGTHREEMETHODS",
                new F("RisingFallingThreeMethods", "Rising/Falling Three Methods", prGroup, inputPriceOHLC, [], outputIntTypePattern)
            },
            { "SEPARATINGLINES", new F("SeparatingLines", "Separating Lines", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "SHOOTINGSTAR", new F("ShootingStar", "Shooting Star", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "SHORTLINE", new F("ShortLine", "Short Line Candle", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "SPINNINGTOP", new F("SpinningTop", "Spinning Top", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "STALLED", new F("Stalled", "Stalled Pattern", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "STICKSANDWICH", new F("StickSandwich", "Stick Sandwich", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "TAKURILINE", new F("TakuriLine", "Takuri Line", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "TASUKIGAP", new F("TasukiGap", "Tasuki Gap", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "THREEBLACKCROWS", new F("ThreeBlackCrows", "Three Black Crows", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "THREEINSIDE", new F("ThreeInside", "Three Inside Up/Down", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "THREELINESTRIKE", new F("ThreeLineStrike", "Three-Line Strike", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "THREEOUTSIDE", new F("ThreeOutside", "Three Outside Up/Down", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            {
                "THREESTARSINSOUTH",
                new F("ThreeStarsInSouth", "Three Stars In The South", prGroup, inputPriceOHLC, [], outputIntTypePattern)
            },
            {
                "THREEWHITESOLDIERS",
                new F("ThreeWhiteSoldiers", "Three Advancing White Soldiers", prGroup, inputPriceOHLC, [], outputIntTypePattern)
            },
            { "THRUSTING", new F("Thrusting", "Thrusting Pattern", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "TRISTAR", new F("Tristar", "Tristar Pattern", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "TWOCROWS", new F("TwoCrows", "Two Crows", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            { "UNIQUETHREERIVER", new F("UniqueThreeRiver", "Unique Three River", prGroup, inputPriceOHLC, [], outputIntTypePattern) },
            {
                "UPDOWNSIDEGAPTHREEMETHODS",
                new F("UpDownSideGapThreeMethods", "Upside/Downside Gap Three Methods", prGroup, inputPriceOHLC, [], outputIntTypePattern)
            },
            { "UPSIDEGAPTWOCROWS", new F("UpsideGapTwoCrows", "Upside Gap Two Crows", prGroup, inputPriceOHLC, [], outputIntTypePattern) }
        };

        FunctionsDefinition = new ReadOnlyDictionary<string, F>(funcDefs);
    }

    /// <summary>
    /// Provides a single point of access to the entire collection of functions supported by the library.
    /// This singleton instance ensures that all functions are readily available
    /// and can be accessed in a consistent manner throughout the application.
    /// </summary>
    /// <remarks>
    /// Additionally, the full power of LINQ can be leveraged to query the functions database,
    /// enabling advanced filtering, sorting, and projection capabilities.
    /// </remarks>
    public static Abstract All => Instance.Value;

    /// <summary>
    /// Finds and returns a function by its name, if it exists in the functions' database.
    /// </summary>
    /// <param name="name">The name of the function to find.</param>
    /// <returns>
    /// The <see cref="IndicatorFunction"/> corresponding to the specified name, or <c>null</c> if the function is not found.
    /// </returns>
    public static F? Function(string name) => All.FunctionsDefinition.GetValueOrDefault(name);

    /// <summary>
    /// Alias for the <see cref="Function"/> method.
    /// </summary>
    public F? this[string name] => Function(name);

    /// <inheritdoc/>
    [MustDisposeResource]
    public IEnumerator<F> GetEnumerator() => FunctionsDefinition.Values.GetEnumerator();

    [MustDisposeResource]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
