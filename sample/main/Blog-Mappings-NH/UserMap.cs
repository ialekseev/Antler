using Blog.Domain.Entities;
using FluentNHibernate.Mapping;

namespace Blog.Mappings.NH
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("USERS");

            Id(a => a.Id).Column("USER_ID").GeneratedBy.Native("USER_ID");
                        
            Map(p => p.Name, "NAME");
            Map(p => p.Email, "EMAIL");
            HasMany(p => p.Posts);
        }
    }
}
