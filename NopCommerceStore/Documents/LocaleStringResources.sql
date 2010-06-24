--new locale resources
declare @resources xml
set @resources='
<Language LanguageID="7">
  <LocaleResource Name="Admin.GlobalSettings.GoogleAdsense.Title">
    <Value>Google Adsense</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.GoogleAdsense.Enabled">
    <Value>Enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.GoogleAdsense.Enabled.Tooltip">
    <Value>Check if you want to enable Google Adsense.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.GoogleAdsense.Code">
    <Value>Google adsense code:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.GoogleAdsense.Code.Tooltip">
    <Value>Place your google adsense code here.</Value>
  </LocaleResource>
  <LocaleResource Name="BlogRSS.Tooltip">
    <Value>Click here to receive automatic BLOG updates from our site</Value>
  </LocaleResource>
  <LocaleResource Name="NewsRSS.Tooltip">
    <Value>Click here to receive automatic NEWS updates from our site</Value>
  </LocaleResource>
  <LocaleResource Name="RecentlyAddedProductsRSS.Tooltip">
    <Value>Click here to be informed automatically when we add new items to our site</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogComments.IPAddress">
    <Value>(IP Address: {0})</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsComments.IPAddress">
    <Value>(IP Address: {0})</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviews.IPAddress">
    <Value>(IP Address: {0})</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.Title">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeAdd.AttributeInfo">
    <Value>Attribute info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeAdd.Options">
    <Value>Options</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeDetails.AttributeInfo">
    <Value>Attribute info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeDetails.Options">
    <Value>Options</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.AvailableAfterSaving">
    <Value>You need to save the specification attribute before you can add options for this specification attribute page.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOptions">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.ErrorMessage">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.DisplayOrder">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.DisplayOrder.RequiredErrorMessage">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.DisplayOrder.RangeErrorMessage">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.Update">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.Update.Tooltip">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.Delete">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.Delete.Tooltip">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.NoOptions">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.NewButton.Text">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.NewButton.Tooltip">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.Title">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.BackToAttributeDetails">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.SaveButton.Text">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.SaveButton.Tooltip">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.Name">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.Name.Tooltip">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.DisplayOrder">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.DisplayOrder.Tooltip">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.DisplayOrder.RequiredErrorMessage">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.DisplayOrder.RangeErrorMessage">
    <Value></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.Name">
    <Value>Option name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.Name.ErrorMessage">
    <Value>Option name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.Update">
    <Value>Update</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.Update.Tooltip">
    <Value>Update specification attribute option</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.Delete">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.Delete.Tooltip">
    <Value>Delete specification attribute option</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.NewButton.Text">
    <Value>Add new specification attribute option</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.AddNew">
    <Value>Add new options</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.New.Name">
    <Value>Option name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.New.Name.Tooltip">
    <Value>The name of the specification attribute option.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.New.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.New.DisplayOrder.Tooltip">
    <Value>The display order of the specification attribute option. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.New.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.New.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptions.New.AddNewButton.Text">
    <Value>Add</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.HidePaymentInfoForZeroOrders">
    <Value>Skip/hide payment info for "zero" total orders:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.HidePaymentInfoForZeroOrders.Tooltip">
    <Value>A Skip/hide payment information page if order total is zero.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ThirdPartyIntegrationTitle">
    <Value>Third-party Integration</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ThirdPartyIntegrationDescription">
    <Value>Manage third-party integration</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.Title">
    <Value>Third-party integration</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.SaveButton.Tooltip">
    <Value>Save changes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.Title">
    <Value>QucikBooks</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.Enabled">
    <Value>Enabled</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.Enabled.Tooltip">
    <Value>Check if you want to enable QuickBooks integration feature.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.Username">
    <Value>Username</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.Username.Tooltip">
    <Value>Username wich will be used by QuickBooks Web Connector to connect to Web service</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.Password">
    <Value>Password</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.Password.Tooltip">
    <Value>Password wich will be used by QuickBooks Web Connector to connect to Web service</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.ItemRef">
    <Value>Item reference</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.ItemRef.Tooltip">
    <Value>Full name of item. Item should exists in QuickBooks</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.Username.ErrorMessage">
    <Value>QuickBooks username is not specified</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.Password.ErrorMessage">
    <Value>QuickBooks password is not specified</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.ItemRef.ErrorMessage">
    <Value>QuickBooks item reference is not specified</Value>
  </LocaleResource>
  <LocaleResource Name="ActivityLog.EditThirdPartyIntegration">
    <Value>Third-party integration settings was changed</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.DicsountAccountRef">
    <Value>Dicsount account reference</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.DicsountAccountRef.Tooltip">
    <Value>Full name of account. Account should exists in QuickBooks</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.DicsountAccountRef.ErrorMessage">
    <Value>QuickBooks discount account reference is not specified</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.ShippingAccountRef">
    <Value>Shipping account reference</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.ShippingAccountRef.Tooltip">
    <Value>Full name of account. Account should exists in QuickBooks</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.ShippingAccountRef.ErrorMessage">
    <Value>QuickBooks shipping account reference is not specified</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.SalesTaxAccountRef">
    <Value>Sales tax account reference</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.SalesTaxAccountRef.Tooltip">
    <Value>Full name of account. Account should exists in QuickBooks</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ThirdPartyIntegration.QuickBooks.SalesTaxAccountRef.ErrorMessage">
    <Value>QuickBooks sales tax account reference is not specified</Value>
  </LocaleResource>
