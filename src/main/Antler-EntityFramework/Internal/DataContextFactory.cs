using System.Reflection;

namespace SmartElk.Antler.EntityFramework.Internal
{
    public class DataContextFactory : IDataContextFactory
    {
        private readonly string _connectionString;
        private readonly Assembly _assemblyWithMappings;
        private readonly bool _enableLazyLoading;

        public DataContextFactory(string connectionString, Assembly assemblyWithMappings, bool enableLazyLoading)
        {
            _connectionString = connectionString;
            _assemblyWithMappings = assemblyWithMappings;
            _enableLazyLoading = enableLazyLoading;
        }

        public DataContextFactory(Assembly assemblyWithMappings, bool enableLazyLoading)
        {
            _assemblyWithMappings = assemblyWithMappings;
            _enableLazyLoading = enableLazyLoading;
        }

        public IDataContext CreateDbContext()
        {
            if (!string.IsNullOrEmpty(_connectionString))
              return new DataContext(_connectionString, _assemblyWithMappings, _enableLazyLoading);
            
            return new DataContext(_assemblyWithMappings, _enableLazyLoading);
        }        
    }
}
