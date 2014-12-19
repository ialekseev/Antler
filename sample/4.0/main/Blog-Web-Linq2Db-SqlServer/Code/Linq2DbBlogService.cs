using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Service.Contract;
using Blog.Service.Contract.Dto;
using Blog.Web.Linq2Db.SqlServer.Code.Entities;
using SmartElk.Antler.Core.Domain;

namespace Blog.Web.Linq2Db.SqlServer.Code
{
    public class Linq2DbBlogService: IBlogService
    {
        public int? CreateUser(CreatedUserDto userDto)
        {
            return UnitOfWork.Do(uow =>
            {
                var found = uow.Repo<User>().AsQueryable().FirstOrDefault(t => t.Name == userDto.Email);
                if (found == null)
                {
                    var user = new User { Name = userDto.Name, Email = userDto.Email };
                    return (int)uow.Repo<User>().Insert<decimal>(user);                    
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
                    return new UserDto() { Id = found.Id, Name = found.Name, Email = found.Email };
                }
                return null;
            });
        }

        public PostDto GetPost(int postId)
        {
            return UnitOfWork.Do(uow =>
                {
                   var post = uow.Repo<Post>().AsQueryable().FirstOrDefault(t => t.Id == postId);
                   if (post != null)
                   {
                      return Linq2DbMapper.Map(post).ApplyAuthorName(uow.Repo<User>().AsQueryable().First(t => t.Id == post.AuthorId));                                        
                   }
                return null;                
            });
        }

        public IList<PostDto> GetAllPosts()
        {
             return UnitOfWork.Do(uow =>
                {
                    var postDtos = uow.Repo<Post>().AsQueryable().ToList().Select(Linq2DbMapper.Map).ToList();
                    var authorsIds = postDtos.Select(t => t.AuthorId).ToList();
                    var authors = uow.Repo<User>().AsQueryable().Where(t => authorsIds.Contains(t.Id)).ToList();
                    return postDtos.ApplyAuthorName(authors);                                                            
                });                            
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
                        AuthorId = uow.Repo<User>().AsQueryable().First(t=>t.Id==postDto.AuthodId).Id
                    };

                    var found = uow.Repo<Post>().AsQueryable().FirstOrDefault(t => t.Title == postDto.Title);
                    if (found == null)
                    {
                        return (int)uow.Repo<Post>().Insert<decimal>(post);                        
                    }
                }
                else
                {
                    var found = uow.Repo<Post>().AsQueryable().FirstOrDefault(t => t.Id == postDto.Id);
                    if (found != null)
                    {
                        found.Title = postDto.Title;
                        found.Text = postDto.Text;
                        found.AuthorId = uow.Repo<User>().AsQueryable().First(t => t.Id == postDto.AuthodId).Id;
                        uow.Repo<Post>().Update(found);
                        return found.Id;
                    }
                }
                return (int?)null;
            }); 
        }

        public void DeletePost(int postId)
        {
            UnitOfWork.Do(uow =>
                {
                    var post = uow.Repo<Post>().AsQueryable().First(t => t.Id == postId);
                    uow.Repo<Post>().Delete(post);
                });
        }
    }
}
