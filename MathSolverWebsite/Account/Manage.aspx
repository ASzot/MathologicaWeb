<%@ Page Title="Manage Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="manage.aspx.cs" Inherits="MathSolverWebsite.Account.Manage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <%
        if (User.Identity.IsAuthenticated)
            Response.Write("<link rel='stylesheet' type='text/css' href='/Content/css/mlogica-def.css' />");
    %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="w-container">
        <hgroup class="title">
            <div class="heading-1">Manage Account</div>
        </hgroup>

        <section id="passwordForm">
            <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
                <p class="message-success"><%: SuccessMessage %></p>
            </asp:PlaceHolder>

            <p class="heading-2" style="text-align: center">
                <%
                    if (User.Identity.IsAuthenticated) 
                        Response.Write("You're logged in as <strong>" + User.Identity.Name + "</strong>");
                %>
            </p>

            <asp:PlaceHolder runat="server" ID="changePassword" Visible="false">
                <h1 class="heading-2">Change password</h1>
                <asp:ChangePassword ID="ChangePassword1" runat="server" CancelDestinationPageUrl="~/" ViewStateMode="Disabled" RenderOuterTable="false" SuccessPageUrl="Manage?m=ChangePwdSuccess">
                    <ChangePasswordTemplate>
                        <p class="validation-summary-errors">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                        <fieldset class="changePassword">
                            <ol class="register-info-list">
                                <li>
                                    <asp:Label runat="server" ID="CurrentPasswordLabel" AssociatedControlID="CurrentPassword">Current password</asp:Label>
                                    <asp:TextBox runat="server" ID="CurrentPassword" CssClass="passwordEntry" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="CurrentPassword"
                                        CssClass="field-validation-error" ErrorMessage="The current password field is required."
                                        ValidationGroup="ChangePassword" />
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="NewPasswordLabel" AssociatedControlID="NewPassword">New password</asp:Label>
                                    <asp:TextBox runat="server" ID="NewPassword" CssClass="passwordEntry" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="NewPassword"
                                        CssClass="field-validation-error" ErrorMessage="The new password is required."
                                        ValidationGroup="ChangePassword" />
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="ConfirmNewPasswordLabel" AssociatedControlID="ConfirmNewPassword">Confirm new password</asp:Label>
                                    <asp:TextBox runat="server" ID="ConfirmNewPassword" CssClass="passwordEntry" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ConfirmNewPassword"
                                        CssClass="field-validation-error" Display="Dynamic" ErrorMessage="Confirm new password is required."
                                        ValidationGroup="ChangePassword" />
                                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                                        CssClass="field-validation-error" Display="Dynamic" ErrorMessage="The new password and confirmation password do not match."
                                        ValidationGroup="ChangePassword" />
                                </li>
                            </ol>
                            <asp:Button CssClass="register-btn login-space" ID="Button2" runat="server" CommandName="ChangePassword" Text="Change password" ValidationGroup="ChangePassword" />
                        </fieldset>
                    </ChangePasswordTemplate>
                </asp:ChangePassword>
            </asp:PlaceHolder>

            <div style="width: 100%">
                <asp:Button runat="server" CssClass="del-btn" ID="deleteAccountBtn" Text="Delete Account" OnClick="deleteAccountBtn_Click" />
            </div>
        </section>
    </div>
</asp:Content>
