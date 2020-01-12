using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Bbands(int startIdx, int endIdx, double[] inReal, int optInTimePeriod, double optInNbDevUp,
            double optInNbDevDn, MAType optInMAType, ref int outBegIdx, ref int outNBElement, double[] outRealUpperBand,
            double[] outRealMiddleBand, double[] outRealLowerBand)
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

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 5;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInNbDevUp == -4E+37)
            {
                optInNbDevUp = 2.0;
            }
            else if ((optInNbDevUp < -3E+37) || (optInNbDevUp > 3E+37))
            {
                return RetCode.BadParam;
            }

            if (optInNbDevDn == -4E+37)
            {
                optInNbDevDn = 2.0;
            }
            else if ((optInNbDevDn < -3E+37) || (optInNbDevDn > 3E+37))
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

            if ((tempBuffer1 == inReal) || (tempBuffer2 == inReal))
            {
                return RetCode.BadParam;
            }

            RetCode retCode = MovingAverage(startIdx, endIdx, inReal, optInTimePeriod, optInMAType, ref outBegIdx, ref outNBElement,
                tempBuffer1);
            if ((retCode != RetCode.Success) || (outNBElement == 0))
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
                retCode = StdDev(outBegIdx, endIdx, inReal, optInTimePeriod, 1.0, ref outBegIdx, ref outNBElement, tempBuffer2);
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

            if (optInNbDevUp != optInNbDevDn)
            {
                if (optInNbDevUp != 1.0)
                {
                    if (optInNbDevDn != 1.0)
                    {
                        i = 0;
                        while (i < outNBElement)
                        {
                            tempReal = tempBuffer2[i];
                            tempReal2 = outRealMiddleBand[i];
                            outRealUpperBand[i] = tempReal2 + (tempReal * optInNbDevUp);
                            outRealLowerBand[i] = tempReal2 - (tempReal * optInNbDevDn);
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
                if (optInNbDevUp != 1.0)
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
                outRealLowerBand[i] = tempReal2 - (tempReal * optInNbDevDn);
                i++;
            }

            Label_025E:
            while (i < outNBElement)
            {
                tempReal = tempBuffer2[i];
                tempReal2 = outRealMiddleBand[i];
                outRealLowerBand[i] = tempReal2 - tempReal;
                outRealUpperBand[i] = tempReal2 + (tempReal * optInNbDevUp);
                i++;
            }

            Label_02B1:
            return RetCode.Success;
        }

        public static RetCode Bbands(int startIdx, int endIdx, float[] inReal, int optInTimePeriod, double optInNbDevUp,
            double optInNbDevDn, MAType optInMAType, ref int outBegIdx, ref int outNBElement, double[] outRealUpperBand,
            double[] outRealMiddleBand, double[] outRealLowerBand)
        {
            int i;
            double tempReal2;
            double tempReal;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 5;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (optInNbDevUp == -4E+37)
            {
                optInNbDevUp = 2.0;
            }
            else if ((optInNbDevUp < -3E+37) || (optInNbDevUp > 3E+37))
            {
                return RetCode.BadParam;
            }

            if (optInNbDevDn == -4E+37)
            {
                optInNbDevDn = 2.0;
            }
            else if ((optInNbDevDn < -3E+37) || (optInNbDevDn > 3E+37))
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

            double[] tempBuffer1 = outRealMiddleBand;
            double[] tempBuffer2 = outRealLowerBand;
            RetCode retCode = MovingAverage(startIdx, endIdx, inReal, optInTimePeriod, optInMAType, ref outBegIdx, ref outNBElement,
                tempBuffer1);
            if ((retCode != RetCode.Success) || (outNBElement == 0))
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
                retCode = StdDev(outBegIdx, endIdx, inReal, optInTimePeriod, 1.0, ref outBegIdx, ref outNBElement, tempBuffer2);
                if (retCode != RetCode.Success)
                {
                    outNBElement = 0;
                    return retCode;
                }
            }

            if (optInNbDevUp != optInNbDevDn)
            {
                if (optInNbDevUp != 1.0)
                {
                    if (optInNbDevDn != 1.0)
                    {
                        i = 0;
                        while (i < outNBElement)
                        {
                            tempReal = tempBuffer2[i];
                            tempReal2 = outRealMiddleBand[i];
                            outRealUpperBand[i] = tempReal2 + (tempReal * optInNbDevUp);
                            outRealLowerBand[i] = tempReal2 - (tempReal * optInNbDevDn);
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
                if (optInNbDevUp != 1.0)
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
                outRealLowerBand[i] = tempReal2 - (tempReal * optInNbDevDn);
                i++;
            }

            Label_020C:
            while (i < outNBElement)
            {
                tempReal = tempBuffer2[i];
                tempReal2 = outRealMiddleBand[i];
                outRealLowerBand[i] = tempReal2 - tempReal;
                outRealUpperBand[i] = tempReal2 + (tempReal * optInNbDevUp);
                i++;
            }

            Label_025F:
            return RetCode.Success;
        }

        public static int BbandsLookback(int optInTimePeriod, double optInNbDevUp, double optInNbDevDn, MAType optInMAType)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 5;
            }
            else if ((optInTimePeriod < 2) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            if (optInNbDevUp == -4E+37)
            {
                optInNbDevUp = 2.0;
            }
            else if ((optInNbDevUp < -3E+37) || (optInNbDevUp > 3E+37))
            {
                return -1;
            }

            if (optInNbDevDn == -4E+37)
            {
                optInNbDevDn = 2.0;
            }
            else if ((optInNbDevDn < -3E+37) || (optInNbDevDn > 3E+37))
            {
                return -1;
            }

            return MovingAverageLookback(optInTimePeriod, optInMAType);
        }
    }
}
