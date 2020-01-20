namespace TALib
{
    public partial class Core
    {
        public static RetCode MovingAverage(int startIdx, int endIdx, double[] inReal, MAType optInMAType, ref int outBegIdx,
            ref int outNBElement, double[] outReal, int optInTimePeriod = 30)
        {
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

            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
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
                        return Sma(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, optInTimePeriod);
                    case MAType.Ema:
                        return Ema(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, optInTimePeriod);
                    case MAType.Wma:
                        return Wma(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, optInTimePeriod);
                    case MAType.Dema:
                        return Dema(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, optInTimePeriod);
                    case MAType.Tema:
                        return Tema(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, optInTimePeriod);
                    case MAType.Trima:
                        return Trima(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, optInTimePeriod);
                    case MAType.Kama:
                        return Kama(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, optInTimePeriod);
                    case MAType.Mama:
                        var dummyBuffer = new double[endIdx - startIdx + 1];
                        return Mama(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, dummyBuffer);
                    case MAType.T3:
                        return T3(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, optInTimePeriod);
                }

                return RetCode.BadParam;
            }

            int nbElement = endIdx - startIdx + 1;
            outNBElement = nbElement;
            int todayIdx = startIdx;
            int outIdx = default;
            while (outIdx < nbElement)
            {
                outReal[outIdx] = inReal[todayIdx];
                outIdx++;
                todayIdx++;
            }

            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode MovingAverage(int startIdx, int endIdx, decimal[] inReal, MAType optInMAType, ref int outBegIdx,
            ref int outNBElement, decimal[] outReal, int optInTimePeriod = 30)
        {
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

            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
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
                        return Sma(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, optInTimePeriod);
                    case MAType.Ema:
                        return Ema(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, optInTimePeriod);
                    case MAType.Wma:
                        return Wma(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, optInTimePeriod);
                    case MAType.Dema:
                        return Dema(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, optInTimePeriod);
                    case MAType.Tema:
                        return Tema(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, optInTimePeriod);
                    case MAType.Trima:
                        return Trima(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, optInTimePeriod);
                    case MAType.Kama:
                        return Kama(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, optInTimePeriod);
                    case MAType.Mama:
                        var dummyBuffer = new decimal[endIdx - startIdx + 1];
                        return Mama(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, dummyBuffer);
                    case MAType.T3:
                        return T3(startIdx, endIdx, inReal, ref outBegIdx, ref outNBElement, outReal, optInTimePeriod);
                }

                return RetCode.BadParam;
            }

            int nbElement = endIdx - startIdx + 1;
            outNBElement = nbElement;
            int todayIdx = startIdx;
            int outIdx = default;
            while (outIdx < nbElement)
            {
                outReal[outIdx] = inReal[todayIdx];
                outIdx++;
                todayIdx++;
            }

            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int MovingAverageLookback(MAType optInMAType, int optInTimePeriod = 30)
        {
            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
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
                        return MamaLookback();

                    case MAType.T3:
                        return T3Lookback(optInTimePeriod);
                }
            }

            return 0;
        }
    }
}
