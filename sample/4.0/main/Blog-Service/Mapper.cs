using Blog.Domain.Entities;
using Blog.Service.Contract.Dto;

namespace Blog.Service
{
    public static class Mapper
    {
        public static PostDto Map(Post post)
        {
            return new PostDto()
                {
                    Id = post.Id,
                    Title = post.Title,
                    Text = post.Text,
                    Created = post.Created,
                    AuthorId = post.Author.Id,
                    AuthorName = post.Author.Name
                };
        }
    }
}
