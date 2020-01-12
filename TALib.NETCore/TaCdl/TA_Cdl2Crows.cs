using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Cdl2Crows(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
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

            int lookbackTotal = Cdl2CrowsLookback();
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

            double BodyLongPeriodTotal = 0.0;
            int BodyLongTrailingIdx = (startIdx - 2) - Globals.candleSettings[0].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                double num24;
                if (i >= (startIdx - 2))
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num24 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num23;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num23 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num20;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num21;
                            double num22;
                            if (inClose[i] >= inOpen[i])
                            {
                                num22 = inClose[i];
                            }
                            else
                            {
                                num22 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num21 = inOpen[i];
                            }
                            else
                            {
                                num21 = inClose[i];
                            }

                            num20 = (inHigh[i] - num22) + (num21 - inLow[i]);
                        }
                        else
                        {
                            num20 = 0.0;
                        }

                        num23 = num20;
                    }

                    num24 = num23;
                }

                BodyLongPeriodTotal += num24;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_014D:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                double num13;
                double num19;
                if (Globals.candleSettings[0].avgPeriod != 0.0)
                {
                    num19 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
                }
                else
                {
                    double num18;
                    if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                    {
                        num18 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        double num17;
                        if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                        {
                            num17 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num14;
                            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                            {
                                double num15;
                                double num16;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num16 = inClose[i - 2];
                                }
                                else
                                {
                                    num16 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num15 = inOpen[i - 2];
                                }
                                else
                                {
                                    num15 = inClose[i - 2];
                                }

                                num14 = (inHigh[i - 2] - num16) + (num15 - inLow[i - 2]);
                            }
                            else
                            {
                                num14 = 0.0;
                            }

                            num17 = num14;
                        }

                        num18 = num17;
                    }

                    num19 = num18;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                {
                    num13 = 2.0;
                }
                else
                {
                    num13 = 1.0;
                }

                if ((Math.Abs((double) (inClose[i - 2] - inOpen[i - 2])) > ((Globals.candleSettings[0].factor * num19) / num13)) &&
                    (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1))
                {
                    double num11;
                    double num12;
                    if (inOpen[i - 1] < inClose[i - 1])
                    {
                        num12 = inOpen[i - 1];
                    }
                    else
                    {
                        num12 = inClose[i - 1];
                    }

                    if (inOpen[i - 2] > inClose[i - 2])
                    {
                        num11 = inOpen[i - 2];
                    }
                    else
                    {
                        num11 = inClose[i - 2];
                    }

                    if ((((num12 > num11) && (((inClose[i] < inOpen[i]) ? -1 : 1) == -1)) &&
                         ((inOpen[i] < inOpen[i - 1]) && (inOpen[i] > inClose[i - 1]))) &&
                        ((inClose[i] > inOpen[i - 2]) && (inClose[i] < inClose[i - 2])))
                    {
                        outInteger[outIdx] = -100;
                        outIdx++;
                        goto Label_036E;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_036E:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
            }
            else
            {
                double num9;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((double) (inClose[BodyLongTrailingIdx] - inOpen[BodyLongTrailingIdx]));
            }
            else
            {
                double num4;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[BodyLongTrailingIdx] - inLow[BodyLongTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[BodyLongTrailingIdx] >= inOpen[BodyLongTrailingIdx])
                        {
                            num3 = inClose[BodyLongTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[BodyLongTrailingIdx];
                        }

                        if (inClose[BodyLongTrailingIdx] >= inOpen[BodyLongTrailingIdx])
                        {
                            num2 = inOpen[BodyLongTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[BodyLongTrailingIdx];
                        }

                        num = (inHigh[BodyLongTrailingIdx] - num3) + (num2 - inLow[BodyLongTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            BodyLongPeriodTotal += num10 - num5;
            i++;
            BodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_014D;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Cdl2Crows(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
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

            int lookbackTotal = Cdl2CrowsLookback();
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

            double BodyLongPeriodTotal = 0.0;
            int BodyLongTrailingIdx = (startIdx - 2) - Globals.candleSettings[0].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                float num24;
                if (i >= (startIdx - 2))
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num24 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num23;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num23 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num20;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num21;
                            float num22;
                            if (inClose[i] >= inOpen[i])
                            {
                                num22 = inClose[i];
                            }
                            else
                            {
                                num22 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num21 = inOpen[i];
                            }
                            else
                            {
                                num21 = inClose[i];
                            }

                            num20 = (inHigh[i] - num22) + (num21 - inLow[i]);
                        }
                        else
                        {
                            num20 = 0.0f;
                        }

                        num23 = num20;
                    }

                    num24 = num23;
                }

                BodyLongPeriodTotal += num24;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_015B:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                double num13;
                double num19;
                if (Globals.candleSettings[0].avgPeriod != 0.0)
                {
                    num19 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
                }
                else
                {
                    float num18;
                    if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                    {
                        num18 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        float num17;
                        if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                        {
                            num17 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            float num14;
                            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                            {
                                float num15;
                                float num16;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num16 = inClose[i - 2];
                                }
                                else
                                {
                                    num16 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num15 = inOpen[i - 2];
                                }
                                else
                                {
                                    num15 = inClose[i - 2];
                                }

                                num14 = (inHigh[i - 2] - num16) + (num15 - inLow[i - 2]);
                            }
                            else
                            {
                                num14 = 0.0f;
                            }

                            num17 = num14;
                        }

                        num18 = num17;
                    }

                    num19 = num18;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                {
                    num13 = 2.0;
                }
                else
                {
                    num13 = 1.0;
                }

                if ((Math.Abs((float) (inClose[i - 2] - inOpen[i - 2])) > ((Globals.candleSettings[0].factor * num19) / num13)) &&
                    (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1))
                {
                    float num11;
                    float num12;
                    if (inOpen[i - 1] < inClose[i - 1])
                    {
                        num12 = inOpen[i - 1];
                    }
                    else
                    {
                        num12 = inClose[i - 1];
                    }

                    if (inOpen[i - 2] > inClose[i - 2])
                    {
                        num11 = inOpen[i - 2];
                    }
                    else
                    {
                        num11 = inClose[i - 2];
                    }

                    if ((((num12 > num11) && (((inClose[i] < inOpen[i]) ? -1 : 1) == -1)) &&
                         ((inOpen[i] < inOpen[i - 1]) && (inOpen[i] > inClose[i - 1]))) &&
                        ((inClose[i] > inOpen[i - 2]) && (inClose[i] < inClose[i - 2])))
                    {
                        outInteger[outIdx] = -100;
                        outIdx++;
                        goto Label_03A6;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_03A6:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
            }
            else
            {
                float num9;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    float num6;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((float) (inClose[BodyLongTrailingIdx] - inOpen[BodyLongTrailingIdx]));
            }
            else
            {
                float num4;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[BodyLongTrailingIdx] - inLow[BodyLongTrailingIdx];
                }
                else
                {
                    float num;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        float num2;
                        float num3;
                        if (inClose[BodyLongTrailingIdx] >= inOpen[BodyLongTrailingIdx])
                        {
                            num3 = inClose[BodyLongTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[BodyLongTrailingIdx];
                        }

                        if (inClose[BodyLongTrailingIdx] >= inOpen[BodyLongTrailingIdx])
                        {
                            num2 = inOpen[BodyLongTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[BodyLongTrailingIdx];
                        }

                        num = (inHigh[BodyLongTrailingIdx] - num3) + (num2 - inLow[BodyLongTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0f;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            BodyLongPeriodTotal += num10 - num5;
            i++;
            BodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_015B;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int Cdl2CrowsLookback()
        {
            return (Globals.candleSettings[0].avgPeriod + 2);
        }
    }
}
