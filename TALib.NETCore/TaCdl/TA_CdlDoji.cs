using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlDoji(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
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

            int lookbackTotal = CdlDojiLookback();
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

            double BodyDojiPeriodTotal = 0.0;
            int BodyDojiTrailingIdx = startIdx - Globals.candleSettings[3].avgPeriod;
            int i = BodyDojiTrailingIdx;
            while (true)
            {
                double num22;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num22 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num21;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num21 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num18;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                        {
                            double num19;
                            double num20;
                            if (inClose[i] >= inOpen[i])
                            {
                                num20 = inClose[i];
                            }
                            else
                            {
                                num20 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num19 = inOpen[i];
                            }
                            else
                            {
                                num19 = inClose[i];
                            }

                            num18 = (inHigh[i] - num20) + (num19 - inLow[i]);
                        }
                        else
                        {
                            num18 = 0.0;
                        }

                        num21 = num18;
                    }

                    num22 = num21;
                }

                BodyDojiPeriodTotal += num22;
                i++;
            }

            int outIdx = 0;
            do
            {
                double num5;
                double num10;
                double num11;
                double num17;
                if (Globals.candleSettings[3].avgPeriod != 0.0)
                {
                    num17 = BodyDojiPeriodTotal / ((double) Globals.candleSettings[3].avgPeriod);
                }
                else
                {
                    double num16;
                    if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                    {
                        num16 = Math.Abs((double) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        double num15;
                        if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                        {
                            num15 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num12;
                            if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                {
                    num11 = 2.0;
                }
                else
                {
                    num11 = 1.0;
                }

                if (Math.Abs((double) (inClose[i] - inOpen[i])) <= ((Globals.candleSettings[3].factor * num17) / num11))
                {
                    outInteger[outIdx] = 100;
                    outIdx++;
                }
                else
                {
                    outInteger[outIdx] = 0;
                    outIdx++;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num9;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num6;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs((double) (inClose[BodyDojiTrailingIdx] - inOpen[BodyDojiTrailingIdx]));
                }
                else
                {
                    double num4;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[BodyDojiTrailingIdx] - inLow[BodyDojiTrailingIdx];
                    }
                    else
                    {
                        double num;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                        {
                            double num2;
                            double num3;
                            if (inClose[BodyDojiTrailingIdx] >= inOpen[BodyDojiTrailingIdx])
                            {
                                num3 = inClose[BodyDojiTrailingIdx];
                            }
                            else
                            {
                                num3 = inOpen[BodyDojiTrailingIdx];
                            }

                            if (inClose[BodyDojiTrailingIdx] >= inOpen[BodyDojiTrailingIdx])
                            {
                                num2 = inOpen[BodyDojiTrailingIdx];
                            }
                            else
                            {
                                num2 = inClose[BodyDojiTrailingIdx];
                            }

                            num = (inHigh[BodyDojiTrailingIdx] - num3) + (num2 - inLow[BodyDojiTrailingIdx]);
                        }
                        else
                        {
                            num = 0.0;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                BodyDojiPeriodTotal += num10 - num5;
                i++;
                BodyDojiTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlDoji(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
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

            int lookbackTotal = CdlDojiLookback();
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

            double BodyDojiPeriodTotal = 0.0;
            int BodyDojiTrailingIdx = startIdx - Globals.candleSettings[3].avgPeriod;
            int i = BodyDojiTrailingIdx;
            while (true)
            {
                float num22;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num22 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num21;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num21 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num18;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                        {
                            float num19;
                            float num20;
                            if (inClose[i] >= inOpen[i])
                            {
                                num20 = inClose[i];
                            }
                            else
                            {
                                num20 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num19 = inOpen[i];
                            }
                            else
                            {
                                num19 = inClose[i];
                            }

                            num18 = (inHigh[i] - num20) + (num19 - inLow[i]);
                        }
                        else
                        {
                            num18 = 0.0f;
                        }

                        num21 = num18;
                    }

                    num22 = num21;
                }

                BodyDojiPeriodTotal += num22;
                i++;
            }

            int outIdx = 0;
            do
            {
                float num5;
                float num10;
                double num11;
                double num17;
                if (Globals.candleSettings[3].avgPeriod != 0.0)
                {
                    num17 = BodyDojiPeriodTotal / ((double) Globals.candleSettings[3].avgPeriod);
                }
                else
                {
                    float num16;
                    if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                    {
                        num16 = Math.Abs((float) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        float num15;
                        if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                        {
                            num15 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            float num12;
                            if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                {
                    num11 = 2.0;
                }
                else
                {
                    num11 = 1.0;
                }

                if (Math.Abs((float) (inClose[i] - inOpen[i])) <= ((Globals.candleSettings[3].factor * num17) / num11))
                {
                    outInteger[outIdx] = 100;
                    outIdx++;
                }
                else
                {
                    outInteger[outIdx] = 0;
                    outIdx++;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num9;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num6;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs((float) (inClose[BodyDojiTrailingIdx] - inOpen[BodyDojiTrailingIdx]));
                }
                else
                {
                    float num4;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[BodyDojiTrailingIdx] - inLow[BodyDojiTrailingIdx];
                    }
                    else
                    {
                        float num;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                        {
                            float num2;
                            float num3;
                            if (inClose[BodyDojiTrailingIdx] >= inOpen[BodyDojiTrailingIdx])
                            {
                                num3 = inClose[BodyDojiTrailingIdx];
                            }
                            else
                            {
                                num3 = inOpen[BodyDojiTrailingIdx];
                            }

                            if (inClose[BodyDojiTrailingIdx] >= inOpen[BodyDojiTrailingIdx])
                            {
                                num2 = inOpen[BodyDojiTrailingIdx];
                            }
                            else
                            {
                                num2 = inClose[BodyDojiTrailingIdx];
                            }

                            num = (inHigh[BodyDojiTrailingIdx] - num3) + (num2 - inLow[BodyDojiTrailingIdx]);
                        }
                        else
                        {
                            num = 0.0f;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                BodyDojiPeriodTotal += num10 - num5;
                i++;
                BodyDojiTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlDojiLookback()
        {
            return Globals.candleSettings[3].avgPeriod;
        }
    }
}
