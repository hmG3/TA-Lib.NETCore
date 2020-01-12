using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Cdl3LineStrike(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int totIdx;
            int num46;
            double[] NearPeriodTotal = new double[4];
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

            int lookbackTotal = Cdl3LineStrikeLookback();
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

            NearPeriodTotal[3] = 0.0;
            NearPeriodTotal[2] = 0.0;
            int NearTrailingIdx = startIdx - Globals.candleSettings[8].avgPeriod;
            int i = NearTrailingIdx;
            while (true)
            {
                double num51;
                double num56;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num56 = Math.Abs((double) (inClose[i - 3] - inOpen[i - 3]));
                }
                else
                {
                    double num55;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num55 = inHigh[i - 3] - inLow[i - 3];
                    }
                    else
                    {
                        double num52;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            double num53;
                            double num54;
                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num54 = inClose[i - 3];
                            }
                            else
                            {
                                num54 = inOpen[i - 3];
                            }

                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num53 = inOpen[i - 3];
                            }
                            else
                            {
                                num53 = inClose[i - 3];
                            }

                            num52 = (inHigh[i - 3] - num54) + (num53 - inLow[i - 3]);
                        }
                        else
                        {
                            num52 = 0.0;
                        }

                        num55 = num52;
                    }

                    num56 = num55;
                }

                NearPeriodTotal[3] += num56;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num51 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num50;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num50 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num47;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            double num48;
                            double num49;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num49 = inClose[i - 2];
                            }
                            else
                            {
                                num49 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num48 = inOpen[i - 2];
                            }
                            else
                            {
                                num48 = inClose[i - 2];
                            }

                            num47 = (inHigh[i - 2] - num49) + (num48 - inLow[i - 2]);
                        }
                        else
                        {
                            num47 = 0.0;
                        }

                        num50 = num47;
                    }

                    num51 = num50;
                }

                NearPeriodTotal[2] += num51;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0256:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                num46 = 1;
            }
            else
            {
                num46 = -1;
            }

            if (((inClose[i - 3] < inOpen[i - 3]) ? -1 : 1) == num46)
            {
                int num45;
                if (inClose[i - 1] >= inOpen[i - 1])
                {
                    num45 = 1;
                }
                else
                {
                    num45 = -1;
                }

                if (((inClose[i - 2] < inOpen[i - 2]) ? -1 : 1) == num45)
                {
                    int num44;
                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num44 = 1;
                    }
                    else
                    {
                        num44 = -1;
                    }

                    if (((inClose[i] < inOpen[i]) ? -1 : 1) == -num44)
                    {
                        double num36;
                        double num42;
                        double num43;
                        if (inOpen[i - 3] < inClose[i - 3])
                        {
                            num43 = inOpen[i - 3];
                        }
                        else
                        {
                            num43 = inClose[i - 3];
                        }

                        if (Globals.candleSettings[8].avgPeriod != 0.0)
                        {
                            num42 = NearPeriodTotal[3] / ((double) Globals.candleSettings[8].avgPeriod);
                        }
                        else
                        {
                            double num41;
                            if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                            {
                                num41 = Math.Abs((double) (inClose[i - 3] - inOpen[i - 3]));
                            }
                            else
                            {
                                double num40;
                                if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                                {
                                    num40 = inHigh[i - 3] - inLow[i - 3];
                                }
                                else
                                {
                                    double num37;
                                    if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                    {
                                        double num38;
                                        double num39;
                                        if (inClose[i - 3] >= inOpen[i - 3])
                                        {
                                            num39 = inClose[i - 3];
                                        }
                                        else
                                        {
                                            num39 = inOpen[i - 3];
                                        }

                                        if (inClose[i - 3] >= inOpen[i - 3])
                                        {
                                            num38 = inOpen[i - 3];
                                        }
                                        else
                                        {
                                            num38 = inClose[i - 3];
                                        }

                                        num37 = (inHigh[i - 3] - num39) + (num38 - inLow[i - 3]);
                                    }
                                    else
                                    {
                                        num37 = 0.0;
                                    }

                                    num40 = num37;
                                }

                                num41 = num40;
                            }

                            num42 = num41;
                        }

                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            num36 = 2.0;
                        }
                        else
                        {
                            num36 = 1.0;
                        }

                        if (inOpen[i - 2] >= (num43 - ((Globals.candleSettings[8].factor * num42) / num36)))
                        {
                            double num28;
                            double num34;
                            double num35;
                            if (inOpen[i - 3] > inClose[i - 3])
                            {
                                num35 = inOpen[i - 3];
                            }
                            else
                            {
                                num35 = inClose[i - 3];
                            }

                            if (Globals.candleSettings[8].avgPeriod != 0.0)
                            {
                                num34 = NearPeriodTotal[3] / ((double) Globals.candleSettings[8].avgPeriod);
                            }
                            else
                            {
                                double num33;
                                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                                {
                                    num33 = Math.Abs((double) (inClose[i - 3] - inOpen[i - 3]));
                                }
                                else
                                {
                                    double num32;
                                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                                    {
                                        num32 = inHigh[i - 3] - inLow[i - 3];
                                    }
                                    else
                                    {
                                        double num29;
                                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                        {
                                            double num30;
                                            double num31;
                                            if (inClose[i - 3] >= inOpen[i - 3])
                                            {
                                                num31 = inClose[i - 3];
                                            }
                                            else
                                            {
                                                num31 = inOpen[i - 3];
                                            }

                                            if (inClose[i - 3] >= inOpen[i - 3])
                                            {
                                                num30 = inOpen[i - 3];
                                            }
                                            else
                                            {
                                                num30 = inClose[i - 3];
                                            }

                                            num29 = (inHigh[i - 3] - num31) + (num30 - inLow[i - 3]);
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

                            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                            {
                                num28 = 2.0;
                            }
                            else
                            {
                                num28 = 1.0;
                            }

                            if (inOpen[i - 2] <= (num35 + ((Globals.candleSettings[8].factor * num34) / num28)))
                            {
                                double num20;
                                double num26;
                                double num27;
                                if (inOpen[i - 2] < inClose[i - 2])
                                {
                                    num27 = inOpen[i - 2];
                                }
                                else
                                {
                                    num27 = inClose[i - 2];
                                }

                                if (Globals.candleSettings[8].avgPeriod != 0.0)
                                {
                                    num26 = NearPeriodTotal[2] / ((double) Globals.candleSettings[8].avgPeriod);
                                }
                                else
                                {
                                    double num25;
                                    if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                                    {
                                        num25 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                                    }
                                    else
                                    {
                                        double num24;
                                        if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                                        {
                                            num24 = inHigh[i - 2] - inLow[i - 2];
                                        }
                                        else
                                        {
                                            double num21;
                                            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                            {
                                                double num22;
                                                double num23;
                                                if (inClose[i - 2] >= inOpen[i - 2])
                                                {
                                                    num23 = inClose[i - 2];
                                                }
                                                else
                                                {
                                                    num23 = inOpen[i - 2];
                                                }

                                                if (inClose[i - 2] >= inOpen[i - 2])
                                                {
                                                    num22 = inOpen[i - 2];
                                                }
                                                else
                                                {
                                                    num22 = inClose[i - 2];
                                                }

                                                num21 = (inHigh[i - 2] - num23) + (num22 - inLow[i - 2]);
                                            }
                                            else
                                            {
                                                num21 = 0.0;
                                            }

                                            num24 = num21;
                                        }

                                        num25 = num24;
                                    }

                                    num26 = num25;
                                }

                                if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                {
                                    num20 = 2.0;
                                }
                                else
                                {
                                    num20 = 1.0;
                                }

                                if (inOpen[i - 1] >= (num27 - ((Globals.candleSettings[8].factor * num26) / num20)))
                                {
                                    double num12;
                                    double num18;
                                    double num19;
                                    if (inOpen[i - 2] > inClose[i - 2])
                                    {
                                        num19 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num19 = inClose[i - 2];
                                    }

                                    if (Globals.candleSettings[8].avgPeriod != 0.0)
                                    {
                                        num18 = NearPeriodTotal[2] / ((double) Globals.candleSettings[8].avgPeriod);
                                    }
                                    else
                                    {
                                        double num17;
                                        if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                                        {
                                            num17 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                                        }
                                        else
                                        {
                                            double num16;
                                            if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                                            {
                                                num16 = inHigh[i - 2] - inLow[i - 2];
                                            }
                                            else
                                            {
                                                double num13;
                                                if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                                {
                                                    double num14;
                                                    double num15;
                                                    if (inClose[i - 2] >= inOpen[i - 2])
                                                    {
                                                        num15 = inClose[i - 2];
                                                    }
                                                    else
                                                    {
                                                        num15 = inOpen[i - 2];
                                                    }

                                                    if (inClose[i - 2] >= inOpen[i - 2])
                                                    {
                                                        num14 = inOpen[i - 2];
                                                    }
                                                    else
                                                    {
                                                        num14 = inClose[i - 2];
                                                    }

                                                    num13 = (inHigh[i - 2] - num15) + (num14 - inLow[i - 2]);
                                                }
                                                else
                                                {
                                                    num13 = 0.0;
                                                }

                                                num16 = num13;
                                            }

                                            num17 = num16;
                                        }

                                        num18 = num17;
                                    }

                                    if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                    {
                                        num12 = 2.0;
                                    }
                                    else
                                    {
                                        num12 = 1.0;
                                    }

                                    if ((inOpen[i - 1] <= (num19 + ((Globals.candleSettings[8].factor * num18) / num12))) &&
                                        ((((inClose[i - 1] >= inOpen[i - 1]) && (inClose[i - 1] > inClose[i - 2])) &&
                                          (((inClose[i - 2] > inClose[i - 3]) && (inOpen[i] > inClose[i - 1])) &&
                                           (inClose[i] < inOpen[i - 3]))) ||
                                         ((((((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1) && (inClose[i - 1] < inClose[i - 2])) &&
                                           ((inClose[i - 2] < inClose[i - 3]) && (inOpen[i] < inClose[i - 1]))) &&
                                          (inClose[i] > inOpen[i - 3]))))
                                    {
                                        int num11;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num11 = 1;
                                        }
                                        else
                                        {
                                            num11 = -1;
                                        }

                                        outInteger[outIdx] = num11 * 100;
                                        outIdx++;
                                        goto Label_0975;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0975:
            totIdx = 3;
            while (totIdx >= 2)
            {
                double num5;
                double num10;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs((double) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    double num9;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num6;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs((double) (inClose[NearTrailingIdx - totIdx] - inOpen[NearTrailingIdx - totIdx]));
                }
                else
                {
                    double num4;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[NearTrailingIdx - totIdx] - inLow[NearTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            double num2;
                            double num3;
                            if (inClose[NearTrailingIdx - totIdx] >= inOpen[NearTrailingIdx - totIdx])
                            {
                                num3 = inClose[NearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[NearTrailingIdx - totIdx];
                            }

                            if (inClose[NearTrailingIdx - totIdx] >= inOpen[NearTrailingIdx - totIdx])
                            {
                                num2 = inOpen[NearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[NearTrailingIdx - totIdx];
                            }

                            num = (inHigh[NearTrailingIdx - totIdx] - num3) + (num2 - inLow[NearTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                NearPeriodTotal[totIdx] += num10 - num5;
                totIdx--;
            }

            i++;
            NearTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0256;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Cdl3LineStrike(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int totIdx;
            int num46;
            double[] NearPeriodTotal = new double[4];
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

            int lookbackTotal = Cdl3LineStrikeLookback();
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

            NearPeriodTotal[3] = 0.0;
            NearPeriodTotal[2] = 0.0;
            int NearTrailingIdx = startIdx - Globals.candleSettings[8].avgPeriod;
            int i = NearTrailingIdx;
            while (true)
            {
                float num51;
                float num56;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num56 = Math.Abs((float) (inClose[i - 3] - inOpen[i - 3]));
                }
                else
                {
                    float num55;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num55 = inHigh[i - 3] - inLow[i - 3];
                    }
                    else
                    {
                        float num52;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            float num53;
                            float num54;
                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num54 = inClose[i - 3];
                            }
                            else
                            {
                                num54 = inOpen[i - 3];
                            }

                            if (inClose[i - 3] >= inOpen[i - 3])
                            {
                                num53 = inOpen[i - 3];
                            }
                            else
                            {
                                num53 = inClose[i - 3];
                            }

                            num52 = (inHigh[i - 3] - num54) + (num53 - inLow[i - 3]);
                        }
                        else
                        {
                            num52 = 0.0f;
                        }

                        num55 = num52;
                    }

                    num56 = num55;
                }

                NearPeriodTotal[3] += num56;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num51 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num50;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num50 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num47;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            float num48;
                            float num49;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num49 = inClose[i - 2];
                            }
                            else
                            {
                                num49 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num48 = inOpen[i - 2];
                            }
                            else
                            {
                                num48 = inClose[i - 2];
                            }

                            num47 = (inHigh[i - 2] - num49) + (num48 - inLow[i - 2]);
                        }
                        else
                        {
                            num47 = 0.0f;
                        }

                        num50 = num47;
                    }

                    num51 = num50;
                }

                NearPeriodTotal[2] += num51;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0272:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                num46 = 1;
            }
            else
            {
                num46 = -1;
            }

            if (((inClose[i - 3] < inOpen[i - 3]) ? -1 : 1) == num46)
            {
                int num45;
                if (inClose[i - 1] >= inOpen[i - 1])
                {
                    num45 = 1;
                }
                else
                {
                    num45 = -1;
                }

                if (((inClose[i - 2] < inOpen[i - 2]) ? -1 : 1) == num45)
                {
                    int num44;
                    if (inClose[i - 1] >= inOpen[i - 1])
                    {
                        num44 = 1;
                    }
                    else
                    {
                        num44 = -1;
                    }

                    if (((inClose[i] < inOpen[i]) ? -1 : 1) == -num44)
                    {
                        double num36;
                        double num42;
                        float num43;
                        if (inOpen[i - 3] < inClose[i - 3])
                        {
                            num43 = inOpen[i - 3];
                        }
                        else
                        {
                            num43 = inClose[i - 3];
                        }

                        if (Globals.candleSettings[8].avgPeriod != 0.0)
                        {
                            num42 = NearPeriodTotal[3] / ((double) Globals.candleSettings[8].avgPeriod);
                        }
                        else
                        {
                            float num41;
                            if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                            {
                                num41 = Math.Abs((float) (inClose[i - 3] - inOpen[i - 3]));
                            }
                            else
                            {
                                float num40;
                                if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                                {
                                    num40 = inHigh[i - 3] - inLow[i - 3];
                                }
                                else
                                {
                                    float num37;
                                    if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                    {
                                        float num38;
                                        float num39;
                                        if (inClose[i - 3] >= inOpen[i - 3])
                                        {
                                            num39 = inClose[i - 3];
                                        }
                                        else
                                        {
                                            num39 = inOpen[i - 3];
                                        }

                                        if (inClose[i - 3] >= inOpen[i - 3])
                                        {
                                            num38 = inOpen[i - 3];
                                        }
                                        else
                                        {
                                            num38 = inClose[i - 3];
                                        }

                                        num37 = (inHigh[i - 3] - num39) + (num38 - inLow[i - 3]);
                                    }
                                    else
                                    {
                                        num37 = 0.0f;
                                    }

                                    num40 = num37;
                                }

                                num41 = num40;
                            }

                            num42 = num41;
                        }

                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            num36 = 2.0;
                        }
                        else
                        {
                            num36 = 1.0;
                        }

                        if (inOpen[i - 2] >= (num43 - ((Globals.candleSettings[8].factor * num42) / num36)))
                        {
                            double num28;
                            double num34;
                            float num35;
                            if (inOpen[i - 3] > inClose[i - 3])
                            {
                                num35 = inOpen[i - 3];
                            }
                            else
                            {
                                num35 = inClose[i - 3];
                            }

                            if (Globals.candleSettings[8].avgPeriod != 0.0)
                            {
                                num34 = NearPeriodTotal[3] / ((double) Globals.candleSettings[8].avgPeriod);
                            }
                            else
                            {
                                float num33;
                                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                                {
                                    num33 = Math.Abs((float) (inClose[i - 3] - inOpen[i - 3]));
                                }
                                else
                                {
                                    float num32;
                                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                                    {
                                        num32 = inHigh[i - 3] - inLow[i - 3];
                                    }
                                    else
                                    {
                                        float num29;
                                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                        {
                                            float num30;
                                            float num31;
                                            if (inClose[i - 3] >= inOpen[i - 3])
                                            {
                                                num31 = inClose[i - 3];
                                            }
                                            else
                                            {
                                                num31 = inOpen[i - 3];
                                            }

                                            if (inClose[i - 3] >= inOpen[i - 3])
                                            {
                                                num30 = inOpen[i - 3];
                                            }
                                            else
                                            {
                                                num30 = inClose[i - 3];
                                            }

                                            num29 = (inHigh[i - 3] - num31) + (num30 - inLow[i - 3]);
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

                            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                            {
                                num28 = 2.0;
                            }
                            else
                            {
                                num28 = 1.0;
                            }

                            if (inOpen[i - 2] <= (num35 + ((Globals.candleSettings[8].factor * num34) / num28)))
                            {
                                double num20;
                                double num26;
                                float num27;
                                if (inOpen[i - 2] < inClose[i - 2])
                                {
                                    num27 = inOpen[i - 2];
                                }
                                else
                                {
                                    num27 = inClose[i - 2];
                                }

                                if (Globals.candleSettings[8].avgPeriod != 0.0)
                                {
                                    num26 = NearPeriodTotal[2] / ((double) Globals.candleSettings[8].avgPeriod);
                                }
                                else
                                {
                                    float num25;
                                    if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                                    {
                                        num25 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                                    }
                                    else
                                    {
                                        float num24;
                                        if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                                        {
                                            num24 = inHigh[i - 2] - inLow[i - 2];
                                        }
                                        else
                                        {
                                            float num21;
                                            if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                            {
                                                float num22;
                                                float num23;
                                                if (inClose[i - 2] >= inOpen[i - 2])
                                                {
                                                    num23 = inClose[i - 2];
                                                }
                                                else
                                                {
                                                    num23 = inOpen[i - 2];
                                                }

                                                if (inClose[i - 2] >= inOpen[i - 2])
                                                {
                                                    num22 = inOpen[i - 2];
                                                }
                                                else
                                                {
                                                    num22 = inClose[i - 2];
                                                }

                                                num21 = (inHigh[i - 2] - num23) + (num22 - inLow[i - 2]);
                                            }
                                            else
                                            {
                                                num21 = 0.0f;
                                            }

                                            num24 = num21;
                                        }

                                        num25 = num24;
                                    }

                                    num26 = num25;
                                }

                                if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                {
                                    num20 = 2.0;
                                }
                                else
                                {
                                    num20 = 1.0;
                                }

                                if (inOpen[i - 1] >= (num27 - ((Globals.candleSettings[8].factor * num26) / num20)))
                                {
                                    double num12;
                                    double num18;
                                    float num19;
                                    if (inOpen[i - 2] > inClose[i - 2])
                                    {
                                        num19 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num19 = inClose[i - 2];
                                    }

                                    if (Globals.candleSettings[8].avgPeriod != 0.0)
                                    {
                                        num18 = NearPeriodTotal[2] / ((double) Globals.candleSettings[8].avgPeriod);
                                    }
                                    else
                                    {
                                        float num17;
                                        if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                                        {
                                            num17 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                                        }
                                        else
                                        {
                                            float num16;
                                            if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                                            {
                                                num16 = inHigh[i - 2] - inLow[i - 2];
                                            }
                                            else
                                            {
                                                float num13;
                                                if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                                {
                                                    float num14;
                                                    float num15;
                                                    if (inClose[i - 2] >= inOpen[i - 2])
                                                    {
                                                        num15 = inClose[i - 2];
                                                    }
                                                    else
                                                    {
                                                        num15 = inOpen[i - 2];
                                                    }

                                                    if (inClose[i - 2] >= inOpen[i - 2])
                                                    {
                                                        num14 = inOpen[i - 2];
                                                    }
                                                    else
                                                    {
                                                        num14 = inClose[i - 2];
                                                    }

                                                    num13 = (inHigh[i - 2] - num15) + (num14 - inLow[i - 2]);
                                                }
                                                else
                                                {
                                                    num13 = 0.0f;
                                                }

                                                num16 = num13;
                                            }

                                            num17 = num16;
                                        }

                                        num18 = num17;
                                    }

                                    if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                                    {
                                        num12 = 2.0;
                                    }
                                    else
                                    {
                                        num12 = 1.0;
                                    }

                                    if ((inOpen[i - 1] <= (num19 + ((Globals.candleSettings[8].factor * num18) / num12))) &&
                                        ((((inClose[i - 1] >= inOpen[i - 1]) && (inClose[i - 1] > inClose[i - 2])) &&
                                          (((inClose[i - 2] > inClose[i - 3]) && (inOpen[i] > inClose[i - 1])) &&
                                           (inClose[i] < inOpen[i - 3]))) ||
                                         ((((((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1) && (inClose[i - 1] < inClose[i - 2])) &&
                                           ((inClose[i - 2] < inClose[i - 3]) && (inOpen[i] < inClose[i - 1]))) &&
                                          (inClose[i] > inOpen[i - 3]))))
                                    {
                                        int num11;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num11 = 1;
                                        }
                                        else
                                        {
                                            num11 = -1;
                                        }

                                        outInteger[outIdx] = num11 * 100;
                                        outIdx++;
                                        goto Label_0A03;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0A03:
            totIdx = 3;
            while (totIdx >= 2)
            {
                float num5;
                float num10;
                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs((float) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    float num9;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        float num6;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[8].rangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs((float) (inClose[NearTrailingIdx - totIdx] - inOpen[NearTrailingIdx - totIdx]));
                }
                else
                {
                    float num4;
                    if (Globals.candleSettings[8].rangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[NearTrailingIdx - totIdx] - inLow[NearTrailingIdx - totIdx];
                    }
                    else
                    {
                        float num;
                        if (Globals.candleSettings[8].rangeType == RangeType.Shadows)
                        {
                            float num2;
                            float num3;
                            if (inClose[NearTrailingIdx - totIdx] >= inOpen[NearTrailingIdx - totIdx])
                            {
                                num3 = inClose[NearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[NearTrailingIdx - totIdx];
                            }

                            if (inClose[NearTrailingIdx - totIdx] >= inOpen[NearTrailingIdx - totIdx])
                            {
                                num2 = inOpen[NearTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[NearTrailingIdx - totIdx];
                            }

                            num = (inHigh[NearTrailingIdx - totIdx] - num3) + (num2 - inLow[NearTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0f;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                NearPeriodTotal[totIdx] += num10 - num5;
                totIdx--;
            }

            i++;
            NearTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0272;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int Cdl3LineStrikeLookback()
        {
            return (Globals.candleSettings[8].avgPeriod + 3);
        }
    }
}
