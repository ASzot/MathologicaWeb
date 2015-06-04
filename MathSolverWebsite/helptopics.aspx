<%@ Page Title="Help Topics" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="helptopics.aspx.cs" Inherits="MathSolverWebsite.HelpTopicsPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

    <meta name="description" content="Learn to use the wide range of features of mathologica, the free online math solver for any student." />

    <link rel="stylesheet" type="text/css" href="Content/css/mlogica-practice.css" />
    <style type="text/css">
        .w-col-4 {
            box-sizing: border-box;
        }
        .hlep {
            box-sizing: border-box;
        }

        .hlep {
          background-color: #2ecc71;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
  <div class="w-section section">
    <div class="ws-container">
      <h1 class="heading-1" style="margin-bottom: 20px">Help</h1>
      <div class="w-row">
        <div class="w-col w-col-4 link-practice">
            <a class="wordlink-help" href="/help/general">
                <div class="hlep">
                    <div>General</div>
                </div>
            </a>
        </div>
        <div class="w-col w-col-4 link-practice">
            <a class="wordlink-help" href="/help/faq">
                <div class="hlep">
                    <div>FAQ</div>
                </div>
            </a>
        </div>
        <div class="w-col w-col-4 link-practice">
            <a class="wordlink-help" href="/help/formatting">
                <div class="hlep">
                    <div>Formatting</div>
                </div>
            </a>
        </div>
      </div>
      <div class="w-row">
        <div class="w-col w-col-4 link-practice">
            <a class="wordlink-help" href="/multiline">
                <div class="hlep">
                    <div>Multi-line</div>
                </div>
            </a>
        </div>
        <div class="w-col w-col-4 link-practice">
            <a class="wordlink-help" href="/help/account">
                <div class="hlep">
                    <div>Account</div>
                </div>
            </a>
        </div>
        <div class="w-col w-col-4 link-practice">
            <a class="wordlink-help" href="/help/graphing">
                <div class="hlep">
                    <div>Graphing</div>
                </div>
            </a>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
