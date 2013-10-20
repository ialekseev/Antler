using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using SmartElk.Antler.Specs.Shared.Entities;

namespace SmartElk.Antler.EntityFramework.Sqlite.Specs.Mappings
{
    public class CountryMap : EntityTypeConfiguration<Country>
    {
        public CountryMap()
		{
			ToTable("COUNTRIES");

            HasKey(c => c.Id).Property(c => c.Id).HasColumnName("COUNTRIES_TEAMS_ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            Property(p => p.Name).HasColumnName("NAME");
            Property(p => p.Language).HasColumnName("LANGUAGE");            
		}
    }
}
