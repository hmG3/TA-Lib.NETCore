using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlStickSandwhich(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
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

            int lookbackTotal = CdlStickSandwhichLookback();
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
                    num29 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num28;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num28 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num25;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            double num26;
                            double num27;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num27 = inClose[i - 2];
                            }
                            else
                            {
                                num27 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num26 = inOpen[i - 2];
                            }
                            else
                            {
                                num26 = inClose[i - 2];
                            }

                            num25 = (inHigh[i - 2] - num27) + (num26 - inLow[i - 2]);
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
            if ((((((inClose[i - 2] < inOpen[i - 2]) ? -1 : 1) == -1) && (inClose[i - 1] >= inOpen[i - 1])) &&
                 (((inClose[i] < inOpen[i]) ? -1 : 1) == -1)) && (inLow[i - 1] > inClose[i - 2]))
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
                        num23 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        double num22;
                        if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                        {
                            num22 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num19;
                            if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                            {
                                double num20;
                                double num21;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num21 = inClose[i - 2];
                                }
                                else
                                {
                                    num21 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num20 = inOpen[i - 2];
                                }
                                else
                                {
                                    num20 = inClose[i - 2];
                                }

                                num19 = (inHigh[i - 2] - num21) + (num20 - inLow[i - 2]);
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

                if (inClose[i] <= (inClose[i - 2] + ((Globals.candleSettings[10].factor * num24) / num18)))
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
                            num16 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                        }
                        else
                        {
                            double num15;
                            if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                            {
                                num15 = inHigh[i - 2] - inLow[i - 2];
                            }
                            else
                            {
                                double num12;
                                if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                {
                                    double num13;
                                    double num14;
                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num14 = inClose[i - 2];
                                    }
                                    else
                                    {
                                        num14 = inOpen[i - 2];
                                    }

                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num13 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num13 = inClose[i - 2];
                                    }

                                    num12 = (inHigh[i - 2] - num14) + (num13 - inLow[i - 2]);
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

                    if (inClose[i] >= (inClose[i - 2] - ((Globals.candleSettings[10].factor * num17) / num11)))
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_0492;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0492:
            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
            }
            else
            {
                double num9;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
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

                        num6 = (inHigh[i - 2] - num8) + (num7 - inLow[i - 2]);
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
                num5 = Math.Abs((double) (inClose[EqualTrailingIdx - 2] - inOpen[EqualTrailingIdx - 2]));
            }
            else
            {
                double num4;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[EqualTrailingIdx - 2] - inLow[EqualTrailingIdx - 2];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[EqualTrailingIdx - 2] >= inOpen[EqualTrailingIdx - 2])
                        {
                            num3 = inClose[EqualTrailingIdx - 2];
                        }
                        else
                        {
                            num3 = inOpen[EqualTrailingIdx - 2];
                        }

                        if (inClose[EqualTrailingIdx - 2] >= inOpen[EqualTrailingIdx - 2])
                        {
                            num2 = inOpen[EqualTrailingIdx - 2];
                        }
                        else
                        {
                            num2 = inClose[EqualTrailingIdx - 2];
                        }

                        num = (inHigh[EqualTrailingIdx - 2] - num3) + (num2 - inLow[EqualTrailingIdx - 2]);
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

        public static RetCode CdlStickSandwhich(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
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

            int lookbackTotal = CdlStickSandwhichLookback();
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
                    num29 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num28;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num28 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num25;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            float num26;
                            float num27;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num27 = inClose[i - 2];
                            }
                            else
                            {
                                num27 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num26 = inOpen[i - 2];
                            }
                            else
                            {
                                num26 = inClose[i - 2];
                            }

                            num25 = (inHigh[i - 2] - num27) + (num26 - inLow[i - 2]);
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
            if ((((((inClose[i - 2] < inOpen[i - 2]) ? -1 : 1) == -1) && (inClose[i - 1] >= inOpen[i - 1])) &&
                 (((inClose[i] < inOpen[i]) ? -1 : 1) == -1)) && (inLow[i - 1] > inClose[i - 2]))
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
                        num23 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        float num22;
                        if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                        {
                            num22 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            float num19;
                            if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                            {
                                float num20;
                                float num21;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num21 = inClose[i - 2];
                                }
                                else
                                {
                                    num21 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num20 = inOpen[i - 2];
                                }
                                else
                                {
                                    num20 = inClose[i - 2];
                                }

                                num19 = (inHigh[i - 2] - num21) + (num20 - inLow[i - 2]);
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

                if (inClose[i] <= (inClose[i - 2] + ((Globals.candleSettings[10].factor * num24) / num18)))
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
                            num16 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                        }
                        else
                        {
                            float num15;
                            if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                            {
                                num15 = inHigh[i - 2] - inLow[i - 2];
                            }
                            else
                            {
                                float num12;
                                if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                {
                                    float num13;
                                    float num14;
                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num14 = inClose[i - 2];
                                    }
                                    else
                                    {
                                        num14 = inOpen[i - 2];
                                    }

                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num13 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num13 = inClose[i - 2];
                                    }

                                    num12 = (inHigh[i - 2] - num14) + (num13 - inLow[i - 2]);
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

                    if (inClose[i] >= (inClose[i - 2] - ((Globals.candleSettings[10].factor * num17) / num11)))
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_04C8;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_04C8:
            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
            }
            else
            {
                float num9;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    float num6;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                    {
                        float num7;
                        float num8;
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

                        num6 = (inHigh[i - 2] - num8) + (num7 - inLow[i - 2]);
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
                num5 = Math.Abs((float) (inClose[EqualTrailingIdx - 2] - inOpen[EqualTrailingIdx - 2]));
            }
            else
            {
                float num4;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[EqualTrailingIdx - 2] - inLow[EqualTrailingIdx - 2];
                }
                else
                {
                    float num;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                    {
                        float num2;
                        float num3;
                        if (inClose[EqualTrailingIdx - 2] >= inOpen[EqualTrailingIdx - 2])
                        {
                            num3 = inClose[EqualTrailingIdx - 2];
                        }
                        else
                        {
                            num3 = inOpen[EqualTrailingIdx - 2];
                        }

                        if (inClose[EqualTrailingIdx - 2] >= inOpen[EqualTrailingIdx - 2])
                        {
                            num2 = inOpen[EqualTrailingIdx - 2];
                        }
                        else
                        {
                            num2 = inClose[EqualTrailingIdx - 2];
                        }

                        num = (inHigh[EqualTrailingIdx - 2] - num3) + (num2 - inLow[EqualTrailingIdx - 2]);
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

        public static int CdlStickSandwhichLookback()
        {
            return (Globals.candleSettings[10].avgPeriod + 2);
        }
    }
}
