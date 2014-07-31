using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.Web.Common;
using Blog.Web.Common.AppStart;
using SmartElk.Antler.Core;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Linq2Db.Configuration;
using SmartElk.Antler.Windsor;

namespace Blog.Web.Linq2Db.SqlServer
{
    //todo: !Warning! Work in progress here. TODO: create mappings for Linq2Db and own implementation of IBlogService(but with same controllers/view)? 

    public class MvcApplication : System.Web.HttpApplication
    {
        public static IAntlerConfigurator AntlerConfigurator { get; private set; }

        protected void Application_Start()
        {
            /***You need to have "Antler" database in your SQL SERVER. See connection string below***/

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new BlogViewEngine());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            AntlerConfigurator = new AntlerConfigurator();
            AntlerConfigurator.UseWindsorContainer().UseStorage(Linq2DbStorage.Use("Data Source=.\\SQLEXPRESS;Initial Catalog=Antler;Integrated Security=True"));                                    
        }
        
        protected void Application_End()
        {
            if (AntlerConfigurator != null)
                AntlerConfigurator.Dispose();
        }
    }
}