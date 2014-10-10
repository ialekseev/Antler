using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.Service.Contract;
using Blog.Web.Common;
using Blog.Web.Common.AppStart;
using Blog.Web.Linq2Db.SqlServer.Code;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SmartElk.Antler.Core;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Linq2Db.Configuration;
using SmartElk.Antler.Windsor;

namespace Blog.Web.Linq2Db.SqlServer
{
    //!Warning! Work in progress here.
    //TODO: generate database
    //TODO: verify sample

    public class MvcApplication : System.Web.HttpApplication
    {
        public static IAntlerConfigurator AntlerConfigurator { get; private set; }

        protected void Application_Start()
        {
            /***Example of using Antler with Castle Windsor IoC container & Linq2Db ORM & SQLEXPRESS database. See connection string below***/

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new BlogViewEngine());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
                        
            var container = new WindsorContainer();
            container.Register(Component.For<IBlogService>().ImplementedBy<Linq2DbBlogService>());
            container.Register(Classes.FromAssemblyNamed("Blog.Web.Common").BasedOn<BaseController>().LifestyleTransient());
                        
            AntlerConfigurator = new AntlerConfigurator();
            AntlerConfigurator.UseWindsorContainer(container).UseStorage(Linq2DbStorage.Use("Data Source=.\\SQLEXPRESS;Initial Catalog=Antler;Integrated Security=True")).CreateInitialData(container.Resolve<IBlogService>());

            ControllerBuilder.Current.SetControllerFactory(new BlogControllerFactory(container.Resolve));
        }
        
        protected void Application_End()
        {
            if (AntlerConfigurator != null)
                AntlerConfigurator.Dispose();
        }
    }
}