namespace TALib
{
    public static partial class Core
    {
        public static RetCode Ma(double[] inReal, int startIdx, int endIdx, double[] outReal, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 30, MAType optInMAType = MAType.Sma)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod == 1)
            {
                int nbElement = endIdx - startIdx + 1;
                outNbElement = nbElement;
                for (int todayIdx = startIdx, outIdx = 0; outIdx < nbElement; outIdx++, todayIdx++)
                {
                    outReal[outIdx] = inReal[todayIdx];
                }

                outBegIdx = startIdx;

                return RetCode.Success;
            }

            switch (optInMAType)
            {
                case MAType.Sma:
                    return Sma(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
                case MAType.Ema:
                    return Ema(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
                case MAType.Wma:
                    return Wma(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
                case MAType.Dema:
                    return Dema(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
                case MAType.Tema:
                    return Tema(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
                case MAType.Trima:
                    return Trima(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
                case MAType.Kama:
                    return Kama(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
                case MAType.Mama:
                    var dummyBuffer = new double[endIdx - startIdx + 1];
                    return Mama(inReal, startIdx, endIdx, outReal, dummyBuffer, out outBegIdx, out outNbElement);
                case MAType.T3:
                    return T3(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
                default:
                    return RetCode.BadParam;
            }
        }

        public static RetCode Ma(decimal[] inReal, int startIdx, int endIdx, decimal[] outReal, out int outBegIdx, out int outNbElement,
            int optInTimePeriod = 30, MAType optInMAType = MAType.Sma)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod == 1)
            {
                int nbElement = endIdx - startIdx + 1;
                outNbElement = nbElement;
                for (int todayIdx = startIdx, outIdx = 0; outIdx < nbElement; outIdx++, todayIdx++)
                {
                    outReal[outIdx] = inReal[todayIdx];
                }

                outBegIdx = startIdx;

                return RetCode.Success;
            }

            switch (optInMAType)
            {
                case MAType.Sma:
                    return Sma(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
                case MAType.Ema:
                    return Ema(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
                case MAType.Wma:
                    return Wma(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
                case MAType.Dema:
                    return Dema(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
                case MAType.Tema:
                    return Tema(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
                case MAType.Trima:
                    return Trima(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
                case MAType.Kama:
                    return Kama(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
                case MAType.Mama:
                    var dummyBuffer = new decimal[endIdx - startIdx + 1];
                    return Mama(inReal, startIdx, endIdx, outReal, dummyBuffer, out outBegIdx, out outNbElement);
                case MAType.T3:
                    return T3(inReal, startIdx, endIdx, outReal, out outBegIdx, out outNbElement, optInTimePeriod);
                default:
                    return RetCode.BadParam;
            }
        }

        public static int MaLookback(int optInTimePeriod = 30, MAType optInMAType = MAType.Sma)
        {
            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return -1;
            }

            if (optInTimePeriod == 1)
            {
                return 0;
            }

            return optInMAType switch
            {
                MAType.Sma => SmaLookback(optInTimePeriod),
                MAType.Ema => EmaLookback(optInTimePeriod),
                MAType.Wma => WmaLookback(optInTimePeriod),
                MAType.Dema => DemaLookback(optInTimePeriod),
                MAType.Tema => TemaLookback(optInTimePeriod),
                MAType.Trima => TrimaLookback(optInTimePeriod),
                MAType.Kama => KamaLookback(optInTimePeriod),
                MAType.Mama => MamaLookback(),
                MAType.T3 => T3Lookback(optInTimePeriod),
                _ => 0
            };
        }
    }
}
