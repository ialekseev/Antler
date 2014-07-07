using LinqToDB.Mapping;

namespace SmartElk.Antler.Linq2Db.SqlServer.Specs.Entities
{
    [Table(Name = "COUNTRIES")]
    public class Country
    {
        [PrimaryKey, Identity]
        public virtual int Id { get; set; }
        
        [Column(Name = "Name")]
        public virtual string Name { get; set; }

        [Column(Name = "Language")]
        public virtual string Language { get; set; }
    }
}
