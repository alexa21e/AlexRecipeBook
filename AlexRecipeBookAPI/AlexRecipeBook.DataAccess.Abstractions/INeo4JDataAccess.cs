using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexRecipeBook.DataAccess.Abstractions
{
    public interface INeo4JDataAccess : IAsyncDisposable
    {
        Task<List<string>> ExecuteReadListAsync(string query, string returnObjectKey, IDictionary<string, object>? parameters = null);
        Task<List<Dictionary<string, object>>> ExecuteReadDictionaryAsync(string query,
            string returnObjectKey, IDictionary<string, object>? parameters = null);
        Task<T> ExecuteReadScalarAsync<T>(string query, IDictionary<string, object>? parameters = null);
        Task<List<Dictionary<string, object>>> ExecuteReadPropertiesAsync(string query,
            IDictionary<string, object>? parameters);
        Task<Dictionary<string, object>> ExecuteReadSingleRecordAsync(string query,
            IDictionary<string, object>? parameters);
        Task<T> ExecuteWriteTransactionAsync<T>(string query, IDictionary<string, object>? parameters = null);

    }
}
