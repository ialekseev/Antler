using System.Collections.Generic;

namespace Blog.Domain.Entities
{
    public class User : Entity<int>
    {
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual IList<Post> Posts { get; set; }

        public User()
        {
            Posts = new List<Post>();
        }
    }
}
