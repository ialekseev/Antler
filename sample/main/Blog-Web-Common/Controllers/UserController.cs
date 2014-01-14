using System.Web.Mvc;

namespace Blog.Web.Common.Controllers
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
