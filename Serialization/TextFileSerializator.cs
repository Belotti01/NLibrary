using NLCommon.Exceptions;
using System.IO;
using System.Threading;
using System.Timers;

namespace NLCommon.Serialization {
	/// <summary>
	///     Collection of methods used to safely access files as text files, without risking
	///     deadlocks or <see cref="IOException"/>s.
	/// </summary>
	public static class TextFileSerializator {
		internal const int BASE_TIMEOUT = 3000;

		/// <returns>An array of <see cref="string"/> containing all the lines of the file.</returns>
		/// <inheritdoc cref="Read(string, int)"/>
		public static string[] ReadLines(string filepath, int timeout = BASE_TIMEOUT) {
			if(timeout <= 0)
				throw new InvalidValueException<int>(timeout, nameof(timeout));
			if(!File.Exists(filepath))
				return default;

			string[] result;
			bool timedOut = false;
			System.Timers.Timer timer = new(timeout);
			timer.Elapsed += (object sender, ElapsedEventArgs e) => timedOut = true;
			timer.Start();
			while(!timedOut) {
				try {
					result = File.ReadAllLines(filepath);
					timer.Stop();
					timer.Dispose();
					return result;
				} catch(IOException) {
					Thread.Sleep(15);
				}
			}
			timer.Dispose();
			throw new FileAccessTimeoutException(filepath, timeout);
		}

		/// <summary>
		///     Open the file <paramref name="filepath"/> and reads all its contents.
		/// </summary>
		/// <param name="filepath">
		///     The relative or absolute path of the file.
		/// </param>
		/// <param name="timeout">
		///     How long (in ms) to retry accessing the file for in case of continuous failure.
		/// </param>
		/// <returns>
		///     A single <see langword="string"/> containing all of the file's content.
		/// </returns>
		/// <exception cref="FileAccessTimeoutException"/>
		public static string Read(string filepath, int timeout = BASE_TIMEOUT) {
			if(timeout <= 0)
				throw new InvalidValueException<int>(timeout, nameof(timeout));
			if(!File.Exists(filepath))
				return default;

			string result;
			bool timedOut = false;
			System.Timers.Timer timer = new(timeout);
			timer.Elapsed += (object sender, ElapsedEventArgs e) => timedOut = true;
			timer.Start();
			while(!timedOut) {
				try {
					result = File.ReadAllText(filepath);
					timer.Stop();
					timer.Dispose();
					return result;
				} catch(IOException) {
					Thread.Sleep(15);
				}
			}
			timer.Dispose();
			throw new FileAccessTimeoutException(filepath, timeout);
		}

		/// <inheritdoc cref="Read(string, int)"/>
		/// <summary>
		///		Create or overwrite the file <paramref name="filepath"/> and write <paramref name="content"/>
		///		inside of it within <paramref name="timeout"/>ms.
		/// </summary>
		/// <param name="filepath">The path and filename of the text file to write to.</param>
		/// <param name="content">The text to be written in the text file.</param>
		/// <exception cref="FileAccessTimeoutException"/>
		public static void Write(string filepath, string content, int timeout = BASE_TIMEOUT) {
			if(timeout <= 0)
				throw new InvalidValueException<int>(timeout, nameof(timeout));

			bool timedOut = false;
			System.Timers.Timer timer = new(timeout);
			timer.Elapsed += (object sender, ElapsedEventArgs e) => timedOut = true;
			timer.Start();
			while(!timedOut) {
				try {
					File.WriteAllText(filepath, content);
					timer.Stop();
					timer.Dispose();
					return;
				} catch(IOException) {
					Thread.Sleep(15);
				}
			}
			timer.Dispose();
			throw new FileAccessTimeoutException(filepath, timeout);
		}
	}

}
