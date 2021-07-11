using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NL.Extensions {

    public static class Strings {

        /// <summary>
        ///     Shorten the <see cref="string"/> to have at most <paramref name="maxLength"/>
        ///     characters.
        /// </summary>
        /// <param name="maxLength">
        ///     The maxmimum length of the returned <see cref="string"/>.</param>
        /// <returns>
        ///     The original <see cref="string"/> if its length is less than <paramref name="maxLength"/>,
        ///     otherwise a new <see cref="string"/> containing the first <paramref name="maxLength"/>
        ///     characters of the original.
        /// </returns>
        public static string Truncate(this string str, uint maxLength) {
            if (str.Length <= maxLength)
                return str;
            else
                return str[..(int)maxLength];
        }

        /// <summary>
        ///     Shorten the <see cref="string"/> to have at most <paramref name="maxLength"/>
        ///     characters, and replace the last 3 characters with "..." if any character
        ///     is removed.
        /// </summary>
        /// <returns>
        /// The original <see cref="string"/> if its length is less than <paramref name="maxLength"/>,
        ///     otherwise a new <see cref="string"/> containing the first <paramref name="maxLength"/>
        ///     characters of the original, minus the last 3 characters which are replaced with
        ///     "...".
        /// </returns>
        /// <inheritdoc cref="Truncate(string, uint)"/>
        public static string TruncateWithSuspension(this string str, uint maxLength) {
            if (maxLength < 3)
                return "...";
            else if (str.Length < maxLength)
                return str;
            else
                return $"{str[..(int)(maxLength - 3)]}...";
        }

        /// <summary>
        ///     Split the <see cref="string"/> for all instances of any WhiteSpace
        ///     character or character group, excluding empty <see cref="string"/>s from the
        ///     resulting array.
        /// </summary>
        /// <returns>
        ///     A <see cref="string"/> array containing the original <see cref="string"/> split
        ///     and with all WhiteSpace characters removed.
        /// </returns>
        public static string[] SplitBySpaces(this string str) {
            return Regex.Split(str, @"\s+")
                .Where(s => !s.IsEmpty())
                .ToArray();
        }

        /// <summary>
        ///     Compare two <see cref="string"/>s ignoring case differences.
        /// </summary>
        /// <param name="other">
        ///     The <see cref="string"/> to compare the preceeding <see cref="string"/> to.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the two <see cref="string"/>s are the same when ignoring
        ///     case differences; <see langword="false"/> otherwise.
        /// </returns>
        public static bool EqualsIgnoreCase(this string str, string other)
            => str.Equals(other, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        ///     Extendable version of <see cref="string.IsNullOrWhiteSpace(string)"/>.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="string"/> is <see cref="null"/>,
        ///     equals <see cref="string.Empty"/> or contains whitespace characters only.
        /// </returns>
        public static bool IsEmpty(this string str)
            => string.IsNullOrWhiteSpace(str);

        /// <summary>
        ///     Checks for an instance of the whole <paramref name="word"/> in the <see cref="string"/>.
        /// </summary>
        /// <param name="word">
        ///     The word to look for in the <see cref="string"/>.
        /// </param>
        /// <param name="stringComparison">
        ///     The <see cref="StringComparison"/> to use to compare the strings.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the <paramref name="word"/> equals any word contained
        ///     in the <see cref="string"/>. If the word contains any whitespace character, this
        ///     will return <see langword="true"/> if the <paramref name="word"/> is contained in
        ///     the <see cref="string"/> as a whole.
        /// </returns>
        public static bool ContainsWord(this string str, string word, StringComparison stringComparison = StringComparison.CurrentCulture) {
            word = word.Trim();
            if (word.Contains(' '))
                return str.Contains(word, stringComparison);

            string[] args = str.SplitBySpaces();
            foreach (string arg in args) {
                if (arg.Equals(word, stringComparison)) {
                    return true;
                }
            }
            return false;
        }

        /// <param name="ignoreCase">
        ///     Whether to ignore case differences for characters or not.
        /// </param>
        /// <inheritdoc cref="ContainsWord(string, string, StringComparison)"/>
        public static bool ContainsWord(this string str, string word, bool ignoreCase = false)
            => str.ContainsWord(word, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);

        /// <summary>
        ///     Join all <see cref="string"/>s in a single <see cref="string"/>,
        ///     separated by the <paramref name="separator"/>.
        /// </summary>
        /// <param name="strings">
        ///     The <see cref="string"/>s to join.
        /// </param>
        /// <param name="separator">
        ///     The divisor to put between every two <see cref="string"/>s.
        /// </param>
        /// <returns>
        ///     A single <see cref="string"/> formed by all the <paramref name="strings"/>
        ///     separated by the <paramref name="separator"/>.
        /// </returns>
        public static string Join(this IEnumerable<string> strings, char separator)
            => string.Join(separator, strings);

        /// <inheritdoc cref="Join(IEnumerable{string}, char)"/>
        public static string Join(this IEnumerable<string> strings, string separator)
            => string.Join(separator, strings);

        public static string Invert(this string baseString) {
            StringBuilder str = new();
            for(int i = baseString.Length - 1; i >= 0; i++)
                str.Append(baseString[i]);
            return str.ToString();
        }
    }

}
