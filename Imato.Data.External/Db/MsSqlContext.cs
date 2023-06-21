using FastMember;
using System.Data;
using System.Data.SqlClient;

namespace Imato.Data.External
{
    public class MsSqlContext : DbContext
    {
        public MsSqlContext(string? tableName = null, string? connectionString = null)
            : base(tableName, connectionString)
        {
            this.connectionString = connectionString
                     ?? "Data Source=localhost; Initial Catalog=master; Trusted_Connection=True;";
        }

        protected override IDbConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        private SqlBulkCopy CreateBulkCopy(IEnumerable<string> fields)
        {
            var bulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.Default);
            bulkCopy.BatchSize = 1_000;
            bulkCopy.BulkCopyTimeout = 60_000;
            bulkCopy.DestinationTableName = tableName;
            foreach (var field in fields)
            {
                bulkCopy.ColumnMappings.Add(field, field);
            }
            return bulkCopy;
        }

        private SqlBulkCopy CreateBulkCopy<T>(IEnumerable<string>? fields = null)
        {
            var properties = typeof(T).GetProperties().Select(x => x.Name);
            if (fields != null && fields.Any())
            {
                var notExists = fields.Where(x => !properties.Contains(x));
                if (notExists.Any())
                {
                    throw new ArgumentOutOfRangeException($"Not exists properties {string.Join(",", notExists)} of type {typeof(T).Name}");
                }
            }
            fields ??= properties;
            return CreateBulkCopy(fields);
        }

        public override async Task SaveAsync<T>(
            IEnumerable<T> data,
            string? columns = null)
        {
            if (tableName != null)
            {
                ConsoleOutput.LogInformation($"Save data to table {tableName}");
                ConsoleOutput.LogDebug(data);

                var columnsList = columns?.Split(";") ?? Array.Empty<string>();

                var dictionaryList = data as IEnumerable<IDictionary<string, object>>;
                if (dictionaryList != null)
                {
                    await SaveAsync(dictionaryList, columnsList);
                    return;
                }

                using var bc = CreateBulkCopy<T>(columnsList);
                using var reader = ObjectReader.Create(data);
                await bc.WriteToServerAsync(reader);
                return;
            }
        }

        private async Task SaveAsync(
            IEnumerable<IDictionary<string, object>> data,
            string[] columns)
        {
            List<string> fields = new List<string>();
            var dataTable = new DataTable();

            foreach (var d in data)
            {
                if (fields.Count == 0)
                {
                    foreach (var key in d.Keys)
                    {
                        if (!(columns.Length > 0 && !columns.Contains(key)))
                        {
                            dataTable.Columns.Add(key);
                            fields.Add(key);
                        }
                    }

                    ConsoleOutput.LogDebug($"Fields: {string.Join(";", fields)}");
                }

                var row = dataTable.NewRow();
                foreach (var key in d.Keys)
                {
                    if (!(columns.Length > 0 && !columns.Contains(key)))
                    {
                        row[key] = d[key];
                    }
                }
                dataTable.Rows.Add(row);
            }

            using var bc = CreateBulkCopy(fields);
            await bc.WriteToServerAsync(dataTable);
        }
    }
}