<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.OrderSummaryControl"
    CodeBehind="OrderSummary.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="GoogleCheckoutButton" Src="~/Modules/GoogleCheckoutButton.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="OrderTotals" Src="~/Modules/OrderTotals.ascx" %>
<asp:Panel class="order-summary-content" runat="server" ID="pnlEmptyCart">
    <%=GetLocaleResourceString("ShoppingCart.CartIsEmpty")%>
</asp:Panel>
<asp:Panel class="order-summary-content" runat="server" ID="pnlCart">
    <%if (IsShoppingCart)
      { %>
    <asp:Panel runat="server" ID="phCoupon" CssClass="coupon-box">
        <b>
            <%=GetLocaleResourceString("ShoppingCart.DiscountCouponCode")%></b>
        <br />
        <%=GetLocaleResourceString("ShoppingCart.DiscountCouponCode.Tooltip")%>
        <br />
        <asp:TextBox ID="txtDiscountCouponCode" runat="server" Width="125px" />&nbsp;
        <asp:Button runat="server" ID="btnApplyDiscountCouponCode" OnClick="btnApplyDiscountCouponCode_Click"
            Text="<% $NopResources:ShoppingCart.ApplyDiscountCouponCodeButton %>" CssClass="applycouponcodebutton"
            CausesValidation="false" />
        <asp:Panel runat="server" ID="pnlDiscountWarnings" CssClass="warning-box" EnableViewState="false"
            Visible="false">
            <br />
            <asp:Label runat="server" ID="lblDiscountWarning" CssClass="warning-text" EnableViewState="false"
                Visible="false"></asp:Label>
        </asp:Panel>
    </asp:Panel>
    <div class="clear">
    </div>
    <asp:Panel runat="server" ID="phGiftCards" CssClass="coupon-box">
        <b>
            <%=GetLocaleResourceString("ShoppingCart.GiftCards")%></b>
        <br />
        <%=GetLocaleResourceString("ShoppingCart.GiftCards.Tooltip")%>
        <br />
        <asp:TextBox ID="txtGiftCardCouponCode" runat="server" Width="125px" />&nbsp;
        <asp:Button runat="server" ID="btnApplyGiftCardsCouponCode" OnClick="btnApplyGiftCardCouponCode_Click"
            Text="<% $NopResources:ShoppingCart.ApplyGiftCardCouponCodeButton %>" CssClass="applycouponcodebutton"
            CausesValidation="false" />
        <asp:Panel runat="server" ID="pnlGiftCardWarnings" CssClass="warning-box" EnableViewState="false"
            Visible="false">
            <br />
            <asp:Label runat="server" ID="lblGiftCardWarning" CssClass="warning-text" EnableViewState="false"
                Visible="false"></asp:Label>
        </asp:Panel>
    </asp:Panel>
    <div class="clear">
    </div>
    <%} %>
    <asp:Panel runat="server" ID="pnlCommonWarnings" CssClass="warning-box" EnableViewState="false"
        Visible="false">
        <asp:Label runat="server" ID="lblCommonWarning" CssClass="warning-text" EnableViewState="false"
            Visible="false"></asp:Label>
    </asp:Panel>
    <table class="cart">
        <tbody>
            <tr class="cart-header-row">
                <%if (IsShoppingCart)
                  { %>
                <td width="10%">
                    <%=GetLocaleResourceString("ShoppingCart.Remove")%>
                </td>
                <%} %>
                <%if (SettingManager.GetSettingValueBoolean("Display.ShowProductImagesOnShoppingCart"))
                  {%>
                <td class="picture">
                </td>
                <%} %>
                <td width="40%">
                    <%=GetLocaleResourceString("ShoppingCart.Product(s)")%>
                </td>
                <td width="20%">
                    <%=GetLocaleResourceString("ShoppingCart.UnitPrice")%>
                </td>
                <td width="10%">
                    <%=GetLocaleResourceString("ShoppingCart.Quantity")%>
                </td>
                <td width="20%" class="end">
                    <%=GetLocaleResourceString("ShoppingCart.ItemTotal")%>
                </td>
            </tr>
            <asp:Repeater ID="rptShoppingCart" runat="server">
                <ItemTemplate>
                    <tr class="cart-item-row">
                        <%if (IsShoppingCart)
                          { %>
                        <td width="10%">
                            <asp:CheckBox runat="server" ID="cbRemoveFromCart" />
                        </td>
                        <%} %>
                        <%if (SettingManager.GetSettingValueBoolean("Display.ShowProductImagesOnShoppingCart"))
                          {%>
                        <td class="productpicture">
                            <asp:Image ID="iProductVariantPicture" runat="server" ImageUrl='<%#GetProductVariantImageUrl((ShoppingCartItem)Container.DataItem)%>'
                                AlternateText="Product picture" />
                        </td>
                        <%} %>
                        <td width="40%" class="product">
                            <a href='<%#GetProductURL((ShoppingCartItem)Container.DataItem)%>' title="View details">
                                <%#Server.HtmlEncode(GetProductVariantName((ShoppingCartItem)Container.DataItem))%></a>
                            <%#GetAttributeDescription((ShoppingCartItem)Container.DataItem)%>
                            <asp:Panel runat="server" ID="pnlWarnings" CssClass="warning-box" EnableViewState="false"
                                Visible="false">
                                <asp:Label runat="server" ID="lblWarning" CssClass="warning-text" EnableViewState="false"
                                    Visible="false"></asp:Label>
                            </asp:Panel>
                        </td>
                        <td width="20%">
                            <%#GetShoppingCartItemUnitPriceString((ShoppingCartItem)Container.DataItem)%>
                        </td>
                        <td width="10%">
                            <%if (IsShoppingCart)
                              { %>
                            <asp:TextBox ID="txtQuantity" size="4" runat="server" Text='<%# Eval("Quantity") %>'
                                SkinID="ShoppingCartQuantityText" />
                            <%}
                              else
                              { %>
                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="Label" />
                            <%} %>
                        </td>
                        <td width="20%" class="end">
                            <%#GetShoppingCartItemSubTotalString((ShoppingCartItem)Container.DataItem)%>
                            <asp:Label ID="lblShoppingCartItemID" runat="server" Visible="false" Text='<%# Eval("ShoppingCartItemID") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <div class="clear">
    </div>
    <div class="cart-footer">
        <%if (IsShoppingCart)
          { %>
        <div class="clear">
        </div>
        <div class="buttons">
            <div class="common-buttons">
                <asp:Button ID="btnUpdate" OnClick="btnUpdate_Click" runat="server" Text="<% $NopResources:ShoppingCart.UpdateCart %>"
                    CssClass="updatecartbutton" />
                <asp:Button ID="btnContinueShopping" OnClick="btnContinueShopping_Click" runat="server"
                    Text="<% $NopResources:ShoppingCart.ContinueShopping %>" CssClass="continueshoppingbutton" />
                <asp:Button ID="btnCheckout" OnClick="btnCheckout_Click" runat="server" Text="<% $NopResources:ShoppingCart.Checkout %>"
                    CssClass="checkoutbutton" />
            </div>
            <div class="addon-buttons">
                <nopCommerce:GoogleCheckoutButton runat="server" ID="btnGoogleCheckoutButton"></nopCommerce:GoogleCheckoutButton>
            </div>
        </div>
        <div class="clear">
        </div>
        <%} %>
        <nopCommerce:OrderTotals runat="server" ID="ctrlOrderTotals"></nopCommerce:OrderTotals>
    </div>
</asp:Panel>