</Language>
'

CREATE TABLE #LocaleStringResourceTmp
	(
		[LanguageID] [int] NOT NULL,
		[ResourceName] [nvarchar](200) NOT NULL,
		[ResourceValue] [nvarchar](max) NOT NULL
	)


INSERT INTO #LocaleStringResourceTmp (LanguageID, ResourceName, ResourceValue)
SELECT	@resources.value('(/Language/@LanguageID)[1]', 'int'), nref.value('@Name', 'nvarchar(200)'), nref.value('Value[1]', 'nvarchar(MAX)')
FROM	@resources.nodes('//Language/LocaleResource') AS R(nref)

DECLARE @LanguageID int
DECLARE @ResourceName nvarchar(200)
DECLARE @ResourceValue nvarchar(MAX)
DECLARE cur_localeresource CURSOR FOR
SELECT LanguageID, ResourceName, ResourceValue
FROM #LocaleStringResourceTmp
OPEN cur_localeresource
FETCH NEXT FROM cur_localeresource INTO @LanguageID, @ResourceName, @ResourceValue
WHILE @@FETCH_STATUS = 0
BEGIN
	IF (EXISTS (SELECT 1 FROM Nop_LocaleStringResource WHERE LanguageID=@LanguageID AND ResourceName=@ResourceName))
	BEGIN
		UPDATE [Nop_LocaleStringResource]
		SET [ResourceValue]=@ResourceValue
		WHERE LanguageID=@LanguageID AND ResourceName=@ResourceName
	END
	ELSE 
	BEGIN
		INSERT INTO [Nop_LocaleStringResource]
		(
			[LanguageID],
			[ResourceName],
			[ResourceValue]
		)
		VALUES
		(
			@LanguageID,
			@ResourceName,
			@ResourceValue
		)
	END
	
	IF (@ResourceValue is null or @ResourceValue = '')
	BEGIN
		DELETE [Nop_LocaleStringResource]
		WHERE LanguageID=@LanguageID AND ResourceName=@ResourceName
	END
	
	FETCH NEXT FROM cur_localeresource INTO @LanguageID, @ResourceName, @ResourceValue
	END
CLOSE cur_localeresource
DEALLOCATE cur_localeresource

DROP TABLE #LocaleStringResourceTmp
GO


