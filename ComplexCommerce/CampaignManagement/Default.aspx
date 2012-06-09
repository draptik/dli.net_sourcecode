<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Ploeh.Samples.Commerce.Campaign.WebForms.Default" Culture="en-US" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="campaignGridView" runat="server" 
            DataSourceID="campaignDataSource" AutoGenerateColumns="False" BackColor="White" 
            BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
            ForeColor="Black" GridLines="Horizontal">
            <Columns>
                <asp:CommandField ShowEditButton="True" />
                <asp:BoundField DataField="Id" HeaderText="Product Id" 
                    ReadOnly="True" SortExpression="Id" />
                <asp:BoundField DataField="ProductName" HeaderText="Product Name" 
                    SortExpression="ProductName" ReadOnly="True" />
                <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" ReadOnly="True" 
                    SortExpression="UnitPrice" />
                <asp:CheckBoxField DataField="IsFeatured" HeaderText="Featured" 
                    SortExpression="IsFeatured" />
                <asp:BoundField DataField="DiscountPrice" HeaderText="Discount Price" 
                    SortExpression="DiscountPrice" />
            </Columns>
            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        <asp:ObjectDataSource ID="campaignDataSource" runat="server" 
            DataObjectTypeName="Ploeh.Samples.Commerce.CampaignPresentation.CampaignItemPresenter" 
            SelectMethod="SelectAll" 
            TypeName="Ploeh.Samples.Commerce.Campaign.WebForms.CampaignDataSource" 
            UpdateMethod="Update"></asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
