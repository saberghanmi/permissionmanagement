using Microsoft.SharePoint;
using Permission.Classes;
using Permission.Classes.Models.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Helper
{
   public static class PermissionService
    {
       public static void AddRule(SPList list, RulePermission rule)
       {

           PermissionSettings settings = new PermissionSettings(list);

           if (settings.PermissionPersisted != null)
           {
               settings.PermissionPersisted.Rules.Add(rule);

               int index = 0;
               settings.PermissionPersisted.Rules.ForEach(r =>
               {
                   index++;
                   r.Order = index;
               });

               PermissionSettings.SaveParametres(list, settings.PermissionPersisted);
           }
           else
           {
               rule.Order = 1;
               PermissionPersisted persisted = new PermissionPersisted();
               persisted.Rules = new List<RulePermission>() { rule };
               PermissionSettings.SaveParametres(list, persisted);
           }

       }


    }
}
