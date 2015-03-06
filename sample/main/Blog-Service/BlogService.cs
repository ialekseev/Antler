using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Domain.Entities;
using Blog.Service.Contract;
using Blog.Service.Contract.Dto;
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

        public UserDto FindUserByName(string name)
        {
            return UnitOfWork.Do(uow =>
                {
                    var found = uow.Repo<User>().AsQueryable().FirstOrDefault(t => t.Name == name);
                    if (found != null)
                    {
                        return new UserDto() {Id = found.Id, Name = found.Name, Email = found.Email};
                    }
                    return null;
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

        public IList<UserDto> GetAllUsers()
        {
            return UnitOfWork.Do(uow => uow.Repo<User>().AsQueryable().Select(t => new UserDto() { Id = t.Id, Name = t.Name, Email = t.Email }).ToList());
        }

        public int? SavePost(SavePostDto postDto)
        {                        
            return UnitOfWork.Do(uow =>
                {
                    if (postDto.Id == 0)
                    {
                        var post = new Post()
                        {
                            Title = postDto.Title,
                            Text = postDto.Text,
                            Created = DateTime.Now,
                            Author = uow.Repo<User>().GetById(postDto.AuthodId)
                        };

                        var found = uow.Repo<Post>().AsQueryable().FirstOrDefault(t => t.Title == postDto.Title);
                        if (found == null)
                        {
                            uow.Repo<Post>().Insert(post);
                            return post.Id;
                        }                        
                    }
                    else
                    {
                        var found = uow.Repo<Post>().GetById(postDto.Id);
                        if (found != null)
                        {
                            found.Title = postDto.Title;
                            found.Text = postDto.Text;
                            found.Author = uow.Repo<User>().GetById(postDto.AuthodId);
                            return found.Id;
                        }                        
                    }
                    return (int?)null;
                });            
        }

        public void DeletePost(int postId)
        {
            UnitOfWork.Do(uow => uow.Repo<Post>().Delete(postId));
        }
    }
}
