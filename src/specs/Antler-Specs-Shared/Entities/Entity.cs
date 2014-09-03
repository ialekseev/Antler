namespace SmartElk.Antler.Specs.Shared.Entities
{
    public class Entity<TId>
    {
        public virtual TId Id { get; set; }

        public override bool Equals(object obj)
        {
            return Id.Equals(((Entity<TId>) obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
