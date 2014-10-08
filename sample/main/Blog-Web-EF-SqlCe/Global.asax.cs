using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.Service;
using Blog.Web.Common;
using Blog.Web.Common.AppStart;
using SmartElk.Antler.Core;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.EntityFramework.SqlCe.Configuration;
using SmartElk.Antler.Windsor;

namespace Blog.Web.EF.SqlCe
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
            AntlerConfigurator.UseWindsorContainer()
                              .UseStorage(EntityFrameworkPlusSqlCe.Use.WithConnectionString("Data Source=|DataDirectory|\\BlogDB.sdf")
                                                                  .WithMappings(Assembly.Load("Blog.Mappings.EF"))).CreateInitialData();
        }
        
        protected void Application_End()
        {
            if (AntlerConfigurator != null)
                AntlerConfigurator.Dispose();
        }
    }
}