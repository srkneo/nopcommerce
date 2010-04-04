<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.TopicLocalizedDetailsControl"
    CodeBehind="TopicLocalizedDetails.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="SimpleTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ConfirmationBox" Src="ConfirmationBox.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-content.png" alt="<%=GetLocaleResourceString("Admin.TopicLocalizedDetails.Title")%>" />
        <%=GetLocaleResourceString("Admin.TopicLocalizedDetails.Title")%>
        <a href="Topics.aspx" title="<%=GetLocaleResourceString("Admin.TopicLocalizedDetails.BackToTopics")%>">
            (<%=GetLocaleResourceString("Admin.TopicLocalizedDetails.BackToTopics")%>)</a>
    </div>
    <div class="options">
        <asp:Button ID="SaveButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.TopicLocalizedDetails.SaveButton.Text %>"
            OnClick="SaveButton_Click" ToolTip="<% $NopResources:Admin.TopicLocalizedDetails.SaveButton.Tooltip %>" />
        <asp:Button ID="DeleteButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.TopicLocalizedDetails.DeleteButton.Text %>"
            OnClick="DeleteButton_Click" CausesValidation="false" ToolTip="<% $NopResources:Admin.TopicLocalizedDetails.DeleteButton.Tooltip %>" />
    </div>
</div>

<ajaxToolkit:TabContainer runat="server" ID="ProductTabs" ActiveTabIndex="0">
    <ajaxToolkit:TabPanel runat="server" ID="pnlProductSpecification" HeaderText="<% $NopResources:Admin.TopicLocalizedDetails.Info %>">
        <ContentTemplate>
            <table class="adminContent">
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblLanguageTitle" Text="<% $NopResources:Admin.TopicLocalizedDetails.Language %>"
                            ToolTip="<% $NopResources:Admin.TopicLocalizedDetails.Language.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblLanguage" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblTopicTitle" Text="<% $NopResources:Admin.TopicLocalizedDetails.Topic %>"
                            ToolTip="<% $NopResources:Admin.TopicLocalizedDetails.Topic.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblTopic" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblLocalizedTopicTitle" Text="<% $NopResources:Admin.TopicLocalizedDetails.LocalizedTopicTitle %>"
                            ToolTip="<% $NopResources:Admin.TopicLocalizedDetails.LocalizedTopicTitle.Tooltip %>"
                            ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:TextBox runat="server" CssClass="adminInput" ID="txtTitle">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblURL" Text="<% $NopResources:Admin.TopicLocalizedDetails.URL %>"
                            ToolTip="<% $NopResources:Admin.TopicLocalizedDetails.URL.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:HyperLink runat="server" ID="hlURL" Target="_blank" />
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblBody" Text="<% $NopResources:Admin.TopicLocalizedDetails.Body %>"
                            ToolTip="<% $NopResources:Admin.TopicLocalizedDetails.Body.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <HTMLEditor:Editor ID="txtBody" runat="server" Height="350" />
                    </td>
                </tr>
                <tr runat="server" id="pnlCreatedOn">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblCreatedOnTitle" Text="<% $NopResources:Admin.TopicLocalizedDetails.CreatedOn %>"
                            ToolTip="<% $NopResources:Admin.TopicLocalizedDetails.CreatedOn.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblCreatedOn" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="pnlUpdatedOn">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblUpdatedOnTitle" Text="<% $NopResources:Admin.TopicLocalizedDetails.UpdatedOn %>"
                            ToolTip="<% $NopResources:Admin.TopicLocalizedDetails.UpdatedOn.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblUpdatedOn" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlSEO" HeaderText="<% $NopResources:Admin.TopicLocalizedDetails.SEO %>">
        <ContentTemplate>
            <table class="adminContent">
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblTopicMetaKeywords" Text="<% $NopResources:Admin.TopicLocalizedDetails.MetaKeywords %>"
                            ToolTip="<% $NopResources:Admin.TopicLocalizedDetails.MetaKeywords.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:TextBox ID="txtMetaKeywords" CssClass="adminInput" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblTopicMetaDescription" Text="<% $NopResources:Admin.TopicLocalizedDetails.MetaDescription %>"
                            ToolTip="<% $NopResources:Admin.TopicLocalizedDetails.MetaDescription.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:TextBox ID="txtMetaDescription" CssClass="adminInput" runat="server" TextMode="MultiLine"
                            Height="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblTopicMetaTitle" Text="<% $NopResources:Admin.TopicLocalizedDetails.MetaTitle %>"
                            ToolTip="<% $NopResources:Admin.TopicLocalizedDetails.MetaTitle.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:TextBox ID="txtMetaTitle" CssClass="adminInput" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
</ajaxToolkit:TabContainer>
<nopCommerce:ConfirmationBox runat="server" ID="cbDelete" TargetControlID="DeleteButton"
    YesText="<% $NopResources:Admin.Common.Yes %>" NoText="<% $NopResources:Admin.Common.No %>"
    ConfirmText="<% $NopResources:Admin.Common.AreYouSure %>" />
