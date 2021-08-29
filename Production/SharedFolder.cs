using NL.Production.Shared;
using NL.Serialization;
using System.IO;
using System.Text;

namespace NL.Production {
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
        public static readonly SharedConfiguration Configuration;

        static SharedFolder() {
            // Initialize filepaths and create missing folders & files
            string root = Path.GetPathRoot(Directory.GetCurrentDirectory());
            SharedSettingsFolder = Path.Combine(root, "CheapHouse");

            Directory.CreateDirectory(SharedSettingsFolder);
            SharedSettingsFilePath = Path.Combine(SharedSettingsFolder, SHARED_SETTINGS_FILENAME);
            if(!Json.TryDeserialize(SharedSettingsFilePath, out Configuration)) {
                Configuration = new SharedConfiguration();
                Json.Serialize(Configuration, SharedSettingsFilePath);
            }
        }
    }
}
