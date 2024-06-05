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

namespace TALib;

public class Abstract : IEnumerable<IndicatorFunction>
{
    private const string HighInput = "High";
    private const string LowInput = "Low";
    private const string CloseInput = "Close";
    private const string OpenInput = "Open";
    private const string VolumeInput = "Volume";
    private const string RealInput = "Real";

    private const string RealOutput = "Real";
    private const string IntegerOutput = "Integer";

    private const string MATypeOption = "MA Type";
    private const string TimePeriodOption = "Time Period";
    private const string PenetrationOption = "Penetration";

    internal static readonly Dictionary<string, IndicatorFunction> FunctionsDefinition = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Accbands", new IndicatorFunction("Accbands", "Acceleration Bands", "Overlap Studies", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, "Real Upper Band|Real Middle Band|Real Lower Band") },
        { "Acos", new IndicatorFunction("Acos", "Vector Trigonometric ACos", "Math Transform", RealInput, String.Empty, RealOutput) },
        { "Ad", new IndicatorFunction("Ad", "Chaikin A/D Line", "Volume Indicators", $"{HighInput}|{LowInput}|{CloseInput}|{VolumeInput}", String.Empty, RealOutput) },
        { "Add", new IndicatorFunction("Add", "Vector Arithmetic Add", "Math Operators", $"{RealInput}|{RealInput}", String.Empty, RealOutput) },
        { "AdOsc", new IndicatorFunction("AdOsc", "Chaikin A/D Oscillator", "Volume Indicators", $"{HighInput}|{LowInput}|{CloseInput}|{VolumeInput}", "Fast Period|Slow Period", RealOutput) },
        { "Adx", new IndicatorFunction("Adx", "Average Directional Movement Index", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
        { "Adxr", new IndicatorFunction("Adxr", "Average Directional Movement Index Rating", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
        { "Apo", new IndicatorFunction("Apo", "Absolute Price Oscillator", "Momentum Indicators", RealInput, $"Fast Period|Slow Period|{MATypeOption}", RealOutput) },
        { "Aroon", new IndicatorFunction("Aroon", "Aroon", "Momentum Indicators", $"{HighInput}|{LowInput}", TimePeriodOption, "Aroon Down|Aroon Up") },
        { "AroonOsc", new IndicatorFunction("AroonOsc", "Aroon Oscillator", "Momentum Indicators", $"{HighInput}|{LowInput}", TimePeriodOption, RealOutput) },
        { "Asin", new IndicatorFunction("Asin", "Vector Trigonometric ASin", "Math Transform", RealInput, String.Empty, RealOutput) },
        { "Atan", new IndicatorFunction("Atan", "Vector Trigonometric ATan", "Math Transform", RealInput, String.Empty, RealOutput) },
        { "Atr", new IndicatorFunction("Atr", "Average True Range", "Volatility Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
        { "AvgDev", new IndicatorFunction("AvgDev", "Average Deviation", "Price Transform", RealInput, TimePeriodOption, RealOutput) },
        { "AvgPrice", new IndicatorFunction("AvgPrice", "Average Price", "Price Transform", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, RealOutput) },
        { "Bbands", new IndicatorFunction("Bbands", "Bollinger Bands", "Overlap Studies", RealInput, $"{TimePeriodOption}|Nb Dev Up|Nb Dev Dn|{MATypeOption}", "Real Upper Band|Real Middle Band|Real Lower Band") },
        { "Beta", new IndicatorFunction("Beta", "Beta", "Statistic Functions", $"{RealInput}|{RealInput}", TimePeriodOption, RealOutput) },
        { "Bop", new IndicatorFunction("Bop", "Balance of Power", "Momentum Indicators", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, RealOutput) },
        { "Cci", new IndicatorFunction("Cci", "Commodity Channel Index", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
        { "Ceil", new IndicatorFunction("Ceil", "Vector Ceil", "Math Transform", RealInput, String.Empty, RealOutput) },
        { "Cmo", new IndicatorFunction("Cmo", "Chande Momentum Oscillator", "Momentum Indicators", RealInput, TimePeriodOption, RealOutput) },
        { "Correl", new IndicatorFunction("Correl", "Pearson's Correlation Coefficient (r)", "Statistic Functions", $"{RealInput}|{RealInput}", TimePeriodOption, RealOutput) },
        { "Cos", new IndicatorFunction("Cos", "Vector Trigonometric Cos", "Math Transform", RealInput, String.Empty, RealOutput) },
        { "Cosh", new IndicatorFunction("Cosh", "Vector Trigonometric Cosh", "Math Transform", RealInput, String.Empty, RealOutput) },
        { "Dema", new IndicatorFunction("Dema", "Double Exponential Moving Average", "Overlap Studies", RealInput, TimePeriodOption, RealOutput) },
        { "Div", new IndicatorFunction("Div", "Vector Arithmetic Div", "Math Operators", $"{RealInput}|{RealInput}", String.Empty, RealOutput) },
        { "Dx", new IndicatorFunction("Dx", "Directional Movement Index", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
        { "Ema", new IndicatorFunction("Ema", "Exponential Moving Average", "Overlap Studies", RealInput, TimePeriodOption, RealOutput) },
        { "Exp", new IndicatorFunction("Exp", "Vector Arithmetic Exp", "Math Transform", RealInput, String.Empty, RealOutput) },
        { "Floor", new IndicatorFunction("Floor", "Vector Floor", "Math Transform", RealInput, String.Empty, RealOutput) },
        { "HtDcPeriod", new IndicatorFunction("HtDcPeriod", "Hilbert Transform - Dominant Cycle Period", "Cycle Indicators", RealInput, String.Empty, RealOutput) },
        { "HtDcPhase", new IndicatorFunction("HtDcPhase", "Hilbert Transform - Dominant Cycle Phase", "Cycle Indicators", RealInput, String.Empty, RealOutput) },
        { "HtPhasor", new IndicatorFunction("HtPhasor", "Hilbert Transform - Phasor Components", "Cycle Indicators", RealInput, String.Empty, "In Phase|Quadrature") },
        { "HtSine", new IndicatorFunction("HtSine", "Hilbert Transform - SineWave", "Cycle Indicators", RealInput, String.Empty, "Sine|Lead Sine") },
        { "HtTrendline", new IndicatorFunction("HtTrendline", "Hilbert Transform - Instantaneous Trendline", "Overlap Studies", RealInput, String.Empty, RealOutput) },
        { "HtTrendMode", new IndicatorFunction("HtTrendMode", "Hilbert Transform - Trend vs Cycle Mode", "Cycle Indicators", RealInput, String.Empty, IntegerOutput) },
        { "Kama", new IndicatorFunction("Kama", "Kaufman Adaptive Moving Average", "Overlap Studies", RealInput, TimePeriodOption, RealOutput) },
        { "LinearReg", new IndicatorFunction("LinearReg", "Linear Regression", "Statistic Functions", RealInput, TimePeriodOption, RealOutput) },
        { "LinearRegAngle", new IndicatorFunction("LinearRegAngle", "Linear Regression Angle", "Statistic Functions", RealInput, TimePeriodOption, RealOutput) },
        { "LinearRegIntercept", new IndicatorFunction("LinearRegIntercept", "Linear Regression Intercept", "Statistic Functions", RealInput, TimePeriodOption, RealOutput) },
        { "LinearRegSlope", new IndicatorFunction("LinearRegSlope", "Linear Regression Slope", "Statistic Functions", RealInput, TimePeriodOption, RealOutput) },
        { "Ln", new IndicatorFunction("Ln", "Vector Log Natural", "Math Transform", RealInput, String.Empty, RealOutput) },
        { "Log10", new IndicatorFunction("Log10", "Vector Log10", "Math Transform", RealInput, String.Empty, RealOutput) },
        { "Ma", new IndicatorFunction("Ma", "Moving Average", "Overlap Studies", RealInput, $"{TimePeriodOption}|{MATypeOption}", RealOutput) },
        { "Macd", new IndicatorFunction("Macd", "Moving Average Convergence/Divergence", "Momentum Indicators", RealInput, "Fast Period|Slow Period|Signal Period", "MACD|MACD Signal|MACD Hist") },
        { "MacdExt", new IndicatorFunction("MacdExt", "MACD with controllable MA type", "Momentum Indicators", RealInput, $"Fast Period|Fast {MATypeOption}|Slow Period|Slow {MATypeOption}|Signal Period|Signal {MATypeOption}", "MACD|MACD Signal|MACD Hist") },
        { "MacdFix", new IndicatorFunction("MacdFix", "Moving Average Convergence/Divergence Fix 12/26", "Momentum Indicators", RealInput, "Signal Period", "MACD|MACD Signal|MACD Hist") },
        { "Mama", new IndicatorFunction("Mama", "MESA Adaptive Moving Average", "Overlap Studies", RealInput, "Fast Limit|Slow Limit", "MAMA|FAMA") },
        { "Mavp", new IndicatorFunction("Mavp", "Moving average with variable period", "Overlap Studies", $"{RealInput}|Periods", $"Min Period|Max Period|{MATypeOption}", RealOutput) },
        { "Max", new IndicatorFunction("Max", "Highest value over a specified period", "Math Operators", RealInput, TimePeriodOption, RealOutput) },
        { "MaxIndex", new IndicatorFunction("MaxIndex", "Index of highest value over a specified period", "Math Operators", RealInput, TimePeriodOption, IntegerOutput) },
        { "MedPrice", new IndicatorFunction("MedPrice", "Median Price", "Price Transform", $"{HighInput}|{LowInput}", String.Empty, RealOutput) },
        { "Mfi", new IndicatorFunction("Mfi", "Money Flow Index", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}|{VolumeInput}", TimePeriodOption, RealOutput) },
        { "MidPoint", new IndicatorFunction("MidPoint", "MidPoint over period", "Overlap Studies", RealInput, TimePeriodOption, RealOutput) },
        { "MidPrice", new IndicatorFunction("MidPrice", "Midpoint Price over period", "Overlap Studies", $"{HighInput}|{LowInput}", TimePeriodOption, RealOutput) },
        { "Min", new IndicatorFunction("Min", "Lowest value over a specified period", "Math Operators", RealInput, TimePeriodOption, RealOutput) },
        { "MinIndex", new IndicatorFunction("MinIndex", "Index of lowest value over a specified period", "Math Operators", RealInput, TimePeriodOption, IntegerOutput) },
        { "MinMax", new IndicatorFunction("MinMax", "Lowest and highest values over a specified period", "Math Operators", RealInput, TimePeriodOption, "Min|Max") },
        { "MinMaxIndex", new IndicatorFunction("MinMaxIndex", "Indexes of lowest and highest values over a specified period", "Math Operators", RealInput, TimePeriodOption, "Min Idx|Max Idx") },
        { "MinusDI", new IndicatorFunction("MinusDI", "Minus Directional Indicator", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
        { "MinusDM", new IndicatorFunction("MinusDM", "Minus Directional Movement", "Momentum Indicators", $"{HighInput}|{LowInput}", TimePeriodOption, RealOutput) },
        { "Mom", new IndicatorFunction("Mom", "Momentum", "Momentum Indicators", RealInput, TimePeriodOption, RealOutput) },
        { "Mult", new IndicatorFunction("Mult", "Vector Arithmetic Mult", "Math Operators", $"{RealInput}|{RealInput}", String.Empty, RealOutput) },
        { "Natr", new IndicatorFunction("Natr", "Normalized Average True Range", "Volatility Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
        { "Obv", new IndicatorFunction("Obv", "On Balance Volume", "Volume Indicators", $"{RealInput}|{VolumeInput}", String.Empty, RealOutput) },
        { "PlusDI", new IndicatorFunction("PlusDI", "Plus Directional Indicator", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
        { "PlusDM", new IndicatorFunction("PlusDM", "Plus Directional Movement", "Momentum Indicators", $"{HighInput}|{LowInput}", TimePeriodOption, RealOutput) },
        { "Ppo", new IndicatorFunction("Ppo", "Percentage Price Oscillator", "Momentum Indicators", RealInput, $"Fast Period|Slow Period|{MATypeOption}", RealOutput) },
        { "Roc", new IndicatorFunction("Roc", "Rate of change : ((price/prevPrice)-1)*100", "Momentum Indicators", RealInput, TimePeriodOption, RealOutput) },
        { "RocP", new IndicatorFunction("RocP", "Rate of change Percentage: (price-prevPrice)/prevPrice", "Momentum Indicators", RealInput, TimePeriodOption, RealOutput) },
        { "RocR", new IndicatorFunction("RocR", "Rate of change ratio: (price/prevPrice)", "Momentum Indicators", RealInput, TimePeriodOption, RealOutput) },
        { "RocR100", new IndicatorFunction("RocR100", "Rate of change ratio 100 scale: (price/prevPrice)*100", "Momentum Indicators", RealInput, TimePeriodOption, RealOutput) },
        { "Rsi", new IndicatorFunction("Rsi", "Relative Strength Index", "Momentum Indicators", RealInput, TimePeriodOption, RealOutput) },
        { "Sar", new IndicatorFunction("Sar", "Parabolic SAR", "Overlap Studies", $"{HighInput}|{LowInput}", "Acceleration|Maximum", RealOutput) },
        { "SarExt", new IndicatorFunction("SarExt", "Parabolic SAR - Extended", "Overlap Studies", $"{HighInput}|{LowInput}", "Start Value|Offset On Reverse|Acceleration Init Long|Acceleration Long|Acceleration Max Long|Acceleration Init Short|Acceleration Short|Acceleration Max Short", RealOutput) },
        { "Sin", new IndicatorFunction("Sin", "Vector Trigonometric Sin", "Math Transform", RealInput, String.Empty, RealOutput) },
        { "Sinh", new IndicatorFunction("Sinh", "Vector Trigonometric Sinh", "Math Transform", RealInput, String.Empty, RealOutput) },
        { "Sma", new IndicatorFunction("Sma", "Simple Moving Average", "Overlap Studies", RealInput, TimePeriodOption, RealOutput) },
        { "Sqrt", new IndicatorFunction("Sqrt", "Vector Square Root", "Math Transform", RealInput, String.Empty, RealOutput) },
        { "StdDev", new IndicatorFunction("StdDev", "Standard Deviation", "Statistic Functions", RealInput, $"{TimePeriodOption}|Nb Dev", RealOutput) },
        { "Stoch", new IndicatorFunction("Stoch", "Stochastic", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", $"Fast K Period|Slow K Period|Slow K {MATypeOption}|Slow D Period|Slow D {MATypeOption}", "Slow K|Slow D") },
        { "StochF", new IndicatorFunction("StochF", "Stochastic Fast", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", $"Fast K Period|Fast D Period|Fast D {MATypeOption}", "Fast K|Fast D") },
        { "StochRsi", new IndicatorFunction("StochRsi", "Stochastic Relative Strength Index", "Momentum Indicators", RealInput, $"{TimePeriodOption}|Fast K Period|Fast D Period|Fast D {MATypeOption}", "Fast K|Fast D") },
        { "Sub", new IndicatorFunction("Sub", "Vector Arithmetic Subtraction", "Math Operators", $"{RealInput}|{RealInput}", String.Empty, RealOutput) },
        { "Sum", new IndicatorFunction("Sum", "Summation", "Math Operators", RealInput, TimePeriodOption, RealOutput) },
        { "T3", new IndicatorFunction("T3", "Triple Exponential Moving Average (T3)", "Overlap Studies", RealInput, $"{TimePeriodOption}|V Factor", RealOutput) },
        { "Tan", new IndicatorFunction("Tan", "Vector Trigonometric Tan", "Math Transform", RealInput, String.Empty, RealOutput) },
        { "Tanh", new IndicatorFunction("Tanh", "Vector Trigonometric Tanh", "Math Transform", RealInput, String.Empty, RealOutput) },
        { "Tema", new IndicatorFunction("Tema", "Triple Exponential Moving Average", "Overlap Studies", RealInput, TimePeriodOption, RealOutput) },
        { "TRange", new IndicatorFunction("TRange", "True Range", "Volatility Indicators", $"{HighInput}|{LowInput}|{CloseInput}", String.Empty, RealOutput) },
        { "Trima", new IndicatorFunction("Trima", "Triangular Moving Average", "Overlap Studies", RealInput, TimePeriodOption, RealOutput) },
        { "Trix", new IndicatorFunction("Trix", "1-day Rate-Of-Change (ROC) of a Triple Smooth EMA", "Momentum Indicators", RealInput, TimePeriodOption, RealOutput) },
        { "Tsf", new IndicatorFunction("Tsf", "Time Series Forecast", "Statistic Functions", RealInput, TimePeriodOption, RealOutput) },
        { "TypPrice", new IndicatorFunction("TypPrice", "Typical Price", "Price Transform", $"{HighInput}|{LowInput}|{CloseInput}", String.Empty, RealOutput) },
        { "UltOsc", new IndicatorFunction("UltOsc", "Ultimate Oscillator", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", $"{TimePeriodOption} 1|{TimePeriodOption} 2|{TimePeriodOption} 3", RealOutput) },
        { "Var", new IndicatorFunction("Var", "Variance", "Statistic Functions", RealInput, TimePeriodOption, RealOutput) },
        { "WclPrice", new IndicatorFunction("WclPrice", "Weighted Close Price", "Price Transform", $"{HighInput}|{LowInput}|{CloseInput}", String.Empty, RealOutput) },
        { "WillR", new IndicatorFunction("WillR", "Williams' %R", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
        { "Wma", new IndicatorFunction("Wma", "Weighted Moving Average", "Overlap Studies", RealInput, TimePeriodOption, RealOutput) },

        { "AbandonedBaby", new IndicatorFunction("AbandonedBaby", "Abandoned Baby", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", PenetrationOption, IntegerOutput) },
        { "AdvanceBlock", new IndicatorFunction("AdvanceBlock", "Advance Block", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "BeltHold", new IndicatorFunction("BeltHold", "Belt-hold", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "Breakaway", new IndicatorFunction("Breakaway", "Breakaway", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "ClosingMarubozu", new IndicatorFunction("ClosingMarubozu", "Closing Marubozu", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "ConcealingBabySwallow", new IndicatorFunction("ConcealingBabySwallow", "Concealing Baby Swallow", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "CounterAttack", new IndicatorFunction("CounterAttack", "Counterattack", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "DarkCloudCover", new IndicatorFunction("DarkCloudCover", "Dark Cloud Cover", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", PenetrationOption, IntegerOutput) },
        { "Doji", new IndicatorFunction("Doji", "Doji", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "DojiStar", new IndicatorFunction("DojiStar", "Doji Star", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "DragonflyDoji", new IndicatorFunction("DragonflyDoji", "Dragonfly Doji", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "Engulfing", new IndicatorFunction("Engulfing", "Engulfing Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "EveningDojiStar", new IndicatorFunction("EveningDojiStar", "Evening Doji Star", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", PenetrationOption, IntegerOutput) },
        { "EveningStar", new IndicatorFunction("EveningStar", "Evening Star", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", PenetrationOption, IntegerOutput) },
        { "GapSideBySideWhiteLines", new IndicatorFunction("GapSideBySideWhiteLines", "Up/Down-gap side-by-side white lines", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "GravestoneDoji", new IndicatorFunction("GravestoneDoji", "Gravestone Doji", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "Hammer", new IndicatorFunction("Hammer", "Hammer", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "HangingMan", new IndicatorFunction("HangingMan", "Hanging Man", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "Harami", new IndicatorFunction("Harami", "Harami Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "HaramiCross", new IndicatorFunction("HaramiCross", "Harami Cross Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "HighWave", new IndicatorFunction("HighWave", "High-Wave Candle", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "Hikkake", new IndicatorFunction("Hikkake", "Hikkake Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "HikkakeModified", new IndicatorFunction("HikkakeModified", "Modified Hikkake Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "HomingPigeon", new IndicatorFunction("HomingPigeon", "Homing Pigeon", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "IdenticalThreeCrows", new IndicatorFunction("IdenticalThreeCrows", "Identical Three Crows", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "InNeck", new IndicatorFunction("InNeck", "In-Neck Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "InvertedHammer", new IndicatorFunction("InvertedHammer", "Inverted Hammer", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|Close", String.Empty, IntegerOutput) },
        { "Kicking", new IndicatorFunction("Kicking", "Kicking", "Pattern Recognition", $"Open|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "KickingByLength", new IndicatorFunction("KickingByLength", "Kicking - bull/bear determined by the longer marubozu", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "LadderBottom", new IndicatorFunction("LadderBottom", "Ladder Bottom", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "LongLeggedDoji", new IndicatorFunction("LongLeggedDoji", "Long Legged Doji", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "LongLine", new IndicatorFunction("LongLine", "Long Line Candle", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "Marubozu", new IndicatorFunction("Marubozu", "Marubozu", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "MatchingLow", new IndicatorFunction("MatchingLow", "Matching Low", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "MatHold", new IndicatorFunction("MatHold", "Mat Hold", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", PenetrationOption, IntegerOutput) },
        { "MorningDojiStar", new IndicatorFunction("MorningDojiStar", "Morning Doji Star", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", PenetrationOption, IntegerOutput) },
        { "MorningStar", new IndicatorFunction("MorningStar", "Morning Star", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", PenetrationOption, IntegerOutput) },
        { "OnNeck", new IndicatorFunction("OnNeck", "On-Neck Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "Piercing", new IndicatorFunction("Piercing", "Piercing Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "RickshawMan", new IndicatorFunction("RickshawMan", "Rickshaw Man", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "RisingFallingThreeMethods", new IndicatorFunction("RisingFallingThreeMethods", "Rising/Falling Three Methods", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "SeparatingLines", new IndicatorFunction("SeparatingLines", "Separating Lines", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "ShootingStar", new IndicatorFunction("ShootingStar", "Shooting Star", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "ShortLine", new IndicatorFunction("ShortLine", "Short Line Candle", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "SpinningTop", new IndicatorFunction("SpinningTop", "Spinning Top", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "StalledPattern", new IndicatorFunction("StalledPattern", "Stalled Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "StickSandwich", new IndicatorFunction("StickSandwich", "Stick Sandwich", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "Takuri", new IndicatorFunction("Takuri", "Takuri Line (Dragonfly Doji with very long lower shadow)", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "TasukiGap", new IndicatorFunction("TasukiGap", "Tasuki Gap", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "ThreeBlackCrows", new IndicatorFunction("ThreeBlackCrows", "Three Black Crows", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "ThreeInside", new IndicatorFunction("ThreeInside", "Three Inside Up/Down", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "ThreeLineStrike", new IndicatorFunction("ThreeLineStrike", "Three-Line Strike", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "ThreeOutside", new IndicatorFunction("ThreeOutside", "Three Outside Up/Down", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "ThreeStarsInSouth", new IndicatorFunction("ThreeStarsInSouth", "Three Stars In The South", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "ThreeWhiteSoldiers", new IndicatorFunction("ThreeWhiteSoldiers", "Three Advancing White Soldiers", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "Thrusting", new IndicatorFunction("Thrusting", "Thrusting Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "Tristar", new IndicatorFunction("Tristar", "Tristar Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "TwoCrows", new IndicatorFunction("TwoCrows", "Two Crows", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "UniqueThreeRiver", new IndicatorFunction("UniqueThreeRiver", "Unique Three River", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "UpDownSideGapThreeMethods", new IndicatorFunction("UpDownSideGapThreeMethods", "Upside/Downside Gap Three Methods", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
        { "UpsideGapTwoCrows", new IndicatorFunction("UpsideGapTwoCrows", "Upside Gap Two Crows", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) }
    };

    private static readonly Lazy<Abstract> Instance = new(() => []);

    private Abstract()
    {
    }

    public static Abstract All => Instance.Value;

    public static IndicatorFunction Find(string name) =>
        FunctionsDefinition.TryGetValue(name, out var function)
            ? function
            : throw new ArgumentException("Function not found in the library", name);

    public IEnumerator<IndicatorFunction> GetEnumerator() => FunctionsDefinition.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
