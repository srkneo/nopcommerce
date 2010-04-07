<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CustomerInfoControl"
    CodeBehind="CustomerInfo.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="EmailTextBox" Src="~/Modules/EmailTextBox.ascx" %>
<div class="customer-info-box">
    <div class="message-error">
        <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
        <br />
        <asp:ValidationSummary ID="valSum" runat="server" ShowSummary="true" DisplayMode="BulletList"
            ValidationGroup="CustomerInfo" />
    </div>
    <div class="clear">
    </div>
    <div class="section-title">
        <%=GetLocaleResourceString("Account.YourPersonalDetails")%>
    </div>
    <div class="clear">
    </div>
    <div class="section-body">
        <table class="table-container">
            <tbody>
                <tr class="row">
                    <td class="item-name">
                        <%=GetLocaleResourceString("Account.Gender")%>:
                    </td>
                    <td class="item-value">
                        <asp:RadioButton runat="server" ID="rbGenderM" GroupName="Gender" Text="<% $NopResources:Account.GenderMale %>"
                            Checked="true" />
                        <asp:RadioButton runat="server" ID="rbGenderF" GroupName="Gender" Text="<% $NopResources:Account.GenderFemale %>" />
                    </td>
                </tr>
                <tr class="row">
                    <td class="item-name">
                        <%=GetLocaleResourceString("Account.FirstName")%>:
                    </td>
                    <td class="item-value">
                        <asp:TextBox ID="txtFirstName" runat="server" MaxLength="40"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                            ErrorMessage="<% $NopResources:Account.FirstNameIsRequired %>" ToolTip="<% $NopResources:Account.FirstNameIsRequired %>"
                            ValidationGroup="CustomerInfo">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="row">
                    <td class="item-name">
                        <%=GetLocaleResourceString("Account.LastName")%>:
                    </td>
                    <td class="item-value">
                        <asp:TextBox ID="txtLastName" runat="server" MaxLength="40"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                            ErrorMessage="<% $NopResources:Account.LastNameIsRequired %>" ToolTip="<% $NopResources:Account.LastNameIsRequired %>"
                            ValidationGroup="CustomerInfo">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="row">
                    <td class="item-name">
                        <%=GetLocaleResourceString("Account.DateOfBirth")%>:
                    </td>
                    <td class="item-value">
                        <asp:DropDownList runat="server" ID="lstBirthDateDays" AutoPostBack="false" ValidationGroup="CustomerInfo" />
                        <asp:DropDownList runat="server" ID="lstBirthDateMonths" AutoPostBack="false" ValidationGroup="CustomerInfo" />
                        <asp:DropDownList runat="server" ID="lstBirthDateYears" AutoPostBack="false" ValidationGroup="CustomerInfo" />
                    </td>
                </tr>
                <tr class="row">
                    <td class="item-name">
                        <%=GetLocaleResourceString("Account.E-Mail")%>:
                    </td>
                    <td class="item-value">
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="40"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="<% $NopResources:Account.E-MailRequired %>" ToolTip="<% $NopResources:Account.E-MailRequired %>"
                            Display="Dynamic" ValidationGroup="CustomerInfo">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server" ID="revEmail" Display="Dynamic" ControlToValidate="txtEmail"
                            ErrorMessage="<% $NopResources:Account.InvalidEmail %>" ToolTip="<% $NopResources:Account.InvalidEmail %>"
                            ValidationExpression="[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+" ValidationGroup="CustomerInfo">*</asp:RegularExpressionValidator>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="clear">
    </div>
    <div class="section-title">
        <%=GetLocaleResourceString("Account.CompanyDetails")%>
    </div>
    <div class="clear">
    </div>
    <div class="section-body">
        <table class="table-container">
            <tbody>
                <tr class="row">
                    <td class="item-name">
                        <%=GetLocaleResourceString("Account.CompanyName")%>:
                    </td>
                    <td class="item-value">
                        <asp:TextBox ID="txtCompany" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="clear">
    </div>
    <div class="section-title">
        <%=GetLocaleResourceString("Account.YourAddress")%>
    </div>
    <div class="clear">
    </div>
    <div class="section-body">
        <table class="table-container">
            <tbody>
                <tr class="row">
                    <td class="item-name">
                        <%=GetLocaleResourceString("Account.StreetAddress")%>:
                    </td>
                    <td class="item-value">
                        <asp:TextBox ID="txtStreetAddress" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvStreetAddress" runat="server" ControlToValidate="txtStreetAddress"
                            ErrorMessage="<% $NopResources:Account.StreetAddressIsRequired %>" ToolTip="<% $NopResources:Account.StreetAddressIsRequired %>"
                            ValidationGroup="CustomerInfo">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="row">
                    <td class="item-name">
                        <%=GetLocaleResourceString("Account.StreetAddress2")%>:
                    </td>
                    <td class="item-value">
                        <asp:TextBox ID="txtStreetAddress2" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr class="row">
                    <td class="item-name">
                        <%=GetLocaleResourceString("Account.PostCode")%>:
                    </td>
                    <td class="item-value">
                        <asp:TextBox ID="txtZipPostalCode" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvZipPostalCode" runat="server" ControlToValidate="txtZipPostalCode"
                            ErrorMessage="<% $NopResources:Account.ZipPostalCodeIsRequired %>" ToolTip="<% $NopResources:Account.ZipPostalCodeIsRequired %>"
                            ValidationGroup="CustomerInfo">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="row">
                    <td class="item-name">
                        <%=GetLocaleResourceString("Account.City")%>:
                    </td>
                    <td class="item-value">
                        <asp:TextBox ID="txtCity" runat="server" MaxLength="40"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity"
                            ErrorMessage="<% $NopResources:Account.CityIsRequired %>" ToolTip="<% $NopResources:Account.CityIsRequired %>"
                            ValidationGroup="CustomerInfo">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="row">
                    <td class="item-name">
                        <%=GetLocaleResourceString("Account.Country")%>:
                    </td>
                    <td class="item-value">
                        <asp:DropDownList ID="ddlCountry" AutoPostBack="True" runat="server" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"
                            Width="137px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="row">
                    <td class="item-name">
                        <%=GetLocaleResourceString("Account.StateProvince")%>:
                    </td>
                    <td class="item-value">
                        <asp:DropDownList ID="ddlStateProvince" AutoPostBack="False" runat="server" Width="137px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="clear">
    </div>
    <div class="section-title">
        <%=GetLocaleResourceString("Account.YourContactInformation")%>
    </div>
    <div class="clear">
    </div>
    <div class="section-body">
        <table class="table-container">
            <tbody>
                <tr class="row">
                    <td class="item-name">
                        <%=GetLocaleResourceString("Account.TelephoneNumber")%>:
                    </td>
                    <td class="item-value">
                        <asp:TextBox ID="txtPhoneNumber" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber"
                            ErrorMessage="<% $NopResources:Account.PhoneNumberIsRequired %>" ToolTip="<% $NopResources:Account.PhoneNumberIsRequired %>"
                            ValidationGroup="CustomerInfo">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="row">
                    <td class="item-name">
                        <%=GetLocaleResourceString("Account.FaxNumber")%>:
                    </td>
                    <td class="item-value">
                        <asp:TextBox ID="txtFaxNumber" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="clear">
    </div>
    <div class="section-title">
        <%=GetLocaleResourceString("Account.Options")%>
    </div>
    <div class="clear">
    </div>
    <div class="section-body">
        <table class="table-container">
            <tbody>
                <tr class="row">
                    <td class="item-name">
                        <%=GetLocaleResourceString("Account.Newsletter")%>:
                    </td>
                    <td class="item-value">
                        <asp:CheckBox ID="cbNewsletter" runat="server"></asp:CheckBox>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="clear">
    </div>
    <asp:PlaceHolder runat="server" ID="divPreferences">
        <div class="section-title">
            <%=GetLocaleResourceString("Account.Preferences")%>
        </div>
        <div class="clear">
        </div>
        <div class="section-body">
            <table class="table-container">
                <tbody>
                    <tr class="row" runat="server" id="trTimeZone">
                        <td class="item-name">
                            <%=GetLocaleResourceString("Account.TimeZone")%>:
                        </td>
                        <td class="item-value">
                            <asp:DropDownList ID="ddlTimeZone" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="row" runat="server" id="trSignature">
                        <td class="item-name">
                            <%=GetLocaleResourceString("Account.Signature")%>:
                        </td>
                        <td class="item-value">
                            <asp:TextBox ID="txtSignature" runat="server" TextMode="MultiLine" MaxLength="300"
                                SkinID="AccountSignatureText"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="clear">
        </div>
    </asp:PlaceHolder>
    <div class="button">
        <asp:Button runat="server" ID="btnSaveCustomerInfo" Text="<% $NopResources:Account.Save %>"
            ValidationGroup="CustomerInfo" OnClick="btnSaveCustomerInfo_Click" CssClass="savecustomerinfobutton">
        </asp:Button>
    </div>
</div>
