<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.ThirdPartyIntegrationControl"
    CodeBehind="ThirdPartyIntegration.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="SimpleTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="NumericTextBox" Src="NumericTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="DecimalTextBox" Src="DecimalTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="EmailTextBox" Src="EmailTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>

<div class="section-header">
    <div class="title">
        <img src="Common/ico-configuration.png" alt="<%=GetLocaleResourceString("Admin.ThirdPartyIntegration.Title")%>" />
        <%=GetLocaleResourceString("Admin.ThirdPartyIntegration.Title")%>
    </div>
    <div class="options">
        <asp:Button runat="server" Text="<% $NopResources:Admin.ThirdPartyIntegration.SaveButton.Text %>"
            CssClass="adminButtonBlue" ID="btnSave" OnClick="btnSave_Click" ToolTip="<% $NopResources:Admin.ThirdPartyIntegration.SaveButton.Tooltip %>" />
    </div>
</div>
<div>

    <script type="text/javascript">
        $(document).ready(function() {
            toggleQuickBooks();
        });

        function toggleQuickBooks() {
            if (getE('<%=cbQuickBooksEnabled.ClientID %>').checked) {
                $('#pnlQuickBooksUsername').show();
                $('#pnlQuickBooksPassword').show();
                $('#pnlQuickBooksItemRef').show();
                $('#pnlQuickBooksDiscountAccountRef').show();
                $('#pnlQuickBooksShippingAccountRef').show();
                $('#pnlQuickBooksSalesTaxAccountRef').show();
                $('#pnlQuickBooksSynButton').show();
                $('#pnlQuickBooksSep1').show();
                $('#pnlQuickBooksSep2').show();
            }
            else {
                $('#pnlQuickBooksUsername').hide();
                $('#pnlQuickBooksPassword').hide();
                $('#pnlQuickBooksItemRef').hide();
                $('#pnlQuickBooksDiscountAccountRef').hide();
                $('#pnlQuickBooksShippingAccountRef').hide();
                $('#pnlQuickBooksSalesTaxAccountRef').hide();
                $('#pnlQuickBooksSynButton').hide();
                $('#pnlQuickBooksSep1').hide();
                $('#pnlQuickBooksSep2').hide();
            }
        }
    </script>

    <ajaxToolkit:TabContainer runat="server" ID="ThirdPartyIntegrationTabs" ActiveTabIndex="0">
        <ajaxToolkit:TabPanel runat="server" ID="pnlQuickBooks" HeaderText="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.Title %>">
            <ContentTemplate>
                <table class="adminContent">
                    <tr>
                        <td class="adminTitle" colspan="2">
                            Notes:
                            <ul>
                                <li>
                                    You can download QuickBooks Web Connector from this
                                    <a href="http://marketplace.intuit.com/webconnector/" target='_blank'>
                                        link
                                    </a>
                                </li>
                                <li>
                                    To add your applciation to QuickBooks Web Connector you should build your own .qwc file.
                                    QWC file example:
                                    <pre>
                                        &lt;?xml version="1.0"?&gt;
                                        &lt;QBWCXML&gt;
                                          &lt;AppID&gt;&lt;/AppID&gt;
                                          &lt;AppName&gt;nopCommerce QuickBooks connector&lt;/AppName&gt;
                                          &lt;AppDescription&gt;nopCommerce QuickBooks connector service&lt;/AppDescription&gt;
                                          &lt;AppURL&gt;http://your_site/QBConnector.asmx&gt;
                                          &lt;AppSupport&gt;&lt;/AppSupport&gt;
                                          &lt;UserName&gt;qb_username&lt;/UserName&gt;
                                          &lt;OwnerID&gt;{67F3B9B1-86F1-4fcc-B1EE-566DE1813D20}&lt;/OwnerID&gt;
                                          &lt;FileID&gt;{A0A44FB5-33D9-4815-AC85-BC87A7E7D1EB}&lt;/FileID&gt;
                                          &lt;QBType&gt;QBFS&lt;/QBType&gt;
                                        &lt;/QBWCXML&gt;
                                    </pre>
                                </li>
                                <li>
                                    Before you start data exchange, ensure that item, discount account, shipping account and sales tax account references are valid.
                                </li>
                                <li>
                                    If something is going wrong look at the system log for details
                                </li>
                                <li>
                                    Use specified username for QWC file and password to add application to QBWC
                                </li>
                            </ul>
                        </td>
                    </tr>
                    <tr class="adminSeparator" id="pnlQuickBooksSep1">
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblQuickBooksEnabled" Text="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.Enabled %>"
                                ToolTip="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.Enabled.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbQuickBooksEnabled" />
                        </td>
                    </tr>
                    <tr id="pnlQuickBooksUsername">
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblQuickBooksUsername" Text="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.Username %>"
                                ToolTip="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.Username.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtQuickBooksUsername" CssClass="adminInput" ErrorMessage="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.Username.ErrorMessage %>" />
                        </td>
                    </tr>
                    <tr id="pnlQuickBooksPassword">
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblQuickBooksPassword" Text="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.Password %>"
                                ToolTip="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.Password.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtQuickBooksPassword" CssClass="adminInput" ErrorMessage="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.Password.ErrorMessage %>" />
                        </td>
                    </tr>
                    <tr id="pnlQuickBooksItemRef">
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblQuickBooksItemRef" Text="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.ItemRef %>"
                                ToolTip="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.ItemRef.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtQuickBooksItemRef" CssClass="adminInput" ErrorMessage="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.ItemRef.ErrorMessage %>" />
                        </td>
                    </tr>
                    <tr id="pnlQuickBooksDiscountAccountRef">
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblQuickBooksDiscountAccountRef" Text="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.DiscountAccountRef %>"
                                ToolTip="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.DiscountAccountRef.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtQuickBooksDiscountAccountRef" CssClass="adminInput" ErrorMessage="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.DiscountAccountRef.ErrorMessage %>" />
                        </td>
                    </tr>
                    <tr id="pnlQuickBooksShippingAccountRef">
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblQuickBooksShippingAccountRef" Text="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.ShippingAccountRef %>"
                                ToolTip="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.ShippingAccountRef.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtQuickBooksShippingAccountRef" CssClass="adminInput" ErrorMessage="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.ShippingAccountRef.ErrorMessage %>" />
                        </td>
                    </tr>
                    <tr id="pnlQuickBooksSalesTaxAccountRef">
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblQuickBooksSalesTaxAccountRef" Text="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.SalesTaxAccountRef %>"
                                ToolTip="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.SalesTaxAccountRef.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtQuickBooksSalesTaxAccountRef" CssClass="adminInput" ErrorMessage="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.SalesTaxAccountRef.ErrorMessage %>" />
                        </td>
                    </tr>
                    <tr class="adminSeparator" id="pnlQuickBooksSep2">
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr id="pnlQuickBooksSynButton">
                        <td colspan="2">
                            <asp:Button runat="server" ID="btnQuickBooksSyn" Text="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.SynButton.Text %>"
            CssClass="adminButtonBlue" OnClick="btnQuickBooksSyn_Click" ToolTip="<% $NopResources:Admin.ThirdPartyIntegration.QuickBooks.SynButton.Tooltip %>" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
</div>
