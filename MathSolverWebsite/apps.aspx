<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="apps.aspx.cs" Inherits="MathSolverWebsite.AppsPage" %>
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

        .w-container {
            max-width: 920px;
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
                <asp:ImageButton CssClass="promote-image" ID="ImageButton1" runat="server" ImageUrl="~/Images/Branding/en_app_rgb_wo_60.png" />
                <asp:ImageButton CssClass="promote-image" ID="ImageButton2" runat="server" ImageUrl="~/Images/Branding/258x67_WP_Store_cyan.png" />
            </div>
        </div>
    </div>

</asp:Content>
