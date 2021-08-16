using System;
using System.Collections.Generic;
using System.Text;

namespace NL.Extensions {
    public static class Characters {

        public static char ToUpper(this char character) {
            return char.IsLetter(character)
                ? char.ToUpper(character)
                : character;
        }

        public static char ToLower(this char character) {
            return char.IsLetter(character)
                ? char.ToLower(character)
                : character;
        }

    }
}
