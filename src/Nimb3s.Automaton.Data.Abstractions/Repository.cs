using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Data.Abstractions
{
    public static class Extensions
    {
        public static Dictionary<string, object> ToKeyValuePair<T>(this IEntity<Guid> obj)
        {
            Dictionary<string, object> paramCollection = new Dictionary<string, object>();

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                //// Skip reference types (but still include string!)
                //if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                //    continue;

                // Skip methods without a public setter
                if (property.GetSetMethod() == null)
                    continue;

                var name = property.Name;
                var value = typeof(T).GetProperty(property.Name).GetValue(obj, null);

                paramCollection.Add(name, value);

            }

            return paramCollection;
        }

        public static Dictionary<string, object> ToPrimaryKey<T>(this IEntity<Guid> obj, string[] primaryKeyNames)
        {
            Dictionary<string, object> paramCollection = new Dictionary<string, object>();

            var properties = typeof(T).GetProperties();

            foreach (var primaryKeyName in primaryKeyNames)
            {
                if (properties.Any(i => i.Name.Equals(primaryKeyName, StringComparison.OrdinalIgnoreCase)))
                {
                    var value = typeof(T).GetProperty(primaryKeyName).GetValue(obj, null);

                    paramCollection.Add(primaryKeyName, value);
                }
            }

            return null;
        }
    }


    public abstract class Repository<T> : IRepository<T> 
        where T: IEntity<Guid>, IDisposable
    {
        private readonly string[] validEntityKeyNames = { "id" };
        private readonly string entityName = typeof(T).Name.Replace("Entity", string.Empty);
        private readonly IDbConnection dbConnection;

        public virtual string Schema => "dbo";
        public Repository()
        {
            this.dbConnection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Automaton;Integrated Security=true");
        }

        public Repository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async Task<T> GetAsync(int Id)
        {
            using (var entity = await dbConnection.QuerySingleAsync<T>($"{Schema}.p_Get{entityName}"))
            {
                return entity;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var items = (await dbConnection.QueryAsync<T>(sql: $"{Schema}.p_GetAll{entityName}", commandType: CommandType.StoredProcedure)).AsList();

            return items;
        }

        public async Task AddAsync(T entity)
        {
            var @params = entity.ToKeyValuePair<T>();

            DynamicParameters dp = new DynamicParameters();

            foreach (var param in @params)
            {
                dp.Add(param.Key, param.Value);
            }

            await dbConnection.ExecuteAsync(sql: $"{Schema}.p_Insert{entityName}", param: dp, commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(T entity)
        {
            var id = entity.ToPrimaryKey<T>(validEntityKeyNames);

            if(id == null)
            {
                throw new ArgumentNullException("", $"{nameof(T)} does not contain a valid primary key name. List of valid primary key names: {string.Join(",", validEntityKeyNames)}");
            }

            DynamicParameters dp = new DynamicParameters();

            foreach (var param in id)
            {
                dp.Add(id.Keys.First(), id.Values.First());
            }

            await dbConnection.ExecuteAsync(sql: $"{Schema}.p_Delete{entityName}", param: dp, commandType: CommandType.StoredProcedure);
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
