using System.Web.Mvc;
using Blog.Web.Code;

namespace Blog.Web.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var users = BlogService.GetAllUsers();
            return View(users);
        }
    }
}
