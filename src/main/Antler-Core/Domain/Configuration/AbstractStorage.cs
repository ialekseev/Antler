using System;
using System.ComponentModel;
using System.Data.Common;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.Core.Domain.Configuration
{
    public abstract class AbstractStorage<TStorage> : IStorage where TStorage : class
    {
        protected Action CommandToTryToApplyOnServer { get; set; }

        protected AbstractStorage()
        {
            CommandToTryToApplyOnServer = () => { };
        }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        public abstract void Configure(IDomainConfigurator configurator);

        //todo: check if it works for Specs & Sample projects
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
