using NHibernate;

namespace SmartElk.Antler.NHibernate
{
    public interface ISessionScopeFactoryEx
    {
        void SetSession(ISession session);
        void ResetSession();
    }
}
