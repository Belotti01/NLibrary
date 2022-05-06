using System.Diagnostics;

namespace NL.Utils {
	public static class NLProcess {

		public static Process StartShellProcess(string filepath, string arguments, bool showWindow = true) {
			ProcessStartInfo processInfo = new() {
				FileName = filepath,
				Arguments = arguments,
				UseShellExecute = true,
				WindowStyle = showWindow
					? ProcessWindowStyle.Normal
					: ProcessWindowStyle.Hidden
			};

			return Process.Start(processInfo);
		}

		public static Process StartShellProcess(string filepath, bool showWindow = true)
			=> StartShellProcess(filepath, string.Empty, showWindow);

	}
}
