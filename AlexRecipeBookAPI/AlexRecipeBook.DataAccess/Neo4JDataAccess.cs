using Microsoft.Extensions.Options;
using Neo4j.Driver;
using AlexRecipeBook.DataAccess.Abstractions;
using Newtonsoft.Json;

namespace AlexRecipeBook.DataAccess
{
    public class Neo4JDataAccess : INeo4JDataAccess
    {
        private readonly IAsyncSession _session;

        private readonly string _database;

        public Neo4JDataAccess(IDriver driver, IOptions<ApplicationSettings> appSettingsOptions)
        {
            _database = appSettingsOptions.Value.Neo4jDatabase ?? "neo4j";
            _session = driver.AsyncSession(o => o.WithDatabase(_database));
        }

        public async Task<List<string>> ExecuteReadListAsync(string query, string returnObjectKey, IDictionary<string, object>? parameters = null)
        {
            return await ExecuteReadTransactionAsync<string>(query, returnObjectKey, parameters);
        }

        public async Task<List<Dictionary<string, object>>> ExecuteReadDictionaryAsync(string query, string returnObjectKey, IDictionary<string, object>? parameters = null)
        {
            return await ExecuteReadTransactionAsync<Dictionary<string, object>>(query, returnObjectKey, parameters);
        }

        public async Task<T> ExecuteReadScalarAsync<T>(string query, IDictionary<string, object>? parameters = null)
        {
            try
            {
                parameters ??= new Dictionary<string, object>();

                var result = await _session.ExecuteReadAsync(async tx =>
                {
                    T? scalar = default;
                    var res = await tx.RunAsync(query, parameters);
                    scalar = (await res.SingleAsync())[0].As<T>();
                    return scalar;
                });

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was a problem while executing database query" + ex.Message);
                throw;
            }
        }

        public async Task<List<Dictionary<string, object>>> ExecuteReadPropertiesAsync(string query, IDictionary<string, object>? parameters)
        {
            try
            {
                parameters ??= new Dictionary<string, object>();

                var result = await _session.ExecuteReadAsync(async tx =>
                {
                    var data = new List<Dictionary<string, object>>();
                    var res = await tx.RunAsync(query, parameters);
                    var records = await res.ToListAsync();
                    data = records.Select(x =>
                    {
                        var properties = new Dictionary<string, object>();
                        foreach (var key in x.Keys)
                        {
                            properties[key] = x[key].As<object>();
                        }
                        return properties;
                    }).ToList();
                    return data;
                });

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was a problem while executing database query" + ex.Message);
                throw;
            }
        }

        public async Task<Dictionary<string, object>> ExecuteReadSingleRecordAsync(string query, IDictionary<string, object>? parameters)
        {
            try
            {
                parameters ??= new Dictionary<string, object>();

                var result = await _session.ExecuteReadAsync(async tx =>
                {
                    var res = await tx.RunAsync(query, parameters);
                    var record = await res.SingleAsync();

                    var properties = new Dictionary<string, object>();
                    foreach (var key in record.Keys)
                    {
                        properties[key] = record[key].As<object>();
                    }

                    return properties;
                });

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was a problem while executing database query" + ex.Message);
                throw;
            }
        }

        private async Task<List<T>> ExecuteReadTransactionAsync<T>(string query, string returnObjectKey, IDictionary<string, object>? parameters)
        {
            try
            {
                parameters ??= new Dictionary<string, object>();

                var result = await _session.ExecuteReadAsync(async tx =>
                {
                    var data = new List<T>();
                    var res = await tx.RunAsync(query, parameters);
                    var records = await res.ToListAsync();
                    data = records.Select(x =>
                    {
                        var nodeProps = JsonConvert.SerializeObject(x.Values[returnObjectKey].As<INode>().Properties);
                        return JsonConvert.DeserializeObject<T>(nodeProps);
                    }).ToList();
                    return data;
                });

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was a problem while executing database query" + ex.Message);
                throw;
            }
        }

        public async Task<T> ExecuteWriteTransactionAsync<T>(string query, IDictionary<string, object>? parameters = null)
        {
            try
            {
                parameters ??= new Dictionary<string, object>();

                var result = await _session.ExecuteWriteAsync(async tx =>
                {
                    T? scalar = default;
                    var res = await tx.RunAsync(query, parameters);
                    scalar = (await res.SingleAsync())[0].As<T>();
                    return scalar;
                });

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was a problem while executing database query" + ex.Message);
                throw;
            }
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _session.CloseAsync();
        }
    }
}
