using Newtonsoft.Json;
using NLCommon.Exceptions;
using System.IO;

namespace NLCommon.Serialization {

	/// <summary>
	///		JSON Serialization and Deserialization methods.
	/// </summary>
	public class JsonSerializator : ISerializator {

		/// <summary>
		///		Write an <see langword="object"/> to a .json file, or overwrite it if
		///		it already exists, if the file is accessible.
		/// </summary>
		/// <param name="obj">
		///		The <see langword="object"/> to serialize.
		/// </param>
		/// <param name="filepath">
		///		The absolute or relative path and filename of the .json file.
		/// </param>
		/// <exception cref="FileAccessTimeoutException"/>
		public static void Serialize(object obj, string filepath) {
			string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
			TextFileSerializator.Write(filepath, json);
		}

		/// <summary>
		///		Read a previously serialized <see langword="object"/> of type 
		///		<typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">
		///		The type of the <see langword="object"/> to return.
		///	</typeparam>
		/// <returns>
		///		The <see langword="object"/> of type <typeparamref name="T"/> retrieved
		///		from the json file if it exists, <see langword="default"/> otherwise.
		/// </returns>
		/// <inheritdoc cref="Serialize(object, string)"/>
		public static T Deserialize<T>(string filepath) {
			if(!File.Exists(filepath))
				return default;
			string json = TextFileSerializator.Read(filepath);
			return JsonConvert.DeserializeObject<T>(json);
		}

		/// <returns>
		///		<see langword="true"/> if the file exists, <see langword="false"/>
		///		otherwise.
		/// </returns>
		/// <inheritdoc cref="Deserialize{T}(string)"/>
		public static bool TryDeserialize<T>(string filepath, out T obj) {
			if(!File.Exists(filepath)) {
				obj = default;
				return false;
			}
			string json = TextFileSerializator.Read(filepath);
			obj = JsonConvert.DeserializeObject<T>(json);
			return true;
		}
	}
}
