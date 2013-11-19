using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Permission.Helper;



namespace Permission.Classes.Models.Rules
{
    [Serializable()]
    public class RoleAssignement 
    {
        public TypeUser TypeUser { get; set; }
        public string ColumnUser { get; set; }

        public string StaticUser { get; set; }

        public string UserProfilePropertyName { get; set; }

        public List<string> RoleDefinitions { get; set; } 
      
    }
    

}
