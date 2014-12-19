using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Blog.Domain.Entities;

namespace Blog.Mappings.EF
{
    public class PostMap : EntityTypeConfiguration<Post>
    {
        public PostMap()
		{
			ToTable("POSTS");

            HasKey(c => c.Id).Property(c => c.Id).HasColumnName("POST_ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            Property(p => p.Title).HasColumnName("TITLE");
            Property(p => p.Text).HasColumnName("TEXT");
            Property(p => p.Created).HasColumnName("DATE");
            HasOptional(p => p.Author);
		}
    }
}
