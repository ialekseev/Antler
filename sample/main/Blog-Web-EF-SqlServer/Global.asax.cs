using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
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
            /***You need to have "Antler" database in your SQL SERVER(if it can't be generated). See connection string below***/
            
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new BlogViewEngine());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            AntlerConfigurator = new AntlerConfigurator();
            AntlerConfigurator.UseBuiltInContainer()
                              .UseStorage(EntityFrameworkStorage.Use.WithConnectionString("Data Source=.\\SQLEXPRESS;Initial Catalog=Antler;Integrated Security=True").WithLazyLoading().WithDatabaseInitializer(new DropCreateDatabaseIfModelChanges<DataContext>())
                                                                  .WithMappings(Assembly.Load("Blog.Mappings.EF")));
                        
            AntlerConfigurator.CreateInitialData();            
        }
        
        protected void Application_End()
        {
            if (AntlerConfigurator != null)
                AntlerConfigurator.Dispose();
        }
    }
}