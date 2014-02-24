using System.Reflection;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.EntityFramework.Internal
{
    public class DataContextFactory : IDataContextFactory
    {
        private readonly string _connectionString;
        private readonly Assembly _assemblyWithMappings;
        private readonly bool _enableLazyLoading;

        public DataContextFactory(string connectionString, Assembly assemblyWithMappings, bool enableLazyLoading)
        {
            Requires.NotNullOrEmpty(connectionString, "connectionString");
            Requires.NotNull(assemblyWithMappings, "assemblyWithMappings");

            _connectionString = connectionString;
            _assemblyWithMappings = assemblyWithMappings;
            _enableLazyLoading = enableLazyLoading;
        }

        public DataContextFactory(Assembly assemblyWithMappings, bool enableLazyLoading)
        {
            Requires.NotNull(assemblyWithMappings, "assemblyWithMappings");

            _assemblyWithMappings = assemblyWithMappings;
            _enableLazyLoading = enableLazyLoading;
        }

        public DataContext CreateDataContext()
        {
            if (!string.IsNullOrEmpty(_connectionString))
              return new DataContext(_connectionString, _assemblyWithMappings, _enableLazyLoading);
            
            return new DataContext(_assemblyWithMappings, _enableLazyLoading);
        }        
    }
}
