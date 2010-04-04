<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.HeaderMenuControl"
    CodeBehind="HeaderMenu.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="SearchBox" Src="~/Modules/SearchBox.ascx" %>
<div class="headermenu">
    <div class="searchbox">
        <nopCommerce:SearchBox runat="server" ID="ctrlSearchBox">
        </nopCommerce:SearchBox>
    </div>
    <ul>
        <li><a href="<%=CommonHelper.GetStoreLocation()%>">
            <%=GetLocaleResourceString("Content.HomePage")%></a> </li>
        <% if (ProductManager.RecentlyAddedProductsEnabled)
           { %>
        <li><a href="<%=Page.ResolveUrl("~/RecentlyAddedProducts.aspx")%>">
            <%=GetLocaleResourceString("Products.NewProducts")%></a> </li>
        <%} %>
        <li><a href="<%=Page.ResolveUrl("~/Search.aspx")%>">
            <%=GetLocaleResourceString("Search.Search")%></a> </li>
        <li><a href="<%=Page.ResolveUrl("~/Account.aspx")%>">
            <%=GetLocaleResourceString("Account.MyAccount")%></a> </li>
        <% if (BlogManager.BlogEnabled)
           { %>
        <li><a href="<%=Page.ResolveUrl("~/Blog.aspx")%>">
            <%=GetLocaleResourceString("Blog.Blog")%></a> </li>
        <%} %>
        <% if (ForumManager.ForumsEnabled)
           { %>
        <li><a href="<%= SEOHelper.GetForumMainURL()%> ">
            <%=GetLocaleResourceString("Forum.Forums")%></a></li>
        <%} %>
        <li><a href="<%=Page.ResolveUrl("~/ContactUs.aspx")%>">
            <%=GetLocaleResourceString("ContactUs.ContactUs")%></a> </li>
    </ul>
</div>
