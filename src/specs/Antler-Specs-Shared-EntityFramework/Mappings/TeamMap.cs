using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using SmartElk.Antler.Specs.Shared.Entities;

namespace SmartElk.Antler.Specs.Shared.EntityFramework.Mappings
{    
    public class TeamMap : EntityTypeConfiguration<Team>
    {
        public TeamMap()
		{
			ToTable("TEAMS");

            HasKey(c => c.Id).Property(c => c.Id).HasColumnName("TEAMS_ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);            
            Property(p => p.Name).HasColumnName("NAME");
            Property(p => p.Description).HasColumnName("DESCRIPTION");
            HasMany(d => d.Members).WithMany(t => t.Teams).Map(m =>
            {
                m.ToTable("TEAMS_MEMBERS_MAP");
                m.MapLeftKey("TEAM_ID");
                m.MapRightKey("MEMBER_GPIN");
            });
            HasOptional(c => c.Country);
		}
    }
}
