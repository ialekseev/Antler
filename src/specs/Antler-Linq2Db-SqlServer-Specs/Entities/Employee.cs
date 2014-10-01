using LinqToDB.Mapping;

namespace SmartElk.Antler.Linq2Db.SqlServer.Specs.Entities
{
    [Table(Name = "EMPLOYEE")]
    public class Employee
    {
        [PrimaryKey, Column(Name = "GPIN")]
        public virtual string Id { get; set; }

        [Column(Name = "EMAIL")]
        public virtual string Email { get; set; }
        
        [Column(Name = "FIRST_NAME")]
        public virtual string FirstName { get; set; }
        
        [Column(Name = "LAST_NAME")]
        public virtual string LastName { get; set; }

        [Column(Name = "JOB_TITLE")]
        public virtual string JobTitle { get; set; }        
    }
}
