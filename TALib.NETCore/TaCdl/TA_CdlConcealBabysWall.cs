using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlConcealBabysWall(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int totIdx;
            double[] ShadowVeryShortPeriodTotal = new double[4];
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

            int lookbackTotal = CdlConcealBabysWallLookback();
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

            ShadowVeryShortPeriodTotal[3] = 0.0;
            ShadowVeryShortPeriodTotal[2] = 0.0;
            ShadowVeryShortPeriodTotal[1] = 0.0;
            int ShadowVeryShortTrailingIdx = startIdx - Globals.candleSettings[7].avgPeriod;
            int i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                double num57;
                double num62;
                double num67;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num67 = Math.Abs((double) (inClose[i - 3] - inOpen[i - 3]));
                }
                else
                {
                    double num66;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num66 = inHigh[i - 3] - inLow[i - 3];
                    }
                    else
                    {
                        double num63;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num64;
                            double num65;
                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num65 = inClose[i - 3];
                            }
                            else
                            {
                                num65 = inOpen[i - 3];
                            }

                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num64 = inOpen[i - 3];
                            }
                            else
                            {
                                num64 = inClose[i - 3];
                            }

                            num63 = (inHigh[i - 3] - num65) + (num64 - inLow[i - 3]);
                        }
                        else
                        {
                            num63 = 0.0;
                        }

                        num66 = num63;
                    }

                    num67 = num66;
                }

                ShadowVeryShortPeriodTotal[3] += num67;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num62 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num61;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num61 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num58;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num59;
                            double num60;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num60 = inClose[i - 2];
                            }
                            else
                            {
                                num60 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num59 = inOpen[i - 2];
                            }
                            else
                            {
                                num59 = inClose[i - 2];
                            }

                            num58 = (inHigh[i - 2] - num60) + (num59 - inLow[i - 2]);
                        }
                        else
                        {
                            num58 = 0.0;
                        }

                        num61 = num58;
                    }

                    num62 = num61;
                }

                ShadowVeryShortPeriodTotal[2] += num62;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num57 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num56;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num56 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num53;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num54;
                            double num55;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num55 = inClose[i - 1];
                            }
                            else
                            {
                                num55 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num54 = inOpen[i - 1];
                            }
                            else
                            {
                                num54 = inClose[i - 1];
                            }

                            num53 = (inHigh[i - 1] - num55) + (num54 - inLow[i - 1]);
                        }
                        else
                        {
                            num53 = 0.0;
                        }

                        num56 = num53;
                    }

                    num57 = num56;
                }

                ShadowVeryShortPeriodTotal[1] += num57;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0336:
            if ((((((inClose[i - 3] < inOpen[i - 3]) ? -1 : 1) == -1) && (((inClose[i - 2] < inOpen[i - 2]) ? -1 : 1) == -1)) &&
                 (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1)) && (((inClose[i] < inOpen[i]) ? -1 : 1) == -1))
            {
                double num45;
                double num51;
                double num52;
                if (inClose[i - 3] >= inOpen[i - 3])
                {
                    num52 = inOpen[i - 3];
                }
                else
                {
                    num52 = inClose[i - 3];
                }

                if (Globals.candleSettings[7].avgPeriod != 0.0)
                {
                    num51 = ShadowVeryShortPeriodTotal[3] / ((double) Globals.candleSettings[7].avgPeriod);
                }
                else
                {
                    double num50;
                    if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                    {
                        num50 = Math.Abs((double) (inClose[i - 3] - inOpen[i - 3]));
                    }
                    else
                    {
                        double num49;
                        if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                        {
                            num49 = inHigh[i - 3] - inLow[i - 3];
                        }
                        else
                        {
                            double num46;
                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                            {
                                double num47;
                                double num48;
                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num48 = inClose[i - 3];
                                }
                                else
                                {
                                    num48 = inOpen[i - 3];
                                }

                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num47 = inOpen[i - 3];
                                }
                                else
                                {
                                    num47 = inClose[i - 3];
                                }

                                num46 = (inHigh[i - 3] - num48) + (num47 - inLow[i - 3]);
                            }
                            else
                            {
                                num46 = 0.0;
                            }

                            num49 = num46;
                        }

                        num50 = num49;
                    }

                    num51 = num50;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                {
                    num45 = 2.0;
                }
                else
                {
                    num45 = 1.0;
                }

                if ((num52 - inLow[i - 3]) < ((Globals.candleSettings[7].factor * num51) / num45))
                {
                    double num37;
                    double num43;
                    double num44;
                    if (inClose[i - 3] >= inOpen[i - 3])
                    {
                        num44 = inClose[i - 3];
                    }
                    else
                    {
                        num44 = inOpen[i - 3];
                    }

                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                    {
                        num43 = ShadowVeryShortPeriodTotal[3] / ((double) Globals.candleSettings[7].avgPeriod);
                    }
                    else
                    {
                        double num42;
                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                        {
                            num42 = Math.Abs((double) (inClose[i - 3] - inOpen[i - 3]));
                        }
                        else
                        {
                            double num41;
                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                            {
                                num41 = inHigh[i - 3] - inLow[i - 3];
                            }
                            else
                            {
                                double num38;
                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                {
                                    double num39;
                                    double num40;
                                    if (inClose[i - 3] >= inOpen[i - 3])
                                    {
                                        num40 = inClose[i - 3];
                                    }
                                    else
                                    {
                                        num40 = inOpen[i - 3];
                                    }

                                    if (inClose[i - 3] >= inOpen[i - 3])
                                    {
                                        num39 = inOpen[i - 3];
                                    }
                                    else
                                    {
                                        num39 = inClose[i - 3];
                                    }

                                    num38 = (inHigh[i - 3] - num40) + (num39 - inLow[i - 3]);
                                }
                                else
                                {
                                    num38 = 0.0;
                                }

                                num41 = num38;
                            }

                            num42 = num41;
                        }

                        num43 = num42;
                    }

                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        num37 = 2.0;
                    }
                    else
                    {
                        num37 = 1.0;
                    }

                    if ((inHigh[i - 3] - num44) < ((Globals.candleSettings[7].factor * num43) / num37))
                    {
                        double num29;
                        double num35;
                        double num36;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num36 = inOpen[i - 2];
                        }
                        else
                        {
                            num36 = inClose[i - 2];
                        }

                        if (Globals.candleSettings[7].avgPeriod != 0.0)
                        {
                            num35 = ShadowVeryShortPeriodTotal[2] / ((double) Globals.candleSettings[7].avgPeriod);
                        }
                        else
                        {
                            double num34;
                            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                            {
                                num34 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                            }
                            else
                            {
                                double num33;
                                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                {
                                    num33 = inHigh[i - 2] - inLow[i - 2];
                                }
                                else
                                {
                                    double num30;
                                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                    {
                                        double num31;
                                        double num32;
                                        if (inClose[i - 2] >= inOpen[i - 2])
                                        {
                                            num32 = inClose[i - 2];
                                        }
                                        else
                                        {
                                            num32 = inOpen[i - 2];
                                        }

                                        if (inClose[i - 2] >= inOpen[i - 2])
                                        {
                                            num31 = inOpen[i - 2];
                                        }
                                        else
                                        {
                                            num31 = inClose[i - 2];
                                        }

                                        num30 = (inHigh[i - 2] - num32) + (num31 - inLow[i - 2]);
                                    }
                                    else
                                    {
                                        num30 = 0.0;
                                    }

                                    num33 = num30;
                                }

                                num34 = num33;
                            }

                            num35 = num34;
                        }

                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            num29 = 2.0;
                        }
                        else
                        {
                            num29 = 1.0;
                        }

                        if ((num36 - inLow[i - 2]) < ((Globals.candleSettings[7].factor * num35) / num29))
                        {
                            double num21;
                            double num27;
                            double num28;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num28 = inClose[i - 2];
                            }
                            else
                            {
                                num28 = inOpen[i - 2];
                            }

                            if (Globals.candleSettings[7].avgPeriod != 0.0)
                            {
                                num27 = ShadowVeryShortPeriodTotal[2] / ((double) Globals.candleSettings[7].avgPeriod);
                            }
                            else
                            {
                                double num26;
                                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                                {
                                    num26 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                                }
                                else
                                {
                                    double num25;
                                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                    {
                                        num25 = inHigh[i - 2] - inLow[i - 2];
                                    }
                                    else
                                    {
                                        double num22;
                                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                        {
                                            double num23;
                                            double num24;
                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num24 = inClose[i - 2];
                                            }
                                            else
                                            {
                                                num24 = inOpen[i - 2];
                                            }

                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num23 = inOpen[i - 2];
                                            }
                                            else
                                            {
                                                num23 = inClose[i - 2];
                                            }

                                            num22 = (inHigh[i - 2] - num24) + (num23 - inLow[i - 2]);
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

                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                            {
                                num21 = 2.0;
                            }
                            else
                            {
                                num21 = 1.0;
                            }

                            if ((inHigh[i - 2] - num28) < ((Globals.candleSettings[7].factor * num27) / num21))
                            {
                                double num19;
                                double num20;
                                if (inOpen[i - 1] > inClose[i - 1])
                                {
                                    num20 = inOpen[i - 1];
                                }
                                else
                                {
                                    num20 = inClose[i - 1];
                                }

                                if (inOpen[i - 2] < inClose[i - 2])
                                {
                                    num19 = inOpen[i - 2];
                                }
                                else
                                {
                                    num19 = inClose[i - 2];
                                }

                                if (num20 < num19)
                                {
                                    double num11;
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

                                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                                    {
                                        num17 = ShadowVeryShortPeriodTotal[1] / ((double) Globals.candleSettings[7].avgPeriod);
                                    }
                                    else
                                    {
                                        double num16;
                                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                                        {
                                            num16 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                                        }
                                        else
                                        {
                                            double num15;
                                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                            {
                                                num15 = inHigh[i - 1] - inLow[i - 1];
                                            }
                                            else
                                            {
                                                double num12;
                                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                                {
                                                    double num13;
                                                    double num14;
                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num14 = inClose[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num14 = inOpen[i - 1];
                                                    }

                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num13 = inOpen[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num13 = inClose[i - 1];
                                                    }

                                                    num12 = (inHigh[i - 1] - num14) + (num13 - inLow[i - 1]);
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

                                    if ((((inHigh[i - 1] - num18) > ((Globals.candleSettings[7].factor * num17) / num11)) &&
                                         (inHigh[i - 1] > inClose[i - 2])) && ((inHigh[i] > inHigh[i - 1]) && (inLow[i] < inLow[i - 1])))
                                    {
                                        outInteger[outIdx] = 100;
                                        outIdx++;
                                        goto Label_0B63;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0B63:
            totIdx = 3;
            while (totIdx >= 1)
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
                goto Label_0336;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlConcealBabysWall(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int totIdx;
            double[] ShadowVeryShortPeriodTotal = new double[4];
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

            int lookbackTotal = CdlConcealBabysWallLookback();
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

            ShadowVeryShortPeriodTotal[3] = 0.0;
            ShadowVeryShortPeriodTotal[2] = 0.0;
            ShadowVeryShortPeriodTotal[1] = 0.0;
            int ShadowVeryShortTrailingIdx = startIdx - Globals.candleSettings[7].avgPeriod;
            int i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                float num57;
                float num62;
                float num67;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num67 = Math.Abs((float) (inClose[i - 3] - inOpen[i - 3]));
                }
                else
                {
                    float num66;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num66 = inHigh[i - 3] - inLow[i - 3];
                    }
                    else
                    {
                        float num63;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num64;
                            float num65;
                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num65 = inClose[i - 3];
                            }
                            else
                            {
                                num65 = inOpen[i - 3];
                            }

                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num64 = inOpen[i - 3];
                            }
                            else
                            {
                                num64 = inClose[i - 3];
                            }

                            num63 = (inHigh[i - 3] - num65) + (num64 - inLow[i - 3]);
                        }
                        else
                        {
                            num63 = 0.0f;
                        }

                        num66 = num63;
                    }

                    num67 = num66;
                }

                ShadowVeryShortPeriodTotal[3] += num67;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num62 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num61;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num61 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num58;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num59;
                            float num60;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num60 = inClose[i - 2];
                            }
                            else
                            {
                                num60 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num59 = inOpen[i - 2];
                            }
                            else
                            {
                                num59 = inClose[i - 2];
                            }

                            num58 = (inHigh[i - 2] - num60) + (num59 - inLow[i - 2]);
                        }
                        else
                        {
                            num58 = 0.0f;
                        }

                        num61 = num58;
                    }

                    num62 = num61;
                }

                ShadowVeryShortPeriodTotal[2] += num62;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num57 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num56;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num56 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num53;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num54;
                            float num55;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num55 = inClose[i - 1];
                            }
                            else
                            {
                                num55 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num54 = inOpen[i - 1];
                            }
                            else
                            {
                                num54 = inClose[i - 1];
                            }

                            num53 = (inHigh[i - 1] - num55) + (num54 - inLow[i - 1]);
                        }
                        else
                        {
                            num53 = 0.0f;
                        }

                        num56 = num53;
                    }

                    num57 = num56;
                }

                ShadowVeryShortPeriodTotal[1] += num57;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0360:
            if ((((((inClose[i - 3] < inOpen[i - 3]) ? -1 : 1) == -1) && (((inClose[i - 2] < inOpen[i - 2]) ? -1 : 1) == -1)) &&
                 (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1)) && (((inClose[i] < inOpen[i]) ? -1 : 1) == -1))
            {
                double num45;
                double num51;
                float num52;
                if (inClose[i - 3] >= inOpen[i - 3])
                {
                    num52 = inOpen[i - 3];
                }
                else
                {
                    num52 = inClose[i - 3];
                }

                if (Globals.candleSettings[7].avgPeriod != 0.0)
                {
                    num51 = ShadowVeryShortPeriodTotal[3] / ((double) Globals.candleSettings[7].avgPeriod);
                }
                else
                {
                    float num50;
                    if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                    {
                        num50 = Math.Abs((float) (inClose[i - 3] - inOpen[i - 3]));
                    }
                    else
                    {
                        float num49;
                        if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                        {
                            num49 = inHigh[i - 3] - inLow[i - 3];
                        }
                        else
                        {
                            float num46;
                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                            {
                                float num47;
                                float num48;
                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num48 = inClose[i - 3];
                                }
                                else
                                {
                                    num48 = inOpen[i - 3];
                                }

                                if (inClose[i - 3] >= inOpen[i - 3])
                                {
                                    num47 = inOpen[i - 3];
                                }
                                else
                                {
                                    num47 = inClose[i - 3];
                                }

                                num46 = (inHigh[i - 3] - num48) + (num47 - inLow[i - 3]);
                            }
                            else
                            {
                                num46 = 0.0f;
                            }

                            num49 = num46;
                        }

                        num50 = num49;
                    }

                    num51 = num50;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                {
                    num45 = 2.0;
                }
                else
                {
                    num45 = 1.0;
                }

                if ((num52 - inLow[i - 3]) < ((Globals.candleSettings[7].factor * num51) / num45))
                {
                    double num37;
                    double num43;
                    float num44;
                    if (inClose[i - 3] >= inOpen[i - 3])
                    {
                        num44 = inClose[i - 3];
                    }
                    else
                    {
                        num44 = inOpen[i - 3];
                    }

                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                    {
                        num43 = ShadowVeryShortPeriodTotal[3] / ((double) Globals.candleSettings[7].avgPeriod);
                    }
                    else
                    {
                        float num42;
                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                        {
                            num42 = Math.Abs((float) (inClose[i - 3] - inOpen[i - 3]));
                        }
                        else
                        {
                            float num41;
                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                            {
                                num41 = inHigh[i - 3] - inLow[i - 3];
                            }
                            else
                            {
                                float num38;
                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                {
                                    float num39;
                                    float num40;
                                    if (inClose[i - 3] >= inOpen[i - 3])
                                    {
                                        num40 = inClose[i - 3];
                                    }
                                    else
                                    {
                                        num40 = inOpen[i - 3];
                                    }

                                    if (inClose[i - 3] >= inOpen[i - 3])
                                    {
                                        num39 = inOpen[i - 3];
                                    }
                                    else
                                    {
                                        num39 = inClose[i - 3];
                                    }

                                    num38 = (inHigh[i - 3] - num40) + (num39 - inLow[i - 3]);
                                }
                                else
                                {
                                    num38 = 0.0f;
                                }

                                num41 = num38;
                            }

                            num42 = num41;
                        }

                        num43 = num42;
                    }

                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        num37 = 2.0;
                    }
                    else
                    {
                        num37 = 1.0;
                    }

                    if ((inHigh[i - 3] - num44) < ((Globals.candleSettings[7].factor * num43) / num37))
                    {
                        double num29;
                        double num35;
                        float num36;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num36 = inOpen[i - 2];
                        }
                        else
                        {
                            num36 = inClose[i - 2];
                        }

                        if (Globals.candleSettings[7].avgPeriod != 0.0)
                        {
                            num35 = ShadowVeryShortPeriodTotal[2] / ((double) Globals.candleSettings[7].avgPeriod);
                        }
                        else
                        {
                            float num34;
                            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                            {
                                num34 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                            }
                            else
                            {
                                float num33;
                                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                {
                                    num33 = inHigh[i - 2] - inLow[i - 2];
                                }
                                else
                                {
                                    float num30;
                                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                    {
                                        float num31;
                                        float num32;
                                        if (inClose[i - 2] >= inOpen[i - 2])
                                        {
                                            num32 = inClose[i - 2];
                                        }
                                        else
                                        {
                                            num32 = inOpen[i - 2];
                                        }

                                        if (inClose[i - 2] >= inOpen[i - 2])
                                        {
                                            num31 = inOpen[i - 2];
                                        }
                                        else
                                        {
                                            num31 = inClose[i - 2];
                                        }

                                        num30 = (inHigh[i - 2] - num32) + (num31 - inLow[i - 2]);
                                    }
                                    else
                                    {
                                        num30 = 0.0f;
                                    }

                                    num33 = num30;
                                }

                                num34 = num33;
                            }

                            num35 = num34;
                        }

                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            num29 = 2.0;
                        }
                        else
                        {
                            num29 = 1.0;
                        }

                        if ((num36 - inLow[i - 2]) < ((Globals.candleSettings[7].factor * num35) / num29))
                        {
                            double num21;
                            double num27;
                            float num28;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num28 = inClose[i - 2];
                            }
                            else
                            {
                                num28 = inOpen[i - 2];
                            }

                            if (Globals.candleSettings[7].avgPeriod != 0.0)
                            {
                                num27 = ShadowVeryShortPeriodTotal[2] / ((double) Globals.candleSettings[7].avgPeriod);
                            }
                            else
                            {
                                float num26;
                                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                                {
                                    num26 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                                }
                                else
                                {
                                    float num25;
                                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                    {
                                        num25 = inHigh[i - 2] - inLow[i - 2];
                                    }
                                    else
                                    {
                                        float num22;
                                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                        {
                                            float num23;
                                            float num24;
                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num24 = inClose[i - 2];
                                            }
                                            else
                                            {
                                                num24 = inOpen[i - 2];
                                            }

                                            if (inClose[i - 2] >= inOpen[i - 2])
                                            {
                                                num23 = inOpen[i - 2];
                                            }
                                            else
                                            {
                                                num23 = inClose[i - 2];
                                            }

                                            num22 = (inHigh[i - 2] - num24) + (num23 - inLow[i - 2]);
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

                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                            {
                                num21 = 2.0;
                            }
                            else
                            {
                                num21 = 1.0;
                            }

                            if ((inHigh[i - 2] - num28) < ((Globals.candleSettings[7].factor * num27) / num21))
                            {
                                float num19;
                                float num20;
                                if (inOpen[i - 1] > inClose[i - 1])
                                {
                                    num20 = inOpen[i - 1];
                                }
                                else
                                {
                                    num20 = inClose[i - 1];
                                }

                                if (inOpen[i - 2] < inClose[i - 2])
                                {
                                    num19 = inOpen[i - 2];
                                }
                                else
                                {
                                    num19 = inClose[i - 2];
                                }

                                if (num20 < num19)
                                {
                                    double num11;
                                    double num17;
                                    float num18;
                                    if (inClose[i - 1] >= inOpen[i - 1])
                                    {
                                        num18 = inClose[i - 1];
                                    }
                                    else
                                    {
                                        num18 = inOpen[i - 1];
                                    }

                                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                                    {
                                        num17 = ShadowVeryShortPeriodTotal[1] / ((double) Globals.candleSettings[7].avgPeriod);
                                    }
                                    else
                                    {
                                        float num16;
                                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                                        {
                                            num16 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                                        }
                                        else
                                        {
                                            float num15;
                                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                            {
                                                num15 = inHigh[i - 1] - inLow[i - 1];
                                            }
                                            else
                                            {
                                                float num12;
                                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                                {
                                                    float num13;
                                                    float num14;
                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num14 = inClose[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num14 = inOpen[i - 1];
                                                    }

                                                    if (inClose[i - 1] >= inOpen[i - 1])
                                                    {
                                                        num13 = inOpen[i - 1];
                                                    }
                                                    else
                                                    {
                                                        num13 = inClose[i - 1];
                                                    }

                                                    num12 = (inHigh[i - 1] - num14) + (num13 - inLow[i - 1]);
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

                                    if ((((inHigh[i - 1] - num18) > ((Globals.candleSettings[7].factor * num17) / num11)) &&
                                         (inHigh[i - 1] > inClose[i - 2])) && ((inHigh[i] > inHigh[i - 1]) && (inLow[i] < inLow[i - 1])))
                                    {
                                        outInteger[outIdx] = 100;
                                        outIdx++;
                                        goto Label_0BFF;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0BFF:
            totIdx = 3;
            while (totIdx >= 1)
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
                goto Label_0360;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlConcealBabysWallLookback()
        {
            return (Globals.candleSettings[7].avgPeriod + 3);
        }
    }
}
