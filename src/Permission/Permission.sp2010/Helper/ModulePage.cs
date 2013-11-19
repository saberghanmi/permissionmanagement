using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Permission.Helper
{
    
    public class ModulePage : LayoutsPageBase
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.MasterPageFile = SPContext.Current.Web.MasterUrl;

        }

        protected void Close()
        {
            if ((SPContext.Current != null) && SPContext.Current.IsPopUI)
            {

                this.Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                this.Context.Response.Flush();
                this.Context.Response.End();
            }

            else
            {
                string source = this.Request.QueryString["Source"];
                if (!string.IsNullOrEmpty(source))
                {
                    this.Response.Redirect(source);
                }
                else
                    this.Response.Redirect(SPContext.Current.Web.Url);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        protected virtual void LoadData()
        {


        }





    }
}
