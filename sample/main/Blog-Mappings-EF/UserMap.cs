using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Blog.Domain.Entities;

namespace Blog.Mappings.EF
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("USERS");

            HasKey(c => c.Id).Property(c => c.Id).HasColumnName("POST_ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.Name).HasColumnName("NAME");
            Property(p => p.Email).HasColumnName("EMAIL");
            HasMany(p => p.Posts);
        }
    }
}
