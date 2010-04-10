<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Modules.ProductCategoryBreadcrumb" Codebehind="ProductCategoryBreadcrumb.ascx.cs" %>
<div class="breadcrumb">
    <a href='<%=Page.ResolveUrl("~/Default.aspx")%>'>
        <%=GetLocaleResourceString("Breadcrumb.Top")%></a> /
    <asp:Repeater ID="rptrCategoryBreadcrumb" runat="server">
        <ItemTemplate>
            <b><a href='<%#SEOHelper.GetCategoryURL(Convert.ToInt32(Eval("CategoryID"))) %>'>
                <%#Server.HtmlEncode(Eval("Name").ToString()) %></a></b>
        </ItemTemplate>
        <SeparatorTemplate>
            /
        </SeparatorTemplate>
    </asp:Repeater>
    / <b>
        <asp:HyperLink runat="server" ID="hlProduct"></asp:HyperLink></b>
    <br />
</div>
