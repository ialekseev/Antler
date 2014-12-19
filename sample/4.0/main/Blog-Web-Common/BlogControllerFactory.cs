using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Blog.Web.Common
{
    public class BlogControllerFactory: DefaultControllerFactory
    {
        private readonly Func<Type,object> _dependencyResolver;
        public BlogControllerFactory(Func<Type, object> dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {                        
            if (controllerType != null)
                return (IController)_dependencyResolver(controllerType);
            return null;                        
        }
    }
}
