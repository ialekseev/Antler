using System.Web.Mvc;
using Blog.Service;

namespace Blog.Web.Controllers
{
    public class HomeController : Controller
    {        
        public ActionResult Index()
        {
            var blogService = new BlogService();
            var posts = blogService.GetAllPosts();
            return View(posts);
        }
    }
}
