using System;

namespace Alaric.Utils
{
    /// <summary>
    /// Provides some advanced math functions
    /// </summary>
    public static class AdvMath
    {
        /// <summary>
        /// Returns the specified base logarithm of the specified number.
        /// </summary>
        /// <param name="Base"></param>
        /// <param name="exponent"></param>
        /// <returns></returns>
        public static double Log(double Base,double exponent)
        {
            return (Math.Log10(exponent) / Math.Log10(Base) + Math.Log(exponent) / Math.Log(Base)) / 2;
        }

        public delegate double IntegralCalculateDelegate(double x);

        /// <summary>
        /// Returns the definite integral of the specified function and specified integral interval.
        /// </summary>
        /// <param name="calcFunction"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="accuracy"></param>
        /// <returns></returns>
        public static double Integral(IntegralCalculateDelegate calcFunction,double minValue,double maxValue,double accuracy)
        {
            int times = (int)((maxValue - minValue) / accuracy);
            double result = 0;
            for (int i = 0; i < times; i++)
            {
                result += calcFunction(minValue + accuracy * i) * accuracy;
            }
            for (int i = 0; i < times; i++)
            {
                result += calcFunction(minValue + accuracy * (i + 1)) * accuracy;
            }
            return result/2;
        }
    }
}
