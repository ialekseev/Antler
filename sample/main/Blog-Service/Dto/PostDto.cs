using System;

namespace Blog.Service.Dto
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
