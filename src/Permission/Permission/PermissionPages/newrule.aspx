<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>


<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="newrule.aspx.cs" Inherits="Permission.Layouts.Permission.newrule"  %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/15/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/15/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/15/InputFormControl.ascx" %>


<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
<script src="jquery-1.8.2.min.js"></script>
       <script type="text/javascript">


           window.addEventListener("load", function () {

            var TypeUserList = '#<%= TypeUserList.ClientID %>';
            var UserProfileBox = '#userProperties';
            var StaticUserBox = '#staticProperties';
            var columnAndUserProfileBox = '#column_userProfileProperties';
               //static uset
            $(TypeUserList).find('input')[0].onclick = function () {
                $(columnAndUserProfileBox).hide();
                $(StaticUserBox).show();

            };

           //user from column
            $(TypeUserList).find('input')[1].onclick = function () {
                $(columnAndUserProfileBox).show();
                $(UserProfileBox).hide();
                $(StaticUserBox).hide();

            };
               //user from user profile
            $(TypeUserList).find('input')[2].onclick = function () {
                $(columnAndUserProfileBox).show();
                $(UserProfileBox).show();
                $(StaticUserBox).hide();


            };
               //static uset
            if ($(TypeUserList).find('input')[0].checked) {
                $(columnAndUserProfileBox).hide();
                $(StaticUserBox).show();
            }
               //user from column
            if ($(TypeUserList).find('input')[1].checked) {
                $(columnAndUserProfileBox).show();
                $(UserProfileBox).hide();
                $(StaticUserBox).hide();
            }
               //user from user profile
            if ($(TypeUserList).find('input')[2].checked) {
                $(columnAndUserProfileBox).show();
                $(UserProfileBox).show();
                $(StaticUserBox).hide();
            }


        });
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    

    <table class="propertysheet" border="0" width="100%" cellspacing="0" cellpadding="0" >
         
        <%-- Title --%>
        <wssuc:InputFormSection ID="InputFormSection1" runat="server"
       Title="<%$Resources:PermissionPages,title%>"
       Description="<%$Resources:PermissionPages,title_desc%>" >
     <template_inputformcontrols>

          <wssuc:InputFormControl runat="server" LabelText="<%$Resources:PermissionPages,title_ind%>">
              <Template_Control>       
               <asp:TextBox ID="TextBoxTitle" runat="server"></asp:TextBox> <br />
<asp:RequiredFieldValidator  ControlToValidate="TextBoxTitle" ValidationGroup="ValidateGroup"  runat="server" ErrorMessage="<%$Resources:PermissionPages,error_required%>"></asp:RequiredFieldValidator>
              </Template_Control>
          </wssuc:InputFormControl>
     </template_inputformcontrols>
</wssuc:InputFormSection>
        

        <%-- Type d'action 
         <wssuc:InputFormSection ID="InputFormSection2" runat="server"
       Title="<%$Resources:PermissionPages,transaction%>"
       Description="<%$Resources:PermissionPages,transaction_desc%>" >
     <template_inputformcontrols>

          <wssuc:InputFormControl runat="server" LabelText="<%$Resources:PermissionPages,transaction_ind%>">
              <Template_Control>       

 <asp:DropDownList ID="ActionList" runat="server">
        <asp:ListItem Selected="True" Text="<%$Resources:PermissionPages,add_item%>" Value="1"></asp:ListItem>
     <asp:ListItem Text="<%$Resources:PermissionPages,update_item%>" Value="2"></asp:ListItem>
     <asp:ListItem Text="<%$Resources:PermissionPages,add_update_item%>" Value="3"></asp:ListItem>
    </asp:DropDownList>

              </Template_Control>
          </wssuc:InputFormControl>
     </template_inputformcontrols>
