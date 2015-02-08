<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FormattingTips.aspx.cs" Inherits="MathSolverWebsite.Help.FormattingTips" %>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <p class="titleText">General Help</p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="centeredDiv">
        <ul>
            <li class="helpTopicListItem wallOfText">
                <p class="sectionHeading">Exponents</p>
                <p>To enter exponents type the '^' character (shift+6) on the keyboard.</p>
            </li>
            <li class="helpTopicListItem wallOfText">
                <p class="sectionHeading">Fractions</p>
                <p>
                    To enter fractions type the '/' character. A formatted fraction will then be created. For more information on how to work with fractions go 
                    <a href="GeneralHelp#fractionHelpParagraph">here</a>.
                </p>
            </li>
            <li class="helpTopicListItem wallOfText">
                <p class="sectionHeading">Subscripts</p>
                <p>To enter subscripts for a variable type the '_' (underscore) character. </p>
            </li>
        </ul>
    </div>
</asp:Content>
