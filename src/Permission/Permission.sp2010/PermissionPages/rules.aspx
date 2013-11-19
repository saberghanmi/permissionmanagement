<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Import Namespace="Permission.Helper" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rules.aspx.cs" Inherits="SharePointProject1.Layouts.Permission.sp2010.rules"  %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" src="~/_controltemplates/ToolBarButton.ascx" %>


<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
     <script type="text/javascript">
         function confirmDelete() {
             var confirmMessage = '<%= HelperUtilities.GetResources("confirm_delete", SPContext.Current.Web.Language) %>';
            var r = confirm(confirmMessage);
            return (r == true);
        }
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table border="0" width="100%" cellspacing="0" cellpadding="0">
	<tr>

		<td align="left" id="mngfieldToobar">
			<wssuc:ToolBar id="onetidMngFieldTB" runat="server">
				<Template_Buttons>
					<wssuc:ToolBarButton runat="server" 
                        Text="<%$Resources:PermissionPages,add_item%>"
						ID="idAddField"
						ToolTip="<%$Resources:PermissionPages,add_item%>"
						ImageUrl="/_layouts/images/newitem.gif?rev=23"
						AccessKey="C" />
                    <wssuc:ToolBarButton runat="server" 
                        Text="<%$Resources:PermissionPages,update_item%>"
						ID="idUpdateFiels"
						ToolTip="<%$Resources:PermissionPages,update_item%>"
						ImageUrl="/_layouts/images/newitem.gif?rev=23"
						AccessKey="C" />
				
                    <wssuc:ToolBarButton runat="server" 
                        Text="<%$Resources:PermissionPages,add_update_item%>"
						ID="idAddUpdateField"
						ToolTip="<%$Resources:PermissionPages,add_update_item%>"
						ImageUrl="/_layouts/images/newitem.gif?rev=23"
						AccessKey="C" />
				</Template_Buttons>
                
			</wssuc:ToolBar>
            
		</td>
        <td>
            <asp:LinkButton runat="server" OnClick="StartStopClick" ID="ListnerButton" ></asp:LinkButton>
        </td>
	</tr>
         <tr>
             <td colspan="2">
                 <asp:Repeater OnItemCommand="RulesList_ItemCommand" ID="RulesList" runat="server">

                     <HeaderTemplate>
                <table width="100%" cellpadding="0" cellspacing="5" border="0" id="onetidMngCytpeRptrTable">
			<colgroup>
				<col width="15%"/>
				<col width="15%"/>
				<col width="55%"/>
				<col width="10%"/>
				<col width="5%"/>
			</colgroup>

                    <tr>
				<th scope="col" class="ms-vh2-nofilter"> <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:PermissionPages,title%>" /></th>
				<th scope="col" class="ms-vh2-nofilter"> <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:PermissionPages,permission_list%>" /></th>
				<th scope="col" class="ms-vh2-nofilter"> <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:PermissionPages,transaction%>" /></th>
                <th scope="col" class="ms-vh2-nofilter"> <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:PermissionPages,stop_heritage%>" /></th>
                        <th></th>
			</tr>
            </HeaderTemplate>

                     <ItemTemplate>
                     <tr>
                         <td valign="top"><a href="<%# Eval("Url") %>"><%# Eval("Title") %></a></td>
                         <td valign="top"><%# Eval("Transaction") %></td>
                         <td valign="top">
                             <%# Eval("Roles") %>
                             <span style="color:red">
                                 <%# Eval("Error") %>
                             </span>
                         </td>
                         <td valign="top">
                             <%# Eval("Heritage") %>
                         </td>
                         <td  valign="top">
                        <asp:LinkButton ID="LinkButton1"  OnClientClick="return confirmDelete()"  runat="server">
                    <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:wss,AclEditor_RemoveUserButton%>" />
                        </asp:LinkButton>
                         </td>
                         </tr>
                         <tr>
                             <td colspan></td>
                         </tr>

                             </ItemTemplate>

                       <FooterTemplate>
                </table>
            </FooterTemplate>
                 </asp:Repeater>

             </td>
         </tr>
    <tr>
        <td colspan="2">
<asp:Label runat="server" ForeColor="Red" ID="ErrorLabel"></asp:Label>
            </td>
    </tr>
         </table>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
<asp:Literal ID="Literal1" runat="server" Text="<%$Resources:PermissionPages,page_title_allrule%>" />
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
    <a runat="server" ID="SettingsLink">
<asp:Literal ID="Literal8" runat="server" Text="<%$Resources:wss,listgeneralsettings_titleintitlearea%>" />
        </a>
    <SharePoint:ClusteredDirectionalSeparatorArrow ID="ClusteredDirectionalSeparatorArrow1" runat="server" />
<asp:Literal ID="Literal2" runat="server" Text="<%$Resources:PermissionPages,page_title_allrule%>" />
</asp:Content>
