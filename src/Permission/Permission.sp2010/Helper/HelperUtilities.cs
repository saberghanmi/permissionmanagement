using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Permission.Helper
{
   public static class HelperUtilities
    {
       public static string GetResources(string resourceName, uint language)
       {
           return SPUtility.GetLocalizedString("$Resources:" + resourceName, "Management.Permission", language);
       }

       public static string ResolvePrincipaleFromLogin(SPWeb web, string login)
       {
           
           SPPrincipalInfo principalInfo= SPUtility.ResolvePrincipal(web, login, SPPrincipalType.All, SPPrincipalSource.All, null, false);
           return principalInfo.DisplayName;
       }

       public static SPUser ResolveUserFromLogin(SPWeb web, string login)
       {
           try
           {
               SPUser user = web.EnsureUser(login);
               return user;
           }
           catch (SPException ex)
           {
               return null;
           }
       }

    }
}
