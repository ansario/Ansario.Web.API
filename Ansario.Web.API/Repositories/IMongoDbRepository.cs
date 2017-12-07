using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ansario.Web.API.Repositories
{
    public interface IMongoDbRepository
    {
        Task<T> GetOne<T>(string database, string collection, FilterDefinition<T> expression);
        Task<List<T>> Get<T>(string database, string collection, FilterDefinition<T> expression);
        Task<List<T>> GetAll<T>(string database, string collection);
        Task Post<T>(string database, string collection, T document);
        Task Upsert<T>(string database, string collection, Expression<Func<T, bool>> expression, T document);
        Task<string> CreateIndex<T>(string database, string collection, IndexKeysDefinition<T> definition);
    }
}
