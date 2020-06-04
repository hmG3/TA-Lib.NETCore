using System;

namespace TALib
{
    /// <summary>
    /// Math library for decimal processing
    /// </summary>
    /// <remarks>https://github.com/raminrahimzada/CSharp-Helper-Classes/tree/master/Math/DecimalMath</remarks>
    internal static class DecimalMath
    {
        /// <summary>
        /// represents π
        /// </summary>
        public const decimal PI = 3.141592653589793238462643383279502884197169399375105820975M;

        /// <summary>
        /// represents 2*π
        /// </summary>
        private const decimal PIx2 = 6.28318530717958647692528676655900576839433879875021164195M;

        /// <summary>
        /// represents E
        /// </summary>
        public const decimal E = 2.718281828459045235360287471352662497757247093699959574967M;

        /// <summary>
        /// represents π/2
        /// </summary>
        private const decimal PIdiv2 = 1.570796326794896619231321691639751442098584699687552910487M;

        /// <summary>
        /// represents π/4
        /// </summary>
        private const decimal PIdiv4 = 0.7853981633974483096156608458198757210492923498437764552437M;

        /// <summary>
        /// represents π/12
        /// </summary>
        private const decimal PIdiv12 = 0.2617993877991494365385536152732919070164307832812588184146M;

        /// <summary>
        /// represents 1.0/E
        /// </summary>
        private const decimal Einv = 0.3678794411714423215955237701614608674458111310317678345078M;

        /// <summary>
        /// log(10,E) factor
        /// </summary>
        private const decimal Log10Inv = 0.4342944819032518276511289189166050822943970058036665661145M;

        /// <summary>
        /// Represents 0.5M
        /// </summary>
        private const decimal Half = 0.5M;

        /// <summary>
        /// Max iterations count in Taylor series
        /// </summary>
        private const int MaxIteration = 100;

        /// <summary>
        /// Analogy of Math.Exp method
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Exp(decimal x)
        {
            int count = default;
            while (x > Decimal.One)
            {
                x--;
                count++;
            }

            while (x < Decimal.Zero)
            {
                x++;
                count--;
            }

            var iteration = 1;
            var result = Decimal.One;
            var factorial = Decimal.One;
            decimal cachedResult;
            do
            {
                cachedResult = result;
                factorial *= x / iteration++;
                result += factorial;
            } while (cachedResult != result);

            if (count != 0)
            {
                result *= PowerN(E, count);
            }

            return result;
        }

        /// <summary>
        /// Power to the integer value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static decimal PowerN(decimal value, int power)
        {
            if (power == Decimal.Zero)
            {
                return Decimal.One;
            }

            if (power < Decimal.Zero)
            {
                return PowerN(Decimal.One / value, -power);
            }

            var q = power;
            var prod = Decimal.One;
            var current = value;
            while (q > 0)
            {
                if (q % 2 == 1)
                {
                    // detects the 1s in the binary expression of power
                    prod = current * prod; // picks up the relevant power
                    q--;
                }

                current *= current; // value^i -> value^(2*i)
                q /= 2;
            }

            return prod;
        }

        /// <summary>
        /// Analogy of Math.Log10
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Log10(decimal x)
        {
            return Log(x) * Log10Inv;
        }

        /// <summary>
        /// Analogy of Math.Log
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Log(decimal x)
        {
            if (x <= Decimal.Zero)
            {
                throw new ArgumentException("x must be greater than Decimal.Zero");
            }

            int count = default;
            while (x >= Decimal.One)
            {
                x *= Einv;
                count++;
            }

            while (x <= Einv)
            {
                x *= E;
                count--;
            }

            x--;
            if (x == 0)
            {
                return count;
            }

            var result = Decimal.Zero;
            int iteration = default;
            var y = Decimal.One;
            var cacheResult = result - Decimal.One;
            while (cacheResult != result && iteration < MaxIteration)
            {
                iteration++;
                cacheResult = result;
                y *= -x;
                result += y / iteration;
            }

            return count - result;
        }

