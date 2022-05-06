using NL.Exceptions;
using System.Timers;

namespace NL.Extensions {

	public static class Actions {

		/// <summary>
		///     Execute a delegate until its execution is successful if it takes less than
		///     <paramref name="timeout"/> ms.
		/// </summary>
		/// <param name="timeout">
		///     The maximum time range in which to retry to complete the action.
		/// </param>
		/// <param name="result">
		///     The result returned by the action if successful. <see langword="default"/> otherwise.
		/// </param>
		/// <param name="parameters">
		///     The parameters to forward to the delegate.
		/// </param>
		/// <returns>
		///     <see langword="true"/> if the action has been successfully completed within <paramref name="timeout"/>
		///     ms, <see langword="false"/> otherwise.
		/// </returns>
		public static bool WithTimeout<T>(this Delegate action, int timeout, out T result, params object[] parameters) {
			if(timeout <= 0)
				throw new InvalidValueException<int>(timeout, nameof(timeout));

			bool timedOut = false;
			Timer timer = new(timeout);
			timer.Elapsed += (object sender, ElapsedEventArgs e) => timedOut = true;
			timer.Start();
			while(!timedOut) {
				try {
					result = (T)action.DynamicInvoke(parameters);
					timer.Stop();
					timer.Dispose();
					return true;
				} catch { }
			}
			timer.Dispose();
			result = default;
			return false;
		}

		/// <inheritdoc cref="WithTimeout{T}(Delegate, int, out T, object[])"/>
		public static bool WithTimeout(this Delegate action, int timeout) {
			if(timeout <= 0)
				throw new InvalidValueException<int>(timeout, nameof(timeout));

			bool timedOut = false;
			Timer timer = new(timeout);
			timer.Elapsed += (object sender, ElapsedEventArgs e) => timedOut = true;
			timer.Start();
			while(!timedOut) {
				try {
					action.DynamicInvoke();
					timer.Stop();
					timer.Dispose();
					return true;
				} catch { }
			}
			timer.Dispose();
			return false;
		}

		/// <inheritdoc cref="WithTimeout{T}(Delegate, int, out T, object[])"/>
		public static bool WithTimeout(this Delegate action, int timeout, params object[] parameters) {
			if(timeout <= 0)
				throw new InvalidValueException<int>(timeout, nameof(timeout));

			bool timedOut = false;
			Timer timer = new(timeout);
			timer.Elapsed += (object sender, ElapsedEventArgs e) => timedOut = true;
			timer.Start();
			while(!timedOut) {
				try {
					action.DynamicInvoke(parameters);
					timer.Stop();
					timer.Dispose();
					return true;
				} catch { }
			}
			timer.Dispose();
			return false;
		}

		/// <summary>
		///     Ignore any <see cref="Exception"/> thrown by this <see cref="Delegate"/>.
		/// </summary>
		/// <param name="action">
		///     The procedure to execute.
		/// </param>
		/// <param name="parameters">
		///     The parameters to forward to the <see cref="Delegate"/>.
		/// </param>
		public static void IgnoreExceptions(this Delegate action, params object[] parameters) {
			try {
				action.DynamicInvoke(parameters);
			} catch { }
		}

		/// <inheritdoc cref="IgnoreExceptions(Delegate, object[])"/>
		public static void IgnoreExceptions(this Delegate action) {
			try {
				action.DynamicInvoke();
			} catch { }
		}

		/// <returns>
		///     The result of type <typeparamref name="T"/> returned by the instance of
		///     the <see cref="Delegate"/>, or <see langword="default"/> if an
		///     <see cref="Exception"/> is thrown by the call.
		/// </returns>
		/// <inheritdoc cref="IgnoreExceptions(Delegate, object[])"/>
		public static T IgnoreExceptions<T>(this Delegate action, params object[] parameters) {
			try {
				return (T)action.DynamicInvoke(parameters);
			} catch {
				return default;
			}
		}

		/// <inheritdoc cref="IgnoreExceptions{T}(Delegate, object[])"/>
		public static T IgnoreExceptions<T>(this Delegate action) {
			try {
				return (T)action.DynamicInvoke();
			} catch {
				return default;
			}
		}
	}

}
