namespace NLCommon.Utils {
	public static class NLMath {

		public static string ToBase(int n, int toBase) {
			return Convert.ToString(n, toBase);
		}

		public static int FromBase(string n, int fromBase) {
			return Convert.ToInt32(n, fromBase);
		}

	}
}
