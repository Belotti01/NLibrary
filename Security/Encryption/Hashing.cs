using System.Security.Cryptography;

namespace NLCommon.Security.Encryption {
	public static class Hashing {
		public static string Sha512(string text) {
			var bytes = Encoding.UTF8.GetBytes(text);
			using SHA512 hash = SHA512.Create();
			var hashedTextBytes = hash.ComputeHash(bytes);

			// Pre-allocate memory: 128 = 512bits / 8bits * 2 characters
			string hashedText = string.Join(string.Empty, hashedTextBytes
					.Select(x => x.ToString("X2")));
			return hashedText;
		}
	}
}
