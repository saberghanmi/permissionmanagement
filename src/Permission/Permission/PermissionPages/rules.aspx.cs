using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Permission.Helper;
using Microsoft.SharePoint.Utilities;
using System.Web;
using System.Web.UI.WebControls;
using Permission.Classes;
using System.Collections.Generic;
using Microsoft.Office.Server.UserProfiles;
using System.Linq;
using Permission.Classes.Models.Rules;
namespace Permission.Layouts.Permission
{
    public partial class rules : ModulePage
    {
        #region static values
		
        internal static readonly string urlWeb = SPContext.Current.Web.Url;
        internal static readonly string addString = HelperUtilities.GetResources("add_item", SPContext.Current.Web.Language);
        internal static readonly string editString = HelperUtilities.GetResources("update_item", SPContext.Current.Web.Language);
        internal static readonly string addEditString = HelperUtilities.GetResources("add_update_item", SPContext.Current.Web.Language);
        internal static readonly string withHeritageString = HelperUtilities.GetResources("with_heritage", SPContext.Current.Web.Language);
        internal static readonly string withoutHeritageString = HelperUtilities.GetResources("without_heritage", SPContext.Current.Web.Language);
        internal static readonly string from = HelperUtilities.GetResources("from", SPContext.Current.Web.Language);
        internal static CorePropertyManager corePropertyManager;

        internal static readonly string error_column = HelperUtilities.GetResources("error_column", SPContext.Current.Web.Language);
        internal static readonly string error_userProfile = HelperUtilities.GetResources("error_userProfile", SPContext.Current.Web.Language);
        internal static readonly string error_user_profile_noProperty = HelperUtilities.GetResources("error_user_profile_noProperty", SPContext.Current.Web.Language);

        internal static readonly string resume_listner = HelperUtilities.GetResources("resume_listner", SPContext.Current.Web.Language);
        internal static readonly string suspend_listner = HelperUtilities.GetResources("suspend_listner", SPContext.Current.Web.Language);
        
 
	    #endregion
        
        
        private string _error
        {
            set
            {
                ErrorLabel.Text = value;
            }
        }

        private bool _isStartedEvent {
            get
            {
                return (bool)ViewState["IsStarted"];
            }
            set {
                if (value)
                {
                    ListnerButton.Text = suspend_listner;
                }
                else
                {
                    ListnerButton.Text = resume_listner;
                }

                ViewState["IsStarted"] = value;
            }
        }

        private string _listId
        {
            get { return ViewState["ListId"].ToString(); }
            set { ViewState["ListId"] = value; }
        }

        protected override void LoadData()
        {
            base.LoadData();

            if (!this.Request.QueryString.AllKeys.Contains(Constante.listId))
            {
                return;
            }
            string listId = this.Request.QueryString[Constante.listId];
            _listId = listId;

            #region ToolBar Button
            string urlNewRule = SPContext.Current.Web.Url + "/PermissionPages/newrule.aspx?"+Constante.listId+"=" +
                listId + "&Source=" + SPEncode.UrlEncode(HttpContext.Current.Request.Url.AbsoluteUri);


            ((ToolBarButton)idAddField).NavigateUrl = urlNewRule + "&" + Constante.TransactionGet + "=" + (int)Transaction.Add;
            ((ToolBarButton)idUpdateFiels).NavigateUrl = urlNewRule + "&" + Constante.TransactionGet + "=" + (int)Transaction.Edit;
            ((ToolBarButton)idAddUpdateField).NavigateUrl = urlNewRule + "&" + Constante.TransactionGet + "=" + (int)Transaction.AddEdit;
            #endregion

            #region User profile properties
            try
            {
                SPServiceContext context = SPServiceContext.GetContext(SPContext.Current.Site);
                UserProfileConfigManager configManager = new UserProfileConfigManager(context);

                ProfilePropertyManager profileManager = configManager.ProfilePropertyManager;


                corePropertyManager = profileManager.GetCoreProperties();
                
            }
            catch (UserProfileApplicationNotAvailableException ex)
            {
                _error = ex.Message;
            }

            #endregion

            SPWeb web = SPContext.Current.Web;
           
            SettingsLink.HRef=web.Url+"/_layouts/listedit.aspx?List="+listId;

            SPList list = web.Lists[new Guid(listId)];

            //verify permission user
            list.CheckPermissions(SPBasePermissions.ManageLists);

            PermissionSettings settings = new PermissionSettings(list);

            
            List<RuleItem> listRules = new List<RuleItem>();
            if (settings.PermissionPersisted != null)
            {
                //create rules
                foreach (RulePermission rule in settings.PermissionPersisted.Rules)
                {
                    listRules.Add(new RuleItem(list, rule));
                }

                RulesList.DataSource = listRules;
                RulesList.DataBind();
            }
            else
            {
                //start listner
                bool oldparametre = web.AllowUnsafeUpdates;
                web.AllowUnsafeUpdates = true;
                ServiceEventReceiver.StartEventReceiver(list);
                web.AllowUnsafeUpdates = oldparametre;
            }

            //Set Permission Persted Object
            PermissionPersisted permissionPersisted = settings.PermissionPersisted;
            VerifyToolBarButton(permissionPersisted);
            ViewState["listRules"] = listRules;
            ViewState["permissionPersisted"] = permissionPersisted;


            //Set IsStared value
            _isStartedEvent = ServiceEventReceiver.IsStartedEventReceiver(list);
        }

        
        protected void RulesList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            SPWeb web = SPContext.Current.Web;
            string listId = _listId;
            SPList list = web.Lists[new Guid(listId)];
            List<RuleItem> listRules=ViewState["listRules"] as List<RuleItem>;
            listRules.RemoveAt(e.Item.ItemIndex);
            RulesList.DataSource = listRules;
            RulesList.DataBind();

