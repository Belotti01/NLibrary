using Newtonsoft.Json;
using NL.Exceptions;
using System.IO;

namespace NL.Serialization {

    /// <summary>
    ///		JSON Serialization and Deserialization methods.
    /// </summary>
    public static class Json {

        /// <summary>
        ///		Write an <see langword="object"/> to a .json file, or overwrite it if
        ///		it already exists, if the file is accessible within <paramref name="timeout"/>ms.
        /// </summary>
        /// <param name="obj">
        ///		The <see langword="object"/> to serialize.
        /// </param>
        /// <param name="filepath">
        ///		The absolute or relative path and filename of the .json file.
        /// </param>
        /// <param name="timeout">
        ///		The maximum time (in milliseconds) it can take to serialize the
        ///		<see langword="object"/>.
        /// </param>
        /// <exception cref="FileAccessTimeoutException"/>
        public static void Serialize(object obj, string filepath, int timeout = TextFile.BASE_TIMEOUT) {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            TextFile.Write(filepath, json, timeout);
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
        /// <inheritdoc cref="Serialize(object, string, int)"/>
        public static T Deserialize<T>(string filepath, int timeout = TextFile.BASE_TIMEOUT) {
            if (!File.Exists(filepath))
                return default;
            string json = TextFile.Read(filepath, timeout);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <returns>
        ///		<see langword="true"/> if the file exists, <see langword="false"/>
        ///		otherwise.
        /// </returns>
        /// <inheritdoc cref="Deserialize{T}(string, int)"/>
        public static bool TryDeserialize<T>(string filepath, out T obj) {
            if (!File.Exists(filepath)) {
                obj = default;
                return false;
            }
            string json = TextFile.Read(filepath);
            obj = JsonConvert.DeserializeObject<T>(json);
            return true;
        }

    }
}
