﻿using System;
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
        public static bool IsEmptyOrWhitespace(this StringBuilder str) {
            return str.ToString().IsEmpty();
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
        public static bool IsNullOrEmpty(this StringBuilder str) {
            return str is null || str.Length == 0;
        }
    }
}