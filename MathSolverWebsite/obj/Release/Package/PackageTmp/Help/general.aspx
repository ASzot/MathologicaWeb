<%@ Page Title="General" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="general.aspx.cs" Inherits="MathSolverWebsite.help.GeneralPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Content/css/mlogica-practice.css" />
    <style type="text/css">
        .body {
            overflow-y: auto;
            font-size: 25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="w-section">
        <div class="w-container" style="max-width: 920px;">
            <div class="sample">
                <div class="heading-1">General Help</div>
                <p>
                    Mathologica is a free math solving service that allows users to compute various mathematical functions. 
                </p>
                <h1 class="heading-2">Using the Math Solver</h1>
                <p>
                   To begin using Mathologica’s services, travel to the calculator page on the navigation bar at the top of the website. Here, you will see an input box where you can input various types of functions and equations. To use, input any numbers you wish to use on your keyboard and select any math-specific symbols from the symbol tab on the right side of the screen. You can cycle through various symbols by using the tabs at the top of the symbols menu. 
                </p>

                <p>
                    After you have your math inputted into the input box, select from the various operations from the dropdown menu at the bottom of the screen. For example, if you have inputed a quadratic expression, you can select whether you want Mathologica to factor by using the quadratic formula or by completing the square. For more information about how to input more advanced functions such as systems of equations, check the <a href="/multiline">multi-line help page</a>. 
                </p>
                
                <p>
                    Finally, click the solve button on the bottom left of your screen to have Mathologica compute your desired task. The original problem will be restated, followed by various lines of work and an explanation leading to the final answer.  
                </p>

                <p>
                    Mathologica functions very similarly to a graphing calculator, meaning your past problems and work will be shown until cleared using the CLR button located on the bottom toolbar. Also located on this toolbar are a + and - symbol. For more information regarding their use, visit the multi-line help page. 
                </p>

                <p>
                    If you wish to save your problem, click on the save icon after inputting and solving the problem.  Before you can save you must create an account with Mathologica by going to the sign-up page. Creating an account is completely free. 
                </p>

                <p style="margin-bottom: 150px;">
                    Mathologica uses certain technologies to input its math, meaning that you <b>cannot copy and paste problems from another source into the solver.</b> 
                </p>
            </div>
        </div>
    </div>
</asp:Content>
