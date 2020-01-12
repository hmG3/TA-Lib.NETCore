using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlHarami(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
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

            int lookbackTotal = CdlHaramiLookback();
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
            int BodyLongTrailingIdx = (startIdx - 1) - Globals.candleSettings[0].avgPeriod;
            int BodyShortTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                double num49;
                if (i >= (startIdx - 1))
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num49 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num48;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num48 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num45;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num46;
                            double num47;
                            if (inClose[i] >= inOpen[i])
                            {
                                num47 = inClose[i];
                            }
                            else
                            {
                                num47 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num46 = inOpen[i];
                            }
                            else
                            {
                                num46 = inClose[i];
                            }

                            num45 = (inHigh[i] - num47) + (num46 - inLow[i]);
                        }
                        else
                        {
                            num45 = 0.0;
                        }

                        num48 = num45;
                    }

                    num49 = num48;
                }

                BodyLongPeriodTotal += num49;
                i++;
            }

            i = BodyShortTrailingIdx;
            while (true)
            {
                double num44;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num43;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num40;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

                BodyShortPeriodTotal += num44;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0234:
            if (Globals.candleSettings[0].avgPeriod != 0.0)
            {
                num39 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
            }
            else
            {
                double num38;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num38 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num37;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num37 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num34;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num35;
                            double num36;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num36 = inClose[i - 1];
                            }
                            else
                            {
                                num36 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num35 = inOpen[i - 1];
                            }
                            else
                            {
                                num35 = inClose[i - 1];
                            }

                            num34 = (inHigh[i - 1] - num36) + (num35 - inLow[i - 1]);
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

            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
            {
                num33 = 2.0;
            }
            else
            {
                num33 = 1.0;
            }

            if (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) > ((Globals.candleSettings[0].factor * num39) / num33))
            {
                double num26;
                double num32;
                if (Globals.candleSettings[2].avgPeriod != 0.0)
                {
                    num32 = BodyShortPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
                }
                else
                {
                    double num31;
                    if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                    {
                        num31 = Math.Abs((double) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        double num30;
                        if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                        {
                            num30 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num27;
                            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                            {
                                double num28;
                                double num29;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num29 = inClose[i];
                                }
                                else
                                {
                                    num29 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num28 = inOpen[i];
                                }
                                else
                                {
                                    num28 = inClose[i];
                                }

                                num27 = (inHigh[i] - num29) + (num28 - inLow[i]);
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

                if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                {
                    num26 = 2.0;
                }
                else
                {
                    num26 = 1.0;
                }

                if (Math.Abs((double) (inClose[i] - inOpen[i])) <= ((Globals.candleSettings[2].factor * num32) / num26))
                {
                    double num24;
                    double num25;
                    if (inClose[i] > inOpen[i])
                    {
                        num25 = inClose[i];
                    }
                    else
                    {
                        num25 = inOpen[i];
                    }

                    if (inClose[i - 1] > inOpen[i - 1])
                    {
                        num24 = inClose[i - 1];
                    }
                    else
                    {
                        num24 = inOpen[i - 1];
                    }

                    if (num25 < num24)
                    {
                        double num22;
                        double num23;
                        if (inClose[i] < inOpen[i])
                        {
                            num23 = inClose[i];
                        }
                        else
                        {
                            num23 = inOpen[i];
                        }

                        if (inClose[i - 1] < inOpen[i - 1])
                        {
                            num22 = inClose[i - 1];
                        }
                        else
                        {
                            num22 = inOpen[i - 1];
                        }

                        if (num23 > num22)
                        {
                            int num21;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num21 = 1;
                            }
                            else
                            {
                                num21 = -1;
                            }

                            outInteger[outIdx] = -num21 * 100;
                            outIdx++;
                            goto Label_0575;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0575:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                double num19;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num16;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num17;
                        double num18;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num18 = inClose[i - 1];
                        }
                        else
                        {
                            num18 = inOpen[i - 1];
                        }

                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num17 = inOpen[i - 1];
                        }
                        else
                        {
                            num17 = inClose[i - 1];
                        }

                        num16 = (inHigh[i - 1] - num18) + (num17 - inLow[i - 1]);
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
                goto Label_0234;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlHarami(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num5;
            float num10;
            float num15;
            float num20;
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

            int lookbackTotal = CdlHaramiLookback();
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
            int BodyLongTrailingIdx = (startIdx - 1) - Globals.candleSettings[0].avgPeriod;
            int BodyShortTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                float num49;
                if (i >= (startIdx - 1))
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num49 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num48;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num48 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num45;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num46;
                            float num47;
                            if (inClose[i] >= inOpen[i])
                            {
                                num47 = inClose[i];
                            }
                            else
                            {
                                num47 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num46 = inOpen[i];
                            }
                            else
                            {
                                num46 = inClose[i];
                            }

                            num45 = (inHigh[i] - num47) + (num46 - inLow[i]);
                        }
                        else
                        {
                            num45 = 0.0f;
                        }

                        num48 = num45;
                    }

                    num49 = num48;
                }

                BodyLongPeriodTotal += num49;
                i++;
            }

            i = BodyShortTrailingIdx;
            while (true)
            {
                float num44;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num43;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num40;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
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

                BodyShortPeriodTotal += num44;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0250:
            if (Globals.candleSettings[0].avgPeriod != 0.0)
            {
                num39 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
            }
            else
            {
                float num38;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num38 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num37;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num37 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num34;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num35;
                            float num36;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num36 = inClose[i - 1];
                            }
                            else
                            {
                                num36 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num35 = inOpen[i - 1];
                            }
                            else
                            {
                                num35 = inClose[i - 1];
                            }

                            num34 = (inHigh[i - 1] - num36) + (num35 - inLow[i - 1]);
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

            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
            {
                num33 = 2.0;
            }
            else
            {
                num33 = 1.0;
            }

            if (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) > ((Globals.candleSettings[0].factor * num39) / num33))
            {
                double num26;
                double num32;
                if (Globals.candleSettings[2].avgPeriod != 0.0)
                {
                    num32 = BodyShortPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
                }
                else
                {
                    float num31;
                    if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                    {
                        num31 = Math.Abs((float) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        float num30;
                        if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                        {
                            num30 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            float num27;
                            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                            {
                                float num28;
                                float num29;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num29 = inClose[i];
                                }
                                else
                                {
                                    num29 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num28 = inOpen[i];
                                }
                                else
                                {
                                    num28 = inClose[i];
                                }

                                num27 = (inHigh[i] - num29) + (num28 - inLow[i]);
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

                if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                {
                    num26 = 2.0;
                }
                else
                {
                    num26 = 1.0;
                }

                if (Math.Abs((float) (inClose[i] - inOpen[i])) <= ((Globals.candleSettings[2].factor * num32) / num26))
                {
                    float num24;
                    float num25;
                    if (inClose[i] > inOpen[i])
                    {
                        num25 = inClose[i];
                    }
                    else
                    {
                        num25 = inOpen[i];
                    }

                    if (inClose[i - 1] > inOpen[i - 1])
                    {
                        num24 = inClose[i - 1];
                    }
                    else
                    {
                        num24 = inOpen[i - 1];
                    }

                    if (num25 < num24)
                    {
                        float num22;
                        float num23;
                        if (inClose[i] < inOpen[i])
                        {
                            num23 = inClose[i];
                        }
                        else
                        {
                            num23 = inOpen[i];
                        }

                        if (inClose[i - 1] < inOpen[i - 1])
                        {
                            num22 = inClose[i - 1];
                        }
                        else
                        {
                            num22 = inOpen[i - 1];
                        }

                        if (num23 > num22)
                        {
                            int num21;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num21 = 1;
                            }
                            else
                            {
                                num21 = -1;
                            }

                            outInteger[outIdx] = -num21 * 100;
                            outIdx++;
                            goto Label_05CB;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_05CB:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                float num19;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    float num16;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        float num17;
                        float num18;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num18 = inClose[i - 1];
                        }
                        else
                        {
                            num18 = inOpen[i - 1];
                        }

                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num17 = inOpen[i - 1];
                        }
                        else
                        {
                            num17 = inClose[i - 1];
                        }

                        num16 = (inHigh[i - 1] - num18) + (num17 - inLow[i - 1]);
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
                goto Label_0250;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlHaramiLookback()
        {
            return (((Globals.candleSettings[2].avgPeriod <= Globals.candleSettings[0].avgPeriod)
                        ? Globals.candleSettings[0].avgPeriod
                        : Globals.candleSettings[2].avgPeriod) + 1);
        }
    }
}
