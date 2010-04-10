<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.DiscountDetailsControl"
    CodeBehind="DiscountDetails.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="DiscountInfo" Src="DiscountInfo.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-promotions.png" alt="<%=GetLocaleResourceString("Admin.DiscountDetails.Title")%>" />
        <%=GetLocaleResourceString("Admin.DiscountDetails.Title")%>
        <a href="Discounts.aspx" title="<%=GetLocaleResourceString("Admin.DiscountDetails.BackToDiscounts")%>">
            (<%=GetLocaleResourceString("Admin.DiscountDetails.BackToDiscounts")%>)</a>
    </div>
    <div class="options">
        <asp:Button ID="SaveButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.DiscountDetails.SaveButton %>"
            OnClick="SaveButton_Click" ToolTip="<% $NopResources:Admin.DiscountDetails.SaveButton.Tooltip %>" />
        <asp:Button ID="DeleteButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.DiscountDetails.DeleteButton %>"
            OnClick="DeleteButton_Click" CausesValidation="false" ToolTip="<% $NopResources:Admin.DiscountDetails.DeleteButton.Tooltip %>" />
    </div>
</div>
<nopCommerce:DiscountInfo ID="ctrlDiscountInfo" runat="server" />
<ajaxToolkit:ConfirmButtonExtender ID="ConfirmDeleteButtonExtender" runat="server"
    TargetControlID="DeleteButton" DisplayModalPopupID="ModalPopupExtenderDelete" />
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderDelete" runat="server" TargetControlID="DeleteButton"
    PopupControlID="pnlDeletePopup" OkControlID="deleteButtonOk" CancelControlID="deleteButtonCancel"
    BackgroundCssClass="modalBackground" />
<asp:Panel ID="pnlDeletePopup" runat="server" Style="display: none; width: 250px;
    background-color: White; border-width: 2px; border-color: Black; border-style: solid;
    padding: 20px;">
    <div style="text-align: center;">
        <%=GetLocaleResourceString("Admin.Common.AreYouSure")%>
        <br />
        <br />
        <asp:Button ID="deleteButtonOk" runat="server" Text="<% $NopResources:Admin.Common.Yes %>" CssClass="adminButton" CausesValidation="false" />
        <asp:Button ID="deleteButtonCancel" runat="server" Text="<% $NopResources:Admin.Common.No %>" CssClass="adminButton"
            CausesValidation="false" />
    </div>
</asp:Panel>
