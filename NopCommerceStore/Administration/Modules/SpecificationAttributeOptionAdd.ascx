<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.SpecificationAttributeOptionAddControl"
    CodeBehind="SpecificationAttributeOptionAdd.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="SimpleTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="NumericTextBox" Src="NumericTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-catalog.png" alt="<%=GetLocaleResourceString("Admin.SpecificationAttributeOptionAdd.Title")%>" />
        <%=GetLocaleResourceString("Admin.SpecificationAttributeOptionAdd.Title")%>
        <asp:HyperLink ID="hlBack" runat="server" Text="<% $NopResources:Admin.SpecificationAttributeOptionAdd.BackToAttributeDetails %>"
            ToolTip="<% $NopResources:Admin.SpecificationAttributeOptionAdd.BackToAttributeDetails %>" />
    </div>
    <div class="options">
        <asp:Button ID="AddButton" runat="server" Text="<% $NopResources:Admin.SpecificationAttributeOptionAdd.SaveButton.Text %>"
            CssClass="adminButtonBlue" OnClick="AddButton_Click" ToolTip="<% $NopResources:Admin.SpecificationAttributeOptionAdd.SaveButton.Tooltip %>" />
    </div>
</div>
<%if (this.HasLocalizableContent)
  { %>
<div id="localizablecontentpanel" class="tabcontainer-usual">
    <ul class="idTabs">
        <li class="idTab"><a href="#idTab_Info1" class="selected">
            <%=GetLocaleResourceString("Admin.Localizable.Standard")%></a></li>
        <asp:Repeater ID="rptrLanguageTabs" runat="server">
            <ItemTemplate>
                <li class="idTab"><a href="#idTab_Info<%# Container.ItemIndex+2 %>">
                    <asp:Image runat="server" ID="imgCFlag" Visible='<%# !String.IsNullOrEmpty(Eval("IconURL").ToString()) %>'
                        AlternateText='<%#Eval("Name")%>' ImageUrl='<%#Eval("IconURL").ToString()%>' />
                    <%#Server.HtmlEncode(Eval("Name").ToString())%></a></li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    <div id="idTab_Info1" class="tab">
        <%} %>
        <table class="adminContent">
            <tr>
                <td class="adminTitle">
                    <nopCommerce:ToolTipLabel runat="server" ID="lblOptionName" Text="<% $NopResources:Admin.SpecificationAttributeOptionAdd.Name %>"
                        ToolTip="<% $NopResources:Admin.SpecificationAttributeOptionAdd.Name.Tooltip %>"
                        ToolTipImage="~/Administration/Common/ico-help.gif" />
                </td>
                <td class="adminData">
                    <nopCommerce:SimpleTextBox runat="server" ID="txtNewOptionName" CssClass="adminInput" />
                </td>
            </tr>
        </table>
        <%if (this.HasLocalizableContent)
          { %></div>
    <asp:Repeater ID="rptrLanguageDivs" runat="server" OnItemDataBound="rptrLanguageDivs_ItemDataBound">
        <ItemTemplate>
            <div id="idTab_Info<%# Container.ItemIndex+2 %>" class="tab">
                <i>
                    <%=GetLocaleResourceString("Admin.Localizable.EmptyFieldNote")%></i>
                <asp:Label ID="lblLanguageID" runat="server" Text='<%#Eval("LanguageID") %>' Visible="false"></asp:Label>
                <table class="adminContent">
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblLocalizedOptionName" Text="<% $NopResources:Admin.SpecificationAttributeOptionAdd.Name %>"
                                ToolTip="<% $NopResources:Admin.SpecificationAttributeOptionAdd.Name.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:TextBox runat="server" ID="txtNewLocalizedOptionName" CssClass="adminInput" />
                        </td>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
<%} %>
<table class="adminContent">
    <tbody>
        <tr>
            <td class="adminTitle">
                <nopCommerce:ToolTipLabel runat="server" ID="lblOptionDisplayOrder" Text="<% $NopResources:Admin.SpecificationAttributeOptionAdd.DisplayOrder %>"
                    ToolTip="<% $NopResources:Admin.SpecificationAttributeOptionAdd.DisplayOrder.Tooltip %>"
                    ToolTipImage="~/Administration/Common/ico-help.gif" />
            </td>
            <td class="adminData">
                <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtNewOptionDisplayOrder"
                    Value="1" RequiredErrorMessage="<% $NopResources:Admin.SpecificationAttributeOptionAdd.DisplayOrder.RequiredErrorMessage %>"
                    RangeErrorMessage="<% $NopResources:Admin.SpecificationAttributeOptionAdd.DisplayOrder.RangeErrorMessage %>"
                    MinimumValue="-99999" MaximumValue="99999" />
            </td>
        </tr>
    </tbody>
</table>
