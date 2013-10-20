namespace SmartElk.Antler.Specs.Shared.Entities
{
    public class Country : Entity<int>
    {
        public virtual string Name { get; set; }
        public virtual string Language { get; set; }
    }
}
