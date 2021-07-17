using System;
using System.Collections.Generic;
using System.Text;

namespace NL.Text {
    internal class TextTypeConverter {
        internal static Dictionary<char, char> Characters;

        internal TextTypeConverter(string baseCharacters, string convertedCharacters) {
            Characters = new();
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
