using Blog.Domain.Entities;
using FluentNHibernate.Mapping;

namespace Blog.Mappings.NH
{
    public class PostMap : ClassMap<Post>
    {
        public PostMap()
		{
			Table("POSTS");

            Id(a => a.Id).Column("POST_ID").GeneratedBy.Native("POST_ID");
                        
            Map(p => p.Title, "TITLE");
            Map(p => p.Text, "TEXT");
            Map(p => p.Created, "DATE");
            References(p => p.Author);
		}
    }
}
