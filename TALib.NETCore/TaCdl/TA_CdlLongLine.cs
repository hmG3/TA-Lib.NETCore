using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlLongLine(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            double num38;
            double num44;
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

            int lookbackTotal = CdlLongLineLookback();
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
            int BodyTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            double ShadowPeriodTotal = 0.0;
            int ShadowTrailingIdx = startIdx - Globals.candleSettings[6].avgPeriod;
            int i = BodyTrailingIdx;
            while (true)
            {
                double num54;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num54 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num53;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num53 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num50;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num51;
                            double num52;
                            if (inClose[i] >= inOpen[i])
                            {
                                num52 = inClose[i];
                            }
                            else
                            {
                                num52 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num51 = inOpen[i];
                            }
                            else
                            {
                                num51 = inClose[i];
                            }

                            num50 = (inHigh[i] - num52) + (num51 - inLow[i]);
                        }
                        else
                        {
                            num50 = 0.0;
                        }

                        num53 = num50;
                    }

                    num54 = num53;
                }

                BodyPeriodTotal += num54;
                i++;
            }

            i = ShadowTrailingIdx;
            while (true)
            {
                double num49;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                {
                    num49 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num48;
                    if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                    {
                        num48 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num45;
                        if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
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

                ShadowPeriodTotal += num49;
                i++;
            }

            int outIdx = 0;
            Label_022E:
            if (Globals.candleSettings[0].avgPeriod != 0.0)
            {
                num44 = BodyPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
            }
            else
            {
                double num43;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num43 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num42;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num42 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num39;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num40;
                            double num41;
                            if (inClose[i] >= inOpen[i])
                            {
                                num41 = inClose[i];
                            }
                            else
                            {
                                num41 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num40 = inOpen[i];
                            }
                            else
                            {
                                num40 = inClose[i];
                            }

                            num39 = (inHigh[i] - num41) + (num40 - inLow[i]);
                        }
                        else
                        {
                            num39 = 0.0;
                        }

                        num42 = num39;
                    }

                    num43 = num42;
                }

                num44 = num43;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
            {
                num38 = 2.0;
            }
            else
            {
                num38 = 1.0;
            }

            if (Math.Abs((double) (inClose[i] - inOpen[i])) > ((Globals.candleSettings[0].factor * num44) / num38))
            {
                double num30;
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

                if (Globals.candleSettings[6].avgPeriod != 0.0)
                {
                    num36 = ShadowPeriodTotal / ((double) Globals.candleSettings[6].avgPeriod);
                }
                else
                {
                    double num35;
                    if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                    {
                        num35 = Math.Abs((double) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        double num34;
                        if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                        {
                            num34 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num31;
                            if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                            {
                                double num32;
                                double num33;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num33 = inClose[i];
                                }
                                else
                                {
                                    num33 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num32 = inOpen[i];
                                }
                                else
                                {
                                    num32 = inClose[i];
                                }

                                num31 = (inHigh[i] - num33) + (num32 - inLow[i]);
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

                if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                {
                    num30 = 2.0;
                }
                else
                {
                    num30 = 1.0;
                }

                if ((inHigh[i] - num37) < ((Globals.candleSettings[6].factor * num36) / num30))
                {
                    double num22;
                    double num28;
                    double num29;
                    if (inClose[i] >= inOpen[i])
                    {
                        num29 = inOpen[i];
                    }
                    else
                    {
                        num29 = inClose[i];
                    }

                    if (Globals.candleSettings[6].avgPeriod != 0.0)
                    {
                        num28 = ShadowPeriodTotal / ((double) Globals.candleSettings[6].avgPeriod);
                    }
                    else
                    {
                        double num27;
                        if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                        {
                            num27 = Math.Abs((double) (inClose[i] - inOpen[i]));
                        }
                        else
                        {
                            double num26;
                            if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                            {
                                num26 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num23;
                                if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                                {
                                    double num24;
                                    double num25;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num25 = inClose[i];
                                    }
                                    else
                                    {
                                        num25 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num24 = inOpen[i];
                                    }
                                    else
                                    {
                                        num24 = inClose[i];
                                    }

                                    num23 = (inHigh[i] - num25) + (num24 - inLow[i]);
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

                    if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                    {
                        num22 = 2.0;
                    }
                    else
                    {
                        num22 = 1.0;
                    }

                    if ((num29 - inLow[i]) < ((Globals.candleSettings[6].factor * num28) / num22))
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
                        goto Label_062D;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_062D:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((double) (inClose[i] - inOpen[i]));
            }
            else
            {
                double num19;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num16;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num17;
                        double num18;
                        if (inClose[i] >= inOpen[i])
                        {
                            num18 = inClose[i];
                        }
                        else
                        {
                            num18 = inOpen[i];
                        }

                        if (inClose[i] >= inOpen[i])
                        {
                            num17 = inOpen[i];
                        }
                        else
                        {
                            num17 = inClose[i];
                        }

                        num16 = (inHigh[i] - num18) + (num17 - inLow[i]);
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
                num15 = Math.Abs((double) (inClose[BodyTrailingIdx] - inOpen[BodyTrailingIdx]));
            }
            else
            {
                double num14;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[BodyTrailingIdx] - inLow[BodyTrailingIdx];
                }
                else
                {
                    double num11;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[BodyTrailingIdx] >= inOpen[BodyTrailingIdx])
                        {
                            num13 = inClose[BodyTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[BodyTrailingIdx];
                        }

                        if (inClose[BodyTrailingIdx] >= inOpen[BodyTrailingIdx])
                        {
                            num12 = inOpen[BodyTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[BodyTrailingIdx];
                        }

                        num11 = (inHigh[BodyTrailingIdx] - num13) + (num12 - inLow[BodyTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            BodyPeriodTotal += num20 - num15;
            if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((double) (inClose[i] - inOpen[i]));
            }
            else
            {
                double num9;
                if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((double) (inClose[ShadowTrailingIdx] - inOpen[ShadowTrailingIdx]));
            }
            else
            {
                double num4;
                if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[ShadowTrailingIdx] - inLow[ShadowTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[ShadowTrailingIdx] >= inOpen[ShadowTrailingIdx])
                        {
                            num3 = inClose[ShadowTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[ShadowTrailingIdx];
                        }

                        if (inClose[ShadowTrailingIdx] >= inOpen[ShadowTrailingIdx])
                        {
                            num2 = inOpen[ShadowTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[ShadowTrailingIdx];
                        }

                        num = (inHigh[ShadowTrailingIdx] - num3) + (num2 - inLow[ShadowTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            ShadowPeriodTotal += num10 - num5;
            i++;
            BodyTrailingIdx++;
            ShadowTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_022E;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlLongLine(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num5;
            float num10;
            float num15;
            float num20;
            double num38;
            double num44;
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

            int lookbackTotal = CdlLongLineLookback();
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
            int BodyTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            double ShadowPeriodTotal = 0.0;
            int ShadowTrailingIdx = startIdx - Globals.candleSettings[6].avgPeriod;
            int i = BodyTrailingIdx;
            while (true)
            {
                float num54;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num54 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num53;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num53 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num50;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num51;
                            float num52;
                            if (inClose[i] >= inOpen[i])
                            {
                                num52 = inClose[i];
                            }
                            else
                            {
                                num52 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num51 = inOpen[i];
                            }
                            else
                            {
                                num51 = inClose[i];
                            }

                            num50 = (inHigh[i] - num52) + (num51 - inLow[i]);
                        }
                        else
                        {
                            num50 = 0.0f;
                        }

                        num53 = num50;
                    }

                    num54 = num53;
                }

                BodyPeriodTotal += num54;
                i++;
            }

            i = ShadowTrailingIdx;
            while (true)
            {
                float num49;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                {
                    num49 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num48;
                    if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                    {
                        num48 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num45;
                        if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
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

                ShadowPeriodTotal += num49;
                i++;
            }

            int outIdx = 0;
            Label_024A:
            if (Globals.candleSettings[0].avgPeriod != 0.0)
            {
                num44 = BodyPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
            }
            else
            {
                float num43;
                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num43 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num42;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num42 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num39;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num40;
                            float num41;
                            if (inClose[i] >= inOpen[i])
                            {
                                num41 = inClose[i];
                            }
                            else
                            {
                                num41 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num40 = inOpen[i];
                            }
                            else
                            {
                                num40 = inClose[i];
                            }

                            num39 = (inHigh[i] - num41) + (num40 - inLow[i]);
                        }
                        else
                        {
                            num39 = 0.0f;
                        }

                        num42 = num39;
                    }

                    num43 = num42;
                }

                num44 = num43;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
            {
                num38 = 2.0;
            }
            else
            {
                num38 = 1.0;
            }

            if (Math.Abs((float) (inClose[i] - inOpen[i])) > ((Globals.candleSettings[0].factor * num44) / num38))
            {
                double num30;
                double num36;
                float num37;
                if (inClose[i] >= inOpen[i])
                {
                    num37 = inClose[i];
                }
                else
                {
                    num37 = inOpen[i];
                }

                if (Globals.candleSettings[6].avgPeriod != 0.0)
                {
                    num36 = ShadowPeriodTotal / ((double) Globals.candleSettings[6].avgPeriod);
                }
                else
                {
                    float num35;
                    if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                    {
                        num35 = Math.Abs((float) (inClose[i] - inOpen[i]));
                    }
                    else
                    {
                        float num34;
                        if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                        {
                            num34 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            float num31;
                            if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                            {
                                float num32;
                                float num33;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num33 = inClose[i];
                                }
                                else
                                {
                                    num33 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num32 = inOpen[i];
                                }
                                else
                                {
                                    num32 = inClose[i];
                                }

                                num31 = (inHigh[i] - num33) + (num32 - inLow[i]);
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

                if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                {
                    num30 = 2.0;
                }
                else
                {
                    num30 = 1.0;
                }

                if ((inHigh[i] - num37) < ((Globals.candleSettings[6].factor * num36) / num30))
                {
                    double num22;
                    double num28;
                    float num29;
                    if (inClose[i] >= inOpen[i])
                    {
                        num29 = inOpen[i];
                    }
                    else
                    {
                        num29 = inClose[i];
                    }

                    if (Globals.candleSettings[6].avgPeriod != 0.0)
                    {
                        num28 = ShadowPeriodTotal / ((double) Globals.candleSettings[6].avgPeriod);
                    }
                    else
                    {
                        float num27;
                        if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
                        {
                            num27 = Math.Abs((float) (inClose[i] - inOpen[i]));
                        }
                        else
                        {
                            float num26;
                            if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                            {
                                num26 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                float num23;
                                if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                                {
                                    float num24;
                                    float num25;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num25 = inClose[i];
                                    }
                                    else
                                    {
                                        num25 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num24 = inOpen[i];
                                    }
                                    else
                                    {
                                        num24 = inClose[i];
                                    }

                                    num23 = (inHigh[i] - num25) + (num24 - inLow[i]);
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

                    if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                    {
                        num22 = 2.0;
                    }
                    else
                    {
                        num22 = 1.0;
                    }

                    if ((num29 - inLow[i]) < ((Globals.candleSettings[6].factor * num28) / num22))
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
                        goto Label_0681;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0681:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs((float) (inClose[i] - inOpen[i]));
            }
            else
            {
                float num19;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    float num16;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        float num17;
                        float num18;
                        if (inClose[i] >= inOpen[i])
                        {
                            num18 = inClose[i];
                        }
                        else
                        {
                            num18 = inOpen[i];
                        }

                        if (inClose[i] >= inOpen[i])
                        {
                            num17 = inOpen[i];
                        }
                        else
                        {
                            num17 = inClose[i];
                        }

                        num16 = (inHigh[i] - num18) + (num17 - inLow[i]);
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
                num15 = Math.Abs((float) (inClose[BodyTrailingIdx] - inOpen[BodyTrailingIdx]));
            }
            else
            {
                float num14;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[BodyTrailingIdx] - inLow[BodyTrailingIdx];
                }
                else
                {
                    float num11;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        float num12;
                        float num13;
                        if (inClose[BodyTrailingIdx] >= inOpen[BodyTrailingIdx])
                        {
                            num13 = inClose[BodyTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[BodyTrailingIdx];
                        }

                        if (inClose[BodyTrailingIdx] >= inOpen[BodyTrailingIdx])
                        {
                            num12 = inOpen[BodyTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[BodyTrailingIdx];
                        }

                        num11 = (inHigh[BodyTrailingIdx] - num13) + (num12 - inLow[BodyTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0f;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            BodyPeriodTotal += num20 - num15;
            if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs((float) (inClose[i] - inOpen[i]));
            }
            else
            {
                float num9;
                if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    float num6;
                    if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
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

            if (Globals.candleSettings[6].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs((float) (inClose[ShadowTrailingIdx] - inOpen[ShadowTrailingIdx]));
            }
            else
            {
                float num4;
                if (Globals.candleSettings[6].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[ShadowTrailingIdx] - inLow[ShadowTrailingIdx];
                }
                else
                {
                    float num;
                    if (Globals.candleSettings[6].rangeType == RangeType.Shadows)
                    {
                        float num2;
                        float num3;
                        if (inClose[ShadowTrailingIdx] >= inOpen[ShadowTrailingIdx])
                        {
                            num3 = inClose[ShadowTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[ShadowTrailingIdx];
                        }

                        if (inClose[ShadowTrailingIdx] >= inOpen[ShadowTrailingIdx])
                        {
                            num2 = inOpen[ShadowTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[ShadowTrailingIdx];
                        }

                        num = (inHigh[ShadowTrailingIdx] - num3) + (num2 - inLow[ShadowTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0f;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            ShadowPeriodTotal += num10 - num5;
            i++;
            BodyTrailingIdx++;
            ShadowTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_024A;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlLongLineLookback()
        {
            return ((Globals.candleSettings[0].avgPeriod <= Globals.candleSettings[6].avgPeriod)
                ? Globals.candleSettings[6].avgPeriod
                : Globals.candleSettings[0].avgPeriod);
        }
    }
}
