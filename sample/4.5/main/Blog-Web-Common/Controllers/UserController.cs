using System.Web.Mvc;
using Blog.Service.Contract;

namespace Blog.Web.Common.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IBlogService blogService) : base(blogService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            var users = BlogService.GetAllUsers();
            return View(users);
        }
    }
}
