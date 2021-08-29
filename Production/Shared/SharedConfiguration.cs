using Newtonsoft.Json;
using NL.Serialization;

namespace NL.Production.Shared {

    public class SharedConfiguration {
        [JsonIgnore]
        private string SharedSettingsFilePath => SharedFolder.SharedSettingsFilePath;

        #region Settings
        [JsonProperty("Username")]
        public string Username { get; set; } = "Guest";
        #endregion

        public void Save() {
            Json.Serialize(this, SharedSettingsFilePath, 10000);
        }
    }

}
