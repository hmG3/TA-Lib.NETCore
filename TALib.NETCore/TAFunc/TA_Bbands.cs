using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Bbands(int startIdx, int endIdx, double[] inReal, MAType optInMAType, ref int outBegIdx, ref int outNBElement,
            double[] outRealUpperBand, double[] outRealMiddleBand, double[] outRealLowerBand, int optInTimePeriod = 5,
            double optInNbDevUp = 2.0, double optInNbDevDn = 2.0)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outRealUpperBand == null || outRealMiddleBand == null || outRealLowerBand == null ||
                optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            double[] tempBuffer1;
            double[] tempBuffer2;
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

            RetCode retCode = Ma(startIdx, endIdx, inReal, optInMAType, ref outBegIdx, ref outNBElement, tempBuffer1,
                optInTimePeriod);
            if (retCode != RetCode.Success || outNBElement == 0)
            {
                outNBElement = 0;
                return retCode;
            }

            if (optInMAType == MAType.Sma)
            {
                TA_INT_StdDevUsingPrecalcMA(inReal, tempBuffer1, outBegIdx, outNBElement, optInTimePeriod, tempBuffer2);
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

            int i;
            double tempReal;
            double tempReal2;
            if (optInNbDevUp.Equals(optInNbDevDn))
            {
                if (optInNbDevUp.Equals(1.0))
                {
                    for (i = 0; i < outNBElement; i++)
                    {
                        tempReal = tempBuffer2[i];
                        tempReal2 = outRealMiddleBand[i];
                        outRealUpperBand[i] = tempReal2 + tempReal;
                        outRealLowerBand[i] = tempReal2 - tempReal;
                    }
                }
                else
                {
                    for (i = 0; i < outNBElement; i++)
                    {
                        tempReal = tempBuffer2[i] * optInNbDevUp;
                        tempReal2 = outRealMiddleBand[i];
                        outRealUpperBand[i] = tempReal2 + tempReal;
                        outRealLowerBand[i] = tempReal2 - tempReal;
                    }
                }
            }
            else if (optInNbDevUp.Equals(1.0))
            {
                for (i = 0; i < outNBElement; i++)
                {
                    tempReal = tempBuffer2[i];
                    tempReal2 = outRealMiddleBand[i];
                    outRealUpperBand[i] = tempReal2 + tempReal;
                    outRealLowerBand[i] = tempReal2 - tempReal * optInNbDevDn;
                }
            }
            else if (optInNbDevDn.Equals(1.0))
            {
                for (i = 0; i < outNBElement; i++)
                {
                    tempReal = tempBuffer2[i];
                    tempReal2 = outRealMiddleBand[i];
                    outRealLowerBand[i] = tempReal2 - tempReal;
                    outRealUpperBand[i] = tempReal2 + tempReal * optInNbDevUp;
                }
            }
            else
            {
                for (i = 0; i < outNBElement; i++)
                {
                    tempReal = tempBuffer2[i];
                    tempReal2 = outRealMiddleBand[i];
                    outRealUpperBand[i] = tempReal2 + tempReal * optInNbDevUp;
                    outRealLowerBand[i] = tempReal2 - tempReal * optInNbDevDn;
                }
            }

            return RetCode.Success;
        }

        public static RetCode Bbands(int startIdx, int endIdx, decimal[] inReal, MAType optInMAType, ref int outBegIdx,
            ref int outNBElement, decimal[] outRealUpperBand, decimal[] outRealMiddleBand, decimal[] outRealLowerBand,
            int optInTimePeriod = 5, decimal optInNbDevUp = 2m, decimal optInNbDevDn = 2m)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outRealUpperBand == null || outRealMiddleBand == null || outRealLowerBand == null ||
                optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            decimal[] tempBuffer1;
            decimal[] tempBuffer2;
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

            RetCode retCode = Ma(startIdx, endIdx, inReal, optInMAType, ref outBegIdx, ref outNBElement, tempBuffer1,
                optInTimePeriod);
            if (retCode != RetCode.Success || outNBElement == 0)
            {
                outNBElement = 0;
                return retCode;
            }

            if (optInMAType == MAType.Sma)
            {
                TA_INT_StdDevUsingPrecalcMA(inReal, tempBuffer1, outBegIdx, outNBElement, optInTimePeriod, tempBuffer2);
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

            int i;
            decimal tempReal;
            decimal tempReal2;
            if (optInNbDevUp == optInNbDevDn)
            {
                if (optInNbDevUp == Decimal.One)
                {
                    for (i = 0; i < outNBElement; i++)
                    {
                        tempReal = tempBuffer2[i];
                        tempReal2 = outRealMiddleBand[i];
                        outRealUpperBand[i] = tempReal2 + tempReal;
                        outRealLowerBand[i] = tempReal2 - tempReal;
                    }
                }
                else
                {
                    for (i = 0; i < outNBElement; i++)
                    {
                        tempReal = tempBuffer2[i] * optInNbDevUp;
                        tempReal2 = outRealMiddleBand[i];
                        outRealUpperBand[i] = tempReal2 + tempReal;
                        outRealLowerBand[i] = tempReal2 - tempReal;
                    }
                }
            }
            else if (optInNbDevUp == Decimal.One)
            {
                for (i = 0; i < outNBElement; i++)
                {
                    tempReal = tempBuffer2[i];
                    tempReal2 = outRealMiddleBand[i];
                    outRealUpperBand[i] = tempReal2 + tempReal;
                    outRealLowerBand[i] = tempReal2 - tempReal * optInNbDevDn;
                }
            }
            else if (optInNbDevDn == Decimal.One)
            {
                for (i = 0; i < outNBElement; i++)
                {
                    tempReal = tempBuffer2[i];
                    tempReal2 = outRealMiddleBand[i];
                    outRealLowerBand[i] = tempReal2 - tempReal;
                    outRealUpperBand[i] = tempReal2 + tempReal * optInNbDevUp;
                }
            }
            else
            {
                for (i = 0; i < outNBElement; i++)
                {
                    tempReal = tempBuffer2[i];
                    tempReal2 = outRealMiddleBand[i];
                    outRealUpperBand[i] = tempReal2 + tempReal * optInNbDevUp;
                    outRealLowerBand[i] = tempReal2 - tempReal * optInNbDevDn;
                }
            }

            return RetCode.Success;
        }

        public static int BbandsLookback(MAType optInMAType, int optInTimePeriod = 5)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return MaLookback(optInMAType, optInTimePeriod);
        }
    }
}
