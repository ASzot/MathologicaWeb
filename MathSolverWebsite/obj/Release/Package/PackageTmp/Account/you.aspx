<%@ Page Title="You" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="you.aspx.cs" Inherits="MathSolverWebsite.Account.You" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <!-- MathJax include. -->
    <script async="async" type="text/javascript" src="https://cdn.mathjax.org/mathjax/latest/MathJax.js?config=AM_HTMLorMML"></script>
    <link rel="stylesheet" type="text/css" href="../Content/css/mlogica-you.css" />

    
    <script>
        $(document).ready(function () {

            $(".pob-problem").click(function (e) {
                // Paste into the input.
                var inputDisp = $(this).find(".hidden").html();
                var inputDispSplit = inputDisp.split('|');
                var inputDispEncoded = encodeURIComponent(inputDispSplit[0]);
                window.location.href = ("/Default?Index=" + inputDispSplit[1] + "&InputDisp=" + inputDispEncoded + ((inputDispSplit[2] == null || inputDispSplit[2] == "") ? "" : "&UseRad=" + inputDispSplit[1]));
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="ws-container">
        <asp:Label runat="server" Text="" CssClass="gen-error" ID="genError"></asp:Label>
        <asp:ListView ID="savedProblemsList" runat="server" GroupItemCount="4" ItemPlaceholderID="itemPlaceHolder1" GroupPlaceholderID="groupPlaceHolder1" 
            OnItemCommand="savedProblemsList_ItemCommand" OnItemDeleting="savedProblemsList_ItemDeleting">
            <EmptyDataTemplate>
                <p class="no-prob-alert">You have no saved problems.</p>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <h1 class="heading-2">Saved Problems</h1>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                    </tr>
                </table>
            </LayoutTemplate>
            <GroupTemplate>
                <tr>
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                </tr>
            </GroupTemplate>
            <ItemTemplate>
                <div class="prob-div">
                    <div class="noselect pointable prob-disp">
                        <div class="pob-problem">
                            <span class="hidden"><%# Eval("problem")%>|0</span>
                            <div>
                                <%# START_MATH + Eval("problem") + END_MATH %>
                            </div>
                        </div>
                    </div>
                    <div class="prob-date">
                        <%# Eval("entry_time") %>
                    </div>
                    <asp:LinkButton runat="server" ID="SelectCategoryButton" CommandName="Delete" 
                        CommandArgument='<%#Eval("problem")%>' >
                        <span class="del-span">
                            Delete
                        </span>
                    </asp:LinkButton>
                </div>
            </ItemTemplate>
        </asp:ListView>
    </div>

</asp:Content>
