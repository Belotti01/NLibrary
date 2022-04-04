using System;

namespace NL.Utils.Types {
	internal struct NLDateTime {
		#region Casts
		public static implicit operator NLDateTime(DateTime value) => new(value);
		public static implicit operator NLDateTime(DateOnly value) => new(value.ToDateTime(TimeOnly.MinValue));
		// Cast from TimeOnly is most likely useless

		public static implicit operator DateTime(NLDateTime value) => value.LocalDateTime.ToLocalTime();
		public static implicit operator DateOnly(NLDateTime value) => value.LocalDate;
		public static implicit operator TimeOnly(NLDateTime value) => value.LocalTime;
		#endregion


		private DateTime _utcValue;
		/// <summary> Retrieve the stored <see cref="DateTime"/> converted to the local time zone. </summary>
		public DateTime LocalDateTime {
			get => _utcValue.ToLocalTime();
			set => UtcDateTime = value;
		}

		/// <summary> Retrieve the stored <see cref="DateTime"/> as Coordinated Universal Time (UTC). </summary>
		public DateTime UtcDateTime {
			get => _utcValue;
			set => _utcValue = value.ToUniversalTime();
		}

		public DateOnly LocalDate {
			get => UtcDateTime.ToLocalTime().GetDate();
			set => UtcDateTime = value.ToDateTime(LocalTime, DateTimeKind.Utc);
		}
		public DateOnly UtcDate {
			get => UtcDateTime.GetDate();
			set => LocalDate = value;
		}

		public TimeOnly LocalTime {
			get => UtcDateTime.ToLocalTime().GetTime();
			set => UtcDateTime = LocalDate.ToDateTime(value, DateTimeKind.Utc);
		}

		public TimeOnly UtcTime {
			get => UtcDateTime.GetTime();
			set => LocalTime = value;
		}


		public NLDateTime(DateTime value = default) {
			_utcValue = value.ToUniversalTime();
		}

	}
}
