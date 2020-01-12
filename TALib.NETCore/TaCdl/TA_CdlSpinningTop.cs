using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlSpinningTop(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num14;
            double num20;
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

            int lookbackTotal = CdlSpinningTopLookback();
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

            double BodyPeriodTotal = 0.0;
            int BodyTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            int i = BodyTrailingIdx;
            while (true)
            {
                double num25;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num25 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num24;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num24 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num21;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            double num22;
                            double num23;
                            if (inClose[i] >= inOpen[i])
                            {
                                num23 = inClose[i];
                            }
                            else
                            {
                                num23 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num22 = inOpen[i];
                            }
                            else
                            {
                                num22 = inClose[i];
                            }

                            num21 = (inHigh[i] - num23) + (num22 - inLow[i]);
                        }
                        else
                        {
                            num21 = 0.0;
                        }

                        num24 = num21;
                    }

                    num25 = num24;
                }

                BodyPeriodTotal += num25;
                i++;
            }

            int outIdx = 0;
            Label_0147:
            if (Globals.candleSettings[2].avgPeriod != 0.0)
            {
                num20 = BodyPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
            }
            else
            {
                double num19;
                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num19 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num18;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num18 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num15;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            double num16;
                            double num17;
                            if (inClose[i] >= inOpen[i])
                            {
                                num17 = inClose[i];
                            }
                            else
                            {
                                num17 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num16 = inOpen[i];
                            }
                            else
                            {
                                num16 = inClose[i];
                            }

                            num15 = (inHigh[i] - num17) + (num16 - inLow[i]);
                        }
                        else
                        {
                            num15 = 0.0;
                        }

                        num18 = num15;
                    }

                    num19 = num18;
                }

                num20 = num19;
            }

            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
            {
                num14 = 2.0;
            }
            else
            {
                num14 = 1.0;
            }

            if (Math.Abs((double) (inClose[i] - inOpen[i])) < ((Globals.candleSettings[2].factor * num20) / num14))
            {
                double num13;
                if (inClose[i] >= inOpen[i])
                {
                    num13 = inClose[i];
                }
                else
                {
                    num13 = inOpen[i];
                }

                if ((inHigh[i] - num13) > Math.Abs((double) (inClose[i] - inOpen[i])))
                {
                    double num12;
                    if (inClose[i] >= inOpen[i])
                    {
                        num12 = inOpen[i];
                    }
                    else
                    {
                        num12 = inClose[i];
                    }

                    if ((num12 - inLow[i]) > Math.Abs((double) (inClose[i] - inOpen[i])))
                    {
                        int num11;
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
                        goto Label_0304;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0304:
            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((double) (inClose[i] - inOpen[i]));
            }
            else
            {
                double num9;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        double num7;
                        double num8;
                        if (inClose[i] >= inOpen[i])
                        {
                            num8 = inClose[i];
                        }
                        else
                        {
                            num8 = inOpen[i];
                        }

                        if (inClose[i] >= inOpen[i])
                        {
                            num7 = inOpen[i];
                        }
                        else
                        {
                            num7 = inClose[i];
                        }

                        num6 = (inHigh[i] - num8) + (num7 - inLow[i]);
                    }
                    else
                    {
                        num6 = 0.0;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((double) (inClose[BodyTrailingIdx] - inOpen[BodyTrailingIdx]));
            }
            else
            {
                double num4;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[BodyTrailingIdx] - inLow[BodyTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[BodyTrailingIdx] >= inOpen[BodyTrailingIdx])
                        {
                            num3 = inClose[BodyTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[BodyTrailingIdx];
                        }

                        if (inClose[BodyTrailingIdx] >= inOpen[BodyTrailingIdx])
                        {
                            num2 = inOpen[BodyTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[BodyTrailingIdx];
                        }

                        num = (inHigh[BodyTrailingIdx] - num3) + (num2 - inLow[BodyTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            BodyPeriodTotal += num10 - num5;
            i++;
            BodyTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0147;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlSpinningTop(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num5;
            float num10;
            double num14;
            double num20;
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

            int lookbackTotal = CdlSpinningTopLookback();
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

            double BodyPeriodTotal = 0.0;
            int BodyTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            int i = BodyTrailingIdx;
            while (true)
            {
                float num25;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num25 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num24;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num24 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num21;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            float num22;
                            float num23;
                            if (inClose[i] >= inOpen[i])
                            {
                                num23 = inClose[i];
                            }
                            else
                            {
                                num23 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num22 = inOpen[i];
                            }
                            else
                            {
                                num22 = inClose[i];
                            }

                            num21 = (inHigh[i] - num23) + (num22 - inLow[i]);
                        }
                        else
                        {
                            num21 = 0.0f;
                        }

                        num24 = num21;
                    }

                    num25 = num24;
                }

                BodyPeriodTotal += num25;
                i++;
            }

            int outIdx = 0;
            Label_0155:
            if (Globals.candleSettings[2].avgPeriod != 0.0)
            {
                num20 = BodyPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
            }
            else
            {
                float num19;
                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num19 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num18;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num18 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num15;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            float num16;
                            float num17;
                            if (inClose[i] >= inOpen[i])
                            {
                                num17 = inClose[i];
                            }
                            else
                            {
                                num17 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num16 = inOpen[i];
                            }
                            else
                            {
                                num16 = inClose[i];
                            }

                            num15 = (inHigh[i] - num17) + (num16 - inLow[i]);
                        }
                        else
                        {
                            num15 = 0.0f;
                        }

                        num18 = num15;
                    }

                    num19 = num18;
                }

                num20 = num19;
            }

            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
            {
                num14 = 2.0;
            }
            else
            {
                num14 = 1.0;
            }

            if (Math.Abs((float) (inClose[i] - inOpen[i])) < ((Globals.candleSettings[2].factor * num20) / num14))
            {
                float num13;
                if (inClose[i] >= inOpen[i])
                {
                    num13 = inClose[i];
                }
                else
                {
                    num13 = inOpen[i];
                }

                if ((inHigh[i] - num13) > Math.Abs((float) (inClose[i] - inOpen[i])))
                {
                    float num12;
                    if (inClose[i] >= inOpen[i])
                    {
                        num12 = inOpen[i];
                    }
                    else
                    {
                        num12 = inClose[i];
                    }

                    if ((num12 - inLow[i]) > Math.Abs((float) (inClose[i] - inOpen[i])))
                    {
                        int num11;
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
                        goto Label_0336;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0336:
            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((float) (inClose[i] - inOpen[i]));
            }
            else
            {
                float num9;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    float num6;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        float num7;
                        float num8;
                        if (inClose[i] >= inOpen[i])
                        {
                            num8 = inClose[i];
                        }
                        else
                        {
                            num8 = inOpen[i];
                        }

                        if (inClose[i] >= inOpen[i])
                        {
                            num7 = inOpen[i];
                        }
                        else
                        {
                            num7 = inClose[i];
                        }

                        num6 = (inHigh[i] - num8) + (num7 - inLow[i]);
                    }
                    else
                    {
                        num6 = 0.0f;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((float) (inClose[BodyTrailingIdx] - inOpen[BodyTrailingIdx]));
            }
            else
            {
                float num4;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[BodyTrailingIdx] - inLow[BodyTrailingIdx];
                }
                else
                {
                    float num;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        float num2;
                        float num3;
                        if (inClose[BodyTrailingIdx] >= inOpen[BodyTrailingIdx])
                        {
                            num3 = inClose[BodyTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[BodyTrailingIdx];
                        }

                        if (inClose[BodyTrailingIdx] >= inOpen[BodyTrailingIdx])
                        {
                            num2 = inOpen[BodyTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[BodyTrailingIdx];
                        }

                        num = (inHigh[BodyTrailingIdx] - num3) + (num2 - inLow[BodyTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0f;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            BodyPeriodTotal += num10 - num5;
            i++;
            BodyTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0155;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlSpinningTopLookback()
        {
            return Globals.candleSettings[2].avgPeriod;
        }
    }
}
