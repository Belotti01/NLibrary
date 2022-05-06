namespace NL.Extensions {
	public static class DateTimes {
		public static TimeOnly GetTime(this DateTime dateTime)
			=> TimeOnly.FromDateTime(dateTime);

		public static DateOnly GetDate(this DateTime dateTime)
			=> DateOnly.FromDateTime(dateTime);

	}
}
