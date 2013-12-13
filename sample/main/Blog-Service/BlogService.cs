using System.Collections.Generic;
using System.Linq;
using Blog.Domain.Entities;
using Blog.Service.Contracts;
using SmartElk.Antler.Core.Domain;

namespace Blog.Service
{
    public class BlogService: IBlogService
    {
        public IList<Post> GetAllPosts()
        {
            return UnitOfWork.Do(uow => uow.Repo<Post>().AsQueryable().ToList());
        }
    }
}
