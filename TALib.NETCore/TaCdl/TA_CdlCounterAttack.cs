using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlCounterAttack(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num15;
            double num20;
            int num50;
            var bodyLongPeriodTotal = new double[2];
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlCounterAttackLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            double equalPeriodTotal = default;
            int equalTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int i = equalTrailingIdx;
            while (true)
            {
                double num65;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num65 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num64;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num64 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num61;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            double num62;
                            double num63;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num63 = inClose[i - 1];
                            }
                            else
                            {
                                num63 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num62 = inOpen[i - 1];
                            }
                            else
                            {
                                num62 = inClose[i - 1];
                            }

                            num61 = inHigh[i - 1] - num63 + (num62 - inLow[i - 1]);
                        }
                        else
                        {
                            num61 = 0.0;
                        }

                        num64 = num61;
                    }

                    num65 = num64;
                }

                equalPeriodTotal += num65;
                i++;
            }

            i = bodyLongTrailingIdx;
            while (true)
            {
                double num55;
                double num60;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num60 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    double num59;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num59 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num56;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num57;
                            double num58;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num58 = inClose[i - 1];
                            }
                            else
                            {
                                num58 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num57 = inOpen[i - 1];
                            }
                            else
                            {
                                num57 = inClose[i - 1];
                            }

                            num56 = inHigh[i - 1] - num58 + (num57 - inLow[i - 1]);
                        }
                        else
                        {
                            num56 = 0.0;
                        }

                        num59 = num56;
                    }

                    num60 = num59;
                }

                bodyLongPeriodTotal[1] += num60;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num55 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num54;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num54 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num51;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num52;
                            double num53;
                            if (inClose[i] >= inOpen[i])
                            {
                                num53 = inClose[i];
                            }
                            else
                            {
                                num53 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num52 = inOpen[i];
                            }
                            else
                            {
                                num52 = inClose[i];
                            }

                            num51 = inHigh[i] - num53 + (num52 - inLow[i]);
                        }
                        else
                        {
                            num51 = 0.0;
                        }

                        num54 = num51;
                    }

                    num55 = num54;
                }

                bodyLongPeriodTotal[0] += num55;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_0345:
            if (inClose[i] >= inOpen[i])
            {
                num50 = 1;
            }
            else
            {
                num50 = -1;
            }

            if ((inClose[i - 1] < inOpen[i - 1] ? -1 : 1) == -num50)
            {
                double num49;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num49 = bodyLongPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    double num48;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num48 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        double num47;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num47 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num44;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                            {
                                double num45;
                                double num46;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num46 = inClose[i - 1];
                                }
                                else
                                {
                                    num46 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num45 = inOpen[i - 1];
                                }
                                else
                                {
                                    num45 = inClose[i - 1];
                                }

                                num44 = inHigh[i - 1] - num46 + (num45 - inLow[i - 1]);
                            }
                            else
                            {
                                num44 = 0.0;
                            }

                            num47 = num44;
                        }

                        num48 = num47;
                    }

                    num49 = num48;
                }

                var num43 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) >
                    Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num49 / num43)
                {
                    double num42;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                    {
                        num42 = bodyLongPeriodTotal[0] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                    }
                    else
                    {
                        double num41;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                        {
                            num41 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            double num40;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                            {
                                num40 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num37;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                                {
                                    double num38;
                                    double num39;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num39 = inClose[i];
                                    }
                                    else
                                    {
                                        num39 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num38 = inOpen[i];
                                    }
                                    else
                                    {
                                        num38 = inClose[i];
                                    }

                                    num37 = inHigh[i] - num39 + (num38 - inLow[i]);
                                }
                                else
                                {
                                    num37 = 0.0;
                                }

                                num40 = num37;
                            }

                            num41 = num40;
                        }

                        num42 = num41;
                    }

                    var num36 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (Math.Abs(inClose[i] - inOpen[i]) > Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num42 / num36)
                    {
                        double num35;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                        {
                            num35 = equalPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                        }
                        else
                        {
                            double num34;
                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                            {
                                num34 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                            }
                            else
                            {
                                double num33;
                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                                {
                                    num33 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    double num30;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                                    {
                                        double num31;
                                        double num32;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num32 = inClose[i - 1];
                                        }
                                        else
                                        {
                                            num32 = inOpen[i - 1];
                                        }

                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num31 = inOpen[i - 1];
                                        }
                                        else
                                        {
                                            num31 = inClose[i - 1];
                                        }

                                        num30 = inHigh[i - 1] - num32 + (num31 - inLow[i - 1]);
                                    }
                                    else
                                    {
                                        num30 = 0.0;
                                    }

                                    num33 = num30;
                                }

                                num34 = num33;
                            }

                            num35 = num34;
                        }

                        var num29 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                        if (inClose[i] <= inClose[i - 1] + Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num35 / num29)
                        {
                            double num28;
                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                            {
                                num28 = equalPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                            }
                            else
                            {
                                double num27;
                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                                {
                                    num27 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                                }
                                else
                                {
                                    double num26;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                                    {
                                        num26 = inHigh[i - 1] - inLow[i - 1];
                                    }
                                    else
                                    {
                                        double num23;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                                        {
                                            double num24;
                                            double num25;
                                            if (inClose[i - 1] >= inOpen[i - 1])
                                            {
                                                num25 = inClose[i - 1];
                                            }
                                            else
                                            {
                                                num25 = inOpen[i - 1];
                                            }

                                            if (inClose[i - 1] >= inOpen[i - 1])
                                            {
                                                num24 = inOpen[i - 1];
                                            }
                                            else
                                            {
                                                num24 = inClose[i - 1];
                                            }

                                            num23 = inHigh[i - 1] - num25 + (num24 - inLow[i - 1]);
                                        }
                                        else
                                        {
                                            num23 = 0.0;
                                        }

                                        num26 = num23;
                                    }

                                    num27 = num26;
                                }

                                num28 = num27;
                            }

                            var num22 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                            if (inClose[i] >= inClose[i - 1] - Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num28 / num22)
                            {
                                int num21;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num21 = 1;
                                }
                                else
                                {
                                    num21 = -1;
                                }

                                outInteger[outIdx] = num21 * 100;
                                outIdx++;
                                goto Label_0902;
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0902:
            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                double num19;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                    {
                        double num17;
                        double num18;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num18 = inClose[i - 1];
                        }
                        else
                        {
                            num18 = inOpen[i - 1];
                        }

                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num17 = inOpen[i - 1];
                        }
                        else
                        {
                            num17 = inClose[i - 1];
                        }

                        num16 = inHigh[i - 1] - num18 + (num17 - inLow[i - 1]);
                    }
                    else
                    {
                        num16 = 0.0;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[equalTrailingIdx - 1] - inOpen[equalTrailingIdx - 1]);
            }
            else
            {
                double num14;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[equalTrailingIdx - 1] - inLow[equalTrailingIdx - 1];
                }
                else
                {
                    double num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num13 = inClose[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num13 = inOpen[equalTrailingIdx - 1];
                        }

                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num12 = inOpen[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num12 = inClose[equalTrailingIdx - 1];
                        }

                        num11 = inHigh[equalTrailingIdx - 1] - num13 + (num12 - inLow[equalTrailingIdx - 1]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            equalPeriodTotal += num20 - num15;
            for (var totIdx = 1; totIdx >= 0; totIdx--)
            {
                double num5;
                double num10;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    double num9;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num6;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num7;
                            double num8;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num8 = inClose[i - totIdx];
                            }
                            else
                            {
                                num8 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num7 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num7 = inClose[i - totIdx];
                            }

                            num6 = inHigh[i - totIdx] - num8 + (num7 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num6 = 0.0;
                        }

                        num9 = num6;
                    }

                    num10 = num9;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs(inClose[bodyLongTrailingIdx - totIdx] - inOpen[bodyLongTrailingIdx - totIdx]);
                }
                else
                {
                    double num4;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[bodyLongTrailingIdx - totIdx] - inLow[bodyLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            double num2;
                            double num3;
                            if (inClose[bodyLongTrailingIdx - totIdx] >= inOpen[bodyLongTrailingIdx - totIdx])
                            {
                                num3 = inClose[bodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[bodyLongTrailingIdx - totIdx];
                            }

                            if (inClose[bodyLongTrailingIdx - totIdx] >= inOpen[bodyLongTrailingIdx - totIdx])
                            {
                                num2 = inOpen[bodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[bodyLongTrailingIdx - totIdx];
                            }

                            num = inHigh[bodyLongTrailingIdx - totIdx] - num3 + (num2 - inLow[bodyLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                bodyLongPeriodTotal[totIdx] += num10 - num5;
            }

            i++;
            equalTrailingIdx++;
            bodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0345;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlCounterAttack(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num15;
            decimal num20;
            int num50;
            var bodyLongPeriodTotal = new decimal[2];
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlCounterAttackLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            decimal equalPeriodTotal = default;
            int equalTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
            int bodyLongTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
            int i = equalTrailingIdx;
            while (true)
            {
                decimal num65;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                {
                    num65 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num64;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                    {
                        num64 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num61;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                        {
                            decimal num62;
                            decimal num63;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num63 = inClose[i - 1];
                            }
                            else
                            {
                                num63 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num62 = inOpen[i - 1];
                            }
                            else
                            {
                                num62 = inClose[i - 1];
                            }

                            num61 = inHigh[i - 1] - num63 + (num62 - inLow[i - 1]);
                        }
                        else
                        {
                            num61 = Decimal.Zero;
                        }

                        num64 = num61;
                    }

                    num65 = num64;
                }

                equalPeriodTotal += num65;
                i++;
            }

            i = bodyLongTrailingIdx;
            while (true)
            {
                decimal num55;
                decimal num60;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num60 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                }
                else
                {
                    decimal num59;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num59 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        decimal num56;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num57;
                            decimal num58;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num58 = inClose[i - 1];
                            }
                            else
                            {
                                num58 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num57 = inOpen[i - 1];
                            }
                            else
                            {
                                num57 = inClose[i - 1];
                            }

                            num56 = inHigh[i - 1] - num58 + (num57 - inLow[i - 1]);
                        }
                        else
                        {
                            num56 = Decimal.Zero;
                        }

                        num59 = num56;
                    }

                    num60 = num59;
                }

                bodyLongPeriodTotal[1] += num60;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num55 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num54;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num54 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num51;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num52;
                            decimal num53;
                            if (inClose[i] >= inOpen[i])
                            {
                                num53 = inClose[i];
                            }
                            else
                            {
                                num53 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num52 = inOpen[i];
                            }
                            else
                            {
                                num52 = inClose[i];
                            }

                            num51 = inHigh[i] - num53 + (num52 - inLow[i]);
                        }
                        else
                        {
                            num51 = Decimal.Zero;
                        }

                        num54 = num51;
                    }

                    num55 = num54;
                }

                bodyLongPeriodTotal[0] += num55;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_036F:
            if (inClose[i] >= inOpen[i])
            {
                num50 = 1;
            }
            else
            {
                num50 = -1;
            }

            if ((inClose[i - 1] < inOpen[i - 1] ? -1 : 1) == -num50)
            {
                decimal num49;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                {
                    num49 = bodyLongPeriodTotal[1] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                }
                else
                {
                    decimal num48;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                    {
                        num48 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                    }
                    else
                    {
                        decimal num47;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                        {
                            num47 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            decimal num44;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                            {
                                decimal num45;
                                decimal num46;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num46 = inClose[i - 1];
                                }
                                else
                                {
                                    num46 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num45 = inOpen[i - 1];
                                }
                                else
                                {
                                    num45 = inClose[i - 1];
                                }

                                num44 = inHigh[i - 1] - num46 + (num45 - inLow[i - 1]);
                            }
                            else
                            {
                                num44 = Decimal.Zero;
                            }

                            num47 = num44;
                        }

                        num48 = num47;
                    }

                    num49 = num48;
                }

                var num43 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) >
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num49 / num43)
                {
                    decimal num42;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod != 0)
                    {
                        num42 = bodyLongPeriodTotal[0] / Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod;
                    }
                    else
                    {
                        decimal num41;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                        {
                            num41 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            decimal num40;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                            {
                                num40 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                decimal num37;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                                {
                                    decimal num38;
                                    decimal num39;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num39 = inClose[i];
                                    }
                                    else
                                    {
                                        num39 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num38 = inOpen[i];
                                    }
                                    else
                                    {
                                        num38 = inClose[i];
                                    }

                                    num37 = inHigh[i] - num39 + (num38 - inLow[i]);
                                }
                                else
                                {
                                    num37 = Decimal.Zero;
                                }

                                num40 = num37;
                            }

                            num41 = num40;
                        }

                        num42 = num41;
                    }

                    var num36 = Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (Math.Abs(inClose[i] - inOpen[i]) >
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyLong].Factor * num42 / num36)
                    {
                        decimal num35;
                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                        {
                            num35 = equalPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                        }
                        else
                        {
                            decimal num34;
                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                            {
                                num34 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                            }
                            else
                            {
                                decimal num33;
                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                                {
                                    num33 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    decimal num30;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                                    {
                                        decimal num31;
                                        decimal num32;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num32 = inClose[i - 1];
                                        }
                                        else
                                        {
                                            num32 = inOpen[i - 1];
                                        }

                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num31 = inOpen[i - 1];
                                        }
                                        else
                                        {
                                            num31 = inClose[i - 1];
                                        }

                                        num30 = inHigh[i - 1] - num32 + (num31 - inLow[i - 1]);
                                    }
                                    else
                                    {
                                        num30 = Decimal.Zero;
                                    }

                                    num33 = num30;
                                }

                                num34 = num33;
                            }

                            num35 = num34;
                        }

                        var num29 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2m : 1m;

                        if (inClose[i] <=
                            inClose[i - 1] + (decimal) Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num35 / num29)
                        {
                            decimal num28;
                            if (Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod != 0)
                            {
                                num28 = equalPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod;
                            }
                            else
                            {
                                decimal num27;
                                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
                                {
                                    num27 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
                                }
                                else
                                {
                                    decimal num26;
                                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                                    {
                                        num26 = inHigh[i - 1] - inLow[i - 1];
                                    }
                                    else
                                    {
                                        decimal num23;
                                        if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                                        {
                                            decimal num24;
                                            decimal num25;
                                            if (inClose[i - 1] >= inOpen[i - 1])
                                            {
                                                num25 = inClose[i - 1];
                                            }
                                            else
                                            {
                                                num25 = inOpen[i - 1];
                                            }

                                            if (inClose[i - 1] >= inOpen[i - 1])
                                            {
                                                num24 = inOpen[i - 1];
                                            }
                                            else
                                            {
                                                num24 = inClose[i - 1];
                                            }

                                            num23 = inHigh[i - 1] - num25 + (num24 - inLow[i - 1]);
                                        }
                                        else
                                        {
                                            num23 = Decimal.Zero;
                                        }

                                        num26 = num23;
                                    }

                                    num27 = num26;
                                }

                                num28 = num27;
                            }

                            var num22 = Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows ? 2m : 1m;

                            if (inClose[i] >=
                                inClose[i - 1] - (decimal) Globals.CandleSettings[(int) CandleSettingType.Equal].Factor * num28 / num22)
                            {
                                int num21;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num21 = 1;
                                }
                                else
                                {
                                    num21 = -1;
                                }

                                outInteger[outIdx] = num21 * 100;
                                outIdx++;
                                goto Label_0976;
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0976:
            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i - 1] - inOpen[i - 1]);
            }
            else
            {
                decimal num19;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    decimal num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                    {
                        decimal num17;
                        decimal num18;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num18 = inClose[i - 1];
                        }
                        else
                        {
                            num18 = inOpen[i - 1];
                        }

                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num17 = inOpen[i - 1];
                        }
                        else
                        {
                            num17 = inClose[i - 1];
                        }

                        num16 = inHigh[i - 1] - num18 + (num17 - inLow[i - 1]);
                    }
                    else
                    {
                        num16 = Decimal.Zero;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[equalTrailingIdx - 1] - inOpen[equalTrailingIdx - 1]);
            }
            else
            {
                decimal num14;
                if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.HighLow)
                {
                    num14 = inHigh[equalTrailingIdx - 1] - inLow[equalTrailingIdx - 1];
                }
                else
                {
                    decimal num11;
                    if (Globals.CandleSettings[(int) CandleSettingType.Equal].RangeType == RangeType.Shadows)
                    {
                        decimal num12;
                        decimal num13;
                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num13 = inClose[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num13 = inOpen[equalTrailingIdx - 1];
                        }

                        if (inClose[equalTrailingIdx - 1] >= inOpen[equalTrailingIdx - 1])
                        {
                            num12 = inOpen[equalTrailingIdx - 1];
                        }
                        else
                        {
                            num12 = inClose[equalTrailingIdx - 1];
                        }

                        num11 = inHigh[equalTrailingIdx - 1] - num13 + (num12 - inLow[equalTrailingIdx - 1]);
                    }
                    else
                    {
                        num11 = Decimal.Zero;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            equalPeriodTotal += num20 - num15;
            for (var totIdx = 1; totIdx >= 0; totIdx--)
            {
                decimal num5;
                decimal num10;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs(inClose[i - totIdx] - inOpen[i - totIdx]);
                }
                else
                {
                    decimal num9;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        decimal num6;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num7;
                            decimal num8;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num8 = inClose[i - totIdx];
                            }
                            else
                            {
                                num8 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num7 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num7 = inClose[i - totIdx];
                            }

                            num6 = inHigh[i - totIdx] - num8 + (num7 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num6 = Decimal.Zero;
                        }

                        num9 = num6;
                    }

                    num10 = num9;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs(inClose[bodyLongTrailingIdx - totIdx] - inOpen[bodyLongTrailingIdx - totIdx]);
                }
                else
                {
                    decimal num4;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[bodyLongTrailingIdx - totIdx] - inLow[bodyLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        decimal num;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyLong].RangeType == RangeType.Shadows)
                        {
                            decimal num2;
                            decimal num3;
                            if (inClose[bodyLongTrailingIdx - totIdx] >= inOpen[bodyLongTrailingIdx - totIdx])
                            {
                                num3 = inClose[bodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[bodyLongTrailingIdx - totIdx];
                            }

                            if (inClose[bodyLongTrailingIdx - totIdx] >= inOpen[bodyLongTrailingIdx - totIdx])
                            {
                                num2 = inOpen[bodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[bodyLongTrailingIdx - totIdx];
                            }

                            num = inHigh[bodyLongTrailingIdx - totIdx] - num3 + (num2 - inLow[bodyLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = Decimal.Zero;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                bodyLongPeriodTotal[totIdx] += num10 - num5;
            }

            i++;
            equalTrailingIdx++;
            bodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_036F;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlCounterAttackLookback()
        {
            int avgPeriod = Math.Max(Globals.CandleSettings[(int) CandleSettingType.Equal].AvgPeriod,
                Globals.CandleSettings[(int) CandleSettingType.BodyLong].AvgPeriod);

            return avgPeriod + 1;
        }
    }
}
