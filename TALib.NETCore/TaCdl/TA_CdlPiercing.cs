using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlPiercing(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int totIdx;
            double[] BodyLongPeriodTotal = new double[2];
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

            int lookbackTotal = CdlPiercingLookback();
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

            BodyLongPeriodTotal[1] = 0.0;
            BodyLongPeriodTotal[0] = 0.0;
            int BodyLongTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                double num29;
                double num34;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num34 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num33;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num33 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num30;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

                            num30 = (inHigh[i - 1] - num32) + (num31 - inLow[i - 1]);
                        }
                        else
                        {
                            num30 = 0.0;
                        }

                        num33 = num30;
                    }

                    num34 = num33;
                }

                BodyLongPeriodTotal[1] += num34;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num29 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num28;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num28 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num25;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num26;
                            double num27;
                            if (inClose[i] >= inOpen[i])
                            {
                                num27 = inClose[i];
                            }
                            else
                            {
                                num27 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num26 = inOpen[i];
                            }
                            else
                            {
                                num26 = inClose[i];
                            }

                            num25 = (inHigh[i] - num27) + (num26 - inLow[i]);
                        }
                        else
                        {
                            num25 = 0.0;
                        }

                        num28 = num25;
                    }

                    num29 = num28;
                }

                BodyLongPeriodTotal[0] += num29;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0237:
            if (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1)
            {
                double num18;
                double num24;
                if (Globals.candleSettings[0].avgPeriod != 0.0)
                {
                    num24 = BodyLongPeriodTotal[1] / ((double) Globals.candleSettings[0].avgPeriod);
                }
                else
                {
                    double num23;
                    if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                    {
                        num23 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                    }
                    else
                    {
                        double num22;
                        if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                        {
                            num22 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num19;
                            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                {
                    num18 = 2.0;
                }
                else
                {
                    num18 = 1.0;
                }

                if ((Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) > ((Globals.candleSettings[0].factor * num24) / num18)) &&
                    (inClose[i] >= inOpen[i]))
                {
                    double num11;
                    double num17;
                    if (Globals.candleSettings[0].avgPeriod != 0.0)
                    {
                        num17 = BodyLongPeriodTotal[0] / ((double) Globals.candleSettings[0].avgPeriod);
                    }
                    else
                    {
                        double num16;
                        if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                        {
                            num16 = Math.Abs((double) (inClose[i] - inOpen[i]));
                        }
                        else
                        {
                            double num15;
                            if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                            {
                                num15 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num12;
                                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                                {
                                    double num13;
                                    double num14;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num14 = inClose[i];
                                    }
                                    else
                                    {
                                        num14 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num13 = inOpen[i];
                                    }
                                    else
                                    {
                                        num13 = inClose[i];
                                    }

                                    num12 = (inHigh[i] - num14) + (num13 - inLow[i]);
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

                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        num11 = 2.0;
                    }
                    else
                    {
                        num11 = 1.0;
                    }

                    if (((Math.Abs((double) (inClose[i] - inOpen[i])) > ((Globals.candleSettings[0].factor * num17) / num11)) &&
                         (inOpen[i] < inLow[i - 1])) && ((inClose[i] < inOpen[i - 1]) &&
                                                         (inClose[i] > (inClose[i - 1] +
                                                                        (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) * 0.5)))))
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_0558;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0558:
            totIdx = 1;
            while (totIdx >= 0)
            {
                double num5;
                double num10;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs((double) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    double num9;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num6;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

                            num6 = (inHigh[i - totIdx] - num8) + (num7 - inLow[i - totIdx]);
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
                    num5 = Math.Abs((double) (inClose[BodyLongTrailingIdx - totIdx] - inOpen[BodyLongTrailingIdx - totIdx]));
                }
                else
                {
                    double num4;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[BodyLongTrailingIdx - totIdx] - inLow[BodyLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num2;
                            double num3;
                            if (inClose[BodyLongTrailingIdx - totIdx] >= inOpen[BodyLongTrailingIdx - totIdx])
                            {
                                num3 = inClose[BodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[BodyLongTrailingIdx - totIdx];
                            }

                            if (inClose[BodyLongTrailingIdx - totIdx] >= inOpen[BodyLongTrailingIdx - totIdx])
                            {
                                num2 = inOpen[BodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[BodyLongTrailingIdx - totIdx];
                            }

                            num = (inHigh[BodyLongTrailingIdx - totIdx] - num3) + (num2 - inLow[BodyLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                BodyLongPeriodTotal[totIdx] += num10 - num5;
                totIdx--;
            }

            i++;
            BodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0237;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlPiercing(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int totIdx;
            double[] BodyLongPeriodTotal = new double[2];
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

            int lookbackTotal = CdlPiercingLookback();
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

            BodyLongPeriodTotal[1] = 0.0;
            BodyLongPeriodTotal[0] = 0.0;
            int BodyLongTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                float num29;
                float num34;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num34 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num33;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num33 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num30;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num31;
                            float num32;
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

                            num30 = (inHigh[i - 1] - num32) + (num31 - inLow[i - 1]);
                        }
                        else
                        {
                            num30 = 0.0f;
                        }

                        num33 = num30;
                    }

                    num34 = num33;
                }

                BodyLongPeriodTotal[1] += num34;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num29 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num28;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num28 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num25;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num26;
                            float num27;
                            if (inClose[i] >= inOpen[i])
                            {
                                num27 = inClose[i];
                            }
                            else
                            {
                                num27 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num26 = inOpen[i];
                            }
                            else
                            {
                                num26 = inClose[i];
                            }

                            num25 = (inHigh[i] - num27) + (num26 - inLow[i]);
                        }
                        else
                        {
                            num25 = 0.0f;
                        }

                        num28 = num25;
                    }

                    num29 = num28;
                }

                BodyLongPeriodTotal[0] += num29;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0253:
            if (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1)
            {
                double num18;
                double num24;
                if (Globals.candleSettings[0].avgPeriod != 0.0)
                {
                    num24 = BodyLongPeriodTotal[1] / ((double) Globals.candleSettings[0].avgPeriod);
                }
                else
                {
                    float num23;
                    if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                    {
                        num23 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                    }
                    else
                    {
                        float num22;
                        if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                        {
                            num22 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            float num19;
                            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                {
                    num18 = 2.0;
                }
                else
                {
                    num18 = 1.0;
                }

                if ((Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) > ((Globals.candleSettings[0].factor * num24) / num18)) &&
                    (inClose[i] >= inOpen[i]))
                {
                    double num11;
                    double num17;
                    if (Globals.candleSettings[0].avgPeriod != 0.0)
                    {
                        num17 = BodyLongPeriodTotal[0] / ((double) Globals.candleSettings[0].avgPeriod);
                    }
                    else
                    {
                        float num16;
                        if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                        {
                            num16 = Math.Abs((float) (inClose[i] - inOpen[i]));
                        }
                        else
                        {
                            float num15;
                            if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                            {
                                num15 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                float num12;
                                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                                {
                                    float num13;
                                    float num14;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num14 = inClose[i];
                                    }
                                    else
                                    {
                                        num14 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num13 = inOpen[i];
                                    }
                                    else
                                    {
                                        num13 = inClose[i];
                                    }

                                    num12 = (inHigh[i] - num14) + (num13 - inLow[i]);
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

                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        num11 = 2.0;
                    }
                    else
                    {
                        num11 = 1.0;
                    }

                    if (((Math.Abs((float) (inClose[i] - inOpen[i])) > ((Globals.candleSettings[0].factor * num17) / num11)) &&
                         (inOpen[i] < inLow[i - 1])) && ((inClose[i] < inOpen[i - 1]) &&
                                                         (inClose[i] > (inClose[i - 1] +
                                                                        (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) * 0.5)))))
                    {
                        outInteger[outIdx] = 100;
                        outIdx++;
                        goto Label_05A6;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_05A6:
            totIdx = 1;
            while (totIdx >= 0)
            {
                float num5;
                float num10;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs((float) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    float num9;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        float num6;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num7;
                            float num8;
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

                            num6 = (inHigh[i - totIdx] - num8) + (num7 - inLow[i - totIdx]);
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
                    num5 = Math.Abs((float) (inClose[BodyLongTrailingIdx - totIdx] - inOpen[BodyLongTrailingIdx - totIdx]));
                }
                else
                {
                    float num4;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[BodyLongTrailingIdx - totIdx] - inLow[BodyLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        float num;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num2;
                            float num3;
                            if (inClose[BodyLongTrailingIdx - totIdx] >= inOpen[BodyLongTrailingIdx - totIdx])
                            {
                                num3 = inClose[BodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[BodyLongTrailingIdx - totIdx];
                            }

                            if (inClose[BodyLongTrailingIdx - totIdx] >= inOpen[BodyLongTrailingIdx - totIdx])
                            {
                                num2 = inOpen[BodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[BodyLongTrailingIdx - totIdx];
                            }

                            num = (inHigh[BodyLongTrailingIdx - totIdx] - num3) + (num2 - inLow[BodyLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0f;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                BodyLongPeriodTotal[totIdx] += num10 - num5;
                totIdx--;
            }

            i++;
            BodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0253;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlPiercingLookback()
        {
            return (Globals.candleSettings[0].avgPeriod + 1);
        }
    }
}