</wssuc:InputFormSection>
            --%>



        <%-- set Permission --%>

          <wssuc:InputFormSection ID="InputFormSection3" runat="server"
       Title="<%$Resources:PermissionPages,permission%>"
       Description="<%$Resources:PermissionPages,permission_desc%>" >
     <template_inputformcontrols>

          <wssuc:InputFormControl runat="server" LabelText="<%$Resources:PermissionPages,permission_ind%>">
              <Template_Control>       

                   
      <table width="100%">
                      <tr>
                          <td valign="top">
                       
                              <asp:RadioButtonList ID="TypeUserList" runat="server">
                                <asp:ListItem Selected="True"  Text="<%$Resources:PermissionPages,property_static_user%>" Value="1"></asp:ListItem>
                             <asp:ListItem   Text="<%$Resources:PermissionPages,property_column%>" Value="2"></asp:ListItem>
                            <asp:ListItem  Text="<%$Resources:PermissionPages,property_user_profile%>" Value="3"></asp:ListItem>
     
                              </asp:RadioButtonList>
                              <br />
                              <%--propriété--%>
                                <div id="staticProperties">
                                      <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:PermissionPages,property_static_user%>" /><br />
                                         
                                    <SharePoint:PeopleEditor AllowEmpty="false" MultiSelect="false" SelectionSet="User,SPGroup" ID="StaticUsers" runat=server></SharePoint:PeopleEditor>
                                </div>

                              <div id="column_userProfileProperties" style="width:200px">
                              <table cellpaddin="0" cellspacing="0">
                                  <tr>
                                         <td valign="top">
                              
                              <div id="userProperties">
                                  <table width="100%" cellpadding="0" cellspacing="0">
                                  <tr>
                                      <td valign="top">
                                           <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:PermissionPages,property_user_profile%>" /><br />
                                            
                                           <asp:DropDownList  DataTextField="DisplayName" DataValueField="Name"  ID="ComboBoxPropertiesUser"  runat="server">

                                            </asp:DropDownList>

                                      </td>
                                      <td valign="top" style="padding-left:5px">
                                          <br />
                                          <strong>
                                          <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:PermissionPages,from%>" />
                             </strong>  
                                      </td>
                                  </tr>
                              </table>

                              </div>
                          </td>
                         <td valign="top">
                            <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:PermissionPages,columns%>" /><br />
                               
                                           <asp:DropDownList DataTextField="Title" DataValueField="Id"  ID="ComboBoxColumn"  runat="server">

                                            </asp:DropDownList>
                         </td>

                                  </tr>
                              </table>
                                  </div>
                          </td>
                       
                          <td valign="top" style="width:200px">
                               <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:PermissionPages,roles%>" /><br />

                              <asp:CheckBoxList ID="CheckBoxListRoles" DataTextFiell="Name" DataValueField="Name" runat="server"></asp:CheckBoxList>
                          </td>
                      </tr>
                  </table>
<asp:Label runat="server" ForeColor="Red" ID="ErrorRuleLabel"></asp:Label>
   <p align="right">
        <asp:Button ID="AddRule" runat="server" OnClick="AddRule_Click" Text="<%$Resources:wss,AclEditor_AddUserButton%>" />
    </p>
   

              </Template_Control>
          </wssuc:InputFormControl>
     </template_inputformcontrols>
</wssuc:InputFormSection>

      <%-- List of permission --%>
         <wssuc:InputFormSection ID="InputFormSection5" runat="server"
       Title="<%$Resources:PermissionPages,permission_list%>"
       Description="<%$Resources:PermissionPages,permission_list_desc%>" >
     <template_inputformcontrols>

          <wssuc:InputFormControl runat="server" LabelText="">
              <Template_Control>       

                   <asp:Repeater OnItemCommand="RepeaterPermission_ItemCommand" ID="RepeaterPermission" runat="server">
        <HeaderTemplate>
            <table width="100%">
                
            
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("Users") %>
                </td>
                 <td>
                    <%# Eval("Roles") %>
                </td>

                <td>
                
                <asp:LinkButton runat="server">
                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:wss,AclEditor_RemoveUserButton%>" /></asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            
            </table>
        </FooterTemplate>
    </asp:Repeater>

    <asp:Label Visible="false" runat="server" ForeColor="Red" Text="<%$Resources:PermissionPages,error_emptyList %>" ID="LabelErrorEmpty"></asp:Label>
              </Template_Control>
          </wssuc:InputFormControl>
     </template_inputformcontrols>
</wssuc:InputFormSection>


      <%-- Héritage --%>
         <wssuc:InputFormSection ID="InputFormSection4" runat="server"
       Title="<%$Resources:PermissionPages,stop_heritage%>"
       Description="<%$Resources:PermissionPages,stop_heritage_desc%>" >
     <template_inputformcontrols>

          <wssuc:InputFormControl runat="server" LabelText="<%$Resources:PermissionPages,stop_heritage_ind%>">
              <Template_Control>       

 <asp:DropDownList ID="HeritageList" runat="server">
        <asp:ListItem Selected="True" Text="<%$Resources:PermissionPages,with_heritage%>" Value="1"></asp:ListItem>
     <asp:ListItem Text="<%$Resources:PermissionPages,without_heritage%>" Value="2"></asp:ListItem>
    </asp:DropDownList>

              </Template_Control>
          </wssuc:InputFormControl>
     </template_inputformcontrols>
</wssuc:InputFormSection>


        </table>
    
         <wssuc:ButtonSection runat="server" ShowStandardCancelButton="true">
    <template_buttons>
       <asp:PlaceHolder ID="PlaceHolder1" runat="server">               
           <asp:Button id="validate" UseSubmitBehavior="false" ValidationGroup="ValidateGroup" OnClick="Validate_Click" runat="server" class="ms-ButtonHeightWidth" 
                       Text="<%$Resources:wss,multipages_okbutton_text%>"  />
       </asp:PlaceHolder>
    </template_buttons>
</wssuc:ButtonSection>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:PermissionPages,page_title_newrule%>" />
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
<asp:Literal ID="Literal2" runat="server" Text="<%$Resources:PermissionPages,page_title_newrule%>" />
</asp:Content>
