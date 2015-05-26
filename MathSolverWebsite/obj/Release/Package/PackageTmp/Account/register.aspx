<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="MathSolverWebsite.Account.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
        .body {
            overflow-y: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="ws-container size-crit">
        <hgroup class="title">
            <div class="heading-1">Register</div>
            <h1 class="heading-2">Use Mathologica to the full potential.</h1>
        </hgroup>

        <asp:CreateUserWizard RequireEmail="false" runat="server" ID="RegisterUser" ViewStateMode="Disabled" OnCreatedUser="RegisterUser_CreatedUser">
            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="wizardStepPlaceholder" />
                <asp:PlaceHolder runat="server" ID="navigationPlaceholder" />
            </LayoutTemplate>
            <WizardSteps>
                <asp:CreateUserWizardStep runat="server" ID="RegisterUserWizardStep">
                    <ContentTemplate>

                        <p class="validation-summary-errors">
                            <asp:Literal runat="server" ID="ErrorMessage" />
                        </p>

                        <fieldset>
                            <legend>Info</legend>
                            <ol class="register-info-list">
                                <li>
                                    <asp:Label ID="Label1" runat="server" AssociatedControlID="UserName">Email</asp:Label>
                                    <asp:TextBox runat="server" ID="UserName" />
                                    <asp:RegularExpressionValidator runat="server" ControlToValidate="UserName" ErrorMessage="Invalid email..."
                                        ValidationExpression="^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$" />
                                </li>
                                <li>
                                    <asp:Label ID="Label3" runat="server" AssociatedControlID="Password">Password</asp:Label>
                                    <asp:TextBox runat="server" ID="Password" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="Password"
                                        CssClass="field-validation-error" ErrorMessage="The password field is required." />
                                    <p class="message-info">
                                        Passwords are required to be a minimum of <%: Membership.MinRequiredPasswordLength %> characters.
                                    </p>
                                </li>
                                <li>
                                    <asp:Label ID="Label4" runat="server" AssociatedControlID="ConfirmPassword">Confirm password</asp:Label>
                                    <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ConfirmPassword"
                                         CssClass="field-validation-error" Display="Dynamic" ErrorMessage="The confirm password field is required." />
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                                         CssClass="field-validation-error" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />
                                </li>
                            </ol>
                            <asp:Button CssClass="register-btn signup-space" ID="Button1" runat="server" CommandName="MoveNext" Text="Sign Up" />
                        </fieldset>
                    </ContentTemplate>
                    <CustomNavigationTemplate />
                </asp:CreateUserWizardStep>
            </WizardSteps>
        </asp:CreateUserWizard>
        
    </div>
</asp:Content>
