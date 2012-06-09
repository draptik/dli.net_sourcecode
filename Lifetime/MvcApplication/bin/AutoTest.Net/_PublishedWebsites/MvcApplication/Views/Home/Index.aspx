<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Ploeh.Samples.Lifetime.MvcApplication.Models.HomeIndexViewModel>" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <% foreach (var p in this.Model.Products)
           { %>
           <div><%= this.Html.Encode(p.Name) %></div>
        <% } %>
    </div>
</asp:Content>
