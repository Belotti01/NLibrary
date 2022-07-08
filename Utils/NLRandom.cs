using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLCommon.Utils {
	public static class NLRandom {
		public static char RandomLetter(bool randomCase = true) {
			char c = (char)Random.Shared.Next('A', 'Z' + 1);
			if(randomCase && Random.Shared.Next(2) == 1) {
				return c.ToLower();
			}
			return c;
		}

		public static string RandomLetters(int amount, bool randomCase = true) {
			StringBuilder sb = new(amount);
			for(int i = 0; i < amount; i++) {
				sb.Append(RandomLetter(randomCase));
			}

			return sb.ToString();
		}

		public static char RandomLetterOrNumber(bool randomCase) {
			char c = (char)Random.Shared.Next('A', 'Z' + 11);

			if(c > 'Z') {
				return (c - 'Z').ToString()[0];
			}
			
			if(randomCase && Random.Shared.Next(2) == 1) {
				return c.ToLower();
			}

			return c;
		}

		public static string RandomLetterOrNumbers(int amount, bool randomCase = true) {
			StringBuilder sb = new(amount);
			for(int i = 0; i < amount; i++) {
				sb.Append(RandomLetterOrNumber(randomCase));
			}

			return sb.ToString();
		}
	}
}
