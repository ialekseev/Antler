using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.Service;
using Blog.Web.Common;
using Blog.Web.Common.AppStart;
using FluentNHibernate.Cfg.Db;
using SmartElk.Antler.Core;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.NHibernate.Configuration;
using SmartElk.Antler.StructureMap;

namespace Blog.Web.NH.SqlServer
{    
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IAntlerConfigurator AntlerConfigurator { get; private set; }

        protected void Application_Start()
        {
            /***See connection string below***/

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new BlogViewEngine());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.SetControllerFactory(new BlogControllerFactory(new BlogService()));

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