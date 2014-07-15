using LinqToDB.Mapping;

namespace SmartElk.Antler.Linq2Db.SqlServer.Specs.Entities
{
    [Table(Name = "COUNTRIES")]
    public class Country
    {
        [PrimaryKey, Identity, Column(Name = "COUNTRIES_TEAMS_ID")]
        public virtual int Id { get; set; }

        [Column(Name = "NAME")]
        public virtual string Name { get; set; }

        [Column(Name = "LANGUAGE")]
        public virtual string Language { get; set; }
    }
}
