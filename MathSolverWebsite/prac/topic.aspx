<%@ Page Title="Topic Name" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="topic.aspx.cs" Inherits="MathSolverWebsite.prac.TopicPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="https://cdn.mathjax.org/mathjax/latest/MathJax.js?config=AM_HTMLorMML"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Content/css/mlogica-practice.css" />

    <style>
        .sample {
          min-height: 200px;
          margin-top: 10px;
          padding: 1px 10px 10px;
          border-radius: 6px;
          font-size: 20px;
          background-color: rgb(253, 253, 253);
        }

        .sample-desc-txt {

        }

        h3 {
            margin-bottom: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="w-section">
        <div class="w-container" style="max-width: 920px;">
            <div id="contentDiv" runat="server" class="sample">

            </div>
        </div>
    </div>
</asp:Content>
