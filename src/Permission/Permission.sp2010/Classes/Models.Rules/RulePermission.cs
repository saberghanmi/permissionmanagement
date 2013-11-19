using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace Permission.Classes.Models.Rules
{
    [Serializable()]
    public class RulePermission
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public Transaction Transaction { get; set; }
        public bool Heritage { get; set; }

        public List<RoleAssignement> RoleAssignements { get; set; }

    }

    
}
