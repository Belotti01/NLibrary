using NL.Exceptions;
using NL.Extensions;
using System;

namespace NL.Prompt {

    public static class Input {

        public static T PickOne<T>(params T[] options) {
            if (options.Length == 0)
                throw new InvalidValueException<T[]>(options, nameof(options));
            for (int i = 0; i < options.Length; i++) {
                Console.WriteLine($"{i + 1}) {options[i]}");
            }

            int input;
            do {
                if (options.Length < 10) {
                    //Less than 10 items - Just read the first input key
                    if (int.TryParse(Console.ReadKey().KeyChar.ToString(), out input)) {
                        if (input > 0 && input <= options.Length) {
                            Output.DeleteLine();
                            return options[input - 1];
                        }
                    }
                } else {
                    //More than 9 items - Wait for the enter key to be pressed
                    input = ReadInt(1, options.Length);
                    Output.DeleteLine();
                    return options[input - 1];
                }
            } while (true);
        }

        public static int ReadInt(int min = -int.MaxValue, int max = int.MaxValue) {
            while (true) {
                if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max) {
                    return value;
                }
                Output.DeleteLine();
            }
        }

        public static double ReadDouble(double min = -double.MaxValue, double max = double.MaxValue) {
            while (true) {
                if (double.TryParse(Console.ReadLine(), out double value) && value >= min && value <= max) {
                    return value;
                }
                Output.DeleteLine();
            }
        }

        public static string ReadLine(int minLength = -1, int maxLength = -1) {
            string input;
            bool isValid;

            while (true) {
                input = Console.ReadLine();
                isValid = !input.IsEmpty()
                    && (maxLength < 1 || input.Length <= maxLength)
                    && (minLength < 1 || input.Length >= minLength);
                if (isValid) {
                    return input;
                }
                Output.DeleteLine();
            }
        }
    }

}
