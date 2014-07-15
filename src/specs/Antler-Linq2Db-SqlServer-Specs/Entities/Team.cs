using LinqToDB.Mapping;

namespace SmartElk.Antler.Linq2Db.SqlServer.Specs.Entities
{
    [Table(Name = "STANDARDS_TEAMS")]
    public class Team
    {
        [PrimaryKey, Identity, Column(Name = "STANDARDS_TEAMS_ID")]
        public virtual int Id { get; set; }
         
        [Column(Name = "NAME")]        
        public virtual string Name { get; set; }
        
        [Column(Name = "BU")]
        public virtual string Description { get; set; }

        [Column(Name = "COUNTRY_ID"), Nullable]
        public virtual int? CountryId { get; set; }
    }
}
