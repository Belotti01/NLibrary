using NLCommon.Exceptions;

namespace NLCommon.Prompt {

	/// <summary>
	///     Methods to request console input from the user.
	/// </summary>
	public static class Input {

		/// <summary>
		///     Print a numbered list of objects of type <typeparamref name="T"/>,
		///     request a number input from the user, and return the picked item.
		/// </summary>
		/// <typeparam name="T">
		///     The type of the objects in the list.
		/// </typeparam>
		/// <param name="options">
		///     All items from which the user can pick from.
		/// </param>
		/// <returns>
		///     The object chosen by the user.
		/// </returns>
		public static T PickOne<T>(params T[] options) {
			if(options.Length == 0)
				throw new InvalidValueException<T[]>(options, nameof(options));
			for(int i = 0; i < options.Length; i++) {
				Console.WriteLine($"{i + 1}) {options[i]}");
			}

			int input;
			do {
				if(options.Length < 10) {
					//Less than 10 items - Just read the first input key.
					if(int.TryParse(Console.ReadKey().KeyChar.ToString(), out input)) {
						if(input > 0 && input <= options.Length) {
							Output.DeleteLine();
							return options[input - 1];
						}
					}
					Output.DeleteLine();
				} else {
					//More than 9 items - Wait for the enter key to be pressed.
					input = ReadInt(1, options.Length);
					Output.DeleteLine();
					return options[input - 1];
				}
			} while(true);
		}

		/// <param name="options">
		///     Dictionary where each key identifies an option to print in
		///     the console, and their value indicates the value to
		///     return when the user picks the relative string.
		/// </param>
		/// <inheritdoc cref="PickOne{T}(T[])"/>
		public static T PickOne<T>(Dictionary<string, T> options) {
			IEnumerable<string> list = options.Select(opt => opt.Key);
			return options[PickOne(list)];
		}

		/// <inheritdoc cref="PickOne{T}(T[])"/>
		public static T PickOne<T>(IEnumerable<T> options)
			=> PickOne(options.ToArray());

		#region Read<T>
		/// <summary>
		///     Request user input until the entered value is valid
		///     and within the range <paramref name="min"/>
		///     to <paramref name="max"/> (both inclusive).
		/// </summary>
		/// <param name="min">
		///     The inclusive minimum input value.
		/// </param>
		/// <param name="max">
		///     The inclusive maximum input value.
		/// </param>
		/// <returns>
		///     The first valid value entered by the user.
		/// </returns>
		public static int ReadInt(int min = -int.MaxValue, int max = int.MaxValue) {
			while(true) {
				if(int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max) {
					return value;
				}
				Output.DeleteLine();
			}
		}

		/// <inheritdoc cref="ReadInt(int, int)"/>
		public static double ReadDouble(double min = -double.MaxValue, double max = double.MaxValue) {
			while(true) {
				if(double.TryParse(Console.ReadLine(), out double value) && value >= min && value <= max) {
					return value;
				}
				Output.DeleteLine();
			}
		}

		/// <summary>
		///     Request user input until the entered line's length
		///     is within the range <paramref name="minLength"/>
		///     to <paramref name="maxLength"/> (both inclusive).
		/// </summary>
		/// <param name="minLength">
		///     The inclusive minimum length of the user input.
		/// </param>
		/// <param name="maxLength">
		///     The inclusive maximum length of the user input.
		/// </param>
		/// <returns>
		///     The first valid value entered by the user.
		/// </returns>
		public static string ReadLine(int minLength = -1, int maxLength = -1) {
			string input;
			bool isValid;

			while(true) {
				input = Console.ReadLine();
				isValid = !input.IsEmpty()
					&& (maxLength < 1 || input.Length <= maxLength)
					&& (minLength < 1 || input.Length >= minLength);
				if(isValid) {
					return input;
				}
				Output.DeleteLine();
			}
		}

		public static char ReadChar() {
			char input;
			do {
				input = Console.ReadKey().KeyChar;
			} while(char.IsControl(input));

			return input;
		}

		/// <summary>
		///     Read the first <see langword="char"/> user-input included in the
		///     specified <paramref name="options"/>.
		/// </summary>
		/// <param name="options">
		///     The possible return values awaited from the user.
		/// </param>
		/// <returns>
		///     A character picked by the user contained inside <paramref name="options"/>.
		/// </returns>
		public static char ReadChar(params char[] options) {
			char input;
			do {
				input = Console.ReadKey().KeyChar;
			} while(char.IsControl(input) || !options.Contains(input));

			return input;
		}

		/// <summary>
		///     Read the first <see langword="char"/> user-input included in the
		///     specified <paramref name="options"/>, case-insensitive.
		/// </summary>
		/// <param name="options">
		///     The possible return values awaited from the user.
		/// </param>
		/// <returns>
		///     The upper-case version of the character picked by the user, contained 
		///     inside <paramref name="options"/>.
		/// </returns>
		public static char ReadCharToUpper(params char[] options) {
			char input;
			options = options
				.Select(c => char.ToUpper(c))
				.ToArray();
			do {
				input = Console.ReadKey().KeyChar;
			} while(char.IsControl(input) || !options.Contains(input.ToUpper()));

			return input.ToUpper();
		}

		/// <summary>
		///     Read the first <see langword="char"/> user-input included in the
		///     specified <paramref name="options"/>, case-insensitive.
		/// </summary>
		/// <param name="options">
		///     The possible return values awaited from the user.
		/// </param>
		/// <returns>
		///     The lower-case version of the character picked by the user, contained 
		///     inside <paramref name="options"/>.
		/// </returns>
		public static char ReadCharToLower(params char[] options) {
			char input;
			options = options
				.Select(c => c.ToLower())
				.ToArray();
			do {
				input = Console.ReadKey().KeyChar;
			} while(char.IsControl(input) || !options.Contains(input.ToLower()));

			return input.ToLower();
		}

		public static string[] ReadArgs(bool acceptEmpty = false) {
			string[] args;
			do {
				args = ReadLine().SplitBySpaces();
			} while(args.Length == 0 || acceptEmpty);
			return args;
		}
		#endregion
	}

}
