using FluentNHibernate.Mapping;
using SmartElk.Antler.Specs.Shared.Entities;

namespace SmartElk.Antler.Specs.Shared.NHibernate.Mappings
{
    public class TeamMap : ClassMap<Team>
    {
        public TeamMap()
		{
			Table("TEAMS");
			
            Id(a => a.Id).GeneratedBy.Native("TEAMS_ID");
			Map(a => a.Name, "NAME");			
			Map(a => a.Description, "DESCRIPTION");			
			HasManyToMany(a => a.Members)
				.Table("TEAMS_MEMBERS_MAP")
				.ParentKeyColumn("TEAM_ID")
				.ChildKeyColumn("MEMBER_GPIN")
				.ExtraLazyLoad();

            References(a => a.Country, "COUNTRY_ID").Fetch.Join();
		}
    }
}
