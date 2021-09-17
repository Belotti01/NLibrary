using System;

namespace NL.Prompt {

    public static class Output {

        public static void Write(object text) {
            Console.ResetColor();
            Console.Write(text);
        }

        public static void WriteLine(object text) {
            Console.ResetColor();
            Console.WriteLine(text);
        }

        public static void Write(object text, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void WriteLine(object text, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
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
            for (int i = 0; i < amount; i++) {
                try {
                    DeleteLine();
                } catch {
                    return;
                }
            }
        }

    }

}
