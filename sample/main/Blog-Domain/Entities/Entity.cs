namespace Blog.Domain.Entities
{
    public class Entity<TId>
    {
        public virtual TId Id { get; set; }
    }
}
