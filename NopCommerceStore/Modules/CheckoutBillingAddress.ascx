<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CheckoutBillingAddressControl"
    CodeBehind="CheckoutBillingAddress.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="OrderSummary" Src="~/Modules/OrderSummary.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="AddressEdit" Src="~/Modules/AddressEdit.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="AddressDisplay" Src="~/Modules/AddressDisplay.ascx" %>
<div class="CheckoutPage">
    <div class="title">
        <%=GetLocaleResourceString("Checkout.BillingAddress")%>
    </div>
    <div class="clear">
    </div>
    <div class="CheckoutData">
        <asp:Panel runat="server" ID="pnlSelectBillingAddress">
            <div class="SelectAddressTitle">
                <%=GetLocaleResourceString("Checkout.SelectBillingAddress")%>
            </div>
            <div class="clear">
            </div>
            <div class="AddressGrid">
                <asp:DataList ID="dlAddress" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                    RepeatLayout="Table" ItemStyle-CssClass="ItemBox">
                    <ItemTemplate>
                        <div class="AddressItem">
                            <div class="SelectButton">
                                <asp:Button runat="server" CommandName="Select" ID="btnSelect" Text='<%#GetLocaleResourceString("Checkout.BillingToThisAddress")%>'
                                    OnCommand="btnSelect_Command" ValidationGroup="SelectBillingAddress" CommandArgument='<%# Eval("AddressID") %>'
                                    SkinID="SelectBillingAddressButton" />
                            </div>
                            <div class="AddressBox">
                                <nopCommerce:AddressDisplay ID="adAddress" runat="server" Address='<%# Container.DataItem %>'
                                    ShowDeleteButton="false" ShowEditButton="false"></nopCommerce:AddressDisplay>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </div>
        </asp:Panel>
        <div class="clear">
        </div>
        <div class="EnterAddressTitle">
            <asp:Label runat="server" ID="lEnterBillingAddress"></asp:Label></div>
        <div class="clear">
        </div>
        <div class="EnterAddress">
            <div runat="server" id="pnlTheSameAsShippingAddress" class="TheSameAddress">
                <asp:Button runat="server" ID="btnTheSameAsShippingAddress" Text="<% $NopResources:Checkout.BillingAddressTheSameAsShippingAddress %>"
                    CausesValidation="false" OnClick="btnTheSameAsShippingAddress_Click" SkinID="SameAsShippingAddressButton" />
            </div>
            <div class="EnterAddressBody">
                <nopCommerce:AddressEdit ID="AddressDisplayCtrl" runat="server" IsNew="true" IsBillingAddress="true" />
            </div>
            <div class="clear">
            </div>
            <div class="Button">
                <asp:Button runat="server" ID="btnNextStep" Text="<% $NopResources:Checkout.NextButton %>"
                    OnClick="btnNextStep_Click" SkinID="NewAddressNextStepButton" />
            </div>
            <div class="ErrorBlock">
                <div class="messageError">
                    <asp:Literal runat="server" ID="lError" EnableViewState="false"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
        <div class="OrderSummaryTitle">
            <%=GetLocaleResourceString("Checkout.OrderSummary")%>
        </div>
        <div class="clear">
        </div>
        <div class="OrderSummaryBody">
            <nopCommerce:OrderSummary ID="OrderSummaryControl" runat="server" IsShoppingCart="false">
            </nopCommerce:OrderSummary>
        </div>
    </div>
</div>
