using FluentNHibernate.Mapping;
using SmartElk.Antler.Specs.Shared.Entities;

namespace SmartElk.Antler.Specs.Shared.NHibernate.Mappings
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table("EmployeeTable");

            Id(a => a.Id, "GPIN");
            Map(a => a.Email, "EMAIL");
            Map(a => a.FirstName, "FIRST_NAME");
            Map(a => a.LastName, "LAST_NAME");
            Map(a => a.JobTitle, "BUS_TITLE");            
            HasManyToMany(a => a.Teams)
                .Table("TEAMS_MEMBERS_MAP")
                .ChildKeyColumn("TEAM_ID")
                .ParentKeyColumn("MEMBER_GPIN")
                .ExtraLazyLoad();  
        }        
    }
}
