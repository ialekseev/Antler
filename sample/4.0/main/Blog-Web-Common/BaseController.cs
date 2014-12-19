using System.Web.Mvc;
using Blog.Service.Contract;
using Blog.Service.Contract.Dto;

namespace Blog.Web.Common
{
    public abstract class BaseController : Controller
    {
        protected IBlogService BlogService { get; set; }
        protected BaseController(IBlogService blogService)
        {
            BlogService = blogService;
        }  
    
        public UserDto CurrentUser
        {
            get { return BlogService.FindUserByName("John"); }
        }
    }
}
