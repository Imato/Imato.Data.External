namespace Imato.Data.External
{
    public interface IDbContext
    {
        Task SaveAsync<T>(IEnumerable<T> data);
    }
}