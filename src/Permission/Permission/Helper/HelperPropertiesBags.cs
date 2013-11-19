using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Helper
{
   public class HelperPropertiesBags
    {
        public static object GetParametreFromList(SPList list, string paramatreName)
        {
            SPFolder folder = list.RootFolder;


            if (folder.Properties.ContainsKey(paramatreName))
                return folder.GetProperty(paramatreName);
            else
            {
                return null;
            }
        }


        public static void CreateUpdateParametreToList(SPList list, string parametreName, object value)
        {
            SPFolder folder = list.RootFolder;

            if (!folder.Properties.ContainsKey(parametreName))
            {
                folder.AddProperty(parametreName, value);
            }
            else
            {
                folder.SetProperty(parametreName, value);
            }
            folder.Update();
        }
        public static void CreateUpdateParametreToList(SPList list, Dictionary<string, object> parametres)
        {
            SPFolder folder = list.RootFolder;
            foreach (var item in parametres)
            {

                if (!folder.Properties.ContainsKey(item.Key))
                {
                    folder.AddProperty(item.Key, item.Value);
                }
                else
                {
                    folder.SetProperty(item.Key, item.Value);
                }
            }
            folder.Update();
        }

    }
}
