using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Antler.NHibernate.Configuration;
using Blog.Web.Common;
using Blog.Web.Common.AppStart;
using FluentNHibernate.Cfg.Db;
using SmartElk.Antler.Core;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.StructureMap;

namespace Blog.Web.NH.SqlServer
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IAntlerConfigurator AntlerConfigurator { get; private set; }

        protected void Application_Start()
        {
            /***You need to create "Antler" database in your SQL SERVER. See connection string below***/

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new BlogViewEngine());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            AntlerConfigurator = new AntlerConfigurator();
            AntlerConfigurator.UseStructureMapContainer()
                              .UseStorage(NHibernateStorage.Use.WithDatabaseConfiguration(MsSqlConfiguration.MsSql2008.ConnectionString("Data Source=.\\SQLEXPRESS;Initial Catalog=Antler;Integrated Security=True"))
                                                                  .WithMappings(Assembly.Load("Blog.Mappings.NH")).WithGeneratedDatabase());
                        
            AntlerConfigurator.CreateInitialData();            
        }
        
        protected void Application_End()
        {
            if (AntlerConfigurator != null)
                AntlerConfigurator.Dispose();
        }
    }
}