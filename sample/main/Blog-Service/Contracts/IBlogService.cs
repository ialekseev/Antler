using System.Collections.Generic;
using Blog.Service.Dto;

namespace Blog.Service.Contracts
{
    public interface IBlogService
    {
        int? CreateUser(CreatedUserDto userDto);
        PostDto GetPost(int postId);
        IList<PostDto> GetAllPosts();
        int? CreatePost(CreatedPostDto postDto);
        void EditPost(EditedPostDto editedPostDto);
    }
}
