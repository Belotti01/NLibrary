using MongoDB.Driver;
using System;
using System.Collections.Generic;
using NL.Exceptions;

namespace NL.Database.MongoDB {
    
    public sealed class MongoDatabase {
        private readonly Dictionary<Type, string> _collections = new();
        private readonly MongoClient _client;
        private static IMongoDatabase _db;
        private readonly string _databaseName;

        public MongoDatabase(string databaseName, string connectionUrl) {
            _databaseName = databaseName;
            _collections = new();

            // Establish connection
            _client = new(connectionUrl);
            _client.StartSession();
            _db = _client.GetDatabase(_databaseName);
        }

        public MongoDatabase(string databaseName, MongoClientSettings clientSettings) {
            _databaseName = databaseName;
            _collections = new();

            // Establish connection
            _client = new(clientSettings);
            _client.StartSession();
            _db = _client.GetDatabase(_databaseName);
        }


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
                typeof(T).GetMethod(nameof(Schema<T>.InitializeCollection)).Invoke(null, new object[] { collectionName });
                _collections.Add(typeof(T), collectionName);
            }
        }
    }

}
