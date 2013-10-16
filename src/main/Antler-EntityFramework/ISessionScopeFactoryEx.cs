using SmartElk.Antler.EntityFramework.Internal;

namespace SmartElk.Antler.EntityFramework
{
    public interface ISessionScopeFactoryEx
    {
        IDataContext CreateDataContext();        
    }
}
