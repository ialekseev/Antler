using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;

namespace SmartElk.Antler.Linq2Db.Configuration
{
    public class Linq2DbStorage : AbstractStorage<Linq2DbStorage>
    {
        private readonly string _connectionString;

        private Linq2DbStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static Linq2DbStorage Use(string connectionString)
        {
            Requires.NotNullOrEmpty(connectionString, "connectionString");
            return new Linq2DbStorage(connectionString);
        }

        public override void Configure(IDomainConfigurator configurator)
        {
            Requires.NotNull(configurator, "configurator");

            CommandToTryToApplyOnServer(); //todo: move this call to the base class(for Linq2Db, NHibernate and EntityFramework adapters)

            var sessionScopeFactory = new Linq2DbSessionScopeFactory(_connectionString);
            configurator.Configuration.Container.PutWithNameOrDefault<ISessionScopeFactory>(sessionScopeFactory, configurator.Name);
        }
    }
}
