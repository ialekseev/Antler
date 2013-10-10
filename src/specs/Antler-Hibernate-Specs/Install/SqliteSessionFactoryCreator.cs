using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;

namespace SmartElk.Antler.Hibernate.Specs.Install
{
    public class SqliteSessionFactoryCreator
    {       
        public static Configuration Configuration { get; set; }

        public static ISessionFactory CreateFactory() //todo: move into configuration
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration
                            .Standard.InMemory
                )
                .Mappings(x => x.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly())).
                ExposeConfiguration(x =>
                {
                    Configuration = x;
                })
                .BuildSessionFactory();
        }        
    }
}
