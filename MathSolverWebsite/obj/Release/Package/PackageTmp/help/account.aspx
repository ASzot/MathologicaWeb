<%@ Page Title="Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="account.aspx.cs" Inherits="MathSolverWebsite.help.AccountPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Content/css/mlogica-practice.css" />
    <style type="text/css">
        .body {
            overflow-y: auto;
            font-size: 25px;
        }
        .pob-problem div {
            cursor: pointer;
            font-size: 25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="w-section">
        <div class="w-container" style="max-width: 920px;">
            <div class="sample">
                <h1 class="heading-1">Account Help</h1>
                <h2 class="heading-2">Account Creation</h2>
                <p>
                    To create an account, navigate to the calculator page and select the “Sign up” button. After 

                    clicking, fill in your email address and desired password in the boxes provided. Retype your 

                    password in the “confirm password” box and select the “Sign up” button again. Congratulations, 

                    you have created your account!
                </p>
                
                <h2 class="heading-2">Logging In</h2>
                <p>
                    To login, simply navigate to the calculator page and select the “Login” button in the bottom right

                    corner. After doing so, input your account email and password, then select “Login” to accesses 

                    your account.
                </p>

                <h2 class="heading-2">Account Features</h2>
                <p>
                    Creating an account with Mathologica allows you to save the problems you input into the 

                    calculator, should you choose to do so. To save a problem, simply input your problem into the 

                    calculator, select what you wish to solve for and click “Solve”. After doing so, select the save 

                    icon <img src="../Images/SaveIcon.png" height="20" width="20" /> underneath the solution to save the problem.

                    To access these problems at a later date, simply sign in to your account and select the “You” 

                    button in the bottom right corner of the calculator page. From there you can see your input, date 

                    created, and you have the choice to delete the problem.
                </p>

                <h2 class="heading-2">Changing Password & Deleting Account:</h2>
                <p style="margin-bottom: 150px;">
                    Navigate to the calculator page and select the button in the bottom right displaying your email. 

                    Select the “Settings” button. From here you may input your current password, then your new 

                    password. Type in your new password again in the “Confirm new password” box and select the 

                    “Change password” button at the bottom to change your password. From this page, should you 

                    choose to, you may delete your account simply by pressing the “Delete Account” button at the 

                    bottom of the page.
                </p>
            </div>
        </div>
    </div>
</asp:Content>
