using System;
using System.Data.SqlClient;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.NHibernate.SqlServer.Specs.Configuration
{
    public static class ConfigurationEx
    {
        public static Action TryToCreateDatabaseCommand(this IAntlerConfigurator configurator, string databaseName)
        {
            Requires.NotNullOrEmpty(databaseName, "databaseName");
            
            return () =>
            {
                try
                {
                    using (var connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Integrated Security=True"))
                    {
                        connection.Open();

                        var command = connection.CreateCommand();
                        command.CommandText = string.Format("CREATE DATABASE {0}", databaseName);
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException e)
                {                    
                }
            };
        }        
    }
}
