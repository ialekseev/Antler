using System;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.Service;
using Blog.Web.Common;
using Blog.Web.Common.AppStart;
using SmartElk.Antler.Core;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Common.Container;
using SmartElk.Antler.EntityFramework.Configuration;
using SmartElk.Antler.EntityFramework.Internal;

namespace Blog.Web.EF.SqlServer
{    
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IAntlerConfigurator AntlerConfigurator { get; private set; }

        protected void Application_Start()
        {
            /***Example of using Antler with BuiltIn container & EntityFramework ORM & SQLEXPRESS database . See connection string below***/
            
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new BlogViewEngine());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            var service = new BlogService();
            
            AntlerConfigurator = new AntlerConfigurator();
            AntlerConfigurator.UseBuiltInContainer()
                              .UseStorage(EntityFrameworkStorage.Use.WithConnectionString("Data Source=.\\SQLEXPRESS;Initial Catalog=Antler;Integrated Security=True").WithLazyLoading().WithDatabaseInitializer(new DropCreateDatabaseAlways<DataContext>())
                                                                  .WithMappings(Assembly.Load("Blog.Mappings.EF"))).CreateInitialData(service);

            ControllerBuilder.Current.SetControllerFactory(new BlogControllerFactory(t => Activator.CreateInstance(t, service)));
        }
        
        protected void Application_End()
        {
            if (AntlerConfigurator != null)
                AntlerConfigurator.Dispose();
        }
    }
}