        /// <summary>
        /// Analogy of Math.Cos
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Cos(decimal x)
        {
            while (x > PIx2)
            {
                x -= PIx2;
            }

            while (x < -PIx2)
            {
                x += PIx2;
            }

            // now x in [-2π;2π]
            if (x >= PI && x <= PIx2)
            {
                return -Cos(x - PI);
            }

            if (x >= -PIx2 && x <= -PI)
            {
                return -Cos(x + PI);
            }

            x *= x;
            //y=1-x/2!+x^2/4!-x^3/6!...
            var xx = -x * Half;
            var y = Decimal.One + xx;
            var cachedY = y - Decimal.One; //init cache  with different value
            for (var i = 1; cachedY != y && i < MaxIteration; i++)
            {
                cachedY = y;
                decimal factor = i * (i + i + 3) + 1; //2i^2+2i+i+1=2i^2+3i+1
                factor = -Half / factor;
                xx *= x * factor;
                y += xx;
            }

            return y;
        }

        /// <summary>
        /// Analogy of Math.Tan
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Tan(decimal x)
        {
            var cos = Cos(x);
            if (cos == Decimal.Zero)
            {
                throw new ArgumentException(nameof(x));
            }

            return Sin(x) / cos;
        }

        /// <summary>
        /// Analogy of Math.Sin
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Sin(decimal x)
        {
            var cos = Cos(x);
            var moduleOfSin = Sqrt(Decimal.One - cos * cos);
            var sineIsPositive = IsSignOfSinePositive(x);

            return sineIsPositive ? moduleOfSin : -moduleOfSin;
        }

        /// <summary>
        /// Analogy of Math.Sqrt
        /// </summary>
        /// <param name="x"></param>
        /// <param name="epsilon">lasts iteration while error less than this epsilon</param>
        /// <returns></returns>
        public static decimal Sqrt(decimal x, decimal epsilon = Decimal.Zero)
        {
            if (x < Decimal.Zero)
            {
                throw new OverflowException("Cannot calculate square root from a negative number");
            }

            //initial approximation
            decimal current = (decimal) Math.Sqrt((double) x), previous;
            do
            {
                previous = current;
                if (previous == Decimal.Zero)
                {
                    return Decimal.Zero;
                }

                current = (previous + x / previous) * Half;
            } while (Math.Abs(previous - current) > epsilon);

            return current;
        }

        /// <summary>
        /// Analogy of Math.Sinh
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Sinh(decimal x)
        {
            var y = Exp(x);
            var yy = Decimal.One / y;

            return (y - yy) * Half;
        }

        /// <summary>
        /// Analogy of Math.Cosh
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Cosh(decimal x)
        {
            var y = Exp(x);
            var yy = Decimal.One / y;

            return (y + yy) * Half;
        }

        /// <summary>
        /// Analogy of Math.Sign
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Sign(decimal x)
        {
            return x < Decimal.Zero ? -1 : x > Decimal.Zero ? 1 : 0;
        }

        /// <summary>
        /// Analogy of Math.Tanh
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Tanh(decimal x)
        {
            var y = Exp(x);
            var yy = Decimal.One / y;

            return (y - yy) / (y + yy);
        }

