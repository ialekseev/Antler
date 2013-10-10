using System.Collections.Generic;

namespace SmartElk.Antler.Hibernate.Specs.Entities
{
    public class Team: Entity<int>
    {        
        public virtual string Name { get; set; }				
		public virtual string BusinessGroup { get; set; }
		public virtual IList<Employee> Members { get; set; }		
    }
}
