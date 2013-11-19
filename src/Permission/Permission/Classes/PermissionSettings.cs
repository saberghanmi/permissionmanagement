using Microsoft.SharePoint;
using Permission.Classes.Models.Rules;
using Permission.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Classes
{
   public class PermissionSettings
    {
       const string permissionValue = "PermissionsValue";

       public PermissionPersisted PermissionPersisted { get; set; }

       public PermissionSettings(SPList list)
       {
           var val= HelperPropertiesBags.GetParametreFromList(list, permissionValue);
           if (val != null)
           {
               string permissionString = val.ToString();
               PermissionPersisted = HelperSerializer.XmlDeserializeFromString(permissionString, typeof(PermissionPersisted)) as PermissionPersisted;
           
           }

       }

       public static void SaveParametres(SPList list, PermissionPersisted permissionPersisted)
       {
           HelperPropertiesBags.CreateUpdateParametreToList(list, permissionValue, HelperSerializer.SerializeToString(permissionPersisted));
       }

     


    }

    [Serializable()]
   public class PermissionPersisted
   {
       public List<RulePermission> Rules { get; set; }
   }
}
