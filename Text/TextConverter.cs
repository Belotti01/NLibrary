using System;
using System.Collections.Generic;
using System.Text;

namespace NL.Text {
    internal class TextConverter {
        internal static Dictionary<char, char> Characters;

        internal TextConverter(string baseCharacters, string convertedCharacters) {
            if(baseCharacters.Length != convertedCharacters.Length) {
                throw new Exception($"The amount of characters contained in the base string is not the same as its converted counterparts':\n" +
                    $"BASE: \"{baseCharacters}\"\n" +
                    $"CONVERTED: \"{convertedCharacters}\"");
            }

            for(int i = 0; i < baseCharacters.Length; i++) {
                Characters.Add(baseCharacters[i], convertedCharacters[i]);
            }
        }

        internal string Convert(string original) {
            StringBuilder str = new();
            foreach(char c in original) {
                if(Characters.ContainsKey(c))
                    str.Append(Characters[c]);
                else
                    str.Append(c);
            }
            return str.ToString();
        }
    }
}
