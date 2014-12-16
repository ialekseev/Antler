using System.Data.Common;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.Service;
using Blog.Service.Contract;
using Blog.Web.Common;
using Blog.Web.Common.AppStart;
using FluentNHibernate.Cfg.Db;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.NHibernate.Configuration;
using SmartElk.Antler.StructureMap;
using SmartElk.Antler.Core;
using StructureMap;

namespace Blog.Web.NH.SqlServer
{    
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IAntlerConfigurator AntlerConfigurator { get; private set; }

        protected void Application_Start()
        {
            /***Example of using Antler with StructureMap IoC container & NHibernate ORM & SqlServer database. See connection string below***/

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new BlogViewEngine());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var container = new Container(x =>
                {
                    x.For<IBlogService>().Use<BlogService>().Singleton();                    
                    x.Scan(s =>
                    {                        
                        s.AddAllTypesOf(typeof(BaseController));
                        s.Assembly("Blog.Web.Common");                                          
                    });
                });
                                                
            const string connectionString = "Data Source=(localdb)\\Projects;Initial Catalog=Antler;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";
            AntlerConfigurator = new AntlerConfigurator();
            AntlerConfigurator.UseStructureMapContainer(container)
                              .UseStorage(NHibernateStorage.Use.WithDatabaseConfiguration(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
                                            .WithMappings(Assembly.Load("Blog.Mappings.NH")).WithCommandToTryToApplyOnServer(DbProviderFactories.GetFactory("System.Data.SqlClient"), connectionString, "CREATE DATABASE Antler")
                                            .WithRegeneratedSchema(true)).CreateInitialData(container.GetInstance<IBlogService>());
            
            ControllerBuilder.Current.SetControllerFactory(new BlogControllerFactory(container.GetInstance));
        }
        
        protected void Application_End()
        {
            if (AntlerConfigurator != null)
                AntlerConfigurator.Dispose();
        }
    }
}