using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Runtime.InteropServices;
using System.Reflection;

namespace NL.Database.MongoDB {

    public abstract class Schema<T> where T : Schema<T> {
        /// <summary>
        ///     This document's unique ID.
        /// </summary>
        [BsonId]
        public ObjectId Id { get; protected set; }

        /// <summary>
        /// The MongoDB collection containing the documents for the type <typeparamref name="T"/>.
        /// </summary>
        [BsonIgnore]
        private static IMongoCollection<T> _collection;

        internal static void InitializeCollection(IMongoCollection<T> collection) {
            _collection = collection;
        }

        /// <summary>
        /// Get an asynchronous iterator for all documents retrieved with the <paramref name="lambda"/>
        /// filter.
        /// </summary>
        /// <param name="lambda">Documents will only be retrieved if this expression returns <see langword="true"></see></param>
        /// <returns>An <see cref="IAsyncCursor{TDocument}"/> that asynchronously iterates through the retrievable documents.</returns>
        protected static IAsyncCursor<T> GetCursor(Expression<Func<T, bool>> lambda) {
            return _collection.FindSync<T>(
                new FilterDefinitionBuilder<T>()
                    .Where(lambda)
            );
        }

        /// <summary>
        /// Get a <see cref="List{T}"/> of objects retrieved with the
        /// <paramref name="lambda"/> filter. Note: Do NOT use this to fetch a single document,
        /// as it can create a lot of useless traffic; use <see cref="GetOneOrDefault(Expression{Func{T, bool}})"/>
        /// instead.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> containing all the retrieved documents.</returns>
        /// <inheritdoc cref="GetCursor(Expression{Func{T, bool}})"/>
        protected static List<T> GetAll(Expression<Func<T, bool>> lambda)
            => GetCursor(lambda).ToList();

        /// <summary>
        /// Get a List of all T objects in the Collection. 
        /// NOTE: Only use if getting all documents is strictly necessary to avoid traffic.
        /// </summary>
        protected static List<T> GetAll()
            => GetCursor(x => true).ToList();

        /// <summary>
        /// Get the first document that returns true when the <paramref name="lambda"/> filter
        /// is applied.
        /// </summary>
        /// <returns>The first document found as an object of type <typeparamref name="T"/>, or
        /// <see langword="default"/> if no result is found.</returns>
        /// <inheritdoc cref="GetCursor(Expression{Func{T, bool}})"/>
        protected static T GetOneOrDefault(Expression<Func<T, bool>> lambda)
            => GetCursor(lambda).FirstOrDefault();

        /// <summary>
        /// Checks whether a document fitting the <paramref name="lambda"/> filter requirements
        /// exists in the collection.
        /// </summary>
        /// <returns><see langword="true"/> if a fitting document is found, <see langword="false"/> otherwise.</returns>
        /// <inheritdoc cref="GetCursor(Expression{Func{T, bool}})"/>
        protected static bool Exists(Expression<Func<T, bool>> lambda)
            => GetOneOrDefault(lambda) is not null;

        /// <summary>
        /// Attempts to create a new document in the Database.
        /// </summary>
        /// <param name="exception">The thrown <see cref="Exception"/> in case of failure, 
        /// <see langword="null"/> otherwise.</param>
        /// <returns><see langword="true"/> if the document is successfully created, <see langword="false"/> otherwise.</returns>
        public bool TryUpload([Optional] out Exception exception) {
            try {
                _collection.InsertOne((T)this);
                exception = null;
                return true;
            }catch(Exception e) {
                exception = e;
                return false;
            }
        }

        /// <returns></returns>
        /// <inheritdoc cref="TryUpload(out Exception)"/>
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
        protected bool TryUpdate<E>(string nameofProperty, E newValue) {
            try {
                Update(nameofProperty, newValue);
                return true;
            }catch {
                return false;
            }
        }

        /// <returns></returns>
        /// <inheritdoc cref="TryUpdate{E}(string, E)"/>
        protected void Update<E>(string nameofProperty, E newValue) {
            PropertyInfo memberData = typeof(T).GetProperty(nameofProperty);
            var attribute = (BsonElementAttribute)memberData.GetCustomAttributes(true)
                .Where(attr => attr is BsonElementAttribute)
                .FirstOrDefault();

            if (attribute == default)
                throw new Exception($"The property {nameofProperty} isn't associated with any BsonElementAttribute.");
            
            _collection.UpdateOne(
                Builders<T>.Filter.Eq(doc => doc.Id, Id),
                Builders<T>.Update.Set(attribute.ElementName, newValue)
            );
        }

        /// <summary>
        ///     Delete the current document from the database.
        /// </summary>
        public void RemoveDocument() {
            _collection.DeleteOne<T>(doc => doc.Id == Id);
        }
    }

}
