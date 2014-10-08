using System;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.Service.Contract;

namespace Blog.Web.Common
{
    public class BlogControllerFactory: DefaultControllerFactory
    {
        private readonly IBlogService _blogService;
        public BlogControllerFactory(IBlogService blogService)
        {
            _blogService = blogService;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType != null)
             return (IController) Activator.CreateInstance(controllerType, _blogService); //todo: use IContainer from AntlerConfigurator here?
            return null;
        }
    }
}
