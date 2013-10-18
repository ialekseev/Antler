using System.Collections.Generic;

namespace SmartElk.Antler.Specs.Shared.Entities
{
    public class Team: Entity<int>
    {        
        public virtual string Name { get; set; }				
		public virtual string BusinessGroup { get; set; }
		public virtual IList<Employee> Members { get; set; }
        public virtual Country Country { get; set; }
        public Team()
        {
            Members=new List<Employee>();
        }
    }
}
