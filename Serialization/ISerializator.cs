using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.Serialization {
	public interface ISerializator {

		static abstract void Serialize(object obj, string filepath);
		static abstract T Deserialize<T>(string filepath);
		static abstract bool TryDeserialize<T>(string filepath, out T obj);

	}
}