        /// <summary>
        /// Analogy of Math.Asin
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Asin(decimal x)
        {
            if (x > Decimal.One || x < -Decimal.One)
            {
                throw new ArgumentException("x must be in [-1,1]");
            }

            //known values
            switch (x)
            {
                case Decimal.Zero:
                    return Decimal.Zero;
                case Decimal.One:
                    return PIdiv2;
            }

            //asin function is odd function
            if (x < Decimal.Zero)
            {
                return -Asin(-x);
            }

            //my optimize trick here

            // used a math formula to speed up :
            // asin(x)=0.5*(π/2-asin(1-2*x*x))
            // if x>=0 is true

            var newX = Decimal.One - 2 * x * x;

            //for calculating new value near to Decimal.Zero than current
            //because we gain more speed with values near to Decimal.Zero
            if (Math.Abs(x) > Math.Abs(newX))
            {
                var t = Asin(newX);
                return Half * (PIdiv2 - t);
            }

            var y = Decimal.Zero;
            var result = x;
            decimal cachedResult;
            var i = 1;
            y += result;
            var xx = x * x;
            do
            {
                cachedResult = result;
                result *= xx * (Decimal.One - Half / i);
                y += result / (2 * i + 1);
                i++;
            } while (cachedResult != result);

            return y;
        }

        /// <summary>
        /// Analogy of Math.Atan
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Atan(decimal x)
        {
            return x switch
            {
                Decimal.Zero => Decimal.Zero,
                Decimal.One => PIdiv4,
                _ => Asin(x / Sqrt(Decimal.One + x * x))
            };
        }

        /// <summary>
        /// Analogy of Math.Acos
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Acos(decimal x)
        {
            return x switch
            {
                Decimal.Zero => PIdiv2,
                Decimal.One => Decimal.Zero,
                _ => (x < Decimal.Zero ? PI - Acos(-x) : PIdiv2 - Asin(x))
            };
        }

        /// <summary>
        /// Analogy of Math.Atan2
        /// for more see this
        /// <seealso cref="http://i.imgur.com/TRLjs8R.png"/>
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Atan2(decimal y, decimal x)
        {
            if (x > Decimal.Zero)
            {
                return Atan(y / x);
            }

            if (x < Decimal.Zero && y >= Decimal.Zero)
            {
                return Atan(y / x) + PI;
            }

            if (x < Decimal.Zero && y < Decimal.Zero)
            {
                return Atan(y / x) - PI;
            }

            return x switch
            {
                Decimal.Zero when y > Decimal.Zero => PIdiv2,
                Decimal.Zero when y < Decimal.Zero => -PIdiv2,
                _ => throw new ArgumentException("invalid atan2 arguments")
            };
        }

        /// <summary>
        /// Converts degrees to radians. (π radians = 180° degrees)
        /// </summary>
        /// <param name="degrees">The degrees to convert.</param>
        public static decimal ToRad(decimal degrees)
        {
            if (degrees % 360m == 0)
            {
                return degrees / 360m * PIx2;
            }

            if (degrees % 270m == 0)
            {
                return degrees / 270m * (PI + PIdiv2);
            }

            if (degrees % 180m == 0)
            {
                return degrees / 180m * PI;
            }

            if (degrees % 90m == 0)
            {
                return degrees / 90m * PIdiv2;
            }

            if (degrees % 45m == 0)
            {
                return degrees / 45m * PIdiv4;
            }

            if (degrees % 15m == 0)
            {
                return degrees / 15m * PIdiv12;
            }

            return degrees * PI / 180m;
        }

        /// <summary>
        /// Converts radians to degrees. (π radians = 180° degrees)
        /// </summary>
        /// <param name="radians">The radians to convert.</param>
        public static decimal ToDeg(decimal radians)
        {
            const decimal ratio = 180m / PI;

            return radians * ratio;
        }

        private static bool IsSignOfSinePositive(decimal x)
        {
            //truncating to [-2π;2π]
            while (x >= PIx2)
            {
                x -= PIx2;
            }

            while (x <= -PIx2)
            {
                x += PIx2;
            }

            //now x in [-2π;2π]
            if (x >= -PIx2 && x <= -PI)
            {
                return true;
            }

            if (x >= -PI && x <= Decimal.Zero)
            {
                return false;
            }

            if (x >= Decimal.Zero && x <= PI)
            {
                return true;
            }

            if (x >= PI && x <= PIx2)
            {
                return false;
            }

            //will not be reached
            throw new ArgumentException(nameof(x));
        }
    }
}
