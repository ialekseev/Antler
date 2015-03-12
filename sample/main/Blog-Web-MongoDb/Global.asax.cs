using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.Service;
using Blog.Service.Contract;
using Blog.Web.Common;
using Blog.Web.Common.AppStart;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.MongoDb.Configuration;
using SmartElk.Antler.StructureMap;
using SmartElk.Antler.Core;
using StructureMap;


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

            var container = new Container(x =>
            {
                x.For<IBlogService>().Use<BlogService>().Singleton();
                x.Scan(s =>
                {
                    s.AddAllTypesOf(typeof(BaseController));
                    s.Assembly("Blog.Web.Common");
                });
            });

            AntlerConfigurator = new AntlerConfigurator();
            AntlerConfigurator.UseStructureMapContainer(container)
                .UseStorage(MongoDbStorage.Use("mongodb://localhost:27017", "Antler")
                    .WithRecreatedDatabase(true).WithIdentityGenerator(()=>new Random().Next(Int32.MinValue, Int32.MaxValue))).CreateInitialData(container.GetInstance<IBlogService>());
                                                                              
            ControllerBuilder.Current.SetControllerFactory(new BlogControllerFactory(t => Activator.CreateInstance(t, service)));
        }
        
        protected void Application_End()
        {
            if (AntlerConfigurator != null)
                AntlerConfigurator.Dispose();
        }
    }
}