using System.Collections.Generic;
using Blog.Service.Contract.Dto;
using Blog.Web.Linq2Db.SqlServer.Code.Entities;
using System.Linq;

namespace Blog.Web.Linq2Db.SqlServer.Code
{
    public static class Linq2DbMapper
    {
        public static PostDto Map(Post post)
        {
            return new PostDto()
            {
                Id = post.Id,
                Title = post.Title,
                Text = post.Text,
                Created = post.Created,
                AuthorId = post.AuthorId                
            };
        }

        public static PostDto ApplyAuthorName(this PostDto post, User author)
        {            
           post.AuthorName = author.Name;            
           return post;
        }

        public static IList<PostDto> ApplyAuthorName(this List<PostDto> posts, IList<User> authors)
        {                        
            posts.ForEach(t =>
            {
                var author = authors.First(p => p.Id == t.AuthorId);
                t.AuthorName = author.Name;                
            });
            return posts;
        }
    }
}