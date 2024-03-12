using System.Collections;
using System.Collections.Generic;

namespace TALib
{
    public class Functions : IEnumerable<Function>
    {
        private const string HighInput = "High";
        private const string LowInput = "Low";
        private const string CloseInput = "Close";
        private const string OpenInput = "Open";
        private const string VolumeInput = "Volume";

        private const string RealOutput = "Real";
        private const string IntegerOutput = "Integer";

        private const string MATypeOption = "MA Type";
        private const string TimePeriodOption = "Time Period";
        private const string PenetrationOption = "Penetration";

        internal static readonly Dictionary<string, Function> FunctionsDefinition = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Accbands", new Function("Accbands", "Acceleration Bands", "Overlap Studies", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, "Real Upper Band|Real Middle Band|Real Lower Band") },
            { "Acos", new Function("Acos", "Vector Trigonometric ACos", "Math Transform", RealOutput, String.Empty, RealOutput) },
            { "Ad", new Function("Ad", "Chaikin A/D Line", "Volume Indicators", $"{HighInput}|{LowInput}|{CloseInput}|{VolumeInput}", String.Empty, RealOutput) },
            { "Add", new Function("Add", "Vector Arithmetic Add", "Math Operators", $"{RealOutput}|{RealOutput}", String.Empty, RealOutput) },
            { "AdOsc", new Function("AdOsc", "Chaikin A/D Oscillator", "Volume Indicators", $"{HighInput}|{LowInput}|{CloseInput}|{VolumeInput}", "Fast Period|Slow Period", RealOutput) },
            { "Adx", new Function("Adx", "Average Directional Movement Index", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
            { "Adxr", new Function("Adxr", "Average Directional Movement Index Rating", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
            { "Apo", new Function("Apo", "Absolute Price Oscillator", "Momentum Indicators", RealOutput, $"Fast Period|Slow Period|{MATypeOption}", RealOutput) },
            { "Aroon", new Function("Aroon", "Aroon", "Momentum Indicators", $"{HighInput}|{LowInput}", TimePeriodOption, "Aroon Down|Aroon Up") },
            { "AroonOsc", new Function("AroonOsc", "Aroon Oscillator", "Momentum Indicators", $"{HighInput}|{LowInput}", TimePeriodOption, RealOutput) },
            { "Asin", new Function("Asin", "Vector Trigonometric ASin", "Math Transform", RealOutput, String.Empty, RealOutput) },
            { "Atan", new Function("Atan", "Vector Trigonometric ATan", "Math Transform", RealOutput, String.Empty, RealOutput) },
            { "Atr", new Function("Atr", "Average True Range", "Volatility Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
            { "AvgDev", new Function("AvgDev", "Average Deviation", "Price Transform", RealOutput, TimePeriodOption, RealOutput) },
            { "AvgPrice", new Function("AvgPrice", "Average Price", "Price Transform", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, RealOutput) },
            { "Bbands", new Function("Bbands", "Bollinger Bands", "Overlap Studies", RealOutput, $"{TimePeriodOption}|Nb Dev Up|Nb Dev Dn|{MATypeOption}", "Real Upper Band|Real Middle Band|Real Lower Band") },
            { "Beta", new Function("Beta", "Beta", "Statistic Functions", $"{RealOutput}|{RealOutput}", TimePeriodOption, RealOutput) },
            { "Bop", new Function("Bop", "Balance of Power", "Momentum Indicators", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, RealOutput) },
            { "Cci", new Function("Cci", "Commodity Channel Index", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
            { "Ceil", new Function("Ceil", "Vector Ceil", "Math Transform", RealOutput, String.Empty, RealOutput) },
            { "Cmo", new Function("Cmo", "Chande Momentum Oscillator", "Momentum Indicators", RealOutput, TimePeriodOption, RealOutput) },
            { "Correl", new Function("Correl", "Pearson's Correlation Coefficient (r)", "Statistic Functions", $"{RealOutput}|{RealOutput}", TimePeriodOption, RealOutput) },
            { "Cos", new Function("Cos", "Vector Trigonometric Cos", "Math Transform", RealOutput, String.Empty, RealOutput) },
            { "Cosh", new Function("Cosh", "Vector Trigonometric Cosh", "Math Transform", RealOutput, String.Empty, RealOutput) },
            { "Dema", new Function("Dema", "Double Exponential Moving Average", "Overlap Studies", RealOutput, TimePeriodOption, RealOutput) },
            { "Div", new Function("Div", "Vector Arithmetic Div", "Math Operators", $"{RealOutput}|{RealOutput}", String.Empty, RealOutput) },
            { "Dx", new Function("Dx", "Directional Movement Index", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
            { "Ema", new Function("Ema", "Exponential Moving Average", "Overlap Studies", RealOutput, TimePeriodOption, RealOutput) },
            { "Exp", new Function("Exp", "Vector Arithmetic Exp", "Math Transform", RealOutput, String.Empty, RealOutput) },
            { "Floor", new Function("Floor", "Vector Floor", "Math Transform", RealOutput, String.Empty, RealOutput) },
            { "HtDcPeriod", new Function("HtDcPeriod", "Hilbert Transform - Dominant Cycle Period", "Cycle Indicators", RealOutput, String.Empty, RealOutput) },
            { "HtDcPhase", new Function("HtDcPhase", "Hilbert Transform - Dominant Cycle Phase", "Cycle Indicators", RealOutput, String.Empty, RealOutput) },
            { "HtPhasor", new Function("HtPhasor", "Hilbert Transform - Phasor Components", "Cycle Indicators", RealOutput, String.Empty, "In Phase|Quadrature") },
            { "HtSine", new Function("HtSine", "Hilbert Transform - SineWave", "Cycle Indicators", RealOutput, String.Empty, "Sine|Lead Sine") },
            { "HtTrendline", new Function("HtTrendline", "Hilbert Transform - Instantaneous Trendline", "Overlap Studies", RealOutput, String.Empty, RealOutput) },
            { "HtTrendMode", new Function("HtTrendMode", "Hilbert Transform - Trend vs Cycle Mode", "Cycle Indicators", RealOutput, String.Empty, IntegerOutput) },
            { "Kama", new Function("Kama", "Kaufman Adaptive Moving Average", "Overlap Studies", RealOutput, TimePeriodOption, RealOutput) },
            { "LinearReg", new Function("LinearReg", "Linear Regression", "Statistic Functions", RealOutput, TimePeriodOption, RealOutput) },
            { "LinearRegAngle", new Function("LinearRegAngle", "Linear Regression Angle", "Statistic Functions", RealOutput, TimePeriodOption, RealOutput) },
            { "LinearRegIntercept", new Function("LinearRegIntercept", "Linear Regression Intercept", "Statistic Functions", RealOutput, TimePeriodOption, RealOutput) },
            { "LinearRegSlope", new Function("LinearRegSlope", "Linear Regression Slope", "Statistic Functions", RealOutput, TimePeriodOption, RealOutput) },
            { "Ln", new Function("Ln", "Vector Log Natural", "Math Transform", RealOutput, String.Empty, RealOutput) },
            { "Log10", new Function("Log10", "Vector Log10", "Math Transform", RealOutput, String.Empty, RealOutput) },
            { "Ma", new Function("Ma", "Moving Average", "Overlap Studies", RealOutput, $"{TimePeriodOption}|{MATypeOption}", RealOutput) },
            { "Macd", new Function("Macd", "Moving Average Convergence/Divergence", "Momentum Indicators", RealOutput, "Fast Period|Slow Period|Signal Period", "MACD|MACD Signal|MACD Hist") },
            { "MacdExt", new Function("MacdExt", "MACD with controllable MA type", "Momentum Indicators", RealOutput, $"Fast Period|Fast {MATypeOption}|Slow Period|Slow {MATypeOption}|Signal Period|Signal {MATypeOption}", "MACD|MACD Signal|MACD Hist") },
            { "MacdFix", new Function("MacdFix", "Moving Average Convergence/Divergence Fix 12/26", "Momentum Indicators", RealOutput, "Signal Period", "MACD|MACD Signal|MACD Hist") },
            { "Mama", new Function("Mama", "MESA Adaptive Moving Average", "Overlap Studies", RealOutput, "Fast Limit|Slow Limit", "MAMA|FAMA") },
            { "Mavp", new Function("Mavp", "Moving average with variable period", "Overlap Studies", $"{RealOutput}|Periods", $"Min Period|Max Period|{MATypeOption}", RealOutput) },
            { "Max", new Function("Max", "Highest value over a specified period", "Math Operators", RealOutput, TimePeriodOption, RealOutput) },
            { "MaxIndex", new Function("MaxIndex", "Index of highest value over a specified period", "Math Operators", RealOutput, TimePeriodOption, IntegerOutput) },
            { "MedPrice", new Function("MedPrice", "Median Price", "Price Transform", $"{HighInput}|{LowInput}", String.Empty, RealOutput) },
            { "Mfi", new Function("Mfi", "Money Flow Index", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}|{VolumeInput}", TimePeriodOption, RealOutput) },
            { "MidPoint", new Function("MidPoint", "MidPoint over period", "Overlap Studies", RealOutput, TimePeriodOption, RealOutput) },
            { "MidPrice", new Function("MidPrice", "Midpoint Price over period", "Overlap Studies", $"{HighInput}|{LowInput}", TimePeriodOption, RealOutput) },
            { "Min", new Function("Min", "Lowest value over a specified period", "Math Operators", RealOutput, TimePeriodOption, RealOutput) },
            { "MinIndex", new Function("MinIndex", "Index of lowest value over a specified period", "Math Operators", RealOutput, TimePeriodOption, IntegerOutput) },
            { "MinMax", new Function("MinMax", "Lowest and highest values over a specified period", "Math Operators", RealOutput, TimePeriodOption, "Min|Max") },
            { "MinMaxIndex", new Function("MinMaxIndex", "Indexes of lowest and highest values over a specified period", "Math Operators", RealOutput, TimePeriodOption, "Min Idx|Max Idx") },
            { "MinusDI", new Function("MinusDI", "Minus Directional Indicator", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
            { "MinusDM", new Function("MinusDM", "Minus Directional Movement", "Momentum Indicators", $"{HighInput}|{LowInput}", TimePeriodOption, RealOutput) },
            { "Mom", new Function("Mom", "Momentum", "Momentum Indicators", RealOutput, TimePeriodOption, RealOutput) },
            { "Mult", new Function("Mult", "Vector Arithmetic Mult", "Math Operators", $"{RealOutput}|{RealOutput}", String.Empty, RealOutput) },
            { "Natr", new Function("Natr", "Normalized Average True Range", "Volatility Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
            { "Obv", new Function("Obv", "On Balance Volume", "Volume Indicators", $"{RealOutput}|{VolumeInput}", String.Empty, RealOutput) },
            { "PlusDI", new Function("PlusDI", "Plus Directional Indicator", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
            { "PlusDM", new Function("PlusDM", "Plus Directional Movement", "Momentum Indicators", $"{HighInput}|{LowInput}", TimePeriodOption, RealOutput) },
            { "Ppo", new Function("Ppo", "Percentage Price Oscillator", "Momentum Indicators", RealOutput, $"Fast Period|Slow Period|{MATypeOption}", RealOutput) },
            { "Roc", new Function("Roc", "Rate of change : ((price/prevPrice)-1)*100", "Momentum Indicators", RealOutput, TimePeriodOption, RealOutput) },
            { "RocP", new Function("RocP", "Rate of change Percentage: (price-prevPrice)/prevPrice", "Momentum Indicators", RealOutput, TimePeriodOption, RealOutput) },
            { "RocR", new Function("RocR", "Rate of change ratio: (price/prevPrice)", "Momentum Indicators", RealOutput, TimePeriodOption, RealOutput) },
            { "RocR100", new Function("RocR100", "Rate of change ratio 100 scale: (price/prevPrice)*100", "Momentum Indicators", RealOutput, TimePeriodOption, RealOutput) },
            { "Rsi", new Function("Rsi", "Relative Strength Index", "Momentum Indicators", RealOutput, TimePeriodOption, RealOutput) },
            { "Sar", new Function("Sar", "Parabolic SAR", "Overlap Studies", $"{HighInput}|{LowInput}", "Acceleration|Maximum", RealOutput) },
            { "SarExt", new Function("SarExt", "Parabolic SAR - Extended", "Overlap Studies", $"{HighInput}|{LowInput}", "Start Value|Offset On Reverse|Acceleration Init Long|Acceleration Long|Acceleration Max Long|Acceleration Init Short|Acceleration Short|Acceleration Max Short", RealOutput) },
            { "Sin", new Function("Sin", "Vector Trigonometric Sin", "Math Transform", RealOutput, String.Empty, RealOutput) },
            { "Sinh", new Function("Sinh", "Vector Trigonometric Sinh", "Math Transform", RealOutput, String.Empty, RealOutput) },
            { "Sma", new Function("Sma", "Simple Moving Average", "Overlap Studies", RealOutput, TimePeriodOption, RealOutput) },
            { "Sqrt", new Function("Sqrt", "Vector Square Root", "Math Transform", RealOutput, String.Empty, RealOutput) },
            { "StdDev", new Function("StdDev", "Standard Deviation", "Statistic Functions", RealOutput, $"{TimePeriodOption}|Nb Dev", RealOutput) },
            { "Stoch", new Function("Stoch", "Stochastic", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", $"Fast K Period|Slow K Period|Slow K {MATypeOption}|Slow D Period|Slow D {MATypeOption}", "Slow K|Slow D") },
            { "StochF", new Function("StochF", "Stochastic Fast", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", $"Fast K Period|Fast D Period|Fast D {MATypeOption}", "Fast K|Fast D") },
            { "StochRsi", new Function("StochRsi", "Stochastic Relative Strength Index", "Momentum Indicators", RealOutput, $"{TimePeriodOption}|Fast K Period|Fast D Period|Fast D {MATypeOption}", "Fast K|Fast D") },
            { "Sub", new Function("Sub", "Vector Arithmetic Subtraction", "Math Operators", $"{RealOutput}|{RealOutput}", String.Empty, RealOutput) },
            { "Sum", new Function("Sum", "Summation", "Math Operators", RealOutput, TimePeriodOption, RealOutput) },
            { "T3", new Function("T3", "Triple Exponential Moving Average (T3)", "Overlap Studies", RealOutput, $"{TimePeriodOption}|V Factor", RealOutput) },
            { "Tan", new Function("Tan", "Vector Trigonometric Tan", "Math Transform", RealOutput, String.Empty, RealOutput) },
            { "Tanh", new Function("Tanh", "Vector Trigonometric Tanh", "Math Transform", RealOutput, String.Empty, RealOutput) },
            { "Tema", new Function("Tema", "Triple Exponential Moving Average", "Overlap Studies", RealOutput, TimePeriodOption, RealOutput) },
            { "TRange", new Function("TRange", "True Range", "Volatility Indicators", $"{HighInput}|{LowInput}|{CloseInput}", String.Empty, RealOutput) },
            { "Trima", new Function("Trima", "Triangular Moving Average", "Overlap Studies", RealOutput, TimePeriodOption, RealOutput) },
            { "Trix", new Function("Trix", "1-day Rate-Of-Change (ROC) of a Triple Smooth EMA", "Momentum Indicators", RealOutput, TimePeriodOption, RealOutput) },
            { "Tsf", new Function("Tsf", "Time Series Forecast", "Statistic Functions", RealOutput, TimePeriodOption, RealOutput) },
            { "TypPrice", new Function("TypPrice", "Typical Price", "Price Transform", $"{HighInput}|{LowInput}|{CloseInput}", String.Empty, RealOutput) },
            { "UltOsc", new Function("UltOsc", "Ultimate Oscillator", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", $"{TimePeriodOption} 1|{TimePeriodOption} 2|{TimePeriodOption} 3", RealOutput) },
            { "Var", new Function("Var", "Variance", "Statistic Functions", RealOutput, TimePeriodOption, RealOutput) },
            { "WclPrice", new Function("WclPrice", "Weighted Close Price", "Price Transform", $"{HighInput}|{LowInput}|{CloseInput}", String.Empty, RealOutput) },
            { "WillR", new Function("WillR", "Williams' %R", "Momentum Indicators", $"{HighInput}|{LowInput}|{CloseInput}", TimePeriodOption, RealOutput) },
            { "Wma", new Function("Wma", "Weighted Moving Average", "Overlap Studies", RealOutput, TimePeriodOption, RealOutput) },

            { "Cdl2Crows", new Function("Cdl2Crows", "Two Crows", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "Cdl3BlackCrows", new Function("Cdl3BlackCrows", "Three Black Crows", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "Cdl3Inside", new Function("Cdl3Inside", "Three Inside Up/Down", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "Cdl3LineStrike", new Function("Cdl3LineStrike", "Three-Line Strike", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "Cdl3Outside", new Function("Cdl3Outside", "Three Outside Up/Down", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "Cdl3StarsInSouth", new Function("Cdl3StarsInSouth", "Three Stars In The South", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "Cdl3WhiteSoldiers", new Function("Cdl3WhiteSoldiers", "Three Advancing White Soldiers", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlAbandonedBaby", new Function("CdlAbandonedBaby", "Abandoned Baby", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", PenetrationOption, IntegerOutput) },
            { "CdlAdvanceBlock", new Function("CdlAdvanceBlock", "Advance Block", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlBeltHold", new Function("CdlBeltHold", "Belt-hold", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlBreakaway", new Function("CdlBreakaway", "Breakaway", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlClosingMarubozu", new Function("CdlClosingMarubozu", "Closing Marubozu", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlConcealBabysWall", new Function("CdlConcealBabysWall", "Concealing Baby Swallow", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlCounterAttack", new Function("CdlCounterAttack", "Counterattack", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlDarkCloudCover", new Function("CdlDarkCloudCover", "Dark Cloud Cover", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", PenetrationOption, IntegerOutput) },
            { "CdlDoji", new Function("CdlDoji", "Doji", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlDojiStar", new Function("CdlDojiStar", "Doji Star", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlDragonflyDoji", new Function("CdlDragonflyDoji", "Dragonfly Doji", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlEngulfing", new Function("CdlEngulfing", "Engulfing Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlEveningDojiStar", new Function("CdlEveningDojiStar", "Evening Doji Star", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", PenetrationOption, IntegerOutput) },
            { "CdlEveningStar", new Function("CdlEveningStar", "Evening Star", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", PenetrationOption, IntegerOutput) },
            { "CdlGapSideSideWhite", new Function("CdlGapSideSideWhite", "Up/Down-gap side-by-side white lines", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlGravestoneDoji", new Function("CdlGravestoneDoji", "Gravestone Doji", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlHammer", new Function("CdlHammer", "Hammer", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlHangingMan", new Function("CdlHangingMan", "Hanging Man", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlHarami", new Function("CdlHarami", "Harami Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlHaramiCross", new Function("CdlHaramiCross", "Harami Cross Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlHighWave", new Function("CdlHighWave", "High-Wave Candle", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlHikkake", new Function("CdlHikkake", "Hikkake Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlHikkakeMod", new Function("CdlHikkakeMod", "Modified Hikkake Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlHomingPigeon", new Function("CdlHomingPigeon", "Homing Pigeon", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlIdentical3Crows", new Function("CdlIdentical3Crows", "Identical Three Crows", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlInNeck", new Function("CdlInNeck", "In-Neck Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlInvertedHammer", new Function("CdlInvertedHammer", "Inverted Hammer", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|Close", String.Empty, IntegerOutput) },
            { "CdlKicking", new Function("CdlKicking", "Kicking", "Pattern Recognition", $"Open|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlKickingByLength", new Function("CdlKickingByLength", "Kicking - bull/bear determined by the longer marubozu", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlLadderBottom", new Function("CdlLadderBottom", "Ladder Bottom", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlLongLeggedDoji", new Function("CdlLongLeggedDoji", "Long Legged Doji", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlLongLine", new Function("CdlLongLine", "Long Line Candle", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlMarubozu", new Function("CdlMarubozu", "Marubozu", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlMatchingLow", new Function("CdlMatchingLow", "Matching Low", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlMatHold", new Function("CdlMatHold", "Mat Hold", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", PenetrationOption, IntegerOutput) },
            { "CdlMorningDojiStar", new Function("CdlMorningDojiStar", "Morning Doji Star", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", PenetrationOption, IntegerOutput) },
            { "CdlMorningStar", new Function("CdlMorningStar", "Morning Star", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", PenetrationOption, IntegerOutput) },
            { "CdlOnNeck", new Function("CdlOnNeck", "On-Neck Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlPiercing", new Function("CdlPiercing", "Piercing Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlRickshawMan", new Function("CdlRickshawMan", "Rickshaw Man", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlRiseFall3Methods", new Function("CdlRiseFall3Methods", "Rising/Falling Three Methods", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlSeparatingLines", new Function("CdlSeparatingLines", "Separating Lines", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlShootingStar", new Function("CdlShootingStar", "Shooting Star", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlShortLine", new Function("CdlShortLine", "Short Line Candle", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlSpinningTop", new Function("CdlSpinningTop", "Spinning Top", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlStalledPattern", new Function("CdlStalledPattern", "Stalled Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlStickSandwich", new Function("CdlStickSandwich", "Stick Sandwich", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlTakuri", new Function("CdlTakuri", "Takuri (Dragonfly Doji with very long lower shadow)", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlTasukiGap", new Function("CdlTasukiGap", "Tasuki Gap", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlThrusting", new Function("CdlThrusting", "Thrusting Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlTristar", new Function("CdlTristar", "Tristar Pattern", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlUnique3River", new Function("CdlUnique3River", "Unique 3 River", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlUpsideGap2Crows", new Function("CdlUpsideGap2Crows", "Upside Gap Two Crows", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) },
            { "CdlXSideGap3Methods", new Function("CdlXSideGap3Methods", "Upside/Downside Gap Three Methods", "Pattern Recognition", $"{OpenInput}|{HighInput}|{LowInput}|{CloseInput}", String.Empty, IntegerOutput) }
        };

        public static Function Find(string name) =>
            FunctionsDefinition.TryGetValue(name, out var function)
                ? function
                : throw new ArgumentException("Function not found", name);

        public IEnumerator<Function> GetEnumerator() => FunctionsDefinition.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
