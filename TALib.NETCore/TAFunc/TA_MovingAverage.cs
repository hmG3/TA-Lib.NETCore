using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode MovingAverage(int startIdx, int endIdx, double[] inReal, int optInTimePeriod, MAType optInMAType,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
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
                optInTimePeriod = 30;
            }
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod != 1)
            {
                switch (optInMAType)
                {
                    case MAType.Sma:
                        return Sma(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);

                    case MAType.Ema:
                        return Ema(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);

                    case MAType.Wma:
                        return Wma(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);

                    case MAType.Dema:
                        return Dema(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);

                    case MAType.Tema:
                        return Tema(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);

                    case MAType.Trima:
                        return Trima(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);

                    case MAType.Kama:
                        return Kama(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);

                    case MAType.Mama:
                    {
                        double[] dummyBuffer = new double[(endIdx - startIdx) + 1];
                        if (dummyBuffer != null)
                        {
                            return Mama(startIdx, endIdx, inReal, 0.5, 0.05, ref outBegIdx, ref outNBElement, outReal, dummyBuffer);
                        }

                        return RetCode.AllocErr;
                    }
                    case MAType.T3:
                        return T3(startIdx, endIdx, inReal, optInTimePeriod, 0.7, ref outBegIdx, ref outNBElement, outReal);
                }

                return RetCode.BadParam;
            }

            int nbElement = (endIdx - startIdx) + 1;
            outNBElement = nbElement;
            int todayIdx = startIdx;
            int outIdx = 0;
            while (outIdx < nbElement)
            {
                outReal[outIdx] = inReal[todayIdx];
                outIdx++;
                todayIdx++;
            }

            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode MovingAverage(int startIdx, int endIdx, float[] inReal, int optInTimePeriod, MAType optInMAType,
            ref int outBegIdx, ref int outNBElement, double[] outReal)
        {
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
                optInTimePeriod = 30;
            }
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod != 1)
            {
                switch (optInMAType)
                {
                    case MAType.Sma:
                        return Sma(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);

                    case MAType.Ema:
                        return Ema(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);

                    case MAType.Wma:
                        return Wma(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);

                    case MAType.Dema:
                        return Dema(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);

                    case MAType.Tema:
                        return Tema(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);

                    case MAType.Trima:
                        return Trima(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);

                    case MAType.Kama:
                        return Kama(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);

                    case MAType.Mama:
                    {
                        double[] dummyBuffer = new double[(endIdx - startIdx) + 1];
                        if (dummyBuffer != null)
                        {
                            return Mama(startIdx, endIdx, inReal, 0.5, 0.05, ref outBegIdx, ref outNBElement, outReal, dummyBuffer);
                        }

                        return RetCode.AllocErr;
                    }
                    case MAType.T3:
                        return T3(startIdx, endIdx, inReal, optInTimePeriod, 0.7, ref outBegIdx, ref outNBElement, outReal);
                }

                return RetCode.BadParam;
            }

            int nbElement = (endIdx - startIdx) + 1;
            outNBElement = nbElement;
            int todayIdx = startIdx;
            int outIdx = 0;
            while (outIdx < nbElement)
            {
                outReal[outIdx] = inReal[todayIdx];
                outIdx++;
                todayIdx++;
            }

            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int MovingAverageLookback(int optInTimePeriod, MAType optInMAType)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 30;
            }
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            if (optInTimePeriod > 1)
            {
                switch (optInMAType)
                {
                    case MAType.Sma:
                        return SmaLookback(optInTimePeriod);

                    case MAType.Ema:
                        return EmaLookback(optInTimePeriod);

                    case MAType.Wma:
                        return WmaLookback(optInTimePeriod);

                    case MAType.Dema:
                        return DemaLookback(optInTimePeriod);

                    case MAType.Tema:
                        return TemaLookback(optInTimePeriod);

                    case MAType.Trima:
                        return TrimaLookback(optInTimePeriod);

                    case MAType.Kama:
                        return KamaLookback(optInTimePeriod);

                    case MAType.Mama:
                        return MamaLookback(0.5, 0.05);

                    case MAType.T3:
                        return T3Lookback(optInTimePeriod, 0.7);
                }
            }

            return 0;
        }
    }
}
