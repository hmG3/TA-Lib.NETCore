using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Cdl3StarsInSouth(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num25;
            double num30;
            double num35;
            double num40;
            double[] ShadowVeryShortPeriodTotal = new double[2];
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

            int lookbackTotal = Cdl3StarsInSouthLookback();
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
            int BodyLongTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            double ShadowLongPeriodTotal = 0.0;
            int ShadowLongTrailingIdx = startIdx - Globals.candleSettings[4].avgPeriod;
            ShadowVeryShortPeriodTotal[1] = 0.0;
            ShadowVeryShortPeriodTotal[0] = 0.0;
            int ShadowVeryShortTrailingIdx = startIdx - Globals.candleSettings[7].avgPeriod;
            double BodyShortPeriodTotal = 0.0;
            int BodyShortTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                double num111;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num111 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num110;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num110 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num107;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            double num108;
                            double num109;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num109 = inClose[i - 2];
                            }
                            else
                            {
                                num109 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num108 = inOpen[i - 2];
                            }
                            else
                            {
                                num108 = inClose[i - 2];
                            }

                            num107 = (inHigh[i - 2] - num109) + (num108 - inLow[i - 2]);
                        }
                        else
                        {
                            num107 = 0.0;
                        }

                        num110 = num107;
                    }

                    num111 = num110;
                }

                BodyLongPeriodTotal += num111;
                i++;
            }

            i = ShadowLongTrailingIdx;
            while (true)
            {
                double num106;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                {
                    num106 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    double num105;
                    if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                    {
                        num105 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        double num102;
                        if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                        {
                            double num103;
                            double num104;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num104 = inClose[i - 2];
                            }
                            else
                            {
                                num104 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num103 = inOpen[i - 2];
                            }
                            else
                            {
                                num103 = inClose[i - 2];
                            }

                            num102 = (inHigh[i - 2] - num104) + (num103 - inLow[i - 2]);
                        }
                        else
                        {
                            num102 = 0.0;
                        }

                        num105 = num102;
                    }

                    num106 = num105;
                }

                ShadowLongPeriodTotal += num106;
                i++;
            }

            i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                double num96;
                double num101;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num101 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    double num100;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num100 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        double num97;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num98;
                            double num99;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num99 = inClose[i - 1];
                            }
                            else
                            {
                                num99 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num98 = inOpen[i - 1];
                            }
                            else
                            {
                                num98 = inClose[i - 1];
                            }

                            num97 = (inHigh[i - 1] - num99) + (num98 - inLow[i - 1]);
                        }
                        else
                        {
                            num97 = 0.0;
                        }

                        num100 = num97;
                    }

                    num101 = num100;
                }

                ShadowVeryShortPeriodTotal[1] += num101;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num96 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num95;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num95 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num92;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num93;
                            double num94;
                            if (inClose[i] >= inOpen[i])
                            {
                                num94 = inClose[i];
                            }
                            else
                            {
                                num94 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num93 = inOpen[i];
                            }
                            else
                            {
                                num93 = inClose[i];
                            }

                            num92 = (inHigh[i] - num94) + (num93 - inLow[i]);
                        }
                        else
                        {
                            num92 = 0.0;
                        }

                        num95 = num92;
                    }

                    num96 = num95;
                }

                ShadowVeryShortPeriodTotal[0] += num96;
                i++;
            }

            i = BodyShortTrailingIdx;
            while (true)
            {
                double num91;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num91 = Math.Abs((double) (inClose[i] - inOpen[i]));
                }
                else
                {
                    double num90;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num90 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num87;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            double num88;
                            double num89;
                            if (inClose[i] >= inOpen[i])
                            {
                                num89 = inClose[i];
                            }
                            else
                            {
                                num89 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num88 = inOpen[i];
                            }
                            else
                            {
                                num88 = inClose[i];
                            }

                            num87 = (inHigh[i] - num89) + (num88 - inLow[i]);
                        }
                        else
                        {
                            num87 = 0.0;
                        }

                        num90 = num87;
                    }

                    num91 = num90;
                }

                BodyShortPeriodTotal += num91;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_052C:
            if (((((inClose[i - 2] < inOpen[i - 2]) ? -1 : 1) == -1) && (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1)) &&
                (((inClose[i] < inOpen[i]) ? -1 : 1) == -1))
            {
                double num80;
                double num86;
                if (Globals.candleSettings[0].avgPeriod != 0.0)
                {
                    num86 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
                }
                else
                {
                    double num85;
                    if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                    {
                        num85 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        double num84;
                        if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                        {
                            num84 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            double num81;
                            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                            {
                                double num82;
                                double num83;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num83 = inClose[i - 2];
                                }
                                else
                                {
                                    num83 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num82 = inOpen[i - 2];
                                }
                                else
                                {
                                    num82 = inClose[i - 2];
                                }

                                num81 = (inHigh[i - 2] - num83) + (num82 - inLow[i - 2]);
                            }
                            else
                            {
                                num81 = 0.0;
                            }

                            num84 = num81;
                        }

                        num85 = num84;
                    }

                    num86 = num85;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                {
                    num80 = 2.0;
                }
                else
                {
                    num80 = 1.0;
                }

                if (Math.Abs((double) (inClose[i - 2] - inOpen[i - 2])) > ((Globals.candleSettings[0].factor * num86) / num80))
                {
                    double num72;
                    double num78;
                    double num79;
                    if (inClose[i - 2] >= inOpen[i - 2])
                    {
                        num79 = inOpen[i - 2];
                    }
                    else
                    {
                        num79 = inClose[i - 2];
                    }

                    if (Globals.candleSettings[4].avgPeriod != 0.0)
                    {
                        num78 = ShadowLongPeriodTotal / ((double) Globals.candleSettings[4].avgPeriod);
                    }
                    else
                    {
                        double num77;
                        if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                        {
                            num77 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
                        }
                        else
                        {
                            double num76;
                            if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                            {
                                num76 = inHigh[i - 2] - inLow[i - 2];
                            }
                            else
                            {
                                double num73;
                                if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                                {
                                    double num74;
                                    double num75;
                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num75 = inClose[i - 2];
                                    }
                                    else
                                    {
                                        num75 = inOpen[i - 2];
                                    }

                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num74 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num74 = inClose[i - 2];
                                    }

                                    num73 = (inHigh[i - 2] - num75) + (num74 - inLow[i - 2]);
                                }
                                else
                                {
                                    num73 = 0.0;
                                }

                                num76 = num73;
                            }

                            num77 = num76;
                        }

                        num78 = num77;
                    }

                    if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                    {
                        num72 = 2.0;
                    }
                    else
                    {
                        num72 = 1.0;
                    }

                    if (((((num79 - inLow[i - 2]) > ((Globals.candleSettings[4].factor * num78) / num72)) &&
                          (Math.Abs((double) (inClose[i - 1] - inOpen[i - 1])) < Math.Abs((double) (inClose[i - 2] - inOpen[i - 2])))) &&
                         ((inOpen[i - 1] > inClose[i - 2]) && (inOpen[i - 1] <= inHigh[i - 2]))) &&
                        ((inLow[i - 1] < inClose[i - 2]) && (inLow[i - 1] >= inLow[i - 2])))
                    {
                        double num64;
                        double num70;
                        double num71;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num71 = inOpen[i - 1];
                        }
                        else
                        {
                            num71 = inClose[i - 1];
                        }

                        if (Globals.candleSettings[7].avgPeriod != 0.0)
                        {
                            num70 = ShadowVeryShortPeriodTotal[1] / ((double) Globals.candleSettings[7].avgPeriod);
                        }
                        else
                        {
                            double num69;
                            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                            {
                                num69 = Math.Abs((double) (inClose[i - 1] - inOpen[i - 1]));
                            }
                            else
                            {
                                double num68;
                                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                {
                                    num68 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    double num65;
                                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                    {
                                        double num66;
                                        double num67;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num67 = inClose[i - 1];
                                        }
                                        else
                                        {
                                            num67 = inOpen[i - 1];
                                        }

                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num66 = inOpen[i - 1];
                                        }
                                        else
                                        {
                                            num66 = inClose[i - 1];
                                        }

                                        num65 = (inHigh[i - 1] - num67) + (num66 - inLow[i - 1]);
                                    }
                                    else
                                    {
                                        num65 = 0.0;
                                    }

                                    num68 = num65;
                                }

                                num69 = num68;
                            }

                            num70 = num69;
                        }

                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            num64 = 2.0;
                        }
                        else
                        {
                            num64 = 1.0;
                        }

                        if ((num71 - inLow[i - 1]) > ((Globals.candleSettings[7].factor * num70) / num64))
                        {
                            double num57;
                            double num63;
                            if (Globals.candleSettings[2].avgPeriod != 0.0)
                            {
                                num63 = BodyShortPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
                            }
                            else
                            {
                                double num62;
                                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                                {
                                    num62 = Math.Abs((double) (inClose[i] - inOpen[i]));
                                }
                                else
                                {
                                    double num61;
                                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                                    {
                                        num61 = inHigh[i] - inLow[i];
                                    }
                                    else
                                    {
                                        double num58;
                                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                        {
                                            double num59;
                                            double num60;
                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num60 = inClose[i];
                                            }
                                            else
                                            {
                                                num60 = inOpen[i];
                                            }

                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num59 = inOpen[i];
                                            }
                                            else
                                            {
                                                num59 = inClose[i];
                                            }

                                            num58 = (inHigh[i] - num60) + (num59 - inLow[i]);
                                        }
                                        else
                                        {
                                            num58 = 0.0;
                                        }

                                        num61 = num58;
                                    }

                                    num62 = num61;
                                }

                                num63 = num62;
                            }

                            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                            {
                                num57 = 2.0;
                            }
                            else
                            {
                                num57 = 1.0;
                            }

                            if (Math.Abs((double) (inClose[i] - inOpen[i])) < ((Globals.candleSettings[2].factor * num63) / num57))
                            {
                                double num49;
                                double num55;
                                double num56;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num56 = inOpen[i];
                                }
                                else
                                {
                                    num56 = inClose[i];
                                }

                                if (Globals.candleSettings[7].avgPeriod != 0.0)
                                {
                                    num55 = ShadowVeryShortPeriodTotal[0] / ((double) Globals.candleSettings[7].avgPeriod);
                                }
                                else
                                {
                                    double num54;
                                    if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                                    {
                                        num54 = Math.Abs((double) (inClose[i] - inOpen[i]));
                                    }
                                    else
                                    {
                                        double num53;
                                        if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                        {
                                            num53 = inHigh[i] - inLow[i];
                                        }
                                        else
                                        {
                                            double num50;
                                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                                    num55 = num54;
                                }

                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                {
                                    num49 = 2.0;
                                }
                                else
                                {
                                    num49 = 1.0;
                                }

                                if ((num56 - inLow[i]) < ((Globals.candleSettings[7].factor * num55) / num49))
                                {
                                    double num41;
                                    double num47;
                                    double num48;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num48 = inClose[i];
                                    }
                                    else
                                    {
                                        num48 = inOpen[i];
                                    }

                                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                                    {
                                        num47 = ShadowVeryShortPeriodTotal[0] / ((double) Globals.candleSettings[7].avgPeriod);
                                    }
                                    else
                                    {
                                        double num46;
                                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                                        {
                                            num46 = Math.Abs((double) (inClose[i] - inOpen[i]));
                                        }
                                        else
                                        {
                                            double num45;
                                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                            {
                                                num45 = inHigh[i] - inLow[i];
                                            }
                                            else
                                            {
                                                double num42;
                                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                                        num47 = num46;
                                    }

                                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                    {
                                        num41 = 2.0;
                                    }
                                    else
                                    {
                                        num41 = 1.0;
                                    }

                                    if ((((inHigh[i] - num48) < ((Globals.candleSettings[7].factor * num47) / num41)) &&
                                         (inLow[i] > inLow[i - 1])) && (inHigh[i] < inHigh[i - 1]))
                                    {
                                        outInteger[outIdx] = 100;
                                        outIdx++;
                                        goto Label_0E31;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0E31:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num40 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
            }
            else
            {
                double num39;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num39 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    double num36;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num37;
                        double num38;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num38 = inClose[i - 2];
                        }
                        else
                        {
                            num38 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num37 = inOpen[i - 2];
                        }
                        else
                        {
                            num37 = inClose[i - 2];
                        }

                        num36 = (inHigh[i - 2] - num38) + (num37 - inLow[i - 2]);
                    }
                    else
                    {
                        num36 = 0.0;
                    }

                    num39 = num36;
                }

                num40 = num39;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num35 = Math.Abs((double) (inClose[BodyLongTrailingIdx - 2] - inOpen[BodyLongTrailingIdx - 2]));
            }
            else
            {
                double num34;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num34 = inHigh[BodyLongTrailingIdx - 2] - inLow[BodyLongTrailingIdx - 2];
                }
                else
                {
                    double num31;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        double num32;
                        double num33;
                        if (inClose[BodyLongTrailingIdx - 2] >= inOpen[BodyLongTrailingIdx - 2])
                        {
                            num33 = inClose[BodyLongTrailingIdx - 2];
                        }
                        else
                        {
                            num33 = inOpen[BodyLongTrailingIdx - 2];
                        }

                        if (inClose[BodyLongTrailingIdx - 2] >= inOpen[BodyLongTrailingIdx - 2])
                        {
                            num32 = inOpen[BodyLongTrailingIdx - 2];
                        }
                        else
                        {
                            num32 = inClose[BodyLongTrailingIdx - 2];
                        }

                        num31 = (inHigh[BodyLongTrailingIdx - 2] - num33) + (num32 - inLow[BodyLongTrailingIdx - 2]);
                    }
                    else
                    {
                        num31 = 0.0;
                    }

                    num34 = num31;
                }

                num35 = num34;
            }

            BodyLongPeriodTotal += num40 - num35;
            if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
            {
                num30 = Math.Abs((double) (inClose[i - 2] - inOpen[i - 2]));
            }
            else
            {
                double num29;
                if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    double num26;
                    if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                    {
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

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num27 = inOpen[i - 2];
                        }
                        else
                        {
                            num27 = inClose[i - 2];
                        }

                        num26 = (inHigh[i - 2] - num28) + (num27 - inLow[i - 2]);
                    }
                    else
                    {
                        num26 = 0.0;
                    }

                    num29 = num26;
                }

                num30 = num29;
            }

            if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
            {
                num25 = Math.Abs((double) (inClose[ShadowLongTrailingIdx - 2] - inOpen[ShadowLongTrailingIdx - 2]));
            }
            else
            {
                double num24;
                if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                {
                    num24 = inHigh[ShadowLongTrailingIdx - 2] - inLow[ShadowLongTrailingIdx - 2];
                }
                else
                {
                    double num21;
                    if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                    {
                        double num22;
                        double num23;
                        if (inClose[ShadowLongTrailingIdx - 2] >= inOpen[ShadowLongTrailingIdx - 2])
                        {
                            num23 = inClose[ShadowLongTrailingIdx - 2];
                        }
                        else
                        {
                            num23 = inOpen[ShadowLongTrailingIdx - 2];
                        }

                        if (inClose[ShadowLongTrailingIdx - 2] >= inOpen[ShadowLongTrailingIdx - 2])
                        {
                            num22 = inOpen[ShadowLongTrailingIdx - 2];
                        }
                        else
                        {
                            num22 = inClose[ShadowLongTrailingIdx - 2];
                        }

                        num21 = (inHigh[ShadowLongTrailingIdx - 2] - num23) + (num22 - inLow[ShadowLongTrailingIdx - 2]);
                    }
                    else
                    {
                        num21 = 0.0;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            ShadowLongPeriodTotal += num30 - num25;
            for (int totIdx = 1; totIdx >= 0; totIdx--)
            {
                double num15;
                double num20;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num20 = Math.Abs((double) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    double num19;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num19 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        double num16;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num17;
                            double num18;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num18 = inClose[i - totIdx];
                            }
                            else
                            {
                                num18 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num17 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num17 = inClose[i - totIdx];
                            }

                            num16 = (inHigh[i - totIdx] - num18) + (num17 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num16 = 0.0;
                        }

                        num19 = num16;
                    }

                    num20 = num19;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num15 = Math.Abs((double) (inClose[ShadowVeryShortTrailingIdx - totIdx] - inOpen[ShadowVeryShortTrailingIdx - totIdx]));
                }
                else
                {
                    double num14;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num14 = inHigh[ShadowVeryShortTrailingIdx - totIdx] - inLow[ShadowVeryShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        double num11;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num12;
                            double num13;
                            if (inClose[ShadowVeryShortTrailingIdx - totIdx] >= inOpen[ShadowVeryShortTrailingIdx - totIdx])
                            {
                                num13 = inClose[ShadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num13 = inOpen[ShadowVeryShortTrailingIdx - totIdx];
                            }

                            if (inClose[ShadowVeryShortTrailingIdx - totIdx] >= inOpen[ShadowVeryShortTrailingIdx - totIdx])
                            {
                                num12 = inOpen[ShadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num12 = inClose[ShadowVeryShortTrailingIdx - totIdx];
                            }

                            num11 = (inHigh[ShadowVeryShortTrailingIdx - totIdx] - num13) +
                                    (num12 - inLow[ShadowVeryShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num11 = 0.0;
                        }

                        num14 = num11;
                    }

                    num15 = num14;
                }

                ShadowVeryShortPeriodTotal[totIdx] += num20 - num15;
            }

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
            ShadowLongTrailingIdx++;
            ShadowVeryShortTrailingIdx++;
            BodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_052C;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Cdl3StarsInSouth(int startIdx, int endIdx, float[] inOpen, float[] inHigh, float[] inLow, float[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            float num5;
            float num10;
            float num25;
            float num30;
            float num35;
            float num40;
            double[] ShadowVeryShortPeriodTotal = new double[2];
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

            int lookbackTotal = Cdl3StarsInSouthLookback();
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
            int BodyLongTrailingIdx = startIdx - Globals.candleSettings[0].avgPeriod;
            double ShadowLongPeriodTotal = 0.0;
            int ShadowLongTrailingIdx = startIdx - Globals.candleSettings[4].avgPeriod;
            ShadowVeryShortPeriodTotal[1] = 0.0;
            ShadowVeryShortPeriodTotal[0] = 0.0;
            int ShadowVeryShortTrailingIdx = startIdx - Globals.candleSettings[7].avgPeriod;
            double BodyShortPeriodTotal = 0.0;
            int BodyShortTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            int i = BodyLongTrailingIdx;
            while (true)
            {
                float num111;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                {
                    num111 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num110;
                    if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                    {
                        num110 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num107;
                        if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                        {
                            float num108;
                            float num109;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num109 = inClose[i - 2];
                            }
                            else
                            {
                                num109 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num108 = inOpen[i - 2];
                            }
                            else
                            {
                                num108 = inClose[i - 2];
                            }

                            num107 = (inHigh[i - 2] - num109) + (num108 - inLow[i - 2]);
                        }
                        else
                        {
                            num107 = 0.0f;
                        }

                        num110 = num107;
                    }

                    num111 = num110;
                }

                BodyLongPeriodTotal += num111;
                i++;
            }

            i = ShadowLongTrailingIdx;
            while (true)
            {
                float num106;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                {
                    num106 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                }
                else
                {
                    float num105;
                    if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                    {
                        num105 = inHigh[i - 2] - inLow[i - 2];
                    }
                    else
                    {
                        float num102;
                        if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                        {
                            float num103;
                            float num104;
                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num104 = inClose[i - 2];
                            }
                            else
                            {
                                num104 = inOpen[i - 2];
                            }

                            if (inClose[i - 2] >= inOpen[i - 2])
                            {
                                num103 = inOpen[i - 2];
                            }
                            else
                            {
                                num103 = inClose[i - 2];
                            }

                            num102 = (inHigh[i - 2] - num104) + (num103 - inLow[i - 2]);
                        }
                        else
                        {
                            num102 = 0.0f;
                        }

                        num105 = num102;
                    }

                    num106 = num105;
                }

                ShadowLongPeriodTotal += num106;
                i++;
            }

            i = ShadowVeryShortTrailingIdx;
            while (true)
            {
                float num96;
                float num101;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num101 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                }
                else
                {
                    float num100;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num100 = inHigh[i - 1] - inLow[i - 1];
                    }
                    else
                    {
                        float num97;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num98;
                            float num99;
                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num99 = inClose[i - 1];
                            }
                            else
                            {
                                num99 = inOpen[i - 1];
                            }

                            if (inClose[i - 1] >= inOpen[i - 1])
                            {
                                num98 = inOpen[i - 1];
                            }
                            else
                            {
                                num98 = inClose[i - 1];
                            }

                            num97 = (inHigh[i - 1] - num99) + (num98 - inLow[i - 1]);
                        }
                        else
                        {
                            num97 = 0.0f;
                        }

                        num100 = num97;
                    }

                    num101 = num100;
                }

                ShadowVeryShortPeriodTotal[1] += num101;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num96 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num95;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num95 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num92;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num93;
                            float num94;
                            if (inClose[i] >= inOpen[i])
                            {
                                num94 = inClose[i];
                            }
                            else
                            {
                                num94 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num93 = inOpen[i];
                            }
                            else
                            {
                                num93 = inClose[i];
                            }

                            num92 = (inHigh[i] - num94) + (num93 - inLow[i]);
                        }
                        else
                        {
                            num92 = 0.0f;
                        }

                        num95 = num92;
                    }

                    num96 = num95;
                }

                ShadowVeryShortPeriodTotal[0] += num96;
                i++;
            }

            i = BodyShortTrailingIdx;
            while (true)
            {
                float num91;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num91 = Math.Abs((float) (inClose[i] - inOpen[i]));
                }
                else
                {
                    float num90;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num90 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        float num87;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            float num88;
                            float num89;
                            if (inClose[i] >= inOpen[i])
                            {
                                num89 = inClose[i];
                            }
                            else
                            {
                                num89 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num88 = inOpen[i];
                            }
                            else
                            {
                                num88 = inClose[i];
                            }

                            num87 = (inHigh[i] - num89) + (num88 - inLow[i]);
                        }
                        else
                        {
                            num87 = 0.0f;
                        }

                        num90 = num87;
                    }

                    num91 = num90;
                }

                BodyShortPeriodTotal += num91;
                i++;
            }

            i = startIdx;
            int outIdx = 0;
            Label_0572:
            if (((((inClose[i - 2] < inOpen[i - 2]) ? -1 : 1) == -1) && (((inClose[i - 1] < inOpen[i - 1]) ? -1 : 1) == -1)) &&
                (((inClose[i] < inOpen[i]) ? -1 : 1) == -1))
            {
                double num80;
                double num86;
                if (Globals.candleSettings[0].avgPeriod != 0.0)
                {
                    num86 = BodyLongPeriodTotal / ((double) Globals.candleSettings[0].avgPeriod);
                }
                else
                {
                    float num85;
                    if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
                    {
                        num85 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                    }
                    else
                    {
                        float num84;
                        if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                        {
                            num84 = inHigh[i - 2] - inLow[i - 2];
                        }
                        else
                        {
                            float num81;
                            if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                            {
                                float num82;
                                float num83;
                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num83 = inClose[i - 2];
                                }
                                else
                                {
                                    num83 = inOpen[i - 2];
                                }

                                if (inClose[i - 2] >= inOpen[i - 2])
                                {
                                    num82 = inOpen[i - 2];
                                }
                                else
                                {
                                    num82 = inClose[i - 2];
                                }

                                num81 = (inHigh[i - 2] - num83) + (num82 - inLow[i - 2]);
                            }
                            else
                            {
                                num81 = 0.0f;
                            }

                            num84 = num81;
                        }

                        num85 = num84;
                    }

                    num86 = num85;
                }

                if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                {
                    num80 = 2.0;
                }
                else
                {
                    num80 = 1.0;
                }

                if (Math.Abs((float) (inClose[i - 2] - inOpen[i - 2])) > ((Globals.candleSettings[0].factor * num86) / num80))
                {
                    double num72;
                    double num78;
                    float num79;
                    if (inClose[i - 2] >= inOpen[i - 2])
                    {
                        num79 = inOpen[i - 2];
                    }
                    else
                    {
                        num79 = inClose[i - 2];
                    }

                    if (Globals.candleSettings[4].avgPeriod != 0.0)
                    {
                        num78 = ShadowLongPeriodTotal / ((double) Globals.candleSettings[4].avgPeriod);
                    }
                    else
                    {
                        float num77;
                        if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                        {
                            num77 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
                        }
                        else
                        {
                            float num76;
                            if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                            {
                                num76 = inHigh[i - 2] - inLow[i - 2];
                            }
                            else
                            {
                                float num73;
                                if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                                {
                                    float num74;
                                    float num75;
                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num75 = inClose[i - 2];
                                    }
                                    else
                                    {
                                        num75 = inOpen[i - 2];
                                    }

                                    if (inClose[i - 2] >= inOpen[i - 2])
                                    {
                                        num74 = inOpen[i - 2];
                                    }
                                    else
                                    {
                                        num74 = inClose[i - 2];
                                    }

                                    num73 = (inHigh[i - 2] - num75) + (num74 - inLow[i - 2]);
                                }
                                else
                                {
                                    num73 = 0.0f;
                                }

                                num76 = num73;
                            }

                            num77 = num76;
                        }

                        num78 = num77;
                    }

                    if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                    {
                        num72 = 2.0;
                    }
                    else
                    {
                        num72 = 1.0;
                    }

                    if (((((num79 - inLow[i - 2]) > ((Globals.candleSettings[4].factor * num78) / num72)) &&
                          (Math.Abs((float) (inClose[i - 1] - inOpen[i - 1])) < Math.Abs((float) (inClose[i - 2] - inOpen[i - 2])))) &&
                         ((inOpen[i - 1] > inClose[i - 2]) && (inOpen[i - 1] <= inHigh[i - 2]))) &&
                        ((inLow[i - 1] < inClose[i - 2]) && (inLow[i - 1] >= inLow[i - 2])))
                    {
                        double num64;
                        double num70;
                        float num71;
                        if (inClose[i - 1] >= inOpen[i - 1])
                        {
                            num71 = inOpen[i - 1];
                        }
                        else
                        {
                            num71 = inClose[i - 1];
                        }

                        if (Globals.candleSettings[7].avgPeriod != 0.0)
                        {
                            num70 = ShadowVeryShortPeriodTotal[1] / ((double) Globals.candleSettings[7].avgPeriod);
                        }
                        else
                        {
                            float num69;
                            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                            {
                                num69 = Math.Abs((float) (inClose[i - 1] - inOpen[i - 1]));
                            }
                            else
                            {
                                float num68;
                                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                {
                                    num68 = inHigh[i - 1] - inLow[i - 1];
                                }
                                else
                                {
                                    float num65;
                                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                    {
                                        float num66;
                                        float num67;
                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num67 = inClose[i - 1];
                                        }
                                        else
                                        {
                                            num67 = inOpen[i - 1];
                                        }

                                        if (inClose[i - 1] >= inOpen[i - 1])
                                        {
                                            num66 = inOpen[i - 1];
                                        }
                                        else
                                        {
                                            num66 = inClose[i - 1];
                                        }

                                        num65 = (inHigh[i - 1] - num67) + (num66 - inLow[i - 1]);
                                    }
                                    else
                                    {
                                        num65 = 0.0f;
                                    }

                                    num68 = num65;
                                }

                                num69 = num68;
                            }

                            num70 = num69;
                        }

                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            num64 = 2.0;
                        }
                        else
                        {
                            num64 = 1.0;
                        }

                        if ((num71 - inLow[i - 1]) > ((Globals.candleSettings[7].factor * num70) / num64))
                        {
                            double num57;
                            double num63;
                            if (Globals.candleSettings[2].avgPeriod != 0.0)
                            {
                                num63 = BodyShortPeriodTotal / ((double) Globals.candleSettings[2].avgPeriod);
                            }
                            else
                            {
                                float num62;
                                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                                {
                                    num62 = Math.Abs((float) (inClose[i] - inOpen[i]));
                                }
                                else
                                {
                                    float num61;
                                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                                    {
                                        num61 = inHigh[i] - inLow[i];
                                    }
                                    else
                                    {
                                        float num58;
                                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                                        {
                                            float num59;
                                            float num60;
                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num60 = inClose[i];
                                            }
                                            else
                                            {
                                                num60 = inOpen[i];
                                            }

                                            if (inClose[i] >= inOpen[i])
                                            {
                                                num59 = inOpen[i];
                                            }
                                            else
                                            {
                                                num59 = inClose[i];
                                            }

                                            num58 = (inHigh[i] - num60) + (num59 - inLow[i]);
                                        }
                                        else
                                        {
                                            num58 = 0.0f;
                                        }

                                        num61 = num58;
                                    }

                                    num62 = num61;
                                }

                                num63 = num62;
                            }

                            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                            {
                                num57 = 2.0;
                            }
                            else
                            {
                                num57 = 1.0;
                            }

                            if (Math.Abs((float) (inClose[i] - inOpen[i])) < ((Globals.candleSettings[2].factor * num63) / num57))
                            {
                                double num49;
                                double num55;
                                float num56;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num56 = inOpen[i];
                                }
                                else
                                {
                                    num56 = inClose[i];
                                }

                                if (Globals.candleSettings[7].avgPeriod != 0.0)
                                {
                                    num55 = ShadowVeryShortPeriodTotal[0] / ((double) Globals.candleSettings[7].avgPeriod);
                                }
                                else
                                {
                                    float num54;
                                    if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                                    {
                                        num54 = Math.Abs((float) (inClose[i] - inOpen[i]));
                                    }
                                    else
                                    {
                                        float num53;
                                        if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                        {
                                            num53 = inHigh[i] - inLow[i];
                                        }
                                        else
                                        {
                                            float num50;
                                            if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                                    num55 = num54;
                                }

                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                {
                                    num49 = 2.0;
                                }
                                else
                                {
                                    num49 = 1.0;
                                }

                                if ((num56 - inLow[i]) < ((Globals.candleSettings[7].factor * num55) / num49))
                                {
                                    double num41;
                                    double num47;
                                    float num48;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num48 = inClose[i];
                                    }
                                    else
                                    {
                                        num48 = inOpen[i];
                                    }

                                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                                    {
                                        num47 = ShadowVeryShortPeriodTotal[0] / ((double) Globals.candleSettings[7].avgPeriod);
                                    }
                                    else
                                    {
                                        float num46;
                                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                                        {
                                            num46 = Math.Abs((float) (inClose[i] - inOpen[i]));
                                        }
                                        else
                                        {
                                            float num45;
                                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                                            {
                                                num45 = inHigh[i] - inLow[i];
                                            }
                                            else
                                            {
                                                float num42;
                                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
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

                                        num47 = num46;
                                    }

                                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                    {
                                        num41 = 2.0;
                                    }
                                    else
                                    {
                                        num41 = 1.0;
                                    }

                                    if ((((inHigh[i] - num48) < ((Globals.candleSettings[7].factor * num47) / num41)) &&
                                         (inLow[i] > inLow[i - 1])) && (inHigh[i] < inHigh[i - 1]))
                                    {
                                        outInteger[outIdx] = 100;
                                        outIdx++;
                                        goto Label_0EFD;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0EFD:
            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num40 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
            }
            else
            {
                float num39;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num39 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    float num36;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        float num37;
                        float num38;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num38 = inClose[i - 2];
                        }
                        else
                        {
                            num38 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num37 = inOpen[i - 2];
                        }
                        else
                        {
                            num37 = inClose[i - 2];
                        }

                        num36 = (inHigh[i - 2] - num38) + (num37 - inLow[i - 2]);
                    }
                    else
                    {
                        num36 = 0.0f;
                    }

                    num39 = num36;
                }

                num40 = num39;
            }

            if (Globals.candleSettings[0].rangeType == RangeType.RealBody)
            {
                num35 = Math.Abs((float) (inClose[BodyLongTrailingIdx - 2] - inOpen[BodyLongTrailingIdx - 2]));
            }
            else
            {
                float num34;
                if (Globals.candleSettings[0].rangeType == RangeType.HighLow)
                {
                    num34 = inHigh[BodyLongTrailingIdx - 2] - inLow[BodyLongTrailingIdx - 2];
                }
                else
                {
                    float num31;
                    if (Globals.candleSettings[0].rangeType == RangeType.Shadows)
                    {
                        float num32;
                        float num33;
                        if (inClose[BodyLongTrailingIdx - 2] >= inOpen[BodyLongTrailingIdx - 2])
                        {
                            num33 = inClose[BodyLongTrailingIdx - 2];
                        }
                        else
                        {
                            num33 = inOpen[BodyLongTrailingIdx - 2];
                        }

                        if (inClose[BodyLongTrailingIdx - 2] >= inOpen[BodyLongTrailingIdx - 2])
                        {
                            num32 = inOpen[BodyLongTrailingIdx - 2];
                        }
                        else
                        {
                            num32 = inClose[BodyLongTrailingIdx - 2];
                        }

                        num31 = (inHigh[BodyLongTrailingIdx - 2] - num33) + (num32 - inLow[BodyLongTrailingIdx - 2]);
                    }
                    else
                    {
                        num31 = 0.0f;
                    }

                    num34 = num31;
                }

                num35 = num34;
            }

            BodyLongPeriodTotal += num40 - num35;
            if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
            {
                num30 = Math.Abs((float) (inClose[i - 2] - inOpen[i - 2]));
            }
            else
            {
                float num29;
                if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i - 2] - inLow[i - 2];
                }
                else
                {
                    float num26;
                    if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                    {
                        float num27;
                        float num28;
                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num28 = inClose[i - 2];
                        }
                        else
                        {
                            num28 = inOpen[i - 2];
                        }

                        if (inClose[i - 2] >= inOpen[i - 2])
                        {
                            num27 = inOpen[i - 2];
                        }
                        else
                        {
                            num27 = inClose[i - 2];
                        }

                        num26 = (inHigh[i - 2] - num28) + (num27 - inLow[i - 2]);
                    }
                    else
                    {
                        num26 = 0.0f;
                    }

                    num29 = num26;
                }

                num30 = num29;
            }

            if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
            {
                num25 = Math.Abs((float) (inClose[ShadowLongTrailingIdx - 2] - inOpen[ShadowLongTrailingIdx - 2]));
            }
            else
            {
                float num24;
                if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                {
                    num24 = inHigh[ShadowLongTrailingIdx - 2] - inLow[ShadowLongTrailingIdx - 2];
                }
                else
                {
                    float num21;
                    if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                    {
                        float num22;
                        float num23;
                        if (inClose[ShadowLongTrailingIdx - 2] >= inOpen[ShadowLongTrailingIdx - 2])
                        {
                            num23 = inClose[ShadowLongTrailingIdx - 2];
                        }
                        else
                        {
                            num23 = inOpen[ShadowLongTrailingIdx - 2];
                        }

                        if (inClose[ShadowLongTrailingIdx - 2] >= inOpen[ShadowLongTrailingIdx - 2])
                        {
                            num22 = inOpen[ShadowLongTrailingIdx - 2];
                        }
                        else
                        {
                            num22 = inClose[ShadowLongTrailingIdx - 2];
                        }

                        num21 = (inHigh[ShadowLongTrailingIdx - 2] - num23) + (num22 - inLow[ShadowLongTrailingIdx - 2]);
                    }
                    else
                    {
                        num21 = 0.0f;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            ShadowLongPeriodTotal += num30 - num25;
            for (int totIdx = 1; totIdx >= 0; totIdx--)
            {
                float num15;
                float num20;
                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num20 = Math.Abs((float) (inClose[i - totIdx] - inOpen[i - totIdx]));
                }
                else
                {
                    float num19;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num19 = inHigh[i - totIdx] - inLow[i - totIdx];
                    }
                    else
                    {
                        float num16;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num17;
                            float num18;
                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num18 = inClose[i - totIdx];
                            }
                            else
                            {
                                num18 = inOpen[i - totIdx];
                            }

                            if (inClose[i - totIdx] >= inOpen[i - totIdx])
                            {
                                num17 = inOpen[i - totIdx];
                            }
                            else
                            {
                                num17 = inClose[i - totIdx];
                            }

                            num16 = (inHigh[i - totIdx] - num18) + (num17 - inLow[i - totIdx]);
                        }
                        else
                        {
                            num16 = 0.0f;
                        }

                        num19 = num16;
                    }

                    num20 = num19;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num15 = Math.Abs((float) (inClose[ShadowVeryShortTrailingIdx - totIdx] - inOpen[ShadowVeryShortTrailingIdx - totIdx]));
                }
                else
                {
                    float num14;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num14 = inHigh[ShadowVeryShortTrailingIdx - totIdx] - inLow[ShadowVeryShortTrailingIdx - totIdx];
                    }
                    else
                    {
                        float num11;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            float num12;
                            float num13;
                            if (inClose[ShadowVeryShortTrailingIdx - totIdx] >= inOpen[ShadowVeryShortTrailingIdx - totIdx])
                            {
                                num13 = inClose[ShadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num13 = inOpen[ShadowVeryShortTrailingIdx - totIdx];
                            }

                            if (inClose[ShadowVeryShortTrailingIdx - totIdx] >= inOpen[ShadowVeryShortTrailingIdx - totIdx])
                            {
                                num12 = inOpen[ShadowVeryShortTrailingIdx - totIdx];
                            }
                            else
                            {
                                num12 = inClose[ShadowVeryShortTrailingIdx - totIdx];
                            }

                            num11 = (inHigh[ShadowVeryShortTrailingIdx - totIdx] - num13) +
                                    (num12 - inLow[ShadowVeryShortTrailingIdx - totIdx]);
                        }
                        else
                        {
                            num11 = 0.0f;
                        }

                        num14 = num11;
                    }

                    num15 = num14;
                }

                ShadowVeryShortPeriodTotal[totIdx] += num20 - num15;
            }

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
            ShadowLongTrailingIdx++;
            ShadowVeryShortTrailingIdx++;
            BodyShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0572;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int Cdl3StarsInSouthLookback()
        {
            int num;
            int avgPeriod;
            if (Globals.candleSettings[0].avgPeriod > Globals.candleSettings[2].avgPeriod)
            {
                avgPeriod = Globals.candleSettings[0].avgPeriod;
            }
            else
            {
                avgPeriod = Globals.candleSettings[2].avgPeriod;
            }

            if (((Globals.candleSettings[7].avgPeriod <= Globals.candleSettings[4].avgPeriod)
                    ? Globals.candleSettings[4].avgPeriod
                    : Globals.candleSettings[7].avgPeriod) > avgPeriod)
            {
                num = (Globals.candleSettings[7].avgPeriod <= Globals.candleSettings[4].avgPeriod)
                    ? Globals.candleSettings[4].avgPeriod
                    : Globals.candleSettings[7].avgPeriod;
            }
            else
            {
                num = (Globals.candleSettings[0].avgPeriod <= Globals.candleSettings[2].avgPeriod)
                    ? Globals.candleSettings[2].avgPeriod
                    : Globals.candleSettings[0].avgPeriod;
            }

            return (num + 2);
        }
    }
}
