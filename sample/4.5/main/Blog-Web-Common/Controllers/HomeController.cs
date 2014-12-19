using System.Web.Mvc;
using Blog.Service.Contract;

namespace Blog.Web.Common.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IBlogService blogService) : base(blogService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {            
            var posts = BlogService.GetAllPosts();
            return View(posts);
        }
    }
}
