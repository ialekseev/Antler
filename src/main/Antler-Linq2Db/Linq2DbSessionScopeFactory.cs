using LinqToDB.DataProvider.SqlServer;
using SmartElk.Antler.Core.Domain;

namespace SmartElk.Antler.Linq2Db
{
    public class Linq2DbSessionScopeFactory: ISessionScopeFactory
    {
        private readonly string _connectionString;
        public Linq2DbSessionScopeFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ISessionScope Open()
        {
            return new Linq2DbSessionScope(SqlServerTools.CreateDataConnection(_connectionString));
        }                   
    }
}
