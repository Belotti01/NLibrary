namespace NL.Utils {
	public static class NLText {

		/// <summary>
		///     Create a <see langword="string"/> where the <see langword="char"/>  
		///     <paramref name="c"/> is repeated <paramref name="times"/> times.
		/// </summary>
		/// <param name="c">
		///     The <see langword="char"/> to repeat.
		/// </param>
		/// <param name="times">
		///     The amount of times the <paramref name="c"/> is repeated.
		/// </param>
		/// <returns>
		///     A <see langword="string"/> long <paramref name="times"/> filled with
		///     the requested <paramref name="c"/>.
		/// </returns>
		public static string Repeated(char c, int times) {
			// Pre-allocate memory
			StringBuilder sb = new(times);
			sb.Append(c, times);
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

	}
}
