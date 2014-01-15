using System.Web.Mvc;

namespace Blog.Web.Common.Controllers
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
