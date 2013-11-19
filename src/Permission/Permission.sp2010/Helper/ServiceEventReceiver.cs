using Microsoft.Office.Server.UserProfiles;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Administration;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Permission.Classes;
using Permission.Classes.Models.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Permission.Helper
{
    public class ServiceEventReceiver
    {
        const string eventReceiverAddName = "PermissionReceiverAddItem";
        const string eventReceiverUpdateName = "PermissionReceiverUpdateItem";
        public static void ApplyPermission(SPWeb web, SPList list, SPListItem item, List<RulePermission> rules)
        {
            item.BreakRoleInheritance(rules[0].Heritage);
            UserProfileManager profileManager = null;
            using (SPSite site = new SPSite(web.Site.ID))
            {
                
                SPServiceContext context = SPServiceContext.GetContext(site);
                profileManager = new UserProfileManager(context);

            }
            foreach (RulePermission rule in rules)
            {
                foreach (RoleAssignement role in rule.RoleAssignements)
                {
                    SPPrincipal user = null;
                    switch (role.TypeUser)
                    {
                        #region switch control

                        case TypeUser.StaticUser:
                            {
                                #region static user
                                SPPrincipalInfo principalInfo = SPUtility.ResolvePrincipal(web, role.StaticUser, SPPrincipalType.All, SPPrincipalSource.All, null, false);
                                switch (principalInfo.PrincipalType)
                                {
                                    case SPPrincipalType.User:
                                        user =web.AllUsers.Cast<SPUser>().FirstOrDefault(u=>u.LoginName==principalInfo.LoginName); break;
                                    case SPPrincipalType.SharePointGroup:
                                        user = web.SiteGroups.Cast<SPGroup>().First(g=>g.LoginName==principalInfo.LoginName); break;
                                    case SPPrincipalType.SecurityGroup:
                                        user = web.AllUsers.Cast<SPUser>().FirstOrDefault(u=>u.LoginName==principalInfo.LoginName); break;
                                }
                                #endregion
                                break;
                            }
                        case TypeUser.PropertyUser:
                            {
                                #region column
                                SPFieldUser fieldUser = list.Fields[new Guid(role.ColumnUser)] as SPFieldUser;

                                if (fieldUser.AllowMultipleValues)
                                {
                                    #region Multiple Users
                                    SPFieldUserValueCollection userValueCollection = (SPFieldUserValueCollection)fieldUser.GetFieldValue(item[fieldUser.InternalName].ToString());

                                    foreach (SPFieldUserValue userV in userValueCollection)
                                    {

                                        SPRoleAssignment roleAssigenment = null;
                                        if (userV.User != null)
                                            roleAssigenment = new SPRoleAssignment(userV.User);
                                        else
                                            roleAssigenment = new SPRoleAssignment(web.SiteGroups[userV.LookupValue]);


                                        role.RoleDefinitions.ForEach(r =>
                                        {
                                            roleAssigenment.RoleDefinitionBindings.Add(web.RoleDefinitions[r]);
                                        });

                                        item.RoleAssignments.Add(roleAssigenment);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region unique user

                                    SPFieldUserValue userValue = (SPFieldUserValue)fieldUser.GetFieldValue(item[fieldUser.InternalName].ToString());

                                    SPRoleAssignment roleAssigenment = null;
                                    if (userValue.User != null)
                                        roleAssigenment = new SPRoleAssignment(userValue.User);
                                    else
                                        roleAssigenment = new SPRoleAssignment(web.SiteGroups[userValue.LookupValue]);


                                    role.RoleDefinitions.ForEach(r =>
                                    {
                                        roleAssigenment.RoleDefinitionBindings.Add(web.RoleDefinitions[r]);
                                    });

                                    item.RoleAssignments.Add(roleAssigenment);
                                    #endregion
                                }

                                #endregion
                                break;
                            }
                        case TypeUser.UserProfileOfPropertyUser:
                            {
                                #region User profile
                                SPFieldUser fieldUser = list.Fields[new Guid(role.ColumnUser)] as SPFieldUser;
                                if (!fieldUser.AllowMultipleValues)
                                {
                                    SPFieldUserValue userValue = (SPFieldUserValue)fieldUser.GetFieldValue(item[fieldUser.InternalName].ToString());
                                    if (userValue.User != null)
                                    {
                                        UserProfile profile = profileManager.GetUserProfile(userValue.User.LoginName);
                                        if (profile[role.UserProfilePropertyName] != null)
                                            user = HelperUtilities.ResolveUserFromLogin(web, profile[role.UserProfilePropertyName].Value.ToString());
                                    }
                                }
                                else
                                {
                                    SPFieldUserValueCollection usersValue = (SPFieldUserValueCollection)fieldUser.GetFieldValue(item[fieldUser.InternalName].ToString());
                                    if (usersValue.Count == 1)
                                    {
                                        if (usersValue[0].User != null)
                                        {
                                            UserProfile profile = profileManager.GetUserProfile(usersValue[0].User.LoginName);
                                            if (profile[role.UserProfilePropertyName] != null)
                                                user = HelperUtilities.ResolveUserFromLogin(web, profile[role.UserProfilePropertyName].Value.ToString());
                                        }
                                    }

                                } 
                                #endregion
                                break;
                            }
                        #endregion

                    }

                    if (user != null)
                    {
                        SPRoleAssignment roleAssigenment = new SPRoleAssignment(user);

                        role.RoleDefinitions.ForEach(r =>
                        {
                            if(web.RoleDefinitions.Cast<SPRoleDefinition>().Count(rl=>rl.Name==r)>0)
                            roleAssigenment.RoleDefinitionBindings.Add(web.RoleDefinitions[r]);
                        });

                        item.RoleAssignments.Add(roleAssigenment);
                    }
                }
            }
            item.Update();
        }

        public static void StartEventReceiver(SPList list)
        {

            if (list.EventReceivers.Cast<SPEventReceiverDefinition>().Count(e => { return e.Name == eventReceiverAddName; }) == 0)
            {
                SPEventReceiverDefinition eventReceiver = list.EventReceivers.Add();

                eventReceiver.Name = eventReceiverAddName;
                eventReceiver.Class = typeof(EventReceiverClass).FullName;
                eventReceiver.Assembly = Assembly.GetAssembly(typeof(EventReceiverClass)).FullName;
                eventReceiver.Type = SPEventReceiverType.ItemAdded;
                eventReceiver.Synchronization = SPEventReceiverSynchronization.Synchronous;
                eventReceiver.Update();
            }
            if (list.EventReceivers.Cast<SPEventReceiverDefinition>().Count(e => { return e.Name == eventReceiverUpdateName; }) == 0)
            {
                SPEventReceiverDefinition eventReceiver = list.EventReceivers.Add();

                eventReceiver.Name = eventReceiverUpdateName;
                eventReceiver.Class = typeof(EventReceiverClass).FullName;
                eventReceiver.Assembly = Assembly.GetAssembly(typeof(EventReceiverClass)).FullName;
                eventReceiver.Type = SPEventReceiverType.ItemUpdated;
                eventReceiver.Synchronization = SPEventReceiverSynchronization.Synchronous;
                eventReceiver.Update();
            }

        }

        public static void StopEventReceiver(SPList list)
        {
         SPEventReceiverDefinition eventNewItem=   list.EventReceivers.Cast<SPEventReceiverDefinition>().FirstOrDefault(e => { return e.Name == eventReceiverAddName; });
         if (eventNewItem != null)
             eventNewItem.Delete();

         SPEventReceiverDefinition eventUpdateItem = list.EventReceivers.Cast<SPEventReceiverDefinition>().FirstOrDefault(e => { return e.Name == eventReceiverUpdateName; });
         if (eventUpdateItem != null)
             eventUpdateItem.Delete();
        }

        public static bool IsStartedEventReceiver(SPList list)
        {
            return list.EventReceivers.Cast<SPEventReceiverDefinition>().Count
                (e => { return (e.Name == eventReceiverAddName) || (e.Name == eventReceiverUpdateName); }) == 2;

        }
    }
}
