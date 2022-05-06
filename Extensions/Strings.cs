#nullable enable
using NL.Security.Encryption;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace NL.Extensions {

	public static class Strings {


		#region String Parsing

		/// <inheritdoc cref="INumber{TSelf}.Parse(ReadOnlySpan{char}, NumberStyles, IFormatProvider?)"/>
		public static T Parse<T>(this string s) where T : INumber<T> {
			return T.Parse(s, NumberFormatInfo.InvariantInfo);
		}

		/// <inheritdoc cref="INumber{TSelf}.TryParse(ReadOnlySpan{char}, NumberStyles, IFormatProvider?, out TSelf)"/>
		public static bool TryParse<T>([NotNullWhen(true)] this string? s, out T result) where T : INumber<T> {
			return T.TryParse(s, NumberFormatInfo.InvariantInfo, out result);
		}

		/// <summary>
		///     Search for the first instance of a number in the 
		///     <see langword="string"/>.
		/// </summary>
		/// <param name="number">
		///     The first number of type <typeparamref name="T"/> found in the 
		///     <see langword="string"/>, or 0 if none is present.
		/// </param>
		/// <returns>
		///     <see langword="true"/> if a valid number is found, 
		///     <see langword="false"/> otherwise.
		/// </returns>
		public static bool TryFindNumber<T>(this string? str, out T number) where T : IParseable<T>, INumber<T> {
			number = default!;

			if(str is null) {
				return false;
			}

			if(number is double or float or decimal) {
				return str.TryFindDecimal(out number);
			} else {
				return str.TryFindInteger(out number);
			}
		}

		/// <summary>
		///     Search for instances of numbers of type <typeparamref name="T"/> in the 
		///     <see langword="string"/>.
		/// </summary>
		/// <param name="numbers">
		///     All the numeric values found in the <see langword="string"/>, or 
		///     an empty array if none is present.
		/// </param>
		/// <returns>
		///     <see langword="true"/> if at least one valid numeric value 
		///     is found, <see langword="false"/> otherwise.
		/// </returns>
		public static bool TryFindNumbers<T>(this string? str, out T[] numbers) where T : INumber<T> {
			numbers = Array.Empty<T>();
			if(str is null) {
				return false;
			}

			if(numbers is double[] or float[] or decimal[]) {
				return str.TryFindDecimals(out numbers!);
			} else {
				return str.TryFindIntegers(out numbers!);
			}
		}


		private static bool TryFindInteger<T>(this string str, out T number) where T : INumber<T> {
			StringBuilder found = new();
			bool result = false;

			for(int i = 0; i < str.Length; i++) {
				if(char.IsDigit(str, i)) {
					found.Append(str[i]);
				} else if(found.Length != 0) {
					result = true;
					break;
				}
			}

			number = result
				? T.Parse(found.ToString(), NumberFormatInfo.CurrentInfo)
				: default!;
			return result;
		}

		private static bool TryFindIntegers<T>([NotNullWhen(true)] this string? str, out T[]? numbers) where T : INumber<T> {
			StringBuilder found = new();
			List<T> parsed = new();

			if(str is null) {
				numbers = null;
				return false;
			}

			for(int i = 0; i < str.Length; i++) {
				if(char.IsDigit(str, i)) {
					found.Append(str[i]);
				} else if(found.Length != 0) {
					parsed.Add(found.ToString().Parse<T>());
					found.Clear();
				}
			}

			if(found.Length != 0) {
				parsed.Add(found.ToString().Parse<T>());
			}

			numbers = parsed.ToArray();
			return numbers.Length != 0;
		}

		private static bool TryFindDecimal<T>(this string? str, out T number) where T : INumber<T> {
			StringBuilder found = new();
			bool pointFound = false;
			bool result;

			if(str is null) {
				number = default!;
				return false;
			}

			for(int i = 0; i < str.Length; i++) {
				if(char.IsDigit(str, i)) {
					found.Append(str[i]);
				} else if((str[i] == '.' || str[i] == ',') && !pointFound) {
					if(found.Length == 0) {
						found.Append('0');
					}
					pointFound = true;
					found.Append('.');
				} else if(found.Length != 0) {
					break;
				}
			}

			result = found.Length != 0;
			number = result
				? found.ToString().Parse<T>()
				: default!;
			return result;
		}

		private static bool TryFindDecimals<T>([NotNullWhen(true)] this string? str, out T[]? numbers) where T : INumber<T> {
			StringBuilder found = new();
			List<T> parsed = new();
			bool pointFound = false;

			if(str is null) {
				numbers = null;
				return false;
			}

			for(int i = 0; i < str.Length; i++) {
				if(char.IsDigit(str, i)) {
					found.Append(str[i]);
				} else if((str[i] == '.' || str[i] == ',') && !pointFound) {
					if(found.Length == 0) {
						found.Append('0');
					}
					pointFound = true;
					found.Append('.');
				} else if(found.Length != 0) {
					parsed.Add(found.ToString().Parse<T>());
					found.Clear();
					pointFound = false;
				}
			}

			if(found.Length != 0) {
				parsed.Add(found.ToString().Parse<T>());
			}

			numbers = parsed.ToArray();
			return numbers.Length != 0;
		}
		#endregion


		#region Encryption & Hashing
		public static string ToSha512(this string str)
			=> Hashing.Sha512(str);
		#endregion


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
		public static string? Truncate(this string? str, int maxLength) {
			if(str is not null && str.Length > maxLength)
				return str[..maxLength];
			else
				return str;
		}

		/// <summary>
		///     Shorten the <see cref="string"/> to have at most <paramref name="maxLength"/>
		///     characters, and replace the last 3 characters with "..." if any character
		///     is removed.
		/// </summary>
		/// <returns>
		///     The original <see cref="string"/> if its length is less than <paramref name="maxLength"/>,
		///     otherwise a new <see cref="string"/> containing the first <paramref name="maxLength"/>
		///     characters of the original, minus the last 3 characters which are replaced with
		///     "...". If the <paramref name="maxLength"/> is less than 4, the <see cref="string"/>
		///     "..." is returned instead.
		/// </returns>
		/// <inheritdoc cref="Truncate(string, int)"/>
		public static string TruncateWithSuspension(this string str, int maxLength) {
			if(maxLength is < 3 and >= 0)
				return "...";
			else if(str.Length < maxLength)
				return str;
			else
				return $"{str[..(maxLength - 3)]}...";
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
		public static string[] SplitBySpaces(this string? str) {
			// Avoided using a simple regex for a performance boost
			var words = Enumerable.Empty<string>();
			StringBuilder word = new();
			if(str is null)
				return words.ToArray();

			foreach(char c in str) {
				if(char.IsWhiteSpace(c)) {
					if(word.Length != 0) {
						words = words.Append(word.ToString());
						word.Clear();
					}
				} else {
					word.Append(c);
				}
			}
			if(word.Length != 0) {
				words.Append(word.ToString());
				word.Clear();
			}

			return words.ToArray();
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
		public static bool EqualsIgnoreCase(this string? str, string? other)
			=> string.Equals(str, other, StringComparison.OrdinalIgnoreCase);

		/// <summary>
		///     Extendable version of <see cref="string.IsNullOrWhiteSpace(string)"/>.
		/// </summary>
		/// <returns>
		///     <see langword="true"/> if the <see cref="string"/> is <see langword="null"/>,
		///     equals <see cref="string.Empty"/> or contains whitespace characters only.
		/// </returns>
		public static bool IsEmpty([NotNullWhen(false)] this string? str)
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
		///     the <see cref="string"/> as a whole. Also returns <see langword="false"/> if either
		///     the <see cref="string"/> or the <paramref name="word"/> is <see langword="null"/>.
		/// </returns>
		public static bool ContainsWord([MaybeNullWhen(false)] this string? str, [MaybeNullWhen(false)] string? word, StringComparison stringComparison = StringComparison.CurrentCulture) {
			if(str is null || word is null)
				return false;

			word = word.Trim();
			if(word.Contains(' '))
				return str.Contains(word, stringComparison);

			string[] args = str.SplitBySpaces();
			foreach(string arg in args) {
				if(arg.Equals(word, stringComparison)) {
					return true;
				}
			}
			return false;
		}

		/// <param name="ignoreCase">
		///     Whether to ignore case differences for characters or not.
		/// </param>
		/// <inheritdoc cref="ContainsWord(string, string, StringComparison)"/>
		public static bool ContainsWord([MaybeNullWhen(false)] this string? str, string? word, bool ignoreCase = false)
			=> str?.ContainsWord(word, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) ?? false;

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

		/// <summary>
		///     Invert the order of <see langword="char"/>s in the
		///     <see langword="string"/>.
		/// </summary>
		/// <returns>
		///     A copy of the <see langword="string"/> with the order of
		///     its <see langword="char"/>s backwards.
		/// </returns>
		public static string? Inverted(this string? baseString) {
			if(baseString is null)
				return null;

			StringBuilder str = new();
			for(int i = baseString.Length - 1; i >= 0; i--)
				str.Append(baseString[i]);
			return str.ToString();
		}

		/// <summary>
		///     Split the <see langword="string"/> for every instance of any
		///     of the specified <paramref name="separators"/>.
		/// </summary>
		/// <param name="skipWhitespaces">
		///     If <see langword="true"/>, resulting <see langword="string"/>s
		///     will already be trimmed.
		/// </param>
		/// <param name="separators">
		///     All the delimiting <see langword="char"/>s that, when found,
		///     will split the <see langword="string"/>.
		/// </param>
		/// <returns>
		///     An array of <see langword="string"/>s containing the non-empty
		///     split parts of the original <see langword="string"/>.
		/// </returns>
		public static string[] Separate(this string str, bool skipWhitespaces, params char[] separators) {
			// Avoided using a Regex for HUGE performance boost;
			// Tested times with the same parameters:
			// Average with Regex.Split: ~58ms
			// Average with this method: ~2ms
			Stack<string> list = new();
			StringBuilder item = new();

			foreach(char c in str) {
				if(separators.Contains(c)) {
					if(item.Length != 0) {
						if(skipWhitespaces)
							list.Push(item.ToString().TrimEnd());
						else
							list.Push(item.ToString());
						item.Clear();
					}
				} else {
					if(!(skipWhitespaces && item.IsNullOrEmpty() && char.IsWhiteSpace(c))) {
						item.Append(c);
					}
				}
			}
			return list.ToArray();
		}

		/// <inheritdoc cref="Separate(string, bool, char[])"/>
		public static string[] Separate(this string str, params char[] separators)
			=> str.Separate(false, separators);



		/// <summary>
		///     Checks whether the string is filled with whitespace-characters only.
		/// </summary>
		/// <param name="str">
		///     The <see langword="string"/> to check.
		/// </param>
		/// <returns>
		///     <see langword="false"/> if the <paramref name="str"/> is <see langword="null"/>,
		///     empty, or contains any non-whitespace character. <see langword="true"/> otherwise.
		/// </returns>
		public static bool IsWhiteSpace(this string? str) {
			if(str?.IsNullOrDefault() ?? true) {
				return false;
			} else {
				foreach(char c in str) {
					if(!char.IsWhiteSpace(c)) {
						return false;
					}
				}
				return true;
			}
		}

		/// <summary>
		///     Split the <see langword="string"/> in 2 parts, where the first 
		///     takes the first <paramref name="index"/> <see langword="char"/>s, 
		///     while the second contains the remainder.
		/// </summary>
		/// <param name="str">
		///     The <see langword="string"/> to split.
		/// </param>
		/// <param name="index">
		///     The length of the first <see langword="string"/>.
		/// </param>
		/// <returns>
		///     A 2-items long <see langword="string[]"/> containing the 2 aforementioned
		///     parts of the <paramref name="str"/>.
		/// </returns>
		/// <exception cref="IndexOutOfRangeException"/>
		public static string[] SplitAt(this string str, int index) {
			return new string[] {
				str[..index], str[index..]
			};
		}

		/// <summary>
		///     Uppercase the first letter found from the start of the <see langword="string"/>.
		/// </summary>
		/// <returns>
		///     A new <see langword="string"/> where the first letter from the start is turned to uppercase.
		/// </returns>
		public static string? UppercaseFirstLetter(this string? str) {
			if(str.IsEmpty()) {
				return str;
			}

			for(int i = 0; i < str.Length; i++) {
				if(char.IsLetter(str[i])) {
					if(char.IsUpper(str[i])) {
						// Already uppercase - no changes required
						return str;
					}

					string[] parts = str.SplitAt(i);
					// parts[1] will never be 0 chars long - hence no check is required
					if(parts[1].Length == 1) {  //Would throw IndexOutOfRangeException if handled normally
						return $"{parts[0]}{parts[1].ToUpper()}";
					} else {
						parts[1] = $"{parts[1][0].ToUpper()}{parts[1][1..]}";
						return $"{parts[0]}{parts[1]}";
					}
				}
			}
			return str; // No letters found
		}

		public static bool IsValidDate([MaybeNullWhen(false)] this string? str) {
			return !str.IsEmpty() && DateTime.TryParse(str, out _);
		}

		public static string Wrapped(this string? str, string wrapper, bool spaced = true) {
			str ??= string.Empty;
			return spaced
				? $"{wrapper} {str} {wrapper}"
				: $"{wrapper}{str}{wrapper}";
		}
	}
}
