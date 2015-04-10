<%@ Page Title="Topic Name" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="topic.aspx.cs" Inherits="MathSolverWebsite.prac.TopicPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Content/css/mlogica-practice.css" />

    <style>
        .sample {
          min-height: 200px;
          margin-top: 10px;
          padding: 1px 10px 10px;
          border-radius: 6px;
          background-color: white;
        }

    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="w-section">
        <div class="w-container" style="max-width: 920px;">
            <div class="sample">
                <h1>Topic Name Here</h1>
                <p>
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse varius enim in eros elementum tristique. Duis cursus, mi quis viverra ornare, eros dolor interdum nulla, ut commodo diam libero vitae erat. Aenean faucibus nibh et justo cursus id rutrum lorem imperdiet. Nunc ut sem vitae risus tristique posuere.
                </p>
                <h3>Examples</h3>
                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse varius enim in eros elementum tristique. Duis cursus, mi quis viverra ornare, eros dolor interdum nulla, ut commodo diam libero vitae erat. Aenean faucibus nibh et justo cursus id rutrum lorem imperdiet. Nunc ut sem vitae risus tristique posuere.</p>
            </div>
        </div>
    </div>
</asp:Content>
