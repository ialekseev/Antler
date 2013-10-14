namespace SmartElk.Antler.EntityFramework.Internal
{
    public class DataContextFactory : IDataContextFactory
    {
        private readonly IDataContext _context;

        public DataContextFactory()
        {            
            _context = new DataContext();
        }

        public IDataContext GetDbContext()
        {
            return _context;
        }        
    }
}
