using System.Web.Mvc;
using Blog.Service;

namespace Blog.Web.Controllers
{
    public class PostController : Controller
    {        
        public ActionResult Edit(int id)
        {
            var blogService = new BlogService();
            var post = blogService.GetPost(id);
            return View(post);
        }        
    }
}
