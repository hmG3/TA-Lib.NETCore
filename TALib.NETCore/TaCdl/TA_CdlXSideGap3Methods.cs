namespace TALib
{
    public partial class Core
    {
        public static RetCode CdlXSideGap3Methods(int startIdx, int endIdx, double[] inOpen, double[] inHigh, double[] inLow,
            double[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int num;
            double num2;
            double num3;
            double num6;
            double num7;
            double num8;
            double num9;
            int num10;
            int num11;
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

            int lookbackTotal = CdlXSideGap3MethodsLookback();
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

            int i = startIdx;
            int outIdx = default;
            Label_0063:
            if (inClose[i - 1] >= inOpen[i - 1])
            {
                num11 = 1;
            }
            else
            {
                num11 = -1;
            }

            if ((inClose[i - 2] < inOpen[i - 2] ? -1 : 1) != num11)
            {
                goto Label_0229;
            }

            if (inClose[i] >= inOpen[i])
            {
                num10 = 1;
            }
            else
            {
                num10 = -1;
            }

            if ((inClose[i - 1] < inOpen[i - 1] ? -1 : 1) != -num10)
            {
                goto Label_0229;
            }

            if (inClose[i - 1] > inOpen[i - 1])
            {
                num9 = inClose[i - 1];
            }
            else
            {
                num9 = inOpen[i - 1];
            }

            if (inOpen[i] >= num9)
            {
                goto Label_0229;
            }

            if (inClose[i - 1] < inOpen[i - 1])
            {
                num8 = inClose[i - 1];
            }
            else
            {
                num8 = inOpen[i - 1];
            }

            if (inOpen[i] <= num8)
            {
                goto Label_0229;
            }

            if (inClose[i - 2] > inOpen[i - 2])
            {
                num7 = inClose[i - 2];
            }
            else
            {
                num7 = inOpen[i - 2];
            }

            if (inClose[i] >= num7)
            {
                goto Label_0229;
            }

            if (inClose[i - 2] < inOpen[i - 2])
            {
                num6 = inClose[i - 2];
            }
            else
            {
                num6 = inOpen[i - 2];
            }

            if (inClose[i] <= num6)
            {
                goto Label_0229;
            }

            if (inClose[i - 2] >= inOpen[i - 2])
            {
                double num4;
                double num5;
                if (inOpen[i - 1] < inClose[i - 1])
                {
                    num5 = inOpen[i - 1];
                }
                else
                {
                    num5 = inClose[i - 1];
                }

                if (inOpen[i - 2] > inClose[i - 2])
                {
                    num4 = inOpen[i - 2];
                }
                else
                {
                    num4 = inClose[i - 2];
                }

                if (num5 > num4)
                {
                    goto Label_0208;
                }
            }

            if (inClose[i - 2] >= inOpen[i - 2])
            {
                goto Label_0229;
            }

            if (inOpen[i - 1] > inClose[i - 1])
            {
                num3 = inOpen[i - 1];
            }
            else
            {
                num3 = inClose[i - 1];
            }

            if (inOpen[i - 2] < inClose[i - 2])
            {
                num2 = inOpen[i - 2];
            }
            else
            {
                num2 = inClose[i - 2];
            }

            if (num3 >= num2)
            {
                goto Label_0229;
            }

            Label_0208:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                num = 1;
            }
            else
            {
                num = -1;
            }

            outInteger[outIdx] = num * 100;
            outIdx++;
            goto Label_0232;
            Label_0229:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_0232:
            i++;
            if (i <= endIdx)
            {
                goto Label_0063;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode CdlXSideGap3Methods(int startIdx, int endIdx, decimal[] inOpen, decimal[] inHigh, decimal[] inLow,
            decimal[] inClose, ref int outBegIdx, ref int outNBElement, int[] outInteger)
        {
            int num;
            decimal num2;
            decimal num3;
            decimal num6;
            decimal num7;
            decimal num8;
            decimal num9;
            int num10;
            int num11;
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

            int lookbackTotal = CdlXSideGap3MethodsLookback();
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

            int i = startIdx;
            int outIdx = default;
            Label_0063:
            if (inClose[i - 1] >= inOpen[i - 1])
            {
                num11 = 1;
            }
            else
            {
                num11 = -1;
            }

            if ((inClose[i - 2] < inOpen[i - 2] ? -1 : 1) != num11)
            {
                goto Label_0263;
            }

            if (inClose[i] >= inOpen[i])
            {
                num10 = 1;
            }
            else
            {
                num10 = -1;
            }

            if ((inClose[i - 1] < inOpen[i - 1] ? -1 : 1) != -num10)
            {
                goto Label_0263;
            }

            if (inClose[i - 1] > inOpen[i - 1])
            {
                num9 = inClose[i - 1];
            }
            else
            {
                num9 = inOpen[i - 1];
            }

            if (inOpen[i] >= num9)
            {
                goto Label_0263;
            }

            if (inClose[i - 1] < inOpen[i - 1])
            {
                num8 = inClose[i - 1];
            }
            else
            {
                num8 = inOpen[i - 1];
            }

            if (inOpen[i] <= num8)
            {
                goto Label_0263;
            }

            if (inClose[i - 2] > inOpen[i - 2])
            {
                num7 = inClose[i - 2];
            }
            else
            {
                num7 = inOpen[i - 2];
            }

            if (inClose[i] >= num7)
            {
                goto Label_0263;
            }

            if (inClose[i - 2] < inOpen[i - 2])
            {
                num6 = inClose[i - 2];
            }
            else
            {
                num6 = inOpen[i - 2];
            }

            if (inClose[i] <= num6)
            {
                goto Label_0263;
            }

            if (inClose[i - 2] >= inOpen[i - 2])
            {
                decimal num4;
                decimal num5;
                if (inOpen[i - 1] < inClose[i - 1])
                {
                    num5 = inOpen[i - 1];
                }
                else
                {
                    num5 = inClose[i - 1];
                }

                if (inOpen[i - 2] > inClose[i - 2])
                {
                    num4 = inOpen[i - 2];
                }
                else
                {
                    num4 = inClose[i - 2];
                }

                if (num5 > num4)
                {
                    goto Label_0240;
                }
            }

            if (inClose[i - 2] >= inOpen[i - 2])
            {
                goto Label_0263;
            }

            if (inOpen[i - 1] > inClose[i - 1])
            {
                num3 = inOpen[i - 1];
            }
            else
            {
                num3 = inClose[i - 1];
            }

            if (inOpen[i - 2] < inClose[i - 2])
            {
                num2 = inOpen[i - 2];
            }
            else
            {
                num2 = inClose[i - 2];
            }

            if (num3 >= num2)
            {
                goto Label_0263;
            }

            Label_0240:
            if (inClose[i - 2] >= inOpen[i - 2])
            {
                num = 1;
            }
            else
            {
                num = -1;
            }

            outInteger[outIdx] = num * 100;
            outIdx++;
            goto Label_026C;
            Label_0263:
            outInteger[outIdx] = 0;
            outIdx++;
            Label_026C:
            i++;
            if (i <= endIdx)
            {
                goto Label_0063;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlXSideGap3MethodsLookback()
        {
            return 2;
        }
    }
}
