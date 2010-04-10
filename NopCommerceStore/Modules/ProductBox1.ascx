<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.ProductBox1Control"
    CodeBehind="ProductBox1.ascx.cs" %>
<div class="ProductItem">
    <div class="title">
        <asp:HyperLink ID="hlProduct" runat="server" />
    </div>
    <div class="picture">
        <asp:HyperLink ID="hlImageLink" runat="server" />
    </div>
    <div class="description">
        <asp:Literal runat="server" ID="lShortDescription"></asp:Literal>
    </div>
    <div class="addInfo">
        <div class="prices">
            <asp:Label ID="lblOldPrice" runat="server" CssClass="oldproductPrice" />
            <br />
            <asp:Label ID="lblPrice" runat="server" CssClass="productPrice" /></div>
        <div class="buttons">
            <asp:Button runat="server" ID="btnProductDetails" OnCommand="btnProductDetails_Click"
                Text="<% $NopResources:Products.ProductDetails %>" ValidationGroup="ProductDetails"
                CommandArgument='<%# Eval("ProductID") %>' SkinID="ProductGridProductDetailButton" /><br />
            <asp:Button runat="server" ID="btnAddToCart" OnCommand="btnAddToCart_Click" Text="<% $NopResources:Products.AddToCart %>"
                ValidationGroup="ProductDetails" CommandArgument='<%# Eval("ProductID") %>' SkinID="ProductGridAddToCartButton" />
        </div>
    </div>
</div>