            PermissionPersisted permissionPersisted = ViewState["permissionPersisted"] as PermissionPersisted;

            permissionPersisted.Rules.RemoveAt(e.Item.ItemIndex);

            PermissionSettings.SaveParametres(list, permissionPersisted);

            VerifyToolBarButton(permissionPersisted);
        }

        protected void VerifyToolBarButton(PermissionPersisted permissionPersisted)
        {
            if (permissionPersisted != null)
            {
                ((ToolBarButton)idAddField).Visible = permissionPersisted.Rules.Count(r => { return r.Transaction == Transaction.Add; }) == 0;
                ((ToolBarButton)idUpdateFiels).Visible = permissionPersisted.Rules.Count(r => { return r.Transaction == Transaction.Edit; }) == 0;
                ((ToolBarButton)idAddUpdateField).Visible = permissionPersisted.Rules.Count(r => { return r.Transaction == Transaction.AddEdit; }) == 0;
            }
      }

        protected void StartStopClick(object sender, EventArgs e)
        {
            SPWeb web = SPContext.Current.Web;
            string listId = _listId;
            SPList list = web.Lists[new Guid(listId)];

            bool started = _isStartedEvent;
            if (started)
                ServiceEventReceiver.StopEventReceiver(list);
            else
                ServiceEventReceiver.StartEventReceiver(list);

            _isStartedEvent = !started;

        }
    }
    [Serializable]
    public class RuleItem
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Transaction { get; set; }
        public string Roles { get; set; }
        public string Heritage { get; set; }
        public string Error { get; set; }

        public RuleItem(SPList list, RulePermission rulePermission)
        {

            this.Title = string.Format("[{0}] {1}", rulePermission.Order, rulePermission.Title);
            this.Url = rules.urlWeb + "/PermissionPages/editrule.aspx?order="+rulePermission.Order;
            switch((int)rulePermission.Transaction)
            {
                case 1: this.Transaction = rules.addString; break;
                case 2: this.Transaction = rules.editString; break;
                case 3: this.Transaction = rules.addEditString; break;
            }

            if (rulePermission.Heritage)
                this.Heritage = rules.withHeritageString;
            else
                this.Heritage = rules.withoutHeritageString;

            string rolesFormat="";
            foreach (RoleAssignement item in rulePermission.RoleAssignements)
            {
                switch ((int)item.TypeUser)
                {
                    case 1: {
                        //
                        rolesFormat += "<strong>[" + HelperUtilities.ResolvePrincipaleFromLogin(SPContext.Current.Web,item.StaticUser) + "]</strong> :";
                        item.RoleDefinitions.ForEach(r => rolesFormat += r + ";");
                        rolesFormat += "<br/>";

                        break; 
                    }
                    case 2:
                        {
                            if (list.Fields.Contains(new Guid(item.ColumnUser)))
                            {
                                rolesFormat += "<strong>[" + list.Fields[new Guid(item.ColumnUser)].Title + "]</strong> :";
                                item.RoleDefinitions.ForEach(r => rolesFormat += r + ";");
                                rolesFormat += "<br/>";
                            }
                            else
                                this.Error = string.Format(rules.error_column, item.ColumnUser);

                            break;
                        }
                    case 3:
                        {

                            if (list.Fields.Contains(new Guid(item.ColumnUser)))
                            {
                                if (rules.corePropertyManager != null)
                                {
                                    CoreProperty property = rules.corePropertyManager.First(p => p.Name == item.UserProfilePropertyName);
                                    if (property != null)
                                    {
                                        rolesFormat += "<strong>[" + property.DisplayName + "]</strong>" + rules.from + "<strong>[" + list.Fields[new Guid(item.ColumnUser)].Title + "]</strong> :";
                                        item.RoleDefinitions.ForEach(r => rolesFormat += r + ";");
                                        rolesFormat += "<br/>";
                                    }
                                    else
                                    {
                                        this.Error = string.Format(rules.error_user_profile_noProperty, item.UserProfilePropertyName);
                                    }
                                }
                                else
                                {
                                    this.Error = string.Format(rules.error_userProfile, item.ColumnUser);
                                }
                            } 
                            else
                                this.Error = string.Format(rules.error_column, item.ColumnUser);

                            break;
                        }
                }

            }
            this.Roles = rolesFormat;

        }
    }
}
