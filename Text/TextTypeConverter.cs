namespace NLCommon.Text {
	internal class TextTypeConverter {
		internal Dictionary<char, char> Characters;

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

		internal char Convert(char original) {
			if(Characters.ContainsKey(original))
				return Characters[original];
			else
				return original;
		}
	}
}
