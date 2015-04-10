<%@ Page Title="Topics" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="topics.aspx.cs" Inherits="MathSolverWebsite.TopicPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <link rel="stylesheet" type="text/css" href="Content/css/mlogica-practice.css" />
    <style type="text/css">
        * {
            box-sizing: border-box;
        }
    </style>
    <link rel="stylesheet" type="text/css" href="Content/css/mlogica-def.css" />

    <script>
        $(document).ready(function () {
            $("#left-help").css("height", $("#right-help").height() + 50);
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
  <div class="w-section" style="overflow-y: hidden !important;">
    <div class="w-row help">
      <div class="w-col w-col-4 help-eft" style="margin-left: -10px; margin-top: -20px;">
        <div class="topic-section" id="left-help">
          <h2>Algebra</h2>
          <a class="w-inline-block link-help" href="practice.aspx">
            <div class="link-backtext">&lt;</div>
          </a>
        </div>
      </div>
      <div class="w-col w-col-8 help" id="right-help" style="margin-top: 20px; margin-bottom: 20px;">
        <div>
          <h4>Equations</h4>
        </div>
        <div class="slink-div"><a class="specific-link" href="/prac/topic.aspx">Absolute Value</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="/prac/topic.aspx">Cubics</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="#">Exponential Equations</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="#">Factors Solving</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="#">Fraction Equations</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="#">Linear Equations</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="#">Logarithmic Equations</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="#">Polynomials</a>
        </div>
        <div>
          <h4>Inequalities</h4>
          <div class="slink-div"><a class="specific-link" href="#">Absolute Value Inequalities</a>
          </div>
          <div class="slink-div"><a class="specific-link" href="#">Compound Inequalities</a>
          </div>
          <div class="slink-div"><a class="specific-link" href="#">Fractional Inequalities</a>
          </div>
          <div class="slink-div"><a class="specific-link" href="#">Linear Inequalities</a>
          </div>
          <div class="slink-div"><a class="specific-link" href="#">Polynomial Inequalities</a>
          </div>
          <div class="slink-div"><a class="specific-link" href="#">Quadratic Inequalities</a>
          </div>
        </div>
        <div>
          <h4>Powers and Root Equations</h4>
          <div class="slink-div"><a class="specific-link" href="#">Complex Root Equations</a>
          </div>
          <div class="slink-div"><a class="specific-link" href="#">DeMovire’s Theorem</a>
          </div>
          <div class="slink-div"><a class="specific-link" href="#">Solving Power and Root Equations</a>
          </div>
        </div>
        <div>
          <h4>Quadratics</h4>
          <div class="slink-div"><a class="specific-link" href="#">Solve through substitution</a>
            <div>
              <h4>Systems of Equations</h4>
              <div class="slink-div"><a class="specific-link" href="#">Elimination</a>
              </div>
              <div class="slink-div"><a class="specific-link" href="#">Substiution</a>
              </div>
            </div>
          </div>
        </div>
        <h4>Simplifying</h4>
        <div class="slink-div"><a class="specific-link" href="#">Algebra Operations</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="#">Approximate Answers</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="#">Expand Expressions</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="#">Complex Numbers</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="#">Polynomial Division</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="#">Fractions</a>
        </div>
        <h4>Simplifying</h4>
        <div class="slink-div"><a class="specific-link" href="#">Composing Functions</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="#">Defining Functions</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="#">Composing Functions</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="#">Defining Functions</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="#">Composing Functions</a>
        </div>
        <div class="slink-div"><a class="specific-link" href="#">Defining Functions</a>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
