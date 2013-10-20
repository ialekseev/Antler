using NHibernate;

namespace Antler.Hibernate
{
    public interface ISessionScopeFactoryEx
    {
        void SetSession(ISession session);
        void ResetSession();
    }
}
