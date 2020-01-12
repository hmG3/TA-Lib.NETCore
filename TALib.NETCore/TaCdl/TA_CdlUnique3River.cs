using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlUnique3River(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            double num28;
            double num34;
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

            int lookbackTotal = CdlUnique3RiverLookback();
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
            int BodyShortTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                double num44;
                if (i >= (startIdx - 2))
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num43;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num40;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

                BodyLongPeriodTotal += num44;
                i++;
            }

            i = BodyShortTrailingIdx;
            while (true)
            {
                double num39;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num39 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num38;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num38 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num35;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            double num36;
                            double num37;
                            if (inClose[i] >= inOpen[i])
                            {
                                num37 = inClose[i];
                            }
                            else
                            {
                                num37 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num36 = inOpen[i];
                            }
                            else
                            {
                                num36 = inClose[i];
                            }

                            num35 = (inHigh[i] - num37) + (num36 - inLow[i]);
                        }
                        else
                        {
                            num35 = 0.0;
                        }

                        num38 = num35;
                    }

                    num39 = num38;
                }

                BodyShortPeriodTotal += num39;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0234:
            if (Globals.candleSettings[0].avgPeriod != 0.0)
            {
                num34 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
            }
            else
            {
                double num33;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num33 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num32;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num32 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num29;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num30;
                            double num31;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num31 = inClose[i - 2];
                            }
                            else
                            {
                                num31 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num30 = inOpen[i - 2];
                            }
                            else
                            {
                                num30 = inClose[i - 2];
                            }

                            num29 = (inHigh[i - 2] - num31) + (num30 - inLow[i - 2]);
                        }
                        else
                        {
                            num29 = 0.0;
                        }

                        num32 = num29;
                    }

                    num33 = num32;
                }

                num34 = num33;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
            {
                num28 = 2.0;
            }
            else
            {
                num28 = 1.0;
            }

            if (((((Math.Abs((double) (inClose[i - 2] - inOpen[i - 2])) > ((Globals.candleSettings[0].factor * num34) / num28)) &&
                   (((inClose[i - 2] < inOpen[i - 2]) ? -1 : 1) == -1)) && (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1)) &&
                 ((inClose[i - 1] > inClose[i - 2]) && (inOpen[i - 1] <= inOpen[i - 2]))) && (inLow[i - 1] < inLow[i - 2]))
            {
                double num21;
                double num27;
                if (Globals.candleSettings[2].avgPeriod != 0.0)
                {
                    num27 = BodyShortPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
                }
                else
                {
                    double num26;
                    if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                    {
                        num26 = Math.Abs((double) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        double num25;
                        if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                        {
                            num25 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num22;
                            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                            {
                                double num23;
                                double num24;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num24 = inClose[i];
                                }
                                else
                                {
                                    num24 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num23 = inOpen[i];
                                }
                                else
                                {
                                    num23 = inClose[i];
                                }

                                num22 = (inHigh[i] - num24) + (num23 - inLow[i]);
                            }
                            else
                            {
                                num22 = 0.0;
                            }

                            num25 = num22;
                        }

                        num26 = num25;
                    }

                    num27 = num26;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                {
                    num21 = 2.0;
                }
                else
                {
                    num21 = 1.0;
                }

                if (((Math.Abs((double) (inClose[i] - inOpen[i])) < ((Globals.candleSettings[2].factor * num27) / num21)) &&
                     (((inClose[i] < inOpen[i]) ? -1 : 1) == 1)) && (inOpen[i] > inLow[i - 1]))
                {
                    outInteger[outIdx] = 100;
                    outIdx++;
                    goto Label_055F;
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_055F:
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

        public static RetCode CdlUnique3River(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num5;
            float num10;
            float num15;
            float num20;
            double num28;
            double num34;
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

            int lookbackTotal = CdlUnique3RiverLookback();
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
            int BodyShortTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                float num44;
                if (i >= (startIdx - 2))
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num43;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num40;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

                BodyLongPeriodTotal += num44;
                i++;
            }

            i = BodyShortTrailingIdx;
            while (true)
            {
                float num39;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num39 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num38;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num38 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num35;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            float num36;
                            float num37;
                            if (inClose[i] >= inOpen[i])
                            {
                                num37 = inClose[i];
                            }
                            else
                            {
                                num37 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num36 = inOpen[i];
                            }
                            else
                            {
                                num36 = inClose[i];
                            }

                            num35 = (inHigh[i] - num37) + (num36 - inLow[i]);
                        }
                        else
                        {
                            num35 = 0.0f;
                        }

                        num38 = num35;
                    }

                    num39 = num38;
                }

                BodyShortPeriodTotal += num39;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0250:
            if (Globals.candleSettings[0].avgPeriod != 0.0)
            {
                num34 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
            }
            else
            {
                float num33;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num33 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num32;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num32 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num29;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num30;
                            float num31;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num31 = inClose[i - 2];
                            }
                            else
                            {
                                num31 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num30 = inOpen[i - 2];
                            }
                            else
                            {
                                num30 = inClose[i - 2];
                            }

                            num29 = (inHigh[i - 2] - num31) + (num30 - inLow[i - 2]);
                        }
                        else
                        {
                            num29 = 0.0f;
                        }

                        num32 = num29;
                    }

                    num33 = num32;
                }

                num34 = num33;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
            {
                num28 = 2.0;
            }
            else
            {
                num28 = 1.0;
            }

            if (((((Math.Abs((float) (inClose[i - 2] - inOpen[i - 2])) > ((Globals.candleSettings[0].factor * num34) / num28)) &&
                   (((inClose[i - 2] < inOpen[i - 2]) ? -1 : 1) == -1)) && (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1)) &&
                 ((inClose[i - 1] > inClose[i - 2]) && (inOpen[i - 1] <= inOpen[i - 2]))) && (inLow[i - 1] < inLow[i - 2]))
            {
                double num21;
                double num27;
                if (Globals.candleSettings[2].avgPeriod != 0.0)
                {
                    num27 = BodyShortPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
                }
                else
                {
                    float num26;
                    if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                    {
                        num26 = Math.Abs((float) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        float num25;
                        if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                        {
                            num25 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            float num22;
                            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                            {
                                float num23;
                                float num24;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num24 = inClose[i];
                                }
                                else
                                {
                                    num24 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num23 = inOpen[i];
                                }
                                else
                                {
                                    num23 = inClose[i];
                                }

                                num22 = (inHigh[i] - num24) + (num23 - inLow[i]);
                            }
                            else
                            {
                                num22 = 0.0f;
                            }

                            num25 = num22;
                        }

                        num26 = num25;
                    }

                    num27 = num26;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                {
                    num21 = 2.0;
                }
                else
                {
                    num21 = 1.0;
                }

                if (((Math.Abs((float) (inClose[i] - inOpen[i])) < ((Globals.candleSettings[2].factor * num27) / num21)) &&
                     (((inClose[i] < inOpen[i]) ? -1 : 1) == 1)) && (inOpen[i] > inLow[i - 1]))
                {
                    outInteger[outIdx] = 100;
                    outIdx++;
                    goto Label_05AD;
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_05AD:
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

        public static int CdlUnique3RiverLookback()
        {
            return (((Globals.candleSettings[2].avgPeriod <= Globals.candleSettings[0].avgPeriod)
                        ? Globals.candleSettings[0].avgPeriod
                        : Globals.candleSettings[2].avgPeriod) + 2);
        }
    }
}
