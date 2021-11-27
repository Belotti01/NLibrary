#nullable enable

using System;
using System.Collections.Generic;
using System.Text;

namespace NL.Extensions {
    public static class StringBuilders {
        /// <summary>
        ///     Check if the <see langword="string"/> built from the
        ///     <see cref="StringBuilder"/> is empty or formed by
        ///     whitespace characters only.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="StringBuilder"/>
        ///     contains no characters or whitespace characters only;
        ///     <see langword="false"/> otherwise.
        /// </returns>
        public static bool IsEmptyOrWhiteSpace(this StringBuilder? sb) {
            return sb is null || sb.ToString().IsEmpty();
        }

        /// <summary>
        ///     Check if the <see langword="string"/> built from the
        ///     <see cref="StringBuilder"/> is empty or formed by
        ///     whitespace characters only.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="StringBuilder"/>
        ///     contains no characters or whitespace characters only;
        ///     <see langword="false"/> otherwise.
        /// </returns>
        public static bool IsNullOrEmpty(this StringBuilder? sb) {
            return sb is null || sb.Length == 0;
        }

        /// <inheritdoc cref="StringBuilder.AppendLine(string)"/>
        public static StringBuilder AppendLine(this StringBuilder sb, object? value) {
            return sb.AppendLine(value?.ToString());
        }

        /// <inheritdoc cref="StringBuilder.Append(object)"/>
        /// <param name="times">
        ///     How many times to repeat the given <paramref name="value"/>.
        /// </param>
        public static StringBuilder Append(this StringBuilder sb, object? value, int times) {
            for (int i = 0; i < times; i++) {
                sb.Append(value ?? "");
            }
            return sb;
        }

        /// <inheritdoc cref="StringBuilder.AppendLine(string)"/>
        /// <param name="times">
        ///     How many times to repeat the given <paramref name="value"/>.
        /// </param>
        public static StringBuilder AppendLine(this StringBuilder sb, object? value, int times) {
            for (int i = 0; i < times; i++) {
                sb.AppendLine(value ?? "");
            }
            return sb;
        }

        /// <inheritdoc cref="StringBuilder.AppendLine(string)"/>
        /// <param name="lines">
        ///     The lines to append to the <see cref="StringBuilder"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="lines"/> is <see langword="null"/>
        /// </exception>
        public static StringBuilder AppendLines(this StringBuilder sb, IEnumerable<object>? lines) {
            _ = lines ?? throw new ArgumentNullException(nameof(lines));

            foreach (object line in lines) {
                sb.AppendLine(line);
            }
            return sb;
        }
    }
}
