<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Templates.Categories.ProductsInGrid"
    CodeBehind="ProductsInGrid.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductBox1" Src="~/Modules/ProductBox1.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="PriceRangeFilter" Src="~/Modules/PriceRangeFilter.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductSpecificationFilter" Src="~/Modules/ProductSpecificationFilter.ascx" %>
<div class="CategoryPage">
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
        <br />
    </div>
    <div class="clear">
    </div>
    <div class="CategoryTitle">
        <asp:Literal runat="server" ID="lName"></asp:Literal>
    </div>
    <div class="clear">
    </div>
    <div class="CategoryDescription">
        <asp:Literal runat="server" ID="lDescription"></asp:Literal>
    </div>
    <div class="clear">
    </div>
    <div class="SubCategoryGrid">
        <asp:DataList ID="dlSubCategories" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
            RepeatLayout="Table" OnItemDataBound="dlSubCategories_ItemDataBound" ItemStyle-CssClass="ItemBox">
            <ItemTemplate>
                <div class="SubCategoryItem">
                    <div class="title">
                        <asp:HyperLink ID="hlCategory" runat="server" />
                    </div>
                    <div class="picture">
                        <asp:HyperLink ID="hlImageLink" runat="server" />
                    </div>
                </div>
            </ItemTemplate>
        </asp:DataList>
    </div>
    <div class="clear">
    </div>
    <asp:Panel runat="server" ID="pnlFeaturedProducts" class="FeaturedProductGrid">
        <div class="title">
            <%=GetLocaleResourceString("Products.FeaturedProducts")%>
        </div>
        <div>
            <asp:DataList ID="dlFeaturedProducts" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                RepeatLayout="Table" ItemStyle-CssClass="ItemBox">
                <ItemTemplate>
                    <nopCommerce:ProductBox1 ID="ctrlProductBox" Product='<%# Container.DataItem %>'
                        runat="server" />
                </ItemTemplate>
            </asp:DataList>
        </div>
    </asp:Panel>
    <div class="clear">
    </div>
    <asp:Panel runat="server" ID="pnlFilters" CssClass="ProductFilters">
        <div class="FilterTitle">
            <asp:Label runat="server" ID="lblProductFilterTitle">
                <%=GetLocaleResourceString("Products.FilterOptionsTitle")%>
            </asp:Label>
        </div>
        <div class="FilterItem">
            <nopCommerce:PriceRangeFilter ID="ctrlPriceRangeFilter" runat="server" />
        </div>
        <div class="FilterItem">
            <nopCommerce:ProductSpecificationFilter ID="ctrlProductSpecificationFilter" runat="server" />
        </div>
    </asp:Panel>
    <div class="clear">
    </div>
    <div class="ProductGrid">
        <asp:DataList ID="dlProducts" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
            RepeatLayout="Table" ItemStyle-CssClass="ItemBox">
            <ItemTemplate>
                <nopCommerce:ProductBox1 ID="ctrlProductBox" Product='<%# Container.DataItem %>'
                    runat="server" />
            </ItemTemplate>
        </asp:DataList>
    </div>
    <div class="ProductPager">
        <nopCommerce:Pager runat="server" ID="productsPager" FirstButtonText="<% $NopResources:Pager.First %>"
            LastButtonText="<% $NopResources:Pager.Last %>" NextButtonText="<% $NopResources:Pager.Next %>"
            PreviousButtonText="<% $NopResources:Pager.Previous %>" CurrentPageText="Pager.CurrentPage" />
    </div>
</div>
