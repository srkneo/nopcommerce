<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Payment.iDeal.ConfigurePaymentMethod"
    CodeBehind="ConfigurePaymentMethod.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="DecimalTextBox" Src="../../Modules/DecimalTextBox.ascx" %>
<table class="adminContent">
    <tr>
        <td colspan="2">
            <b>If you're using this gateway remember that you should set your store primary currency
                to Euro.</b>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            Merchant ID:
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtMerchantID" CssClass="adminInput"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            Sub ID:
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtSubID" CssClass="adminInput"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            Hash key:
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtHashKey" CssClass="adminInput"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            URL:
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtUrl" CssClass="adminInput"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            Additional fee [<%=CurrencyManager.PrimaryStoreCurrency.CurrencyCode%>]:
        </td>
        <td class="adminData">
            <nopCommerce:DecimalTextBox runat="server" ID="txtAdditionalFee" Value="0" RequiredErrorMessage="Additional fee is required"
                MinimumValue="0" MaximumValue="999999" RangeErrorMessage="The value must be from 0 to 999999"
                CssClass="adminInput"></nopCommerce:DecimalTextBox>
        </td>
    </tr>
</table>
