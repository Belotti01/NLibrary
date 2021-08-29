using System;
using System.Collections.Generic;
using System.Text;

namespace NL.Extensions {
    public static class Characters {

        public static char ToUpper(this char character) {
            return char.IsLetter(character)
                ? char.ToUpper(character)
                : character;
        }

        public static char ToLower(this char character) {
            return char.IsLetter(character)
                ? char.ToLower(character)
                : character;
        }

        /// <summary>
        ///     Convert the numeric value of the <paramref name="character"/> to its
        ///     equivalent <see langword="int"/>.
        /// </summary>
        /// <param name="character">
        ///     The <see langword="char"/> to parse.
        /// </param>
        /// <returns>
        ///     The <see langword="int"/> equivalent of the <paramref name="character"/>,
        ///     or -1 if it's not a valid number.
        /// </returns>
        public static int Parse(this char character) {
            return (int)char.GetNumericValue(character);
        }

        /// <param name="number">
        ///     The <see langword="int"/> equivalent of the <paramref name="character"/>,
        ///     or -1 if it's not a valid number.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="character"/> is a valid
        ///     numeric value, <see langword="false"/> otherwise.
        /// </returns>
        /// <inheritdoc cref="Parse(char)"/>
        public static bool TryParse(this char character, out int number) {
            number = (int)char.GetNumericValue(character);
            return number != -1;
        }

    }
}
