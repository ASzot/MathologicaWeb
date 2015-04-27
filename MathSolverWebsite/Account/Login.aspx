<%@ Page Title="Login in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="MathSolverWebsite.Account.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">    
    <div class="ws-container">
        <hgroup class="title">
            <div class="heading-1">Log In</div>
        </hgroup>
        <section id="loginForm">
            <asp:Login ID="Login1" runat="server" ViewStateMode="Disabled" DestinationPageUrl="~/" RenderOuterTable="false">
                <LayoutTemplate>
                    <fieldset>
                        <ol class="register-info-list">
                            <li>
                                <asp:Label ID="Label1" runat="server" AssociatedControlID="UserName">User name</asp:Label>
                                <asp:TextBox runat="server" ID="UserName" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="UserName" CssClass="field-validation-error" ErrorMessage="A username is required." />
                            </li>
                            <li>
                                <asp:Label ID="Label2" runat="server" AssociatedControlID="Password">Password</asp:Label>
                                <asp:TextBox runat="server" ID="Password" TextMode="Password" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Password" CssClass="field-validation-error" ErrorMessage="A password is required." />
                            </li>
                            <li>
                                <asp:CheckBox runat="server" ID="RememberMe" />
                                <asp:Label ID="Label3" runat="server" AssociatedControlID="RememberMe" CssClass="checkbox">Remember me?</asp:Label>
                            </li>
                        </ol>
                        <p class="validation-summary-errors">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                        <asp:Button CssClass="register-btn login-space" ID="Button1" runat="server" CommandName="Login" Text="Log in" />
                    </fieldset>
                </LayoutTemplate>
            </asp:Login>
            <p class="register-hint">
                <asp:HyperLink CssClass="to-register register-btn" runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Sign Up</asp:HyperLink>
                if you don't have an account.
            </p>
        </section>
    </div>
</asp:Content>
