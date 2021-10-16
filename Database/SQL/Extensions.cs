using NL.Extensions;
using System;

namespace NL.Database.SQL {
	public static class Extensions {
		private const string DATETIME_FORMATTER = "yyyy-MM-dd HH:mm:ss.fff";
		private const string DATE_FORMATTER = "yyyy-MM-dd";
		private const string TIME_FORMATTER = "HH:mm:ss.fff";

		public static string ToSQL(this string str, bool enclose = true) {
			if(str.IsNullOrDefault()) {
				return Wrap("", enclose);
			}else {
				str = str.Replace("'", "''");
				return Wrap(str, enclose);
			}
		}

		public static string ToSQLDateTime(this DateTime date, bool enclose = true) {
			return Wrap(date.ToString(DATETIME_FORMATTER), enclose);
		}

		public static string ToSQLTime(this DateTime date, bool enclose = true) {
			return Wrap(date.ToString(TIME_FORMATTER), enclose);
		}

		public static string ToSQLDate(this DateTime date, bool enclose = true) {
			return Wrap(date.ToString(DATE_FORMATTER), enclose);
		}

		private static string Wrap(string str, bool enclose) {
			return enclose
				? $"'{str}'"
				: str;
		}
	}
}
