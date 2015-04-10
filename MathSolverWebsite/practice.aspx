<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="practice.aspx.cs" Inherits="MathSolverWebsite.PracticePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <link rel="stylesheet" type="text/css" href="Content/css/mlogica-practice.css" />
    <style type="text/css">
        * {
            box-sizing: border-box;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
  <div class="w-section section">
    <div class="ws-container">
      <h1 class="heading-1" style="margin-bottom: 20px">Practice</h1>
      <div class="w-row">
        <div class="w-col w-col-4 link-practice">
          <div class="hlep">
            <div>
              <div><a class="wordlink-help" href="topics.aspx">Algebra</a>
              </div>
            </div>
          </div>
        </div>
        <div class="w-col w-col-4 link-practice">
          <div class="hlep">
            <div>
              <div><a class="wordlink-help" href="topics.aspx">Trig</a>
              </div>
            </div>
          </div>
        </div>
        <div class="w-col w-col-4 link-practice">
          <div class="hlep">
            <div>
              <div><a class="wordlink-help" href="topics.aspx">Pre-Calc</a>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="w-row">
        <div class="w-col w-col-4 link-practice">
          <div class="hlep">
            <div>
              <div><a class="wordlink-help" href="general.html">Lin Alg</a>
              </div>
            </div>
          </div>
        </div>
        <div class="w-col w-col-4 link-practice">
          <div class="hlep">
            <div>
              <div><a class="wordlink-help" href="faq.html">Prob</a>
              </div>
            </div>
          </div>
        </div>
        <div class="w-col w-col-4 link-practice">
          <div class="hlep">
            <div>
              <div><a class="wordlink-help" href="account-creation.html">Calculus</a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
