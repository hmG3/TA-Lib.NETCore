using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlTristar(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num39;
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

            int lookbackTotal = CdlTristarLookback();
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

            double bodyPeriodTotal = default;
            int bodyTrailingIdx = startIdx - 2 - Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
            int i = bodyTrailingIdx;
            while (true)
            {
                double num44;
                if (i >= startIdx - 2)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num43;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num40;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            double num41;
                            double num42;
                            if (inClose[i] >= inOpen[i])
                            {
                                num42 = inClose[i];
                            }
                            else
                            {
                                num42 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num41 = inOpen[i];
                            }
                            else
                            {
                                num41 = inClose[i];
                            }

                            num40 = inHigh[i] - num42 + (num41 - inLow[i]);
                        }
                        else
                        {
                            num40 = 0.0;
                        }

                        num43 = num40;
                    }

                    num44 = num43;
                }

                bodyPeriodTotal += num44;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_014D:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod != 0)
            {
                num39 = bodyPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
            }
            else
            {
                double num38;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num38 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    double num37;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num37 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num34;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            double num35;
                            double num36;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num36 = inClose[i - 2];
                            }
                            else
                            {
                                num36 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num35 = inOpen[i - 2];
                            }
                            else
                            {
                                num35 = inClose[i - 2];
                            }

                            num34 = inHigh[i - 2] - num36 + (num35 - inLow[i - 2]);
                        }
                        else
                        {
                            num34 = 0.0;
                        }

                        num37 = num34;
                    }

                    num38 = num37;
                }

                num39 = num38;
            }

            var num33 = Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) <=
                Globals.CandleSettings[(int) CandleSettingType.BodyDoji].Factor * num39 / num33)
            {
                double num32;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod != 0)
                {
                    num32 = bodyPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
                }
                else
                {
                    double num31;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                    {
                        num31 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        double num30;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                        {
                            num30 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num27;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                            {
                                double num28;
                                double num29;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num29 = inClose[i - 2];
                                }
                                else
                                {
                                    num29 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num28 = inOpen[i - 2];
                                }
                                else
                                {
                                    num28 = inClose[i - 2];
                                }

                                num27 = inHigh[i - 2] - num29 + (num28 - inLow[i - 2]);
                            }
                            else
                            {
                                num27 = 0.0;
                            }

                            num30 = num27;
                        }

                        num31 = num30;
                    }

                    num32 = num31;
                }

                var num26 = Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) <=
                    Globals.CandleSettings[(int) CandleSettingType.BodyDoji].Factor * num32 / num26)
                {
                    double num25;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod != 0)
                    {
                        num25 = bodyPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
                    }
                    else
                    {
                        double num24;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                        {
                            num24 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                        }
                        else
                        {
                            double num23;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                            {
                                num23 = inHigh[i - 2] - inLow[i - 2];
                            }
                            else
                            {
                                double num20;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                                {
                                    double num21;
                                    double num22;
                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num22 = inClose[i - 2];
                                    }
                                    else
                                    {
                                        num22 = inOpen[i - 2];
                                    }

                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num21 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num21 = inClose[i - 2];
                                    }

                                    num20 = inHigh[i - 2] - num22 + (num21 - inLow[i - 2]);
                                }
                                else
                                {
                                    num20 = 0.0;
                                }

                                num23 = num20;
                            }

                            num24 = num23;
                        }

                        num25 = num24;
                    }

                    var num19 = Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                    if (Math.Abs(inClose[i] - inOpen[i]) <=
                        Globals.CandleSettings[(int) CandleSettingType.BodyDoji].Factor * num25 / num19)
                    {
                        double num13;
                        double num14;
                        double num17;
                        double num18;
                        outInteger[outIdx] = 0;
                        if (inOpen[i - 1] < inClose[i - 1])
                        {
                            num18 = inOpen[i - 1];
                        }
                        else
                        {
                            num18 = inClose[i - 1];
                        }

                        if (inOpen[i - 2] > inClose[i - 2])
                        {
                            num17 = inOpen[i - 2];
                        }
                        else
                        {
                            num17 = inClose[i - 2];
                        }

                        if (num18 > num17)
                        {
                            double num15;
                            double num16;
                            if (inOpen[i] > inClose[i])
                            {
                                num16 = inOpen[i];
                            }
                            else
                            {
                                num16 = inClose[i];
                            }

                            if (inOpen[i - 1] > inClose[i - 1])
                            {
                                num15 = inOpen[i - 1];
                            }
                            else
                            {
                                num15 = inClose[i - 1];
                            }

                            if (num16 < num15)
                            {
                                outInteger[outIdx] = -100;
                            }
                        }

                        if (inOpen[i - 1] > inClose[i - 1])
                        {
                            num14 = inOpen[i - 1];
                        }
                        else
                        {
                            num14 = inClose[i - 1];
                        }

                        if (inOpen[i - 2] < inClose[i - 2])
                        {
                            num13 = inOpen[i - 2];
                        }
                        else
                        {
                            num13 = inClose[i - 2];
                        }

                        if (num14 < num13)
                        {
                            double num11;
                            double num12;
                            if (inOpen[i] < inClose[i])
                            {
                                num12 = inOpen[i];
                            }
                            else
                            {
                                num12 = inClose[i];
                            }

                            if (inOpen[i - 1] < inClose[i - 1])
                            {
                                num11 = inOpen[i - 1];
                            }
                            else
                            {
                                num11 = inClose[i - 1];
                            }

                            if (num12 > num11)
                            {
                                outInteger[outIdx] = 100;
                            }
                        }

                        outIdx++;
                        goto Label_0681;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0681:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                    {
                        double num7;
                        double num8;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num8 = inClose[i - 2];
                        }
                        else
                        {
                            num8 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num7 = inOpen[i - 2];
                        }
                        else
                        {
                            num7 = inClose[i - 2];
                        }

                        num6 = inHigh[i - 2] - num8 + (num7 - inLow[i - 2]);
                    }
                    else
                    {
                        num6 = 0.0;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[bodyTrailingIdx] - inOpen[bodyTrailingIdx]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyTrailingIdx] - inLow[bodyTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num3 = inClose[bodyTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[bodyTrailingIdx];
                        }

                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num2 = inOpen[bodyTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[bodyTrailingIdx];
                        }

                        num = inHigh[bodyTrailingIdx] - num3 + (num2 - inLow[bodyTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            bodyPeriodTotal += num10 - num5;
            i++;
            bodyTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_014D;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlTristar(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            decimal num39;
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

            int lookbackTotal = CdlTristarLookback();
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

            decimal bodyPeriodTotal = default;
            int bodyTrailingIdx = startIdx - 2 - Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
            int i = bodyTrailingIdx;
            while (true)
            {
                decimal num44;
                if (i >= startIdx - 2)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num43;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num40;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            decimal num41;
                            decimal num42;
                            if (inClose[i] >= inOpen[i])
                            {
                                num42 = inClose[i];
                            }
                            else
                            {
                                num42 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num41 = inOpen[i];
                            }
                            else
                            {
                                num41 = inClose[i];
                            }

                            num40 = inHigh[i] - num42 + (num41 - inLow[i]);
                        }
                        else
                        {
                            num40 = Decimal.Zero;
                        }

                        num43 = num40;
                    }

                    num44 = num43;
                }

                bodyPeriodTotal += num44;
                i++;
            }

            i = startIdx;
            int outIdx = default;
            Label_015B:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod != 0)
            {
                num39 = bodyPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
            }
            else
            {
                decimal num38;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num38 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                }
                else
                {
                    decimal num37;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num37 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        decimal num34;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            decimal num35;
                            decimal num36;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num36 = inClose[i - 2];
                            }
                            else
                            {
                                num36 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num35 = inOpen[i - 2];
                            }
                            else
                            {
                                num35 = inClose[i - 2];
                            }

                            num34 = inHigh[i - 2] - num36 + (num35 - inLow[i - 2]);
                        }
                        else
                        {
                            num34 = Decimal.Zero;
                        }

                        num37 = num34;
                    }

                    num38 = num37;
                }

                num39 = num38;
            }

            var num33 = Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i - 2] - inOpen[i - 2]) <=
                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyDoji].Factor * num39 / num33)
            {
                decimal num32;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod != 0)
                {
                    num32 = bodyPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
                }
                else
                {
                    decimal num31;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                    {
                        num31 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                    }
                    else
                    {
                        decimal num30;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                        {
                            num30 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            decimal num27;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                            {
                                decimal num28;
                                decimal num29;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num29 = inClose[i - 2];
                                }
                                else
                                {
                                    num29 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num28 = inOpen[i - 2];
                                }
                                else
                                {
                                    num28 = inClose[i - 2];
                                }

                                num27 = inHigh[i - 2] - num29 + (num28 - inLow[i - 2]);
                            }
                            else
                            {
                                num27 = Decimal.Zero;
                            }

                            num30 = num27;
                        }

                        num31 = num30;
                    }

                    num32 = num31;
                }

                var num26 = Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i - 1] - inOpen[i - 1]) <=
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyDoji].Factor * num32 / num26)
                {
                    decimal num25;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod != 0)
                    {
                        num25 = bodyPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
                    }
                    else
                    {
                        decimal num24;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                        {
                            num24 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
                        }
                        else
                        {
                            decimal num23;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                            {
                                num23 = inHigh[i - 2] - inLow[i - 2];
                            }
                            else
                            {
                                decimal num20;
                                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                                {
                                    decimal num21;
                                    decimal num22;
                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num22 = inClose[i - 2];
                                    }
                                    else
                                    {
                                        num22 = inOpen[i - 2];
                                    }

                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num21 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num21 = inClose[i - 2];
                                    }

                                    num20 = inHigh[i - 2] - num22 + (num21 - inLow[i - 2]);
                                }
                                else
                                {
                                    num20 = Decimal.Zero;
                                }

                                num23 = num20;
                            }

                            num24 = num23;
                        }

                        num25 = num24;
                    }

                    var num19 = Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows ? 2m : 1m;

                    if (Math.Abs(inClose[i] - inOpen[i]) <=
                        (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyDoji].Factor * num25 / num19)
                    {
                        decimal num13;
                        decimal num14;
                        decimal num17;
                        decimal num18;
                        outInteger[outIdx] = 0;
                        if (inOpen[i - 1] < inClose[i - 1])
                        {
                            num18 = inOpen[i - 1];
                        }
                        else
                        {
                            num18 = inClose[i - 1];
                        }

                        if (inOpen[i - 2] > inClose[i - 2])
                        {
                            num17 = inOpen[i - 2];
                        }
                        else
                        {
                            num17 = inClose[i - 2];
                        }

                        if (num18 > num17)
                        {
                            decimal num15;
                            decimal num16;
                            if (inOpen[i] > inClose[i])
                            {
                                num16 = inOpen[i];
                            }
                            else
                            {
                                num16 = inClose[i];
                            }

                            if (inOpen[i - 1] > inClose[i - 1])
                            {
                                num15 = inOpen[i - 1];
                            }
                            else
                            {
                                num15 = inClose[i - 1];
                            }

                            if (num16 < num15)
                            {
                                outInteger[outIdx] = -100;
                            }
                        }

                        if (inOpen[i - 1] > inClose[i - 1])
                        {
                            num14 = inOpen[i - 1];
                        }
                        else
                        {
                            num14 = inClose[i - 1];
                        }

                        if (inOpen[i - 2] < inClose[i - 2])
                        {
                            num13 = inOpen[i - 2];
                        }
                        else
                        {
                            num13 = inClose[i - 2];
                        }

                        if (num14 < num13)
                        {
                            decimal num11;
                            decimal num12;
                            if (inOpen[i] < inClose[i])
                            {
                                num12 = inOpen[i];
                            }
                            else
                            {
                                num12 = inClose[i];
                            }

                            if (inOpen[i - 1] < inClose[i - 1])
                            {
                                num11 = inOpen[i - 1];
                            }
                            else
                            {
                                num11 = inClose[i - 1];
                            }

                            if (num12 > num11)
                            {
                                outInteger[outIdx] = 100;
                            }
                        }

                        outIdx++;
                        goto Label_06ED;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_06ED:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i - 2] - inOpen[i - 2]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                    {
                        decimal num7;
                        decimal num8;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num8 = inClose[i - 2];
                        }
                        else
                        {
                            num8 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num7 = inOpen[i - 2];
                        }
                        else
                        {
                            num7 = inClose[i - 2];
                        }

                        num6 = inHigh[i - 2] - num8 + (num7 - inLow[i - 2]);
                    }
                    else
                    {
                        num6 = Decimal.Zero;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[bodyTrailingIdx] - inOpen[bodyTrailingIdx]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyTrailingIdx] - inLow[bodyTrailingIdx];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num3 = inClose[bodyTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[bodyTrailingIdx];
                        }

                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num2 = inOpen[bodyTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[bodyTrailingIdx];
                        }

                        num = inHigh[bodyTrailingIdx] - num3 + (num2 - inLow[bodyTrailingIdx]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            bodyPeriodTotal += num10 - num5;
            i++;
            bodyTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_015B;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlTristarLookback()
        {
            return Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod + 2;
        }
    }
}
