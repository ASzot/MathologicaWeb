<%@ Page Title="Practice" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="practice.aspx.cs" Inherits="MathSolverWebsite.PracticePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

    <meta name="description" content="Practice problems ranging from algebra to higher level calculus, provides the question, answer, process, and learning for free." />

    <link rel="stylesheet" type="text/css" href="Content/css/mlogica-practice.css" />
    <style type="text/css">
        .w-col-4 {
            box-sizing: border-box;
        }
        .hlep {
            box-sizing: border-box;
        }
    </style>

    <!-- Google analytics -->
    <script>
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
            m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

        ga('create', 'UA-56848508-1', 'auto');
        ga('send', 'pageview');
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
  <div class="w-section section">
    <div class="ws-container">
      <h1 class="heading-1" style="margin-bottom: 20px">Practice</h1>
      <div class="w-row">
        <div class="w-col w-col-4 link-practice">
            <a class="wordlink-help" href="topics?tn=Algebra">  
                <div class="hlep">
                    <div>
                        Algebra
                    </div>
                </div>
            </a>
        </div>
        <div class="w-col w-col-4 link-practice">
            <a class="wordlink-help" href="topics?tn=Trigonometry">
              <div class="hlep">
                <div>
                    Trig
                </div>
              </div>
            </a>
        </div>
        <div class="w-col w-col-4 link-practice">
            <a class="wordlink-help" href="topics?tn=Pre-Calc">  
                <div class="hlep">
                    <div>
                        Pre-Calc
                    </div>
                </div>
            </a>
        </div>
      <div class="w-row">
        <div class="w-col w-col-4 link-practice">
            <a class="wordlink-help" href="topics?tn=Calculus">  
                <div class="hlep">
                    <div>
                        Calculus
                    </div>
                </div>
            </a>
        </div>
        <div class="w-col w-col-4 link-practice">
            <a class="wordlink-help" href="topics?tn=Linear+Algebra">  
                <div class="hlep">
                    <div>
                        Lin Alg
                    </div>
                </div>
            </a>
        </div>
        <div class="w-col w-col-4 link-practice">
            <a class="wordlink-help" href="topics?tn=Advanced+Calculus">  
                <div class="hlep hlep-larger">
                    <div>
                        Adv Calculus
                    </div>
                </div>
            </a>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
