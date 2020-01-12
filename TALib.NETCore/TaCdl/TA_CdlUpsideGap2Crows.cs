using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlUpsideGap2Crows(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
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

            int lookbackTotal = CdlUpsideGap2CrowsLookback();
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
            double BodyShortPeriodTotal = 0.0;
            int BodyLongTrailingIdx = (startIdx - 2) - Globals.candleSettings[0].avgPeriod;
            int BodyShortTrailingIdx = (startIdx - 1) - Globals.candleSettings[2].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                double num46;
                if (i >= (startIdx - 2))
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num46 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num45;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num45 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num42;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num43;
                            double num44;
                            if (inClose[i] >= inOpen[i])
                            {
                                num44 = inClose[i];
                            }
                            else
                            {
                                num44 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num43 = inOpen[i];
                            }
                            else
                            {
                                num43 = inClose[i];
                            }

                            num42 = (inHigh[i] - num44) + (num43 - inLow[i]);
                        }
                        else
                        {
                            num42 = 0.0;
                        }

                        num45 = num42;
                    }

                    num46 = num45;
                }

                BodyLongPeriodTotal += num46;
                i++;
            }

            i = BodyShortTrailingIdx;
            while (true)
            {
                double num41;
                if (i >= (startIdx - 1))
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num41 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num40;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num40 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num37;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

                            num37 = (inHigh[i] - num39) + (num38 - inLow[i]);
                        }
                        else
                        {
                            num37 = 0.0;
                        }

                        num40 = num37;
                    }

                    num41 = num40;
                }

                BodyShortPeriodTotal += num41;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0238:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                double num30;
                double num36;
                if (Globals.candleSettings[0].avgPeriod != 0.0)
                {
                    num36 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
                }
                else
                {
                    double num35;
                    if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                    {
                        num35 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        double num34;
                        if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                        {
                            num34 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num31;
                            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                            {
                                double num32;
                                double num33;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num33 = inClose[i - 2];
                                }
                                else
                                {
                                    num33 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num32 = inOpen[i - 2];
                                }
                                else
                                {
                                    num32 = inClose[i - 2];
                                }

                                num31 = (inHigh[i - 2] - num33) + (num32 - inLow[i - 2]);
                            }
                            else
                            {
                                num31 = 0.0;
                            }

                            num34 = num31;
                        }

                        num35 = num34;
                    }

                    num36 = num35;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                {
                    num30 = 2.0;
                }
                else
                {
                    num30 = 1.0;
                }

                if ((Math.Abs((double) (inClose[i - 2] - inOpen[i - 2])) > ((Globals.candleSettings[0].factor * num36) / num30)) &&
                    (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1))
                {
                    double num23;
                    double num29;
                    if (Globals.candleSettings[2].avgPeriod != 0.0)
                    {
                        num29 = BodyShortPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
                    }
                    else
                    {
                        double num28;
                        if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                        {
                            num28 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                        }
                        else
                        {
                            double num27;
                            if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                            {
                                num27 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num24;
                                if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                {
                                    double num25;
                                    double num26;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num26 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num26 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num25 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num25 = inClose[i - 1];
                                    }

                                    num24 = (inHigh[i - 1] - num26) + (num25 - inLow[i - 1]);
                                }
                                else
                                {
                                    num24 = 0.0;
                                }

                                num27 = num24;
                            }

                            num28 = num27;
                        }

                        num29 = num28;
                    }

                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        num23 = 2.0;
                    }
                    else
                    {
                        num23 = 1.0;
                    }

                    if (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) <= ((Globals.candleSettings[2].factor * num29) / num23))
                    {
                        double num21;
                        double num22;
                        if (inOpen[i - 1] < inClose[i - 1])
                        {
                            num22 = inOpen[i - 1];
                        }
                        else
                        {
                            num22 = inClose[i - 1];
                        }

                        if (inOpen[i - 2] > inClose[i - 2])
                        {
                            num21 = inOpen[i - 2];
                        }
                        else
                        {
                            num21 = inClose[i - 2];
                        }

                        if ((((num22 > num21) && (((inClose[i] < inOpen[i]) ? -1 : 1) == -1)) &&
                             ((inOpen[i] > inOpen[i - 1]) && (inClose[i] < inClose[i - 1]))) && (inClose[i] > inClose[i - 2]))
                        {
                            outInteger[outIdx] = -100;
                            outIdx++;
                            goto Label_05B1;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_05B1:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
            }
            else
            {
                double num19;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    double num16;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num17;
                        double num18;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num18 = inClose[i - 2];
                        }
                        else
                        {
                            num18 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num17 = inOpen[i - 2];
                        }
                        else
                        {
                            num17 = inClose[i - 2];
                        }

                        num16 = (inHigh[i - 2] - num18) + (num17 - inLow[i - 2]);
                    }
                    else
                    {
                        num16 = 0.0;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs((double) (inClose[BodyLongTrailingIdx] - inOpen[BodyLongTrailingIdx]));
            }
            else
            {
                double num14;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[BodyLongTrailingIdx] - inLow[BodyLongTrailingIdx];
                }
                else
                {
                    double num11;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[BodyLongTrailingIdx] >= inOpen[BodyLongTrailingIdx])
                        {
                            num13 = inClose[BodyLongTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[BodyLongTrailingIdx];
                        }

                        if (inClose[BodyLongTrailingIdx] >= inOpen[BodyLongTrailingIdx])
                        {
                            num12 = inOpen[BodyLongTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[BodyLongTrailingIdx];
                        }

                        num11 = (inHigh[BodyLongTrailingIdx] - num13) + (num12 - inLow[BodyLongTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            BodyLongPeriodTotal += num20 - num15;
            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                double num9;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((double) (inClose[BodyShortTrailingIdx] - inOpen[BodyShortTrailingIdx]));
            }
            else
            {
                double num4;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[BodyShortTrailingIdx] - inLow[BodyShortTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[BodyShortTrailingIdx] >= inOpen[BodyShortTrailingIdx])
                        {
                            num3 = inClose[BodyShortTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[BodyShortTrailingIdx];
                        }

                        if (inClose[BodyShortTrailingIdx] >= inOpen[BodyShortTrailingIdx])
                        {
                            num2 = inOpen[BodyShortTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[BodyShortTrailingIdx];
                        }

                        num = (inHigh[BodyShortTrailingIdx] - num3) + (num2 - inLow[BodyShortTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            BodyShortPeriodTotal += num10 - num5;
            i++;
            BodyLongTrailingIdx++;
            BodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0238;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlUpsideGap2Crows(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num5;
            float num10;
            float num15;
            float num20;
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

            int lookbackTotal = CdlUpsideGap2CrowsLookback();
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
            double BodyShortPeriodTotal = 0.0;
            int BodyLongTrailingIdx = (startIdx - 2) - Globals.candleSettings[0].avgPeriod;
            int BodyShortTrailingIdx = (startIdx - 1) - Globals.candleSettings[2].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                float num46;
                if (i >= (startIdx - 2))
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num46 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num45;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num45 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num42;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num43;
                            float num44;
                            if (inClose[i] >= inOpen[i])
                            {
                                num44 = inClose[i];
                            }
                            else
                            {
                                num44 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num43 = inOpen[i];
                            }
                            else
                            {
                                num43 = inClose[i];
                            }

                            num42 = (inHigh[i] - num44) + (num43 - inLow[i]);
                        }
                        else
                        {
                            num42 = 0.0f;
                        }

                        num45 = num42;
                    }

                    num46 = num45;
                }

                BodyLongPeriodTotal += num46;
                i++;
            }

            i = BodyShortTrailingIdx;
            while (true)
            {
                float num41;
                if (i >= (startIdx - 1))
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num41 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num40;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num40 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num37;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            float num38;
                            float num39;
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

                            num37 = (inHigh[i] - num39) + (num38 - inLow[i]);
                        }
                        else
                        {
                            num37 = 0.0f;
                        }

                        num40 = num37;
                    }

                    num41 = num40;
                }

                BodyShortPeriodTotal += num41;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0254:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                double num30;
                double num36;
                if (Globals.candleSettings[0].avgPeriod != 0.0)
                {
                    num36 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
                }
                else
                {
                    float num35;
                    if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                    {
                        num35 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        float num34;
                        if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                        {
                            num34 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            float num31;
                            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                            {
                                float num32;
                                float num33;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num33 = inClose[i - 2];
                                }
                                else
                                {
                                    num33 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num32 = inOpen[i - 2];
                                }
                                else
                                {
                                    num32 = inClose[i - 2];
                                }

                                num31 = (inHigh[i - 2] - num33) + (num32 - inLow[i - 2]);
                            }
                            else
                            {
                                num31 = 0.0f;
                            }

                            num34 = num31;
                        }

                        num35 = num34;
                    }

                    num36 = num35;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                {
                    num30 = 2.0;
                }
                else
                {
                    num30 = 1.0;
                }

                if ((Math.Abs((float) (inClose[i - 2] - inOpen[i - 2])) > ((Globals.candleSettings[0].factor * num36) / num30)) &&
                    (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1))
                {
                    double num23;
                    double num29;
                    if (Globals.candleSettings[2].avgPeriod != 0.0)
                    {
                        num29 = BodyShortPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
                    }
                    else
                    {
                        float num28;
                        if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                        {
                            num28 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                        }
                        else
                        {
                            float num27;
                            if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                            {
                                num27 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                float num24;
                                if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                {
                                    float num25;
                                    float num26;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num26 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num26 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num25 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num25 = inClose[i - 1];
                                    }

                                    num24 = (inHigh[i - 1] - num26) + (num25 - inLow[i - 1]);
                                }
                                else
                                {
                                    num24 = 0.0f;
                                }

                                num27 = num24;
                            }

                            num28 = num27;
                        }

                        num29 = num28;
                    }

                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        num23 = 2.0;
                    }
                    else
                    {
                        num23 = 1.0;
                    }

                    if (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) <= ((Globals.candleSettings[2].factor * num29) / num23))
                    {
                        float num21;
                        float num22;
                        if (inOpen[i - 1] < inClose[i - 1])
                        {
                            num22 = inOpen[i - 1];
                        }
                        else
                        {
                            num22 = inClose[i - 1];
                        }

                        if (inOpen[i - 2] > inClose[i - 2])
                        {
                            num21 = inOpen[i - 2];
                        }
                        else
                        {
                            num21 = inClose[i - 2];
                        }

                        if ((((num22 > num21) && (((inClose[i] < inOpen[i]) ? -1 : 1) == -1)) &&
                             ((inOpen[i] > inOpen[i - 1]) && (inClose[i] < inClose[i - 1]))) && (inClose[i] > inClose[i - 2]))
                        {
                            outInteger[outIdx] = -100;
                            outIdx++;
                            goto Label_0607;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0607:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
            }
            else
            {
                float num19;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    float num16;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        float num17;
                        float num18;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num18 = inClose[i - 2];
                        }
                        else
                        {
                            num18 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num17 = inOpen[i - 2];
                        }
                        else
                        {
                            num17 = inClose[i - 2];
                        }

                        num16 = (inHigh[i - 2] - num18) + (num17 - inLow[i - 2]);
                    }
                    else
                    {
                        num16 = 0.0f;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs((float) (inClose[BodyLongTrailingIdx] - inOpen[BodyLongTrailingIdx]));
            }
            else
            {
                float num14;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[BodyLongTrailingIdx] - inLow[BodyLongTrailingIdx];
                }
                else
                {
                    float num11;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        float num12;
                        float num13;
                        if (inClose[BodyLongTrailingIdx] >= inOpen[BodyLongTrailingIdx])
                        {
                            num13 = inClose[BodyLongTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[BodyLongTrailingIdx];
                        }

                        if (inClose[BodyLongTrailingIdx] >= inOpen[BodyLongTrailingIdx])
                        {
                            num12 = inOpen[BodyLongTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[BodyLongTrailingIdx];
                        }

                        num11 = (inHigh[BodyLongTrailingIdx] - num13) + (num12 - inLow[BodyLongTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0f;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            BodyLongPeriodTotal += num20 - num15;
            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                float num9;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    float num6;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((float) (inClose[BodyShortTrailingIdx] - inOpen[BodyShortTrailingIdx]));
            }
            else
            {
                float num4;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[BodyShortTrailingIdx] - inLow[BodyShortTrailingIdx];
                }
                else
                {
                    float num;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        float num2;
                        float num3;
                        if (inClose[BodyShortTrailingIdx] >= inOpen[BodyShortTrailingIdx])
                        {
                            num3 = inClose[BodyShortTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[BodyShortTrailingIdx];
                        }

                        if (inClose[BodyShortTrailingIdx] >= inOpen[BodyShortTrailingIdx])
                        {
                            num2 = inOpen[BodyShortTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[BodyShortTrailingIdx];
                        }

                        num = (inHigh[BodyShortTrailingIdx] - num3) + (num2 - inLow[BodyShortTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0f;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            BodyShortPeriodTotal += num10 - num5;
            i++;
            BodyLongTrailingIdx++;
            BodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0254;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlUpsideGap2CrowsLookback()
        {
            return (((Globals.candleSettings[2].avgPeriod <= Globals.candleSettings[0].avgPeriod)
                        ? Globals.candleSettings[0].avgPeriod
                        : Globals.candleSettings[2].avgPeriod) + 2);
        }
    }
}
