<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CheckoutConfirmControl"
    CodeBehind="CheckoutConfirm.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="OrderSummary" Src="~/Modules/OrderSummary.ascx" %>
<div class="CheckoutPage">
    <div class="title">
        <%=GetLocaleResourceString("Checkout.ConfirmYourOrder")%>
    </div>
    <div class="clear">
    </div>
    <div class="CheckoutData">
        <div class="ConfirmOrder">
            <div class="SelectButton">
                <asp:Button runat="server" ID="btnNextStep" Text="<% $NopResources:Checkout.ConfirmButton %>"
                    OnClick="btnNextStep_Click" SkinID="ConfirmOrderNextStepButton" />
            </div>
            <div class="clear">
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
