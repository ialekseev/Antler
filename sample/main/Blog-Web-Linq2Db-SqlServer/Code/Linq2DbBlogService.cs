using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Service.Contract;
using Blog.Service.Contract.Dto;
using Blog.Web.Linq2Db.SqlServer.Code.Entities;
using SmartElk.Antler.Core.Domain;

namespace Blog.Web.Linq2Db.SqlServer.Code
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
                    var user = new User { Name = userDto.Name, Email = userDto.Email };
                    return uow.Repo<User>().Insert<int>(user);                    
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
            throw new NotImplementedException();
        }

        public IList<PostDto> GetAllPosts()
        {
            throw new NotImplementedException();
        }

        public IList<UserDto> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public int? SavePost(SavePostDto postDto)
        {
            throw new NotImplementedException();
        }

        public void DeletePost(int postId)
        {
            throw new NotImplementedException();
        }
    }
}
