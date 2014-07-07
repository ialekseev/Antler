using LinqToDB.Mapping;

namespace SmartElk.Antler.Linq2Db.SqlServer.Specs.Entities
{
    [Table(Name = "TEAMS_MEMBERS_MAP")]
    public class TeamEmployeeMap
    {                
        [Column(Name = "TEAM_ID")]
        public virtual int TeamId { get; set; }

        [Column(Name = "MEMBER_GPIN")]
        public virtual string EmployeeId { get; set; }
    }
}
