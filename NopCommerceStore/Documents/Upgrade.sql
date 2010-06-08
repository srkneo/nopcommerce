IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Media.Product.DefaultPictureZoomEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Media.Product.DefaultPictureZoomEnabled', N'False', N'')
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_LanguageLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_LanguageLoadAll]
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_LanguageDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_LanguageDelete]
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_LanguageInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_LanguageInsert]
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_LanguageLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_LanguageLoadByPrimaryKey]
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_LanguageUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_LanguageUpdate]
GO

--mappings
IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_Category_Discount_Mapping_Nop_Category'
           AND parent_obj = Object_id('Nop_Category_Discount_Mapping')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_Category_Discount_Mapping
DROP CONSTRAINT FK_Nop_Category_Discount_Mapping_Nop_Category
GO
ALTER TABLE [dbo].[Nop_Category_Discount_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Nop_Category_Discount_Mapping_Nop_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Nop_Category] ([CategoryID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_Category_Discount_Mapping_Nop_Discount'
           AND parent_obj = Object_id('Nop_Category_Discount_Mapping')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_Category_Discount_Mapping
DROP CONSTRAINT FK_Nop_Category_Discount_Mapping_Nop_Discount
GO
ALTER TABLE [dbo].[Nop_Category_Discount_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Nop_Category_Discount_Mapping_Nop_Discount] FOREIGN KEY([DiscountID])
REFERENCES [dbo].[Nop_Discount] ([DiscountID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_CustomerRole_Discount_Mapping_Nop_CustomerRole'
           AND parent_obj = Object_id('Nop_CustomerRole_Discount_Mapping')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_CustomerRole_Discount_Mapping
DROP CONSTRAINT FK_Nop_CustomerRole_Discount_Mapping_Nop_CustomerRole
GO
ALTER TABLE [dbo].[Nop_CustomerRole_Discount_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Nop_CustomerRole_Discount_Mapping_Nop_CustomerRole] FOREIGN KEY([CustomerRoleID])
REFERENCES [dbo].[Nop_CustomerRole] ([CustomerRoleID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_CustomerRole_Discount_Mapping_Nop_Discount'
           AND parent_obj = Object_id('Nop_CustomerRole_Discount_Mapping')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_CustomerRole_Discount_Mapping
DROP CONSTRAINT FK_Nop_CustomerRole_Discount_Mapping_Nop_Discount
GO
ALTER TABLE [dbo].[Nop_CustomerRole_Discount_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Nop_CustomerRole_Discount_Mapping_Nop_Discount] FOREIGN KEY([DiscountID])
REFERENCES [dbo].[Nop_Discount] ([DiscountID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CountryDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CountryDelete]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CountryInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CountryInsert]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CountryLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CountryLoadAll]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CountryLoadAllForBilling]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CountryLoadAllForBilling]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CountryLoadAllForRegistration]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CountryLoadAllForRegistration]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CountryLoadAllForShipping]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CountryLoadAllForShipping]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CountryLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CountryLoadByPrimaryKey]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CountryLoadByThreeLetterISOCode]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CountryLoadByThreeLetterISOCode]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CountryLoadByTwoLetterISOCode]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CountryLoadByTwoLetterISOCode]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CountryUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CountryUpdate]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_StateProvinceDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_StateProvinceDelete]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_StateProvinceInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_StateProvinceInsert]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_StateProvinceLoadAllByCountryID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_StateProvinceLoadAllByCountryID]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_StateProvinceLoadByAbbreviation]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_StateProvinceLoadByAbbreviation]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_StateProvinceLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_StateProvinceLoadByPrimaryKey]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_StateProvinceUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_StateProvinceUpdate]
GO

IF EXISTS(SELECT 1 from dbo.sysobjects 
			WHERE NAME='DF_Nop_Affiliate_Active' 
			and parent_obj=object_id('[dbo].[Nop_Affiliate]'))
	ALTER TABLE [dbo].[Nop_Affiliate] DROP CONSTRAINT [DF_Nop_Affiliate_Active]

	exec ('ALTER TABLE [dbo].[Nop_Affiliate] ALTER COLUMN [Active] bit NOT NULL')
GO


IF EXISTS(SELECT 1 from dbo.sysobjects 
			WHERE NAME='DF_Nop_Country_Published' 
			and parent_obj=object_id('[dbo].[Nop_Country]'))
	ALTER TABLE [dbo].[Nop_Country] DROP CONSTRAINT [DF_Nop_Country_Published]

	exec ('ALTER TABLE [dbo].[Nop_Country] ALTER COLUMN [Published] bit NOT NULL')
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicDelete]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicInsert]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLoadAll]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLoadByPrimaryKey]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLocalizedDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLocalizedDelete]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLocalizedInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLocalizedInsert]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLocalizedLoadAllByName]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLocalizedLoadAllByName]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLocalizedLoadByNameAndLanguageID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLocalizedLoadByNameAndLanguageID]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLocalizedLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLocalizedLoadByPrimaryKey]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLocalizedLoadByTopicIDAndLanguageID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLocalizedLoadByTopicIDAndLanguageID]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicLocalizedUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicLocalizedUpdate]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_TopicUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_TopicUpdate]
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_QueuedEmailDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_QueuedEmailDelete]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_QueuedEmailInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_QueuedEmailInsert]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_QueuedEmailLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_QueuedEmailLoadAll]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_QueuedEmailLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_QueuedEmailLoadByPrimaryKey]
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_QueuedEmailUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_QueuedEmailUpdate]
GO