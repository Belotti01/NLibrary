using NLCommon.Utils;

namespace NLCommon.Extensions {

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

		/// <inheritdoc cref="INumber{TSelf}.Clamp(TSelf, TSelf, TSelf)"/>
		public static T Clamp<T>(this T value, T min, T max) where T : INumber<T> {
			return T.Clamp(value, min, max);
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
		public static string ToString<T>(this T value, int decimals) where T : INumber<T> {
			string[] parts = value.ToString().Split('.');
			StringBuilder ret = new(parts[0]);
			if(decimals == 0) {
				return ret.ToString();
			}

			if(parts.Length == 1) {
				ret.Append(NLText.Repeated('0', decimals));
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
	}

}
