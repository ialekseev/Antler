using System;
using System.Data.Common;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.Core.Domain.Configuration
{
    public abstract class AbstractRelationalStorage<TStorage> : AbstractStorage where TStorage: class
    {
        protected Action CommandToTryToApplyOnServer { get; set; }

        protected AbstractRelationalStorage()
        {
            CommandToTryToApplyOnServer = () => { };
        }
        
        public override void Configure(IDomainConfigurator configurator)
        {
            Requires.NotNull(configurator, "configurator");

            CommandToTryToApplyOnServer();

            base.Configure(configurator);
        }
                        
        public TStorage WithCommandToTryToApplyOnServer(DbProviderFactory providerFactory, string connectionString,
                                                 string commandText)
        {
            Requires.NotNull(providerFactory, "providerFactory");
            Requires.NotNullOrEmpty(connectionString, "connectionString");
            Requires.NotNullOrEmpty(commandText, "commandText");

            CommandToTryToApplyOnServer = () =>
            {
                try
                {
                    using (var sqlConn = providerFactory.CreateConnection())
                    {
                        sqlConn.ConnectionString = connectionString;

                        sqlConn.Open();

                        using (var cmd = sqlConn.CreateCommand())
                        {
                            cmd.CommandText = commandText;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (DbException e)
                {
                }
            };

            return this as TStorage;
        }  
    }
}
