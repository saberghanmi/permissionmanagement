using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Permission.Classes.Models.Rules;
using Permission.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Classes
{
    class EventReceiverClass : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            SPList list = properties.List;
            SPWeb web=properties.Web;

            PermissionSettings settings = new PermissionSettings(list);
            List<RulePermission> rules = settings.PermissionPersisted.Rules.Where(r =>
            {
                return r.Transaction == Transaction.Add ||
                    r.Transaction == Transaction.AddEdit;
            }).ToList();

            if (rules.Count != 0)
            {
                
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(web.Url))
                    {
                        using (SPWeb web1 = site.OpenWeb())
                        {
                            SPList list1 = web1.Lists[list.ID];
                            ServiceEventReceiver.ApplyPermission(web1, list1, list1.Items.GetItemById(properties.ListItemId), rules);

                        }

                    }
                });
            }

            base.ItemAdded(properties);
        }

        /// <summary>
        /// An item is being updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            SPWeb web = properties.Web;
            SPList list = properties.List;
           
            PermissionSettings settings = new PermissionSettings(list);
            List<RulePermission> rules = settings.PermissionPersisted.Rules.Where(r =>
            {
                return r.Transaction == Transaction.Edit ||
                    r.Transaction == Transaction.AddEdit;
            }).ToList();

            if (rules.Count != 0)
            {

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(web.Url))
                    {
                        using (SPWeb web1 = site.OpenWeb())
                        {
                            SPList list1 = web1.Lists[list.ID];
                            ServiceEventReceiver.ApplyPermission(web1, list1,list1.Items.GetItemById(properties.ListItemId), rules);

                        }

                    }
                });
            }
            

            base.ItemUpdated(properties);
        }

       
    }
}
