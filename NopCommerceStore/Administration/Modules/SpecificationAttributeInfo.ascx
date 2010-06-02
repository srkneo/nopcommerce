<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.SpecificationAttributeInfoControl"
    CodeBehind="SpecificationAttributeInfo.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="SimpleTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="NumericTextBox" Src="NumericTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
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
                    <nopCommerce:ToolTipLabel runat="server" ID="lblName" Text="<% $NopResources:Admin.SpecificationAttributeInfo.Name %>"
                        ToolTip="<% $NopResources:Admin.SpecificationAttributeInfo.Name.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                </td>
                <td class="adminData">
                    <nopCommerce:SimpleTextBox runat="server" ID="txtName" CssClass="adminInput" ErrorMessage="<% $NopResources:Admin.SpecificationAttributeInfo.Name.ErrorMessage %>" />
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
                <asp:Label ID="lblLanguageId" runat="server" Text='<%#Eval("LanguageId") %>' Visible="false"></asp:Label>
                <table class="adminContent">
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblName" Text="<% $NopResources:Admin.SpecificationAttributeInfo.Name %>"
                                ToolTip="<% $NopResources:Admin.SpecificationAttributeInfo.Name.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:TextBox runat="server" ID="txtLocalizedName" CssClass="adminInput" />
                        </td>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
<%} %>
<table class="adminContent">
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblDisplayOrder" Text="<% $NopResources:Admin.SpecificationAttributeInfo.DisplayOrder %>"
                ToolTip="<% $NopResources:Admin.SpecificationAttributeInfo.DisplayOrder.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtDisplayOrder"
                Value="1" RequiredErrorMessage="<% $NopResources:Admin.SpecificationAttributeInfo.DisplayOrder.RequiredErrorMessage %>"
                RangeErrorMessage="<% $NopResources:Admin.SpecificationAttributeInfo.DisplayOrder.RangeErrorMessage %>"
                MinimumValue="-99999" MaximumValue="99999" ValidationGroup="NewProductSpecification" />
        </td>
    </tr>
</table>
<div id="pnlSpecAttrOptions" runat="server">
    <hr />
    <p>
        <strong>
            <%=GetLocaleResourceString("Admin.SpecificationAttributeInfo.AttributeOptions")%></strong></p>
    <asp:GridView ID="grdSpecificationAttributeOptions" runat="server" AutoGenerateColumns="false"
        DataKeyNames="SpecificationAttributeOptionId" OnRowDeleting="OnSpecificationAttributeOptionsDeleting"
        OnRowCommand="OnSpecificationAttributeOptionsCommand" OnRowDataBound="OnSpecificationAttributeOptionsDataBound">
        <Columns>
            <asp:TemplateField HeaderText="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption %>">
                <ItemTemplate>
                    <%if (this.HasLocalizableContent)
                      { %>
                    <div style="clear: both; padding-bottom: 15px;">
                        <div style="width: 15%; float: left;">
                            <%=GetLocaleResourceString("Admin.Localizable.Standard")%>:
                        </div>
                        <div style="width: 85%; float: left;">
                            <%} %><nopCommerce:SimpleTextBox runat="server" ID="txtOptionName" ErrorMessage="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.ErrorMessage %>"
                                Text='<%# Eval("Name") %>' ValidationGroup="SpecificationAttributeOption" CssClass="adminInput"
                                Width="100%" />
                            <asp:HiddenField ID="hfSpecificationAttributeOptionId" runat="server" Value='<%# Eval("SpecificationAttributeOptionId") %>' />
                        </div>
                    </div>
                    <%if (this.HasLocalizableContent)
                      { %>
                    <asp:Repeater ID="rptrLanguageDivs2" runat="server" OnItemDataBound="rptrLanguageDivs2_ItemDataBound">
                        <ItemTemplate>
                            <div style="clear: both; padding-bottom: 15px;">
                                <div style="width: 15%; float: left;">
                                    <asp:Image runat="server" ID="imgCFlag" Visible='<%# !String.IsNullOrEmpty(Eval("IconURL").ToString()) %>'
                                    AlternateText='<%#Eval("Name")%>' ImageUrl='<%#Eval("IconURL").ToString()%>' /> <%#Server.HtmlEncode(Eval("Name").ToString())%>:
                                </div>
                                <div style="width: 85%; float: left;">
                                    <asp:TextBox runat="server" ID="txtLocalizedOptionName" CssClass="adminInput" Width="100%" />
                                    <asp:Label ID="lblLanguageId" runat="server" Text='<%#Eval("LanguageId") %>' Visible="false"></asp:Label>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <%} %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.DisplayOrder %>"
                ItemStyle-Width="10%">
                <ItemTemplate>
                    <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" Width="50px" ID="txtOptionDisplayOrder"
                        Value='<%# Eval("DisplayOrder") %>' RequiredErrorMessage="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.DisplayOrder.RequiredErrorMessage %>"
                        RangeErrorMessage="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.DisplayOrder.RangeErrorMessage %>"
                        ValidationGroup="SpecificationAttributeOption" MinimumValue="-99999" MaximumValue="99999" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.Update %>"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Button ID="btnUpdate" runat="server" CssClass="adminButton" Text="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.Update %>"
                        ValidationGroup="SpecificationAttributeOption" CommandName="UpdateOption" ToolTip="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.Update.Tooltip %>" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.Delete %>"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Button ID="btnDelete" runat="server" CssClass="adminButton" Text="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.Delete %>"
                        CausesValidation="false" CommandName="Delete" ToolTip="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.Delete.Tooltip %>" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <p>
                <%#GetLocaleResourceString("Admin.SpecificationAttributeInfo.AttributeOption.NoOptions")%></p>
        </EmptyDataTemplate>
    </asp:GridView>
    <p>
        <asp:Button ID="btnAddSpecificationAttributeOption" runat="server" CssClass="adminButtonBlue"
            Text="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.NewButton.Text %>"
            OnClick="btnAddSpecificationAttributeOption_Click" CausesValidation="false" ToolTip="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.NewButton.Tooltip %>" /></p>
</div>
