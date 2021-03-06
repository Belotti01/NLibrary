namespace NLCommon.Serialization {
	public interface ISerializator {

		static abstract void Serialize(object obj, string filepath);
		static abstract T Deserialize<T>(string filepath);
		static abstract bool TryDeserialize<T>(string filepath, out T obj);

	}
}
