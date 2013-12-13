using System.Collections.Generic;
using Blog.Domain.Entities;

namespace Blog.Service.Contracts
{
    public interface IBlogService
    {
        IList<Post> GetAllPosts();
    }
}
