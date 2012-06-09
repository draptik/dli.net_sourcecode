<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BasketViewModel>" %>
<%@ Import Namespace="Ploeh.Samples.Commerce.Web.PresentationModel.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Basket
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Basket</h2>
    <div>
    <% foreach (var item in this.Model.Contents)
       { %>
       <div>
       <%= this.Html.Encode(item) %>
       </div>
    <% } %>
    </div>
    <%= this.Html.ActionLink("Empty basket", "Empty", "Basket") %>
</asp:Content>
