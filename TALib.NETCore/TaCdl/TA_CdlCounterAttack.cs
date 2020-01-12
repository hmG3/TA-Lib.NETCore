using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlCounterAttack(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num15;
            double num20;
            int num50;
            double[] BodyLongPeriodTotal = new double[2];
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

            int lookbackTotal = CdlCounterAttackLookback();
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

            double EqualPeriodTotal = 0.0;
            int EqualTrailingIdx = startIdx - Globals.candleSettings[10].avgPeriod;
            BodyLongPeriodTotal[1] = 0.0;
            BodyLongPeriodTotal[0] = 0.0;
            int BodyLongTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            int i = EqualTrailingIdx;
            while (true)
            {
                double num65;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                {
                    num65 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num64;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num64 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num61;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            double num62;
                            double num63;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num63 = inClose[i - 1];
                            }
                            else
                            {
                                num63 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num62 = inOpen[i - 1];
                            }
                            else
                            {
                                num62 = inClose[i - 1];
                            }

                            num61 = (inHigh[i - 1] - num63) + (num62 - inLow[i - 1]);
                        }
                        else
                        {
                            num61 = 0.0;
                        }

                        num64 = num61;
                    }

                    num65 = num64;
                }

                EqualPeriodTotal += num65;
                i++;
            }

            i = BodyLongTrailingIdx;
            while (true)
            {
                double num55;
                double num60;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num60 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num59;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num59 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num56;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num57;
                            double num58;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num58 = inClose[i - 1];
                            }
                            else
                            {
                                num58 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num57 = inOpen[i - 1];
                            }
                            else
                            {
                                num57 = inClose[i - 1];
                            }

                            num56 = (inHigh[i - 1] - num58) + (num57 - inLow[i - 1]);
                        }
                        else
                        {
                            num56 = 0.0;
                        }

                        num59 = num56;
                    }

                    num60 = num59;
                }

                BodyLongPeriodTotal[1] += num60;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num55 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num54;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num54 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num51;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num52;
                            double num53;
                            if (inClose[i] >= inOpen[i])
                            {
                                num53 = inClose[i];
                            }
                            else
                            {
                                num53 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num52 = inOpen[i];
                            }
                            else
                            {
                                num52 = inClose[i];
                            }

                            num51 = (inHigh[i] - num53) + (num52 - inLow[i]);
                        }
                        else
                        {
                            num51 = 0.0;
                        }

                        num54 = num51;
                    }

                    num55 = num54;
                }

                BodyLongPeriodTotal[0] += num55;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0345:
            if (inClose[i] >= inOpen[i])
            {
                num50 = 1;
            }
            else
            {
                num50 = -1;
            }

            if (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -num50)
            {
                double num43;
                double num49;
                if (Globals.candleSettings[0].avgPeriod != 0.0)
                {
                    num49 = BodyLongPeriodTotal[1] / ((double) Globals.candleSettings[0].avgPeriod);
                }
                else
                {
                    double num48;
                    if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                    {
                        num48 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                    }
                    else
                    {
                        double num47;
                        if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                        {
                            num47 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            double num44;
                            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                            {
                                double num45;
                                double num46;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num46 = inClose[i - 1];
                                }
                                else
                                {
                                    num46 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num45 = inOpen[i - 1];
                                }
                                else
                                {
                                    num45 = inClose[i - 1];
                                }

                                num44 = (inHigh[i - 1] - num46) + (num45 - inLow[i - 1]);
                            }
                            else
                            {
                                num44 = 0.0;
                            }

                            num47 = num44;
                        }

                        num48 = num47;
                    }

                    num49 = num48;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                {
                    num43 = 2.0;
                }
                else
                {
                    num43 = 1.0;
                }

                if (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) > ((Globals.candleSettings[0].factor * num49) / num43))
                {
                    double num36;
                    double num42;
                    if (Globals.candleSettings[0].avgPeriod != 0.0)
                    {
                        num42 = BodyLongPeriodTotal[0] / ((double) Globals.candleSettings[0].avgPeriod);
                    }
                    else
                    {
                        double num41;
                        if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                        {
                            num41 = Math.Abs((double) (inClose[i] - inOpen[i]));
                        }
                        else
                        {
                            double num40;
                            if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                            {
                                num40 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num37;
                                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

                        num42 = num41;
                    }

                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        num36 = 2.0;
                    }
                    else
                    {
                        num36 = 1.0;
                    }

                    if (Math.Abs((double) (inClose[i] - inOpen[i])) > ((Globals.candleSettings[0].factor * num42) / num36))
                    {
                        double num29;
                        double num35;
                        if (Globals.candleSettings[10].avgPeriod != 0.0)
                        {
                            num35 = EqualPeriodTotal / ((double) Globals.candleSettings[10].avgPeriod);
                        }
                        else
                        {
                            double num34;
                            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                            {
                                num34 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                            }
                            else
                            {
                                double num33;
                                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                                {
                                    num33 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    double num30;
                                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                    {
                                        double num31;
                                        double num32;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num32 = inClose[i - 1];
                                        }
                                        else
                                        {
                                            num32 = inOpen[i - 1];
                                        }

                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num31 = inOpen[i - 1];
                                        }
                                        else
                                        {
                                            num31 = inClose[i - 1];
                                        }

                                        num30 = (inHigh[i - 1] - num32) + (num31 - inLow[i - 1]);
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

                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            num29 = 2.0;
                        }
                        else
                        {
                            num29 = 1.0;
                        }

                        if (inClose[i] <= (inClose[i - 1] + ((Globals.candleSettings[10].factor * num35) / num29)))
                        {
                            double num22;
                            double num28;
                            if (Globals.candleSettings[10].avgPeriod != 0.0)
                            {
                                num28 = EqualPeriodTotal / ((double) Globals.candleSettings[10].avgPeriod);
                            }
                            else
                            {
                                double num27;
                                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                                {
                                    num27 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                                }
                                else
                                {
                                    double num26;
                                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                                    {
                                        num26 = inHigh[i - 1] - inLow[i - 1];
                                    }
                                    else
                                    {
                                        double num23;
                                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                        {
                                            double num24;
                                            double num25;
                                            if (inClose[i - 1] >= inOpen[i - 1])
                                            {
                                                num25 = inClose[i - 1];
                                            }
                                            else
                                            {
                                                num25 = inOpen[i - 1];
                                            }

                                            if (inClose[i - 1] >= inOpen[i - 1])
                                            {
                                                num24 = inOpen[i - 1];
                                            }
                                            else
                                            {
                                                num24 = inClose[i - 1];
                                            }

                                            num23 = (inHigh[i - 1] - num25) + (num24 - inLow[i - 1]);
                                        }
                                        else
                                        {
                                            num23 = 0.0;
                                        }

                                        num26 = num23;
                                    }

                                    num27 = num26;
                                }

                                num28 = num27;
                            }

                            if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                            {
                                num22 = 2.0;
                            }
                            else
                            {
                                num22 = 1.0;
                            }

                            if (inClose[i] >= (inClose[i - 1] - ((Globals.candleSettings[10].factor * num28) / num22)))
                            {
                                int num21;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num21 = 1;
                                }
                                else
                                {
                                    num21 = -1;
                                }

                                outInteger[outIdx] = num21 * 100;
                                outIdx++;
                                goto Label_0902;
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0902:
            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                double num19;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    double num16;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs((double) (inClose[EqualTrailingIdx - 1] - inOpen[EqualTrailingIdx - 1]));
            }
            else
            {
                double num14;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[EqualTrailingIdx - 1] - inLow[EqualTrailingIdx - 1];
                }
                else
                {
                    double num11;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[EqualTrailingIdx - 1] >= inOpen[EqualTrailingIdx - 1])
                        {
                            num13 = inClose[EqualTrailingIdx - 1];
                        }
                        else
                        {
                            num13 = inOpen[EqualTrailingIdx - 1];
                        }

                        if (inClose[EqualTrailingIdx - 1] >= inOpen[EqualTrailingIdx - 1])
                        {
                            num12 = inOpen[EqualTrailingIdx - 1];
                        }
                        else
                        {
                            num12 = inClose[EqualTrailingIdx - 1];
                        }

                        num11 = (inHigh[EqualTrailingIdx - 1] - num13) + (num12 - inLow[EqualTrailingIdx - 1]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            EqualPeriodTotal += num20 - num15;
            for (int totIdx = 1; totIdx >= 0; totIdx--)
            {
                double num5;
                double num10;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs((double) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    double num9;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num6;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs((double) (inClose[BodyLongTrailingIdx - totIdx] - inOpen[BodyLongTrailingIdx - totIdx]));
                }
                else
                {
                    double num4;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[BodyLongTrailingIdx - totIdx] - inLow[BodyLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num2;
                            double num3;
                            if (inClose[BodyLongTrailingIdx - totIdx] >= inOpen[BodyLongTrailingIdx - totIdx])
                            {
                                num3 = inClose[BodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[BodyLongTrailingIdx - totIdx];
                            }

                            if (inClose[BodyLongTrailingIdx - totIdx] >= inOpen[BodyLongTrailingIdx - totIdx])
                            {
                                num2 = inOpen[BodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[BodyLongTrailingIdx - totIdx];
                            }

                            num = (inHigh[BodyLongTrailingIdx - totIdx] - num3) + (num2 - inLow[BodyLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                BodyLongPeriodTotal[totIdx] += num10 - num5;
            }

            i++;
            EqualTrailingIdx++;
            BodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0345;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlCounterAttack(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num15;
            float num20;
            int num50;
            double[] BodyLongPeriodTotal = new double[2];
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

            int lookbackTotal = CdlCounterAttackLookback();
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

            double EqualPeriodTotal = 0.0;
            int EqualTrailingIdx = startIdx - Globals.candleSettings[10].avgPeriod;
            BodyLongPeriodTotal[1] = 0.0;
            BodyLongPeriodTotal[0] = 0.0;
            int BodyLongTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            int i = EqualTrailingIdx;
            while (true)
            {
                float num65;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                {
                    num65 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num64;
                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                    {
                        num64 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num61;
                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            float num62;
                            float num63;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num63 = inClose[i - 1];
                            }
                            else
                            {
                                num63 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num62 = inOpen[i - 1];
                            }
                            else
                            {
                                num62 = inClose[i - 1];
                            }

                            num61 = (inHigh[i - 1] - num63) + (num62 - inLow[i - 1]);
                        }
                        else
                        {
                            num61 = 0.0f;
                        }

                        num64 = num61;
                    }

                    num65 = num64;
                }

                EqualPeriodTotal += num65;
                i++;
            }

            i = BodyLongTrailingIdx;
            while (true)
            {
                float num55;
                float num60;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num60 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num59;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num59 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num56;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num57;
                            float num58;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num58 = inClose[i - 1];
                            }
                            else
                            {
                                num58 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num57 = inOpen[i - 1];
                            }
                            else
                            {
                                num57 = inClose[i - 1];
                            }

                            num56 = (inHigh[i - 1] - num58) + (num57 - inLow[i - 1]);
                        }
                        else
                        {
                            num56 = 0.0f;
                        }

                        num59 = num56;
                    }

                    num60 = num59;
                }

                BodyLongPeriodTotal[1] += num60;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num55 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num54;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num54 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num51;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num52;
                            float num53;
                            if (inClose[i] >= inOpen[i])
                            {
                                num53 = inClose[i];
                            }
                            else
                            {
                                num53 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num52 = inOpen[i];
                            }
                            else
                            {
                                num52 = inClose[i];
                            }

                            num51 = (inHigh[i] - num53) + (num52 - inLow[i]);
                        }
                        else
                        {
                            num51 = 0.0f;
                        }

                        num54 = num51;
                    }

                    num55 = num54;
                }

                BodyLongPeriodTotal[0] += num55;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_036F:
            if (inClose[i] >= inOpen[i])
            {
                num50 = 1;
            }
            else
            {
                num50 = -1;
            }

            if (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -num50)
            {
                double num43;
                double num49;
                if (Globals.candleSettings[0].avgPeriod != 0.0)
                {
                    num49 = BodyLongPeriodTotal[1] / ((double) Globals.candleSettings[0].avgPeriod);
                }
                else
                {
                    float num48;
                    if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                    {
                        num48 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                    }
                    else
                    {
                        float num47;
                        if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                        {
                            num47 = inHigh[i - 1] - inLow[i - 1];
                        }
                        else
                        {
                            float num44;
                            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                            {
                                float num45;
                                float num46;
                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num46 = inClose[i - 1];
                                }
                                else
                                {
                                    num46 = inOpen[i - 1];
                                }

                                if (inClose[i - 1] >= inOpen[i - 1])
                                {
                                    num45 = inOpen[i - 1];
                                }
                                else
                                {
                                    num45 = inClose[i - 1];
                                }

                                num44 = (inHigh[i - 1] - num46) + (num45 - inLow[i - 1]);
                            }
                            else
                            {
                                num44 = 0.0f;
                            }

                            num47 = num44;
                        }

                        num48 = num47;
                    }

                    num49 = num48;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                {
                    num43 = 2.0;
                }
                else
                {
                    num43 = 1.0;
                }

                if (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) > ((Globals.candleSettings[0].factor * num49) / num43))
                {
                    double num36;
                    double num42;
                    if (Globals.candleSettings[0].avgPeriod != 0.0)
                    {
                        num42 = BodyLongPeriodTotal[0] / ((double) Globals.candleSettings[0].avgPeriod);
                    }
                    else
                    {
                        float num41;
                        if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                        {
                            num41 = Math.Abs((float) (inClose[i] - inOpen[i]));
                        }
                        else
                        {
                            float num40;
                            if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                            {
                                num40 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                float num37;
                                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

                        num42 = num41;
                    }

                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        num36 = 2.0;
                    }
                    else
                    {
                        num36 = 1.0;
                    }

                    if (Math.Abs((float) (inClose[i] - inOpen[i])) > ((Globals.candleSettings[0].factor * num42) / num36))
                    {
                        double num29;
                        double num35;
                        if (Globals.candleSettings[10].avgPeriod != 0.0)
                        {
                            num35 = EqualPeriodTotal / ((double) Globals.candleSettings[10].avgPeriod);
                        }
                        else
                        {
                            float num34;
                            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                            {
                                num34 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                            }
                            else
                            {
                                float num33;
                                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                                {
                                    num33 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    float num30;
                                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                    {
                                        float num31;
                                        float num32;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num32 = inClose[i - 1];
                                        }
                                        else
                                        {
                                            num32 = inOpen[i - 1];
                                        }

                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num31 = inOpen[i - 1];
                                        }
                                        else
                                        {
                                            num31 = inClose[i - 1];
                                        }

                                        num30 = (inHigh[i - 1] - num32) + (num31 - inLow[i - 1]);
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

                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                        {
                            num29 = 2.0;
                        }
                        else
                        {
                            num29 = 1.0;
                        }

                        if (inClose[i] <= (inClose[i - 1] + ((Globals.candleSettings[10].factor * num35) / num29)))
                        {
                            double num22;
                            double num28;
                            if (Globals.candleSettings[10].avgPeriod != 0.0)
                            {
                                num28 = EqualPeriodTotal / ((double) Globals.candleSettings[10].avgPeriod);
                            }
                            else
                            {
                                float num27;
                                if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
                                {
                                    num27 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                                }
                                else
                                {
                                    float num26;
                                    if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                                    {
                                        num26 = inHigh[i - 1] - inLow[i - 1];
                                    }
                                    else
                                    {
                                        float num23;
                                        if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                                        {
                                            float num24;
                                            float num25;
                                            if (inClose[i - 1] >= inOpen[i - 1])
                                            {
                                                num25 = inClose[i - 1];
                                            }
                                            else
                                            {
                                                num25 = inOpen[i - 1];
                                            }

                                            if (inClose[i - 1] >= inOpen[i - 1])
                                            {
                                                num24 = inOpen[i - 1];
                                            }
                                            else
                                            {
                                                num24 = inClose[i - 1];
                                            }

                                            num23 = (inHigh[i - 1] - num25) + (num24 - inLow[i - 1]);
                                        }
                                        else
                                        {
                                            num23 = 0.0f;
                                        }

                                        num26 = num23;
                                    }

                                    num27 = num26;
                                }

                                num28 = num27;
                            }

                            if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                            {
                                num22 = 2.0;
                            }
                            else
                            {
                                num22 = 1.0;
                            }

                            if (inClose[i] >= (inClose[i - 1] - ((Globals.candleSettings[10].factor * num28) / num22)))
                            {
                                int num21;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num21 = 1;
                                }
                                else
                                {
                                    num21 = -1;
                                }

                                outInteger[outIdx] = num21 * 100;
                                outIdx++;
                                goto Label_0976;
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0976:
            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
            }
            else
            {
                float num19;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i - 1] - inLow[i - 1];
                }
                else
                {
                    float num16;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[10].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs((float) (inClose[EqualTrailingIdx - 1] - inOpen[EqualTrailingIdx - 1]));
            }
            else
            {
                float num14;
                if (Globals.candleSettings[10].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[EqualTrailingIdx - 1] - inLow[EqualTrailingIdx - 1];
                }
                else
                {
                    float num11;
                    if (Globals.candleSettings[10].rangeType == RangeType.Shadows)
                    {
                        float num12;
                        float num13;
                        if (inClose[EqualTrailingIdx - 1] >= inOpen[EqualTrailingIdx - 1])
                        {
                            num13 = inClose[EqualTrailingIdx - 1];
                        }
                        else
                        {
                            num13 = inOpen[EqualTrailingIdx - 1];
                        }

                        if (inClose[EqualTrailingIdx - 1] >= inOpen[EqualTrailingIdx - 1])
                        {
                            num12 = inOpen[EqualTrailingIdx - 1];
                        }
                        else
                        {
                            num12 = inClose[EqualTrailingIdx - 1];
                        }

                        num11 = (inHigh[EqualTrailingIdx - 1] - num13) + (num12 - inLow[EqualTrailingIdx - 1]);
                    }
                    else
                    {
                        num11 = 0.0f;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            EqualPeriodTotal += num20 - num15;
            for (int totIdx = 1; totIdx >= 0; totIdx--)
            {
                float num5;
                float num10;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs((float) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    float num9;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        float num6;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
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

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs((float) (inClose[BodyLongTrailingIdx - totIdx] - inOpen[BodyLongTrailingIdx - totIdx]));
                }
                else
                {
                    float num4;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[BodyLongTrailingIdx - totIdx] - inLow[BodyLongTrailingIdx - totIdx];
                    }
                    else
                    {
                        float num;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num2;
                            float num3;
                            if (inClose[BodyLongTrailingIdx - totIdx] >= inOpen[BodyLongTrailingIdx - totIdx])
                            {
                                num3 = inClose[BodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num3 = inOpen[BodyLongTrailingIdx - totIdx];
                            }

                            if (inClose[BodyLongTrailingIdx - totIdx] >= inOpen[BodyLongTrailingIdx - totIdx])
                            {
                                num2 = inOpen[BodyLongTrailingIdx - totIdx];
                            }
                            else
                            {
                                num2 = inClose[BodyLongTrailingIdx - totIdx];
                            }

                            num = (inHigh[BodyLongTrailingIdx - totIdx] - num3) + (num2 - inLow[BodyLongTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num = 0.0f;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                BodyLongPeriodTotal[totIdx] += num10 - num5;
            }

            i++;
            EqualTrailingIdx++;
            BodyLongTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_036F;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlCounterAttackLookback()
        {
            return (((Globals.candleSettings[10].avgPeriod <= Globals.candleSettings[0].avgPeriod)
                        ? Globals.candleSettings[0].avgPeriod
                        : Globals.candleSettings[10].avgPeriod) + 1);
        }
    }
}
