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
