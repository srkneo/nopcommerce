﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Templates.Products.VariantsInGrid"
    CodeBehind="VariantsInGrid.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductVariantsInGrid" Src="~/Modules/ProductVariantsInGrid.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductInfo" Src="~/Modules/ProductInfo.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductCategoryBreadcrumb" Src="~/Modules/ProductCategoryBreadcrumb.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductRating" Src="~/Modules/ProductRating.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductEmailAFriendButton" Src="~/Modules/ProductEmailAFriendButton.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductAddToCompareList" Src="~/Modules/ProductAddToCompareList.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductSpecs" Src="~/Modules/ProductSpecifications.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="RelatedProducts" Src="~/Modules/RelatedProducts.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductReviews" Src="~/Modules/ProductReviews.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductsAlsoPurchased" Src="~/Modules/ProductsAlsoPurchased.ascx" %>
<nopCommerce:ProductCategoryBreadcrumb ID="ctrlProductCategoryBreadcrumb" runat="server">
</nopCommerce:ProductCategoryBreadcrumb>
<div class="clear">
</div>
<div class="ProductDetailsPage">
    <div class="ProductEssential">
        <nopCommerce:ProductInfo ID="ctrlProductInfo" runat="server"></nopCommerce:ProductInfo>
    </div>
    <div class="clear">
    </div>
    <div class="ProductCollateral">
        <nopCommerce:ProductRating ID="ctrlProductRating" runat="server"></nopCommerce:ProductRating>
        <div class="clear">
        </div>
        <nopCommerce:ProductEmailAFriendButton ID="ctrlProductEmailAFriendButton" runat="server">
        </nopCommerce:ProductEmailAFriendButton>
        &nbsp;
        <nopCommerce:ProductAddToCompareList ID="ctrlProductAddToCompareList" runat="server">
        </nopCommerce:ProductAddToCompareList>
        <div class="clear">
        </div>
        <nopCommerce:ProductVariantsInGrid ID="ctrlProductVariantsInGrid" runat="server">
        </nopCommerce:ProductVariantsInGrid>
        <div class="clear">
        </div>
        <div>
            <nopCommerce:ProductsAlsoPurchased ID="ctrlProductsAlsoPurchased" runat="server" />
        </div>
        <div class="clear">
        </div>
        <div>
            <nopCommerce:RelatedProducts ID="ctrlRelatedProducts" runat="server"></nopCommerce:RelatedProducts>
        </div>
        <div class="clear">
        </div>
        <ajaxToolkit:TabContainer runat="server" ID="ProductsTabs" ActiveTabIndex="0" SkinID="TabContainer-ProductDetails">
            <ajaxToolkit:TabPanel runat="server" ID="pnlProductReviews" HeaderText="<% $NopResources:Products.ProductReviews %>">
                <ContentTemplate>
                    <nopCommerce:ProductReviews ID="ctrlProductReviews" runat="server" ShowWriteReview="true">
                    </nopCommerce:ProductReviews>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel runat="server" ID="pnlProductSpecs" HeaderText="<% $NopResources:Products.ProductSpecs %>">
                <ContentTemplate>
                    <nopCommerce:ProductSpecs ID="ctrlProductSpecs" runat="server"></nopCommerce:ProductSpecs>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
    </div>
</div>
