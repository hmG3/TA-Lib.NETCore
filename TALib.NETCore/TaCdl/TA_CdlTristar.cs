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
            double num33;
            double num39;
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

            double BodyPeriodTotal = 0.0;
            int BodyTrailingIdx = (startIdx - 2) - Globals.candleSettings[3].avgPeriod;
            int i = BodyTrailingIdx;
            while (true)
            {
                double num44;
                if (i >= (startIdx - 2))
                {
                    break;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num43;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num40;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

                            num40 = (inHigh[i] - num42) + (num41 - inLow[i]);
                        }
                        else
                        {
                            num40 = 0.0;
                        }

                        num43 = num40;
                    }

                    num44 = num43;
                }

                BodyPeriodTotal += num44;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_014D:
            if (Globals.candleSettings[3].avgPeriod != 0.0)
            {
                num39 = BodyPeriodTotal / ((double) Globals.candleSettings[3].avgPeriod);
            }
            else
            {
                double num38;
                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num38 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num37;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num37 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num34;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

                            num34 = (inHigh[i - 2] - num36) + (num35 - inLow[i - 2]);
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

            if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
            {
                num33 = 2.0;
            }
            else
            {
                num33 = 1.0;
            }

            if (Math.Abs((double) (inClose[i - 2] - inOpen[i - 2])) <= ((Globals.candleSettings[3].factor * num39) / num33))
            {
                double num26;
                double num32;
                if (Globals.candleSettings[3].avgPeriod != 0.0)
                {
                    num32 = BodyPeriodTotal / ((double) Globals.candleSettings[3].avgPeriod);
                }
                else
                {
                    double num31;
                    if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                    {
                        num31 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        double num30;
                        if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                        {
                            num30 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num27;
                            if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

                                num27 = (inHigh[i - 2] - num29) + (num28 - inLow[i - 2]);
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

                if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                {
                    num26 = 2.0;
                }
                else
                {
                    num26 = 1.0;
                }

                if (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) <= ((Globals.candleSettings[3].factor * num32) / num26))
                {
                    double num19;
                    double num25;
                    if (Globals.candleSettings[3].avgPeriod != 0.0)
                    {
                        num25 = BodyPeriodTotal / ((double) Globals.candleSettings[3].avgPeriod);
                    }
                    else
                    {
                        double num24;
                        if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                        {
                            num24 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                        }
                        else
                        {
                            double num23;
                            if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                            {
                                num23 = inHigh[i - 2] - inLow[i - 2];
                            }
                            else
                            {
                                double num20;
                                if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

                                    num20 = (inHigh[i - 2] - num22) + (num21 - inLow[i - 2]);
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

                    if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                    {
                        num19 = 2.0;
                    }
                    else
                    {
                        num19 = 1.0;
                    }

                    if (Math.Abs((double) (inClose[i] - inOpen[i])) <= ((Globals.candleSettings[3].factor * num25) / num19))
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
            if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
            }
            else
            {
                double num9;
                if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((double) (inClose[BodyTrailingIdx] - inOpen[BodyTrailingIdx]));
            }
            else
            {
                double num4;
                if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[BodyTrailingIdx] - inLow[BodyTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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
                goto Label_014D;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlTristar(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num5;
            float num10;
            double num33;
            double num39;
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

            double BodyPeriodTotal = 0.0;
            int BodyTrailingIdx = (startIdx - 2) - Globals.candleSettings[3].avgPeriod;
            int i = BodyTrailingIdx;
            while (true)
            {
                float num44;
                if (i >= (startIdx - 2))
                {
                    break;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num43;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num40;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                        {
                            float num41;
                            float num42;
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

                            num40 = (inHigh[i] - num42) + (num41 - inLow[i]);
                        }
                        else
                        {
                            num40 = 0.0f;
                        }

                        num43 = num40;
                    }

                    num44 = num43;
                }

                BodyPeriodTotal += num44;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_015B:
            if (Globals.candleSettings[3].avgPeriod != 0.0)
            {
                num39 = BodyPeriodTotal / ((double) Globals.candleSettings[3].avgPeriod);
            }
            else
            {
                float num38;
                if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                {
                    num38 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num37;
                    if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                    {
                        num37 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num34;
                        if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                        {
                            float num35;
                            float num36;
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

                            num34 = (inHigh[i - 2] - num36) + (num35 - inLow[i - 2]);
                        }
                        else
                        {
                            num34 = 0.0f;
                        }

                        num37 = num34;
                    }

                    num38 = num37;
                }

                num39 = num38;
            }

            if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
            {
                num33 = 2.0;
            }
            else
            {
                num33 = 1.0;
            }

            if (Math.Abs((float) (inClose[i - 2] - inOpen[i - 2])) <= ((Globals.candleSettings[3].factor * num39) / num33))
            {
                double num26;
                double num32;
                if (Globals.candleSettings[3].avgPeriod != 0.0)
                {
                    num32 = BodyPeriodTotal / ((double) Globals.candleSettings[3].avgPeriod);
                }
                else
                {
                    float num31;
                    if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                    {
                        num31 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        float num30;
                        if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                        {
                            num30 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            float num27;
                            if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                            {
                                float num28;
                                float num29;
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

                                num27 = (inHigh[i - 2] - num29) + (num28 - inLow[i - 2]);
                            }
                            else
                            {
                                num27 = 0.0f;
                            }

                            num30 = num27;
                        }

                        num31 = num30;
                    }

                    num32 = num31;
                }

                if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                {
                    num26 = 2.0;
                }
                else
                {
                    num26 = 1.0;
                }

                if (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) <= ((Globals.candleSettings[3].factor * num32) / num26))
                {
                    double num19;
                    double num25;
                    if (Globals.candleSettings[3].avgPeriod != 0.0)
                    {
                        num25 = BodyPeriodTotal / ((double) Globals.candleSettings[3].avgPeriod);
                    }
                    else
                    {
                        float num24;
                        if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
                        {
                            num24 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                        }
                        else
                        {
                            float num23;
                            if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                            {
                                num23 = inHigh[i - 2] - inLow[i - 2];
                            }
                            else
                            {
                                float num20;
                                if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                                {
                                    float num21;
                                    float num22;
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

                                    num20 = (inHigh[i - 2] - num22) + (num21 - inLow[i - 2]);
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

                    if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
                    {
                        num19 = 2.0;
                    }
                    else
                    {
                        num19 = 1.0;
                    }

                    if (Math.Abs((float) (inClose[i] - inOpen[i])) <= ((Globals.candleSettings[3].factor * num25) / num19))
                    {
                        float num13;
                        float num14;
                        float num17;
                        float num18;
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
                            float num15;
                            float num16;
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
                            float num11;
                            float num12;
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
            if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
            }
            else
            {
                float num9;
                if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    float num6;
                    if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[3].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((float) (inClose[BodyTrailingIdx] - inOpen[BodyTrailingIdx]));
            }
            else
            {
                float num4;
                if (Globals.candleSettings[3].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[BodyTrailingIdx] - inLow[BodyTrailingIdx];
                }
                else
                {
                    float num;
                    if (Globals.candleSettings[3].rangeType == RangeType.Shadows)
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
                goto Label_015B;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlTristarLookback()
        {
            return (Globals.candleSettings[3].avgPeriod + 2);
        }
    }
}
