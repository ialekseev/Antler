using SmartElk.Antler.EntityFramework.Configuration;

namespace SmartElk.Antler.EntityFramework.SqlServer.Configuration
{
    public class EntityFrameworkPlusSqlServer : EntityFrameworkStorage
    {
        public static EntityFrameworkPlusSqlServer Use
        {
            get { return new EntityFrameworkPlusSqlServer(); }            
        }        
    }
}
