<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Modules.CategoryNavigation" Codebehind="CategoryNavigation.ascx.cs" %>
<div class="category-navigation">
    <div class="title">
        <%=GetLocaleResourceString("Category.Categories")%>
    </div>
    <div class="clear"></div>
    <div class="treeview">
        <ul>
            <asp:PlaceHolder runat="server" ID="phCategories" />
        </ul>
    </div>
</div>
