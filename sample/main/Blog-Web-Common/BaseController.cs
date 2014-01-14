using System.Web.Mvc;
using Blog.Service;
using Blog.Service.Contracts;
using Blog.Service.Dto;

namespace Blog.Web.Common
{
    public abstract class BaseController : Controller
    {
        protected IBlogService BlogService { get; set; }
        protected BaseController()
        {
            BlogService = new BlogService();
        }  
    
        public UserDto CurrentUser
        {
            get { return BlogService.FindUserByName("John"); }
        }
    }
}
