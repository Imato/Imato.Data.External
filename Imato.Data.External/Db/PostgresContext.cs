namespace Imato.Data.External.Db
{
    internal class PostgresContext : DbContext
    {
        public PostgresContext(string? tableName = null, string? connectionString = null) : base(tableName, connectionString)
        {
        }

        public override Task SaveAsync<T>(IEnumerable<T> data)
        {
            throw new NotImplementedException();
        }
    }
}