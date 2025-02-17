﻿<%@ Page Title="Topics" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="topics.aspx.cs" Inherits="MathSolverWebsite.TopicsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <link rel="stylesheet" type="text/css" href="Content/css/mlogica-practice.css" />
    <style type="text/css">
        * {
            box-sizing: border-box;
        }
        @media only screen and (max-device-width: 480px) {
            .header {
                margin-top: -50px;
            }
        }
    </style>
    <link rel="stylesheet" type="text/css" href="Content/css/mlogica-def.css" />

    <script>
        $(document).ready(function () {
            var rightHeight = $("#<% = rightHelp.ClientID %>").height();
            $("#left-help").css("height", rightHeight + 50);
        });
    </script>

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

    <div class="w-section" style="overflow-y: hidden !important;">
        <div class="w-row help">
            <div class="w-col w-col-4 help-eft" style="margin-left: -10px; margin-top: -20px;">
                <div class="topic-section" id="left-help">
                    <h2 id="practiceTopicTitleId" runat="server">Algebra</h2>
                    <a class="w-inline-block link-help" href="/practice">
                        <div class="link-backtext">&lt;</div>
                    </a>
                </div>
            </div>
            <div class="w-col w-col-8 help" id="rightHelp" style="margin-top: 20px; margin-bottom: 20px;" runat="server">
            </div>
        </div>
    </div>
</asp:Content>