namespace Imato.Data.External
{
    public class DataProcess<T> : BaseProcess
    {
        private readonly IDbContext _dbContext;

        public DataProcess(string[] args) : base(args)
        {
            _dbContext = DbContext.Create(
                GetParameter("Table"),
                GetParameter("ConnectionString"));
        }

        public override async Task RunAsync()
        {
            try
            {
                ConsoleOutput.LogDebug("Get data");
                var data = (await DataAsync())
                    .Union(Data());
                ConsoleOutput.LogDebug("Save data");
                var columns = GetParameter("Columns");
                await _dbContext.SaveAsync(data, columns);
            }
            catch (Exception ex)
            {
                Console.Error.Write(ex.ToString());
            }

            ConsoleOutput.LogDebug("Done");
        }

        protected override void PrintHelp()
        {
            base.PrintHelp();
            Console.WriteLine("DataProcess");
            Console.WriteLine("Destination input table:");
            Console.WriteLine("\tConnectionString=, Table=");
        }

        /// <summary>
        /// Override and create data output
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected virtual Task<IEnumerable<T>> DataAsync()
        {
            return Task.FromResult(Enumerable.Empty<T>());
        }

        /// <summary>
        /// Override and create data output
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected virtual IEnumerable<T> Data()
        {
            return Enumerable.Empty<T>();
        }
    }

    public class DataProcess : DataProcess<IDictionary<string, object>>
    {
        public DataProcess(string[] args) : base(args)
        {
        }
    }
}