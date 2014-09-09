using System;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using SmartElk.Antler.Core.Common;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.EntityFramework.Internal
{
    public class DataContextFactory : IDataContextFactory
    {
        private readonly Option<string> _connectionString;
        private readonly Assembly _assemblyWithMappings;
        private readonly Action<DbContextConfiguration> _applyOnConfiguration;

        public DataContextFactory(Option<string> connectionString, Assembly assemblyWithMappings, Action<DbContextConfiguration> applyOnConfiguration)
        {
            Requires.NotNull(connectionString, "connectionString");
            Requires.NotNull(assemblyWithMappings, "assemblyWithMappings");
            Requires.NotNull(applyOnConfiguration, "applyOnConfiguration");

            _connectionString = connectionString;
            _assemblyWithMappings = assemblyWithMappings;
            _applyOnConfiguration = applyOnConfiguration;
        }

        public DataContextFactory(Assembly assemblyWithMappings, Action<DbContextConfiguration> applyOnConfiguration)
            : this(Option<string>.None, assemblyWithMappings, applyOnConfiguration)
        {            
        }

        public DataContext CreateDataContext()
        {
            if (_connectionString.IsSome)
            {
                Assumes.True(!string.IsNullOrEmpty(_connectionString.Value), "Can't proceed without Connection String");
                return new DataContext(_connectionString.Value, _assemblyWithMappings, _applyOnConfiguration);
            }
                
            return new DataContext(_assemblyWithMappings, _applyOnConfiguration);
        }        
    }
}
