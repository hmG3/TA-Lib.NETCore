using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Cdl3BlackCrows(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int totIdx;
            double[] ShadowVeryShortPeriodTotal = new double[3];
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

            int lookbackTotal = Cdl3BlackCrowsLookback();
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

            ShadowVeryShortPeriodTotal[2] = 0.0;
            ShadowVeryShortPeriodTotal[1] = 0.0;
            ShadowVeryShortPeriodTotal[0] = 0.0;
            int ShadowVeryShortTrailingIdx = startIdx - Globals.candleSettings[7].avgPeriod;
            int i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                double num39;
                double num44;
                double num49;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num49 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num48;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num48 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num45;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num46;
                            double num47;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num47 = inClose[i - 2];
                            }
                            else
                            {
                                num47 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num46 = inOpen[i - 2];
                            }
                            else
                            {
                                num46 = inClose[i - 2];
                            }

                            num45 = (inHigh[i - 2] - num47) + (num46 - inLow[i - 2]);
                        }
                        else
                        {
                            num45 = 0.0;
                        }

                        num48 = num45;
                    }

                    num49 = num48;
                }

                ShadowVeryShortPeriodTotal[2] += num49;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num43;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num40;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num41;
                            double num42;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num42 = inClose[i - 1];
                            }
                            else
                            {
                                num42 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num41 = inOpen[i - 1];
                            }
                            else
                            {
                                num41 = inClose[i - 1];
                            }

                            num40 = (inHigh[i - 1] - num42) + (num41 - inLow[i - 1]);
                        }
                        else
                        {
                            num40 = 0.0;
                        }

                        num43 = num40;
                    }

                    num44 = num43;
                }

                ShadowVeryShortPeriodTotal[1] += num44;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num39 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num38;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num38 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num35;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                ShadowVeryShortPeriodTotal[0] += num39;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0317:
            if ((inClose[i - 3] >= inOpen[i - 3]) && (((inClose[i - 2] < inOpen[i - 2]) ? -1 : 1) == -1))
            {
                double num27;
                double num33;
                double num34;
                if (inClose[i - 2] >= inOpen[i - 2])
                {
                    num34 = inOpen[i - 2];
                }
                else
                {
                    num34 = inClose[i - 2];
                }

                if (Globals.candleSettings[7].avgPeriod != 0.0)
                {
                    num33 = ShadowVeryShortPeriodTotal[2] / ((double) Globals.candleSettings[7].avgPeriod);
                }
                else
                {
                    double num32;
                    if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                    {
                        num32 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        double num31;
                        if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                        {
                            num31 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num28;
                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                            {
                                double num29;
                                double num30;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num30 = inClose[i - 2];
                                }
                                else
                                {
                                    num30 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num29 = inOpen[i - 2];
                                }
                                else
                                {
                                    num29 = inClose[i - 2];
                                }

                                num28 = (inHigh[i - 2] - num30) + (num29 - inLow[i - 2]);
                            }
                            else
                            {
                                num28 = 0.0;
                            }

                            num31 = num28;
                        }

                        num32 = num31;
                    }

                    num33 = num32;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                {
                    num27 = 2.0;
                }
                else
                {
                    num27 = 1.0;
                }

                if (((num34 - inLow[i - 2]) < ((Globals.candleSettings[7].factor * num33) / num27)) &&
                    (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1))
                {
                    double num19;
                    double num25;
                    double num26;
                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num26 = inOpen[i - 1];
                    }
                    else
                    {
                        num26 = inClose[i - 1];
                    }

                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                    {
                        num25 = ShadowVeryShortPeriodTotal[1] / ((double) Globals.candleSettings[7].avgPeriod);
                    }
                    else
                    {
                        double num24;
                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                        {
                            num24 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                        }
                        else
                        {
                            double num23;
                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                            {
                                num23 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                double num20;
                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                {
                                    double num21;
                                    double num22;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num22 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num22 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num21 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num21 = inClose[i - 1];
                                    }

                                    num20 = (inHigh[i - 1] - num22) + (num21 - inLow[i - 1]);
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

                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        num19 = 2.0;
                    }
                    else
                    {
                        num19 = 1.0;
                    }

                    if (((num26 - inLow[i - 1]) < ((Globals.candleSettings[7].factor * num25) / num19)) &&
                        (((inClose[i] < inOpen[i]) ? -1 : 1) == -1))
                    {
                        double num11;
                        double num17;
                        double num18;
                        if (inClose[i] >= inOpen[i])
                        {
                            num18 = inOpen[i];
                        }
                        else
                        {
                            num18 = inClose[i];
                        }

                        if (Globals.candleSettings[7].avgPeriod != 0.0)
                        {
                            num17 = ShadowVeryShortPeriodTotal[0] / ((double) Globals.candleSettings[7].avgPeriod);
                        }
                        else
                        {
                            double num16;
                            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                            {
                                num16 = Math.Abs((double) (inClose[i] - inOpen[i]));
                            }
                            else
                            {
                                double num15;
                                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                {
                                    num15 = inHigh[i] - inLow[i];
                                }
                                else
                                {
                                    double num12;
                                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            num11 = 2.0;
                        }
                        else
                        {
                            num11 = 1.0;
                        }

                        if (((((num18 - inLow[i]) < ((Globals.candleSettings[7].factor * num17) / num11)) &&
                              (inOpen[i - 1] < inOpen[i - 2])) && ((inOpen[i - 1] > inClose[i - 2]) && (inOpen[i] < inOpen[i - 1]))) &&
                            (((inOpen[i] > inClose[i - 1]) && (inHigh[i - 3] > inClose[i - 2])) &&
                             ((inClose[i - 2] > inClose[i - 1]) && (inClose[i - 1] > inClose[i]))))
                        {
                            outInteger[outIdx] = -100;
                            outIdx++;
                            goto Label_081B;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_081B:
            totIdx = 2;
            while (totIdx >= 0)
            {
                double num5;
                double num10;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs((double) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    double num9;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num6;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs((double) (inClose[ShadowVeryShortTrailingIdx - totIdx] - inOpen[ShadowVeryShortTrailingIdx - totIdx]));
                }
                else
                {
                    double num4;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[ShadowVeryShortTrailingIdx - totIdx] - inLow[ShadowVeryShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num2;
                            double num3;
                            if (inClose[ShadowVeryShortTrailingIdx - totIdx] >= inOpen[ShadowVeryShortTrailingIdx - totIdx])
                            {
                                num3 = inClose[ShadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[ShadowVeryShortTrailingIdx - totIdx];
                            }

                            if (inClose[ShadowVeryShortTrailingIdx - totIdx] >= inOpen[ShadowVeryShortTrailingIdx - totIdx])
                            {
                                num2 = inOpen[ShadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[ShadowVeryShortTrailingIdx - totIdx];
                            }

                            num = (inHigh[ShadowVeryShortTrailingIdx - totIdx] - num3) +
                                  (num2 - inLow[ShadowVeryShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                ShadowVeryShortPeriodTotal[totIdx] += num10 - num5;
                totIdx--;
            }

            i++;
            ShadowVeryShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0317;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Cdl3BlackCrows(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int totIdx;
            double[] ShadowVeryShortPeriodTotal = new double[3];
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

            int lookbackTotal = Cdl3BlackCrowsLookback();
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

            ShadowVeryShortPeriodTotal[2] = 0.0;
            ShadowVeryShortPeriodTotal[1] = 0.0;
            ShadowVeryShortPeriodTotal[0] = 0.0;
            int ShadowVeryShortTrailingIdx = startIdx - Globals.candleSettings[7].avgPeriod;
            int i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                float num39;
                float num44;
                float num49;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num49 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num48;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num48 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num45;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num46;
                            float num47;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num47 = inClose[i - 2];
                            }
                            else
                            {
                                num47 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num46 = inOpen[i - 2];
                            }
                            else
                            {
                                num46 = inClose[i - 2];
                            }

                            num45 = (inHigh[i - 2] - num47) + (num46 - inLow[i - 2]);
                        }
                        else
                        {
                            num45 = 0.0f;
                        }

                        num48 = num45;
                    }

                    num49 = num48;
                }

                ShadowVeryShortPeriodTotal[2] += num49;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num44 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num43;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num43 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num40;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num41;
                            float num42;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num42 = inClose[i - 1];
                            }
                            else
                            {
                                num42 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num41 = inOpen[i - 1];
                            }
                            else
                            {
                                num41 = inClose[i - 1];
                            }

                            num40 = (inHigh[i - 1] - num42) + (num41 - inLow[i - 1]);
                        }
                        else
                        {
                            num40 = 0.0f;
                        }

                        num43 = num40;
                    }

                    num44 = num43;
                }

                ShadowVeryShortPeriodTotal[1] += num44;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num39 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num38;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num38 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num35;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                ShadowVeryShortPeriodTotal[0] += num39;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0341:
            if ((inClose[i - 3] >= inOpen[i - 3]) && (((inClose[i - 2] < inOpen[i - 2]) ? -1 : 1) == -1))
            {
                double num27;
                double num33;
                float num34;
                if (inClose[i - 2] >= inOpen[i - 2])
                {
                    num34 = inOpen[i - 2];
                }
                else
                {
                    num34 = inClose[i - 2];
                }

                if (Globals.candleSettings[7].avgPeriod != 0.0)
                {
                    num33 = ShadowVeryShortPeriodTotal[2] / ((double) Globals.candleSettings[7].avgPeriod);
                }
                else
                {
                    float num32;
                    if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                    {
                        num32 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        float num31;
                        if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                        {
                            num31 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            float num28;
                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                            {
                                float num29;
                                float num30;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num30 = inClose[i - 2];
                                }
                                else
                                {
                                    num30 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num29 = inOpen[i - 2];
                                }
                                else
                                {
                                    num29 = inClose[i - 2];
                                }

                                num28 = (inHigh[i - 2] - num30) + (num29 - inLow[i - 2]);
                            }
                            else
                            {
                                num28 = 0.0f;
                            }

                            num31 = num28;
                        }

                        num32 = num31;
                    }

                    num33 = num32;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                {
                    num27 = 2.0;
                }
                else
                {
                    num27 = 1.0;
                }

                if (((num34 - inLow[i - 2]) < ((Globals.candleSettings[7].factor * num33) / num27)) &&
                    (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1))
                {
                    double num19;
                    double num25;
                    float num26;
                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num26 = inOpen[i - 1];
                    }
                    else
                    {
                        num26 = inClose[i - 1];
                    }

                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                    {
                        num25 = ShadowVeryShortPeriodTotal[1] / ((double) Globals.candleSettings[7].avgPeriod);
                    }
                    else
                    {
                        float num24;
                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                        {
                            num24 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                        }
                        else
                        {
                            float num23;
                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                            {
                                num23 = inHigh[i - 1] - inLow[i - 1];
                            }
                            else
                            {
                                float num20;
                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                {
                                    float num21;
                                    float num22;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num22 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num22 = inOpen[i - 1];
                                    }

                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num21 = inOpen[i - 1];
                                    }
                                    else
                                    {
                                        num21 = inClose[i - 1];
                                    }

                                    num20 = (inHigh[i - 1] - num22) + (num21 - inLow[i - 1]);
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

                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        num19 = 2.0;
                    }
                    else
                    {
                        num19 = 1.0;
                    }

                    if (((num26 - inLow[i - 1]) < ((Globals.candleSettings[7].factor * num25) / num19)) &&
                        (((inClose[i] < inOpen[i]) ? -1 : 1) == -1))
                    {
                        double num11;
                        double num17;
                        float num18;
                        if (inClose[i] >= inOpen[i])
                        {
                            num18 = inOpen[i];
                        }
                        else
                        {
                            num18 = inClose[i];
                        }

                        if (Globals.candleSettings[7].avgPeriod != 0.0)
                        {
                            num17 = ShadowVeryShortPeriodTotal[0] / ((double) Globals.candleSettings[7].avgPeriod);
                        }
                        else
                        {
                            float num16;
                            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                            {
                                num16 = Math.Abs((float) (inClose[i] - inOpen[i]));
                            }
                            else
                            {
                                float num15;
                                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                {
                                    num15 = inHigh[i] - inLow[i];
                                }
                                else
                                {
                                    float num12;
                                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            num11 = 2.0;
                        }
                        else
                        {
                            num11 = 1.0;
                        }

                        if (((((num18 - inLow[i]) < ((Globals.candleSettings[7].factor * num17) / num11)) &&
                              (inOpen[i - 1] < inOpen[i - 2])) && ((inOpen[i - 1] > inClose[i - 2]) && (inOpen[i] < inOpen[i - 1]))) &&
                            (((inOpen[i] > inClose[i - 1]) && (inHigh[i - 3] > inClose[i - 2])) &&
                             ((inClose[i - 2] > inClose[i - 1]) && (inClose[i - 1] > inClose[i]))))
                        {
                            outInteger[outIdx] = -100;
                            outIdx++;
                            goto Label_0891;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0891:
            totIdx = 2;
            while (totIdx >= 0)
            {
                float num5;
                float num10;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs((float) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    float num9;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        float num6;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs((float) (inClose[ShadowVeryShortTrailingIdx - totIdx] - inOpen[ShadowVeryShortTrailingIdx - totIdx]));
                }
                else
                {
                    float num4;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[ShadowVeryShortTrailingIdx - totIdx] - inLow[ShadowVeryShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        float num;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num2;
                            float num3;
                            if (inClose[ShadowVeryShortTrailingIdx - totIdx] >= inOpen[ShadowVeryShortTrailingIdx - totIdx])
                            {
                                num3 = inClose[ShadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[ShadowVeryShortTrailingIdx - totIdx];
                            }

                            if (inClose[ShadowVeryShortTrailingIdx - totIdx] >= inOpen[ShadowVeryShortTrailingIdx - totIdx])
                            {
                                num2 = inOpen[ShadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[ShadowVeryShortTrailingIdx - totIdx];
                            }

                            num = (inHigh[ShadowVeryShortTrailingIdx - totIdx] - num3) +
                                  (num2 - inLow[ShadowVeryShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0f;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                ShadowVeryShortPeriodTotal[totIdx] += num10 - num5;
                totIdx--;
            }

            i++;
            ShadowVeryShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0341;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int Cdl3BlackCrowsLookback()
        {
            return (Globals.candleSettings[7].avgPeriod + 3);
        }
    }
}
