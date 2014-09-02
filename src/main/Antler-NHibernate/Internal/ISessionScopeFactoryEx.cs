using NHibernate;

namespace SmartElk.Antler.NHibernate.Internal
{
    public interface ISessionScopeFactoryEx
    {
        void SetSession(ISession session);
        void ResetSession();
    }
}
