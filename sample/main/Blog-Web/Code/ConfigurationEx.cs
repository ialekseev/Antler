using System;
using Blog.Service;
using Blog.Service.Dto;
using SmartElk.Antler.Core.Abstractions.Configuration;

namespace Blog.Web.Code
{
    public static class ConfigurationEx
    {
        public static void CreateInitialData(this IAntlerConfigurator configurator)
        {
            var blogService = new BlogService();
                                    
            var userId = blogService.CreateUser(new CreatedUserDto()
                {
                    Name = "John",
                    Email = "John@example.com"
                });
            
            if (!userId.HasValue)
                throw new Exception("Problem with creating User");
                                    
            blogService.SavePost(new SavePostDto()
            {
                Title = "Great post about programming",
                Text = "Programming is a ...",
                AuthodId = userId.Value
            });

            blogService.SavePost(new SavePostDto()
            {
                Title = "Great post about fishing",
                Text = "Fishing is a ...",
                AuthodId = userId.Value
            });            
        }
    }
}