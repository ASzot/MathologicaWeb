<%@ Page Title="Practice" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="practice.aspx.cs" Inherits="MathSolverWebsite.PracticePage" %>
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
              <div><a class="wordlink-help" href="topics?tn=Algebra">Algebra</a>
              </div>
            </div>
          </div>
        </div>
        <div class="w-col w-col-4 link-practice">
          <div class="hlep">
            <div>
              <div><a class="wordlink-help" href="topics?tn=Trigonometry">Trig</a>
              </div>
            </div>
          </div>
        </div>
        <div class="w-col w-col-4 link-practice">
          <div class="hlep">
            <div>
              <div><a class="wordlink-help" href="topics?tn=Pre-Calc">Pre-Calc</a>
              </div>
            </div>
          </div>
        </div>
      </div>
        <div class="w-col w-col-4 link-practice">
          <div class="hlep">
            <div>
              <div><a class="wordlink-help" href="topics?tn=Calculus">Calculus</a>
              </div>
            </div>
          </div>
        </div>
      <div class="w-row">
        <div class="w-col w-col-4 link-practice">
          <div class="hlep">
            <div>
              <div><a class="wordlink-help" href="topics?tn=Linear+Algebra">Lin Alg</a>
              </div>
            </div>
          </div>
        </div>
        <div class="w-col w-col-4 link-practice">
          <div class="hlep" style="padding-top: 60px;">
            <div>
              <div><a class="wordlink-help" href="topics?tn=Advanced+Calculus">Adv Calculus</a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
