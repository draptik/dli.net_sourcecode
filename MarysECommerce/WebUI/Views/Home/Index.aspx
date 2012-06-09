<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Ploeh.Samples.Mary.ECommerce.Data.Sql" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Featured Products</h2>
    <div>
    <% var products =
           (IEnumerable<Product>)this.ViewData["Products"];
       foreach (var product in products)
       { %>
       <div>
       <%= this.Html.Encode(product.Name) %>
       (<%= this.Html.Encode(product.UnitPrice.ToString("C")) %>)
       </div>
    <% } %>
    </div>
</asp:Content>
