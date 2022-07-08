using Newtonsoft.Json;
using NLCommon.Serialization;

namespace NLCommon.Production.Shared {

	public class SharedConfiguration {
		[JsonIgnore]
		private string SharedSettingsFilePath => SharedFolder.SharedSettingsFilePath;

		#region Settings
		[JsonProperty("Username")]
		public string Username { get; set; } = "Guest";
		#endregion

		/// <summary>
		///     Overwrite the shared settings with the last 
		///     assigned values.
		/// </summary>
		public void Save() {
			JsonSerializator.Serialize(this, SharedSettingsFilePath);
		}

		/// <summary>
		///     Read the latest saved settings, overwriting 
		///     all recent changes.
		/// </summary>
		public void DiscardChanges() {
			SharedFolder.ReadConfiguration();
		}
	}

}
