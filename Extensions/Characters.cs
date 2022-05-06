using NL.Utils;

namespace NL.Extensions {
	public static class Characters {

		/// <inheritdoc cref="char.ToUpper(char)"/>
		public static char ToUpper(this char c) {
			return char.IsLetter(c)
				? char.ToUpper(c)
				: c;
		}

		/// <inheritdoc cref="char.ToLower(char)"/>
		public static char ToLower(this char c) {
			return char.IsLetter(c)
				? char.ToLower(c)
				: c;
		}

		/// <summary>
		///     Convert the numeric value of the <paramref name="c"/> to its
		///     equivalent <see langword="int"/>.
		/// </summary>
		/// <param name="c">
		///     The <see langword="char"/> to parse.
		/// </param>
		/// <returns>
		///     The <see langword="int"/> equivalent of the <paramref name="c"/>,
		///     or -1 if it's not a valid number.
		/// </returns>
		public static int Parse(this char c) {
			return (int)char.GetNumericValue(c);
		}

		/// <param name="number">
		///     The <see langword="int"/> equivalent of the <paramref name="c"/>,
		///     or -1 if it's not a valid number.
		/// </param>
		/// <returns>
		///     <see langword="true"/> if <paramref name="c"/> is a valid
		///     numeric value, <see langword="false"/> otherwise.
		/// </returns>
		/// <inheritdoc cref="Parse(char)"/>
		public static bool TryParse(this char c, out int number) {
			number = (int)char.GetNumericValue(c);
			return number != -1;
		}

		/// <inheritdoc cref="NLText.Repeated(char, int)"/>
		public static string ToString(this char c, int times)
			=> NLText.Repeated(c, times);

	}
}
