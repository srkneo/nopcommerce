<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.ProductInfoControl"
    CodeBehind="ProductInfo.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductShareButton" Src="~/Modules/ProductShareButton.ascx" %>

<script language="javascript" type="text/javascript">
    function UpdateMainImage(url) {
        var imgMain = document.getElementById('<%=defaultImage.ClientID%>');
        imgMain.src = url;
    }
</script>

<div class="product-details-info">
    <div class="picture">
        <a href="<%= DefaultPictureUrl %>" rel="lightbox-p" title="<%= lProductName.Text%>">
            <asp:Image ID="defaultImage" runat="server" />
        </a>
    </div>
    <div class="overview">
        <h3 class="productname">
            <asp:Literal ID="lProductName" runat="server" />
        </h3>
        <br />
        <div class="shortdescription">
            <asp:Literal ID="lShortDescription" runat="server" />
        </div>
        <asp:ListView ID="lvProductPictures" runat="server" GroupItemCount="3">
            <LayoutTemplate>
                <table>
                    <asp:PlaceHolder runat="server" ID="groupPlaceHolder"></asp:PlaceHolder>
                </table>
            </LayoutTemplate>
            <GroupTemplate>
                <tr>
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                </tr>
            </GroupTemplate>
            <ItemTemplate>
                <td align="left">
                    <a href="<%#PictureManager.GetPictureUrl((int)Eval("PictureId"))%>" rel="lightbox-p"
                        title="<%= lProductName.Text%>">
                        <img src="<%#PictureManager.GetPictureUrl((int)Eval("PictureId"), 70)%>" alt="Product image" /></a>
                </td>
            </ItemTemplate>
        </asp:ListView>
        <div class="clear">
        </div>
        <nopCommerce:ProductShareButton ID="ctrlProductShareButton" runat="server" />
    </div>
    <div class="fulldescription">
        <asp:Literal ID="lFullDescription" runat="server" />
    </div>
</div>
