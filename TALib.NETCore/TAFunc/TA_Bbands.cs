using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Bbands(int startIdx, int endIdx, double[] inReal, MAType optInMAType, ref int outBegIdx, ref int outNBElement,
            double[] outRealUpperBand, double[] outRealMiddleBand, double[] outRealLowerBand, int optInTimePeriod = 5,
            double optInNbDevUp = 2.0, double optInNbDevDn = 2.0)
        {
            int i;
            double tempReal2;
            double tempReal;
            double[] tempBuffer2;
            double[] tempBuffer1;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outRealUpperBand == null)
            {
                return RetCode.BadParam;
            }

            if (outRealMiddleBand == null)
            {
                return RetCode.BadParam;
            }

            if (outRealLowerBand == null)
            {
                return RetCode.BadParam;
            }

            if (inReal == outRealUpperBand)
            {
                tempBuffer1 = outRealMiddleBand;
                tempBuffer2 = outRealLowerBand;
            }
            else if (inReal == outRealLowerBand)
            {
                tempBuffer1 = outRealMiddleBand;
                tempBuffer2 = outRealUpperBand;
            }
            else if (inReal == outRealMiddleBand)
            {
                tempBuffer1 = outRealLowerBand;
                tempBuffer2 = outRealUpperBand;
            }
            else
            {
                tempBuffer1 = outRealMiddleBand;
                tempBuffer2 = outRealUpperBand;
            }

            if (tempBuffer1 == inReal || tempBuffer2 == inReal)
            {
                return RetCode.BadParam;
            }

            RetCode retCode = MovingAverage(startIdx, endIdx, inReal, optInMAType, ref outBegIdx, ref outNBElement, tempBuffer1,
                optInTimePeriod);
            if (retCode != RetCode.Success || outNBElement == 0)
            {
                outNBElement = 0;
                return retCode;
            }

            if (optInMAType == MAType.Sma)
            {
                TA_INT_stddev_using_precalc_ma(inReal, tempBuffer1, outBegIdx, outNBElement, optInTimePeriod, tempBuffer2);
            }
            else
            {
                retCode = StdDev(outBegIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, tempBuffer2, optInTimePeriod);
                if (retCode != RetCode.Success)
                {
                    outNBElement = 0;
                    return retCode;
                }
            }

            if (tempBuffer1 != outRealMiddleBand)
            {
                Array.Copy(tempBuffer1, 0, outRealMiddleBand, 0, outNBElement);
            }

            if (!optInNbDevUp.Equals(optInNbDevDn))
            {
                if (!optInNbDevUp.Equals(1.0))
                {
                    if (!optInNbDevDn.Equals(1.0))
                    {
                        i = 0;
                        while (i < outNBElement)
                        {
                            tempReal = tempBuffer2[i];
                            tempReal2 = outRealMiddleBand[i];
                            outRealUpperBand[i] = tempReal2 + tempReal * optInNbDevUp;
                            outRealLowerBand[i] = tempReal2 - tempReal * optInNbDevDn;
                            i++;
                        }

                        goto Label_02B1;
                    }

                    i = 0;
                    goto Label_025E;
                }

                i = 0;
            }
            else
            {
                if (!optInNbDevUp.Equals(1.0))
                {
                    i = 0;
                }
                else
                {
                    i = 0;
                    while (i < outNBElement)
                    {
                        tempReal = tempBuffer2[i];
                        tempReal2 = outRealMiddleBand[i];
                        outRealUpperBand[i] = tempReal2 + tempReal;
                        outRealLowerBand[i] = tempReal2 - tempReal;
                        i++;
                    }

                    goto Label_02B1;
                }

                while (i < outNBElement)
                {
                    tempReal = tempBuffer2[i] * optInNbDevUp;
                    tempReal2 = outRealMiddleBand[i];
                    outRealUpperBand[i] = tempReal2 + tempReal;
                    outRealLowerBand[i] = tempReal2 - tempReal;
                    i++;
                }

                goto Label_02B1;
            }

            while (true)
            {
                if (i >= outNBElement)
                {
                    goto Label_02B1;
                }

                tempReal = tempBuffer2[i];
                tempReal2 = outRealMiddleBand[i];
                outRealUpperBand[i] = tempReal2 + tempReal;
                outRealLowerBand[i] = tempReal2 - tempReal * optInNbDevDn;
                i++;
            }

            Label_025E:
            while (i < outNBElement)
            {
                tempReal = tempBuffer2[i];
                tempReal2 = outRealMiddleBand[i];
                outRealLowerBand[i] = tempReal2 - tempReal;
                outRealUpperBand[i] = tempReal2 + tempReal * optInNbDevUp;
                i++;
            }

            Label_02B1:
            return RetCode.Success;
        }

        public static RetCode Bbands(int startIdx, int endIdx, decimal[] inReal, MAType optInMAType, ref int outBegIdx,
            ref int outNBElement, decimal[] outRealUpperBand, decimal[] outRealMiddleBand, decimal[] outRealLowerBand,
            int optInTimePeriod = 5, decimal optInNbDevUp = 2m, decimal optInNbDevDn = 2m)
        {
            int i;
            decimal tempReal2;
            decimal tempReal;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (outRealUpperBand == null)
            {
                return RetCode.BadParam;
            }

            if (outRealMiddleBand == null)
            {
                return RetCode.BadParam;
            }

            if (outRealLowerBand == null)
            {
                return RetCode.BadParam;
            }

            decimal[] tempBuffer1 = outRealMiddleBand;
            decimal[] tempBuffer2 = outRealLowerBand;
            RetCode retCode = MovingAverage(startIdx, endIdx, inReal, optInMAType, ref outBegIdx, ref outNBElement, tempBuffer1,
                optInTimePeriod);
            if (retCode != RetCode.Success || outNBElement == 0)
            {
                outNBElement = 0;
                return retCode;
            }

            if (optInMAType == MAType.Sma)
            {
                TA_INT_stddev_using_precalc_ma(inReal, tempBuffer1, outBegIdx, outNBElement, optInTimePeriod, tempBuffer2);
            }
            else
            {
                retCode = StdDev(outBegIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, tempBuffer2, optInTimePeriod);
                if (retCode != RetCode.Success)
                {
                    outNBElement = 0;
                    return retCode;
                }
            }

            if (optInNbDevUp != optInNbDevDn)
            {
                if (optInNbDevUp != Decimal.One)
                {
                    if (optInNbDevDn != Decimal.One)
                    {
                        i = 0;
                        while (i < outNBElement)
                        {
                            tempReal = tempBuffer2[i];
                            tempReal2 = outRealMiddleBand[i];
                            outRealUpperBand[i] = tempReal2 + tempReal * optInNbDevUp;
                            outRealLowerBand[i] = tempReal2 - tempReal * optInNbDevDn;
                            i++;
                        }

                        goto Label_025F;
                    }

                    i = 0;
                    goto Label_020C;
                }

                i = 0;
            }
            else
            {
                if (optInNbDevUp != Decimal.One)
                {
                    i = 0;
                }
                else
                {
                    i = 0;
                    while (i < outNBElement)
                    {
                        tempReal = tempBuffer2[i];
                        tempReal2 = outRealMiddleBand[i];
                        outRealUpperBand[i] = tempReal2 + tempReal;
                        outRealLowerBand[i] = tempReal2 - tempReal;
                        i++;
                    }

                    goto Label_025F;
                }

                while (i < outNBElement)
                {
                    tempReal = tempBuffer2[i] * optInNbDevUp;
                    tempReal2 = outRealMiddleBand[i];
                    outRealUpperBand[i] = tempReal2 + tempReal;
                    outRealLowerBand[i] = tempReal2 - tempReal;
                    i++;
                }

                goto Label_025F;
            }

            while (true)
            {
                if (i >= outNBElement)
                {
                    goto Label_025F;
                }

                tempReal = tempBuffer2[i];
                tempReal2 = outRealMiddleBand[i];
                outRealUpperBand[i] = tempReal2 + tempReal;
                outRealLowerBand[i] = tempReal2 - tempReal * optInNbDevDn;
                i++;
            }

            Label_020C:
            while (i < outNBElement)
            {
                tempReal = tempBuffer2[i];
                tempReal2 = outRealMiddleBand[i];
                outRealLowerBand[i] = tempReal2 - tempReal;
                outRealUpperBand[i] = tempReal2 + tempReal * optInNbDevUp;
                i++;
            }

            Label_025F:
            return RetCode.Success;
        }

        public static int BbandsLookback(MAType optInMAType, int optInTimePeriod = 5)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return MovingAverageLookback(optInMAType, optInTimePeriod);
        }
    }
}
