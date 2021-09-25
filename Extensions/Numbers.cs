using System;
using System.Text;

namespace NL.Extensions {

    public static class Numbers {

        #region ROUNDING
        /// <summary>
        ///     Round the value to the closest integer.
        /// </summary>
        public static int Rounded(this double value)
            => (int)Math.Round(value);

        /// <inheritdoc cref="Rounded(double)"/>
        public static int Rounded(this float value)
            => (int)Math.Round(value);

        /// <summary>
        ///     Round the value to the closest smaller or equal integer.
        /// </summary>
        public static int Floored(this double value)
            => (int)Math.Floor(value);

        /// <inheritdoc cref="Floored(double)"/>
        public static int Floored(this float value)
            => (int)Math.Floor(value);

        /// <summary>
        ///     Round the value to the closest greater or equal integer.
        /// </summary>
        public static int Ceiling(this double value)
            => (int)Math.Ceiling(value);

        /// <inheritdoc cref="Ceiling(double)"/>
        public static int Ceiling(this float value)
            => (int)Math.Ceiling(value);
        #endregion

        /// <summary>
        ///     Clamp the number in between a <paramref name="min"/> and a
        ///     <paramref name="max"/> value.
        /// </summary>
        /// <param name="value">
        ///     The base value.
        /// </param>
        /// <param name="min">
        ///     The minimum value returned.
        /// </param>
        /// <param name="max">
        ///     The maximum value returned.
        /// </param>
        /// <returns>
        ///     <paramref name="min"/> if the <paramref name="value"/> is
        ///     smaller than <paramref name="min"/>; <paramref name="max"/>
        ///     if the <paramref name="value"/> is bigger than <paramref name="max"/>;
        ///     <paramref name="value"/> otherwise.
        /// </returns>
        public static int Clamp(this int value, int min, int max) {
            return value < min
                ? min
                : value > max
                    ? max
                    : value;
        }

        /// <inheritdoc cref="Clamp(int, int, int)"/>
        public static float Clamp(this float value, float min, float max) {
            return value < min
                ? min
                : value > max
                    ? max
                    : value;
        }

        /// <inheritdoc cref="Clamp(int, int, int)"/>
        public static double Clamp(this double value, double min, double max) {
            return value < min
                ? min
                : value > max
                    ? max
                    : value;
        }

        /// <summary>
        ///     Get the result of the call to <c>value.ToString()</c> with <paramref name="decimals"/>
        ///     amount of decimal places.
        /// </summary>
        /// <param name="decimals">
        ///     The amount of decimal places to include in the returned <see cref="string"/>.
        /// </param>
        /// <returns>
        ///     A <see cref="string"/> format of the value with <paramref name="decimals"/> decimal places.
        /// </returns>
        public static string ToString(this double value, int decimals) {
            string[] parts = value.ToString().Split('.');
            StringBuilder ret = new(parts[0]);
            if (decimals == 0) {
                return ret.ToString();
            }

            return decimals < parts[1].Length
                ? ret.Append('.')
                    .Append(parts[1][..decimals])
                    .ToString()
                : ret.Append('.')
                    .Append(parts[1])
                    .Append('0', decimals - parts[1].Length)
                    .ToString();
        }

        /// <inheritdoc cref="ToString(double, int)"/>
        public static string ToString(this float value, int decimals)
            => ((double)value).ToString(decimals);

        /// <inheritdoc cref="ToString(double, int)"/>
        public static string ToString(this decimal value, int decimals)
            => ((double)value).ToString(decimals);
    }

}
