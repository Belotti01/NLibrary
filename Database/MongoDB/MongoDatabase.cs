using MongoDB.Driver;
using System;
using System.Collections.Generic;
using NL.Exceptions;

namespace NL.Database.MongoDB {
    
    /// <summary>
    ///     Access point object to a MongoDB database.
    /// </summary>
    public sealed class MongoDatabase : IDisposable, ICloneable {
        private Dictionary<Type, string> _collections = new();
        private MongoClient _client;
        private static IMongoDatabase _db;
        private string _databaseName;

        /// <summary>
        ///     Instance a connection to the MongoDB database.
        /// </summary>
        /// <param name="databaseName">
        ///     The name of the database to connect to.
        /// </param>
        /// <param name="connectionUrl">
        ///     The link used to establish the connection to the database.
        /// </param>
        public MongoDatabase(string databaseName, string connectionUrl) {
            _databaseName = databaseName;
            _collections = new();

            // Establish connection
            _client = new(connectionUrl);
            _client.StartSession();
            _db = _client.GetDatabase(_databaseName);
        }

        /// <inheritdoc cref="MongoDatabase(string, string)"/>
        /// <param name="clientSettings">
        ///     The settings used to establish the connection to the database.
        /// </param>
        public MongoDatabase(string databaseName, MongoClientSettings clientSettings) {
            _databaseName = databaseName;
            _collections = new();

            // Establish connection
            _client = new(clientSettings);
            _client.StartSession();
            _db = _client.GetDatabase(_databaseName);
        }

        /// <summary>
        ///     Register a <see cref="Schema{T}"/> and link it to the
        ///     relative <see cref="IMongoCollection{TDocument}"/>.
        /// </summary>
        /// <typeparam name="T">
        ///     The <see cref="Schema{T}"/> to link to the collection.
        /// </typeparam>
        /// <param name="collectionName">
        ///     The name of the collection to link.
        /// </param>
        /// <param name="createIfMissing">
        ///     When <see langword="true"/>, if the specified collection is not found,
        ///     a new one is created called <paramref name="collectionName"/>.
        /// </param>
        /// <exception cref="InvalidCollectionException"/>
        /// <exception cref="DuplicateItemException"/>
        public void AddCollectionSchema<T>(string collectionName, bool createIfMissing = false) where T : Schema<T> {
            IMongoCollection<T> collection = _db.GetCollection<T>(collectionName);
            if (collection is null) {
                if(createIfMissing) {
                    _db.CreateCollection(collectionName);
                }else {
                    throw new InvalidCollectionException(collectionName);
                }
            }

            if(_collections.ContainsKey(typeof(T))) {
                throw new DuplicateItemException(nameof(collectionName), collectionName);
            }else {
                Schema<T>.InitializeCollection(collection);
                _collections.Add(typeof(T), collectionName);
            }
        }

        public void Dispose() {
            _collections.Clear();
            _collections = default;
            _client = default;
            _db = default;
            _databaseName = default;
        }

        /// <inheritdoc cref="ICloneable.Clone"/>
        public object Clone() {
            MongoDatabase copy = new(_databaseName, _client.Settings);
            copy._collections = _collections;
            return copy;
        }

        /// <summary>
        ///     Strongly-typed versione of the <see cref="Clone"/> method.
        /// </summary>
        /// <returns>
        ///     An exact copy of this <see cref="MongoDatabase"/>.
        /// </returns>
        public MongoDatabase Copy()
            => (MongoDatabase)Clone();
    }

}
