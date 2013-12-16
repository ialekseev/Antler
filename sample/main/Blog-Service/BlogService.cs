using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Domain.Entities;
using Blog.Service.Contracts;
using Blog.Service.Dto;
using SmartElk.Antler.Core.Domain;

namespace Blog.Service
{
    public class BlogService: IBlogService
    {        
        public int? CreateUser(CreatedUserDto userDto)
        {
            return UnitOfWork.Do(uow =>
                {
                    var found = uow.Repo<User>().AsQueryable().FirstOrDefault(t => t.Name == userDto.Email);
                    if (found == null)
                    {
                        var user = new User {Name = userDto.Name, Email = userDto.Email};
                        uow.Repo<User>().Insert(user);
                        return user.Id;
                    }
                    return (int?)null;
                });
        }
        
        public PostDto GetPost(int postId)
        {
            return UnitOfWork.Do(uow =>
                {
                    var post = uow.Repo<Post>().GetById(postId);
                    return post != null ? Mapper.Map(post) : null;
                });
        }
        
        public IList<PostDto> GetAllPosts()
        {
            return UnitOfWork.Do(uow => uow.Repo<Post>().AsQueryable().ToList().Select(Mapper.Map).ToList());
        }

        public int? CreatePost(CreatedPostDto postDto)
        {                        
            return UnitOfWork.Do(uow =>
                {
                    var post = new Post()
                    {
                        Title = postDto.Title,
                        Text = postDto.Text,
                        Created = DateTime.Now,
                        Author = new User() { Id = postDto.AuthodId }
                    };

                    var found = uow.Repo<Post>().AsQueryable().FirstOrDefault(t => t.Title == postDto.Title);
                    if (found == null)
                    {
                        uow.Repo<Post>().Insert(post);
                        return post.Id;
                    }
                    return (int?)null;
                });            
        }

        public void EditPost(EditedPostDto editedPostDto)
        {
            UnitOfWork.Do(uof =>
                {
                    var found = uof.Repo<Post>().GetById(editedPostDto.Id);
                    if (found != null)
                    {
                        found.Title = editedPostDto.Title;
                        found.Text = editedPostDto.Text;                        
                    }
                });
        }
    }
}
