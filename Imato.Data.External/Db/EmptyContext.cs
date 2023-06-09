﻿namespace Imato.Data.External.Db
{
    public class EmptyContext : DbContext
    {
        public override Task SaveAsync<T>(
            IEnumerable<T> data,
            string? columns = null)
        {
            if (data == null || !data.Any())
                return Task.CompletedTask;

            ConsoleOutput.WriteCsv(data);
            return Task.CompletedTask;
        }
    }
}