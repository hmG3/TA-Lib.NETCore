using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlDoji(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow, double[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
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

            int lookbackTotal = CdlDojiLookback();
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

            double bodyDojiPeriodTotal = default;
            int bodyDojiTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
            int i = bodyDojiTrailingIdx;
            while (true)
            {
                double num22;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num22 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num21;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num21 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num18;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            double num19;
                            double num20;
                            if (inClose[i] >= inOpen[i])
                            {
                                num20 = inClose[i];
                            }
                            else
                            {
                                num20 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num19 = inOpen[i];
                            }
                            else
                            {
                                num19 = inClose[i];
                            }

                            num18 = inHigh[i] - num20 + (num19 - inLow[i]);
                        }
                        else
                        {
                            num18 = 0.0;
                        }

                        num21 = num18;
                    }

                    num22 = num21;
                }

                bodyDojiPeriodTotal += num22;
                i++;
            }

            int outIdx = default;
            do
            {
                double num5;
                double num10;
                double num17;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod != 0)
                {
                    num17 = bodyDojiPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
                }
                else
                {
                    double num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                    {
                        num16 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        double num15;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                        {
                            num15 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num12;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
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

                                num12 = inHigh[i] - num14 + (num13 - inLow[i]);
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

                var num11 = Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows ? 2.0 : 1.0;

                if (Math.Abs(inClose[i] - inOpen[i]) <= Globals.CandleSettings[(int) CandleSettingType.BodyDoji].Factor * num17 / num11)
                {
                    outInteger[outIdx] = 100;
                    outIdx++;
                }
                else
                {
                    outInteger[outIdx] = 0;
                    outIdx++;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num9;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num6;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
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

                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs(inClose[bodyDojiTrailingIdx] - inOpen[bodyDojiTrailingIdx]);
                }
                else
                {
                    double num4;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[bodyDojiTrailingIdx] - inLow[bodyDojiTrailingIdx];
                    }
                    else
                    {
                        double num;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            double num2;
                            double num3;
                            if (inClose[bodyDojiTrailingIdx] >= inOpen[bodyDojiTrailingIdx])
                            {
                                num3 = inClose[bodyDojiTrailingIdx];
                            }
                            else
                            {
                                num3 = inOpen[bodyDojiTrailingIdx];
                            }

                            if (inClose[bodyDojiTrailingIdx] >= inOpen[bodyDojiTrailingIdx])
                            {
                                num2 = inOpen[bodyDojiTrailingIdx];
                            }
                            else
                            {
                                num2 = inClose[bodyDojiTrailingIdx];
                            }

                            num = inHigh[bodyDojiTrailingIdx] - num3 + (num2 - inLow[bodyDojiTrailingIdx]);
                        }
                        else
                        {
                            num = 0.0;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                bodyDojiPeriodTotal += num10 - num5;
                i++;
                bodyDojiTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlDoji(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow, decimal[] inClose,
            ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
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

            int lookbackTotal = CdlDojiLookback();
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

            decimal bodyDojiPeriodTotal = default;
            int bodyDojiTrailingIdx = startIdx - Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
            int i = bodyDojiTrailingIdx;
            while (true)
            {
                decimal num22;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num22 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num21;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num21 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num18;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            decimal num19;
                            decimal num20;
                            if (inClose[i] >= inOpen[i])
                            {
                                num20 = inClose[i];
                            }
                            else
                            {
                                num20 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num19 = inOpen[i];
                            }
                            else
                            {
                                num19 = inClose[i];
                            }

                            num18 = inHigh[i] - num20 + (num19 - inLow[i]);
                        }
                        else
                        {
                            num18 = Decimal.Zero;
                        }

                        num21 = num18;
                    }

                    num22 = num21;
                }

                bodyDojiPeriodTotal += num22;
                i++;
            }

            int outIdx = default;
            do
            {
                decimal num5;
                decimal num10;
                decimal num17;
                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod != 0)
                {
                    num17 = bodyDojiPeriodTotal / Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
                }
                else
                {
                    decimal num16;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                    {
                        num16 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        decimal num15;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                        {
                            num15 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            decimal num12;
                            if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                            {
                                decimal num13;
                                decimal num14;
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

                                num12 = inHigh[i] - num14 + (num13 - inLow[i]);
                            }
                            else
                            {
                                num12 = Decimal.Zero;
                            }

                            num15 = num12;
                        }

                        num16 = num15;
                    }

                    num17 = num16;
                }

                var num11 = Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows ? 2m : 1m;

                if (Math.Abs(inClose[i] - inOpen[i]) <=
                    (decimal) Globals.CandleSettings[(int) CandleSettingType.BodyDoji].Factor * num17 / num11)
                {
                    outInteger[outIdx] = 100;
                    outIdx++;
                }
                else
                {
                    outInteger[outIdx] = 0;
                    outIdx++;
                }

                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num10 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    decimal num9;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num9 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        decimal num6;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
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

                if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.RealBody)
                {
                    num5 = Math.Abs(inClose[bodyDojiTrailingIdx] - inOpen[bodyDojiTrailingIdx]);
                }
                else
                {
                    decimal num4;
                    if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.HighLow)
                    {
                        num4 = inHigh[bodyDojiTrailingIdx] - inLow[bodyDojiTrailingIdx];
                    }
                    else
                    {
                        decimal num;
                        if (Globals.CandleSettings[(int) CandleSettingType.BodyDoji].RangeType == RangeType.Shadows)
                        {
                            decimal num2;
                            decimal num3;
                            if (inClose[bodyDojiTrailingIdx] >= inOpen[bodyDojiTrailingIdx])
                            {
                                num3 = inClose[bodyDojiTrailingIdx];
                            }
                            else
                            {
                                num3 = inOpen[bodyDojiTrailingIdx];
                            }

                            if (inClose[bodyDojiTrailingIdx] >= inOpen[bodyDojiTrailingIdx])
                            {
                                num2 = inOpen[bodyDojiTrailingIdx];
                            }
                            else
                            {
                                num2 = inClose[bodyDojiTrailingIdx];
                            }

                            num = inHigh[bodyDojiTrailingIdx] - num3 + (num2 - inLow[bodyDojiTrailingIdx]);
                        }
                        else
                        {
                            num = Decimal.Zero;
                        }

                        num4 = num;
                    }

                    num5 = num4;
                }

                bodyDojiPeriodTotal += num10 - num5;
                i++;
                bodyDojiTrailingIdx++;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlDojiLookback()
        {
            return Globals.CandleSettings[(int) CandleSettingType.BodyDoji].AvgPeriod;
        }
    }
}
