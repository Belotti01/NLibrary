namespace NLCommon.Extensions {

	public static class Tasks {

		/// <inheritdoc cref="Task.Wait()"/>
		public static T Wait<T>(this Task<T> task) {
			task.Wait();
			return task.Result;
		}

		/// <summary>
		///     Ignore any <see cref="Exception"/> thrown by this <see cref="Task"/>.
		/// </summary>
		public static async Task IgnoreExceptions(this Task task) {
			try {
				task.Start();
				await task;
			} catch { }
		}

		/// <summary>
		///     Ignore any <see cref="Exception"/> thrown by this <see cref="Task"/>.
		/// </summary>
		/// <returns>
		///     The result of the <see cref="Task"/> if no <see cref="Exception"/> is thrown,
		///     <see langword="default"/> otherwise.
		/// </returns>
		public static async Task<T> IgnoreExceptions<T>(this Task<T> task) {
			try {
				task.Start();
				return await task;
			} catch {
				return default;
			}
		}
	}

}
