--new locale resources
declare @resources xml
set @resources='
<Language LanguageID="7">
  <LocaleResource Name="Admin.Localizable.Standard">
    <Value>Standard</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Localizable.EmptyFieldNote">
    <Value>Please note that if a field is left empty, the standard field will be used.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.FlagImageFileName">
    <Value>Flag image file name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.FlagImageFileName.Tooltip">
    <Value>The flag image file name. The image should be saved into \images\flags\ directory.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.PdfLogo">
    <Value>PDF logo:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.PdfLogo.Tooltip">
    <Value>Image file what will be displayed in PDF order invoices.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.PdfLogoRemove">
    <Value>Remove</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.PdfLogoRemove.Tooltip">
    <Value>Remove PDF logo image.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.CopyImages">
    <Value>Copy images:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SMSAlerts.SendTestSMS">
    <Value>Send test SMS(save settings first by clicking "Save" button)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SMSAlerts.TestPhone">
    <Value>Test phone number:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SMSAlerts.TestPhone.Tooltip">
    <Value>Enter the test phone number.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SMSAlerts.SendTestSMSButton">
    <Value>Send</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SMSAlerts.SendTestSMSButton.Tooltip">
    <Value>Click to send a test SMS</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SMSAlerts.SendTestSMSSuccess">
    <Value>SMS has been sent.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SMSAlerts.SendTestSMSFail">
    <Value>SMS has not been sent.</Value>
  </LocaleResource>
  <LocaleResource Name="DatePicker2.Day">
    <Value>Day</Value>
  </LocaleResource>
  <LocaleResource Name="DatePicker2.Month">
    <Value>Month</Value>
  </LocaleResource>
  <LocaleResource Name="DatePicker2.Year">
    <Value>Year</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ForumUrl">
    <Value>Forum URL rewrite format:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ForumUrl.Tooltip">
    <Value>The format for forum urls. Must have 3 arguments i.e. "{0}boards/f/{1}/{2}.aspx"</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ForumUrl.ErrorMessage">
    <Value>Invalid format for forum urls.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ForumGroupUrl">
    <Value>Forum Group URL rewrite format:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ForumGroupUrl.Tooltip">
    <Value>The format for forum group urls. Must have 3 arguments i.e. "{0}boards/fg/{1}/{2}.aspx"</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ForumGroupUrl.ErrorMessage">
    <Value>Invalid format for forum group urls.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ForumTopicUrl">
    <Value>Forum Topic URL rewrite format:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ForumTopicUrl.Tooltip">
    <Value>The format for forum topic urls. Must have 3 arguments i.e. "{0}boards/t/{1}/{2}.aspx"</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ForumTopicUrl.ErrorMessage">
    <Value>Invalid format for forum topic urls.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogSettings.PostsPageSize">
    <Value>Posts page size:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogSettings.PostsPageSize.Tooltip">
    <Value>Set the page size for posts</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogSettings.PostsPageSize.RequiredErrorMessage">
    <Value>Post page size is required.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogSettings.PostsPageSize.RangeErrorMessage">
    <Value>The value must be from 0 to 999999.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAdd.CustomerRolePrices">
    <Value>Prices By Customer Roles</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantDetails.CustomerRolePrices">
    <Value>Prices By Customer Roles</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.CustomerRole">
    <Value>Customer role</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.Price">
    <Value>Price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.Price.RequiredErrorMessage">
    <Value>Price is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.Price.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.Update">
    <Value>Update</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.Update.Tooltip">
    <Value>Update price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.Delete">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.Delete.Tooltip">
    <Value>Delete price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.AddNew">
    <Value>Add new price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.New.CustomerRole">
    <Value>Customer role:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.New.CustomerRole.Tooltip">
    <Value>Select a customer role</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.New.Price">
    <Value>Price:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.New.Price.Tooltip">
    <Value>Price for a selected customer role</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.New.Price.RequiredErrorMessage">
    <Value>Price is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.New.Price.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.New.AddNewButton.Text">
    <Value>Add price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.New.AddNewButton.Tooltip">
    <Value>Add price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.AvailableAfterSaving">
    <Value>You need to save the product variant before you can enable this pricing feature for this product variant page.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPricesByCustomerRole.NoCustomerRoleDefined">
    <Value>No customer roles defined. Create at least one customer role.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.Active">
    <Value>Is Active:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.Active.Tooltip">
    <Value>Indicating whether the message template is active.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionProvidersHome.Froogle.TitleDescription">
    <Value>Manage froogle (Google Base).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionProvidersHome.Froogle.Title">
    <Value>Froogle (Google Base)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Froogle.Title">
    <Value>Froogle (Google Base)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.FroogleTitle">
    <Value>Froogle (Google Base)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.FroogleDescription">
    <Value>Manage Froogle (Google Base) Provider</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.StartDate.Tooltip">
    <Value>The start date for the search.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.EndDate.Tooltip">
    <Value>The end date for the search.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.StartDate.Tooltip">
    <Value>The start date for the report.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.EndDate.Tooltip">
    <Value>The end date for the report.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.RegistrationFrom.Tooltip">
    <Value>The registration from date for the search.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.RegistrationTo.Tooltip">
    <Value>The registration to date for the search.</Value>
  </LocaleResource>  
  <LocaleResource Name="Admin.MessageQueue.StartDate.Tooltip">
    <Value>The start date for the search.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.EndDate.Tooltip">
    <Value>The end date for the search.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ActivityLog.CreatedOnFrom.Tooltip">
    <Value>The creation from date for the search.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ActivityLog.CreatedOnTo.Tooltip">
    <Value>The creation to date for the search.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PurchasedGiftCards.PurchasedFrom.Tooltip">
    <Value>The purchased from date for the search.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PurchasedGiftCards.PurchasedTo.Tooltip">
    <Value>The purchased to date for the search.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.CustomerEntersPrice">
    <Value>Customer enters price:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.CustomerEntersPrice.Tooltip">
    <Value>An option indicating whether customer should enter price.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.MinimumCustomerEnteredPrice">
    <Value>Minimum amount:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.MinimumCustomerEnteredPrice.Tooltip">
    <Value>Enter a minimum amount.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.MinimumCustomerEnteredPrice.RequiredErrorMessage">
    <Value>Minimum amount is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.MinimumCustomerEnteredPrice.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.MaximumCustomerEnteredPrice">
    <Value>Maximum amount:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.MaximumCustomerEnteredPrice.Tooltip">
    <Value>Enter a maximum amount.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.MaximumCustomerEnteredPrice.RequiredErrorMessage">
    <Value>Maximum amount is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.MaximumCustomerEnteredPrice.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.CustomerEntersPrice">
    <Value>Customer enters price:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.CustomerEntersPrice.Tooltip">
    <Value>An option indicating whether customer should enter price.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.MinimumCustomerEnteredPrice">
    <Value>Minimum amount:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.MinimumCustomerEnteredPrice.Tooltip">
    <Value>Enter a minimum amount.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.MinimumCustomerEnteredPrice.RequiredErrorMessage">
    <Value>Minimum amount is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.MinimumCustomerEnteredPrice.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.MaximumCustomerEnteredPrice">
    <Value>Maximum amount:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.MaximumCustomerEnteredPrice.Tooltip">
    <Value>Enter a maximum amount.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.MaximumCustomerEnteredPrice.RequiredErrorMessage">
    <Value>Maximum amount is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.MaximumCustomerEnteredPrice.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Products.EnterProductPrice">
    <Value>Please enter your price:</Value>
  </LocaleResource>
  <LocaleResource Name="Products.CustomerEnteredPrice.EnterPrice">
    <Value>Enter price</Value>
  </LocaleResource>
  <LocaleResource Name="Products.CustomerEnteredPrice.Range">
    <Value>The price must be from {0} to {1}</Value>
  </LocaleResource>
  <LocaleResource Name="ShoppingCart.CustomerEnteredPrice.RangeError">
    <Value>The price must be from {0} to {1}</Value>
  </LocaleResource>
  <LocaleResource Name="Checkout.IAcceptTermsOfService">
    <Value>I agree with the terms of service and I adhere to them unconditionally {0}.</Value>
  </LocaleResource>
  <LocaleResource Name="Checkout.PleaseAcceptTermsOfService">
    <Value>Please accept the terms of service before the next step.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.General.TermsOfService">
    <Value>Terms of service</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.General.TermsOfService.Tooltip">
    <Value>Require customers to accept or decline terms of service before processing the order</Value>
  </LocaleResource>
  <LocaleResource Name="Checkout.AcceptTermsOfService.Read">
    <Value>(read)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CheckoutAttributesTitle">
    <Value>Checkout Attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CheckoutAttributesDescription">
    <Value>Manage Checkout Attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AttributesHome.CheckoutAttributes.TitleDescription">
    <Value>Manage checkout attributes.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AttributesHome.CheckoutAttributes.Title">
    <Value>Checkout Attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AttributesHome.CheckoutAttributes.Description">
    <Value>You can create checkout attributes to provide your customers with more options during checkout (e.g. Offer gift wrapping or messaging options)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributes.Title">
    <Value>Checkout Attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributes.AddButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributes.AddButton.Tooltip">
    <Value>Add a new checkout attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributes.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributes.IsRequired">
    <Value>Is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributes.AttributeControlType">
    <Value>Control type</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributes.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributes.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.Name.Text">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.Name.Tooltip">
    <Value>The name of the checkout attribute.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.TextPrompt.Text">
    <Value>Text prompt:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.TextPrompt.Tooltip">
    <Value>Enter text prompt.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.Required">
    <Value>Required:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.Required.Tooltip">
    <Value>When an attribute is required, the customer must choose an appropriate attribute value before they can continue.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.ShippableProductRequired">
    <Value>Shippable product required:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.ShippableProductRequired.Tooltip">
    <Value>An option indicating whether shippable products are required in order to display this attribute.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.TaxExempt">
    <Value>Tax exempt:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.TaxExempt.Tooltip">
    <Value>Determines whether this option is tax exempt (tax will not be applied to this option at checkout).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.TaxCategory">
    <Value>Tax category:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.TaxCategory.Tooltip">
    <Value>The tax classification for this option. You can manage tax classifications from Configuration : Tax : Tax Classes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.ControlType">
    <Value>Control Type:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.ControlType.Tooltip">
    <Value>Choose how to display your attribute values.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.DisplayOrder.Tooltip">
    <Value>The product attribute display order. 1 represents the first item in the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeAdd.AddNew">
    <Value>Add a new checkout attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GetLocaleResourceString.BackToList">
    <Value>back to checkout attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeAdd.SaveButton.Tooltip">
    <Value>Save attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeAdd.AttributeInfo">
    <Value>Attribute Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeAdd.Values">
    <Value>Attribute Values</Value>
  </LocaleResource>
  <LocaleResource Name="ActivityLog.AddNewCheckoutAttribute">
    <Value>Added a new checkout attribute (''{0}'')</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeDetails.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeDetails.BackToList">
    <Value>back to checkout attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeDetails.SaveButton.Tooltip">
    <Value>Save attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeDetails.DeleteButton.Tooltip">
    <Value>Delete attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeDetails.AttributeInfo">
    <Value>Attribute Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeDetails.Values">
    <Value>Attribute Values</Value>
  </LocaleResource>
  <LocaleResource Name="ActivityLog.EditCheckoutAttribute">
    <Value>Edited a checkout attribute (''{0}'')</Value>
  </LocaleResource>
  <LocaleResource Name="ActivityLog.DeleteCheckoutAttribute">
    <Value>Deleted a checkout attribute (''{0}'')</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.AvailableAfterSaving">
    <Value>You need to save the checkout attribute before you can add values for this checkout attribute page.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.Name.Tooltip">
    <Value>The attribute value name e.g. ''Yes'' or ''No'' for Gift Wrapping checkout attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.PriceAdjustment">
    <Value>Price adjustment:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.PriceAdjustment.Tooltip">
    <Value>The price adjustment applied when choosing this attribute value e.g. ''10'' to add 10 euros.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.PriceAdjustment.RequiredErrorMessage">
    <Value>Price adjustment is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.PriceAdjustment.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.WeightAdjustment">
    <Value>Weight adjustment:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.WeightAdjustment.Tooltip">
    <Value>The weight adjustment applied when choosing this attribute value</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.WeightAdjustment.RequiredErrorMessage">
    <Value>Weight adjustment is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.WeightAdjustment.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.PreSelected">
    <Value>Pre-selected:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.PreSelected.Tooltip">
    <Value>Determines whether this attribute value is pre selected for the customer.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.DisplayOrder.Tooltip">
    <Value>The display order of the attribute value. 1 represents the first item in attribute value list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.AddNewButton.Text">
    <Value>Add</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeValues.New.AddNewButton.Tooltip">
    <Value>Add attribute value</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfoValues.AddNew">
    <Value>Add new values</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.PriceAdjustment">
    <Value>Price adjustment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.PriceAdjustment.RequiredErrorMessage">
    <Value>Price adjustment is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.PriceAdjustment.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.WeightAdjustment">
    <Value>Weight adjustment:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.WeightAdjustment.RequiredErrorMessage">
    <Value>Weight adjustment is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.WeightAdjustment.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.IsPreSelected">
    <Value>Is pre-selected</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.Update">
    <Value>Update</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.Delete">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CheckoutAttributeInfo.ValuesNotRequiredForThisControlType">
    <Value>Values are not required for this attribute control type</Value>
  </LocaleResource>
  <LocaleResource Name="OrderStatus.Pending">
    <Value>Pending</Value>
  </LocaleResource>
  <LocaleResource Name="OrderStatus.Processing">
    <Value>Processing</Value>
  </LocaleResource>
  <LocaleResource Name="OrderStatus.Complete">
    <Value>Complete</Value>
  </LocaleResource>
  <LocaleResource Name="OrderStatus.Cancelled">
    <Value>Cancelled</Value>
  </LocaleResource>
  <LocaleResource Name="ShippingStatus.ShippingNotRequired">
    <Value>Shipping not required</Value>
  </LocaleResource>
  <LocaleResource Name="ShippingStatus.NotYetShipped">
    <Value>Not yet shipped</Value>
  </LocaleResource>
  <LocaleResource Name="ShippingStatus.Shipped">
    <Value>Shipped</Value>
  </LocaleResource>
  <LocaleResource Name="PaymentStatus.Pending">
    <Value>Pending</Value>
  </LocaleResource>
  <LocaleResource Name="PaymentStatus.Authorized">
    <Value>Authorized</Value>
  </LocaleResource>
  <LocaleResource Name="PaymentStatus.Paid">
    <Value>Paid</Value>
  </LocaleResource>
  <LocaleResource Name="PaymentStatus.Refunded">
    <Value>Refunded</Value>
  </LocaleResource>
  <LocaleResource Name="PaymentStatus.Voided">
    <Value>Voided</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.Title">
    <Value>Reward Points</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.Enabled">
    <Value>Enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.Enabled.Tooltip">
    <Value>Check if you want to enable the Reward Points Program.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.Rate">
    <Value>Exchange rate:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.Rate.Tooltip">
    <Value>Specify reward points exchange rate.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.Rate.Tooltip2">
    <Value>1 reward point = </Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.EarningRewardPoints">
    <Value>Earning Reward Points:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.PointsForRegistration">
    <Value>Points for registration:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.PointsForRegistration.Tooltip">
    <Value>Specify number of points awarded for registration.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.PointsForRegistration.RequiredErrorMessage">
    <Value>Points for registration is required.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.PointsForRegistration.RangeErrorMessage">
    <Value>The value must be from 0 to 999999.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.PointsForPurchases">
    <Value>Points for purchases:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.PointsForPurchases.Tooltip">
    <Value>Specify number of points awarded for purchases.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.PointsForPurchases_Amount.RequiredErrorMessage">
    <Value>Points for purchases is required.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.PointsForPurchases_Amount.RangeErrorMessage">
    <Value>The value must be from 0 to 999999.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.PointsForPurchases_Points.RequiredErrorMessage">
    <Value>Points for purchases is required.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.PointsForPurchases_Points.RangeErrorMessage">
    <Value>The value must be from 0 to 999999.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.PointsForPurchases.Awarded">
    <Value>Awarded order status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.PointsForPurchases.Awarded.Tooltip">
    <Value>Points are awarded when the order status is...</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.PointsForPurchases.Canceled">
    <Value>Canceled order status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.RewardPoints.PointsForPurchases.Canceled.Tooltip">
    <Value>Points are canceled when the order status is...</Value>
  </LocaleResource>
  <LocaleResource Name="Checkout.UseRewardPoints">
    <Value>Use my reward points, {0} reward points ({1}) available</Value>
  </LocaleResource>
  <LocaleResource Name="ShoppingCart.Totals.RewardPoints">
    <Value>{0} reward points</Value>
  </LocaleResource>
  <LocaleResource Name="RewardPoints.Message.RedeemedForOrder">
    <Value>Redeemed for order #{0}</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerDetails.RewardPoints">
    <Value>Reward Points</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRewardPoints.Disabled">
    <Value>The Reward Points Program is disabled</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRewardPoints.Grid.Points">
    <Value>Points</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRewardPoints.Grid.Balance">
    <Value>Balance</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRewardPoints.Grid.Message">
    <Value>Message</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRewardPoints.Grid.Date">
    <Value>Date</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRewardPoints.Add">
    <Value>Add points</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRewardPoints.Add.Points">
    <Value>Points:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRewardPoints.Add.Points.Tooltip">
    <Value>Enter points to add.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRewardPoints.Add.Points.RequiredErrorMessage">
    <Value>Points field is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRewardPoints.Add.Points.RangeErrorMessage">
    <Value>The value must be from -999999 to 999999.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRewardPoints.Add.Message">
    <Value>Message:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRewardPoints.Add.Message.Tooltip">
    <Value>Enter message (comment).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRewardPoints.Add.Message.ErrorMessage">
    <Value>Message is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRewardPoints.AddButton.Text">
    <Value>Add reward points</Value>
  </LocaleResource>
  <LocaleResource Name="Order.Totals.RewardPoints">
    <Value>{0} reward points</Value>
  </LocaleResource>
  <LocaleResource Name="Order.ProductsGrid.Total">
    <Value>Total</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.RewardPoints">
    <Value>{0} reward points:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.RewardPoints.Tooltip">
    <Value>Redeemed reward points.</Value>
  </LocaleResource>
  <LocaleResource Name="Customer.RewardPoints.Overview">
    <Value>Reward Points</Value>
  </LocaleResource>
  <LocaleResource Name="Customer.RewardPoints.History">
    <Value>History</Value>
  </LocaleResource>
  <LocaleResource Name="Customer.RewardPoints.NoHistory">
    <Value>There is no balance history yet</Value>
  </LocaleResource>
  <LocaleResource Name="Customer.RewardPoints.CurrentBalance">
    <Value>Your current balance is {0} reward points ({1}).</Value>
  </LocaleResource>
  <LocaleResource Name="Customer.RewardPoints.CurrentRate">
    <Value>Each {0} spent will earn {1} reward points.</Value>
  </LocaleResource>
  <LocaleResource Name="Customer.RewardPoints.Grid.Points">
    <Value>Points</Value>
  </LocaleResource>
  <LocaleResource Name="Customer.RewardPoints.Grid.Balance">
    <Value>Balance</Value>
  </LocaleResource>
  <LocaleResource Name="Customer.RewardPoints.Grid.Message">
    <Value>Message</Value>
  </LocaleResource>
  <LocaleResource Name="Customer.RewardPoints.Grid.Date">
    <Value>Date</Value>
  </LocaleResource>
  <LocaleResource Name="Account.RewardPoints">
    <Value>Reward Points</Value>
  </LocaleResource>
  <LocaleResource Name="RewardPoints.Message.EarnedForRegistration">
    <Value>Registered as customer</Value>
  </LocaleResource>
  <LocaleResource Name="RewardPoints.Message.EarnedForOrder">
    <Value>Earned promotion for order #{0}</Value>
  </LocaleResource>
  <LocaleResource Name="RewardPoints.Message.ReducedForOrder">
    <Value>Reduced promotion for order #{0}</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.ACL.Description">
    <Value>Manage access control list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.Title">
    <Value>Form Fields:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.Description">
    <Value>You can create and manage the form fields available during registration (public store) below.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.GenderEnabled">
    <Value>''Gender'' enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.GenderEnabled.Tooltip">
    <Value>Set if ''Gender'' is enabled.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.DateOfBirthEnabled">
    <Value>''Date of Birth'' enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.DateOfBirthEnabled.Tooltip">
    <Value>Set if ''Date of Birth'' is enabled.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.CompanyEnabled">
    <Value>''Company'' enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.CompanyEnabled.Tooltip">
    <Value>Set if ''Company'' is enabled.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.CompanyRequired">
    <Value>''Company'' required:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.CompanyRequired.Tooltip">
    <Value>Set if ''Company'' is required.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.StreetAddressEnabled">
    <Value>''Street Address'' enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.StreetAddressEnabled.Tooltip">
    <Value>Set if ''Street Address'' is enabled.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.StreetAddressRequired">
    <Value>''Street Address'' required:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.StreetAddressRequired.Tooltip">
    <Value>Set if ''Street Address'' is required.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.StreetAddress2Enabled">
    <Value>''Street Address 2'' enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.StreetAddress2Enabled.Tooltip">
    <Value>Set if ''Street Address 2'' is enabled.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.StreetAddress2Required">
    <Value>''Street Address 2'' required:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.StreetAddress2Required.Tooltip">
    <Value>Set if ''Street Address 2'' is required.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.PostCodeEnabled">
    <Value>''Post Code'' enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.PostCodeEnabled.Tooltip">
    <Value>Set if ''Post Code'' is enabled.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.PostCodeRequired">
    <Value>''Post Code'' required:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.PostCodeRequired.Tooltip">
    <Value>Set if ''Post Code'' is required.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.CityEnabled">
    <Value>''City'' enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.CityEnabled.Tooltip">
    <Value>Set if ''City'' is enabled.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.CityRequired">
    <Value>''City'' required:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.CityRequired.Tooltip">
    <Value>Set if ''City'' is required.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.CountryEnabled">
    <Value>''Country'' enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.CountryEnabled.Tooltip">
    <Value>Set if ''Country'' is enabled.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.StateEnabled">
    <Value>''State/Province'' enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.StateEnabled.Tooltip">
    <Value>Set if ''State/Province'' is enabled.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.PhoneEnabled">
    <Value>''Phone Number'' enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.PhoneEnabled.Tooltip">
    <Value>Set if ''Phone Number'' is enabled.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.PhoneRequired">
    <Value>''Phone Number'' required:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.PhoneRequired.Tooltip">
    <Value>Set if ''Phone Number'' is required.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.FaxEnabled">
    <Value>''Fax Number'' enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.FaxEnabled.Tooltip">
    <Value>Set if ''Fax Number'' is enabled.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.FaxRequired">
    <Value>''Fax Number'' required:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.FormFields.FaxRequired.Tooltip">
    <Value>Set if ''Fax Number'' is required.</Value>
  </LocaleResource>
  <LocaleResource Name="Account.CompanyIsRequired">
    <Value>Company is required</Value>
  </LocaleResource>
  <LocaleResource Name="Account.StreetAddress2IsRequired">
    <Value>Street address 2 is required</Value>
  </LocaleResource>
  <LocaleResource Name="Account.FaxIsRequired">
    <Value>Fax is required</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.RewardPoints">
    <Value>{0} reward points:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerDetails.CurrentWishlist">
    <Value>Current Wishlist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerWishlist.Empty">
    <Value>Wishlist is empty</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerWishlist.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerWishlist.Name.Tooltip">
    <Value>View details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerWishlist.Price">
    <Value>Price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerWishlist.Quantity">
    <Value>Quantity</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerWishlist.Total">
    <Value>Total</Value>
  </LocaleResource>
  <LocaleResource Name="Common.AreYouSure">
    <Value>Are you sure?</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerWishlist.Disabled">
    <Value>Wishlist is disabled</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Categories.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Categories.Published">
    <Value>Published</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Categories.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Categories.Edit.Tooltip">
    <Value>Edit category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.DeleteButton.Text">
    <Value>Delete selected</Value>
  </LocaleResource>
  <LocaleResource Name="Account.DownloadableProducts.ProductsGrid.Download.na">
    <Value>n/a</Value>
  </LocaleResource>
  <LocaleResource Name="Order.ProductsGrid.Download.na">
    <Value>n/a</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GiftCardInfo.IsRecipientNotified">
    <Value>Is recipient notified:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GiftCardInfo.IsRecipientNotified.Tooltip">
    <Value>Is recipient has been notified.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GiftCardInfo.NotifyRecipientButton">
    <Value>Notify Recipient</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CustomerReportsTitle">
    <Value>Customer Statistics</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CustomerReportsDescription">
    <Value>Customer Statistics</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomersHome.CustomerReports.TitleDescription">
    <Value>Customer Statistics</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomersHome.CustomerReports.Title">
    <Value>Customer Statistics</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomersHome.CustomerReports.Description">
    <Value>View number of new customers and your best customers.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.Title">
    <Value>Customer Statistics</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByOrderTotal.Title">
    <Value>Top 20 customer by order total</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByNumberOfOrder.Title">
    <Value>Top 20 customers by number of orders</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByOrderTotal.StartDate">
    <Value>Start date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByOrderTotal.StartDate.Tooltip">
    <Value>The start date for the search.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByOrderTotal.EndDate">
    <Value>End date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByOrderTotal.EndDate.Tooltip">
    <Value>The end date for the search.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByOrderTotal.OrderStatus">
    <Value>Order status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByOrderTotal.OrderStatus.Tooltip">
    <Value>Search by a specific order status e.g. Complete.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByOrderTotal.PaymentStatus">
    <Value>Payment status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByOrderTotal.PaymentStatus.Tooltip">
    <Value>Search by a specific payment status e.g. Paid.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByOrderTotal.ShippingStatus">
    <Value>Shipping status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByOrderTotal.ShippingStatus.Tooltip">
    <Value>Search by a specific shipping status e.g. Not yet shipped.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByOrderTotal.SearchButton">
    <Value>View report</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByOrderTotal.SearchButton.Tooltip">
    <Value>View report based on the criteria below</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByOrderTotal.CustomerColumn">
    <Value>Customer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByOrderTotal.OrderTotalColumn">
    <Value>Order total</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByOrderTotal.NumberOfOrdersColumn">
    <Value>Number of orders</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Common.CustomerGuest">
    <Value>Guest</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByNumberOfOrder.StartDate">
    <Value>Start date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByNumberOfOrder.StartDate.Tooltip">
    <Value>The start date for the search.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByNumberOfOrder.EndDate">
    <Value>End date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByNumberOfOrder.EndDate.Tooltip">
    <Value>The end date for the search.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByNumberOfOrder.OrderStatus">
    <Value>Order status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByNumberOfOrder.OrderStatus.Tooltip">
    <Value>Search by a specific order status e.g. Complete.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByNumberOfOrder.PaymentStatus">
    <Value>Payment status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByNumberOfOrder.PaymentStatus.Tooltip">
    <Value>Search by a specific payment status e.g. Paid.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByNumberOfOrder.ShippingStatus">
    <Value>Shipping status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByNumberOfOrder.ShippingStatus.Tooltip">
    <Value>Search by a specific shipping status e.g. Not yet shipped.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByNumberOfOrder.SearchButton">
    <Value>View report</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByNumberOfOrder.SearchButton.Tooltip">
    <Value>View report based on the criteria below</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByNumberOfOrder.CustomerColumn">
    <Value>Customer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByNumberOfOrder.OrderTotalColumn">
    <Value>Order total</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByNumberOfOrder.NumberOfOrdersColumn">
    <Value>Number of orders</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.RegisteredCustomers.Title">
    <Value>Registered customers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ShowAdminProductImages">
    <Value>Show product images in admin area:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ShowAdminProductImages.Tooltip">
    <Value>Check if you want to see the product images in admin area.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.Image">
    <Value>Image</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.Image">
    <Value>Image</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.Image">
    <Value>Image</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.Image">
    <Value>Image</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryProducts.Image">
    <Value>Image</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.RelatedProducts.Image">
    <Value>Image</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerProducts.Image">
    <Value>Image</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByLanguage.Title">
    <Value>Language Distribution</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByLanguage.Tooltip">
    <Value>Language distribution allows you to determine the general language your customers use on your shop.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByLanguage.LanguageColumn">
    <Value>Language</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByLanguage.CustomerCountColumn">
    <Value>Count</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Common.Unknown">
    <Value>Unknown</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByGender.Title">
    <Value>Gender Distribution</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByGender.Tooltip">
    <Value>Gender distribution allows you to determine the percentage of men and women among your customers.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByGender.GenderColumn">
    <Value>Gender</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByGender.CustomerCountColumn">
    <Value>Count</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByGender.Male">
    <Value>Male</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByGender.Female">
    <Value>Female</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByCountry.Title">
    <Value>Country Distribution</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByCountry.Tooltip">
    <Value>Country distribution allows you to determine in which part of the world your customers are.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByCountry.CountryColumn">
    <Value>Country</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerReports.ByCountry.CustomerCountColumn">
    <Value>Count</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.BillingCountry">
    <Value>Billing country:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.BillingCountry.Tooltip">
    <Value>The customer''s billing country.</Value>
  </LocaleResource>
  <LocaleResource Name="ShippingStatus.Delivered">
    <Value>Delivered</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.DeliveryDate">
    <Value>Delivery date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.DeliveryDate.Tooltip">
    <Value>The date this order was delivered.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.SetAsDeliveredButton.Text">
    <Value>Set as delivered</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippedDate.NotYet">
    <Value>Not yet</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.DeliveryDate.NotYet">
    <Value>Not yet</Value>
  </LocaleResource>
  <LocaleResource Name="Order.DeliveredOn">
    <Value>Delivered on</Value>
  </LocaleResource>
  <LocaleResource Name="Order.NotYetShipped">
    <Value>Not shipped yet</Value>
  </LocaleResource>
  <LocaleResource Name="Order.Order.NotYetDelivered">
    <Value>Not delivered yet</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.BulkEditProductsTitle">
    <Value>Bulk Edit Products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.BulkEditProductsDescription">
    <Value>Bulk Edit Products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductsHome.BulkEditProducts.TitleDescription">
    <Value>Bulk Edit Products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductsHome.BulkEditProducts.Title">
    <Value>Bulk Edit Products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductsHome.BulkEditProducts.Description">
    <Value>Want to make changes to multiple products at once? Bulk edit your products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.Title">
    <Value>Bulk Edit Products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.SearchButton.Text">
    <Value>Search</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.SearchButton.Tooltip">
    <Value>Search product variants based on the criteria below</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.UpdateButton.Text">
    <Value>Update selected</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.ProductName">
    <Value>Product name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.ProductName.Tooltip">
    <Value>A product name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.Category">
    <Value>Category:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.Category.Tooltip">
    <Value>Search by a specific category.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.Manufacturer">
    <Value>Manufacturer:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.Manufacturer.Tooltip">
    <Value>Search by a specific manufacturer.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.NameColumn">
    <Value>Full product variant name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.NoProductsFound">
    <Value>No product variants found</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.PriceColumn">
    <Value>Price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.PriceColumn.RequiredErrorMessage">
    <Value>Price is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.PriceColumn.RangeErrorMessage">
    <Value>The price must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.OldPriceColumn">
    <Value>Old price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.OldPriceColumn.RequiredErrorMessage">
    <Value>Old price is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.OldPriceColumn.RangeErrorMessage">
    <Value>The old price must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.PublishedColumn">
    <Value>Published</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.Description">
    <Value>Note: you''re editing product variants (not products)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BulkEditProducts.SuccessfullyUpdated">
    <Value>All product variants have been successfully updated.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BillingAddress.FullName">
    <Value>Full name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BillingAddress.FirstName">
    <Value>First name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BillingAddress.LastName">
    <Value>Last name:></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BillingAddress.Company">
    <Value>Company:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BillingAddress.Address1">
    <Value>Address 1:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BillingAddress.Address2">
    <Value>Address 2:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BillingAddress.City">
    <Value>City:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BillingAddress.StateProvince">
    <Value>State / Province:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BillingAddress.ZipPostalCode">
    <Value>Zip / PostalCode:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BillingAddress.Country">
    <Value>Country:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingAddress.FullName">
    <Value>Full name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingAddress.FirstName">
    <Value>First name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingAddress.LastName">
    <Value>Last name:></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingAddress.Company">
    <Value>Company:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingAddress.Address1">
    <Value>Address 1:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingAddress.Address2">
    <Value>Address 2:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingAddress.City">
    <Value>City:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingAddress.StateProvince">
    <Value>State / Province:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingAddress.ZipPostalCode">
    <Value>Zip / PostalCode:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingAddress.Country">
    <Value>Country:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.EditBillingAddressButton.Text">
    <Value>Edit address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CancelBillingAddressButton.Text">
    <Value>Cancel</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.SaveBillingAddressButton.Text">
    <Value>Save address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.EditShippingAddressButton.Text">
    <Value>Edit address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CancelShippingAddressButton.Text">
    <Value>Cancel</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.SaveShippingAddressButton.Text">
    <Value>Save address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PictureBrowser.Title">
    <Value>Image browser</Value>
  </LocaleResource>    
  <LocaleResource Name="Admin.PictureBrowser.Pictures">
    <Value>Pictures</Value>
  </LocaleResource>    
  <LocaleResource Name="Admin.PictureBrowser.PictureID">
    <Value>Picture ID</Value>
  </LocaleResource>    
  <LocaleResource Name="Admin.PictureBrowser.Picture">
    <Value>Picture</Value>
  </LocaleResource>    
  <LocaleResource Name="Admin.PictureBrowser.Details">
    <Value>Details</Value>
  </LocaleResource>    
  <LocaleResource Name="Admin.PictureBrowser.GenerateAnotherSize">
    <Value>Generate another size</Value>
  </LocaleResource>    
  <LocaleResource Name="Admin.PictureBrowser.GenerateAnotherSize.Tooltip">
    <Value>Generate another size</Value>
  </LocaleResource>    
  <LocaleResource Name="Admin.PictureBrowser.GenerateAnotherSize.RequiredErrorMessage">
    <Value>A size is required</Value>
  </LocaleResource>    
  <LocaleResource Name="Admin.PictureBrowser.GenerateAnotherSize.RangeErrorMessage">
    <Value>Minimum image width is 30, maximum is 2000</Value>
  </LocaleResource>    
  <LocaleResource Name="Admin.PictureBrowser.Insert">
    <Value>Insert</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PictureBrowser.UploadNewPicture">
    <Value>Upload a new picture</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PictureBrowser.UploadNewPicture.ToolTip">
    <Value>Upload a new picture</Value>
  </LocaleResource>  
  <LocaleResource Name="Admin.PictureBrowser.FileUpload">
    <Value>Browse for a picture on your harddrive</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PictureBrowser.SaveNewPicture">
    <Value>Save the new picture</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PictureBrowser.SaveNewPicture.ToolTip">
    <Value>Save the new picture</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PictureBrowser.PageSize">
    <Value>Page size</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PictureBrowser.PageSize.Tooltip">
    <Value>Page size</Value>
  </LocaleResource>
  <LocaleResource Name="Search.AdvancedSearch">
    <Value>Advanced search</Value>
  </LocaleResource>
  <LocaleResource Name="Search.AllCategories">
    <Value>All</Value>
  </LocaleResource>
  <LocaleResource Name="Search.AllManufacturers">
    <Value>All</Value>
  </LocaleResource>
  <LocaleResource Name="Search.Categories">
    <Value>Categories:</Value>
  </LocaleResource>
  <LocaleResource Name="Search.Manufacturers">
    <Value>Manufacturer:</Value>
  </LocaleResource>
  <LocaleResource Name="Search.PriceRange">
    <Value>Price range:</Value>
  </LocaleResource>
  <LocaleResource Name="Search.From">
    <Value>From</Value>
  </LocaleResource>
  <LocaleResource Name="Search.To">
    <Value>to</Value>
  </LocaleResource>
  <LocaleResource Name="Search.SearchKeyword">
    <Value>Search keyword:</Value>
  </LocaleResource>
  <LocaleResource Name="Forum.Sticky">
    <Value>Sticky</Value>
  </LocaleResource>      
  <LocaleResource Name="Admin.CustomerAvatar.Image">
    <Value>Avatar image:</Value>
  </LocaleResource>        
  <LocaleResource Name="Admin.CustomerAvatar.Image.Tooltip">
    <Value>Customer avatar.</Value>
  </LocaleResource>        
  <LocaleResource Name="Admin.CustomerAvatar.UploadAvatar">
    <Value>Upload</Value>
  </LocaleResource>        
  <LocaleResource Name="Admin.CustomerAvatar.RemoveAvatar">
    <Value>Remove</Value>
  </LocaleResource>        
  <LocaleResource Name="Admin.CustomerAvatar.UploadAvatarRules">
    <Value>Avatar must be in GIF or JPEG format with the maximum size of 20 KB</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerDetails.CustomerAvatar">
    <Value>Customer Avatar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.ShowOnHomePage">
    <Value>Show on home page:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.ShowOnHomePage.Tooltip">
    <Value>Check if you want to show a catgory on home page.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.General.HidePricesForNonRegistered">
    <Value>Hide prices for non-registered users:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.General.HidePricesForNonRegistered.Tooltip">
    <Value>Check to to disable product prices for all non-registered users so that anyone browsing the site cant see prices.</Value>
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
	
	
	FETCH NEXT FROM cur_localeresource INTO @LanguageID, @ResourceName, @ResourceValue
	END
CLOSE cur_localeresource
DEALLOCATE cur_localeresource

DROP TABLE #LocaleStringResourceTmp
GO
