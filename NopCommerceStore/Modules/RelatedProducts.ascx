<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Modules.RelatedProductsControl" Codebehind="RelatedProducts.ascx.cs" %>
<div class="RelatedProductsGrid">
    <div class="title">
        <%=GetLocaleResourceString("Products.RelatedProducts")%>
    </div>
    <div class="clear">
    </div>
    <asp:DataList ID="dlRelatedProducts" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
        RepeatLayout="Table" OnItemDataBound="dlRelatedProducts_ItemDataBound" ItemStyle-CssClass="ItemBox">
        <ItemTemplate>
            <div class="RelatedItem">
                <div class="title">
                    <asp:HyperLink ID="hlProduct" runat="server" />
                </div>
                <div class="picture">
                    <asp:HyperLink ID="hlImageLink" runat="server" />
                </div>
            </div>
        </ItemTemplate>
    </asp:DataList>
</div>
