using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlBreakaway(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            int num11;
            double num12;
            double num13;
            int num16;
            int num17;
            int num18;
            double num19;
            double num25;
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

            int lookbackTotal = CdlBreakawayLookback();
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
            int BodyLongTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                double num30;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num30 = Math.Abs((double) (inClose[i - 4] - inOpen[i - 4]));
                }
                else
                {
                    double num29;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num29 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        double num26;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num27;
                            double num28;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num28 = inClose[i - 4];
                            }
                            else
                            {
                                num28 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num27 = inOpen[i - 4];
                            }
                            else
                            {
                                num27 = inClose[i - 4];
                            }

                            num26 = (inHigh[i - 4] - num28) + (num27 - inLow[i - 4]);
                        }
                        else
                        {
                            num26 = 0.0;
                        }

                        num29 = num26;
                    }

                    num30 = num29;
                }

                BodyLongPeriodTotal += num30;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0168:
            if (Globals.candleSettings[0].avgPeriod != 0.0)
            {
                num25 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
            }
            else
            {
                double num24;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num24 = Math.Abs((double) (inClose[i - 4] - inOpen[i - 4]));
                }
                else
                {
                    double num23;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num23 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        double num20;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num21;
                            double num22;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num22 = inClose[i - 4];
                            }
                            else
                            {
                                num22 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num21 = inOpen[i - 4];
                            }
                            else
                            {
                                num21 = inClose[i - 4];
                            }

                            num20 = (inHigh[i - 4] - num22) + (num21 - inLow[i - 4]);
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

            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
            {
                num19 = 2.0;
            }
            else
            {
                num19 = 1.0;
            }

            if (Math.Abs((double) (inClose[i - 4] - inOpen[i - 4])) <= ((Globals.candleSettings[0].factor * num25) / num19))
            {
                goto Label_04B7;
            }

            if (inClose[i - 3] >= inOpen[i - 3])
            {
                num18 = 1;
            }
            else
            {
                num18 = -1;
            }

            if (((inClose[i - 4] < inOpen[i - 4]) ? -1 : 1) != num18)
            {
                goto Label_04B7;
            }

            if (inClose[i - 1] >= inOpen[i - 1])
            {
                num17 = 1;
            }
            else
            {
                num17 = -1;
            }

            if (((inClose[i - 3] < inOpen[i - 3]) ? -1 : 1) != num17)
            {
                goto Label_04B7;
            }

            if (inClose[i] >= inOpen[i])
            {
                num16 = 1;
            }
            else
            {
                num16 = -1;
            }

            if (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) != -num16)
            {
                goto Label_04B7;
            }

            if (((inClose[i - 4] < inOpen[i - 4]) ? -1 : 1) == -1)
            {
                double num14;
                double num15;
                if (inOpen[i - 3] > inClose[i - 3])
                {
                    num15 = inOpen[i - 3];
                }
                else
                {
                    num15 = inClose[i - 3];
                }

                if (inOpen[i - 4] < inClose[i - 4])
                {
                    num14 = inOpen[i - 4];
                }
                else
                {
                    num14 = inClose[i - 4];
                }

                if ((((num15 < num14) && (inHigh[i - 2] < inHigh[i - 3])) &&
                     ((inLow[i - 2] < inLow[i - 3]) && (inHigh[i - 1] < inHigh[i - 2]))) &&
                    (((inLow[i - 1] < inLow[i - 2]) && (inClose[i] > inOpen[i - 3])) && (inClose[i] < inClose[i - 4])))
                {
                    goto Label_0497;
                }
            }

            if (inClose[i - 4] < inOpen[i - 4])
            {
                goto Label_04B7;
            }

            if (inOpen[i - 3] < inClose[i - 3])
            {
                num13 = inOpen[i - 3];
            }
            else
            {
                num13 = inClose[i - 3];
            }

            if (inOpen[i - 4] > inClose[i - 4])
            {
                num12 = inOpen[i - 4];
            }
            else
            {
                num12 = inClose[i - 4];
            }

            if ((((num13 <= num12) || (inHigh[i - 2] <= inHigh[i - 3])) ||
                 ((inLow[i - 2] <= inLow[i - 3]) || (inHigh[i - 1] <= inHigh[i - 2]))) ||
                (((inLow[i - 1] <= inLow[i - 2]) || (inClose[i] >= inOpen[i - 3])) || (inClose[i] <= inClose[i - 4])))
            {
                goto Label_04B7;
            }

            Label_0497:
            if (inClose[i] >= inOpen[i])
            {
                num11 = 1;
            }
            else
            {
                num11 = -1;
            }

            outInteger[outIdx] = num11 * 100;
            outIdx++;
            goto Label_04C0;
            Label_04B7:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_04C0:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((double) (inClose[i - 4] - inOpen[i - 4]));
            }
            else
            {
                double num9;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 4] - inLow[i - 4];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num7;
                        double num8;
                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num8 = inClose[i - 4];
                        }
                        else
                        {
                            num8 = inOpen[i - 4];
                        }

                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num7 = inOpen[i - 4];
                        }
                        else
                        {
                            num7 = inClose[i - 4];
                        }

                        num6 = (inHigh[i - 4] - num8) + (num7 - inLow[i - 4]);
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
                num5 = Math.Abs((double) (inClose[BodyLongTrailingIdx - 4] - inOpen[BodyLongTrailingIdx - 4]));
            }
            else
            {
                double num4;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[BodyLongTrailingIdx - 4] - inLow[BodyLongTrailingIdx - 4];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[BodyLongTrailingIdx - 4] >= inOpen[BodyLongTrailingIdx - 4])
                        {
                            num3 = inClose[BodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num3 = inOpen[BodyLongTrailingIdx - 4];
                        }

                        if (inClose[BodyLongTrailingIdx - 4] >= inOpen[BodyLongTrailingIdx - 4])
                        {
                            num2 = inOpen[BodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num2 = inClose[BodyLongTrailingIdx - 4];
                        }

                        num = (inHigh[BodyLongTrailingIdx - 4] - num3) + (num2 - inLow[BodyLongTrailingIdx - 4]);
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
                goto Label_0168;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlBreakaway(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num5;
            float num10;
            int num11;
            float num12;
            float num13;
            int num16;
            int num17;
            int num18;
            double num19;
            double num25;
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

            int lookbackTotal = CdlBreakawayLookback();
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
            int BodyLongTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                float num30;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num30 = Math.Abs((float) (inClose[i - 4] - inOpen[i - 4]));
                }
                else
                {
                    float num29;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num29 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        float num26;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num27;
                            float num28;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num28 = inClose[i - 4];
                            }
                            else
                            {
                                num28 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num27 = inOpen[i - 4];
                            }
                            else
                            {
                                num27 = inClose[i - 4];
                            }

                            num26 = (inHigh[i - 4] - num28) + (num27 - inLow[i - 4]);
                        }
                        else
                        {
                            num26 = 0.0f;
                        }

                        num29 = num26;
                    }

                    num30 = num29;
                }

                BodyLongPeriodTotal += num30;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0176:
            if (Globals.candleSettings[0].avgPeriod != 0.0)
            {
                num25 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
            }
            else
            {
                float num24;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num24 = Math.Abs((float) (inClose[i - 4] - inOpen[i - 4]));
                }
                else
                {
                    float num23;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num23 = inHigh[i - 4] - inLow[i - 4];
                    }
                    else
                    {
                        float num20;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num21;
                            float num22;
                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num22 = inClose[i - 4];
                            }
                            else
                            {
                                num22 = inOpen[i - 4];
                            }

                            if (inClose[i - 4] >= inOpen[i - 4])
                            {
                                num21 = inOpen[i - 4];
                            }
                            else
                            {
                                num21 = inClose[i - 4];
                            }

                            num20 = (inHigh[i - 4] - num22) + (num21 - inLow[i - 4]);
                        }
                        else
                        {
                            num20 = 0.0f;
                        }

                        num23 = num20;
                    }

                    num24 = num23;
                }

                num25 = num24;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
            {
                num19 = 2.0;
            }
            else
            {
                num19 = 1.0;
            }

            if (Math.Abs((float) (inClose[i - 4] - inOpen[i - 4])) <= ((Globals.candleSettings[0].factor * num25) / num19))
            {
                goto Label_0518;
            }

            if (inClose[i - 3] >= inOpen[i - 3])
            {
                num18 = 1;
            }
            else
            {
                num18 = -1;
            }

            if (((inClose[i - 4] < inOpen[i - 4]) ? -1 : 1) != num18)
            {
                goto Label_0518;
            }

            if (inClose[i - 1] >= inOpen[i - 1])
            {
                num17 = 1;
            }
            else
            {
                num17 = -1;
            }

            if (((inClose[i - 3] < inOpen[i - 3]) ? -1 : 1) != num17)
            {
                goto Label_0518;
            }

            if (inClose[i] >= inOpen[i])
            {
                num16 = 1;
            }
            else
            {
                num16 = -1;
            }

            if (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) != -num16)
            {
                goto Label_0518;
            }

            if (((inClose[i - 4] < inOpen[i - 4]) ? -1 : 1) == -1)
            {
                float num14;
                float num15;
                if (inOpen[i - 3] > inClose[i - 3])
                {
                    num15 = inOpen[i - 3];
                }
                else
                {
                    num15 = inClose[i - 3];
                }

                if (inOpen[i - 4] < inClose[i - 4])
                {
                    num14 = inOpen[i - 4];
                }
                else
                {
                    num14 = inClose[i - 4];
                }

                if ((((num15 < num14) && (inHigh[i - 2] < inHigh[i - 3])) &&
                     ((inLow[i - 2] < inLow[i - 3]) && (inHigh[i - 1] < inHigh[i - 2]))) &&
                    (((inLow[i - 1] < inLow[i - 2]) && (inClose[i] > inOpen[i - 3])) && (inClose[i] < inClose[i - 4])))
                {
                    goto Label_04F6;
                }
            }

            if (inClose[i - 4] < inOpen[i - 4])
            {
                goto Label_0518;
            }

            if (inOpen[i - 3] < inClose[i - 3])
            {
                num13 = inOpen[i - 3];
            }
            else
            {
                num13 = inClose[i - 3];
            }

            if (inOpen[i - 4] > inClose[i - 4])
            {
                num12 = inOpen[i - 4];
            }
            else
            {
                num12 = inClose[i - 4];
            }

            if ((((num13 <= num12) || (inHigh[i - 2] <= inHigh[i - 3])) ||
                 ((inLow[i - 2] <= inLow[i - 3]) || (inHigh[i - 1] <= inHigh[i - 2]))) ||
                (((inLow[i - 1] <= inLow[i - 2]) || (inClose[i] >= inOpen[i - 3])) || (inClose[i] <= inClose[i - 4])))
            {
                goto Label_0518;
            }

            Label_04F6:
            if (inClose[i] >= inOpen[i])
            {
                num11 = 1;
            }
            else
            {
                num11 = -1;
            }

            outInteger[outIdx] = num11 * 100;
            outIdx++;
            goto Label_0521;
            Label_0518:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_0521:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((float) (inClose[i - 4] - inOpen[i - 4]));
            }
            else
            {
                float num9;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 4] - inLow[i - 4];
                }
                else
                {
                    float num6;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        float num7;
                        float num8;
                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num8 = inClose[i - 4];
                        }
                        else
                        {
                            num8 = inOpen[i - 4];
                        }

                        if (inClose[i - 4] >= inOpen[i - 4])
                        {
                            num7 = inOpen[i - 4];
                        }
                        else
                        {
                            num7 = inClose[i - 4];
                        }

                        num6 = (inHigh[i - 4] - num8) + (num7 - inLow[i - 4]);
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
                num5 = Math.Abs((float) (inClose[BodyLongTrailingIdx - 4] - inOpen[BodyLongTrailingIdx - 4]));
            }
            else
            {
                float num4;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[BodyLongTrailingIdx - 4] - inLow[BodyLongTrailingIdx - 4];
                }
                else
                {
                    float num;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        float num2;
                        float num3;
                        if (inClose[BodyLongTrailingIdx - 4] >= inOpen[BodyLongTrailingIdx - 4])
                        {
                            num3 = inClose[BodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num3 = inOpen[BodyLongTrailingIdx - 4];
                        }

                        if (inClose[BodyLongTrailingIdx - 4] >= inOpen[BodyLongTrailingIdx - 4])
                        {
                            num2 = inOpen[BodyLongTrailingIdx - 4];
                        }
                        else
                        {
                            num2 = inClose[BodyLongTrailingIdx - 4];
                        }

                        num = (inHigh[BodyLongTrailingIdx - 4] - num3) + (num2 - inLow[BodyLongTrailingIdx - 4]);
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
                goto Label_0176;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlBreakawayLookback()
        {
            return (Globals.candleSettings[0].avgPeriod + 4);
        }
    }
}
