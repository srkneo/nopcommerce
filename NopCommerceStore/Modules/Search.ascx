<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.SearchControl"
    CodeBehind="Search.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductBox2" Src="~/Modules/ProductBox2.ascx" %>
<div class="search-panel">
    <div class="page-title">
        <h1><%=GetLocaleResourceString("Search.Search")%></h1>
    </div>
    <div class="clear">
    </div>
    <div class="search-input">
        <table width="100%">
            <tbody>
                <tr>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtSearchTerm" Width="100%" SkinID="SearchText"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <br style="line-height: 5px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox runat="server" ID="chSearchInProductDescriptions" Checked="false" />
                        <%=GetLocaleResourceString("Search.SearchInProductDescriptions")%>
                    </td>
                    <td align="right">
                        <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="<% $NopResources:Search.SearchButton %>"
                            CssClass="searchbutton" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <u>
                            <asp:Label runat="server" ID="lblError" EnableViewState="false"></asp:Label>
                        </u>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="clear">
    </div>
    <div class="search-results">
        <asp:Label runat="server" ID="lblNoResults" Text="<% $NopResources:Search.NoResultsText %>" Visible="false" CssClass="result" />
        <div class="product-list1">
            <asp:ListView ID="lvProducts" runat="server" OnPagePropertiesChanging="lvProducts_OnPagePropertiesChanging">
                <LayoutTemplate>
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                </LayoutTemplate>
                <ItemTemplate>
                    <div class="item-box">
                        <nopCommerce:ProductBox2 ID="ctrlProductBox" Product='<%# Container.DataItem %>'
                            runat="server" />
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
        <div class="pager">
            <asp:DataPager ID="pagerProducts" runat="server" PagedControlID="lvProducts" PageSize="10">
                <Fields>
                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True" FirstPageText="<% $NopResources:Search.First %>" LastPageText="<% $NopResources:Search.Last %>" NextPageText="<% $NopResources:Search.Next %>" PreviousPageText="<% $NopResources:Search.Previous %>" />
                </Fields>
            </asp:DataPager>
        </div>
    </div>
</div>
