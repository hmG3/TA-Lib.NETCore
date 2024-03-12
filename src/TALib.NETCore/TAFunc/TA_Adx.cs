namespace TALib
{
    public static partial class Core
    {
        public static RetCode Adx(double[] inHigh, double[] inLow, double[] inClose, int startIdx, int endIdx, double[] outReal,
            out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = AdxLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            double tempReal;
            double diffM;
            double diffP;
            double plusDI;
            double minusDI;
            int today = startIdx;
            outBegIdx = today;
            double prevMinusDM = default;
            double prevPlusDM = default;
            double prevTR = default;
            today = startIdx - lookbackTotal;
            double prevHigh = inHigh[today];
            double prevLow = inLow[today];
            double prevClose = inClose[today];
            int i = optInTimePeriod - 1;
            while (i-- > 0)
            {
                today++;
                tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;

                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;

                if (diffM > 0.0 && diffP < diffM)
                {
                    prevMinusDM += diffM;
                }
                else if (diffP > 0.0 && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }

                TrueRange(prevHigh, prevLow, prevClose, out tempReal);
                prevTR += tempReal;
                prevClose = inClose[today];
            }

            double sumDX = default;
            i = optInTimePeriod;
            while (i-- > 0)
            {
                today++;
                tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;

                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;

                prevMinusDM -= prevMinusDM / optInTimePeriod;
                prevPlusDM -= prevPlusDM / optInTimePeriod;
                if (diffM > 0.0 && diffP < diffM)
                {
                    prevMinusDM += diffM;
                }
                else if (diffP > 0.0 && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }

                TrueRange(prevHigh, prevLow, prevClose, out tempReal);
                prevTR = prevTR - prevTR / optInTimePeriod + tempReal;
                prevClose = inClose[today];
                if (!TA_IsZero(prevTR))
                {
                    minusDI = 100.0 * (prevMinusDM / prevTR);
                    plusDI = 100.0 * (prevPlusDM / prevTR);
                    tempReal = minusDI + plusDI;
                    if (!TA_IsZero(tempReal))
                    {
                        sumDX += 100.0 * (Math.Abs(minusDI - plusDI) / tempReal);
                    }
                }
            }

            double prevADX = sumDX / optInTimePeriod;

            i = (int) Globals.UnstablePeriod[(int) FuncUnstId.Adx];
            while (i-- > 0)
            {
                today++;
                tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;

                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;

                prevMinusDM -= prevMinusDM / optInTimePeriod;
                prevPlusDM -= prevPlusDM / optInTimePeriod;

                if (diffM > 0.0 && diffP < diffM)
                {
                    prevMinusDM += diffM;
                }
                else if (diffP > 0.0 && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }

                TrueRange(prevHigh, prevLow, prevClose, out tempReal);
                prevTR = prevTR - prevTR / optInTimePeriod + tempReal;
                prevClose = inClose[today];
                if (!TA_IsZero(prevTR))
                {
                    minusDI = 100.0 * (prevMinusDM / prevTR);
                    plusDI = 100.0 * (prevPlusDM / prevTR);
                    tempReal = minusDI + plusDI;
                    if (!TA_IsZero(tempReal))
                    {
                        tempReal = 100.0 * (Math.Abs(minusDI - plusDI) / tempReal);
                        prevADX = (prevADX * (optInTimePeriod - 1) + tempReal) / optInTimePeriod;
                    }
                }
            }

            outReal[0] = prevADX;
            var outIdx = 1;

            while (today < endIdx)
            {
                today++;
                tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;

                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;

                prevMinusDM -= prevMinusDM / optInTimePeriod;
                prevPlusDM -= prevPlusDM / optInTimePeriod;

                if (diffM > 0.0 && diffP < diffM)
                {
                    prevMinusDM += diffM;
                }
                else if (diffP > 0.0 && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }

                TrueRange(prevHigh, prevLow, prevClose, out tempReal);
                prevTR = prevTR - prevTR / optInTimePeriod + tempReal;
                prevClose = inClose[today];
                if (!TA_IsZero(prevTR))
                {
                    minusDI = 100.0 * (prevMinusDM / prevTR);
                    plusDI = 100.0 * (prevPlusDM / prevTR);
                    tempReal = minusDI + plusDI;
                    if (!TA_IsZero(tempReal))
                    {
                        tempReal = 100.0 * (Math.Abs(minusDI - plusDI) / tempReal);
                        prevADX = (prevADX * (optInTimePeriod - 1) + tempReal) / optInTimePeriod;
                    }
                }

                outReal[outIdx++] = prevADX;
            }

            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode Adx(decimal[] inHigh, decimal[] inLow, decimal[] inClose, int startIdx, int endIdx, decimal[] outReal,
            out int outBegIdx, out int outNbElement, int optInTimePeriod = 14)
        {
            outBegIdx = outNbElement = 0;

            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = AdxLookback(optInTimePeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                return RetCode.Success;
            }

            decimal tempReal;
            decimal diffM;
            decimal diffP;
            decimal plusDI;
            decimal minusDI;
            int today = startIdx;
            outBegIdx = today;
            decimal prevMinusDM = default;
            decimal prevPlusDM = default;
            decimal prevTR = default;
            today = startIdx - lookbackTotal;
            decimal prevHigh = inHigh[today];
            decimal prevLow = inLow[today];
            decimal prevClose = inClose[today];
            int i = optInTimePeriod - 1;
            while (i-- > 0)
            {
                today++;
                tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;

                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;

                if (diffM > Decimal.Zero && diffP < diffM)
                {
                    prevMinusDM += diffM;
                }
                else if (diffP > Decimal.Zero && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }

                TrueRange(prevHigh, prevLow, prevClose, out tempReal);
                prevTR += tempReal;
                prevClose = inClose[today];
            }

            decimal sumDX = default;
            i = optInTimePeriod;
            while (i-- > 0)
            {
                today++;
                tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;

                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;

                prevMinusDM -= prevMinusDM / optInTimePeriod;
                prevPlusDM -= prevPlusDM / optInTimePeriod;
                if (diffM > Decimal.Zero && diffP < diffM)
                {
                    prevMinusDM += diffM;
                }
                else if (diffP > Decimal.Zero && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }

                TrueRange(prevHigh, prevLow, prevClose, out tempReal);
                prevTR = prevTR - prevTR / optInTimePeriod + tempReal;
                prevClose = inClose[today];
                if (!TA_IsZero(prevTR))
                {
                    minusDI = 100m * (prevMinusDM / prevTR);
                    plusDI = 100m * (prevPlusDM / prevTR);
                    tempReal = minusDI + plusDI;
                    if (!TA_IsZero(tempReal))
                    {
                        sumDX += 100m * (Math.Abs(minusDI - plusDI) / tempReal);
                    }
                }
            }

            decimal prevADX = sumDX / optInTimePeriod;

            i = (int) Globals.UnstablePeriod[(int) FuncUnstId.Adx];
            while (i-- > 0)
            {
                today++;
                tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;

                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;

                prevMinusDM -= prevMinusDM / optInTimePeriod;
                prevPlusDM -= prevPlusDM / optInTimePeriod;

                if (diffM > Decimal.Zero && diffP < diffM)
                {
                    prevMinusDM += diffM;
                }
                else if (diffP > Decimal.Zero && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }

                TrueRange(prevHigh, prevLow, prevClose, out tempReal);
                prevTR = prevTR - prevTR / optInTimePeriod + tempReal;
                prevClose = inClose[today];
                if (!TA_IsZero(prevTR))
                {
                    minusDI = 100m * (prevMinusDM / prevTR);
                    plusDI = 100m * (prevPlusDM / prevTR);
                    tempReal = minusDI + plusDI;
                    if (!TA_IsZero(tempReal))
                    {
                        tempReal = 100m * (Math.Abs(minusDI - plusDI) / tempReal);
                        prevADX = (prevADX * (optInTimePeriod - 1) + tempReal) / optInTimePeriod;
                    }
                }
            }

            outReal[0] = prevADX;
            var outIdx = 1;

            while (today < endIdx)
            {
                today++;
                tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;

                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;

                prevMinusDM -= prevMinusDM / optInTimePeriod;
                prevPlusDM -= prevPlusDM / optInTimePeriod;

                if (diffM > Decimal.Zero && diffP < diffM)
                {
                    prevMinusDM += diffM;
                }
                else if (diffP > Decimal.Zero && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }

                TrueRange(prevHigh, prevLow, prevClose, out tempReal);
                prevTR = prevTR - prevTR / optInTimePeriod + tempReal;
                prevClose = inClose[today];
                if (!TA_IsZero(prevTR))
                {
                    minusDI = 100m * (prevMinusDM / prevTR);
                    plusDI = 100m * (prevPlusDM / prevTR);
                    tempReal = minusDI + plusDI;
                    if (!TA_IsZero(tempReal))
                    {
                        tempReal = 100m * (Math.Abs(minusDI - plusDI) / tempReal);
                        prevADX = (prevADX * (optInTimePeriod - 1) + tempReal) / optInTimePeriod;
                    }
                }

                outReal[outIdx++] = prevADX;
            }

            outNbElement = outIdx;

            return RetCode.Success;
        }

        public static int AdxLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod * 2 + (int) Globals.UnstablePeriod[(int) FuncUnstId.Adx] - 1;
        }
    }
}
