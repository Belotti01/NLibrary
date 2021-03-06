using NLCommon.Production.Shared;
using NLCommon.Serialization;
using System.IO;

namespace NLCommon.Production {
	/// <summary>
	///     Access point to files, folders and settings shared
	///     between programs.
	/// </summary>
	public static partial class SharedFolder {

		private const string SHARED_SETTINGS_FILENAME = "CheapHouse_Config.json";
		/// <summary>
		///     The full path to the folder storing the programs' settings and data.
		/// </summary>
		public static readonly string SharedSettingsFolder;
		/// <summary>
		///     The full path to the shared configuration's json file.
		/// </summary>
		public static readonly string SharedSettingsFilePath;
		/// <summary>
		///     The configuration shared between programs.
		/// </summary>
		public static SharedConfiguration Configuration { get; private set; }

		static SharedFolder() {
			// Initialize filepaths and create missing folders & files
			string root = Path.GetPathRoot(Directory.GetCurrentDirectory());
			SharedSettingsFolder = Path.Combine(root, "Program Files", "CheapHouse");

			Directory.CreateDirectory(SharedSettingsFolder);
			SharedSettingsFilePath = Path.Combine(SharedSettingsFolder, SHARED_SETTINGS_FILENAME);
			ReadConfiguration();
		}

		internal static void ReadConfiguration() {
			if(JsonSerializator.TryDeserialize(SharedSettingsFilePath, out SharedConfiguration config)) {
				Configuration = config;
			} else {
				Configuration = new SharedConfiguration();
				JsonSerializator.Serialize(Configuration, SharedSettingsFilePath);
			}
		}
	}
}
