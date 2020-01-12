using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlMatchingLow(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (((inOpen == null) || (inHigh == null)) || ((inLow == null) || (inClose == null)))
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlMatchingLowLookback();
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

            double EqualPeriodTotal = 0.0;
            int EqualTrailingIdx = startIdx - Globals.candleSettings[10].avgPeriod;
            int i = EqualTrailingIdx;
            while (true)
            {
                double num29;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                {
                    num29 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num28;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num28 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num25;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            double num26;
                            double num27;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num27 = inClose[i - 1];
                            }
                            else
                            {
                                num27 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num26 = inOpen[i - 1];
                            }
                            else
                            {
                                num26 = inClose[i - 1];
                            }

                            num25 = (inHigh[i - 1] - num27) + (num26 - inLow[i - 1]);
                        }
                        else
                        {
                            num25 = 0.0;
                        }

                        num28 = num25;
                    }

                    num29 = num28;
                }

                EqualPeriodTotal += num29;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_016C:
            if ((((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1) && (((inClose[i] < inOpen[i]) ? -1 : 1) == -1))
            {
                double num18;
                double num24;
                if (Globals.candleSettings[10].avgPeriod != 0.0)
                {
                    num24 = EqualPeriodTotal / ((double) Globals.candleSettings[10].avgPeriod);
                }
                else
                {
                    double num23;
                    if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                    {
                        num23 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                    }
                    else
                    {
                        double num22;
                        if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                        {
                            num22 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num19;
                            if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                            {
                                double num20;
                                double num21;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num21 = inClose[i - 1];
                                }
                                else
                                {
                                    num21 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num20 = inOpen[i - 1];
                                }
                                else
                                {
                                    num20 = inClose[i - 1];
                                }

                                num19 = (inHigh[i - 1] - num21) + (num20 - inLow[i - 1]);
                            }
                            else
                            {
                                num19 = 0.0;
                            }

                            num22 = num19;
                        }

                        num23 = num22;
                    }

                    num24 = num23;
                }

                if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                {
                    num18 = 2.0;
                }
                else
                {
                    num18 = 1.0;
                }

                if (inClose[i] <= (inClose[i - 1] + ((Globals.candleSettings[10].factor * num24) / num18)))
                {
                    double num11;
                    double num17;
                    if (Globals.candleSettings[10].avgPeriod != 0.0)
                    {
                        num17 = EqualPeriodTotal / ((double) Globals.candleSettings[10].avgPeriod);
                    }
                    else
                    {
                        double num16;
                        if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                        {
                            num16 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                        }
                        else
                        {
                            double num15;
                            if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                            {
                                num15 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num12;
                                if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                {
                                    double num13;
                                    double num14;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num14 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num14 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num13 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num13 = inClose[i - 1];
                                    }

                                    num12 = (inHigh[i - 1] - num14) + (num13 - inLow[i - 1]);
                                }
                                else
                                {
                                    num12 = 0.0;
                                }

                                num15 = num12;
                            }

                            num16 = num15;
                        }

                        num17 = num16;
                    }

                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                    {
                        num11 = 2.0;
                    }
                    else
                    {
                        num11 = 1.0;
                    }

                    if (inClose[i] >= (inClose[i - 1] - ((Globals.candleSettings[10].factor * num17) / num11)))
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_046A;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_046A:
            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                double num9;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                    {
                        double num7;
                        double num8;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num8 = inClose[i - 1];
                        }
                        else
                        {
                            num8 = inOpen[i - 1];
                        }

                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num7 = inOpen[i - 1];
                        }
                        else
                        {
                            num7 = inClose[i - 1];
                        }

                        num6 = (inHigh[i - 1] - num8) + (num7 - inLow[i - 1]);
                    }
                    else
                    {
                        num6 = 0.0;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((double) (inClose[EqualTrailingIdx - 1] - inOpen[EqualTrailingIdx - 1]));
            }
            else
            {
                double num4;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[EqualTrailingIdx - 1] - inLow[EqualTrailingIdx - 1];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[EqualTrailingIdx - 1] >= inOpen[EqualTrailingIdx - 1])
                        {
                            num3 = inClose[EqualTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[EqualTrailingIdx - 1];
                        }

                        if (inClose[EqualTrailingIdx - 1] >= inOpen[EqualTrailingIdx - 1])
                        {
                            num2 = inOpen[EqualTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[EqualTrailingIdx - 1];
                        }

                        num = (inHigh[EqualTrailingIdx - 1] - num3) + (num2 - inLow[EqualTrailingIdx - 1]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            EqualPeriodTotal += num10 - num5;
            i++;
            EqualTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_016C;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlMatchingLow(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num5;
            float num10;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (((inOpen == null) || (inHigh == null)) || ((inLow == null) || (inClose == null)))
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlMatchingLowLookback();
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

            double EqualPeriodTotal = 0.0;
            int EqualTrailingIdx = startIdx - Globals.candleSettings[10].avgPeriod;
            int i = EqualTrailingIdx;
            while (true)
            {
                float num29;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                {
                    num29 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num28;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num28 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num25;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            float num26;
                            float num27;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num27 = inClose[i - 1];
                            }
                            else
                            {
                                num27 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num26 = inOpen[i - 1];
                            }
                            else
                            {
                                num26 = inClose[i - 1];
                            }

                            num25 = (inHigh[i - 1] - num27) + (num26 - inLow[i - 1]);
                        }
                        else
                        {
                            num25 = 0.0f;
                        }

                        num28 = num25;
                    }

                    num29 = num28;
                }

                EqualPeriodTotal += num29;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_017A:
            if ((((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1) && (((inClose[i] < inOpen[i]) ? -1 : 1) == -1))
            {
                double num18;
                double num24;
                if (Globals.candleSettings[10].avgPeriod != 0.0)
                {
                    num24 = EqualPeriodTotal / ((double) Globals.candleSettings[10].avgPeriod);
                }
                else
                {
                    float num23;
                    if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                    {
                        num23 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                    }
                    else
                    {
                        float num22;
                        if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                        {
                            num22 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            float num19;
                            if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                            {
                                float num20;
                                float num21;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num21 = inClose[i - 1];
                                }
                                else
                                {
                                    num21 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num20 = inOpen[i - 1];
                                }
                                else
                                {
                                    num20 = inClose[i - 1];
                                }

                                num19 = (inHigh[i - 1] - num21) + (num20 - inLow[i - 1]);
                            }
                            else
                            {
                                num19 = 0.0f;
                            }

                            num22 = num19;
                        }

                        num23 = num22;
                    }

                    num24 = num23;
                }

                if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                {
                    num18 = 2.0;
                }
                else
                {
                    num18 = 1.0;
                }

                if (inClose[i] <= (inClose[i - 1] + ((Globals.candleSettings[10].factor * num24) / num18)))
                {
                    double num11;
                    double num17;
                    if (Globals.candleSettings[10].avgPeriod != 0.0)
                    {
                        num17 = EqualPeriodTotal / ((double) Globals.candleSettings[10].avgPeriod);
                    }
                    else
                    {
                        float num16;
                        if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                        {
                            num16 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                        }
                        else
                        {
                            float num15;
                            if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                            {
                                num15 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                float num12;
                                if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                {
                                    float num13;
                                    float num14;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num14 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num14 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num13 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num13 = inClose[i - 1];
                                    }

                                    num12 = (inHigh[i - 1] - num14) + (num13 - inLow[i - 1]);
                                }
                                else
                                {
                                    num12 = 0.0f;
                                }

                                num15 = num12;
                            }

                            num16 = num15;
                        }

                        num17 = num16;
                    }

                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                    {
                        num11 = 2.0;
                    }
                    else
                    {
                        num11 = 1.0;
                    }

                    if (inClose[i] >= (inClose[i - 1] - ((Globals.candleSettings[10].factor * num17) / num11)))
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_049C;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_049C:
            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                float num9;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    float num6;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                    {
                        float num7;
                        float num8;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num8 = inClose[i - 1];
                        }
                        else
                        {
                            num8 = inOpen[i - 1];
                        }

                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num7 = inOpen[i - 1];
                        }
                        else
                        {
                            num7 = inClose[i - 1];
                        }

                        num6 = (inHigh[i - 1] - num8) + (num7 - inLow[i - 1]);
                    }
                    else
                    {
                        num6 = 0.0f;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((float) (inClose[EqualTrailingIdx - 1] - inOpen[EqualTrailingIdx - 1]));
            }
            else
            {
                float num4;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[EqualTrailingIdx - 1] - inLow[EqualTrailingIdx - 1];
                }
                else
                {
                    float num;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                    {
                        float num2;
                        float num3;
                        if (inClose[EqualTrailingIdx - 1] >= inOpen[EqualTrailingIdx - 1])
                        {
                            num3 = inClose[EqualTrailingIdx - 1];
                        }
                        else
                        {
                            num3 = inOpen[EqualTrailingIdx - 1];
                        }

                        if (inClose[EqualTrailingIdx - 1] >= inOpen[EqualTrailingIdx - 1])
                        {
                            num2 = inOpen[EqualTrailingIdx - 1];
                        }
                        else
                        {
                            num2 = inClose[EqualTrailingIdx - 1];
                        }

                        num = (inHigh[EqualTrailingIdx - 1] - num3) + (num2 - inLow[EqualTrailingIdx - 1]);
                    }
                    else
                    {
                        num = 0.0f;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            EqualPeriodTotal += num10 - num5;
            i++;
            EqualTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_017A;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlMatchingLowLookback()
        {
            return (Globals.candleSettings[10].avgPeriod + 1);
        }
    }
}
