using FluentNHibernate.Mapping;
using SmartElk.Antler.Specs.Shared.Entities;

namespace SmartElk.Antler.Hibernate.Specs.Mappings
{
    public class CountryMap : ClassMap<Country>
    {
        public CountryMap()
		{
			Table("COUNTRIES");

            Id(a => a.Id).GeneratedBy.Native("COUNTRIES_ID");
			Map(a => a.Name, "NAME");
            Map(a => a.Language, "LANGUAGE");
		}
    }
}
