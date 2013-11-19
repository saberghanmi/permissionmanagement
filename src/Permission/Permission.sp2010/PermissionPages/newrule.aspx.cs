using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Permission.Helper;
using Microsoft.Office.Server.UserProfiles;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Permission.Classes.Models.Rules;
namespace SharePointProject1.Layouts.Permission.sp2010
{
    public partial class newrule : ModulePage
    {
        #region messages

        internal static string error_exist_rule = HelperUtilities.GetResources("error_rule_exit", SPContext.Current.Web.Language);
        internal static string error_no_role = HelperUtilities.GetResources("error_rule_no_role", SPContext.Current.Web.Language);
        internal static string error_no_static_user = HelperUtilities.GetResources("error_no_static_user", SPContext.Current.Web.Language);
        internal static string from = HelperUtilities.GetResources("from", SPContext.Current.Web.Language);
        #endregion
        private string ErrorRule
        {
            set
            {
                ErrorRuleLabel.Text = value;
            }
        }

        private void ShowErrorLine(bool show)
        {
            LabelErrorEmpty.Visible = show;
        }



        protected override void LoadData()
        {
            base.LoadData();

            SPWeb web = SPContext.Current.Web;
            string listId = this.Request.QueryString[Constante.listId];

            SPList list = web.Lists[new Guid(listId)];

            list.CheckPermissions(SPBasePermissions.ManageLists);

            #region User profile properties
            try
            {
                SPServiceContext context = SPServiceContext.GetContext(web.Site);
                UserProfileConfigManager configManager = new UserProfileConfigManager(context);

                ProfilePropertyManager profileManager = configManager.ProfilePropertyManager;


                CorePropertyManager corePropertyManager = profileManager.GetCoreProperties();

                ComboBoxPropertiesUser.DataSource = corePropertyManager.Where(p => p.Type == "Person");
                ComboBoxPropertiesUser.DataBind();
            }
            catch (UserProfileApplicationNotAvailableException ex)
            {

                TypeUserList.Items[0].Selected = true;
                TypeUserList.Items[1].Enabled = false;
            }

            #endregion


            ComboBoxColumn.DataSource = list.Fields.Cast<SPField>().Where(f => f.Type == SPFieldType.User);
            ComboBoxColumn.DataBind();


            CheckBoxListRoles.DataSource = web.RoleDefinitions;
            CheckBoxListRoles.DataBind();

            List<PermessionLine> lines = new List<PermessionLine>();
            ViewState["lines"] = lines;
        }


        protected void AddRule_Click(object sender, EventArgs e)
        {
            ErrorRule = "";

            List<PermessionLine> lines = ViewState["lines"] as List<PermessionLine>;
            //no static User
            if (int.Parse(TypeUserList.SelectedValue) == 1 && StaticUsers.Accounts.Count == 0)
            {
                ErrorRule = error_no_static_user;
                return;
            }

            //some line exist
            if (lines.Count(l =>
            {
                return
                    (l.TypeUser == int.Parse(TypeUserList.SelectedValue)
                    && l.PropertyUserName == ComboBoxPropertiesUser.SelectedValue
                    && l.ColumnId == ComboBoxColumn.SelectedValue
                    && l.LoginUser == ((StaticUsers.Accounts.Count == 0) ? "" : StaticUsers.Accounts[0].ToString()));
            }) > 0)
            {
                ErrorRule = error_exist_rule;
                return;
            }

            //no role
            if (CheckBoxListRoles.Items.Cast<ListItem>().Count(i => i.Selected) == 0)
            {
                ErrorRule = error_no_role;
                return;
            }

            string roles = "";
            foreach (ListItem item in CheckBoxListRoles.Items)
            {
                if (item.Selected)
                {
                    roles += item.Text + ";";
                }
            }

            roles = roles.Remove(roles.Length - 1);

            lines.Add(new PermessionLine()
            {
                TypeUser = int.Parse(TypeUserList.SelectedValue),
                PropertyUser = ComboBoxPropertiesUser.SelectedItem.Text,
                PropertyUserName = ComboBoxPropertiesUser.SelectedValue,
                LoginUser = ((StaticUsers.Accounts.Count == 0) ? "" : StaticUsers.Accounts[0].ToString()),
                ColumnId = ComboBoxColumn.SelectedValue,
                Column = ComboBoxColumn.SelectedItem.Text,
                Roles = roles
            });

            RepeaterPermission.DataSource = lines;
            RepeaterPermission.DataBind();

            ShowErrorLine(false);
        }

        protected void RepeaterPermission_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            List<PermessionLine> lines = ViewState["lines"] as List<PermessionLine>;

            lines.RemoveAt(e.Item.ItemIndex);
            RepeaterPermission.DataSource = lines;
            RepeaterPermission.DataBind();

        }


        protected void Validate_Click(object sender, EventArgs e)
        {
            List<PermessionLine> lines = ViewState["lines"] as List<PermessionLine>;
            if (lines.Count == 0)
            {
                ShowErrorLine(true);
                return;
            }

            SPWeb web = SPContext.Current.Web;
            string listId = this.Request.QueryString[Constante.listId];
            string transactionId = this.Request.QueryString[Constante.TransactionGet];
            SPList list = web.Lists[new Guid(listId)];
            List<RoleAssignement> roleAssigements = new List<RoleAssignement>();



            lines.ForEach(l =>
            {
                roleAssigements.Add(new RoleAssignement()
                {

                    TypeUser = (TypeUser)l.TypeUser,
                    ColumnUser = l.ColumnId,
                    UserProfilePropertyName = l.PropertyUserName,
                    RoleDefinitions = l.Roles.Split(';').ToList(),
                    StaticUser = l.LoginUser
                });

            });


            RulePermission rule = new RulePermission()
            {
                Title = TextBoxTitle.Text,
                Transaction = (Transaction)int.Parse(transactionId),
                Heritage = HeritageList.SelectedIndex == 0,
                RoleAssignements = roleAssigements
            };

            PermissionService.AddRule(list, rule);

            this.Close();
        }

    }

    [Serializable]
    public class PermessionLine
    {


        public int TypeUser { get; set; }

        public string PropertyUser { get; set; }
        public string PropertyUserName { get; set; }



        public string Column { get; set; }
        public string ColumnId { get; set; }

        public string Roles { get; set; }

        public string LoginUser { get; set; }
        public string Users
        {
            get
            {
                switch (this.TypeUser)
                {
                    case 1: return HelperUtilities.ResolvePrincipaleFromLogin(SPContext.Current.Web, this.LoginUser); break;
                    case 2: return this.Column; break;
                    default: return string.Format("[{0}] {1} [{2}]", this.PropertyUser, newrule.from, this.Column); break;
                }
            }


        }

    }
}
