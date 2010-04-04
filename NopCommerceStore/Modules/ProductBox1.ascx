<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.ProductBox1Control"
    CodeBehind="ProductBox1.ascx.cs" %>
<div class="product-item">
    <div class="product-title">
        <asp:HyperLink ID="hlProduct" runat="server" />
    </div>
    <div class="picture">
        <asp:HyperLink ID="hlImageLink" runat="server" />
    </div>
    <div class="description">
        <asp:Literal runat="server" ID="lShortDescription"></asp:Literal>
    </div>
    <div class="add-info">
        <div class="prices">
            <asp:Label ID="lblOldPrice" runat="server" CssClass="oldproductPrice" />
            <br />
            <asp:Label ID="lblPrice" runat="server" CssClass="productPrice" /></div>
        <div class="buttons">
            <asp:Button runat="server" ID="btnProductDetails" OnCommand="btnProductDetails_Click"
                Text="<% $NopResources:Products.ProductDetails %>" ValidationGroup="ProductDetails"
                CommandArgument='<%# Eval("ProductID") %>' CssClass="productgridproductdetailbutton" /><br />
            <asp:Button runat="server" ID="btnAddToCart" OnCommand="btnAddToCart_Click" Text="<% $NopResources:Products.AddToCart %>"
                ValidationGroup="ProductDetails" CommandArgument='<%# Eval("ProductID") %>' CssClass="productgridaddtocartbutton" />
        </div>
    </div>
</div>
