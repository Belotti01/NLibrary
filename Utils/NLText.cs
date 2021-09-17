using NL.Extensions;
using System;
using System.Drawing;
using System.Text;

namespace NL.Utils {
    public static class NLText {

        /// <summary>
        ///     Create a <see langword="string"/> where the <paramref name="character"/>
        ///     is repeated <paramref name="times"/> times.
        /// </summary>
        /// <param name="character">
        ///     The <see langword="char"/> to repeat.
        /// </param>
        /// <param name="times">
        ///     The amount of times the <paramref name="character"/> is repeated.
        /// </param>
        /// <returns>
        ///     A <see langword="string"/> long <paramref name="times"/> filled with
        ///     the requested <paramref name="character"/>.
        /// </returns>
        public static string Repeated(char character, int times) {
            // Pre-allocate memory
            StringBuilder sb = new(times);
            sb.Append(character, times);
            return sb.ToString();
        }

        /// <summary>
        ///     Create a <see langword="string"/> where the <paramref name="str"/>
        ///     is repeated <paramref name="times"/> times.
        /// </summary>
        /// <param name="str">
        ///     The <see langword="string"/> to repeat.
        /// </param>
        /// <param name="times">
        ///     The amount of times the <paramref name="str"/> is repeated.
        /// </param>
        /// <returns>
        ///     A <see langword="string"/> filled with the requested 
        ///     <paramref name="str"/> repeated <paramref name="times"/> times.
        /// </returns>
        public static string Repeated(string str, int times) {
            // Pre-allocate memory
            StringBuilder sb = new(str.Length * times);
            for(int i = 0; i < times; i++) {
                sb.Append(str);
			}
            return sb.ToString();
        }

        /// <summary>
        ///     Get the rounded width of the <paramref name="str"/> relative to the width
        ///     of a single whitespace.
        /// </summary>
        /// <param name="str">
        ///     The <see langword="string"/> to measure.
        /// </param>
        /// <returns>
        ///     The width of the <paramref name="str"/>, divided by the width of a single
        ///     whitespace.
        /// </returns>
        public static int GetVisualWidth(string str) {
            using Font f = new("Microsoft Sans Serif", 14, FontStyle.Regular);
            using Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            return (g.MeasureString(str, f).Width / g.MeasureString(" ", f).Width).Rounded();
        }

    }
}
