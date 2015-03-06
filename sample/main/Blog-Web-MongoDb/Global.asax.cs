using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.Service;
using Blog.Web.Common;
using Blog.Web.Common.AppStart;
using SmartElk.Antler.Core;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.MongoDb.Configuration;
using SmartElk.Antler.StructureMap;


namespace Blog.Web.MongoDb
{    
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IAntlerConfigurator AntlerConfigurator { get; private set; }

        protected void Application_Start()
        {
            /***Example of using Antler with StructureMap container & MongoDb . See connection string below***/
            
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new BlogViewEngine());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            var service = new BlogService();

            AntlerConfigurator = new AntlerConfigurator();
            AntlerConfigurator.UseStructureMapContainer()
                .UseStorage(MongoDbStorage.Use("mongodb://localhost:27017", "Antler")
                    .WithRecreatedDatabase(true));
                                                                              
            ControllerBuilder.Current.SetControllerFactory(new BlogControllerFactory(t => Activator.CreateInstance(t, service)));
        }
        
        protected void Application_End()
        {
            if (AntlerConfigurator != null)
                AntlerConfigurator.Dispose();
        }
    }
}