<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="contact.aspx.cs" Inherits="MathSolverWebsite.contact" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style type="text/css">
        .body {
            overflow-y: auto;
            font-size: 20px;
        }

        .msg-body {
            width: 100%;
            margin-left: auto;
            margin-right: auto;
            resize: none;
            font-size: 20px;
            font-family: Roboto, sans-serif;
            height: 250px;
            border: 1px solid black;
        }

        .msg-optional-notice {
            color: gray;
            font-size: 15px;
        }

        .email-enter {
            width: 300px;
            height: 30px;
            font-size: 20px;
            border: 1px solid black;
        }

        input {
            padding-left: 5px;
        }

        .msg-prompt {
            margin-top: 20px;
            margin-bottom: 5px;
            font-size: 25px;
        }

        .submit-btn {
            display: block;
            right: 0px;
            background-color: #1abc9c;
            color: white;
            width: 200px;
            height: 60px;
            font-size: 28px;
            border: 0px;
            margin-top: 20px;
            margin-bottom: 150px;
        }

            .submit-btn:hover {
                cursor: pointer;
            }

        .centered-container {
            width: 50%;
            min-width: 400px;
            margin-left: auto;
            margin-right: auto;
        }

        .success-msg {
            color: #1abc9c;
            text-align: center;
        }

        .error-msg {
            color: #e74c3c;
            text-align: center;
        }

        .output-area a {
            margin-left: auto;
            margin-right: auto;
            width: 238px;
            display: block;
            color: #339ce1;
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
    <div class="ws-container">
        <div class="centered-container">
            <div class="heading-1">Suggestions</div>
            <div runat="server" id="outputDisp" class="output-area">

            </div>
            <p class="msg-prompt">Email</p>
            <asp:TextBox MaxLength="100" CssClass="email-enter" ID="emailTxtBox" placeholder="Email" runat="server"></asp:TextBox>
            <asp:Label ID="Label2" CssClass="msg-optional-notice" runat="server" Text="(Optional)" />
        
            <p class="msg-prompt">Message</p>
            <textarea maxlength="600" id="messageBody" class="msg-body" runat="server">

            </textarea>

            <asp:Button ID="submitBtn" CssClass="submit-btn" runat="server" Text="Submit" OnClick="submitBtn_Click" />
        </div>
    </div>
</asp:Content>
