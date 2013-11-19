using NHibernate;

namespace Antler.NHibernate
{
    public interface ISessionScopeFactoryEx
    {
        void SetSession(ISession session);
        void ResetSession();
    }
}
