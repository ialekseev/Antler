using System.Reflection;

namespace SmartElk.Antler.EntityFramework.Internal
{
    public class DataContextFactory : IDataContextFactory
    {
        private readonly string _connectionString;
        private readonly Assembly _assemblyWithMappings;

        public DataContextFactory(string connectionString, Assembly assemblyWithMappings)
        {
            _connectionString = connectionString;
            _assemblyWithMappings = assemblyWithMappings;
        }
        
        public DataContextFactory(Assembly assemblyWithMappings)
        {
            _assemblyWithMappings = assemblyWithMappings;
        }

        public IDataContext CreateDbContext()
        {
            if (!string.IsNullOrEmpty(_connectionString))
              return new DataContext(_connectionString, _assemblyWithMappings);
            
            return new DataContext(_assemblyWithMappings);
        }        
    }
}
