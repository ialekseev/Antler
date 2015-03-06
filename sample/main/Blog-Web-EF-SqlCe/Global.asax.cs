using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.Service;
using Blog.Service.Contract;
using Blog.Web.Common;
using Blog.Web.Common.AppStart;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
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
            /***Example of using Antler with Castle Windsor IoC container & EntityFramework ORM & Sql CE database. See connection string below***/

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new BlogViewEngine());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var container = new WindsorContainer();
            container.Register(Component.For<IBlogService>().ImplementedBy<BlogService>().LifestyleSingleton());
            container.Register(Classes.FromAssemblyNamed("Blog.Web.Common").BasedOn<BaseController>().LifestyleTransient());
                        
            AntlerConfigurator = new AntlerConfigurator();
            AntlerConfigurator.UseWindsorContainer(container)
                              .UseStorage(EntityFrameworkPlusSqlCe.Use.WithConnectionString("Data Source=|DataDirectory|\\BlogDB.sdf")
                                                                  .WithMappings(Assembly.Load("Blog.Mappings.EF"))).CreateInitialData(container.Resolve<IBlogService>());

            ControllerBuilder.Current.SetControllerFactory(new BlogControllerFactory(container.Resolve));
        }
        
        protected void Application_End()
        {
            if (AntlerConfigurator != null)
                AntlerConfigurator.Dispose();
        }
    }
}