using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode Beta(int startIdx, int endIdx, double[] inReal0, double[] inReal1, int optInTimePeriod, ref int outBegIdx,
            ref int outNBElement, double[] outReal)
        {
            double x;
            double y;
            double S_xx = 0.0;
            double S_xy = 0.0;
            double S_x = 0.0;
            double S_y = 0.0;
            double last_price_x = 0.0;
            double last_price_y = 0.0;
            double trailing_last_price_x = 0.0;
            double trailing_last_price_y = 0.0;
            double tmp_real = 0.0;
            double n = 0.0;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal0 == null)
            {
                return RetCode.BadParam;
            }

            if (inReal1 == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 5;
            }
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int nbInitialElementNeeded = optInTimePeriod;
            if (startIdx < nbInitialElementNeeded)
            {
                startIdx = nbInitialElementNeeded;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int trailingIdx = startIdx - nbInitialElementNeeded;
            trailing_last_price_x = inReal0[trailingIdx];
            last_price_x = trailing_last_price_x;
            trailing_last_price_y = inReal1[trailingIdx];
            last_price_y = trailing_last_price_y;
            trailingIdx++;
            int i = trailingIdx;
            while (true)
            {
                if (i >= startIdx)
                {
                    break;
                }

                tmp_real = inReal0[i];
                if ((-1E-08 >= last_price_x) || (last_price_x >= 1E-08))
                {
                    x = (tmp_real - last_price_x) / last_price_x;
                }
                else
                {
                    x = 0.0;
                }

                last_price_x = tmp_real;
                tmp_real = inReal1[i];
                i++;
                if ((-1E-08 >= last_price_y) || (last_price_y >= 1E-08))
                {
                    y = (tmp_real - last_price_y) / last_price_y;
                }
                else
                {
                    y = 0.0;
                }

                last_price_y = tmp_real;
                S_xx += x * x;
                S_xy += x * y;
                S_x += x;
                S_y += y;
            }

            int outIdx = 0;
            n = optInTimePeriod;
            do
            {
                tmp_real = inReal0[i];
                if ((-1E-08 >= last_price_x) || (last_price_x >= 1E-08))
                {
                    x = (tmp_real - last_price_x) / last_price_x;
                }
                else
                {
                    x = 0.0;
                }

                last_price_x = tmp_real;
                tmp_real = inReal1[i];
                i++;
                if ((-1E-08 >= last_price_y) || (last_price_y >= 1E-08))
                {
                    y = (tmp_real - last_price_y) / last_price_y;
                }
                else
                {
                    y = 0.0;
                }

                last_price_y = tmp_real;
                S_xx += x * x;
                S_xy += x * y;
                S_x += x;
                S_y += y;
                tmp_real = inReal0[trailingIdx];
                if ((-1E-08 >= trailing_last_price_x) || (trailing_last_price_x >= 1E-08))
                {
                    x = (tmp_real - trailing_last_price_x) / trailing_last_price_x;
                }
                else
                {
                    x = 0.0;
                }

                trailing_last_price_x = tmp_real;
                tmp_real = inReal1[trailingIdx];
                trailingIdx++;
                if ((-1E-08 >= trailing_last_price_y) || (trailing_last_price_y >= 1E-08))
                {
                    y = (tmp_real - trailing_last_price_y) / trailing_last_price_y;
                }
                else
                {
                    y = 0.0;
                }

                trailing_last_price_y = tmp_real;
                tmp_real = (n * S_xx) - (S_x * S_x);
                if ((-1E-08 >= tmp_real) || (tmp_real >= 1E-08))
                {
                    outReal[outIdx] = ((n * S_xy) - (S_x * S_y)) / tmp_real;
                    outIdx++;
                }
                else
                {
                    outReal[outIdx] = 0.0;
                    outIdx++;
                }

                S_xx -= x * x;
                S_xy -= x * y;
                S_x -= x;
                S_y -= y;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static RetCode Beta(int startIdx, int endIdx, float[] inReal0, float[] inReal1, int optInTimePeriod, ref int outBegIdx,
            ref int outNBElement, double[] outReal)
        {
            double x;
            double y;
            double S_xx = 0.0;
            double S_xy = 0.0;
            double S_x = 0.0;
            double S_y = 0.0;
            double last_price_x = 0.0;
            double last_price_y = 0.0;
            double trailing_last_price_x = 0.0;
            double trailing_last_price_y = 0.0;
            double tmp_real = 0.0;
            double n = 0.0;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if ((endIdx < 0) || (endIdx < startIdx))
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal0 == null)
            {
                return RetCode.BadParam;
            }

            if (inReal1 == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 5;
            }
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            int nbInitialElementNeeded = optInTimePeriod;
            if (startIdx < nbInitialElementNeeded)
            {
                startIdx = nbInitialElementNeeded;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int trailingIdx = startIdx - nbInitialElementNeeded;
            trailing_last_price_x = inReal0[trailingIdx];
            last_price_x = trailing_last_price_x;
            trailing_last_price_y = inReal1[trailingIdx];
            last_price_y = trailing_last_price_y;
            trailingIdx++;
            int i = trailingIdx;
            while (true)
            {
                if (i >= startIdx)
                {
                    break;
                }

                tmp_real = inReal0[i];
                if ((-1E-08 >= last_price_x) || (last_price_x >= 1E-08))
                {
                    x = (tmp_real - last_price_x) / last_price_x;
                }
                else
                {
                    x = 0.0;
                }

                last_price_x = tmp_real;
                tmp_real = inReal1[i];
                i++;
                if ((-1E-08 >= last_price_y) || (last_price_y >= 1E-08))
                {
                    y = (tmp_real - last_price_y) / last_price_y;
                }
                else
                {
                    y = 0.0;
                }

                last_price_y = tmp_real;
                S_xx += x * x;
                S_xy += x * y;
                S_x += x;
                S_y += y;
            }

            int outIdx = 0;
            n = optInTimePeriod;
            do
            {
                tmp_real = inReal0[i];
                if ((-1E-08 >= last_price_x) || (last_price_x >= 1E-08))
                {
                    x = (tmp_real - last_price_x) / last_price_x;
                }
                else
                {
                    x = 0.0;
                }

                last_price_x = tmp_real;
                tmp_real = inReal1[i];
                i++;
                if ((-1E-08 >= last_price_y) || (last_price_y >= 1E-08))
                {
                    y = (tmp_real - last_price_y) / last_price_y;
                }
                else
                {
                    y = 0.0;
                }

                last_price_y = tmp_real;
                S_xx += x * x;
                S_xy += x * y;
                S_x += x;
                S_y += y;
                tmp_real = inReal0[trailingIdx];
                if ((-1E-08 >= trailing_last_price_x) || (trailing_last_price_x >= 1E-08))
                {
                    x = (tmp_real - trailing_last_price_x) / trailing_last_price_x;
                }
                else
                {
                    x = 0.0;
                }

                trailing_last_price_x = tmp_real;
                tmp_real = inReal1[trailingIdx];
                trailingIdx++;
                if ((-1E-08 >= trailing_last_price_y) || (trailing_last_price_y >= 1E-08))
                {
                    y = (tmp_real - trailing_last_price_y) / trailing_last_price_y;
                }
                else
                {
                    y = 0.0;
                }

                trailing_last_price_y = tmp_real;
                tmp_real = (n * S_xx) - (S_x * S_x);
                if ((-1E-08 >= tmp_real) || (tmp_real >= 1E-08))
                {
                    outReal[outIdx] = ((n * S_xy) - (S_x * S_y)) / tmp_real;
                    outIdx++;
                }
                else
                {
                    outReal[outIdx] = 0.0;
                    outIdx++;
                }

                S_xx -= x * x;
                S_xy -= x * y;
                S_x -= x;
                S_y -= y;
            } while (i <= endIdx);

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int BetaLookback(int optInTimePeriod)
        {
            if (optInTimePeriod == -2147483648)
            {
                optInTimePeriod = 5;
            }
            else if ((optInTimePeriod < 1) || (optInTimePeriod > 0x186a0))
            {
                return -1;
            }

            return optInTimePeriod;
        }
    }
}
