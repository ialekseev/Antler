using System;
using Blog.Service.Contract;
using Blog.Service.Contract.Dto;
using SmartElk.Antler.Core.Abstractions.Configuration;

namespace Blog.Web.Common
{
    public static class ConfigurationEx
    {
        public static void CreateInitialData(this IAntlerConfigurator configurator, IBlogService blogService)
        {                                                
            var userId = blogService.CreateUser(new CreatedUserDto()
                {
                    Name = "John",
                    Email = "John@example.com"
                });
            
            if (!userId.HasValue)
                throw new Exception("Problem while creating User");
                                    
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