namespace TALib
{
    public partial class Core
    {
        public static RetCode Obv(int startIdx, int endIdx, double[] inReal, double[] inVolume, ref int outBegIdx, ref int outNBElement,
            double[] outReal)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || inVolume == null || outReal == null)
            {
                return RetCode.BadParam;
            }

            double prevOBV = inVolume[startIdx];
            double prevReal = inReal[startIdx];
            int outIdx = default;

            for (int i = startIdx; i <= endIdx; i++)
            {
                double tempReal = inReal[i];
                if (tempReal > prevReal)
                {
                    prevOBV += inVolume[i];
                }
                else if (tempReal < prevReal)
                {
                    prevOBV -= inVolume[i];
                }

                outReal[outIdx++] = prevOBV;
                prevReal = tempReal;
            }

            outBegIdx = startIdx;
            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode Obv(int startIdx, int endIdx, decimal[] inReal, decimal[] inVolume, ref int outBegIdx, ref int outNBElement,
            decimal[] outReal)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || inVolume == null || outReal == null)
            {
                return RetCode.BadParam;
            }

            decimal prevOBV = inVolume[startIdx];
            decimal prevReal = inReal[startIdx];
            int outIdx = default;

            for (int i = startIdx; i <= endIdx; i++)
            {
                decimal tempReal = inReal[i];
                if (tempReal > prevReal)
                {
                    prevOBV += inVolume[i];
                }
                else if (tempReal < prevReal)
                {
                    prevOBV -= inVolume[i];
                }

                outReal[outIdx++] = prevOBV;
                prevReal = tempReal;
            }

            outBegIdx = startIdx;
            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static int ObvLookback()
        {
            return 0;
        }
    }
}
