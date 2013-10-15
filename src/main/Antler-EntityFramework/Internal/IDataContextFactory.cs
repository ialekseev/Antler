namespace SmartElk.Antler.EntityFramework.Internal
{
    public interface IDataContextFactory
    {
        IDataContext CreateDbContext();
    }
}
