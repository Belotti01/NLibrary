using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace NLCommon.Database.MongoDB {

	/// <summary>
	///     Access point to a MongoDB schema which allows for read-write operations.
	/// </summary>
	public abstract class Schema<T> : ReadOnlySchema<T> where T : Schema<T> {

		/// <summary>
		/// The MongoDB collection containing the documents for the type <typeparamref name="T"/>.
		/// </summary>
		[BsonIgnore, NotNull]
		protected static IMongoCollection<T>? _collection;

		/// <summary>
		/// Attempts to create a new document in the Database.
		/// </summary>
		/// <returns><see langword="true"/> if the document is successfully created, <see langword="false"/> otherwise.</returns>
		public bool TryUpload() {
			try {
				_collection.InsertOne((T)this);
				return true;
			} catch {
				return false;
			}
		}

		/// <returns></returns>
		/// <inheritdoc cref="TryUpload()"/>
		public void Upload() {
			_collection.InsertOne((T)this);
		}

		/// <summary>
		/// Replace the value in the DB of the <paramref name="nameofProperty"/> with 
		/// <paramref name="newValue"/>.
		/// </summary>
		/// <typeparam name="E"></typeparam>
		/// <param name="nameofProperty">Simply use <c>nameof(property)</c>.</param>
		/// <param name="newValue">The new value of the property of type <typeparamref name="E"/>.</param>
		public bool TryUpdate<E>(string nameofProperty, E newValue) {
			try {
				Update(nameofProperty, newValue);
				return true;
			} catch {
				return false;
			}
		}

		/// <returns></returns>
		/// <inheritdoc cref="TryUpdate{E}(string, E)"/>
		public void Update<E>(string nameofProperty, E newValue) {
			PropertyInfo memberData = typeof(T).GetProperty(nameofProperty);
			var attribute = (BsonElementAttribute)memberData.GetCustomAttributes(true)
				.Where(attr => attr is BsonElementAttribute)
				.FirstOrDefault();

			if(attribute == default)
				throw new Exception($"The property {nameofProperty} isn't associated with any BsonElementAttribute.");

			_collection.UpdateOne(
				Builders<T>.Filter.Eq(doc => doc.Id, Id),
				Builders<T>.Update.Set(attribute.ElementName, newValue)
			);
		}

		/// <summary>
		///     Update all properties of this <see cref="Schema{T}"/> object on 
		///     the database.
		/// </summary>
		public void UpdateAll() {
			IEnumerable<PropertyInfo> memberData = typeof(T).GetProperties()
				.Where(p => p.CustomAttributes
					.Any(a => a.AttributeType == typeof(BsonElementAttribute))
				);
			// To minimize traffic, get the last DB state of this object and
			// only send update queries for properties with different values
			Schema<T> dbValues = GetOneOrDefault(x => x.Id == Id);
			IEnumerable<PropertyInfo> toUpdate = memberData
				.Where(x => x.GetValue(dbValues).Equals(x.GetValue(this)));

			foreach(PropertyInfo property in toUpdate) {
				Update(property.Name, property.GetValue(this));
			}
		}

		/// <summary>
		///     Delete the current document from the database.
		/// </summary>
		public void RemoveDocument() {
			_collection.DeleteOne(doc => doc.Id == Id);
		}
	}

}
