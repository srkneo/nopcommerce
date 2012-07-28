alter table ProductVariantAttributeValue
MODIFY PriceAdjustment decimal(18,2),
MODIFY WeightAdjustment decimal(18,2);
-- GO

alter table ProductVariant
MODIFY AdditionalShippingCharge decimal(18,2),
MODIFY Price decimal(18,2),
MODIFY OldPrice decimal(18,2),
MODIFY ProductCost decimal(18,2),
MODIFY SpecialPrice decimal(18,2),
MODIFY MinimumCustomerEnteredPrice decimal(18,2),
MODIFY MaximumCustomerEnteredPrice decimal(18,2),
MODIFY Weight decimal(18,2),
MODIFY Length decimal(18,2),
MODIFY Width decimal(18,2),
MODIFY Height decimal(18,2);
-- GO

alter table TierPrice
MODIFY Price decimal(18,2);
-- GO

alter table RewardPointsHistory
MODIFY UsedAmount decimal(18,2);
-- GO

alter table Currency
MODIFY Rate decimal(18,2);
-- GO

alter table MeasureDimension
MODIFY Ratio decimal(18,2);
-- GO

alter table MeasureWeight
MODIFY Ratio decimal(18,2);
-- GO

alter table Discount
MODIFY DiscountPercentage decimal(18,2),
MODIFY DiscountAmount decimal(18,2);
-- GO

alter table DiscountRequirement
MODIFY SpentAmount decimal(18,2);
-- GO

alter table CheckoutAttributeValue
MODIFY PriceAdjustment decimal(18,2),
MODIFY WeightAdjustment decimal(18,2);
-- GO

alter table GiftCard
MODIFY Amount decimal(18,2);
-- GO

alter table GiftCardUsageHistory
MODIFY UsedValue decimal(18,2);
-- GO

alter table `Order`
MODIFY CurrencyRate decimal(18,2),
MODIFY OrderSubtotalInclTax decimal(18,2),
MODIFY OrderSubtotalExclTax decimal(18,2),
MODIFY OrderSubTotalDiscountInclTax decimal(18,2),
MODIFY OrderSubTotalDiscountExclTax decimal(18,2),
MODIFY OrderShippingInclTax decimal(18,2),
MODIFY OrderShippingExclTax decimal(18,2),
MODIFY PaymentMethodAdditionalFeeInclTax decimal(18,2),
MODIFY PaymentMethodAdditionalFeeExclTax decimal(18,2),
MODIFY OrderTax decimal(18,2),
MODIFY OrderDiscount decimal(18,2),
MODIFY OrderTotal decimal(18,2),
MODIFY RefundedAmount decimal(18,2);
-- GO

alter table OrderProductVariant
MODIFY UnitPriceInclTax decimal(18,2),
MODIFY UnitPriceExclTax decimal(18,2),
MODIFY PriceInclTax decimal(18,2),
MODIFY PriceExclTax decimal(18,2),
MODIFY DiscountAmountInclTax decimal(18,2),
MODIFY DiscountAmountExclTax decimal(18,2),
MODIFY ItemWeight decimal(18,2);
-- GO

alter table ShoppingCartItem
MODIFY CustomerEnteredPrice decimal(18,2);
-- GO

alter table Shipment
MODIFY TotalWeight decimal(18,2);
-- GO