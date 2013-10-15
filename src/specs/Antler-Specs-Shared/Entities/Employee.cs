using System.Collections.Generic;

namespace SmartElk.Antler.Specs.Shared.Entities
{
    public class Employee: Entity<string>
    {        
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }        
        public virtual string Email { get; set; }
        public virtual string JobTitle { get; set; }        
        public virtual IList<Team> Teams { get; set; }
    }
}
