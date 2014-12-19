using LinqToDB.Mapping;

namespace Blog.Web.Linq2Db.SqlServer.Code.Entities
{
    [Table(Name = "USERS")]
    public class User
    {
        [PrimaryKey, Identity, Column(Name = "USER_ID")]
        public virtual int Id { get; set; }
        
        [Column(Name = "NAME")]
        public virtual string Name { get; set; }
        
        [Column(Name = "EMAIL")]
        public virtual string Email { get; set; }        
    }
}
