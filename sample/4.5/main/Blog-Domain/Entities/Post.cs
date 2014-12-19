using System;

namespace Blog.Domain.Entities
{
    public class Post: Entity<int>
    {
        public virtual string Title { get; set; }
        public virtual string Text { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual User Author { get; set; }
    }
}
