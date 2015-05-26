<%@ Page Title="Help Topics" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="helptopics.aspx.cs" Inherits="MathSolverWebsite.HelpTopicsPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
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
          <div class="hlep">
            <div>
              <div><a class="wordlink-help" href="/help/general">General</a>
              </div>
            </div>
          </div>
        </div>
        <div class="w-col w-col-4 link-practice">
          <div class="hlep">
            <div>
              <div><a class="wordlink-help" href="/help/faq">FAQ</a>
              </div>
            </div>
          </div>
        </div>
        <div class="w-col w-col-4 link-practice">
          <div class="hlep">
            <div>
              <div><a class="wordlink-help" href="/help/formatting">Formatting</a>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="w-row">
        <div class="w-col w-col-4 link-practice">
          <div class="hlep">
            <div>
              <div><a class="wordlink-help" href="/help/changelog">Change Log</a>
              </div>
            </div>
          </div>
        </div>
        <div class="w-col w-col-4 link-practice">
          <div class="hlep">
            <div>
              <div><a class="wordlink-help" href="/help/account">Account</a>
              </div>
            </div>
          </div>
        </div>
        <div class="w-col w-col-4 link-practice">
          <div class="hlep">
            <div>
              <div><a class="wordlink-help" href="/help/graphing">Graphing</a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
