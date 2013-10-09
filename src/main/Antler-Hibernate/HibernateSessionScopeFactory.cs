using SmartElk.Antler.Domain;

namespace Antler.Hibernate
{
    public class HibernateSessionScopeFactory: ISessionScopeFactory
    {
        public ISessionScope Open()
        {            
            return new HibernateSessionScope();
        }
    }
}
