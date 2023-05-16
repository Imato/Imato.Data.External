using Imato.Data.External.Db;
using System.Data;

namespace Imato.Data.External
{
    public abstract class DbContext : IDbContext
    {
        protected string? connectionString, tableName;

        public DbContext(string? tableName = null, string? connectionString = null)
        {
            this.tableName = tableName;
            this.connectionString = connectionString;
        }

        public abstract Task SaveAsync<T>(IEnumerable<T> data);

        public static IDbContext Create(string? tableName = null, string? connectionString = null)
        {
            if (tableName == null && connectionString == null)
            {
                return new EmptyContext();
            }

            if (connectionString != null
                && connectionString.Contains("Host")
                && tableName != null)
            {
                return new PostgresContext(tableName, connectionString);
            }

            if (tableName != null)
            {
                return new MsSqlContext(tableName, connectionString);
            }

            throw new NotImplementedException($"Cannot create db context with parameters TableName={tableName} ConnectionString={connectionString}");
        }

        protected virtual IDbConnection GetConnection()
        {
            throw new NotImplementedException();
        }
    }
}