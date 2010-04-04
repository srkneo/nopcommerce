<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.CustomersControl" CodeBehind="Customers.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="DatePicker" Src="DatePicker.ascx" %>

<div class="section-header">
    <div class="title">
        <img src="Common/ico-customers.png" alt="<%=GetLocaleResourceString("Admin.Customers.Title")%>" />
        <%=GetLocaleResourceString("Admin.Customers.Title")%>
    </div>
    <div class="options">
        <asp:Button ID="SearchButton" runat="server" Text="<% $NopResources:Admin.Customers.SearchButton.Text %>"
            CssClass="adminButtonBlue" OnClick="SearchButton_Click" ToolTip="<% $NopResources:Admin.Customers.SearchButton.Tooltip %>" />
        <asp:Button runat="server" Text="<% $NopResources:Admin.Customers.ExportXMLButton.Text %>"
            CssClass="adminButtonBlue" ID="btnExportXML" OnClick="btnExportXML_Click" ValidationGroup="ExportXML"
            ToolTip="<% $NopResources:Admin.Customers.ExportXMLButton.Tooltip %>" />
        <asp:Button runat="server" Text="<% $NopResources:Admin.Customers.ExportXLS.Text %>"
            CssClass="adminButtonBlue" ID="btnExportXLS" OnClick="btnExportXLS_Click" ValidationGroup="ExportXLS"
            ToolTip="<% $NopResources:Admin.Customers.ExportXLS.Tooltip %>" />
        <asp:Button runat="server" Text="<% $NopResources:Admin.Customers.ImportXLS.Text %>"
            CssClass="adminButtonBlue" ID="btnImportXLS" OnClick="btnImportXLS_Click" ToolTip="<% $NopResources:Admin.Customers.ImportXLS.Tooltip %>" />
        <input type="button" onclick="location.href='CustomerAdd.aspx'" value="<%=GetLocaleResourceString("Admin.Customers.AddNewButton.Text")%>"
            id="btnAddNew" class="adminButtonBlue" title="<%=GetLocaleResourceString("Admin.Customers.AddNewButton.Tooltip")%>" />
    </div>
</div>
<table width="100%">
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblRegistrationFrom" Text="<% $NopResources:Admin.Customers.RegistrationFrom %>"
                ToolTip="<% $NopResources:Admin.Customers.RegistrationFrom.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:DatePicker runat="server" ID="ctrlStartDatePicker" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblRegistrationTo" Text="<% $NopResources:Admin.Customers.RegistrationTo %>"
                ToolTip="<% $NopResources:Admin.Customers.RegistrationTo.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:DatePicker runat="server" ID="ctrlEndDatePicker" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblEmail" Text="<% $NopResources:Admin.Customers.Email %>"
                ToolTip="<% $NopResources:Admin.Customers.Email.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtEmail" CssClass="adminInput" runat="server"></asp:TextBox>
        </td>
    </tr>
    <asp:PlaceHolder runat="server" ID="phUsername">
        <tr>
            <td class="adminTitle">
                <nopCommerce:ToolTipLabel runat="server" ID="lblUsername" Text="<% $NopResources:Admin.Customers.Username %>"
                    ToolTip="<% $NopResources:Admin.Customers.Username.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
            </td>
            <td class="adminData">
                <asp:TextBox ID="txtUsername" CssClass="adminInput" runat="server"></asp:TextBox>
            </td>
        </tr>
    </asp:PlaceHolder>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblDontLoadGuestCustomers" Text="<% $NopResources:Admin.Customers.DontLoadGuest %>"
                ToolTip="<% $NopResources:Admin.Customers.DontLoadGuest.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox runat="server" ID="cbDontLoadGuestCustomers"></asp:CheckBox>
        </td>
    </tr>
</table>
<p>
</p>
<asp:GridView ID="gvCustomers" runat="server" AutoGenerateColumns="False" Width="100%"
    OnPageIndexChanging="gvCustomers_PageIndexChanging" AllowPaging="true" PageSize="15">
    <Columns>
        <asp:BoundField DataField="CustomerID" HeaderText="Customer ID" Visible="False">
        </asp:BoundField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.Customers.EmailColumn %>" ItemStyle-Width="20%">
            <ItemTemplate>
                <%#GetCustomerInfo((Customer)Container.DataItem)%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.Customers.NameColumn %>" ItemStyle-Width="20%">
            <ItemTemplate>
                <%#Server.HtmlEncode(Eval("FullName").ToString())%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.Customers.UsernameColumn %>"
            ItemStyle-Width="20%">
            <ItemTemplate>
                <%#Server.HtmlEncode(Eval("Username").ToString())%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.Customers.ActiveColumn %>"
            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <nopCommerce:ImageCheckBox runat="server" ID="cbActive" Checked='<%# Eval("Active") %>'>
                </nopCommerce:ImageCheckBox>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.Customers.RegistrationColumn %>"
            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <%#DateTimeHelper.ConvertToUserTime((DateTime)Eval("RegistrationDate")).ToString()%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.Customers.EditColumn %>" HeaderStyle-HorizontalAlign="Center"
            ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <a href="CustomerDetails.aspx?CustomerID=<%#Eval("CustomerID")%>" title="<%#GetLocaleResourceString("Admin.Customers.EditColumn.Tooltip")%>">
                    <%#GetLocaleResourceString("Admin.Customers.EditColumn")%>
                </a>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<ajaxToolkit:ConfirmButtonExtender ID="cbeImportXLS" runat="server" TargetControlID="btnImportXLS"
    DisplayModalPopupID="mpeImportXLS" />
<ajaxToolkit:ModalPopupExtender runat="server" ID="mpeImportXLS" TargetControlID="btnImportXLS"
    OkControlID="btnImportXLSOk" CancelControlID="btnImportXLSCancel" PopupControlID="pnlImportXLSPopupPanel"
    BackgroundCssClass="modalBackground" />
<asp:Panel runat="server" ID="pnlImportXLSPopupPanel" Style="display: none; width: 250px;
    background-color: White; border-width: 2px; border-color: Black; border-style: solid;
    padding: 20px;">
    <div style="text-align: center;">
        <%=GetLocaleResourceString("Admin.Customers.ImportXLS.ExcelFile")%>
        <asp:FileUpload runat="server" ID="fuXlsFile" />
        <asp:Button ID="btnImportXLSOk" runat="server" Text="<% $NopResources:Admin.Common.OK %>"
            CssClass="adminButton" CausesValidation="false" />
        <asp:Button ID="btnImportXLSCancel" runat="server" Text="<% $NopResources:Admin.Common.Cancel %>"
            CssClass="adminButton" CausesValidation="false" />
    </div>
</asp:Panel>
