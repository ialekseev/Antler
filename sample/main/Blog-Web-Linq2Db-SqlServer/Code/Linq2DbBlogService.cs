using System;
using System.Collections.Generic;
using Blog.Service.Contract;
using Blog.Service.Contract.Dto;

namespace Blog.Web.Linq2Db.SqlServer.Code
{
    public class BlogService: IBlogService
    {
        public int? CreateUser(CreatedUserDto userDto)
        {
            throw new NotImplementedException();
        }

        public UserDto FindUserByName(string name)
        {
            throw new NotImplementedException();
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
