using NL.Utils;

namespace NL.Prompt {

	public static class Output {
		private static int _indentation = 0;
		/// <summary> The indentation level of the outputted text. </summary>
		public static int Indentation {
			get => _indentation;
			set => _indentation = Math.Max(value, 0);
		}

		private static string _indentationString = "\t";
		/// <summary> 
		///     The string to prepend <see cref="Indentation"/> times to the output text.
		/// </summary>
		public static string IndentationString {
			get => _indentationString;
			set => _indentationString = value ?? string.Empty;
		}

		private static string Indent(object text) {
			return _indentation == 0
				? text.ToString()
				: $"{NLText.Repeated(_indentationString, _indentation)}{text}";
		}

		public static void Write(object text) {
			Console.ResetColor();
			Console.Write(Indent(text));
		}

		public static void WriteLine(object text) {
			Console.ResetColor();
			Console.WriteLine(Indent(text));
		}

		public static void Write(object text, ConsoleColor color) {
			Console.ForegroundColor = color;
			Console.Write(Indent(text));
			Console.ResetColor();
		}

		public static void WriteLine(object text, ConsoleColor color) {
			Console.ForegroundColor = color;
			Console.WriteLine(Indent(text));
			Console.ResetColor();
		}

		/// <summary>
		///     Delete a single line from the <see cref="Console"/> interface.
		/// </summary>
		public static void DeleteLine() {
			int currentLineCursor = Console.CursorTop;
			Console.SetCursorPosition(0, Console.CursorTop);
			Console.Write(new string(' ', Console.BufferWidth));
			Console.SetCursorPosition(0, currentLineCursor);
		}

		/// <summary>
		///     Delete <paramref name="amount"/> lines from the <see cref="Console"/> interface.
		/// </summary>
		public static void DeleteLines(int amount) {
			for(int i = 0; i < amount; i++) {
				try {
					DeleteLine();
				} catch {
					return;
				}
			}
		}

	}

}
