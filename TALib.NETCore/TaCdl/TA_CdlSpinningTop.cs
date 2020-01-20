using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlSpinningTop(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            double num5;
            double num10;
            double num20;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlSpinningTopLookback();
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

            double bodyPeriodTotal = default;
            int bodyTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            int i = bodyTrailingIdx;
            while (true)
            {
                double num25;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num25 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num24;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num24 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num21;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num22;
                            double num23;
                            if (inClose[i] >= inOpen[i])
                            {
                                num23 = inClose[i];
                            }
                            else
                            {
                                num23 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num22 = inOpen[i];
                            }
                            else
                            {
                                num22 = inClose[i];
                            }

                            num21 = inHigh[i] - num23 + (num22 - inLow[i]);
                        }
                        else
                        {
                            num21 = 0.0;
                        }

                        num24 = num21;
                    }

                    num25 = num24;
                }

                bodyPeriodTotal += num25;
                i++;
            }

            int outIdx = default;
            Label_0147:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
            {
                num20 = bodyPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            }
            else
            {
                double num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num19 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num18;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num18 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num15;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            double num16;
                            double num17;
                            if (inClose[i] >= inOpen[i])
                            {
                                num17 = inClose[i];
                            }
                            else
                            {
                                num17 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num16 = inOpen[i];
                            }
                            else
                            {
                                num16 = inClose[i];
                            }

                            num15 = inHigh[i] - num17 + (num16 - inLow[i]);
                        }
                        else
                        {
                            num15 = 0.0;
                        }

                        num18 = num15;
                    }

                    num19 = num18;
                }

                num20 = num19;
            }

            var num14 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2.0 : 1.0;

            if (Math.Abs(inClose[i] - inOpen[i]) < Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num20 / num14)
            {
                double num13;
                if (inClose[i] >= inOpen[i])
                {
                    num13 = inClose[i];
                }
                else
                {
                    num13 = inOpen[i];
                }

                if (inHigh[i] - num13 > Math.Abs(inClose[i] - inOpen[i]))
                {
                    double num12;
                    if (inClose[i] >= inOpen[i])
                    {
                        num12 = inOpen[i];
                    }
                    else
                    {
                        num12 = inClose[i];
                    }

                    if (num12 - inLow[i] > Math.Abs(inClose[i] - inOpen[i]))
                    {
                        int num11;
                        if (inClose[i] >= inOpen[i])
                        {
                            num11 = 1;
                        }
                        else
                        {
                            num11 = -1;
                        }

                        outInteger[outIdx] = num11 * 100;
                        outIdx++;
                        goto Label_0304;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0304:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
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

                        num6 = inHigh[i] - num8 + (num7 - inLow[i]);
                    }
                    else
                    {
                        num6 = 0.0;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[bodyTrailingIdx] - inOpen[bodyTrailingIdx]);
            }
            else
            {
                double num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyTrailingIdx] - inLow[bodyTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num3 = inClose[bodyTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[bodyTrailingIdx];
                        }

                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num2 = inOpen[bodyTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[bodyTrailingIdx];
                        }

                        num = inHigh[bodyTrailingIdx] - num3 + (num2 - inLow[bodyTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            bodyPeriodTotal += num10 - num5;
            i++;
            bodyTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0147;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlSpinningTop(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            decimal num5;
            decimal num10;
            decimal num20;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlSpinningTopLookback();
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

            decimal bodyPeriodTotal = default;
            int bodyTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            int i = bodyTrailingIdx;
            while (true)
            {
                decimal num25;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num25 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num24;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num24 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num21;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num22;
                            decimal num23;
                            if (inClose[i] >= inOpen[i])
                            {
                                num23 = inClose[i];
                            }
                            else
                            {
                                num23 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num22 = inOpen[i];
                            }
                            else
                            {
                                num22 = inClose[i];
                            }

                            num21 = inHigh[i] - num23 + (num22 - inLow[i]);
                        }
                        else
                        {
                            num21 = Decimal.Zero;
                        }

                        num24 = num21;
                    }

                    num25 = num24;
                }

                bodyPeriodTotal += num25;
                i++;
            }

            int outIdx = default;
            Label_0155:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod != 0)
            {
                num20 = bodyPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
            }
            else
            {
                decimal num19;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
                {
                    num19 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num18;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                    {
                        num18 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num15;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                        {
                            decimal num16;
                            decimal num17;
                            if (inClose[i] >= inOpen[i])
                            {
                                num17 = inClose[i];
                            }
                            else
                            {
                                num17 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num16 = inOpen[i];
                            }
                            else
                            {
                                num16 = inClose[i];
                            }

                            num15 = inHigh[i] - num17 + (num16 - inLow[i]);
                        }
                        else
                        {
                            num15 = Decimal.Zero;
                        }

                        num18 = num15;
                    }

                    num19 = num18;
                }

                num20 = num19;
            }

            var num14 = Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows ? 2m : 1m;

            if (Math.Abs(inClose[i] - inOpen[i]) <
                (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyShort].Factor * num20 / num14)
            {
                decimal num13;
                if (inClose[i] >= inOpen[i])
                {
                    num13 = inClose[i];
                }
                else
                {
                    num13 = inOpen[i];
                }

                if (inHigh[i] - num13 > Math.Abs(inClose[i] - inOpen[i]))
                {
                    decimal num12;
                    if (inClose[i] >= inOpen[i])
                    {
                        num12 = inOpen[i];
                    }
                    else
                    {
                        num12 = inClose[i];
                    }

                    if (num12 - inLow[i] > Math.Abs(inClose[i] - inOpen[i]))
                    {
                        int num11;
                        if (inClose[i] >= inOpen[i])
                        {
                            num11 = 1;
                        }
                        else
                        {
                            num11 = -1;
                        }

                        outInteger[outIdx] = num11 * 100;
                        outIdx++;
                        goto Label_0336;
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_0336:
            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                decimal num9;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    decimal num6;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        decimal num7;
                        decimal num8;
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

                        num6 = inHigh[i] - num8 + (num7 - inLow[i]);
                    }
                    else
                    {
                        num6 = Decimal.Zero;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[bodyTrailingIdx] - inOpen[bodyTrailingIdx]);
            }
            else
            {
                decimal num4;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.HighLow)
                {
                    num4 = inHigh[bodyTrailingIdx] - inLow[bodyTrailingIdx];
                }
                else
                {
                    decimal num;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyShort].RangeType == RangeType.Shadows)
                    {
                        decimal num2;
                        decimal num3;
                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num3 = inClose[bodyTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[bodyTrailingIdx];
                        }

                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num2 = inOpen[bodyTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[bodyTrailingIdx];
                        }

                        num = inHigh[bodyTrailingIdx] - num3 + (num2 - inLow[bodyTrailingIdx]);
                    }
                    else
                    {
                        num = Decimal.Zero;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            bodyPeriodTotal += num10 - num5;
            i++;
            bodyTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0155;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlSpinningTopLookback()
        {
            return Globals.CandleSettings[(int) CandleSettingType.BodyShort].AvgPeriod;
        }
    }
}
