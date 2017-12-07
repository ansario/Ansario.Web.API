using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ansario.Web.API.Repositories
{
    public class MongoDbRepository : IMongoDbRepository
    {
        private static IMongoClient _client;

        public MongoDbRepository()
        {
            _client = new MongoClient(ConfigurationManager.AppSettings["ConnectionString"]);
        }

        public Task<string> CreateIndex<T>(string database, string collection, IndexKeysDefinition<T> definition)
        {
            return _client.GetDatabase(database).GetCollection<T>(collection).Indexes.CreateOneAsync(definition);
        }

        public Task<T> GetOne<T>(string database, string collection, FilterDefinition<T> expression)
        {
            return _client.GetDatabase(database).GetCollection<T>(collection).Find(expression).Limit(1).FirstOrDefaultAsync();
        }

        public Task<List<T>> Get<T>(string database, string collection, FilterDefinition<T> expression)
        {
            return _client.GetDatabase(database).GetCollection<T>(collection).Find(expression).ToListAsync();
        }

        public Task<List<T>> GetAll<T>(string database, string collection)
        {
            return _client.GetDatabase(database).GetCollection<T>(collection).Find(Builders<T>.Filter.Empty).ToListAsync();
        }

        public Task Post<T>(string database, string collection, T document)
        {
            return _client.GetDatabase(database).GetCollection<T>(collection).InsertOneAsync(document);
        }

        public Task Upsert<T>(string database, string collection, Expression<Func<T, bool>> expression, T document)
        {
            return _client.GetDatabase(database).GetCollection<T>(collection).ReplaceOneAsync<T>(expression, document, new UpdateOptions { IsUpsert = true });
        }
    }
}