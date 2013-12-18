using System.Web.Mvc;
using Blog.Web.Code;

namespace Blog.Web.Controllers
{
    public class HomeController : BaseController
    {        
        [HttpGet]
        public ActionResult Index()
        {            
            var posts = BlogService.GetAllPosts();
            return View(posts);
        }
    }
}
