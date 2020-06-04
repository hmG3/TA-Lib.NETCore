namespace TALib
{
    public partial class Core
    {
        public static RetCode Ma(int startIdx, int endIdx, double[] inReal, MAType optInMAType, out int outBegIdx,
            out int outNbElement, double[] outReal, int optInTimePeriod = 30)
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

            if (optInTimePeriod != 1)
            {
                switch (optInMAType)
                {
                    case MAType.Sma:
                        return Sma(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, optInTimePeriod);
                    case MAType.Ema:
                        return Ema(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, optInTimePeriod);
                    case MAType.Wma:
                        return Wma(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, optInTimePeriod);
                    case MAType.Dema:
                        return Dema(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, optInTimePeriod);
                    case MAType.Tema:
                        return Tema(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, optInTimePeriod);
                    case MAType.Trima:
                        return Trima(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, optInTimePeriod);
                    case MAType.Kama:
                        return Kama(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, optInTimePeriod);
                    case MAType.Mama:
                        var dummyBuffer = new double[endIdx - startIdx + 1];
                        return Mama(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, dummyBuffer);
                    case MAType.T3:
                        return T3(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, optInTimePeriod);
                }

                return RetCode.BadParam;
            }

            int nbElement = endIdx - startIdx + 1;
            outNbElement = nbElement;
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

        public static RetCode Ma(int startIdx, int endIdx, decimal[] inReal, MAType optInMAType, out int outBegIdx,
            out int outNbElement, decimal[] outReal, int optInTimePeriod = 30)
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

            if (optInTimePeriod != 1)
            {
                switch (optInMAType)
                {
                    case MAType.Sma:
                        return Sma(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, optInTimePeriod);
                    case MAType.Ema:
                        return Ema(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, optInTimePeriod);
                    case MAType.Wma:
                        return Wma(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, optInTimePeriod);
                    case MAType.Dema:
                        return Dema(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, optInTimePeriod);
                    case MAType.Tema:
                        return Tema(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, optInTimePeriod);
                    case MAType.Trima:
                        return Trima(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, optInTimePeriod);
                    case MAType.Kama:
                        return Kama(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, optInTimePeriod);
                    case MAType.Mama:
                        var dummyBuffer = new decimal[endIdx - startIdx + 1];
                        return Mama(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, dummyBuffer);
                    case MAType.T3:
                        return T3(startIdx, endIdx, inReal, out outBegIdx, out outNbElement, outReal, optInTimePeriod);
                }

                return RetCode.BadParam;
            }

            int nbElement = endIdx - startIdx + 1;
            outNbElement = nbElement;
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

        public static int MaLookback(MAType optInMAType, int optInTimePeriod = 30)
        {
            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return -1;
            }

            if (optInTimePeriod <= 1)
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
