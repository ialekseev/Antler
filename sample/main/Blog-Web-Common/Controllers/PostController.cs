using System.Web.Mvc;
using Blog.Service.Contract;
using Blog.Service.Contract.Dto;

namespace Blog.Web.Common.Controllers
{
    public class PostController : BaseController
    {
        public PostController(IBlogService blogService) : base(blogService)
        {
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {            
            var post = BlogService.GetPost(id);
            var editedPostDto = new SavePostDto()
                {
                    Id = post.Id,
                    Title = post.Title,
                    Text = post.Text,
                    AuthodId = CurrentUser.Id
                };
            return View(editedPostDto);
        }
        
        [HttpGet]
        public ActionResult New()
        {            
            return View("Edit", new SavePostDto());
        }

        [HttpPost]
        public ActionResult Save(SavePostDto editedPostDto)
        {
            editedPostDto.AuthodId = CurrentUser.Id;
            BlogService.SavePost(editedPostDto);
            return View("Edit", editedPostDto);
        }
        
        [HttpDelete]
        public ActionResult Delete(int id)
        {            
            BlogService.DeletePost(id);
            return RedirectToAction("Index", "Home");
        }
    }
}
