using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Bson;

namespace NL.Database.MongoDB {

    public abstract class ReadOnlySchema<T> where T : ReadOnlySchema<T> {
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
        public static IAsyncCursor<T> GetCursor(Expression<Func<T, bool>> lambda) {
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
        public static List<T> GetAll(Expression<Func<T, bool>> lambda)
            => GetCursor(lambda).ToList();

        /// <summary>
        /// Get a List of all T objects in the Collection. 
        /// NOTE: Only use if getting all documents is strictly necessary to avoid traffic.
        /// </summary>
        public static List<T> GetAll()
            => GetCursor(x => true).ToList();

        /// <summary>
        /// Get the first document that returns true when the <paramref name="lambda"/> filter
        /// is applied.
        /// </summary>
        /// <returns>The first document found as an object of type <typeparamref name="T"/>, or
        /// <see langword="default"/> if no result is found.</returns>
        /// <inheritdoc cref="GetCursor(Expression{Func{T, bool}})"/>
        public static T GetOneOrDefault(Expression<Func<T, bool>> lambda)
            => GetCursor(lambda).FirstOrDefault();

        /// <summary>
        /// Checks whether a document fitting the <paramref name="lambda"/> filter requirements
        /// exists in the collection.
        /// </summary>
        /// <returns><see langword="true"/> if a fitting document is found, <see langword="false"/> otherwise.</returns>
        /// <inheritdoc cref="GetCursor(Expression{Func{T, bool}})"/>
        public static bool Exists(Expression<Func<T, bool>> lambda)
            => GetOneOrDefault(lambda) is not null;
    }

}
