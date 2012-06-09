<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<FeaturedProductsViewModel>" %>
<%@ Import Namespace="Ploeh.Samples.Commerce.Web.PresentationModel.Models" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Featured Products</h2>
    <div>
    <% foreach (var product in this.Model.Products)
       { %>
       <div>
       <%= this.Html.Encode(product.SummaryText) %>
       <%= this.Html.ActionLink("Add to basket", "Add", "Basket", new { Id = product.Id }, null) %>
       </div>
    <% } %>
    </div>
    <h3>Select currency:</h3>
    <div><%= this.Html.ActionLink("DKK", "SetCurrency", new { id = "DKK"} ) %></div>
    <div><%= this.Html.ActionLink("USD", "SetCurrency", new { id = "USD"} ) %></div>
    <div><%= this.Html.ActionLink("EUR", "SetCurrency", new { id = "EUR"} ) %></div>
</asp:Content>
