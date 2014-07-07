using LinqToDB.Data;
using SmartElk.Antler.Core.Domain;

namespace SmartElk.Antler.Linq2Db
{
    public class Linq2DbSessionScopeFactory: ISessionScopeFactory
    {
        private readonly string _databaseName;
        public Linq2DbSessionScopeFactory(string databaseName)
        {
            _databaseName = databaseName;
        }

        public ISessionScope Open()
        {
            return new Linq2DbSessionScope(new DataConnection(_databaseName));
        }                   
    }
}
