using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode StdDev(int startIdx, int endIdx, double[] inReal, ref int outBegIdx, ref int outNBElement, double[] outReal,
            int optInTimePeriod = 5, double optInNbDev = 1.0)
        {
            int i;
            double tempReal;
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

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            RetCode retCode = TA_INT_VAR(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            if (optInNbDev.Equals(1.0))
            {
                i = 0;
                while (i < outNBElement)
                {
                    tempReal = outReal[i];
                    if (tempReal >= 1E-08)
                    {
                        outReal[i] = Math.Sqrt(tempReal);
                    }
                    else
                    {
                        outReal[i] = 0.0;
                    }

                    i++;
                }
            }
            else
            {
                for (i = 0; i < outNBElement; i++)
                {
                    tempReal = outReal[i];
                    if (tempReal >= 1E-08)
                    {
                        outReal[i] = Math.Sqrt(tempReal) * optInNbDev;
                    }
                    else
                    {
                        outReal[i] = 0.0;
                    }
                }
            }

            return RetCode.Success;
        }

        public static RetCode StdDev(int startIdx, int endIdx, decimal[] inReal, ref int outBegIdx, ref int outNBElement, decimal[] outReal,
            int optInTimePeriod = 5, decimal optInNbDev = 1m)
        {
            int i;
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

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            RetCode retCode = TA_INT_VAR(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            if (optInNbDev == Decimal.One)
            {
                i = 0;
                while (i < outNBElement)
                {
                    tempReal = outReal[i];
                    if (tempReal >= 1E-08m)
                    {
                        outReal[i] = DecimalMath.Sqrt(tempReal);
                    }
                    else
                    {
                        outReal[i] = Decimal.Zero;
                    }

                    i++;
                }
            }
            else
            {
                for (i = 0; i < outNBElement; i++)
                {
                    tempReal = outReal[i];
                    if (tempReal >= 1E-08m)
                    {
                        outReal[i] = DecimalMath.Sqrt(tempReal) * optInNbDev;
                    }
                    else
                    {
                        outReal[i] = Decimal.Zero;
                    }
                }
            }

            return RetCode.Success;
        }

        public static int StdDevLookback(int optInTimePeriod = 5)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return VarianceLookback(optInTimePeriod);
        }
    }
}
