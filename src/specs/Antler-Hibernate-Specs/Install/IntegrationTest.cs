using NHibernate;
using NHibernate.Tool.hbm2ddl;
using SmartElk.Antler.Domain;

namespace SmartElk.Antler.Hibernate.Specs.Install
{
    public class IntegrationTest
    {
        public IntegrationTest()
        {
            Install.RegisterComponents();

            //todo: move into configuration
            UnitOfWork.DoAfterSessionOpening =
                sessionScope =>
                new SchemaExport(SqliteSessionFactoryCreator.Configuration).Execute(false, true, false,
                                                                                    ((ISession)
                                                                                     (sessionScope.InternalSession))
                                                                                        .Connection, null);
        }
    }
}
