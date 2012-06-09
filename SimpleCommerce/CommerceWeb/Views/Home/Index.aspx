<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<FeaturedProductsViewModel>" %>
<%@ Import Namespace="Ploeh.Samples.Commerce.Web.Models" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Featured Products</h2>
    <div>
    <% foreach (var product in this.Model.Products)
       { %>
       <div><%= this.Html.Encode(product.SummaryText) %></div>
    <% } %>
    </div>
</asp:Content>
