--replace ntext and image database types with nvarchar(MAX) and varbinary(MAX)
ALTER TABLE [dbo].[Nop_Setting] ALTER COLUMN [Description] [nvarchar](MAX) NOT NULL
GO

ALTER TABLE [dbo].[Nop_Product] ALTER COLUMN [ShortDescription] [nvarchar](MAX) NOT NULL
GO

ALTER TABLE [dbo].[Nop_Product] ALTER COLUMN [FullDescription] [nvarchar](MAX) NOT NULL
GO

ALTER TABLE [dbo].[Nop_Product] ALTER COLUMN [AdminComment] [nvarchar](MAX) NOT NULL
GO

ALTER TABLE [dbo].[Nop_Category] ALTER COLUMN [Description] [nvarchar](MAX) NOT NULL
GO

ALTER TABLE [dbo].[Nop_Manufacturer] ALTER COLUMN [Description] [nvarchar](MAX) NOT NULL
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CategoryInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CategoryInsert]
GO
CREATE PROCEDURE [dbo].[Nop_CategoryInsert]
(
	@CategoryID int = NULL output,
	@Name nvarchar(400),
	@Description nvarchar(MAX),
	@TemplateID int,
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100),
	@ParentCategoryID int,	
	@PictureID int,
	@PageSize int,
	@PriceRanges nvarchar(400),
	@Published bit,
	@Deleted bit,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Category]
	(
		[Name],
		[Description],
		TemplateID,
		MetaKeywords,
		MetaDescription,
		MetaTitle,
		SEName,
		ParentCategoryID,
		PictureID,
		PageSize,
		PriceRanges,
		Published,
		Deleted,
		DisplayOrder,
		CreatedOn,
		UpdatedOn	
	)
	VALUES
	(
		@Name,
		@Description,
		@TemplateID,
		@MetaKeywords,
		@MetaDescription,
		@MetaTitle,
		@SEName,
		@ParentCategoryID,
		@PictureID,
		@PageSize,
		@PriceRanges,
		@Published,
		@Deleted,
		@DisplayOrder,
		@CreatedOn,
		@UpdatedOn
	)

	set @CategoryID=SCOPE_IDENTITY()
END
GO




IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CategoryUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CategoryUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_CategoryUpdate]
(
	@CategoryID int,
	@Name nvarchar(400),
	@Description nvarchar(MAX),
	@TemplateID int,
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100),
	@ParentCategoryID int,
	@PictureID int,
	@PageSize int,
	@PriceRanges nvarchar(400),
	@Published bit,
	@Deleted bit,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Category]
	SET
		[Name]=@Name,
		[Description]=@Description,
		TemplateID=@TemplateID,
		MetaKeywords=@MetaKeywords,
		MetaDescription=@MetaDescription,
		MetaTitle=@MetaTitle,
		SEName=@SEName,
		ParentCategoryID=@ParentCategoryID,
		PictureID=@PictureID,
		PageSize=@PageSize,
		PriceRanges=@PriceRanges,
		Published=@Published,
		Deleted=@Deleted,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn

	WHERE
		CategoryID = @CategoryID
END
GO




IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ManufacturerInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ManufacturerInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ManufacturerInsert]
(
	@ManufacturerID int = NULL output,
	@Name nvarchar(400),
	@Description nvarchar(MAX),
	@TemplateID int,
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100),
	@PictureID int,
	@PageSize int,
	@PriceRanges nvarchar(400),
	@Published bit,
	@Deleted bit,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Manufacturer]
	(
		[Name],
		[Description],
		TemplateID,
		MetaKeywords,
		MetaDescription,
		MetaTitle,
		SEName,
		PictureID,
		PageSize,
		PriceRanges,
		Published,
		Deleted,
		DisplayOrder,
		CreatedOn,
		UpdatedOn	
	)
	VALUES
	(
		@Name,
		@Description,
		@TemplateID,
		@MetaKeywords,
		@MetaDescription,
		@MetaTitle,
		@SEName,
		@PictureID,
		@PageSize,
		@PriceRanges,
		@Published,
		@Deleted,
		@DisplayOrder,
		@CreatedOn,
		@UpdatedOn
	)

	set @ManufacturerID=SCOPE_IDENTITY()
END
GO





IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ManufacturerUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ManufacturerUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ManufacturerUpdate]
(
	@ManufacturerID int,
	@Name nvarchar(400),
	@Description nvarchar(MAX),
	@TemplateID int,
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100),
	@PictureID int,
	@PageSize int,
	@PriceRanges nvarchar(400),
	@Published bit,
	@Deleted bit,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Manufacturer]
	SET
		[Name]=@Name,
		[Description]=@Description,
		TemplateID=@TemplateID,
		MetaKeywords=@MetaKeywords,
		MetaDescription=@MetaDescription,
		MetaTitle=@MetaTitle,
		SEName=@SEName,
		PictureID=@PictureID,
		PageSize=@PageSize,
		PriceRanges=@PriceRanges,
		Published=@Published,
		Deleted=@Deleted,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn

	WHERE
		ManufacturerID = @ManufacturerID
END
GO





IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ProductInsert]
(
	@ProductID int = NULL output,
	@Name nvarchar(400),
	@ShortDescription nvarchar(MAX),
	@FullDescription nvarchar(MAX),
	@AdminComment nvarchar(MAX),
	@ProductTypeID int,
	@TemplateID int,
	@ShowOnHomePage bit,
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100),
	@AllowCustomerReviews bit,
	@AllowCustomerRatings bit,
	@RatingSum int,
	@TotalRatingVotes int,
	@Published bit,
	@Deleted bit,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Product]
	(
		[Name],
		ShortDescription,
		FullDescription,
		AdminComment,
		ProductTypeID,
		TemplateID,
		ShowOnHomePage,
		MetaKeywords,
		MetaDescription,
		MetaTitle,
		SEName,
		AllowCustomerReviews,
		AllowCustomerRatings,
		RatingSum,
		TotalRatingVotes,
		Published,
		Deleted,
		CreatedOn,
		UpdatedOn
	)
	VALUES
	(
		@Name,
		@ShortDescription,
		@FullDescription,
		@AdminComment,
		@ProductTypeID,
		@TemplateID,
		@ShowOnHomePage,
		@MetaKeywords,
		@MetaDescription,
		@MetaTitle,
		@SEName,
		@AllowCustomerReviews,
		@AllowCustomerRatings,
		@RatingSum,
		@TotalRatingVotes,
		@Published,
		@Deleted,
		@CreatedOn,
		@UpdatedOn
	)

	set @ProductID=SCOPE_IDENTITY()
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ProductUpdate]
(
	@ProductID int,
	@Name nvarchar(400),
	@ShortDescription nvarchar(MAX),
	@FullDescription nvarchar(MAX),
	@AdminComment nvarchar(MAX),
	@ProductTypeID int,
	@TemplateID int,
	@ShowOnHomePage bit,
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100),
	@AllowCustomerReviews bit,
	@AllowCustomerRatings bit,
	@RatingSum int,
	@TotalRatingVotes int,
	@Published bit,
	@Deleted bit,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Product]
	SET
		[Name]=@Name,
		ShortDescription=@ShortDescription,
		FullDescription=@FullDescription,
		AdminComment=@AdminComment,
		ProductTypeID=@ProductTypeID,
		TemplateID=@TemplateID,
		ShowOnHomePage=@ShowOnHomePage,
		MetaKeywords=@MetaKeywords,
		MetaDescription=@MetaDescription,
		MetaTitle=@MetaTitle,
		SEName=@SEName,
		AllowCustomerReviews=@AllowCustomerReviews,
		AllowCustomerRatings=@AllowCustomerRatings,
		RatingSum=@RatingSum,
		TotalRatingVotes=@TotalRatingVotes,
		Published=@Published,
		Deleted=@Deleted,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		[ProductID] = @ProductID
END
GO





IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SettingInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SettingInsert]
GO
CREATE PROCEDURE [dbo].[Nop_SettingInsert]
(
	@SettingId int = NULL output,
	@Name nvarchar(200),
	@Value nvarchar(2000),	
	@Description nvarchar(MAX)
)
AS
BEGIN
	INSERT
	INTO [Nop_Setting]
	(
			[Name],
			[Value],	
			[Description]
	)
	VALUES
	(
			@Name,
			@Value,	
			@Description
	)

	set @SettingId=SCOPE_IDENTITY()
END
GO






IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SettingUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SettingUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_SettingUpdate]
(
	@SettingID int,
	@Name nvarchar(200),
	@Value nvarchar(2000),	
	@Description nvarchar(MAX)
)
AS
BEGIN
	UPDATE [Nop_Setting]
	SET
			[Name]=@Name,
			[Value]=@Value,	
			[Description]=@Description
	WHERE
		[SettingID] = @SettingID
END
GO



ALTER TABLE [dbo].[Nop_Download] ALTER COLUMN [DownloadBinary] [varbinary](MAX) NULL
GO

ALTER TABLE [dbo].[Nop_Picture] ALTER COLUMN [PictureBinary] [varbinary](MAX) NOT NULL
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_DownloadInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_DownloadInsert]
GO
CREATE PROCEDURE [dbo].[Nop_DownloadInsert]
(
	@DownloadID int = NULL output,
	@UseDownloadURL bit,
	@DownloadURL nvarchar(400),
	@DownloadBinary varbinary(MAX),
	@ContentType nvarchar(20),
	@Filename nvarchar(100),
	@Extension nvarchar(20),
	@IsNew	bit
)
AS
BEGIN
	INSERT
	INTO [Nop_Download]
	(
		[UseDownloadURL],
		[DownloadURL],
		[DownloadBinary],
		[Filename],
		[ContentType],
		[Extension],
		[IsNew]
	)
	VALUES
	(
		@UseDownloadURL,
		@DownloadURL,
		@DownloadBinary,
		@Filename,
		@ContentType,
		@Extension,
		@IsNew
	)

	set @DownloadID=SCOPE_IDENTITY()
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_DownloadUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_DownloadUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_DownloadUpdate]
(
	@DownloadID int,
	@UseDownloadURL bit,
	@DownloadURL nvarchar(400),
	@DownloadBinary varbinary(MAX),
	@ContentType nvarchar(20),
	@Filename nvarchar(100),
	@Extension nvarchar(20),
	@IsNew	bit
)
AS
BEGIN

	UPDATE [Nop_Download]
	SET		
		[UseDownloadURL]=@UseDownloadURL,
		[DownloadURL]=@DownloadURL,
		[DownloadBinary]=@DownloadBinary,
		[ContentType] = @ContentType,
		[Filename] = @Filename,
		[Extension] = @Extension,
		[IsNew] = @IsNew
	WHERE
		DownloadID = @DownloadID

END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_PictureInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_PictureInsert]
GO
CREATE PROCEDURE [dbo].[Nop_PictureInsert]
(
	@PictureID int = NULL output,
	@PictureBinary varbinary(MAX),	
	@Extension nvarchar(20),
	@IsNew	bit
)
AS
BEGIN
	INSERT
	INTO [Nop_Picture]
	(
		PictureBinary,
		Extension,
		IsNew
	)
	VALUES
	(
		@PictureBinary,
		@Extension,
		@IsNew
	)

	set @PictureID=SCOPE_IDENTITY()
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_PictureUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_PictureUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_PictureUpdate]
(
	@PictureID int,
	@PictureBinary varbinary(MAX),
	@Extension nvarchar(20),
	@IsNew	bit
)
AS
BEGIN

	UPDATE [Nop_Picture]
	SET
		PictureBinary=@PictureBinary,
		Extension=@Extension,
		IsNew=@IsNew
	WHERE
		PictureID = @PictureID

END
GO



--category localization
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[NOP_getnotnullnotempty]') AND xtype in (N'FN', N'IF', N'TF'))
DROP FUNCTION [dbo].[NOP_getnotnullnotempty]
GO
CREATE FUNCTION dbo.NOP_getnotnullnotempty
(
    @p1 nvarchar(max) = null, 
    @p2 nvarchar(max) = null
)
RETURNS nvarchar(max)
AS
BEGIN
    IF @p1 IS NULL
        return @p2
    IF @p1 =''
        return @p2

    return @p1
END
GO



if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_CategoryLocalized]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Nop_CategoryLocalized](
	[CategoryLocalizedID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NOT NULL,
	[LanguageID] [int] NOT NULL,
	[Name] [nvarchar](400) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[MetaKeywords] [nvarchar](400) NOT NULL,
	[MetaDescription] [nvarchar](4000) NOT NULL,
	[MetaTitle] [nvarchar](400) NOT NULL,
	[SEName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Nop_CategoryLocalized] PRIMARY KEY CLUSTERED 
(
	[CategoryLocalizedID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [IX_Nop_CategoryLocalized_Unique1] UNIQUE NONCLUSTERED 
(
	[CategoryID] ASC,
	[LanguageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO



IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_CategoryLocalized_Nop_Category'
           AND parent_obj = Object_id('Nop_CategoryLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_CategoryLocalized
DROP CONSTRAINT FK_Nop_CategoryLocalized_Nop_Category
GO
ALTER TABLE [dbo].[Nop_CategoryLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_CategoryLocalized_Nop_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Nop_Category] ([CategoryID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_CategoryLocalized_Nop_Language'
           AND parent_obj = Object_id('Nop_CategoryLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_CategoryLocalized
DROP CONSTRAINT FK_Nop_CategoryLocalized_Nop_Language
GO
ALTER TABLE [dbo].[Nop_CategoryLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_CategoryLocalized_Nop_Language] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Nop_Language] ([LanguageID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CategoryLocalizedCleanUp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CategoryLocalizedCleanUp]
GO
CREATE PROCEDURE [dbo].[Nop_CategoryLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_CategoryLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '') AND		
		([Description] IS NULL OR [Description] = '') AND
		(MetaKeywords IS NULL or MetaKeywords = '') AND
		(MetaDescription IS NULL or MetaDescription = '') AND
		(MetaTitle IS NULL or MetaTitle = '') AND
		(SEName IS NULL or SEName = '') 
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CategoryLocalizedInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CategoryLocalizedInsert]
GO
CREATE PROCEDURE [dbo].[Nop_CategoryLocalizedInsert]
(
	@CategoryLocalizedID int = NULL output,
	@CategoryID int,
	@LanguageID int,
	@Name nvarchar(400),
	@Description nvarchar(max),
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100)
)
AS
BEGIN
	INSERT
	INTO [Nop_CategoryLocalized]
	(
		CategoryID,
		LanguageID,
		[Name],
		[Description],		
		MetaKeywords,
		MetaDescription,
		MetaTitle,
		SEName
	)
	VALUES
	(
		@CategoryID,
		@LanguageID,
		@Name,
		@Description,
		@MetaKeywords,
		@MetaDescription,
		@MetaTitle,
		@SEName
	)

	set @CategoryLocalizedID=@@identity

	EXEC Nop_CategoryLocalizedCleanUp
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CategoryLocalizedLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CategoryLocalizedLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_CategoryLocalizedLoadByPrimaryKey]
	@CategoryLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_CategoryLocalized]
	WHERE CategoryLocalizedID = @CategoryLocalizedID
	ORDER BY CategoryLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CategoryLocalizedLoadByCategoryIDAndLanguageID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CategoryLocalizedLoadByCategoryIDAndLanguageID]
GO
CREATE PROCEDURE [dbo].[Nop_CategoryLocalizedLoadByCategoryIDAndLanguageID]
	@CategoryID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_CategoryLocalized]
	WHERE CategoryID = @CategoryID AND LanguageID=@LanguageID
	ORDER BY CategoryLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CategoryLocalizedUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CategoryLocalizedUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_CategoryLocalizedUpdate]
(
	@CategoryLocalizedID int,
	@CategoryID int,
	@LanguageID int,
	@Name nvarchar(400),
	@Description nvarchar(max),
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100)
)
AS
BEGIN
	
	UPDATE [Nop_CategoryLocalized]
	SET
		[CategoryID]=@CategoryID,
		[LanguageID]=@LanguageID,
		[Name]=@Name,
		[Description]=@Description,		
		MetaKeywords=@MetaKeywords,
		MetaDescription=@MetaDescription,
		MetaTitle=@MetaTitle,
		SEName=@SEName		
	WHERE
		CategoryLocalizedID = @CategoryLocalizedID

	EXEC Nop_CategoryLocalizedCleanUp
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CategoryLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CategoryLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_CategoryLoadAll]
	@ShowHidden bit = 0,
	@ParentCategoryID int = 0,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		c.CategoryID, 
		dbo.NOP_getnotnullnotempty(cl.Name,c.Name) as [Name],
		dbo.NOP_getnotnullnotempty(cl.Description,c.Description) as [Description],
		c.TemplateID, 
		dbo.NOP_getnotnullnotempty(cl.MetaKeywords,c.MetaKeywords) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(cl.MetaDescription,c.MetaDescription) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(cl.MetaTitle,c.MetaTitle) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(cl.SEName,c.SEName) as [SEName],
		c.ParentCategoryID, 
		c.PictureID, 
		c.PageSize, 
		c.PriceRanges, 
		c.Published,
		c.Deleted, 
		c.DisplayOrder, 
		c.CreatedOn, 
		c.UpdatedOn
	FROM [Nop_Category] c
		LEFT OUTER JOIN [Nop_CategoryLocalized] cl 
		ON c.CategoryID = cl.CategoryID AND cl.LanguageID = @LanguageID	
	WHERE 
		(c.Published = 1 or @ShowHidden = 1) AND 
		c.Deleted=0 AND 
		c.ParentCategoryID=@ParentCategoryID
	order by c.DisplayOrder
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CategoryLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CategoryLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_CategoryLoadByPrimaryKey]
(
	@CategoryID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		c.CategoryID, 
		dbo.NOP_getnotnullnotempty(cl.Name,c.Name) as [Name],
		dbo.NOP_getnotnullnotempty(cl.Description,c.Description) as [Description],
		c.TemplateID, 
		dbo.NOP_getnotnullnotempty(cl.MetaKeywords,c.MetaKeywords) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(cl.MetaDescription,c.MetaDescription) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(cl.MetaTitle,c.MetaTitle) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(cl.SEName,c.SEName) as [SEName],
		c.ParentCategoryID, 
		c.PictureID, 
		c.PageSize, 
		c.PriceRanges, 
		c.Published,
		c.Deleted, 
		c.DisplayOrder,
		c.CreatedOn, 
		c.UpdatedOn
	FROM [Nop_Category] c
		LEFT OUTER JOIN [Nop_CategoryLocalized] cl 
		ON c.CategoryID = cl.CategoryID AND cl.LanguageID = @LanguageID	
	WHERE 
		(c.CategoryID = @CategoryID) 
END
GO


-- Add Flag column to Nop_Language
IF NOT EXISTS (SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Language]') and NAME='FlagImageFileName')
BEGIN
	ALTER TABLE [dbo].[Nop_Language] 
	ADD FlagImageFileName nvarchar(50) NOT NULL CONSTRAINT [DF_Nop_Language_FlagImageFileName] DEFAULT ((''))
END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Nop_LanguageInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_LanguageInsert]
GO
CREATE PROCEDURE [dbo].[Nop_LanguageInsert]
(
	@LanguageId int = NULL output,
	@Name nvarchar(100),
	@LanguageCulture nvarchar(20),
	@FlagImageFileName nvarchar(50),
	@Published bit,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_Language]
	(
		[Name],
		[LanguageCulture],
		[FlagImageFileName],
		[Published],
		[DisplayOrder]
	)
	VALUES
	(
		@Name,
		@LanguageCulture,
		@FlagImageFileName,
		@Published,
		@DisplayOrder
	)

	set @LanguageId=SCOPE_IDENTITY()
END
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Nop_LanguageUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_LanguageUpdate]
GO

CREATE PROCEDURE [dbo].[Nop_LanguageUpdate]
(
	@LanguageId int,
	@Name nvarchar(100),
	@LanguageCulture nvarchar(20),
	@FlagImageFileName nvarchar(50),
	@Published bit,
	@DisplayOrder int
)
AS
BEGIN
	UPDATE [Nop_Language]
	SET
		[Name] = @Name,
		[LanguageCulture] = @LanguageCulture,
		[FlagImageFileName] = @FlagImageFileName,
		[Published] = @Published,
		[DisplayOrder] = @DisplayOrder
	WHERE
		[LanguageId] = @LanguageId
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_NewsLetterSubscriptionLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_NewsLetterSubscriptionLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_NewsLetterSubscriptionLoadAll]
(
	@ShowHidden bit = 0
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		nls.* 
	FROM
		[Nop_NewsLetterSubscription] nls
	LEFT OUTER JOIN 
		Nop_Customer c 
	ON 
		nls.Email=c.Email
	WHERE
		(nls.Active = 1 OR @ShowHidden = 1) AND 
		(c.CustomerID IS NULL OR (c.Active = 1 AND c.Deleted = 0))
END
GO



--manufacturer localization

if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_ManufacturerLocalized]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Nop_ManufacturerLocalized](
	[ManufacturerLocalizedID] [int] IDENTITY(1,1) NOT NULL,
	[ManufacturerID] [int] NOT NULL,
	[LanguageID] [int] NOT NULL,
	[Name] [nvarchar](400) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[MetaKeywords] [nvarchar](400) NOT NULL,
	[MetaDescription] [nvarchar](4000) NOT NULL,
	[MetaTitle] [nvarchar](400) NOT NULL,
	[SEName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Nop_ManufacturerLocalized] PRIMARY KEY CLUSTERED 
(
	[ManufacturerLocalizedID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [IX_Nop_ManufacturerLocalized_Unique1] UNIQUE NONCLUSTERED 
(
	[ManufacturerID] ASC,
	[LanguageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO



IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_ManufacturerLocalized_Nop_Manufacturer'
           AND parent_obj = Object_id('Nop_ManufacturerLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_ManufacturerLocalized
DROP CONSTRAINT FK_Nop_ManufacturerLocalized_Nop_Manufacturer
GO
ALTER TABLE [dbo].[Nop_ManufacturerLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_ManufacturerLocalized_Nop_Manufacturer] FOREIGN KEY([ManufacturerID])
REFERENCES [dbo].[Nop_Manufacturer] ([ManufacturerID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_ManufacturerLocalized_Nop_Language'
           AND parent_obj = Object_id('Nop_ManufacturerLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_ManufacturerLocalized
DROP CONSTRAINT FK_Nop_ManufacturerLocalized_Nop_Language
GO
ALTER TABLE [dbo].[Nop_ManufacturerLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_ManufacturerLocalized_Nop_Language] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Nop_Language] ([LanguageID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ManufacturerLocalizedCleanUp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ManufacturerLocalizedCleanUp]
GO
CREATE PROCEDURE [dbo].[Nop_ManufacturerLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_ManufacturerLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '') AND		
		([Description] IS NULL OR [Description] = '') AND
		(MetaKeywords IS NULL or MetaKeywords = '') AND
		(MetaDescription IS NULL or MetaDescription = '') AND
		(MetaTitle IS NULL or MetaTitle = '') AND
		(SEName IS NULL or SEName = '') 
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ManufacturerLocalizedInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ManufacturerLocalizedInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ManufacturerLocalizedInsert]
(
	@ManufacturerLocalizedID int = NULL output,
	@ManufacturerID int,
	@LanguageID int,
	@Name nvarchar(400),
	@Description nvarchar(max),
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100)
)
AS
BEGIN
	INSERT
	INTO [Nop_ManufacturerLocalized]
	(
		ManufacturerID,
		LanguageID,
		[Name],
		[Description],		
		MetaKeywords,
		MetaDescription,
		MetaTitle,
		SEName
	)
	VALUES
	(
		@ManufacturerID,
		@LanguageID,
		@Name,
		@Description,
		@MetaKeywords,
		@MetaDescription,
		@MetaTitle,
		@SEName
	)

	set @ManufacturerLocalizedID=@@identity

	EXEC Nop_ManufacturerLocalizedCleanUp
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ManufacturerLocalizedLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ManufacturerLocalizedLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_ManufacturerLocalizedLoadByPrimaryKey]
	@ManufacturerLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ManufacturerLocalized]
	WHERE ManufacturerLocalizedID = @ManufacturerLocalizedID
	ORDER BY ManufacturerLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ManufacturerLocalizedLoadByManufacturerIDAndLanguageID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ManufacturerLocalizedLoadByManufacturerIDAndLanguageID]
GO
CREATE PROCEDURE [dbo].[Nop_ManufacturerLocalizedLoadByManufacturerIDAndLanguageID]
	@ManufacturerID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ManufacturerLocalized]
	WHERE ManufacturerID = @ManufacturerID AND LanguageID=@LanguageID
	ORDER BY ManufacturerLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ManufacturerLocalizedUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ManufacturerLocalizedUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ManufacturerLocalizedUpdate]
(
	@ManufacturerLocalizedID int,
	@ManufacturerID int,
	@LanguageID int,
	@Name nvarchar(400),
	@Description nvarchar(max),
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100)
)
AS
BEGIN
	
	UPDATE [Nop_ManufacturerLocalized]
	SET
		[ManufacturerID]=@ManufacturerID,
		[LanguageID]=@LanguageID,
		[Name]=@Name,
		[Description]=@Description,		
		MetaKeywords=@MetaKeywords,
		MetaDescription=@MetaDescription,
		MetaTitle=@MetaTitle,
		SEName=@SEName		
	WHERE
		ManufacturerLocalizedID = @ManufacturerLocalizedID

	EXEC Nop_ManufacturerLocalizedCleanUp
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ManufacturerLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ManufacturerLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_ManufacturerLoadAll]
	@ShowHidden bit = 0,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		m.ManufacturerID, 
		dbo.NOP_getnotnullnotempty(ml.Name,m.Name) as [Name],
		dbo.NOP_getnotnullnotempty(ml.Description,m.Description) as [Description],
		m.TemplateID, 
		dbo.NOP_getnotnullnotempty(ml.MetaKeywords,m.MetaKeywords) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(ml.MetaDescription,m.MetaDescription) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(ml.MetaTitle,m.MetaTitle) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(ml.SEName,m.SEName) as [SEName],
		m.PictureID, 
		m.PageSize, 
		m.PriceRanges, 
		m.Published,
		m.Deleted, 
		m.DisplayOrder, 
		m.CreatedOn, 
		m.UpdatedOn
	FROM [Nop_Manufacturer] m
		LEFT OUTER JOIN [Nop_ManufacturerLocalized] ml 
		ON m.ManufacturerID = ml.ManufacturerID AND ml.LanguageID = @LanguageID	
	WHERE 
		(m.Published = 1 or @ShowHidden = 1) AND 
		m.Deleted=0
	order by m.DisplayOrder
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ManufacturerLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ManufacturerLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_ManufacturerLoadByPrimaryKey]
(
	@ManufacturerID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		m.ManufacturerID, 
		dbo.NOP_getnotnullnotempty(ml.Name,m.Name) as [Name],
		dbo.NOP_getnotnullnotempty(ml.Description,m.Description) as [Description],
		m.TemplateID, 
		dbo.NOP_getnotnullnotempty(ml.MetaKeywords,m.MetaKeywords) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(ml.MetaDescription,m.MetaDescription) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(ml.MetaTitle,m.MetaTitle) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(ml.SEName,m.SEName) as [SEName],
		m.PictureID, 
		m.PageSize, 
		m.PriceRanges, 
		m.Published,
		m.Deleted, 
		m.DisplayOrder, 
		m.CreatedOn, 
		m.UpdatedOn
	FROM [Nop_Manufacturer] m
		LEFT OUTER JOIN [Nop_ManufacturerLocalized] ml 
		ON m.ManufacturerID = ml.ManufacturerID AND ml.LanguageID = @LanguageID	
	WHERE 
		(m.ManufacturerID = @ManufacturerID) 
END
GO

IF EXISTS (
		SELECT 1
		FROM [dbo].[Nop_CustomerAction]
		WHERE [SystemKeyword] = N'ManageCoutriesStates')
BEGIN
	UPDATE 
		[dbo].[Nop_CustomerAction] 
	SET 
		[Name] = N'Manage Countries / States',
		[SystemKeyword] = N'ManageCountriesStates'
	WHERE 
		[SystemKeyword] = N'ManageCoutriesStates'
END
GO



--product/product variant localization

if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_ProductLocalized]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Nop_ProductLocalized](
	[ProductLocalizedID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[LanguageID] [int] NOT NULL,
	[Name] [nvarchar](400) NOT NULL,
	[ShortDescription] [nvarchar](max) NOT NULL,
	[FullDescription] [nvarchar](max) NOT NULL,
	[MetaKeywords] [nvarchar](400) NOT NULL,
	[MetaDescription] [nvarchar](4000) NOT NULL,
	[MetaTitle] [nvarchar](400) NOT NULL,
	[SEName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Nop_ProductLocalized] PRIMARY KEY CLUSTERED 
(
	[ProductLocalizedID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [IX_Nop_ProductLocalized_Unique1] UNIQUE NONCLUSTERED 
(
	[ProductID] ASC,
	[LanguageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO



IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_ProductLocalized_Nop_Product'
           AND parent_obj = Object_id('Nop_ProductLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_ProductLocalized
DROP CONSTRAINT FK_Nop_ProductLocalized_Nop_Product
GO
ALTER TABLE [dbo].[Nop_ProductLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_ProductLocalized_Nop_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Nop_Product] ([ProductID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_ProductLocalized_Nop_Language'
           AND parent_obj = Object_id('Nop_ProductLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_ProductLocalized
DROP CONSTRAINT FK_Nop_ProductLocalized_Nop_Language
GO
ALTER TABLE [dbo].[Nop_ProductLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_ProductLocalized_Nop_Language] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Nop_Language] ([LanguageID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductLocalizedCleanUp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductLocalizedCleanUp]
GO
CREATE PROCEDURE [dbo].[Nop_ProductLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_ProductLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '') AND		
		([ShortDescription] IS NULL OR [ShortDescription] = '') AND
		([FullDescription] IS NULL OR [FullDescription] = '') AND
		(MetaKeywords IS NULL or MetaKeywords = '') AND
		(MetaDescription IS NULL or MetaDescription = '') AND
		(MetaTitle IS NULL or MetaTitle = '') AND
		(SEName IS NULL or SEName = '') 
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductLocalizedInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductLocalizedInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ProductLocalizedInsert]
(
	@ProductLocalizedID int = NULL output,
	@ProductID int,
	@LanguageID int,
	@Name nvarchar(400),
	@ShortDescription nvarchar(max),
	@FullDescription nvarchar(max),
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100)
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductLocalized]
	(
		ProductID,
		LanguageID,
		[Name],
		[ShortDescription],
		[FullDescription],	
		MetaKeywords,
		MetaDescription,
		MetaTitle,
		SEName
	)
	VALUES
	(
		@ProductID,
		@LanguageID,
		@Name,
		@ShortDescription,
		@FullDescription,
		@MetaKeywords,
		@MetaDescription,
		@MetaTitle,
		@SEName
	)

	set @ProductLocalizedID=@@identity

	EXEC Nop_ProductLocalizedCleanUp
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductLocalizedLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductLocalizedLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_ProductLocalizedLoadByPrimaryKey]
	@ProductLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ProductLocalized]
	WHERE ProductLocalizedID = @ProductLocalizedID
	ORDER BY ProductLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductLocalizedLoadByProductIDAndLanguageID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductLocalizedLoadByProductIDAndLanguageID]
GO
CREATE PROCEDURE [dbo].[Nop_ProductLocalizedLoadByProductIDAndLanguageID]
	@ProductID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ProductLocalized]
	WHERE ProductID = @ProductID AND LanguageID=@LanguageID
	ORDER BY ProductLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductLocalizedUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductLocalizedUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ProductLocalizedUpdate]
(
	@ProductLocalizedID int,
	@ProductID int,
	@LanguageID int,
	@Name nvarchar(400),
	@ShortDescription nvarchar(max),
	@FullDescription nvarchar(max),
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100)
)
AS
BEGIN
	
	UPDATE [Nop_ProductLocalized]
	SET
		[ProductID]=@ProductID,
		[LanguageID]=@LanguageID,
		[Name]=@Name,
		[ShortDescription]=@ShortDescription,
		[FullDescription]=@FullDescription,
		MetaKeywords=@MetaKeywords,
		MetaDescription=@MetaDescription,
		MetaTitle=@MetaTitle,
		SEName=@SEName		
	WHERE
		ProductLocalizedID = @ProductLocalizedID

	EXEC Nop_ProductLocalizedCleanUp
END
GO


if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_ProductVariantLocalized]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Nop_ProductVariantLocalized](
	[ProductVariantLocalizedID] [int] IDENTITY(1,1) NOT NULL,
	[ProductVariantID] [int] NOT NULL,
	[LanguageID] [int] NOT NULL,
	[Name] [nvarchar](400) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Nop_ProductVariantLocalized] PRIMARY KEY CLUSTERED 
(
	[ProductVariantLocalizedID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [IX_Nop_ProductVariantLocalized_Unique1] UNIQUE NONCLUSTERED 
(
	[ProductVariantID] ASC,
	[LanguageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO



IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_ProductVariantLocalized_Nop_ProductVariant'
           AND parent_obj = Object_id('Nop_ProductVariantLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_ProductVariantLocalized
DROP CONSTRAINT FK_Nop_ProductVariantLocalized_Nop_ProductVariant
GO
ALTER TABLE [dbo].[Nop_ProductVariantLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_ProductVariantLocalized_Nop_ProductVariant] FOREIGN KEY([ProductVariantID])
REFERENCES [dbo].[Nop_ProductVariant] ([ProductVariantID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_ProductVariantLocalized_Nop_Language'
           AND parent_obj = Object_id('Nop_ProductVariantLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_ProductVariantLocalized
DROP CONSTRAINT FK_Nop_ProductVariantLocalized_Nop_Language
GO
ALTER TABLE [dbo].[Nop_ProductVariantLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_ProductVariantLocalized_Nop_Language] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Nop_Language] ([LanguageID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantLocalizedCleanUp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantLocalizedCleanUp]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_ProductVariantLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '') AND		
		([Description] IS NULL OR [Description] = '')
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantLocalizedInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantLocalizedInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantLocalizedInsert]
(
	@ProductVariantLocalizedID int = NULL output,
	@ProductVariantID int,
	@LanguageID int,
	@Name nvarchar(400),
	@Description nvarchar(max)
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductVariantLocalized]
	(
		ProductVariantID,
		LanguageID,
		[Name],
		[Description]
	)
	VALUES
	(
		@ProductVariantID,
		@LanguageID,
		@Name,
		@Description
	)

	set @ProductVariantLocalizedID=@@identity

	EXEC Nop_ProductVariantLocalizedCleanUp
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantLocalizedLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantLocalizedLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantLocalizedLoadByPrimaryKey]
	@ProductVariantLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ProductVariantLocalized]
	WHERE ProductVariantLocalizedID = @ProductVariantLocalizedID
	ORDER BY ProductVariantLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantLocalizedLoadByProductVariantIDAndLanguageID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantLocalizedLoadByProductVariantIDAndLanguageID]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantLocalizedLoadByProductVariantIDAndLanguageID]
	@ProductVariantID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ProductVariantLocalized]
	WHERE ProductVariantID = @ProductVariantID AND LanguageID=@LanguageID
	ORDER BY ProductVariantLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantLocalizedUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantLocalizedUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantLocalizedUpdate]
(
	@ProductVariantLocalizedID int,
	@ProductVariantID int,
	@LanguageID int,
	@Name nvarchar(400),
	@Description nvarchar(max)
)
AS
BEGIN
	
	UPDATE [Nop_ProductVariantLocalized]
	SET
		[ProductVariantID]=@ProductVariantID,
		[LanguageID]=@LanguageID,
		[Name]=@Name,
		[Description]=@Description	
	WHERE
		ProductVariantLocalizedID = @ProductVariantLocalizedID

	EXEC Nop_ProductVariantLocalizedCleanUp
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_ProductLoadAll]
	@ShowHidden bit = 0,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		p.[ProductId],
		dbo.NOP_getnotnullnotempty(pl.[Name],p.[Name]) as [Name],
		dbo.NOP_getnotnullnotempty(pl.[ShortDescription],p.[ShortDescription]) as [ShortDescription],
		dbo.NOP_getnotnullnotempty(pl.[FullDescription],p.[FullDescription]) as [FullDescription],
		p.[AdminComment], 
		p.[ProductTypeID], 
		p.[TemplateID], 
		p.[ShowOnHomePage], 
		dbo.NOP_getnotnullnotempty(pl.[MetaKeywords],p.[MetaKeywords]) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(pl.[MetaDescription],p.[MetaDescription]) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(pl.[MetaTitle],p.[MetaTitle]) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(pl.[SEName],p.[SEName]) as [SEName],
		p.[AllowCustomerReviews], 
		p.[AllowCustomerRatings], 
		p.[RatingSum], 
		p.[TotalRatingVotes], 
		p.[Published], 
		p.[Deleted], 
		p.[CreatedOn], 
		p.[UpdatedOn]
	FROM [Nop_Product] p
	LEFT OUTER JOIN Nop_ProductLocalized pl
			ON p.ProductId = pl.ProductId AND pl.LanguageId = @LanguageID
	WHERE (Published = 1 or @ShowHidden = 1) and Deleted=0
	ORDER BY p.[Name]
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductLoadAllPaged]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductLoadAllPaged]
GO
CREATE PROCEDURE [dbo].[Nop_ProductLoadAllPaged]
(
	@CategoryID			int = 0,
	@ManufacturerID		int = 0,
	@FeaturedProducts	bit = null,	--0 featured only , 1 not featured only, null - load all products
	@PriceMin			money = null,
	@PriceMax			money = null,
	@Keywords			nvarchar(MAX),	
	@SearchDescriptions bit = 0,
	@ShowHidden			bit = 0,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@FilteredSpecs		nvarchar(300) = null,	--filter by attributes (comma-separated list). e.g. 14,15,16
	@LanguageID			int = 0,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	--init
	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = '%' + rtrim(ltrim(@Keywords)) + '%'

	SET @PriceMin = isnull(@PriceMin, 0)
	SET @PriceMax = isnull(@PriceMax, 2147483644)

	--display order
	CREATE TABLE #DisplayOrder
	(
		ProductID int not null PRIMARY KEY,
		DisplayOrder int not null
	)	

	IF @CategoryID IS NOT NULL AND @CategoryID > 0
		BEGIN
			INSERT #DisplayOrder 
			SELECT pcm.ProductID, pcm.DisplayOrder 
			FROM [Nop_Product_Category_Mapping] pcm WHERE pcm.CategoryID = @CategoryID
		END
    ELSE IF @ManufacturerID IS NOT NULL AND @ManufacturerID > 0
		BEGIN
			INSERT #DisplayOrder 
			SELECT pmm.ProductID, pmm.Displayorder 
			FROM [Nop_Product_Manufacturer_Mapping] pmm WHERE pmm.ManufacturerID = @ManufacturerID
		END
	ELSE
		BEGIN
			INSERT #DisplayOrder 
			SELECT p.ProductID, 1 
			FROM [Nop_Product] p
			ORDER BY p.[Name]
		END
	
	--filter by attributes
	SET @FilteredSpecs = isnull(@FilteredSpecs, '')
	CREATE TABLE #FilteredSpecs
	(
		SpecificationAttributeOptionID int not null
	)
	INSERT INTO #FilteredSpecs (SpecificationAttributeOptionID)
	SELECT CAST(data as int) FROM dbo.[NOP_splitstring_to_table](@FilteredSpecs, ',');
	
	DECLARE @SpecAttributesCount int	
	SELECT @SpecAttributesCount = COUNT(1) FROM #FilteredSpecs

	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		ProductID int NOT NULL,
		DisplayOrder int NOT NULL,
	)
	INSERT INTO #PageIndex (ProductID, DisplayOrder)
	SELECT DISTINCT p.ProductID, do.DisplayOrder
	FROM Nop_Product p with (NOLOCK) 
	LEFT OUTER JOIN Nop_Product_Category_Mapping pcm with (NOLOCK) ON p.ProductID=pcm.ProductID
	LEFT OUTER JOIN Nop_Product_Manufacturer_Mapping pmm with (NOLOCK) ON p.ProductID=pmm.ProductID
	LEFT OUTER JOIN Nop_ProductVariant pv with (NOLOCK) ON p.ProductID = pv.ProductID
	LEFT OUTER JOIN Nop_ProductVariantLocalized pvl with (NOLOCK) ON pv.ProductVariantID = pvl.ProductVariantID AND pvl.LanguageID = @LanguageID
	LEFT OUTER JOIN Nop_ProductLocalized pl with (NOLOCK) ON p.ProductID = pl.ProductID AND pl.LanguageID = @LanguageID
	JOIN #DisplayOrder do on p.ProductID = do.ProductID
	WHERE 
		(
			(
				@ShowHidden = 1 OR p.Published = 1
			)
		AND 
			(
				@ShowHidden = 1 OR pv.Published = 1
			)
		AND 
			(
				p.Deleted=0
			)
		AND (
				@CategoryID IS NULL OR @CategoryID=0
				OR (pcm.CategoryID=@CategoryID AND (@FeaturedProducts IS NULL OR pcm.IsFeaturedProduct=@FeaturedProducts))
			)
		AND (
				@ManufacturerID IS NULL OR @ManufacturerID=0
				OR (pmm.ManufacturerID=@ManufacturerID AND (@FeaturedProducts IS NULL OR pmm.IsFeaturedProduct=@FeaturedProducts))
			)
		AND (
				pv.Price BETWEEN @PriceMin AND @PriceMax
			)
		AND	(
				-- search standard content
				patindex(@Keywords, isnull(p.name, '')) > 0
				or patindex(@Keywords, isnull(pv.name, '')) > 0
				or patindex(@Keywords, isnull(pv.sku , '')) > 0
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(p.ShortDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(p.FullDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pv.Description, '')) > 0)					
				-- search language content
				or patindex(@Keywords, isnull(pl.name, '')) > 0
				or patindex(@Keywords, isnull(pvl.name, '')) > 0
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pl.ShortDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pl.FullDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pvl.Description, '')) > 0)
			)
		AND
			(
				@ShowHidden = 1
				OR
				(getutcdate() between isnull(pv.AvailableStartDateTime, '1/1/1900') and isnull(pv.AvailableEndDateTime, '1/1/2999'))
			)
		AND
			(
				--filter by specs
				@SpecAttributesCount = 0
				OR
				(
					NOT EXISTS(
						SELECT 1 
						FROM #FilteredSpecs [fs]
						WHERE [fs].SpecificationAttributeOptionID NOT IN (
							SELECT psam.SpecificationAttributeOptionID
							FROM dbo.Nop_Product_SpecificationAttribute_Mapping psam
							WHERE psam.AllowFiltering = 1 AND psam.ProductID = p.ProductID
							)
						)
					
				)
			)
		)
	ORDER BY do.DisplayOrder

	--total records
	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	--return
	SELECT  
		p.[ProductId],
		dbo.NOP_getnotnullnotempty(pl.[Name],p.[Name]) as [Name],
		dbo.NOP_getnotnullnotempty(pl.[ShortDescription],p.[ShortDescription]) as [ShortDescription],
		dbo.NOP_getnotnullnotempty(pl.[FullDescription],p.[FullDescription]) as [FullDescription],
		p.[AdminComment], 
		p.[ProductTypeID], 
		p.[TemplateID], 
		p.[ShowOnHomePage], 
		dbo.NOP_getnotnullnotempty(pl.[MetaKeywords],p.[MetaKeywords]) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(pl.[MetaDescription],p.[MetaDescription]) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(pl.[MetaTitle],p.[MetaTitle]) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(pl.[SEName],p.[SEName]) as [SEName],
		p.[AllowCustomerReviews], 
		p.[AllowCustomerRatings], 
		p.[RatingSum], 
		p.[TotalRatingVotes], 
		p.[Published], 
		p.[Deleted], 
		p.[CreatedOn], 
		p.[UpdatedOn]
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Product p on p.ProductID = [pi].ProductID
		LEFT OUTER JOIN Nop_ProductLocalized pl on (pl.ProductID = p.ProductID AND pl.LanguageID = @LanguageID) 
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #DisplayOrder
	DROP TABLE #FilteredSpecs
	DROP TABLE #PageIndex
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_ProductLoadByPrimaryKey]
(
	@ProductID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		p.[ProductId],
		dbo.NOP_getnotnullnotempty(pl.[Name],p.[Name]) as [Name],
		dbo.NOP_getnotnullnotempty(pl.[ShortDescription],p.[ShortDescription]) as [ShortDescription],
		dbo.NOP_getnotnullnotempty(pl.[FullDescription],p.[FullDescription]) as [FullDescription],
		p.[AdminComment], 
		p.[ProductTypeID], 
		p.[TemplateID], 
		p.[ShowOnHomePage], 
		dbo.NOP_getnotnullnotempty(pl.[MetaKeywords],p.[MetaKeywords]) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(pl.[MetaDescription],p.[MetaDescription]) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(pl.[MetaTitle],p.[MetaTitle]) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(pl.[SEName],p.[SEName]) as [SEName],
		p.[AllowCustomerReviews], 
		p.[AllowCustomerRatings], 
		p.[RatingSum], 
		p.[TotalRatingVotes], 
		p.[Published], 
		p.[Deleted], 
		p.[CreatedOn], 
		p.[UpdatedOn]
	FROM [Nop_Product] p
	LEFT OUTER JOIN Nop_ProductLocalized pl ON p.ProductId = pl.ProductId AND pl.LanguageId = @LanguageID
	WHERE
		(p.ProductID = @ProductID)
END
GO





IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantLoadByPrimaryKey]
(
	@ProductVariantID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT	 
		pv.ProductVariantId, 
		pv.ProductID, 
		dbo.NOP_getnotnullnotempty(pvl.[Name],pv.[Name]) as [Name], 
		pv.SKU, 
		dbo.NOP_getnotnullnotempty(pvl.[Description],pv.[Description]) as [Description], 
		pv.AdminComment, 
		pv.ManufacturerPartNumber, 
		pv.IsGiftCard, 
		pv.IsDownload, 
		pv.DownloadID,                      
		pv.UnlimitedDownloads, 
		pv.MaxNumberOfDownloads, 
		pv.DownloadExpirationDays, 
		pv.DownloadActivationType, 
		pv.HasSampleDownload, 
		pv.SampleDownloadID,                       
		pv.HasUserAgreement, 
		pv.UserAgreementText, 
		pv.IsRecurring, 
		pv.CycleLength, 
		pv.CyclePeriod,
		pv.TotalCycles, 
		pv.IsShipEnabled, 
		pv.IsFreeShipping, 
		pv.AdditionalShippingCharge, 
		pv.IsTaxExempt, 
		pv.TaxCategoryID, 
		pv.ManageInventory, 
		pv.StockQuantity, 
		pv.DisplayStockAvailability, 
		pv.MinStockQuantity,                       
		pv.LowStockActivityID, 
		pv.NotifyAdminForQuantityBelow, 
		pv.AllowOutOfStockOrders, 
		pv.OrderMinimumQuantity, 
		pv.OrderMaximumQuantity, 
		pv.WarehouseID, 
		pv.DisableBuyButton, 
		pv.Price, 
		pv.OldPrice, 
		pv.ProductCost, 
		pv.Weight, 
		pv.Length, 
		pv.Width, 
		pv.Height, 
		pv.PictureID, 
		pv.AvailableStartDateTime, 
		pv.AvailableEndDateTime, 
		pv.Published,                      
		pv.Deleted, 
		pv.DisplayOrder, 
		pv.CreatedOn, 
		pv.UpdatedOn
	FROM [Nop_ProductVariant] pv
		LEFT OUTER JOIN [Nop_ProductVariantLocalized] pvl 
		ON pvl.ProductVariantId = pv.ProductVariantId AND pvl.LanguageID = @LanguageID
	WHERE
		(pv.ProductVariantID = @ProductVariantID)
END
GO





IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantLoadByProductID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantLoadByProductID]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantLoadByProductID]
(
	@ProductID int,
	@LanguageID int,
	@ShowHidden bit = 0
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		pv.ProductVariantId, 
		pv.ProductID, 
		dbo.NOP_getnotnullnotempty(pvl.[Name],pv.[Name]) as [Name], 
		pv.SKU, 
		dbo.NOP_getnotnullnotempty(pvl.[Description],pv.[Description]) as [Description], 
		pv.AdminComment, 
		pv.ManufacturerPartNumber, 
		pv.IsGiftCard, 
		pv.IsDownload, 
		pv.DownloadID,                      
		pv.UnlimitedDownloads, 
		pv.MaxNumberOfDownloads, 
		pv.DownloadExpirationDays, 
		pv.DownloadActivationType, 
		pv.HasSampleDownload, 
		pv.SampleDownloadID,                       
		pv.HasUserAgreement, 
		pv.UserAgreementText, 
		pv.IsRecurring, 
		pv.CycleLength, 
		pv.CyclePeriod,
		pv.TotalCycles, 
		pv.IsShipEnabled, 
		pv.IsFreeShipping, 
		pv.AdditionalShippingCharge, 
		pv.IsTaxExempt, 
		pv.TaxCategoryID, 
		pv.ManageInventory, 
		pv.StockQuantity, 
		pv.DisplayStockAvailability, 
		pv.MinStockQuantity,                       
		pv.LowStockActivityID, 
		pv.NotifyAdminForQuantityBelow, 
		pv.AllowOutOfStockOrders, 
		pv.OrderMinimumQuantity, 
		pv.OrderMaximumQuantity, 
		pv.WarehouseID, 
		pv.DisableBuyButton, 
		pv.Price, 
		pv.OldPrice, 
		pv.ProductCost, 
		pv.Weight, 
		pv.Length, 
		pv.Width, 
		pv.Height, 
		pv.PictureID, 
		pv.AvailableStartDateTime, 
		pv.AvailableEndDateTime, 
		pv.Published,                      
		pv.Deleted, 
		pv.DisplayOrder, 
		pv.CreatedOn, 
		pv.UpdatedOn
	FROM [Nop_ProductVariant] pv
		LEFT OUTER JOIN [Nop_ProductVariantLocalized] pvl 
		ON pvl.ProductVariantId = pv.ProductVariantId AND pvl.LanguageID = @LanguageID
	WHERE 
			(@ShowHidden = 1 OR pv.Published = 1) 
		AND 
			pv.Deleted=0
		AND 
			pv.ProductID = @ProductID
		AND 
			(
				@ShowHidden = 1
				OR
				(getutcdate() between isnull(pv.AvailableStartDateTime, '1/1/1900') and isnull(pv.AvailableEndDateTime, '1/1/2999'))
			)
	order by pv.DisplayOrder
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductAlsoPurchasedLoadByProductID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductAlsoPurchasedLoadByProductID]
GO
CREATE PROCEDURE [dbo].[Nop_ProductAlsoPurchasedLoadByProductID]
(
	@ProductID			int,
	@LanguageID			int,
	@ShowHidden			bit,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		ProductID int NOT NULL,
		ProductsPurchased int NOT NULL,
	)

	INSERT INTO #PageIndex (ProductID, ProductsPurchased)
	SELECT p.ProductID, SUM(opv.Quantity) as ProductsPurchased
	FROM    
		dbo.Nop_OrderProductVariant opv WITH (NOLOCK)
		INNER JOIN dbo.Nop_ProductVariant pv ON pv.ProductVariantId = opv.ProductVariantId
		INNER JOIN dbo.Nop_Product p ON p.ProductId = pv.ProductId
	WHERE
		opv.OrderID IN 
		(
			/* This inner query should retrieve all orders that have contained the productID */
			SELECT 
				DISTINCT OrderID
			FROM 
				dbo.Nop_OrderProductVariant opv2 WITH (NOLOCK)
				INNER JOIN dbo.Nop_ProductVariant pv2 ON pv2.ProductVariantId = opv2.ProductVariantId
				INNER JOIN dbo.Nop_Product p2 ON p2.ProductId = pv2.ProductId			
			WHERE 
				p2.ProductID = @ProductID
		)
		AND 
			(
				p.ProductId != @ProductID
			)
		AND 
			(
				@ShowHidden = 1 OR p.Published = 1
			)
		AND 
			(
				p.Deleted = 0
			)
		AND 
			(
				@ShowHidden = 1
				OR
				GETUTCDATE() BETWEEN ISNULL(pv.AvailableStartDateTime, '1/1/1900') AND ISNULL(pv.AvailableEndDateTime, '1/1/2999')
			)
	GROUP BY
		p.ProductId
	ORDER BY 
		ProductsPurchased desc


	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		p.[ProductId],
		dbo.NOP_getnotnullnotempty(pl.[Name],p.[Name]) as [Name],
		dbo.NOP_getnotnullnotempty(pl.[ShortDescription],p.[ShortDescription]) as [ShortDescription],
		dbo.NOP_getnotnullnotempty(pl.[FullDescription],p.[FullDescription]) as [FullDescription],
		p.[AdminComment], 
		p.[ProductTypeID], 
		p.[TemplateID], 
		p.[ShowOnHomePage], 
		dbo.NOP_getnotnullnotempty(pl.[MetaKeywords],p.[MetaKeywords]) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(pl.[MetaDescription],p.[MetaDescription]) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(pl.[MetaTitle],p.[MetaTitle]) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(pl.[SEName],p.[SEName]) as [SEName],
		p.[AllowCustomerReviews], 
		p.[AllowCustomerRatings], 
		p.[RatingSum], 
		p.[TotalRatingVotes], 
		p.[Published], 
		p.[Deleted], 
		p.[CreatedOn], 
		p.[UpdatedOn]
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Product p on p.ProductID = [pi].ProductID
		LEFT OUTER JOIN Nop_ProductLocalized pl with (NOLOCK) ON p.ProductID = pl.ProductID AND pl.LanguageID = @LanguageID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #PageIndex

END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductLoadDisplayedOnHomePage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductLoadDisplayedOnHomePage]
GO
CREATE PROCEDURE [dbo].[Nop_ProductLoadDisplayedOnHomePage]
(
	@ShowHidden		bit = 0,
	@LanguageID		int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		p.[ProductId],
		dbo.NOP_getnotnullnotempty(pl.[Name],p.[Name]) as [Name],
		dbo.NOP_getnotnullnotempty(pl.[ShortDescription],p.[ShortDescription]) as [ShortDescription],
		dbo.NOP_getnotnullnotempty(pl.[FullDescription],p.[FullDescription]) as [FullDescription],
		p.[AdminComment], 
		p.[ProductTypeID], 
		p.[TemplateID], 
		p.[ShowOnHomePage], 
		dbo.NOP_getnotnullnotempty(pl.[MetaKeywords],p.[MetaKeywords]) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(pl.[MetaDescription],p.[MetaDescription]) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(pl.[MetaTitle],p.[MetaTitle]) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(pl.[SEName],p.[SEName]) as [SEName],
		p.[AllowCustomerReviews], 
		p.[AllowCustomerRatings], 
		p.[RatingSum], 
		p.[TotalRatingVotes], 
		p.[Published], 
		p.[Deleted], 
		p.[CreatedOn], 
		p.[UpdatedOn]
	FROM [Nop_Product] p
		LEFT OUTER JOIN Nop_ProductLocalized pl with (NOLOCK) ON p.ProductID = pl.ProductID AND pl.LanguageID = @LanguageID
	WHERE (p.Published = 1 or @ShowHidden = 1) and p.ShowOnHomePage=1 and p.Deleted=0 
	order by p.[Name]
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductLoadRecentlyAdded]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductLoadRecentlyAdded]
GO
CREATE PROCEDURE [dbo].[Nop_ProductLoadRecentlyAdded] 
(	
	@Number			int,
	@LanguageID		int,
	@ShowHidden		bit = 0
)
AS
BEGIN
    SET NOCOUNT ON
    IF @Number is null or @Number = 0
        SET @Number = 20

	CREATE TABLE #ProductFilter
	(
	    ProductFilterID int IDENTITY (1, 1) NOT NULL,
	    ProductID int not null
	)
	
	INSERT #ProductFilter (ProductID)
	SELECT p.ProductID
	FROM Nop_Product p with (NOLOCK)
	WHERE
		(p.Published = 1 or @ShowHidden = 1) AND
		p.Deleted = 0
	ORDER BY p.CreatedOn desc

	SELECT
		p.[ProductId],
		dbo.NOP_getnotnullnotempty(pl.[Name],p.[Name]) as [Name],
		dbo.NOP_getnotnullnotempty(pl.[ShortDescription],p.[ShortDescription]) as [ShortDescription],
		dbo.NOP_getnotnullnotempty(pl.[FullDescription],p.[FullDescription]) as [FullDescription],
		p.[AdminComment], 
		p.[ProductTypeID], 
		p.[TemplateID], 
		p.[ShowOnHomePage], 
		dbo.NOP_getnotnullnotempty(pl.[MetaKeywords],p.[MetaKeywords]) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(pl.[MetaDescription],p.[MetaDescription]) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(pl.[MetaTitle],p.[MetaTitle]) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(pl.[SEName],p.[SEName]) as [SEName],
		p.[AllowCustomerReviews], 
		p.[AllowCustomerRatings], 
		p.[RatingSum], 
		p.[TotalRatingVotes], 
		p.[Published], 
		p.[Deleted], 
		p.[CreatedOn], 
		p.[UpdatedOn]
	FROM 
		Nop_Product p with (NOLOCK)
		inner join #ProductFilter pf with (NOLOCK) ON p.ProductID = pf.ProductID
		LEFT OUTER JOIN Nop_ProductLocalized pl with (NOLOCK) ON p.ProductID = pl.ProductID AND pl.LanguageID = @LanguageID
	WHERE pf.ProductFilterID <= @Number
	DROP TABLE #ProductFilter
END
GO

--attribute localization
if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_ProductAttributeLocalized]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Nop_ProductAttributeLocalized](
	[ProductAttributeLocalizedID] [int] IDENTITY(1,1) NOT NULL,
	[ProductAttributeID] [int] NOT NULL,
	[LanguageID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](400) NOT NULL,
 CONSTRAINT [PK_Nop_ProductAttributeLocalized] PRIMARY KEY CLUSTERED 
(
	[ProductAttributeLocalizedID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [IX_Nop_ProductAttributeLocalized_Unique1] UNIQUE NONCLUSTERED 
(
	[ProductAttributeID] ASC,
	[LanguageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO



IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_ProductAttributeLocalized_Nop_ProductAttribute'
           AND parent_obj = Object_id('Nop_ProductAttributeLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_ProductAttributeLocalized
DROP CONSTRAINT FK_Nop_ProductAttributeLocalized_Nop_ProductAttribute
GO
ALTER TABLE [dbo].[Nop_ProductAttributeLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_ProductAttributeLocalized_Nop_ProductAttribute] FOREIGN KEY([ProductAttributeID])
REFERENCES [dbo].[Nop_ProductAttribute] ([ProductAttributeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_ProductAttributeLocalized_Nop_Language'
           AND parent_obj = Object_id('Nop_ProductAttributeLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_ProductAttributeLocalized
DROP CONSTRAINT FK_Nop_ProductAttributeLocalized_Nop_Language
GO
ALTER TABLE [dbo].[Nop_ProductAttributeLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_ProductAttributeLocalized_Nop_Language] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Nop_Language] ([LanguageID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductAttributeLocalizedCleanUp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductAttributeLocalizedCleanUp]
GO
CREATE PROCEDURE [dbo].[Nop_ProductAttributeLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_ProductAttributeLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '') AND		
		([Description] IS NULL OR [Description] = '')
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductAttributeLocalizedInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductAttributeLocalizedInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ProductAttributeLocalizedInsert]
(
	@ProductAttributeLocalizedID int = NULL output,
	@ProductAttributeID int,
	@LanguageID int,
	@Name nvarchar(100),
	@Description nvarchar(400)
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductAttributeLocalized]
	(
		ProductAttributeID,
		LanguageID,
		[Name],
		[Description]
	)
	VALUES
	(
		@ProductAttributeID,
		@LanguageID,
		@Name,
		@Description
	)

	set @ProductAttributeLocalizedID=@@identity

	EXEC Nop_ProductAttributeLocalizedCleanUp
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductAttributeLocalizedLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductAttributeLocalizedLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_ProductAttributeLocalizedLoadByPrimaryKey]
	@ProductAttributeLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ProductAttributeLocalized]
	WHERE ProductAttributeLocalizedID = @ProductAttributeLocalizedID
	ORDER BY ProductAttributeLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductAttributeLocalizedLoadByProductAttributeIDAndLanguageID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductAttributeLocalizedLoadByProductAttributeIDAndLanguageID]
GO
CREATE PROCEDURE [dbo].[Nop_ProductAttributeLocalizedLoadByProductAttributeIDAndLanguageID]
	@ProductAttributeID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ProductAttributeLocalized]
	WHERE ProductAttributeID = @ProductAttributeID AND LanguageID=@LanguageID
	ORDER BY ProductAttributeLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductAttributeLocalizedUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductAttributeLocalizedUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ProductAttributeLocalizedUpdate]
(
	@ProductAttributeLocalizedID int,
	@ProductAttributeID int,
	@LanguageID int,
	@Name nvarchar(100),
	@Description nvarchar(400)
)
AS
BEGIN
	
	UPDATE [Nop_ProductAttributeLocalized]
	SET
		[ProductAttributeID]=@ProductAttributeID,
		[LanguageID]=@LanguageID,
		[Name]=@Name,
		[Description]=@Description
	WHERE
		ProductAttributeLocalizedID = @ProductAttributeLocalizedID

	EXEC Nop_ProductAttributeLocalizedCleanUp
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductAttributeLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductAttributeLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_ProductAttributeLoadAll]
(
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		pa.ProductAttributeID, 
		dbo.NOP_getnotnullnotempty(pal.Name,pa.Name) as [Name], 
		dbo.NOP_getnotnullnotempty(pal.Description,pa.Description) as [Description]
	FROM [Nop_ProductAttribute] pa
		LEFT OUTER JOIN [Nop_ProductAttributeLocalized] pal
		ON pa.ProductAttributeID = pal.ProductAttributeID AND pal.LanguageID = @LanguageID	
	order by pa.[Name]
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductAttributeLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductAttributeLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_ProductAttributeLoadByPrimaryKey]
(
	@ProductAttributeID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		pa.ProductAttributeID, 
		dbo.NOP_getnotnullnotempty(pal.Name,pa.Name) as [Name], 
		dbo.NOP_getnotnullnotempty(pal.Description,pa.Description) as [Description]
	FROM [Nop_ProductAttribute] pa
		LEFT OUTER JOIN [Nop_ProductAttributeLocalized] pal
		ON pa.ProductAttributeID = pal.ProductAttributeID AND pal.LanguageID = @LanguageID	
	WHERE
		pa.ProductAttributeID = @ProductAttributeID
END
GO


if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_SpecificationAttributeLocalized]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Nop_SpecificationAttributeLocalized](
	[SpecificationAttributeLocalizedID] [int] IDENTITY(1,1) NOT NULL,
	[SpecificationAttributeID] [int] NOT NULL,
	[LanguageID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Nop_SpecificationAttributeLocalized] PRIMARY KEY CLUSTERED 
(
	[SpecificationAttributeLocalizedID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [IX_Nop_SpecificationAttributeLocalized_Unique1] UNIQUE NONCLUSTERED 
(
	[SpecificationAttributeID] ASC,
	[LanguageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO



IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_SpecificationAttributeLocalized_Nop_SpecificationAttribute'
           AND parent_obj = Object_id('Nop_SpecificationAttributeLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_SpecificationAttributeLocalized
DROP CONSTRAINT FK_Nop_SpecificationAttributeLocalized_Nop_SpecificationAttribute
GO
ALTER TABLE [dbo].[Nop_SpecificationAttributeLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_SpecificationAttributeLocalized_Nop_SpecificationAttribute] FOREIGN KEY([SpecificationAttributeID])
REFERENCES [dbo].[Nop_SpecificationAttribute] ([SpecificationAttributeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_SpecificationAttributeLocalized_Nop_Language'
           AND parent_obj = Object_id('Nop_SpecificationAttributeLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_SpecificationAttributeLocalized
DROP CONSTRAINT FK_Nop_SpecificationAttributeLocalized_Nop_Language
GO
ALTER TABLE [dbo].[Nop_SpecificationAttributeLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_SpecificationAttributeLocalized_Nop_Language] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Nop_Language] ([LanguageID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeLocalizedCleanUp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeLocalizedCleanUp]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_SpecificationAttributeLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '')
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeLocalizedInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeLocalizedInsert]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeLocalizedInsert]
(
	@SpecificationAttributeLocalizedID int = NULL output,
	@SpecificationAttributeID int,
	@LanguageID int,
	@Name nvarchar(100)
)
AS
BEGIN
	INSERT
	INTO [Nop_SpecificationAttributeLocalized]
	(
		SpecificationAttributeID,
		LanguageID,
		[Name]
	)
	VALUES
	(
		@SpecificationAttributeID,
		@LanguageID,
		@Name
	)

	set @SpecificationAttributeLocalizedID=@@identity

	EXEC Nop_SpecificationAttributeLocalizedCleanUp
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeLocalizedLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeLocalizedLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeLocalizedLoadByPrimaryKey]
	@SpecificationAttributeLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_SpecificationAttributeLocalized]
	WHERE SpecificationAttributeLocalizedID = @SpecificationAttributeLocalizedID
	ORDER BY SpecificationAttributeLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeLocalizedLoadBySpecificationAttributeIDAndLanguageID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeLocalizedLoadBySpecificationAttributeIDAndLanguageID]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeLocalizedLoadBySpecificationAttributeIDAndLanguageID]
	@SpecificationAttributeID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_SpecificationAttributeLocalized]
	WHERE SpecificationAttributeID = @SpecificationAttributeID AND LanguageID=@LanguageID
	ORDER BY SpecificationAttributeLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeLocalizedUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeLocalizedUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeLocalizedUpdate]
(
	@SpecificationAttributeLocalizedID int,
	@SpecificationAttributeID int,
	@LanguageID int,
	@Name nvarchar(100)
)
AS
BEGIN
	
	UPDATE [Nop_SpecificationAttributeLocalized]
	SET
		[SpecificationAttributeID]=@SpecificationAttributeID,
		[LanguageID]=@LanguageID,
		[Name]=@Name		
	WHERE
		SpecificationAttributeLocalizedID = @SpecificationAttributeLocalizedID

	EXEC Nop_SpecificationAttributeLocalizedCleanUp
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeLoadAll]
(
	@LanguageID int
)
AS
BEGIN

	SET NOCOUNT ON
	
	SELECT
		sa.SpecificationAttributeID, 
		dbo.NOP_getnotnullnotempty(sal.Name,sa.Name) as [Name],
		sa.DisplayOrder
	FROM [Nop_SpecificationAttribute] sa
		LEFT OUTER JOIN [Nop_SpecificationAttributeLocalized] sal
		ON sa.SpecificationAttributeID = sal.SpecificationAttributeID AND sal.LanguageID = @LanguageID	
	ORDER BY sa.DisplayOrder
	
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeLoadByPrimaryKey]
(
	@SpecificationAttributeID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		sa.SpecificationAttributeID, 
		dbo.NOP_getnotnullnotempty(sal.Name,sa.Name) as [Name],
		sa.DisplayOrder
	FROM [Nop_SpecificationAttribute] sa
		LEFT OUTER JOIN [Nop_SpecificationAttributeLocalized] sal
		ON sa.SpecificationAttributeID = sal.SpecificationAttributeID AND sal.LanguageID = @LanguageID	
	WHERE
		sa.SpecificationAttributeID = @SpecificationAttributeID
END
GO

		
--forum URL rewrites
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'SEO.ForumGroup.UrlRewriteFormat')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'SEO.ForumGroup.UrlRewriteFormat', N'{0}boards/fg/{1}/{2}.aspx', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'SEO.Forum.UrlRewriteFormat')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'SEO.Forum.UrlRewriteFormat', N'{0}boards/f/{1}/{2}.aspx', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'SEO.ForumTopic.UrlRewriteFormat')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'SEO.ForumTopic.UrlRewriteFormat', N'{0}boards/t/{1}/{2}.aspx', N'')
END
GO


--attribute values localization

if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_ProductVariantAttributeValueLocalized]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Nop_ProductVariantAttributeValueLocalized](
	[ProductVariantAttributeValueLocalizedID] [int] IDENTITY(1,1) NOT NULL,
	[ProductVariantAttributeValueID] [int] NOT NULL,
	[LanguageID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Nop_ProductVariantAttributeValueLocalized] PRIMARY KEY CLUSTERED 
(
	[ProductVariantAttributeValueLocalizedID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [IX_Nop_ProductVariantAttributeValueLocalized_Unique1] UNIQUE NONCLUSTERED 
(
	[ProductVariantAttributeValueID] ASC,
	[LanguageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO



IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_ProductVariantAttributeValueLocalized_Nop_ProductVariantAttributeValue'
           AND parent_obj = Object_id('Nop_ProductVariantAttributeValueLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_ProductVariantAttributeValueLocalized
DROP CONSTRAINT FK_Nop_ProductVariantAttributeValueLocalized_Nop_ProductVariantAttributeValue
GO
ALTER TABLE [dbo].[Nop_ProductVariantAttributeValueLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_ProductVariantAttributeValueLocalized_Nop_ProductVariantAttributeValue] FOREIGN KEY([ProductVariantAttributeValueID])
REFERENCES [dbo].[Nop_ProductVariantAttributeValue] ([ProductVariantAttributeValueID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_ProductVariantAttributeValueLocalized_Nop_Language'
           AND parent_obj = Object_id('Nop_ProductVariantAttributeValueLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_ProductVariantAttributeValueLocalized
DROP CONSTRAINT FK_Nop_ProductVariantAttributeValueLocalized_Nop_Language
GO
ALTER TABLE [dbo].[Nop_ProductVariantAttributeValueLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_ProductVariantAttributeValueLocalized_Nop_Language] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Nop_Language] ([LanguageID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantAttributeValueLocalizedCleanUp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLocalizedCleanUp]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_ProductVariantAttributeValueLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '')
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantAttributeValueLocalizedInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLocalizedInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLocalizedInsert]
(
	@ProductVariantAttributeValueLocalizedID int = NULL output,
	@ProductVariantAttributeValueID int,
	@LanguageID int,
	@Name nvarchar(100)
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductVariantAttributeValueLocalized]
	(
		ProductVariantAttributeValueID,
		LanguageID,
		[Name]
	)
	VALUES
	(
		@ProductVariantAttributeValueID,
		@LanguageID,
		@Name
	)

	set @ProductVariantAttributeValueLocalizedID=@@identity

	EXEC Nop_ProductVariantAttributeValueLocalizedCleanUp
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantAttributeValueLocalizedLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLocalizedLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLocalizedLoadByPrimaryKey]
	@ProductVariantAttributeValueLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ProductVariantAttributeValueLocalized]
	WHERE ProductVariantAttributeValueLocalizedID = @ProductVariantAttributeValueLocalizedID
	ORDER BY ProductVariantAttributeValueLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantAttributeValueLocalizedLoadByProductVariantAttributeValueIDAndLanguageID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLocalizedLoadByProductVariantAttributeValueIDAndLanguageID]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLocalizedLoadByProductVariantAttributeValueIDAndLanguageID]
	@ProductVariantAttributeValueID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_ProductVariantAttributeValueLocalized]
	WHERE ProductVariantAttributeValueID = @ProductVariantAttributeValueID AND LanguageID=@LanguageID
	ORDER BY ProductVariantAttributeValueLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantAttributeValueLocalizedUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLocalizedUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLocalizedUpdate]
(
	@ProductVariantAttributeValueLocalizedID int,
	@ProductVariantAttributeValueID int,
	@LanguageID int,
	@Name nvarchar(100)
)
AS
BEGIN
	
	UPDATE [Nop_ProductVariantAttributeValueLocalized]
	SET
		[ProductVariantAttributeValueID]=@ProductVariantAttributeValueID,
		[LanguageID]=@LanguageID,
		[Name]=@Name	
	WHERE
		ProductVariantAttributeValueLocalizedID = @ProductVariantAttributeValueLocalizedID

	EXEC Nop_ProductVariantAttributeValueLocalizedCleanUp
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantAttributeValueLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLoadByPrimaryKey]
(
	@ProductVariantAttributeValueID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		pvav.ProductVariantAttributeValueID, 
		pvav.ProductVariantAttributeID, 
		dbo.NOP_getnotnullnotempty(pvavl.Name,pvav.Name) as [Name],
		pvav.PriceAdjustment, 
		pvav.WeightAdjustment, 
		pvav.IsPreSelected, 
		pvav.DisplayOrder
	FROM [Nop_ProductVariantAttributeValue] pvav
		LEFT OUTER JOIN [Nop_ProductVariantAttributeValueLocalized] pvavl 
		ON pvav.ProductVariantAttributeValueID = pvavl.ProductVariantAttributeValueID AND pvavl.LanguageID = @LanguageID	
	WHERE
		pvav.ProductVariantAttributeValueID = @ProductVariantAttributeValueID
END
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantAttributeValueLoadByProductVariantAttributeID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLoadByProductVariantAttributeID]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantAttributeValueLoadByProductVariantAttributeID]
(
	@ProductVariantAttributeID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		pvav.ProductVariantAttributeValueID, 
		pvav.ProductVariantAttributeID, 
		dbo.NOP_getnotnullnotempty(pvavl.Name,pvav.Name) as [Name],
		pvav.PriceAdjustment, 
		pvav.WeightAdjustment, 
		pvav.IsPreSelected, 
		pvav.DisplayOrder
	FROM [Nop_ProductVariantAttributeValue] pvav
		LEFT OUTER JOIN [Nop_ProductVariantAttributeValueLocalized] pvavl 
		ON pvav.ProductVariantAttributeValueID = pvavl.ProductVariantAttributeValueID AND pvavl.LanguageID = @LanguageID	
	WHERE 
		pvav.ProductVariantAttributeID=@ProductVariantAttributeID
	ORDER BY pvav.[DisplayOrder]
END
GO


if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_SpecificationAttributeOptionLocalized]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Nop_SpecificationAttributeOptionLocalized](
	[SpecificationAttributeOptionLocalizedID] [int] IDENTITY(1,1) NOT NULL,
	[SpecificationAttributeOptionID] [int] NOT NULL,
	[LanguageID] [int] NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_Nop_SpecificationAttributeOptionLocalized] PRIMARY KEY CLUSTERED 
(
	[SpecificationAttributeOptionLocalizedID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [IX_Nop_SpecificationAttributeOptionLocalized_Unique1] UNIQUE NONCLUSTERED 
(
	[SpecificationAttributeOptionID] ASC,
	[LanguageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO



IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_SpecificationAttributeOptionLocalized_Nop_SpecificationAttributeOption'
           AND parent_obj = Object_id('Nop_SpecificationAttributeOptionLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_SpecificationAttributeOptionLocalized
DROP CONSTRAINT FK_Nop_SpecificationAttributeOptionLocalized_Nop_SpecificationAttributeOption
GO
ALTER TABLE [dbo].[Nop_SpecificationAttributeOptionLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_SpecificationAttributeOptionLocalized_Nop_SpecificationAttributeOption] FOREIGN KEY([SpecificationAttributeOptionID])
REFERENCES [dbo].[Nop_SpecificationAttributeOption] ([SpecificationAttributeOptionID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_SpecificationAttributeOptionLocalized_Nop_Language'
           AND parent_obj = Object_id('Nop_SpecificationAttributeOptionLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_SpecificationAttributeOptionLocalized
DROP CONSTRAINT FK_Nop_SpecificationAttributeOptionLocalized_Nop_Language
GO
ALTER TABLE [dbo].[Nop_SpecificationAttributeOptionLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_SpecificationAttributeOptionLocalized_Nop_Language] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Nop_Language] ([LanguageID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeOptionLocalizedCleanUp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLocalizedCleanUp]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_SpecificationAttributeOptionLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '')
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeOptionLocalizedInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLocalizedInsert]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLocalizedInsert]
(
	@SpecificationAttributeOptionLocalizedID int = NULL output,
	@SpecificationAttributeOptionID int,
	@LanguageID int,
	@Name nvarchar(500)
)
AS
BEGIN
	INSERT
	INTO [Nop_SpecificationAttributeOptionLocalized]
	(
		SpecificationAttributeOptionID,
		LanguageID,
		[Name]
	)
	VALUES
	(
		@SpecificationAttributeOptionID,
		@LanguageID,
		@Name
	)

	set @SpecificationAttributeOptionLocalizedID=@@identity

	EXEC Nop_SpecificationAttributeOptionLocalizedCleanUp
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeOptionLocalizedLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLocalizedLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLocalizedLoadByPrimaryKey]
	@SpecificationAttributeOptionLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_SpecificationAttributeOptionLocalized]
	WHERE SpecificationAttributeOptionLocalizedID = @SpecificationAttributeOptionLocalizedID
	ORDER BY SpecificationAttributeOptionLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeOptionLocalizedLoadBySpecificationAttributeOptionIDAndLanguageID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLocalizedLoadBySpecificationAttributeOptionIDAndLanguageID]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLocalizedLoadBySpecificationAttributeOptionIDAndLanguageID]
	@SpecificationAttributeOptionID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_SpecificationAttributeOptionLocalized]
	WHERE SpecificationAttributeOptionID = @SpecificationAttributeOptionID AND LanguageID=@LanguageID
	ORDER BY SpecificationAttributeOptionLocalizedID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeOptionLocalizedUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLocalizedUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLocalizedUpdate]
(
	@SpecificationAttributeOptionLocalizedID int,
	@SpecificationAttributeOptionID int,
	@LanguageID int,
	@Name nvarchar(500)
)
AS
BEGIN
	
	UPDATE [Nop_SpecificationAttributeOptionLocalized]
	SET
		[SpecificationAttributeOptionID]=@SpecificationAttributeOptionID,
		[LanguageID]=@LanguageID,
		[Name]=@Name		
	WHERE
		SpecificationAttributeOptionLocalizedID = @SpecificationAttributeOptionLocalizedID

	EXEC Nop_SpecificationAttributeOptionLocalizedCleanUp
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeOptionFilter_LoadByFilter]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeOptionFilter_LoadByFilter]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionFilter_LoadByFilter]
(
	@CategoryID int,
	@LanguageID int
)
AS
BEGIN
	SELECT 
		sa.SpecificationAttributeID,
		dbo.NOP_getnotnullnotempty(sal.Name,sa.Name) as [SpecificationAttributeName],
		sa.DisplayOrder,
		sao.SpecificationAttributeOptionID,
		dbo.NOP_getnotnullnotempty(saol.Name,sao.Name) as [SpecificationAttributeOptionName]
	FROM Nop_Product_SpecificationAttribute_Mapping psam with (NOLOCK)
		INNER JOIN Nop_SpecificationAttributeOption sao with (NOLOCK) ON
			sao.SpecificationAttributeOptionID = psam.SpecificationAttributeOptionID
		INNER JOIN Nop_SpecificationAttribute sa with (NOLOCK) ON
			sa.SpecificationAttributeID = sao.SpecificationAttributeID	
		INNER JOIN Nop_Product_Category_Mapping pcm with (NOLOCK) ON 
			pcm.ProductID = psam.ProductID	
		INNER JOIN Nop_Product p ON 
			psam.ProductID = p.ProductID
		LEFT OUTER JOIN Nop_ProductVariant pv with (NOLOCK) ON 
			p.ProductID = pv.ProductID
		LEFT OUTER JOIN [Nop_SpecificationAttributeLocalized] sal with (NOLOCK) ON 
			sa.SpecificationAttributeID = sal.SpecificationAttributeID AND sal.LanguageID = @LanguageID	
		LEFT OUTER JOIN [Nop_SpecificationAttributeOptionLocalized] saol with (NOLOCK) ON 
			sao.SpecificationAttributeOptionID = saol.SpecificationAttributeOptionID AND saol.LanguageID = @LanguageID	
	WHERE 
			p.Published = 1
		AND 
			pv.Published = 1
		AND 
			p.Deleted=0
		AND
			pcm.CategoryID = @CategoryID
		AND
			psam.AllowFiltering = 1
		AND
			getutcdate() between isnull(pv.AvailableStartDateTime, '1/1/1900') and isnull(pv.AvailableEndDateTime, '1/1/2999')
	GROUP BY
		sa.SpecificationAttributeID, 
		dbo.NOP_getnotnullnotempty(sal.Name,sa.Name),
		sa.DisplayOrder,
		sao.SpecificationAttributeOptionID,
		dbo.NOP_getnotnullnotempty(saol.Name,sao.Name)
	ORDER BY sa.DisplayOrder, [SpecificationAttributeName], [SpecificationAttributeOptionName]
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeOptionLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLoadAll]
(
	@LanguageID int
)
AS
BEGIN

	SELECT 
		sao.SpecificationAttributeOptionID, 
		sao.SpecificationAttributeID, 
		dbo.NOP_getnotnullnotempty(saol.Name,sao.Name) as [Name],
		sao.DisplayOrder
	FROM Nop_SpecificationAttributeOption sao
		LEFT OUTER JOIN [Nop_SpecificationAttributeOptionLocalized] saol with (NOLOCK) ON 
			sao.SpecificationAttributeOptionID = saol.SpecificationAttributeOptionID AND saol.LanguageID = @LanguageID	
	ORDER BY sao.DisplayOrder
	
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeOptionLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLoadByPrimaryKey]

	@SpecificationAttributeOptionID int,
	@LanguageID int

AS
BEGIN

	SELECT
		sao.SpecificationAttributeOptionID, 
		sao.SpecificationAttributeID, 
		dbo.NOP_getnotnullnotempty(saol.Name,sao.Name) as [Name],
		sao.DisplayOrder
	FROM Nop_SpecificationAttributeOption sao
		LEFT OUTER JOIN [Nop_SpecificationAttributeOptionLocalized] saol with (NOLOCK) ON 
			sao.SpecificationAttributeOptionID = saol.SpecificationAttributeOptionID AND saol.LanguageID = @LanguageID	
	WHERE 
		sao.SpecificationAttributeOptionID = @SpecificationAttributeOptionID

END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeOptionLoadBySpecificationAttributeID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLoadBySpecificationAttributeID]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLoadBySpecificationAttributeID]

	@SpecificationAttributeID int,
	@LanguageID int

AS
BEGIN

	SELECT
		sao.SpecificationAttributeOptionID, 
		sao.SpecificationAttributeID, 
		dbo.NOP_getnotnullnotempty(saol.Name,sao.Name) as [Name],
		sao.DisplayOrder
	FROM Nop_SpecificationAttributeOption sao
		LEFT OUTER JOIN [Nop_SpecificationAttributeOptionLocalized] saol with (NOLOCK) ON 
			sao.SpecificationAttributeOptionID = saol.SpecificationAttributeOptionID AND saol.LanguageID = @LanguageID	
	WHERE
		sao.SpecificationAttributeID = @SpecificationAttributeID
	ORDER BY sao.DisplayOrder

END
GO


--prices depending on customer role
if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_CustomerRole_ProductPrice]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Nop_CustomerRole_ProductPrice](
	[CustomerRoleProductPriceID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerRoleID] [int] NOT NULL,
	[ProductVariantID] [int] NOT NULL,
	[Price] [money] NOT NULL
 CONSTRAINT [PK_Nop_CustomerRole_ProductPrice] PRIMARY KEY CLUSTERED 
(
	[CustomerRoleProductPriceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
) ON [PRIMARY]
END
GO

IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_CustomerRole_ProductPrice_Nop_CustomerRole'
           AND parent_obj = Object_id('Nop_CustomerRole_ProductPrice')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_CustomerRole_ProductPrice
DROP CONSTRAINT FK_Nop_CustomerRole_ProductPrice_Nop_CustomerRole
GO
ALTER TABLE [dbo].[Nop_CustomerRole_ProductPrice]  WITH CHECK ADD  CONSTRAINT [FK_Nop_CustomerRole_ProductPrice_Nop_CustomerRole] FOREIGN KEY([CustomerRoleID])
REFERENCES [dbo].[Nop_CustomerRole] ([CustomerRoleID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_CustomerRole_ProductPrice_Nop_ProductVariant'
           AND parent_obj = Object_id('Nop_CustomerRole_ProductPrice')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_CustomerRole_ProductPrice
DROP CONSTRAINT FK_Nop_CustomerRole_ProductPrice_Nop_ProductVariant
GO
ALTER TABLE [dbo].[Nop_CustomerRole_ProductPrice]  WITH CHECK ADD  CONSTRAINT [FK_Nop_CustomerRole_ProductPrice_Nop_ProductVariant] FOREIGN KEY([ProductVariantID])
REFERENCES [dbo].[Nop_ProductVariant] ([ProductVariantID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO




IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerRole_ProductPriceDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerRole_ProductPriceDelete]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerRole_ProductPriceDelete]
(
	@CustomerRoleProductPriceID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_CustomerRole_ProductPrice]
	WHERE
		[CustomerRoleProductPriceID] = @CustomerRoleProductPriceID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerRole_ProductPriceInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerRole_ProductPriceInsert]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerRole_ProductPriceInsert]
(
	@CustomerRoleProductPriceID int = NULL output,
	@CustomerRoleID int,
	@ProductVariantID int,
	@Price money
)
AS
BEGIN
	INSERT
	INTO [Nop_CustomerRole_ProductPrice]
	(
		[CustomerRoleID],
		[ProductVariantID],
		[Price]
	)
	VALUES
	(
		@CustomerRoleID,
		@ProductVariantID,
		@Price
	)

	set @CustomerRoleProductPriceID=SCOPE_IDENTITY()
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerRole_ProductPriceLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerRole_ProductPriceLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerRole_ProductPriceLoadAll]
(
	@ProductVariantID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_CustomerRole_ProductPrice]
	WHERE
		ProductVariantID= @ProductVariantID
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerRole_ProductPriceLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerRole_ProductPriceLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerRole_ProductPriceLoadByPrimaryKey]
(
	@CustomerRoleProductPriceID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_CustomerRole_ProductPrice]
	WHERE
		[CustomerRoleProductPriceID] = @CustomerRoleProductPriceID
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerRole_ProductPriceUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerRole_ProductPriceUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerRole_ProductPriceUpdate]
(
	@CustomerRoleProductPriceID int,
	@CustomerRoleID int,
	@ProductVariantID int,
	@Price money
)
AS
BEGIN
	UPDATE [Nop_CustomerRole_ProductPrice]
	SET
		[CustomerRoleID] = @CustomerRoleID,
		[ProductVariantID] = @ProductVariantID,
		[Price] = @Price
	WHERE
		CustomerRoleProductPriceID = @CustomerRoleProductPriceID
END
GO

-- blog paging
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_BlogPostLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_BlogPostLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_BlogPostLoadAll]
(
	@LanguageID	int,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		BlogPostID int NOT NULL,
	)

	INSERT INTO #PageIndex (BlogPostID)
	SELECT
		bp.BlogPostID
	FROM 
	    Nop_BlogPost bp 
	WITH 
		(NOLOCK)
	WHERE
		@LanguageID IS NULL OR @LanguageID = 0 OR LanguageID = @LanguageID
	ORDER BY 
		CreatedOn 
	DESC


	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		bp.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_BlogPost bp on bp.BlogPostID = [pi].BlogPostID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0
END
GO

-- localized message templates IsActive

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_MessageTemplateLocalized]') and NAME='IsActive')
BEGIN
	ALTER TABLE [dbo].[Nop_MessageTemplateLocalized] 
	ADD IsActive bit NOT NULL CONSTRAINT [DF_Nop_MessageTemplateLocalized_IsActive] DEFAULT ((1))
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_MessageTemplateLocalizedInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_MessageTemplateLocalizedInsert]
GO
CREATE PROCEDURE [dbo].[Nop_MessageTemplateLocalizedInsert]
(
	@MessageTemplateLocalizedID int = NULL output,
	@MessageTemplateID int,
	@LanguageID int,
	@BCCEmailAddresses nvarchar(200),
	@Subject nvarchar(200),
	@Body nvarchar(MAX),
	@IsActive bit
)
AS
BEGIN
	INSERT
	INTO [Nop_MessageTemplateLocalized]
	(
		MessageTemplateID,
		LanguageID,
		BCCEmailAddresses,
		[Subject],
		Body,
		IsActive
	)
	VALUES
	(
		@MessageTemplateID,
		@LanguageID,
		@BCCEmailAddresses,
		@Subject,
		@Body,
		@IsActive
	)

	set @MessageTemplateLocalizedID=SCOPE_IDENTITY()
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_MessageTemplateLocalizedUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_MessageTemplateLocalizedUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_MessageTemplateLocalizedUpdate]
(
	@MessageTemplateLocalizedID int,
	@MessageTemplateID int,
	@LanguageID int,
	@BCCEmailAddresses nvarchar(200),	
	@Subject nvarchar(200),
	@Body nvarchar(MAX),
	@IsActive bit
)
AS
BEGIN

	UPDATE [Nop_MessageTemplateLocalized]
	SET
		MessageTemplateID=@MessageTemplateID,
		LanguageID=@LanguageID,
		BCCEmailAddresses=@BCCEmailAddresses,
		[Subject]=@Subject,
		Body=@Body,
		IsActive=@IsActive
	WHERE
		MessageTemplateLocalizedID = @MessageTemplateLocalizedID

END
GO


--order average report bug fixed
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderAverageReport]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderAverageReport]
GO
CREATE PROCEDURE [dbo].[Nop_OrderAverageReport]
(
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@OrderStatusID int
)
AS
BEGIN

	SET NOCOUNT ON

	SELECT 
		SUM(o.OrderTotal) as SumOrders,
		COUNT(1) as CountOrders
	FROM [Nop_Order] o
	WHERE 
		(@StartTime is NULL or @StartTime <= o.CreatedOn) AND
		(@EndTime is NULL or @EndTime >= o.CreatedOn) AND
		o.OrderTotal > 0 AND 
		o.OrderStatusID=@OrderStatusID AND 
		o.Deleted=0	
END
GO

--further report bug fixes (exact datetime)
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ActivityLogLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ActivityLogLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_ActivityLogLoadAll]
(
	@CreatedOnFrom datetime = NULL,
	@CreatedOnTo datetime = NULL,
	@Email nvarchar(200),
	@Username nvarchar(200),
	@ActivityLogTypeID int,
	@PageIndex int = 0, 
	@PageSize int = 2147483644,
	@TotalRecords int = null OUTPUT
)
AS
BEGIN
	SET @Email = isnull(@Email, '')
	SET @Email = '%' + rtrim(ltrim(@Email)) + '%'

	SET @Username = isnull(@Username, '')
	SET @Username = '%' + rtrim(ltrim(@Username)) + '%'


	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		ActivityLogID int NOT NULL,
		CreatedOn datetime NOT NULL
	)

	INSERT INTO #PageIndex (ActivityLogID, CreatedOn)
	SELECT DISTINCT
		al.ActivityLogID,
		al.CreatedOn
	FROM [Nop_ActivityLog] al with (NOLOCK)
	INNER JOIN [Nop_Customer] c on c.CustomerID = al.CustomerID
	WHERE 
		(@CreatedOnFrom is NULL or @CreatedOnFrom <= al.CreatedOn) and
		(@CreatedOnTo is NULL or @CreatedOnTo >= al.CreatedOn) and 
		(patindex(@Email, isnull(c.Email, '')) > 0) and
		(patindex(@Username, isnull(c.Username, '')) > 0) and
		(c.IsGuest=0) and (c.deleted=0) and
		(@ActivityLogTypeID is null or (al.ActivityLogTypeID=@ActivityLogTypeID)) 
	ORDER BY al.CreatedOn DESC

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT
		al.*
	FROM
		#PageIndex [pi]
		INNER JOIN [Nop_ActivityLog] al on al.ActivityLogID = [pi].ActivityLogID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #PageIndex	
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerLoadAll]
(
	@StartTime				datetime = NULL,
	@EndTime				datetime = NULL,
	@Email					nvarchar(200),
	@Username				nvarchar(200),
	@DontLoadGuestCustomers	bit = 0,
	@PageIndex				int = 0, 
	@PageSize				int = 2147483644,
	@TotalRecords			int = null OUTPUT
)
AS
BEGIN

	SET @Email = isnull(@Email, '')
	SET @Email = '%' + rtrim(ltrim(@Email)) + '%'

	SET @Username = isnull(@Username, '')
	SET @Username = '%' + rtrim(ltrim(@Username)) + '%'


	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	DECLARE @TotalThreads int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		CustomerID int NOT NULL,
		RegistrationDate datetime NOT NULL,
	)

	INSERT INTO #PageIndex (CustomerID, RegistrationDate)
	SELECT DISTINCT
		c.CustomerID, c.RegistrationDate
	FROM [Nop_Customer] c with (NOLOCK)
	WHERE 
		(@StartTime is NULL or @StartTime <= c.RegistrationDate) and
		(@EndTime is NULL or @EndTime >= c.RegistrationDate) and 
		(patindex(@Email, isnull(c.Email, '')) > 0) and
		(patindex(@Username, isnull(c.Username, '')) > 0) and
		(@DontLoadGuestCustomers = 0 or (c.IsGuest=0)) and 
		c.deleted=0
	order by c.RegistrationDate desc 

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		c.*
	FROM
		#PageIndex [pi]
		INNER JOIN [Nop_Customer] c on c.CustomerID = [pi].CustomerID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #PageIndex
	
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_GiftCardLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_GiftCardLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_GiftCardLoadAll]
(
	@OrderID int,
	@CustomerID int,
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@OrderStatusID int,
	@PaymentStatusID int,
	@ShippingStatusID int,
	@IsGiftCardActivated bit = null, --0 not activated records, 1 activated records, null - load all records
	@GiftCardCouponCode nvarchar(100)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_GiftCard]
	WHERE GiftCardID IN
	(
		SELECT DISTINCT gc.GiftCardID
		FROM [Nop_GiftCard] gc
		INNER JOIN [Nop_OrderProductVariant] opv ON gc.PurchasedOrderProductVariantID=opv.OrderProductVariantID
		INNER JOIN [Nop_Order] o ON opv.OrderID=o.OrderID
		WHERE
			(@OrderID IS NULL OR @OrderID=0 or o.OrderID = @OrderID) and
			(@CustomerID IS NULL OR @CustomerID=0 or o.CustomerID = @CustomerID) and
			(@StartTime is NULL or @StartTime <= gc.CreatedOn) and
			(@EndTime is NULL or @EndTime >= gc.CreatedOn) and 
			(@OrderStatusID IS NULL or @OrderStatusID=0 or o.OrderStatusID = @OrderStatusID) and
			(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o.PaymentStatusID = @PaymentStatusID) and
			(@ShippingStatusID IS NULL OR @ShippingStatusID = 0 OR o.ShippingStatusID = @ShippingStatusID) and
			(@IsGiftCardActivated IS NULL OR gc.IsGiftCardActivated = @IsGiftCardActivated) and
			(@GiftCardCouponCode IS NULL OR @GiftCardCouponCode ='' OR gc.GiftCardCouponCode = @GiftCardCouponCode)		
	)
	ORDER BY CreatedOn desc, GiftCardID 
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderProductVariantLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderProductVariantLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_OrderProductVariantLoadAll]
(
	@OrderID int,
	@CustomerID int,
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@OrderStatusID int,
	@PaymentStatusID int,
	@ShippingStatusID int,
	@LoadDownloableProductsOnly bit = NULL
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		opv.*
	FROM [Nop_OrderProductVariant] opv
	INNER JOIN [Nop_Order] o ON opv.OrderID=o.OrderID
	INNER JOIN [Nop_ProductVariant] pv ON opv.ProductVariantID=pv.ProductVariantID
	WHERE
		(@OrderID IS NULL OR @OrderID=0 or o.OrderID = @OrderID) and
		(@CustomerID IS NULL OR @CustomerID=0 or o.CustomerID = @CustomerID) and
		(@StartTime is NULL or @StartTime <= o.CreatedOn) and
		(@EndTime is NULL or @EndTime >= o.CreatedOn) and 
		(@OrderStatusID IS NULL or @OrderStatusID=0 or o.OrderStatusID = @OrderStatusID) and
		(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o.PaymentStatusID = @PaymentStatusID) and
		(@ShippingStatusID IS NULL OR @ShippingStatusID = 0 OR o.ShippingStatusID = @ShippingStatusID) and
		((@LoadDownloableProductsOnly IS NULL OR @LoadDownloableProductsOnly = 0) OR (pv.IsDownload=1)) and
		(o.Deleted=0)		
	ORDER BY o.CreatedOn desc, [opv].OrderProductVariantID 
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderProductVariantReport]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderProductVariantReport]
GO
CREATE PROCEDURE [dbo].[Nop_OrderProductVariantReport]
(
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@OrderStatusID int,
	@PaymentStatusID int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT DISTINCT opv.ProductVariantID,
		(	
			select sum(opv2.PriceExclTax)
			from Nop_OrderProductVariant opv2
			INNER JOIN [Nop_Order] o2 
			on o2.OrderId = opv2.OrderID 
			where
				(@StartTime is NULL or @StartTime <= o2.CreatedOn) and
				(@EndTime is NULL or @EndTime >= o2.CreatedOn) and 
				(@OrderStatusID IS NULL or @OrderStatusID=0 or o2.OrderStatusID = @OrderStatusID) and
				(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o2.PaymentStatusID = @PaymentStatusID) and
				(o2.Deleted=0) and 
				(opv2.ProductVariantID = opv.ProductVariantID)) PriceExclTax, 
		(
			select sum(opv2.Quantity)  
			from Nop_OrderProductVariant opv2 
			INNER JOIN [Nop_Order] o2 
			on o2.OrderId = opv2.OrderID 
			where
				(@StartTime is NULL or @StartTime <= o2.CreatedOn) and
				(@EndTime is NULL or @EndTime >= o2.CreatedOn) and 
				(@OrderStatusID IS NULL or @OrderStatusID=0 or o2.OrderStatusID = @OrderStatusID) and
				(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o2.PaymentStatusID = @PaymentStatusID) and
				(o2.Deleted=0) and 
				(opv2.ProductVariantID = opv.ProductVariantID)) Total 
	FROM Nop_OrderProductVariant opv 
	INNER JOIN [Nop_Order] o 
	on o.OrderId = opv.OrderID
	WHERE
		(@StartTime is NULL or @StartTime <= o.CreatedOn) and
		(@EndTime is NULL or @EndTime >= o.CreatedOn) and 
		(@OrderStatusID IS NULL or @OrderStatusID=0 or o.OrderStatusID = @OrderStatusID) and
		(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o.PaymentStatusID = @PaymentStatusID) and
		(o.Deleted=0)

END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderSearch]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderSearch]
GO
CREATE PROCEDURE [dbo].[Nop_OrderSearch]
(
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@CustomerEmail nvarchar(255) = NULL,
	@OrderStatusID int,
	@PaymentStatusID int,
	@ShippingStatusID int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		o.*
	FROM [Nop_Order] o
	LEFT OUTER JOIN [Nop_Customer] c ON o.CustomerID = c.CustomerID
	WHERE
		(@CustomerEmail IS NULL or LEN(@CustomerEmail)=0 or (c.Email like '%' + COALESCE(@CustomerEmail,c.Email) + '%')) and
		(@StartTime is NULL or @StartTime <= o.CreatedOn) and
		(@EndTime is NULL or @EndTime >= o.CreatedOn) and 
		(@OrderStatusID IS NULL or @OrderStatusID=0 or o.OrderStatusID = @OrderStatusID) and
		(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o.PaymentStatusID = @PaymentStatusID) and
		(@ShippingStatusID IS NULL OR @ShippingStatusID = 0 OR o.ShippingStatusID = @ShippingStatusID) and
		(o.Deleted=0)
	ORDER BY o.CreatedOn desc
END
GO




IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_QueuedEmailLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_QueuedEmailLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_QueuedEmailLoadAll]
(	
	@FromEmail nvarchar(255) = NULL,
	@ToEmail nvarchar(255) = NULL,
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@QueuedEmailCount int,
	@LoadNotSentItemsOnly bit,
	@MaxSendTries int
)
AS
BEGIN
	IF (@QueuedEmailCount > 0)
	SET ROWCOUNT @QueuedEmailCount

	SELECT qu.*
	FROM [Nop_QueuedEmail] qu
	WHERE 
		(@FromEmail IS NULL or LEN(@FromEmail)=0 or (qu.[From] like '%' + COALESCE(@FromEmail,qu.[From]) + '%')) AND
		(@ToEmail IS NULL or LEN(@ToEmail)=0 or (qu.[To] like '%' + COALESCE(@ToEmail,qu.[To]) + '%')) AND
		(@StartTime is NULL or @StartTime <= qu.CreatedOn) AND
		(@EndTime is NULL or @EndTime >= qu.CreatedOn) AND 
		((@LoadNotSentItemsOnly IS NULL OR @LoadNotSentItemsOnly=0) OR (qu.SentOn IS NULL)) AND
		(qu.SendTries < @MaxSendTries)
	ORDER BY qu.Priority desc, qu.CreatedOn ASC
	
	SET ROWCOUNT 0
END
GO


-- allow customer to enter price
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ProductVariant]') and NAME='CustomerEntersPrice')
BEGIN
	ALTER TABLE [dbo].[Nop_ProductVariant] 
	ADD CustomerEntersPrice bit NOT NULL CONSTRAINT [DF_Nop_ProductVariant_CustomerEntersPrice] DEFAULT ((0))
END
GO
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ProductVariant]') and NAME='MinimumCustomerEnteredPrice')
BEGIN
	ALTER TABLE [dbo].[Nop_ProductVariant] 
	ADD MinimumCustomerEnteredPrice money NOT NULL CONSTRAINT [DF_Nop_ProductVariant_MinimumCustomerEnteredPrice] DEFAULT ((0))
END
GO
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ProductVariant]') and NAME='MaximumCustomerEnteredPrice')
BEGIN
	ALTER TABLE [dbo].[Nop_ProductVariant] 
	ADD MaximumCustomerEnteredPrice money NOT NULL CONSTRAINT [DF_Nop_ProductVariant_MaximumCustomerEnteredPrice] DEFAULT ((1000))
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantLoadByPrimaryKey]
(
	@ProductVariantID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT	 
		pv.ProductVariantId, 
		pv.ProductID, 
		dbo.NOP_getnotnullnotempty(pvl.[Name],pv.[Name]) as [Name], 
		pv.SKU, 
		dbo.NOP_getnotnullnotempty(pvl.[Description],pv.[Description]) as [Description], 
		pv.AdminComment, 
		pv.ManufacturerPartNumber, 
		pv.IsGiftCard, 
		pv.IsDownload, 
		pv.DownloadID,                      
		pv.UnlimitedDownloads, 
		pv.MaxNumberOfDownloads, 
		pv.DownloadExpirationDays, 
		pv.DownloadActivationType, 
		pv.HasSampleDownload, 
		pv.SampleDownloadID,                       
		pv.HasUserAgreement, 
		pv.UserAgreementText, 
		pv.IsRecurring, 
		pv.CycleLength, 
		pv.CyclePeriod,
		pv.TotalCycles, 
		pv.IsShipEnabled, 
		pv.IsFreeShipping, 
		pv.AdditionalShippingCharge, 
		pv.IsTaxExempt, 
		pv.TaxCategoryID, 
		pv.ManageInventory, 
		pv.StockQuantity, 
		pv.DisplayStockAvailability, 
		pv.MinStockQuantity,                       
		pv.LowStockActivityID, 
		pv.NotifyAdminForQuantityBelow, 
		pv.AllowOutOfStockOrders, 
		pv.OrderMinimumQuantity, 
		pv.OrderMaximumQuantity, 
		pv.WarehouseID, 
		pv.DisableBuyButton, 
		pv.Price, 
		pv.OldPrice, 
		pv.ProductCost,
		pv.CustomerEntersPrice,
		pv.MinimumCustomerEnteredPrice,
		pv.MaximumCustomerEnteredPrice,
		pv.Weight, 
		pv.Length, 
		pv.Width, 
		pv.Height, 
		pv.PictureID, 
		pv.AvailableStartDateTime, 
		pv.AvailableEndDateTime, 
		pv.Published,                      
		pv.Deleted, 
		pv.DisplayOrder, 
		pv.CreatedOn, 
		pv.UpdatedOn
	FROM [Nop_ProductVariant] pv
		LEFT OUTER JOIN [Nop_ProductVariantLocalized] pvl 
		ON pvl.ProductVariantId = pv.ProductVariantId AND pvl.LanguageID = @LanguageID
	WHERE
		(pv.ProductVariantID = @ProductVariantID)
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantLoadByProductID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantLoadByProductID]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantLoadByProductID]
(
	@ProductID int,
	@LanguageID int,
	@ShowHidden bit = 0
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		pv.ProductVariantId, 
		pv.ProductID, 
		dbo.NOP_getnotnullnotempty(pvl.[Name],pv.[Name]) as [Name], 
		pv.SKU, 
		dbo.NOP_getnotnullnotempty(pvl.[Description],pv.[Description]) as [Description], 
		pv.AdminComment, 
		pv.ManufacturerPartNumber, 
		pv.IsGiftCard, 
		pv.IsDownload, 
		pv.DownloadID,                      
		pv.UnlimitedDownloads, 
		pv.MaxNumberOfDownloads, 
		pv.DownloadExpirationDays, 
		pv.DownloadActivationType, 
		pv.HasSampleDownload, 
		pv.SampleDownloadID,                       
		pv.HasUserAgreement, 
		pv.UserAgreementText, 
		pv.IsRecurring, 
		pv.CycleLength, 
		pv.CyclePeriod,
		pv.TotalCycles, 
		pv.IsShipEnabled, 
		pv.IsFreeShipping, 
		pv.AdditionalShippingCharge, 
		pv.IsTaxExempt, 
		pv.TaxCategoryID, 
		pv.ManageInventory, 
		pv.StockQuantity, 
		pv.DisplayStockAvailability, 
		pv.MinStockQuantity,                       
		pv.LowStockActivityID, 
		pv.NotifyAdminForQuantityBelow, 
		pv.AllowOutOfStockOrders, 
		pv.OrderMinimumQuantity, 
		pv.OrderMaximumQuantity, 
		pv.WarehouseID, 
		pv.DisableBuyButton, 
		pv.Price, 
		pv.OldPrice, 
		pv.ProductCost, 
		pv.CustomerEntersPrice,
		pv.MinimumCustomerEnteredPrice,
		pv.MaximumCustomerEnteredPrice,
		pv.Weight, 
		pv.Length, 
		pv.Width, 
		pv.Height, 
		pv.PictureID, 
		pv.AvailableStartDateTime, 
		pv.AvailableEndDateTime, 
		pv.Published,                      
		pv.Deleted, 
		pv.DisplayOrder, 
		pv.CreatedOn, 
		pv.UpdatedOn
	FROM [Nop_ProductVariant] pv
		LEFT OUTER JOIN [Nop_ProductVariantLocalized] pvl 
		ON pvl.ProductVariantId = pv.ProductVariantId AND pvl.LanguageID = @LanguageID
	WHERE 
			(@ShowHidden = 1 OR pv.Published = 1) 
		AND 
			pv.Deleted=0
		AND 
			pv.ProductID = @ProductID
		AND 
			(
				@ShowHidden = 1
				OR
				(getutcdate() between isnull(pv.AvailableStartDateTime, '1/1/1900') and isnull(pv.AvailableEndDateTime, '1/1/2999'))
			)
	order by pv.DisplayOrder
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantInsert]
(
    @ProductVariantID int = NULL output,
    @ProductId int,
    @Name nvarchar(400),
    @SKU nvarchar (400),
    @Description nvarchar(4000),
    @AdminComment nvarchar(4000),
    @ManufacturerPartNumber nvarchar(100),
	@IsGiftCard bit,
    @IsDownload bit,
    @DownloadID int,
	@UnlimitedDownloads bit,
	@MaxNumberOfDownloads int,
	@DownloadExpirationDays int,
	@DownloadActivationType int,
	@HasSampleDownload bit,
	@SampleDownloadID int,
	@HasUserAgreement bit,
	@UserAgreementText nvarchar(MAX),
    @IsRecurring bit,
    @CycleLength int,
    @CyclePeriod int,
    @TotalCycles int,
    @IsShipEnabled bit,
    @IsFreeShipping bit,
	@AdditionalShippingCharge money,
    @IsTaxExempt bit,
    @TaxCategoryID int,
	@ManageInventory int,
    @StockQuantity int,
	@DisplayStockAvailability bit,
    @MinStockQuantity int,
    @LowStockActivityID int,
	@NotifyAdminForQuantityBelow int,
	@AllowOutOfStockOrders bit,
	@OrderMinimumQuantity int,
	@OrderMaximumQuantity int,
    @WarehouseId int,
    @DisableBuyButton int,
    @Price money,
    @OldPrice money,
	@ProductCost money,
	@CustomerEntersPrice bit,
	@MinimumCustomerEnteredPrice money,
	@MaximumCustomerEnteredPrice money,
    @Weight float,
    @Length decimal(18, 4),
    @Width decimal(18, 4),
    @Height decimal(18, 4),
    @PictureID int,
	@AvailableStartDateTime datetime,
	@AvailableEndDateTime datetime,
    @Published bit,
    @Deleted bit,
    @DisplayOrder int,
	@CreatedOn datetime,
    @UpdatedOn datetime
)
AS
BEGIN
    INSERT
    INTO [Nop_ProductVariant]
    (
        ProductId,
        [Name],
        SKU,
        [Description],
        AdminComment,
        ManufacturerPartNumber,
		IsGiftCard,
        IsDownload,
        DownloadID,
		UnlimitedDownloads,
		MaxNumberOfDownloads,
		DownloadExpirationDays,
		DownloadActivationType,
		HasSampleDownload,
		SampleDownloadID,
		HasUserAgreement,
		UserAgreementText,
		IsRecurring,
		CycleLength,
		CyclePeriod,
		TotalCycles,
        IsShipEnabled,
        IsFreeShipping,
		AdditionalShippingCharge,
        IsTaxExempt,
        TaxCategoryID,
		ManageInventory,
		DisplayStockAvailability,
        StockQuantity,
        MinStockQuantity,
        LowStockActivityID,
		NotifyAdminForQuantityBelow,
		AllowOutOfStockOrders,
		OrderMinimumQuantity,
		OrderMaximumQuantity,
        WarehouseId,
        DisableBuyButton,
        Price,
        OldPrice,
		ProductCost,
		CustomerEntersPrice,
		MinimumCustomerEnteredPrice,
		MaximumCustomerEnteredPrice,
        Weight,
        [Length],
        Width,
        Height,
        PictureID,
		AvailableStartDateTime,
		AvailableEndDateTime,
        Published,
        Deleted,
        DisplayOrder,
        CreatedOn,
        UpdatedOn
    )
    VALUES
    (
        @ProductId,
        @Name,
        @SKU,
        @Description,
        @AdminComment,
        @ManufacturerPartNumber,
		@IsGiftCard,
        @IsDownload,
        @DownloadID,
		@UnlimitedDownloads,
		@MaxNumberOfDownloads,
		@DownloadExpirationDays,
		@DownloadActivationType,
		@HasSampleDownload,
		@SampleDownloadID,
		@HasUserAgreement,
		@UserAgreementText,
		@IsRecurring,
		@CycleLength,
		@CyclePeriod,
		@TotalCycles,
        @IsShipEnabled,
        @IsFreeShipping,
		@AdditionalShippingCharge,
        @IsTaxExempt,
        @TaxCategoryID,
		@ManageInventory,
		@DisplayStockAvailability,
        @StockQuantity,
        @MinStockQuantity,
        @LowStockActivityID,
		@NotifyAdminForQuantityBelow,
		@AllowOutOfStockOrders,
		@OrderMinimumQuantity,
		@OrderMaximumQuantity,
        @WarehouseId,
        @DisableBuyButton,
        @Price,
        @OldPrice,
		@ProductCost,
		@CustomerEntersPrice,
		@MinimumCustomerEnteredPrice,
		@MaximumCustomerEnteredPrice,
        @Weight,
        @Length,
        @Width,
        @Height,
        @PictureID,
		@AvailableStartDateTime,
		@AvailableEndDateTime,
        @Published,
        @Deleted,
        @DisplayOrder,
        @CreatedOn,
        @UpdatedOn
    )

    set @ProductVariantID=SCOPE_IDENTITY()
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantUpdate]
(
	@ProductVariantID int,
	@ProductId int,
	@Name nvarchar(400),
	@SKU nvarchar (400),
	@Description nvarchar(4000),
	@AdminComment nvarchar(4000),
	@ManufacturerPartNumber nvarchar(100),
	@IsGiftCard bit,
	@IsDownload bit,
	@DownloadID int,
	@UnlimitedDownloads bit,
	@MaxNumberOfDownloads int,
	@DownloadExpirationDays int,
	@DownloadActivationType int,
	@HasSampleDownload bit,
	@SampleDownloadID int,
	@HasUserAgreement bit,
	@UserAgreementText nvarchar(MAX),
    @IsRecurring bit,
    @CycleLength int,
    @CyclePeriod int,
    @TotalCycles int,
	@IsShipEnabled bit,
	@IsFreeShipping bit,
	@AdditionalShippingCharge money,
	@IsTaxExempt bit,
	@TaxCategoryID int,
	@ManageInventory int,
	@StockQuantity int,
	@DisplayStockAvailability bit,
	@MinStockQuantity int,
	@LowStockActivityID int,
	@NotifyAdminForQuantityBelow int,
	@AllowOutOfStockOrders bit,
	@OrderMinimumQuantity int,
	@OrderMaximumQuantity int,
	@WarehouseId int,
	@DisableBuyButton bit,
	@Price money,
	@OldPrice money,
	@ProductCost money,
	@CustomerEntersPrice bit,
	@MinimumCustomerEnteredPrice money,
	@MaximumCustomerEnteredPrice money,
	@Weight float,
	@Length decimal(18, 4),
	@Width decimal(18, 4),
	@Height decimal(18, 4),
	@PictureID int,
	@AvailableStartDateTime datetime,
	@AvailableEndDateTime datetime,
	@Published bit,
	@Deleted bit,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_ProductVariant]
	SET
		ProductId=@ProductId,
		[Name]=@Name,
		[SKU]=@SKU,
		[Description]=@Description,
		AdminComment=@AdminComment,
		ManufacturerPartNumber=@ManufacturerPartNumber,		
		IsGiftCard=@IsGiftCard,
		IsDownload=@IsDownload,
		DownloadID=@DownloadID,
		UnlimitedDownloads=@UnlimitedDownloads,
		MaxNumberOfDownloads=@MaxNumberOfDownloads,
		DownloadExpirationDays=@DownloadExpirationDays,
		DownloadActivationType=@DownloadActivationType,
		HasSampleDownload=@HasSampleDownload,
		SampleDownloadID=@SampleDownloadID,
		HasUserAgreement=@HasUserAgreement,
		UserAgreementText=@UserAgreementText,
		IsRecurring=@IsRecurring,
		CycleLength=@CycleLength,
		CyclePeriod=@CyclePeriod,
		TotalCycles=@TotalCycles,
		IsShipEnabled=@IsShipEnabled,
		IsFreeShipping=@IsFreeShipping,
		AdditionalShippingCharge=@AdditionalShippingCharge,
		IsTaxExempt=@IsTaxExempt,
		TaxCategoryID=@TaxCategoryID,
		ManageInventory=@ManageInventory,
		StockQuantity=@StockQuantity,
		DisplayStockAvailability=@DisplayStockAvailability,
		MinStockQuantity=@MinStockQuantity,
		LowStockActivityID=@LowStockActivityID,
		NotifyAdminForQuantityBelow=@NotifyAdminForQuantityBelow,
		AllowOutOfStockOrders=@AllowOutOfStockOrders,
		OrderMinimumQuantity=@OrderMinimumQuantity,
		OrderMaximumQuantity=@OrderMaximumQuantity,
		WarehouseId=@WarehouseId,
		DisableBuyButton=@DisableBuyButton,
		Price=@Price,
		OldPrice=@OldPrice,
		ProductCost=@ProductCost,
		CustomerEntersPrice=@CustomerEntersPrice,
		MinimumCustomerEnteredPrice=@MinimumCustomerEnteredPrice,
		MaximumCustomerEnteredPrice=@MaximumCustomerEnteredPrice,
		Weight=@Weight,
		[Length]=@Length,
		Width=@Width,
		Height=@Height,
		PictureID=@PictureID,
		AvailableStartDateTime=@AvailableStartDateTime,
		AvailableEndDateTime=@AvailableEndDateTime,
		Published=@Published,
		Deleted=@Deleted,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		ProductVariantID = @ProductVariantID
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ShoppingCartItem]') and NAME='CustomerEnteredPrice')
BEGIN
	ALTER TABLE [dbo].[Nop_ShoppingCartItem] 
	ADD CustomerEnteredPrice money NOT NULL CONSTRAINT [DF_Nop_ShoppingCartItem_CustomerEnteredPrice] DEFAULT ((0))
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ShoppingCartItemInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ShoppingCartItemInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ShoppingCartItemInsert]
(
	@ShoppingCartItemID int = NULL output,
	@ShoppingCartTypeID int,
	@CustomerSessionGUID uniqueidentifier,
	@ProductVariantID int,
	@AttributesXML XML,
	@CustomerEnteredPrice money,
	@Quantity int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_ShoppingCartItem]
	(
		ShoppingCartTypeID,
		CustomerSessionGUID,
		ProductVariantID,
		AttributesXML,
		CustomerEnteredPrice,
		Quantity,
		CreatedOn,
		UpdatedOn
	)
	VALUES
	(
		@ShoppingCartTypeID,
		@CustomerSessionGUID,
		@ProductVariantID,
		@AttributesXML,
		@CustomerEnteredPrice,
		@Quantity,
		@CreatedOn,
		@UpdatedOn
	)

	set @ShoppingCartItemID=SCOPE_IDENTITY()
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ShoppingCartItemUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ShoppingCartItemUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ShoppingCartItemUpdate]
(
	@ShoppingCartItemID int,
	@ShoppingCartTypeID int,
	@CustomerSessionGUID uniqueidentifier,
	@ProductVariantID int,
	@AttributesXML XML,
	@CustomerEnteredPrice money,
	@Quantity int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_ShoppingCartItem]
	SET
			ShoppingCartTypeID=@ShoppingCartTypeID,
			CustomerSessionGUID=@CustomerSessionGUID,
			ProductVariantID=@ProductVariantID,	
			AttributesXML=@AttributesXML,
			CustomerEnteredPrice=@CustomerEnteredPrice,
			Quantity=@Quantity,
			CreatedOn=@CreatedOn,
			UpdatedOn=@UpdatedOn
	WHERE
		ShoppingCartItemID = @ShoppingCartItemID
END
GO


-- Terms and conditions
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Checkout.TermsOfServiceEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Checkout.TermsOfServiceEnabled', N'false', N'')
END
GO



--customizable checkout attributes
if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_CheckoutAttribute]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Nop_CheckoutAttribute](
	[CheckoutAttributeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[TextPrompt] [nvarchar](300) NOT NULL,
	[IsRequired] bit NOT NULL,
	[ShippableProductRequired] bit NOT NULL,
	[IsTaxExempt] bit NOT NULL,
	[TaxCategoryID] int NOT NULL,
	[AttributeControlTypeID] int NOT NULL,
	[DisplayOrder] int NOT NULL,
 CONSTRAINT [PK_Nop_CheckoutAttribute] PRIMARY KEY CLUSTERED 
(
	[CheckoutAttributeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
) ON [PRIMARY]
END
GO




if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_CheckoutAttributeLocalized]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Nop_CheckoutAttributeLocalized](
	[CheckoutAttributeLocalizedID] [int] IDENTITY(1,1) NOT NULL,
	[CheckoutAttributeID] [int] NOT NULL,
	[LanguageID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[TextPrompt] [nvarchar](300) NOT NULL,
 CONSTRAINT [PK_Nop_CheckoutAttributeLocalized] PRIMARY KEY CLUSTERED 
(
	[CheckoutAttributeLocalizedID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [IX_Nop_CheckoutAttributeLocalized_Unique1] UNIQUE NONCLUSTERED 
(
	[CheckoutAttributeID] ASC,
	[LanguageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO




IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_CheckoutAttributeLocalized_Nop_CheckoutAttribute'
           AND parent_obj = Object_id('Nop_CheckoutAttributeLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_CheckoutAttributeLocalized
DROP CONSTRAINT FK_Nop_CheckoutAttributeLocalized_Nop_CheckoutAttribute
GO
ALTER TABLE [dbo].[Nop_CheckoutAttributeLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_CheckoutAttributeLocalized_Nop_CheckoutAttribute] FOREIGN KEY([CheckoutAttributeID])
REFERENCES [dbo].[Nop_CheckoutAttribute] ([CheckoutAttributeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_CheckoutAttributeLocalized_Nop_Language'
           AND parent_obj = Object_id('Nop_CheckoutAttributeLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_CheckoutAttributeLocalized
DROP CONSTRAINT FK_Nop_CheckoutAttributeLocalized_Nop_Language
GO
ALTER TABLE [dbo].[Nop_CheckoutAttributeLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_CheckoutAttributeLocalized_Nop_Language] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Nop_Language] ([LanguageID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO



if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_CheckoutAttributeValue]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Nop_CheckoutAttributeValue](
	[CheckoutAttributeValueID] [int] IDENTITY(1,1) NOT NULL,
	[CheckoutAttributeID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[PriceAdjustment] money NOT NULL,
	[WeightAdjustment] decimal(18, 4) NOT NULL,
	[IsPreSelected] bit NOT NULL,
	[DisplayOrder] int NOT NULL,
 CONSTRAINT [PK_Nop_CheckoutAttributeValue] PRIMARY KEY CLUSTERED 
(
	[CheckoutAttributeValueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
) ON [PRIMARY]
END
GO

IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_CheckoutAttributeValue_Nop_CheckoutAttribute'
           AND parent_obj = Object_id('Nop_CheckoutAttributeValue')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_CheckoutAttributeValue
DROP CONSTRAINT FK_Nop_CheckoutAttributeValue_Nop_CheckoutAttribute
GO
ALTER TABLE [dbo].[Nop_CheckoutAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_Nop_CheckoutAttributeValue_Nop_CheckoutAttribute] FOREIGN KEY([CheckoutAttributeID])
REFERENCES [dbo].[Nop_CheckoutAttribute] ([CheckoutAttributeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO



if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_CheckoutAttributeValueLocalized]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Nop_CheckoutAttributeValueLocalized](
	[CheckoutAttributeValueLocalizedID] [int] IDENTITY(1,1) NOT NULL,
	[CheckoutAttributeValueID] [int] NOT NULL,
	[LanguageID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Nop_CheckoutAttributeValueLocalized] PRIMARY KEY CLUSTERED 
(
	[CheckoutAttributeValueLocalizedID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [IX_Nop_CheckoutAttributeValueLocalized_Unique1] UNIQUE NONCLUSTERED 
(
	[CheckoutAttributeValueID] ASC,
	[LanguageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO



IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_CheckoutAttributeValueLocalized_Nop_CheckoutAttributeValue'
           AND parent_obj = Object_id('Nop_CheckoutAttributeValueLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_CheckoutAttributeValueLocalized
DROP CONSTRAINT FK_Nop_CheckoutAttributeValueLocalized_Nop_CheckoutAttributeValue
GO
ALTER TABLE [dbo].[Nop_CheckoutAttributeValueLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_CheckoutAttributeValueLocalized_Nop_CheckoutAttributeValue] FOREIGN KEY([CheckoutAttributeValueID])
REFERENCES [dbo].[Nop_CheckoutAttributeValue] ([CheckoutAttributeValueID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_CheckoutAttributeValueLocalized_Nop_Language'
           AND parent_obj = Object_id('Nop_CheckoutAttributeValueLocalized')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_CheckoutAttributeValueLocalized
DROP CONSTRAINT FK_Nop_CheckoutAttributeValueLocalized_Nop_Language
GO
ALTER TABLE [dbo].[Nop_CheckoutAttributeValueLocalized]  WITH CHECK ADD  CONSTRAINT [FK_Nop_CheckoutAttributeValueLocalized_Nop_Language] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Nop_Language] ([LanguageID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO





IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeDelete]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeDelete]
(
	@CheckoutAttributeID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_CheckoutAttribute]
	WHERE
		CheckoutAttributeID = @CheckoutAttributeID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeInsert]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeInsert]
(
	@CheckoutAttributeID int = NULL output,
	@Name nvarchar(100),
	@TextPrompt nvarchar(300),
	@IsRequired bit,
	@ShippableProductRequired bit,
	@IsTaxExempt bit,
	@TaxCategoryID int,
	@AttributeControlTypeID int,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_CheckoutAttribute]
	(
		[Name],
		[TextPrompt],
		[IsRequired],
		[ShippableProductRequired],
		[IsTaxExempt],
		[TaxCategoryID],
		[AttributeControlTypeID],
		[DisplayOrder]
	)
	VALUES
	(
		@Name,
		@TextPrompt,
		@IsRequired,
		@ShippableProductRequired,
		@IsTaxExempt,
		@TaxCategoryID,
		@AttributeControlTypeID,
		@DisplayOrder
	)

	set @CheckoutAttributeID=SCOPE_IDENTITY()
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeLoadAll]
(
	@LanguageID int,
	@DontLoadShippableProductRequired bit
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		ca.[CheckoutAttributeID],
		dbo.NOP_getnotnullnotempty(cal.Name,ca.Name) as [Name],
		dbo.NOP_getnotnullnotempty(cal.TextPrompt,ca.TextPrompt) as [TextPrompt],
		ca.[IsRequired],
		ca.[ShippableProductRequired],
		ca.[IsTaxExempt],
		ca.[TaxCategoryID],
		ca.[AttributeControlTypeID],
		ca.[DisplayOrder]		
	FROM [Nop_CheckoutAttribute] ca
		LEFT OUTER JOIN [Nop_CheckoutAttributeLocalized] cal
		ON ca.CheckoutAttributeID = cal.CheckoutAttributeID AND cal.LanguageID = @LanguageID	
	WHERE (@DontLoadShippableProductRequired=0 OR ca.[ShippableProductRequired]=0)
	order by ca.[DisplayOrder]
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeLoadByPrimaryKey]
(
	@CheckoutAttributeID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		ca.[CheckoutAttributeID],
		dbo.NOP_getnotnullnotempty(cal.Name,ca.Name) as [Name],
		dbo.NOP_getnotnullnotempty(cal.TextPrompt,ca.TextPrompt) as [TextPrompt],
		ca.[IsRequired],
		ca.[ShippableProductRequired],
		ca.[IsTaxExempt],
		ca.[TaxCategoryID],
		ca.[AttributeControlTypeID],
		ca.[DisplayOrder]		
	FROM [Nop_CheckoutAttribute] ca
		LEFT OUTER JOIN [Nop_CheckoutAttributeLocalized] cal
		ON ca.CheckoutAttributeID = cal.CheckoutAttributeID AND cal.LanguageID = @LanguageID
	WHERE
		ca.CheckoutAttributeID = @CheckoutAttributeID
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeUpdate]
(
	@CheckoutAttributeID int,
	@Name nvarchar(100),
	@TextPrompt nvarchar(300),
	@IsRequired bit,
	@ShippableProductRequired bit,
	@IsTaxExempt bit,
	@TaxCategoryID int,
	@AttributeControlTypeID int,
	@DisplayOrder int
)
AS
BEGIN

	UPDATE [Nop_CheckoutAttribute]
	SET
		[Name]=@Name,
		[TextPrompt]=@TextPrompt,
		[IsRequired]=@IsRequired,
		[ShippableProductRequired]=@ShippableProductRequired,
		[IsTaxExempt]=@IsTaxExempt,
		[TaxCategoryID]=@TaxCategoryID,
		[AttributeControlTypeID]=@AttributeControlTypeID,
		[DisplayOrder]=@DisplayOrder
	WHERE
		CheckoutAttributeID = @CheckoutAttributeID
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeLocalizedCleanUp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeLocalizedCleanUp]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_CheckoutAttributeLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '') AND		
		([TextPrompt] IS NULL OR [TextPrompt] = '')
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeLocalizedInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeLocalizedInsert]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeLocalizedInsert]
(
	@CheckoutAttributeLocalizedID int = NULL output,
	@CheckoutAttributeID int,
	@LanguageID int,
	@Name nvarchar(100),
	@TextPrompt nvarchar(300)
)
AS
BEGIN
	INSERT
	INTO [Nop_CheckoutAttributeLocalized]
	(
		CheckoutAttributeID,
		LanguageID,
		[Name],
		[TextPrompt]
	)
	VALUES
	(
		@CheckoutAttributeID,
		@LanguageID,
		@Name,
		@TextPrompt
	)

	set @CheckoutAttributeLocalizedID=@@identity

	EXEC Nop_CheckoutAttributeLocalizedCleanUp
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeLocalizedLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeLocalizedLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeLocalizedLoadByPrimaryKey]
	@CheckoutAttributeLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_CheckoutAttributeLocalized]
	WHERE CheckoutAttributeLocalizedID = @CheckoutAttributeLocalizedID
	ORDER BY CheckoutAttributeLocalizedID
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeLocalizedLoadByCheckoutAttributeIDAndLanguageID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeLocalizedLoadByCheckoutAttributeIDAndLanguageID]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeLocalizedLoadByCheckoutAttributeIDAndLanguageID]
	@CheckoutAttributeID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_CheckoutAttributeLocalized]
	WHERE CheckoutAttributeID = @CheckoutAttributeID AND LanguageID=@LanguageID
	ORDER BY CheckoutAttributeLocalizedID
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeLocalizedUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeLocalizedUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeLocalizedUpdate]
(
	@CheckoutAttributeLocalizedID int,
	@CheckoutAttributeID int,
	@LanguageID int,
	@Name nvarchar(100),
	@TextPrompt nvarchar(300)
)
AS
BEGIN
	
	UPDATE [Nop_CheckoutAttributeLocalized]
	SET
		[CheckoutAttributeID]=@CheckoutAttributeID,
		[LanguageID]=@LanguageID,
		[Name]=@Name,
		[TextPrompt]=@TextPrompt
	WHERE
		CheckoutAttributeLocalizedID = @CheckoutAttributeLocalizedID

	EXEC Nop_CheckoutAttributeLocalizedCleanUp
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeValueDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeValueDelete]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueDelete]
(
	@CheckoutAttributeValueID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_CheckoutAttributeValue]
	WHERE
		CheckoutAttributeValueID = @CheckoutAttributeValueID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeValueInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeValueInsert]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueInsert]
(
	@CheckoutAttributeValueID int = NULL output,
	@CheckoutAttributeID int,
	@Name nvarchar (100),
	@PriceAdjustment money,
	@WeightAdjustment decimal(18, 4),
	@IsPreSelected bit,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_CheckoutAttributeValue]
	(
		[CheckoutAttributeID],
		[Name],
		[PriceAdjustment],
		[WeightAdjustment],
		[IsPreSelected],
		[DisplayOrder]
	)
	VALUES
	(
		@CheckoutAttributeID,
		@Name,
		@PriceAdjustment,
		@WeightAdjustment,
		@IsPreSelected,
		@DisplayOrder
	)

	set @CheckoutAttributeValueID=SCOPE_IDENTITY()
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeValueLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeValueLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueLoadByPrimaryKey]
(
	@CheckoutAttributeValueID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		cav.CheckoutAttributeValueID, 
		cav.CheckoutAttributeID, 
		dbo.NOP_getnotnullnotempty(cavl.Name,cav.Name) as [Name],
		cav.PriceAdjustment, 
		cav.WeightAdjustment, 
		cav.IsPreSelected, 
		cav.DisplayOrder
	FROM [Nop_CheckoutAttributeValue] cav
		LEFT OUTER JOIN [Nop_CheckoutAttributeValueLocalized] cavl 
		ON cav.CheckoutAttributeValueID = cavl.CheckoutAttributeValueID AND cavl.LanguageID = @LanguageID	
	WHERE
		cav.CheckoutAttributeValueID = @CheckoutAttributeValueID
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeValueLoadByCheckoutAttributeID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeValueLoadByCheckoutAttributeID]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueLoadByCheckoutAttributeID]
(
	@CheckoutAttributeID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		cav.CheckoutAttributeValueID, 
		cav.CheckoutAttributeID, 
		dbo.NOP_getnotnullnotempty(cavl.Name,cav.Name) as [Name],
		cav.PriceAdjustment, 
		cav.WeightAdjustment, 
		cav.IsPreSelected, 
		cav.DisplayOrder
	FROM [Nop_CheckoutAttributeValue] cav
		LEFT OUTER JOIN [Nop_CheckoutAttributeValueLocalized] cavl 
		ON cav.CheckoutAttributeValueID = cavl.CheckoutAttributeValueID AND cavl.LanguageID = @LanguageID	
	WHERE 
		cav.CheckoutAttributeID=@CheckoutAttributeID
	ORDER BY cav.[DisplayOrder]
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeValueUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeValueUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueUpdate]
(
	@CheckoutAttributeValueID int,
	@CheckoutAttributeID int,
	@Name nvarchar (100),
	@PriceAdjustment money,
	@WeightAdjustment decimal(18, 4),
	@IsPreSelected bit,
	@DisplayOrder int
)
AS
BEGIN

	UPDATE [Nop_CheckoutAttributeValue]
	SET
		CheckoutAttributeID=@CheckoutAttributeID,
		[Name]=@Name,
		PriceAdjustment=@PriceAdjustment,
		WeightAdjustment=@WeightAdjustment,
		IsPreSelected=@IsPreSelected,
		DisplayOrder=@DisplayOrder
	WHERE
		CheckoutAttributeValueID = @CheckoutAttributeValueID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeValueLocalizedCleanUp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeValueLocalizedCleanUp]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueLocalizedCleanUp]

AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM
		[Nop_CheckoutAttributeValueLocalized]
	WHERE
		([Name] IS NULL OR [Name] = '')
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeValueLocalizedInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeValueLocalizedInsert]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueLocalizedInsert]
(
	@CheckoutAttributeValueLocalizedID int = NULL output,
	@CheckoutAttributeValueID int,
	@LanguageID int,
	@Name nvarchar(100)
)
AS
BEGIN
	INSERT
	INTO [Nop_CheckoutAttributeValueLocalized]
	(
		CheckoutAttributeValueID,
		LanguageID,
		[Name]
	)
	VALUES
	(
		@CheckoutAttributeValueID,
		@LanguageID,
		@Name
	)

	set @CheckoutAttributeValueLocalizedID=@@identity

	EXEC Nop_CheckoutAttributeValueLocalizedCleanUp
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeValueLocalizedLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeValueLocalizedLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueLocalizedLoadByPrimaryKey]
	@CheckoutAttributeValueLocalizedID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_CheckoutAttributeValueLocalized]
	WHERE CheckoutAttributeValueLocalizedID = @CheckoutAttributeValueLocalizedID
	ORDER BY CheckoutAttributeValueLocalizedID
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeValueLocalizedLoadByCheckoutAttributeValueIDAndLanguageID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeValueLocalizedLoadByCheckoutAttributeValueIDAndLanguageID]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueLocalizedLoadByCheckoutAttributeValueIDAndLanguageID]
	@CheckoutAttributeValueID int,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM [Nop_CheckoutAttributeValueLocalized]
	WHERE CheckoutAttributeValueID = @CheckoutAttributeValueID AND LanguageID=@LanguageID
	ORDER BY CheckoutAttributeValueLocalizedID
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CheckoutAttributeValueLocalizedUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CheckoutAttributeValueLocalizedUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_CheckoutAttributeValueLocalizedUpdate]
(
	@CheckoutAttributeValueLocalizedID int,
	@CheckoutAttributeValueID int,
	@LanguageID int,
	@Name nvarchar(100)
)
AS
BEGIN
	
	UPDATE [Nop_CheckoutAttributeValueLocalized]
	SET
		[CheckoutAttributeValueID]=@CheckoutAttributeValueID,
		[LanguageID]=@LanguageID,
		[Name]=@Name	
	WHERE
		CheckoutAttributeValueLocalizedID = @CheckoutAttributeValueLocalizedID

	EXEC Nop_CheckoutAttributeValueLocalizedCleanUp
END
GO


IF (NOT EXISTS(SELECT 1 FROM [Nop_ActivityLogType] WHERE [SystemKeyword] = N'DeleteCheckoutAttribute'))
BEGIN
	INSERT INTO [Nop_ActivityLogType] ([SystemKeyword], [Name], [Enabled]) VALUES (N'DeleteCheckoutAttribute', N'Delete a checkout attribute', 1)
END
GO

IF (NOT EXISTS(SELECT 1 FROM [Nop_ActivityLogType] WHERE [SystemKeyword] = N'EditCheckoutAttribute'))
BEGIN
	INSERT INTO [Nop_ActivityLogType] ([SystemKeyword], [Name], [Enabled]) VALUES (N'EditCheckoutAttribute', N'Edit a checkout attribute', 1)
END
GO

IF (NOT EXISTS(SELECT 1 FROM [Nop_ActivityLogType] WHERE [SystemKeyword] = N'AddNewCheckoutAttribute'))
BEGIN
	INSERT INTO [Nop_ActivityLogType] ([SystemKeyword], [Name], [Enabled]) VALUES (N'AddNewCheckoutAttribute', N'Add a new checkout attribute', 1)
END
GO

IF NOT EXISTS (SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Customer]') and NAME='CheckoutAttributes')
BEGIN
	ALTER TABLE [dbo].[Nop_Customer] 
	ADD CheckoutAttributes xml NOT NULL CONSTRAINT [DF_Nop_Customer_CheckoutAttributes] DEFAULT ((''))
END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerInsert]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerInsert]
(
	@CustomerId int = NULL output,
	@CustomerGUID uniqueidentifier,
	@Email nvarchar(255),
	@PasswordHash nvarchar(255),
	@SaltKey nvarchar(255),
	@AffiliateID int,
	@BillingAddressID int,
	@ShippingAddressID int,
	@LastPaymentMethodID int,
	@LastAppliedCouponCode nvarchar(100),
	@GiftCardCouponCodes xml,
	@CheckoutAttributes xml,
	@LanguageID int,
	@CurrencyID int,
	@TaxDisplayTypeID int,
	@IsTaxExempt bit,
	@IsAdmin bit,
	@IsGuest bit,
	@IsForumModerator bit,
	@TotalForumPosts int,
	@Signature nvarchar(300),
	@AdminComment nvarchar(4000),
	@Active bit,
	@Deleted bit,
	@RegistrationDate datetime,
	@TimeZoneID nvarchar(200),
	@Username nvarchar(100),
	@AvatarID int
)
AS
BEGIN
	INSERT
	INTO [Nop_Customer]
	(
		CustomerGUID,
		Email,
		PasswordHash,
		SaltKey,
		AffiliateID,
		BillingAddressID,
		ShippingAddressID,
		LastPaymentMethodID,
		LastAppliedCouponCode,
		GiftCardCouponCodes,
		CheckoutAttributes,
		LanguageID,
		CurrencyID,
		TaxDisplayTypeID,
		IsTaxExempt,
		IsAdmin,
		IsGuest,
		IsForumModerator,
		TotalForumPosts,
		[Signature],
		AdminComment,
		Active,
		Deleted,
		RegistrationDate,
		TimeZoneID,
		Username,
		AvatarID
	)
	VALUES
	(
		@CustomerGUID,
		@Email,
		@PasswordHash,
		@SaltKey,
		@AffiliateID,
		@BillingAddressID,
		@ShippingAddressID,
		@LastPaymentMethodID,
		@LastAppliedCouponCode,
		@GiftCardCouponCodes,
		@CheckoutAttributes,
		@LanguageID,
		@CurrencyID,
		@TaxDisplayTypeID,
		@IsTaxExempt,
		@IsAdmin,
		@IsGuest,
		@IsForumModerator,
		@TotalForumPosts,
		@Signature,
		@AdminComment,
		@Active,
		@Deleted,
		@RegistrationDate,
		@TimeZoneID,
		@Username,
		@AvatarID
	)

	set @CustomerId=SCOPE_IDENTITY()
END
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerUpdate]
(
	@CustomerId int,
	@CustomerGUID uniqueidentifier,
	@Email nvarchar(255),
	@PasswordHash nvarchar(255),
	@SaltKey nvarchar(255),
	@AffiliateID int,
	@BillingAddressID int,
	@ShippingAddressID int,
	@LastPaymentMethodID int,
	@LastAppliedCouponCode nvarchar(100),
	@GiftCardCouponCodes xml,
	@CheckoutAttributes xml,
	@LanguageID int,
	@CurrencyID int,
	@TaxDisplayTypeID int,
	@IsTaxExempt bit,
	@IsAdmin bit,
	@IsGuest bit,
	@IsForumModerator bit,
	@TotalForumPosts int,
	@Signature nvarchar(300),
	@AdminComment nvarchar(4000),
	@Active bit,
	@Deleted bit,
	@RegistrationDate datetime,
	@TimeZoneID nvarchar(200),
	@Username nvarchar(100),
	@AvatarID int
)
AS
BEGIN

	UPDATE [Nop_Customer]
	SET
		CustomerGUID=@CustomerGUID,
		Email=@Email,
		PasswordHash=@PasswordHash,
		SaltKey=@SaltKey,
		AffiliateID=@AffiliateID,
		BillingAddressID=@BillingAddressID,
		ShippingAddressID=@ShippingAddressID,
		LastPaymentMethodID=@LastPaymentMethodID,
		LastAppliedCouponCode=@LastAppliedCouponCode,
		GiftCardCouponCodes=@GiftCardCouponCodes,
		CheckoutAttributes=@CheckoutAttributes,
		LanguageID=@LanguageID,
		CurrencyID=@CurrencyID,
		TaxDisplayTypeID=@TaxDisplayTypeID,
		IsTaxExempt=@IsTaxExempt,
		IsAdmin=@IsAdmin,
		IsGuest=@IsGuest,
		IsForumModerator=@IsForumModerator,
		TotalForumPosts=@TotalForumPosts,
		[Signature]=@Signature,
		AdminComment=@AdminComment,
		Active=@Active,
		Deleted=@Deleted,
		RegistrationDate=@RegistrationDate,
		TimeZoneID=@TimeZoneID,
		Username=@Username,
		AvatarID=@AvatarID
	WHERE
		[CustomerId] = @CustomerId

END
GO

IF NOT EXISTS (SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='CheckoutAttributeDescription')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD CheckoutAttributeDescription nvarchar(4000) NOT NULL CONSTRAINT [DF_Nop_Order_CheckoutAttributeDescription] DEFAULT ((''))
END
GO

IF NOT EXISTS (SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='CheckoutAttributesXML')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD CheckoutAttributesXML xml NOT NULL CONSTRAINT [DF_Nop_Order_CheckoutAttributesXML] DEFAULT ((''))
END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderInsert]
GO
CREATE PROCEDURE [dbo].[Nop_OrderInsert]
(
	@OrderID int = NULL output,
	@OrderGUID uniqueidentifier,
	@CustomerID int,
	@CustomerLanguageID int,
	@CustomerTaxDisplayTypeID int,
	@CustomerIP nvarchar(50),
	@OrderSubtotalInclTax money,
	@OrderSubtotalExclTax money,
	@OrderShippingInclTax money,
	@OrderShippingExclTax money,
	@PaymentMethodAdditionalFeeInclTax money,
	@PaymentMethodAdditionalFeeExclTax money,
	@OrderTax money,
	@OrderTotal money,
	@OrderDiscount money,
	@OrderSubtotalInclTaxInCustomerCurrency money,
	@OrderSubtotalExclTaxInCustomerCurrency money,
	@OrderShippingInclTaxInCustomerCurrency money,
	@OrderShippingExclTaxInCustomerCurrency money,
	@PaymentMethodAdditionalFeeInclTaxInCustomerCurrency money,
	@PaymentMethodAdditionalFeeExclTaxInCustomerCurrency money,
	@OrderTaxInCustomerCurrency money,
	@OrderTotalInCustomerCurrency money,
	@OrderDiscountInCustomerCurrency money,
	@CheckoutAttributeDescription nvarchar(4000),
	@CheckoutAttributesXML xml,
	@CustomerCurrencyCode nvarchar(5),
	@OrderWeight float,
	@AffiliateID int,
	@OrderStatusID int,
	@AllowStoringCreditCardNumber bit,
	@CardType nvarchar(100),
	@CardName nvarchar(100),
	@CardNumber nvarchar(100),
	@MaskedCreditCardNumber nvarchar(100),
	@CardCVV2 nvarchar(100),
	@CardExpirationMonth nvarchar(100),
	@CardExpirationYear nvarchar(100),
	@PaymentMethodID int,
	@PaymentMethodName nvarchar(100),
	@AuthorizationTransactionID nvarchar(4000),
	@AuthorizationTransactionCode nvarchar(4000),
	@AuthorizationTransactionResult nvarchar(1000),
	@CaptureTransactionID nvarchar(4000),
	@CaptureTransactionResult nvarchar(1000),
	@SubscriptionTransactionID nvarchar(4000),
	@PurchaseOrderNumber nvarchar(100),
	@PaymentStatusID int,
	@PaidDate datetime,
	@BillingFirstName nvarchar(100),
	@BillingLastName nvarchar(100),
	@BillingPhoneNumber nvarchar(50),
	@BillingEmail nvarchar(255),
	@BillingFaxNumber nvarchar(50),
	@BillingCompany nvarchar(100),
	@BillingAddress1 nvarchar(100),
	@BillingAddress2 nvarchar(100),
	@BillingCity nvarchar(100),
	@BillingStateProvince nvarchar(100),
	@BillingStateProvinceID int,
	@BillingZipPostalCode nvarchar(10),
	@BillingCountry nvarchar(100),
	@BillingCountryID int,
	@ShippingStatusID int,
	@ShippingFirstName nvarchar(100),
	@ShippingLastName nvarchar(100),
	@ShippingPhoneNumber nvarchar(50),
	@ShippingEmail nvarchar(255),
	@ShippingFaxNumber nvarchar(50),
	@ShippingCompany nvarchar(100),
	@ShippingAddress1 nvarchar(100),
	@ShippingAddress2 nvarchar(100),
	@ShippingCity nvarchar(100),
	@ShippingStateProvince nvarchar(100),
	@ShippingStateProvinceID int,
	@ShippingZipPostalCode nvarchar(10),
	@ShippingCountry nvarchar(100),
	@ShippingCountryID int,
	@ShippingMethod nvarchar(100),
	@ShippingRateComputationMethodID int,
	@ShippedDate datetime,
	@TrackingNumber nvarchar(100),
	@Deleted bit,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Order]
	(
		OrderGUID,
		CustomerID,
		CustomerLanguageID,
		CustomerTaxDisplayTypeID,
		CustomerIP,
		OrderSubtotalInclTax,
		OrderSubtotalExclTax,
		OrderShippingInclTax,
		OrderShippingExclTax,
		PaymentMethodAdditionalFeeInclTax,
		PaymentMethodAdditionalFeeExclTax,
		OrderTax,
		OrderTotal,
		OrderDiscount,
		OrderSubtotalInclTaxInCustomerCurrency,
		OrderSubtotalExclTaxInCustomerCurrency,
		OrderShippingInclTaxInCustomerCurrency,
		OrderShippingExclTaxInCustomerCurrency,
		PaymentMethodAdditionalFeeInclTaxInCustomerCurrency,
		PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
		OrderTaxInCustomerCurrency,
		OrderTotalInCustomerCurrency,
		OrderDiscountInCustomerCurrency,
		CheckoutAttributeDescription,
		CheckoutAttributesXML,
		CustomerCurrencyCode,
		OrderWeight,
		AffiliateID,
		OrderStatusID,
		AllowStoringCreditCardNumber,
		CardType,
		CardName,
		CardNumber,
		MaskedCreditCardNumber,
		CardCVV2,
		CardExpirationMonth,
		CardExpirationYear,
		PaymentMethodID,
		PaymentMethodName,
		AuthorizationTransactionID,
		AuthorizationTransactionCode,
		AuthorizationTransactionResult,
		CaptureTransactionID,
		CaptureTransactionResult,
		SubscriptionTransactionID,
		PurchaseOrderNumber,
		PaymentStatusID,
		PaidDate,
		BillingFirstName,
		BillingLastName,
		BillingPhoneNumber,
		BillingEmail,
		BillingFaxNumber,
		BillingCompany,
		BillingAddress1,
		BillingAddress2,
		BillingCity,
		BillingStateProvince,
		BillingStateProvinceID,
		BillingZipPostalCode,
		BillingCountry,
		BillingCountryID,
		ShippingStatusID,
		ShippingFirstName,
		ShippingLastName,
		ShippingPhoneNumber,
		ShippingEmail,
		ShippingFaxNumber,
		ShippingCompany,
		ShippingAddress1,
		ShippingAddress2,
		ShippingCity,
		ShippingStateProvince,
		ShippingZipPostalCode,
		ShippingStateProvinceID,
		ShippingCountry,
		ShippingCountryID,
		ShippingMethod,
		ShippingRateComputationMethodID,
		ShippedDate,
		TrackingNumber,
		Deleted,
		CreatedOn
	)
	VALUES
	(
		@OrderGUID,
		@CustomerID,
		@CustomerLanguageID,		
		@CustomerTaxDisplayTypeID,
		@CustomerIP,
		@OrderSubtotalInclTax,
		@OrderSubtotalExclTax,		
		@OrderShippingInclTax,
		@OrderShippingExclTax,
		@PaymentMethodAdditionalFeeInclTax,
		@PaymentMethodAdditionalFeeExclTax,
		@OrderTax,
		@OrderTotal,
		@OrderDiscount,		
		@OrderSubtotalInclTaxInCustomerCurrency,
		@OrderSubtotalExclTaxInCustomerCurrency,		
		@OrderShippingInclTaxInCustomerCurrency,
		@OrderShippingExclTaxInCustomerCurrency,
		@PaymentMethodAdditionalFeeInclTaxInCustomerCurrency,
		@PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
		@OrderTaxInCustomerCurrency,
		@OrderTotalInCustomerCurrency,
		@OrderDiscountInCustomerCurrency,
		@CheckoutAttributeDescription,
		@CheckoutAttributesXML,
		@CustomerCurrencyCode,
		@OrderWeight,
		@AffiliateID,
		@OrderStatusID,
		@AllowStoringCreditCardNumber,
		@CardType,
		@CardName,
		@CardNumber,
		@MaskedCreditCardNumber,
		@CardCVV2,
		@CardExpirationMonth,
		@CardExpirationYear,
		@PaymentMethodID,
		@PaymentMethodName,
		@AuthorizationTransactionID,
		@AuthorizationTransactionCode,
		@AuthorizationTransactionResult,
		@CaptureTransactionID,
		@CaptureTransactionResult,
		@SubscriptionTransactionID,
		@PurchaseOrderNumber,
		@PaymentStatusID,
		@PaidDate,
		@BillingFirstName,
		@BillingLastName,
		@BillingPhoneNumber,
		@BillingEmail,
		@BillingFaxNumber,
		@BillingCompany,
		@BillingAddress1,
		@BillingAddress2,
		@BillingCity,
		@BillingStateProvince,
		@BillingStateProvinceID,
		@BillingZipPostalCode,
		@BillingCountry,
		@BillingCountryID,
		@ShippingStatusID,
		@ShippingFirstName,
		@ShippingLastName,
		@ShippingPhoneNumber,
		@ShippingEmail,
		@ShippingFaxNumber,
		@ShippingCompany,
		@ShippingAddress1,
		@ShippingAddress2,
		@ShippingCity,
		@ShippingStateProvince,
		@ShippingZipPostalCode,
		@ShippingStateProvinceID,
		@ShippingCountry,
		@ShippingCountryID,
		@ShippingMethod,
		@ShippingRateComputationMethodID,
		@ShippedDate,
		@TrackingNumber,
		@Deleted,
		@CreatedOn
	)

	set @OrderID=SCOPE_IDENTITY()
END
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_OrderUpdate]
(
	@OrderID int,
	@OrderGUID uniqueidentifier,
	@CustomerID int,
	@CustomerLanguageID int,
	@CustomerTaxDisplayTypeID int,
	@CustomerIP nvarchar(50),
	@OrderSubtotalInclTax money,
	@OrderSubtotalExclTax money,
	@OrderShippingInclTax money,
	@OrderShippingExclTax money,
	@PaymentMethodAdditionalFeeInclTax money,
	@PaymentMethodAdditionalFeeExclTax money,
	@OrderTax money,
	@OrderTotal money,
	@OrderDiscount money,
	@OrderSubtotalInclTaxInCustomerCurrency money,
	@OrderSubtotalExclTaxInCustomerCurrency money,
	@OrderShippingInclTaxInCustomerCurrency money,
	@OrderShippingExclTaxInCustomerCurrency money,
	@PaymentMethodAdditionalFeeInclTaxInCustomerCurrency money,
	@PaymentMethodAdditionalFeeExclTaxInCustomerCurrency money,
	@OrderTaxInCustomerCurrency money,
	@OrderTotalInCustomerCurrency money,
	@OrderDiscountInCustomerCurrency money,
	@CheckoutAttributeDescription nvarchar(4000),
	@CheckoutAttributesXML xml,
	@CustomerCurrencyCode nvarchar(5),
	@OrderWeight float,
	@AffiliateID int,
	@OrderStatusID int,
	@AllowStoringCreditCardNumber bit,
	@CardType nvarchar(100),
	@CardName nvarchar(100),
	@CardNumber nvarchar(100),
	@MaskedCreditCardNumber nvarchar(100),
	@CardCVV2 nvarchar(100),
	@CardExpirationMonth nvarchar(100),
	@CardExpirationYear nvarchar(100),
	@PaymentMethodID int,
	@PaymentMethodName nvarchar(100),
	@AuthorizationTransactionID nvarchar(4000),
	@AuthorizationTransactionCode nvarchar(4000),
	@AuthorizationTransactionResult nvarchar(1000),
	@CaptureTransactionID nvarchar(4000),
	@CaptureTransactionResult nvarchar(1000),
	@SubscriptionTransactionID nvarchar(4000),
	@PurchaseOrderNumber nvarchar(100),
	@PaymentStatusID int,
	@PaidDate datetime,
	@BillingFirstName nvarchar(100),
	@BillingLastName nvarchar(100),
	@BillingPhoneNumber nvarchar(50),
	@BillingEmail nvarchar(255),
	@BillingFaxNumber nvarchar(50),
	@BillingCompany nvarchar(100),
	@BillingAddress1 nvarchar(100),
	@BillingAddress2 nvarchar(100),
	@BillingCity nvarchar(100),
	@BillingStateProvince nvarchar(100),
	@BillingStateProvinceID int,
	@BillingZipPostalCode nvarchar(10),
	@BillingCountry nvarchar(100),
	@BillingCountryID int,
	@ShippingStatusID int,
	@ShippingFirstName nvarchar(100),
	@ShippingLastName nvarchar(100),
	@ShippingPhoneNumber nvarchar(50),
	@ShippingEmail nvarchar(255),
	@ShippingFaxNumber nvarchar(50),
	@ShippingCompany nvarchar(100),
	@ShippingAddress1 nvarchar(100),
	@ShippingAddress2 nvarchar(100),
	@ShippingCity nvarchar(100),
	@ShippingStateProvince nvarchar(100),
	@ShippingStateProvinceID int,
	@ShippingZipPostalCode nvarchar(10),
	@ShippingCountry nvarchar(100),
	@ShippingCountryID int,
	@ShippingMethod nvarchar(100),
	@ShippingRateComputationMethodID int,
	@ShippedDate datetime,
	@TrackingNumber nvarchar(100),
	@Deleted bit,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Order]
	SET
		OrderGUID=@OrderGUID,
		CustomerID=@CustomerID,
		CustomerLanguageID=@CustomerLanguageID,		
		CustomerTaxDisplayTypeID=@CustomerTaxDisplayTypeID,
		CustomerIP=@CustomerIP,
		OrderSubtotalInclTax=@OrderSubtotalInclTax,
		OrderSubtotalExclTax=@OrderSubtotalExclTax,		
		OrderShippingInclTax=@OrderShippingInclTax,
		OrderShippingExclTax=@OrderShippingExclTax,
		PaymentMethodAdditionalFeeInclTax=@PaymentMethodAdditionalFeeInclTax,
		PaymentMethodAdditionalFeeExclTax=@PaymentMethodAdditionalFeeExclTax,
		OrderTax=@OrderTax,
		OrderTotal=@OrderTotal,
		OrderDiscount=@OrderDiscount,
		OrderSubtotalInclTaxInCustomerCurrency=@OrderSubtotalInclTaxInCustomerCurrency,
		OrderSubtotalExclTaxInCustomerCurrency=@OrderSubtotalExclTaxInCustomerCurrency,
		OrderShippingInclTaxInCustomerCurrency=@OrderShippingInclTaxInCustomerCurrency,
		OrderShippingExclTaxInCustomerCurrency=@OrderShippingExclTaxInCustomerCurrency,	
		PaymentMethodAdditionalFeeInclTaxInCustomerCurrency=@PaymentMethodAdditionalFeeInclTaxInCustomerCurrency,
		PaymentMethodAdditionalFeeExclTaxInCustomerCurrency=@PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,	
		OrderTaxInCustomerCurrency=@OrderTaxInCustomerCurrency,
		OrderTotalInCustomerCurrency=@OrderTotalInCustomerCurrency,
		OrderDiscountInCustomerCurrency=@OrderDiscountInCustomerCurrency,
		CheckoutAttributeDescription=@CheckoutAttributeDescription,
		CheckoutAttributesXML=@CheckoutAttributesXML,
		CustomerCurrencyCode=@CustomerCurrencyCode,
		OrderWeight=@OrderWeight,
		AffiliateID=@AffiliateID,
		OrderStatusID=@OrderStatusID,
		AllowStoringCreditCardNumber=@AllowStoringCreditCardNumber,
		CardType=@CardType,
		CardName=@CardName,
		CardNumber=@CardNumber,
		MaskedCreditCardNumber=@MaskedCreditCardNumber,
		CardCVV2=@CardCVV2,
		CardExpirationMonth=@CardExpirationMonth,
		CardExpirationYear=@CardExpirationYear,
		PaymentMethodID=@PaymentMethodID,
		PaymentMethodName=@PaymentMethodName,
		AuthorizationTransactionID=@AuthorizationTransactionID,
		AuthorizationTransactionCode=@AuthorizationTransactionCode,
		AuthorizationTransactionResult=@AuthorizationTransactionResult,
		CaptureTransactionID=@CaptureTransactionID,
		CaptureTransactionResult=@CaptureTransactionResult,
		SubscriptionTransactionID=@SubscriptionTransactionID,
		PurchaseOrderNumber=@PurchaseOrderNumber,
		PaymentStatusID=@PaymentStatusID,
		PaidDate=@PaidDate,
		BillingFirstName=@BillingFirstName,
		BillingLastName=@BillingLastName,
		BillingPhoneNumber=@BillingPhoneNumber,
		BillingEmail=@BillingEmail,
		BillingFaxNumber=@BillingFaxNumber,
		BillingCompany=@BillingCompany,
		BillingAddress1=@BillingAddress1,
		BillingAddress2=@BillingAddress2,
		BillingCity=@BillingCity,
		BillingStateProvince=@BillingStateProvince,
		BillingStateProvinceID=@BillingStateProvinceID,
		BillingZipPostalCode=@BillingZipPostalCode,
		BillingCountry=@BillingCountry,
		BillingCountryID=@BillingCountryID,
		ShippingStatusID=@ShippingStatusID,
		ShippingFirstName=@ShippingFirstName,
		ShippingLastName=@ShippingLastName,
		ShippingPhoneNumber=@ShippingPhoneNumber,
		ShippingEmail=@ShippingEmail,
		ShippingFaxNumber=@ShippingFaxNumber,
		ShippingCompany=@ShippingCompany,
		ShippingAddress1=@ShippingAddress1,
		ShippingAddress2=@ShippingAddress2,
		ShippingCity=@ShippingCity,
		ShippingStateProvince=@ShippingStateProvince,
		ShippingStateProvinceID=@ShippingStateProvinceID,
		ShippingZipPostalCode=@ShippingZipPostalCode,
		ShippingCountry=@ShippingCountry,
		ShippingCountryID=@ShippingCountryID,
		ShippingMethod=@ShippingMethod,
		ShippingRateComputationMethodID=@ShippingRateComputationMethodID,
		ShippedDate=@ShippedDate,
		TrackingNumber=@TrackingNumber,
		Deleted=@Deleted,
		CreatedOn=@CreatedOn
	WHERE
		OrderID = @OrderID
END
GO


--Reward Points
if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_RewardPointsHistory]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[Nop_RewardPointsHistory](
	[RewardPointsHistoryID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[OrderID] [int] NOT NULL,
	[Points] [int] NOT NULL,
	[PointsBalance] [int] NOT NULL,
	[UsedAmount] [money] NOT NULL,
	[UsedAmountInCustomerCurrency] [money] NOT NULL,
	[CustomerCurrencyCode] [nvarchar](5) NOT NULL,
	[Message] [nvarchar](1000) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [Nop_RewardPointsHistory_PK] PRIMARY KEY CLUSTERED 
(
	[RewardPointsHistoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_RewardPointsHistory_Nop_Customer'
           AND parent_obj = Object_id('Nop_RewardPointsHistory')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_RewardPointsHistory
DROP CONSTRAINT FK_Nop_RewardPointsHistory_Nop_Customer
GO
ALTER TABLE [dbo].[Nop_RewardPointsHistory]  WITH CHECK ADD  CONSTRAINT [FK_Nop_RewardPointsHistory_Nop_Customer] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Nop_Customer] ([CustomerID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_RewardPointsHistoryDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_RewardPointsHistoryDelete]
GO
CREATE PROCEDURE [dbo].[Nop_RewardPointsHistoryDelete]
(
	@RewardPointsHistoryID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_RewardPointsHistory]
	WHERE
		RewardPointsHistoryID = @RewardPointsHistoryID
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_RewardPointsHistoryInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_RewardPointsHistoryInsert]
GO
CREATE PROCEDURE [dbo].[Nop_RewardPointsHistoryInsert]
(
	@RewardPointsHistoryID int = NULL output,
	@CustomerID int,
	@OrderID int,
	@Points int,
	@PointsBalance int,
	@UsedAmount money,
	@UsedAmountInCustomerCurrency money,
	@CustomerCurrencyCode nvarchar(5),
	@Message nvarchar(1000),
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_RewardPointsHistory]
	(
		[CustomerID],
		[OrderID],
		[Points],
		[PointsBalance],
		[UsedAmount],
		[UsedAmountInCustomerCurrency],
		[CustomerCurrencyCode],
		[Message],
		[CreatedOn]
	)
	VALUES
	(
		@CustomerID,
		@OrderID,
		@Points,
		@PointsBalance,
		@UsedAmount,
		@UsedAmountInCustomerCurrency,
		@CustomerCurrencyCode,
		@Message,
		@CreatedOn
	)

	set @RewardPointsHistoryID=SCOPE_IDENTITY()
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_RewardPointsHistoryLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_RewardPointsHistoryLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_RewardPointsHistoryLoadAll]
(
	@CustomerID int,
	@OrderID int,
	@PageIndex int = 0, 
	@PageSize int = 2147483644,
	@TotalRecords int = null OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		RewardPointsHistoryID int NOT NULL,
		CreatedOn datetime NOT NULL
	)

	INSERT INTO #PageIndex (RewardPointsHistoryID, CreatedOn)
	SELECT DISTINCT
		rph.RewardPointsHistoryID,
		rph.CreatedOn
	FROM [Nop_RewardPointsHistory] rph with (NOLOCK)
	WHERE 
		(
			@CustomerID IS NULL OR @CustomerID=0
			OR (rph.CustomerID=@CustomerID)
		)
		AND
		(
			@OrderID IS NULL OR @OrderID=0
			OR (rph.OrderID=@OrderID)
		)
	ORDER BY rph.CreatedOn DESC, RewardPointsHistoryID

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT
		rph.*
	FROM
		#PageIndex [pi]
		INNER JOIN [Nop_RewardPointsHistory] rph on rph.RewardPointsHistoryID = [pi].RewardPointsHistoryID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #PageIndex
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_RewardPointsHistoryLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_RewardPointsHistoryLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_RewardPointsHistoryLoadByPrimaryKey]
(
	@RewardPointsHistoryID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_RewardPointsHistory]
	WHERE
		RewardPointsHistoryID = @RewardPointsHistoryID
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_RewardPointsHistoryUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_RewardPointsHistoryUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_RewardPointsHistoryUpdate]
(
	@RewardPointsHistoryID int,
	@CustomerID int,
	@OrderID int,
	@Points int,
	@PointsBalance int,
	@UsedAmount money,
	@UsedAmountInCustomerCurrency money,
	@CustomerCurrencyCode nvarchar(5),
	@Message nvarchar(1000),
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_RewardPointsHistory]
	SET
		[CustomerID] = @CustomerID,
		[OrderID] = @OrderID,
		[Points] = @Points,
		[PointsBalance] = @PointsBalance,
		[UsedAmount] = @UsedAmount,
		[UsedAmountInCustomerCurrency] = @UsedAmountInCustomerCurrency,
		[CustomerCurrencyCode] = @CustomerCurrencyCode,
		[Message] = @Message,
		[CreatedOn] = @CreatedOn
	WHERE
		RewardPointsHistoryID=@RewardPointsHistoryID
END
GO

-- rename IsSenderNotified to IsRecipientNotified

IF EXISTS (SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_GiftCard]') and NAME='IsSenderNotified')
BEGIN
 ALTER TABLE [dbo].[Nop_GiftCard] ADD IsRecipientNotified BIT NOT NULL CONSTRAINT [DF_Nop_GiftCard_IsRecipientNotified] DEFAULT ((0))
 EXEC('UPDATE [dbo].[Nop_GiftCard] SET IsRecipientNotified=IsSenderNotified')
 ALTER TABLE [dbo].[Nop_GiftCard] DROP COLUMN IsSenderNotified
END
GO

-- SP [dbo].[Nop_GiftCardUpdate]

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_GiftCardUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_GiftCardUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_GiftCardUpdate]
(
	@GiftCardID int,
	@PurchasedOrderProductVariantID int,
	@Amount money,
	@IsGiftCardActivated bit,
	@GiftCardCouponCode nvarchar(100),
	@RecipientName nvarchar(100),
	@RecipientEmail nvarchar(100),
	@SenderName nvarchar(100),
	@SenderEmail nvarchar(100),
	@Message nvarchar(4000),
	@IsRecipientNotified bit,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_GiftCard]
	SET
		[PurchasedOrderProductVariantID] = @PurchasedOrderProductVariantID,
		[Amount] = @Amount,
		[IsGiftCardActivated] = @IsGiftCardActivated,
		[GiftCardCouponCode]= @GiftCardCouponCode,
		[RecipientName]=@RecipientName,
		[RecipientEmail]=@RecipientEmail,
		[SenderName]=@SenderName,
		[SenderEmail]=@SenderEmail,
		[Message]=@Message,
		[IsRecipientNotified]=@IsRecipientNotified,
		[CreatedOn] = @CreatedOn
	WHERE
		GiftCardID=@GiftCardID
END
GO

-- SP [dbo].[Nop_GiftCardInsert]

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_GiftCardInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_GiftCardInsert]
GO
CREATE PROCEDURE [dbo].[Nop_GiftCardInsert]
(
	@GiftCardID int = NULL output,
	@PurchasedOrderProductVariantID int,
	@Amount money,
	@IsGiftCardActivated bit,
	@GiftCardCouponCode nvarchar(100),
	@RecipientName nvarchar(100),
	@RecipientEmail nvarchar(100),
	@SenderName nvarchar(100),
	@SenderEmail nvarchar(100),
	@Message nvarchar(4000),
	@IsRecipientNotified bit,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_GiftCard]
	(
		[PurchasedOrderProductVariantID],
		[Amount],
		[IsGiftCardActivated],
		[GiftCardCouponCode],
		[RecipientName],
		[RecipientEmail],
		[SenderName],
		[SenderEmail],
		[Message],
		[IsRecipientNotified],
		[CreatedOn]
	)
	VALUES
	(
		@PurchasedOrderProductVariantID,
		@Amount,
		@IsGiftCardActivated,
		@GiftCardCouponCode,
		@RecipientName,
		@RecipientEmail,
		@SenderName,
		@SenderEmail,
		@Message,
		@IsRecipientNotified,
		@CreatedOn
	)

	set @GiftCardID=SCOPE_IDENTITY()
END
GO

--report
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerBestReport]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerBestReport]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerBestReport]
(
	@StartTime				datetime = NULL,
	@EndTime				datetime = NULL,
	@OrderStatusID			int,
	@PaymentStatusID		int,
	@ShippingStatusID		int,
	@OrderBy				int = 1 
)
AS
BEGIN

	SELECT TOP 20 c.CustomerID, SUM(o.OrderTotal) AS OrderTotal, COUNT(o.OrderID) AS OrderCount
	FROM [Nop_Customer] c
	INNER JOIN [Nop_Order] o
	ON c.CustomerID = o.CustomerID
	WHERE
		c.Deleted = 0 AND
		o.Deleted = 0 AND
		(@StartTime is NULL or @StartTime <= o.CreatedOn) AND
		(@EndTime is NULL or @EndTime >= o.CreatedOn) AND 
		(@OrderStatusID IS NULL or @OrderStatusID=0 or o.OrderStatusID = @OrderStatusID) AND
		(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o.PaymentStatusID = @PaymentStatusID) AND
		(@ShippingStatusID IS NULL OR @ShippingStatusID = 0 OR o.ShippingStatusID = @ShippingStatusID) --AND
	GROUP BY c.CustomerID
	ORDER BY case @OrderBy when 1 then SUM(o.OrderTotal) when 2 then COUNT(o.OrderID) else SUM(o.OrderTotal) end desc

END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerReportByLanguage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerReportByLanguage]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerReportByLanguage]
AS
BEGIN

	SELECT c.LanguageId, COUNT(c.LanguageId) as CustomerCount
	FROM [Nop_Customer] c
	WHERE
		c.Deleted = 0
	GROUP BY c.LanguageId
	ORDER BY CustomerCount desc

END
GO



IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[NOP_getcustomerattributevalue]') AND xtype in (N'FN', N'IF', N'TF'))
DROP FUNCTION [dbo].[NOP_getcustomerattributevalue]
GO
CREATE FUNCTION [dbo].[NOP_getcustomerattributevalue]
(
    @CustomerID int, 
    @AttributeKey nvarchar(100)
)
RETURNS nvarchar(1000)
AS
BEGIN
	
	DECLARE @AttributeValue nvarchar(1000)
	SET @AttributeValue = N''

	IF (EXISTS (SELECT ca.[Value] FROM [Nop_CustomerAttribute] ca 
				WHERE ca.CustomerID = @CustomerID AND
					  ca.[Key] = @AttributeKey))
	BEGIN
		SELECT @AttributeValue = ca.[Value] FROM [Nop_CustomerAttribute] ca 
				WHERE ca.CustomerID = @CustomerID AND
					  ca.[Key] = @AttributeKey	
	END
	  
	return @AttributeValue  
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerReportByAttributeKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerReportByAttributeKey]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerReportByAttributeKey]
(
	@CustomerAttributeKey nvarchar(100)
)
AS
BEGIN

	SELECT dbo.[NOP_getcustomerattributevalue] (c.CustomerId, @CustomerAttributeKey) as AttributeKey, 
		   Count(c.CustomerId) as CustomerCount
FROM [Nop_Customer] c
WHERE
	c.Deleted = 0
GROUP BY dbo.[NOP_getcustomerattributevalue] (c.CustomerId, @CustomerAttributeKey)
ORDER BY CustomerCount desc

END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderProductVariantReport]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderProductVariantReport]
GO
CREATE PROCEDURE [dbo].[Nop_OrderProductVariantReport]
(
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@OrderStatusID int,
	@PaymentStatusID int,
	@BillingCountryID int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT DISTINCT opv.ProductVariantID,
		(	
			select sum(opv2.PriceExclTax)
			from Nop_OrderProductVariant opv2
			INNER JOIN [Nop_Order] o2 
			on o2.OrderId = opv2.OrderID 
			where
				(@StartTime is NULL or @StartTime <= o2.CreatedOn) and
				(@EndTime is NULL or @EndTime >= o2.CreatedOn) and 
				(@OrderStatusID IS NULL or @OrderStatusID=0 or o2.OrderStatusID = @OrderStatusID) and
				(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o2.PaymentStatusID = @PaymentStatusID) and
				(@BillingCountryID IS NULL or @BillingCountryID=0 or o2.BillingCountryID = @BillingCountryID) and
				(o2.Deleted=0) and 
				(opv2.ProductVariantID = opv.ProductVariantID)) PriceExclTax, 
		(
			select sum(opv2.Quantity)  
			from Nop_OrderProductVariant opv2 
			INNER JOIN [Nop_Order] o2 
			on o2.OrderId = opv2.OrderID 
			where
				(@StartTime is NULL or @StartTime <= o2.CreatedOn) and
				(@EndTime is NULL or @EndTime >= o2.CreatedOn) and 
				(@OrderStatusID IS NULL or @OrderStatusID=0 or o2.OrderStatusID = @OrderStatusID) and
				(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o2.PaymentStatusID = @PaymentStatusID) and
				(@BillingCountryID IS NULL or @BillingCountryID=0 or o2.BillingCountryID = @BillingCountryID) and
				(o2.Deleted=0) and 
				(opv2.ProductVariantID = opv.ProductVariantID)) Total 
	FROM Nop_OrderProductVariant opv 
	INNER JOIN [Nop_Order] o 
	on o.OrderId = opv.OrderID
	WHERE
		(@StartTime is NULL or @StartTime <= o.CreatedOn) and
		(@EndTime is NULL or @EndTime >= o.CreatedOn) and 
		(@OrderStatusID IS NULL or @OrderStatusID=0 or o.OrderStatusID = @OrderStatusID) and
		(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o.PaymentStatusID = @PaymentStatusID) and
		(@BillingCountryID IS NULL or @BillingCountryID=0 or o.BillingCountryID = @BillingCountryID) and
		(o.Deleted=0)

END
GO

--new shipping status
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_ShippingStatus]
		WHERE [ShippingStatusID] = 40)
BEGIN
	INSERT [dbo].[Nop_ShippingStatus] ([ShippingStatusID], [Name])
	VALUES (40, N'Delivered')

	exec('update [Nop_Order] set ShippingStatusID=40 where ShippingStatusID=30')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_MessageTemplate]
		WHERE [Name] = N'OrderDelivered.CustomerNotification')
BEGIN
	INSERT [dbo].[Nop_MessageTemplate] ([Name])
	VALUES (N'OrderDelivered.CustomerNotification')

	DECLARE @MessageTemplateID INT 
	SELECT @MessageTemplateID =	mt.MessageTemplateID FROM Nop_MessageTemplate mt
							WHERE mt.Name = N'OrderDelivered.CustomerNotification' 

	IF (@MessageTemplateID > 0)
	BEGIN
		INSERT [dbo].[Nop_MessageTemplateLocalized] ([MessageTemplateID], [LanguageID], [BCCEmailAddresses], [Subject], [Body]) 
		VALUES (@MessageTemplateID, 7, N'Your order from %Store.Name% has been delivered.', N'',  N'<p><a href="%Store.URL%"> %Store.Name%</a> <br /> <br /> Hello %Order.CustomerFullName%, <br /> Good news! You order has been delivered. <br /> Order Number: %Order.OrderNumber%<br /> Order Details: <a href="%Order.OrderURLForCustomer%" target="_blank">%Order.OrderURLForCustomer%</a><br /> Date Ordered: %Order.CreatedOn%<br /> <br /> <br /> <br /> Billing Address<br /> %Order.BillingFirstName% %Order.BillingLastName%<br /> %Order.BillingAddress1%<br /> %Order.BillingCity% %Order.BillingZipPostalCode%<br /> %Order.BillingStateProvince% %Order.BillingCountry%<br /> <br /> <br /> <br /> Shipping Address<br /> %Order.ShippingFirstName% %Order.ShippingLastName%<br /> %Order.ShippingAddress1%<br /> %Order.ShippingCity% %Order.ShippingZipPostalCode%<br /> %Order.ShippingStateProvince% %Order.ShippingCountry%<br /> <br /> Shipping Method: %Order.ShippingMethod% <br /> <br /> %Order.Product(s)% </p>')
	END
END
GO


IF NOT EXISTS (SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='DeliveryDate')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD DeliveryDate datetime

	exec ('UPDATE [Nop_Order] SET DeliveryDate=ShippedDate')
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderInsert]
GO
CREATE PROCEDURE [dbo].[Nop_OrderInsert]
(
	@OrderID int = NULL output,
	@OrderGUID uniqueidentifier,
	@CustomerID int,
	@CustomerLanguageID int,
	@CustomerTaxDisplayTypeID int,
	@CustomerIP nvarchar(50),
	@OrderSubtotalInclTax money,
	@OrderSubtotalExclTax money,
	@OrderShippingInclTax money,
	@OrderShippingExclTax money,
	@PaymentMethodAdditionalFeeInclTax money,
	@PaymentMethodAdditionalFeeExclTax money,
	@OrderTax money,
	@OrderTotal money,
	@OrderDiscount money,
	@OrderSubtotalInclTaxInCustomerCurrency money,
	@OrderSubtotalExclTaxInCustomerCurrency money,
	@OrderShippingInclTaxInCustomerCurrency money,
	@OrderShippingExclTaxInCustomerCurrency money,
	@PaymentMethodAdditionalFeeInclTaxInCustomerCurrency money,
	@PaymentMethodAdditionalFeeExclTaxInCustomerCurrency money,
	@OrderTaxInCustomerCurrency money,
	@OrderTotalInCustomerCurrency money,
	@OrderDiscountInCustomerCurrency money,
	@CheckoutAttributeDescription nvarchar(4000),
	@CheckoutAttributesXML xml,
	@CustomerCurrencyCode nvarchar(5),
	@OrderWeight float,
	@AffiliateID int,
	@OrderStatusID int,
	@AllowStoringCreditCardNumber bit,
	@CardType nvarchar(100),
	@CardName nvarchar(100),
	@CardNumber nvarchar(100),
	@MaskedCreditCardNumber nvarchar(100),
	@CardCVV2 nvarchar(100),
	@CardExpirationMonth nvarchar(100),
	@CardExpirationYear nvarchar(100),
	@PaymentMethodID int,
	@PaymentMethodName nvarchar(100),
	@AuthorizationTransactionID nvarchar(4000),
	@AuthorizationTransactionCode nvarchar(4000),
	@AuthorizationTransactionResult nvarchar(1000),
	@CaptureTransactionID nvarchar(4000),
	@CaptureTransactionResult nvarchar(1000),
	@SubscriptionTransactionID nvarchar(4000),
	@PurchaseOrderNumber nvarchar(100),
	@PaymentStatusID int,
	@PaidDate datetime,
	@BillingFirstName nvarchar(100),
	@BillingLastName nvarchar(100),
	@BillingPhoneNumber nvarchar(50),
	@BillingEmail nvarchar(255),
	@BillingFaxNumber nvarchar(50),
	@BillingCompany nvarchar(100),
	@BillingAddress1 nvarchar(100),
	@BillingAddress2 nvarchar(100),
	@BillingCity nvarchar(100),
	@BillingStateProvince nvarchar(100),
	@BillingStateProvinceID int,
	@BillingZipPostalCode nvarchar(10),
	@BillingCountry nvarchar(100),
	@BillingCountryID int,
	@ShippingStatusID int,
	@ShippingFirstName nvarchar(100),
	@ShippingLastName nvarchar(100),
	@ShippingPhoneNumber nvarchar(50),
	@ShippingEmail nvarchar(255),
	@ShippingFaxNumber nvarchar(50),
	@ShippingCompany nvarchar(100),
	@ShippingAddress1 nvarchar(100),
	@ShippingAddress2 nvarchar(100),
	@ShippingCity nvarchar(100),
	@ShippingStateProvince nvarchar(100),
	@ShippingStateProvinceID int,
	@ShippingZipPostalCode nvarchar(10),
	@ShippingCountry nvarchar(100),
	@ShippingCountryID int,
	@ShippingMethod nvarchar(100),
	@ShippingRateComputationMethodID int,
	@ShippedDate datetime,
	@DeliveryDate datetime,
	@TrackingNumber nvarchar(100),
	@Deleted bit,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Order]
	(
		OrderGUID,
		CustomerID,
		CustomerLanguageID,
		CustomerTaxDisplayTypeID,
		CustomerIP,
		OrderSubtotalInclTax,
		OrderSubtotalExclTax,
		OrderShippingInclTax,
		OrderShippingExclTax,
		PaymentMethodAdditionalFeeInclTax,
		PaymentMethodAdditionalFeeExclTax,
		OrderTax,
		OrderTotal,
		OrderDiscount,
		OrderSubtotalInclTaxInCustomerCurrency,
		OrderSubtotalExclTaxInCustomerCurrency,
		OrderShippingInclTaxInCustomerCurrency,
		OrderShippingExclTaxInCustomerCurrency,
		PaymentMethodAdditionalFeeInclTaxInCustomerCurrency,
		PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
		OrderTaxInCustomerCurrency,
		OrderTotalInCustomerCurrency,
		OrderDiscountInCustomerCurrency,
		CheckoutAttributeDescription,
		CheckoutAttributesXML,
		CustomerCurrencyCode,
		OrderWeight,
		AffiliateID,
		OrderStatusID,
		AllowStoringCreditCardNumber,
		CardType,
		CardName,
		CardNumber,
		MaskedCreditCardNumber,
		CardCVV2,
		CardExpirationMonth,
		CardExpirationYear,
		PaymentMethodID,
		PaymentMethodName,
		AuthorizationTransactionID,
		AuthorizationTransactionCode,
		AuthorizationTransactionResult,
		CaptureTransactionID,
		CaptureTransactionResult,
		SubscriptionTransactionID,
		PurchaseOrderNumber,
		PaymentStatusID,
		PaidDate,
		BillingFirstName,
		BillingLastName,
		BillingPhoneNumber,
		BillingEmail,
		BillingFaxNumber,
		BillingCompany,
		BillingAddress1,
		BillingAddress2,
		BillingCity,
		BillingStateProvince,
		BillingStateProvinceID,
		BillingZipPostalCode,
		BillingCountry,
		BillingCountryID,
		ShippingStatusID,
		ShippingFirstName,
		ShippingLastName,
		ShippingPhoneNumber,
		ShippingEmail,
		ShippingFaxNumber,
		ShippingCompany,
		ShippingAddress1,
		ShippingAddress2,
		ShippingCity,
		ShippingStateProvince,
		ShippingZipPostalCode,
		ShippingStateProvinceID,
		ShippingCountry,
		ShippingCountryID,
		ShippingMethod,
		ShippingRateComputationMethodID,
		ShippedDate,
		DeliveryDate,
		TrackingNumber,
		Deleted,
		CreatedOn
	)
	VALUES
	(
		@OrderGUID,
		@CustomerID,
		@CustomerLanguageID,		
		@CustomerTaxDisplayTypeID,
		@CustomerIP,
		@OrderSubtotalInclTax,
		@OrderSubtotalExclTax,		
		@OrderShippingInclTax,
		@OrderShippingExclTax,
		@PaymentMethodAdditionalFeeInclTax,
		@PaymentMethodAdditionalFeeExclTax,
		@OrderTax,
		@OrderTotal,
		@OrderDiscount,		
		@OrderSubtotalInclTaxInCustomerCurrency,
		@OrderSubtotalExclTaxInCustomerCurrency,		
		@OrderShippingInclTaxInCustomerCurrency,
		@OrderShippingExclTaxInCustomerCurrency,
		@PaymentMethodAdditionalFeeInclTaxInCustomerCurrency,
		@PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
		@OrderTaxInCustomerCurrency,
		@OrderTotalInCustomerCurrency,
		@OrderDiscountInCustomerCurrency,
		@CheckoutAttributeDescription,
		@CheckoutAttributesXML,
		@CustomerCurrencyCode,
		@OrderWeight,
		@AffiliateID,
		@OrderStatusID,
		@AllowStoringCreditCardNumber,
		@CardType,
		@CardName,
		@CardNumber,
		@MaskedCreditCardNumber,
		@CardCVV2,
		@CardExpirationMonth,
		@CardExpirationYear,
		@PaymentMethodID,
		@PaymentMethodName,
		@AuthorizationTransactionID,
		@AuthorizationTransactionCode,
		@AuthorizationTransactionResult,
		@CaptureTransactionID,
		@CaptureTransactionResult,
		@SubscriptionTransactionID,
		@PurchaseOrderNumber,
		@PaymentStatusID,
		@PaidDate,
		@BillingFirstName,
		@BillingLastName,
		@BillingPhoneNumber,
		@BillingEmail,
		@BillingFaxNumber,
		@BillingCompany,
		@BillingAddress1,
		@BillingAddress2,
		@BillingCity,
		@BillingStateProvince,
		@BillingStateProvinceID,
		@BillingZipPostalCode,
		@BillingCountry,
		@BillingCountryID,
		@ShippingStatusID,
		@ShippingFirstName,
		@ShippingLastName,
		@ShippingPhoneNumber,
		@ShippingEmail,
		@ShippingFaxNumber,
		@ShippingCompany,
		@ShippingAddress1,
		@ShippingAddress2,
		@ShippingCity,
		@ShippingStateProvince,
		@ShippingZipPostalCode,
		@ShippingStateProvinceID,
		@ShippingCountry,
		@ShippingCountryID,
		@ShippingMethod,
		@ShippingRateComputationMethodID,
		@ShippedDate,
		@DeliveryDate,
		@TrackingNumber,
		@Deleted,
		@CreatedOn
	)

	set @OrderID=SCOPE_IDENTITY()
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_OrderUpdate]
(
	@OrderID int,
	@OrderGUID uniqueidentifier,
	@CustomerID int,
	@CustomerLanguageID int,
	@CustomerTaxDisplayTypeID int,
	@CustomerIP nvarchar(50),
	@OrderSubtotalInclTax money,
	@OrderSubtotalExclTax money,
	@OrderShippingInclTax money,
	@OrderShippingExclTax money,
	@PaymentMethodAdditionalFeeInclTax money,
	@PaymentMethodAdditionalFeeExclTax money,
	@OrderTax money,
	@OrderTotal money,
	@OrderDiscount money,
	@OrderSubtotalInclTaxInCustomerCurrency money,
	@OrderSubtotalExclTaxInCustomerCurrency money,
	@OrderShippingInclTaxInCustomerCurrency money,
	@OrderShippingExclTaxInCustomerCurrency money,
	@PaymentMethodAdditionalFeeInclTaxInCustomerCurrency money,
	@PaymentMethodAdditionalFeeExclTaxInCustomerCurrency money,
	@OrderTaxInCustomerCurrency money,
	@OrderTotalInCustomerCurrency money,
	@OrderDiscountInCustomerCurrency money,
	@CheckoutAttributeDescription nvarchar(4000),
	@CheckoutAttributesXML xml,
	@CustomerCurrencyCode nvarchar(5),
	@OrderWeight float,
	@AffiliateID int,
	@OrderStatusID int,
	@AllowStoringCreditCardNumber bit,
	@CardType nvarchar(100),
	@CardName nvarchar(100),
	@CardNumber nvarchar(100),
	@MaskedCreditCardNumber nvarchar(100),
	@CardCVV2 nvarchar(100),
	@CardExpirationMonth nvarchar(100),
	@CardExpirationYear nvarchar(100),
	@PaymentMethodID int,
	@PaymentMethodName nvarchar(100),
	@AuthorizationTransactionID nvarchar(4000),
	@AuthorizationTransactionCode nvarchar(4000),
	@AuthorizationTransactionResult nvarchar(1000),
	@CaptureTransactionID nvarchar(4000),
	@CaptureTransactionResult nvarchar(1000),
	@SubscriptionTransactionID nvarchar(4000),
	@PurchaseOrderNumber nvarchar(100),
	@PaymentStatusID int,
	@PaidDate datetime,
	@BillingFirstName nvarchar(100),
	@BillingLastName nvarchar(100),
	@BillingPhoneNumber nvarchar(50),
	@BillingEmail nvarchar(255),
	@BillingFaxNumber nvarchar(50),
	@BillingCompany nvarchar(100),
	@BillingAddress1 nvarchar(100),
	@BillingAddress2 nvarchar(100),
	@BillingCity nvarchar(100),
	@BillingStateProvince nvarchar(100),
	@BillingStateProvinceID int,
	@BillingZipPostalCode nvarchar(10),
	@BillingCountry nvarchar(100),
	@BillingCountryID int,
	@ShippingStatusID int,
	@ShippingFirstName nvarchar(100),
	@ShippingLastName nvarchar(100),
	@ShippingPhoneNumber nvarchar(50),
	@ShippingEmail nvarchar(255),
	@ShippingFaxNumber nvarchar(50),
	@ShippingCompany nvarchar(100),
	@ShippingAddress1 nvarchar(100),
	@ShippingAddress2 nvarchar(100),
	@ShippingCity nvarchar(100),
	@ShippingStateProvince nvarchar(100),
	@ShippingStateProvinceID int,
	@ShippingZipPostalCode nvarchar(10),
	@ShippingCountry nvarchar(100),
	@ShippingCountryID int,
	@ShippingMethod nvarchar(100),
	@ShippingRateComputationMethodID int,
	@ShippedDate datetime,
	@DeliveryDate datetime,
	@TrackingNumber nvarchar(100),
	@Deleted bit,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Order]
	SET
		OrderGUID=@OrderGUID,
		CustomerID=@CustomerID,
		CustomerLanguageID=@CustomerLanguageID,		
		CustomerTaxDisplayTypeID=@CustomerTaxDisplayTypeID,
		CustomerIP=@CustomerIP,
		OrderSubtotalInclTax=@OrderSubtotalInclTax,
		OrderSubtotalExclTax=@OrderSubtotalExclTax,		
		OrderShippingInclTax=@OrderShippingInclTax,
		OrderShippingExclTax=@OrderShippingExclTax,
		PaymentMethodAdditionalFeeInclTax=@PaymentMethodAdditionalFeeInclTax,
		PaymentMethodAdditionalFeeExclTax=@PaymentMethodAdditionalFeeExclTax,
		OrderTax=@OrderTax,
		OrderTotal=@OrderTotal,
		OrderDiscount=@OrderDiscount,
		OrderSubtotalInclTaxInCustomerCurrency=@OrderSubtotalInclTaxInCustomerCurrency,
		OrderSubtotalExclTaxInCustomerCurrency=@OrderSubtotalExclTaxInCustomerCurrency,
		OrderShippingInclTaxInCustomerCurrency=@OrderShippingInclTaxInCustomerCurrency,
		OrderShippingExclTaxInCustomerCurrency=@OrderShippingExclTaxInCustomerCurrency,	
		PaymentMethodAdditionalFeeInclTaxInCustomerCurrency=@PaymentMethodAdditionalFeeInclTaxInCustomerCurrency,
		PaymentMethodAdditionalFeeExclTaxInCustomerCurrency=@PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,	
		OrderTaxInCustomerCurrency=@OrderTaxInCustomerCurrency,
		OrderTotalInCustomerCurrency=@OrderTotalInCustomerCurrency,
		OrderDiscountInCustomerCurrency=@OrderDiscountInCustomerCurrency,
		CheckoutAttributeDescription=@CheckoutAttributeDescription,
		CheckoutAttributesXML=@CheckoutAttributesXML,
		CustomerCurrencyCode=@CustomerCurrencyCode,
		OrderWeight=@OrderWeight,
		AffiliateID=@AffiliateID,
		OrderStatusID=@OrderStatusID,
		AllowStoringCreditCardNumber=@AllowStoringCreditCardNumber,
		CardType=@CardType,
		CardName=@CardName,
		CardNumber=@CardNumber,
		MaskedCreditCardNumber=@MaskedCreditCardNumber,
		CardCVV2=@CardCVV2,
		CardExpirationMonth=@CardExpirationMonth,
		CardExpirationYear=@CardExpirationYear,
		PaymentMethodID=@PaymentMethodID,
		PaymentMethodName=@PaymentMethodName,
		AuthorizationTransactionID=@AuthorizationTransactionID,
		AuthorizationTransactionCode=@AuthorizationTransactionCode,
		AuthorizationTransactionResult=@AuthorizationTransactionResult,
		CaptureTransactionID=@CaptureTransactionID,
		CaptureTransactionResult=@CaptureTransactionResult,
		SubscriptionTransactionID=@SubscriptionTransactionID,
		PurchaseOrderNumber=@PurchaseOrderNumber,
		PaymentStatusID=@PaymentStatusID,
		PaidDate=@PaidDate,
		BillingFirstName=@BillingFirstName,
		BillingLastName=@BillingLastName,
		BillingPhoneNumber=@BillingPhoneNumber,
		BillingEmail=@BillingEmail,
		BillingFaxNumber=@BillingFaxNumber,
		BillingCompany=@BillingCompany,
		BillingAddress1=@BillingAddress1,
		BillingAddress2=@BillingAddress2,
		BillingCity=@BillingCity,
		BillingStateProvince=@BillingStateProvince,
		BillingStateProvinceID=@BillingStateProvinceID,
		BillingZipPostalCode=@BillingZipPostalCode,
		BillingCountry=@BillingCountry,
		BillingCountryID=@BillingCountryID,
		ShippingStatusID=@ShippingStatusID,
		ShippingFirstName=@ShippingFirstName,
		ShippingLastName=@ShippingLastName,
		ShippingPhoneNumber=@ShippingPhoneNumber,
		ShippingEmail=@ShippingEmail,
		ShippingFaxNumber=@ShippingFaxNumber,
		ShippingCompany=@ShippingCompany,
		ShippingAddress1=@ShippingAddress1,
		ShippingAddress2=@ShippingAddress2,
		ShippingCity=@ShippingCity,
		ShippingStateProvince=@ShippingStateProvince,
		ShippingStateProvinceID=@ShippingStateProvinceID,
		ShippingZipPostalCode=@ShippingZipPostalCode,
		ShippingCountry=@ShippingCountry,
		ShippingCountryID=@ShippingCountryID,
		ShippingMethod=@ShippingMethod,
		ShippingRateComputationMethodID=@ShippingRateComputationMethodID,
		ShippedDate=@ShippedDate,
		DeliveryDate=@DeliveryDate,
		TrackingNumber=@TrackingNumber,
		Deleted=@Deleted,
		CreatedOn=@CreatedOn
	WHERE
		OrderID = @OrderID
END
GO

--textbox attribute settings
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'ProductAttribute.Textbox.Width')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'ProductAttribute.Textbox.Width', N'300', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'ProductAttribute.MultiTextbox.Width')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'ProductAttribute.MultiTextbox.Width', N'300', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'ProductAttribute.MultiTextbox.Height')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'ProductAttribute.MultiTextbox.Height', N'150', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'CheckoutAttribute.Textbox.Width')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'CheckoutAttribute.Textbox.Width', N'300', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'CheckoutAttribute.MultiTextbox.Width')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'CheckoutAttribute.MultiTextbox.Width', N'300', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'CheckoutAttribute.MultiTextbox.Height')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'CheckoutAttribute.MultiTextbox.Height', N'150', N'')
END
GO

--bulk edit products
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantLoadAll]
(
	@CategoryID			int = 0,
	@ManufacturerID		int = 0,
	@Keywords			nvarchar(MAX),
	@ShowHidden			bit = 0,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	--init
	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = '%' + rtrim(ltrim(@Keywords)) + '%'

	--display order
	CREATE TABLE #DisplayOrder
	(
		ProductID int not null PRIMARY KEY,
		DisplayOrder int not null
	)	

	IF @CategoryID IS NOT NULL AND @CategoryID > 0
		BEGIN
			INSERT #DisplayOrder 
			SELECT pcm.ProductID, pcm.DisplayOrder 
			FROM [Nop_Product_Category_Mapping] pcm WHERE pcm.CategoryID = @CategoryID
		END
    ELSE IF @ManufacturerID IS NOT NULL AND @ManufacturerID > 0
		BEGIN
			INSERT #DisplayOrder 
			SELECT pmm.ProductID, pmm.Displayorder 
			FROM [Nop_Product_Manufacturer_Mapping] pmm WHERE pmm.ManufacturerID = @ManufacturerID
		END
	ELSE
		BEGIN
			INSERT #DisplayOrder 
			SELECT p.ProductID, 1 
			FROM [Nop_Product] p
			ORDER BY p.[Name]
		END
	
	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		ProductVariantID int NOT NULL,
		DisplayOrder int NOT NULL,
	)
	INSERT INTO #PageIndex (ProductVariantID, DisplayOrder)
	SELECT DISTINCT pv.ProductVariantID, do.DisplayOrder
	FROM Nop_Product p with (NOLOCK) 
	LEFT OUTER JOIN Nop_Product_Category_Mapping pcm with (NOLOCK) ON p.ProductID=pcm.ProductID
	LEFT OUTER JOIN Nop_Product_Manufacturer_Mapping pmm with (NOLOCK) ON p.ProductID=pmm.ProductID
	LEFT OUTER JOIN Nop_ProductVariant pv with (NOLOCK) ON p.ProductID = pv.ProductID
	JOIN #DisplayOrder do on p.ProductID = do.ProductID
	WHERE 
		(
			(
				@ShowHidden = 1 OR p.Published = 1
			)
		AND 
			(
				@ShowHidden = 1 OR pv.Published = 1
			)
		AND 
			(
				p.Deleted=0
			)
		AND (
				@CategoryID IS NULL OR @CategoryID=0
				OR pcm.CategoryID=@CategoryID
			)
		AND (
				@ManufacturerID IS NULL OR @ManufacturerID=0
				OR pmm.ManufacturerID=@ManufacturerID
			)
		AND	(
				-- search standard content
				patindex(@Keywords, isnull(p.name, '')) > 0
				or patindex(@Keywords, isnull(pv.name, '')) > 0
				or patindex(@Keywords, isnull(pv.sku , '')) > 0
			)
		AND
			(
				@ShowHidden = 1
				OR
				(getutcdate() between isnull(pv.AvailableStartDateTime, '1/1/1900') and isnull(pv.AvailableEndDateTime, '1/1/2999'))
			)
		)
	ORDER BY do.DisplayOrder

	--total records
	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	--return
	SELECT  
		pv.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_ProductVariant pv on pv.ProductVariantID = [pi].ProductVariantID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #DisplayOrder
	DROP TABLE #PageIndex
END
GO

--picture management changes
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_PictureLoadAllPaged]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_PictureLoadAllPaged]
GO
CREATE PROCEDURE [dbo].[Nop_PictureLoadAllPaged]
(
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		PictureID int NOT NULL		 
	)
	INSERT INTO #PageIndex (PictureID)
	SELECT
		PictureID
	FROM [Nop_Picture]
	ORDER BY PictureID DESC

	--total records
	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn

	SELECT [p].* FROM [Nop_Picture] [p]
		INNER JOIN #PageIndex [pi]
		ON [p].PictureID = [pi].PictureID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound

	SET ROWCOUNT 0
	
	DROP TABLE #PageIndex

END
GO

-- add ShowOnHomePage field to Nop_Category table

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Category]') and NAME='ShowOnHomePage')
BEGIN
	ALTER TABLE [dbo].[Nop_Category] 
	ADD ShowOnHomePage bit NOT NULL CONSTRAINT [DF_Nop_Category_ShowOnHomePage] DEFAULT ((0))
END
GO

-- SP Nop_CategoryInsert
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CategoryInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CategoryInsert]
GO
CREATE PROCEDURE [dbo].[Nop_CategoryInsert]
(
	@CategoryID int = NULL output,
	@Name nvarchar(400),
	@Description nvarchar(MAX),
	@TemplateID int,
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100),
	@ParentCategoryID int,	
	@PictureID int,
	@PageSize int,
	@PriceRanges nvarchar(400),
	@ShowOnHomePage bit,
	@Published bit,
	@Deleted bit,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Category]
	(
		[Name],
		[Description],
		TemplateID,
		MetaKeywords,
		MetaDescription,
		MetaTitle,
		SEName,
		ParentCategoryID,
		PictureID,
		PageSize,
		PriceRanges,
		ShowOnHomePage,
		Published,
		Deleted,
		DisplayOrder,
		CreatedOn,
		UpdatedOn
	)
	VALUES
	(
		@Name,
		@Description,
		@TemplateID,
		@MetaKeywords,
		@MetaDescription,
		@MetaTitle,
		@SEName,
		@ParentCategoryID,
		@PictureID,
		@PageSize,
		@PriceRanges,
		@ShowOnHomePage,
		@Published,
		@Deleted,
		@DisplayOrder,
		@CreatedOn,
		@UpdatedOn
	)

	set @CategoryID=SCOPE_IDENTITY()
END
GO

-- SP Nop_CategoryUpdate
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CategoryUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CategoryUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_CategoryUpdate]
(
	@CategoryID int,
	@Name nvarchar(400),
	@Description nvarchar(MAX),
	@TemplateID int,
	@MetaKeywords nvarchar(400),
	@MetaDescription nvarchar(4000),
	@MetaTitle nvarchar(400),
	@SEName nvarchar(100),
	@ParentCategoryID int,
	@PictureID int,
	@PageSize int,
	@PriceRanges nvarchar(400),
	@ShowOnHomePage bit,
	@Published bit,
	@Deleted bit,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Category]
	SET
		[Name]=@Name,
		[Description]=@Description,
		TemplateID=@TemplateID,
		MetaKeywords=@MetaKeywords,
		MetaDescription=@MetaDescription,
		MetaTitle=@MetaTitle,
		SEName=@SEName,
		ParentCategoryID=@ParentCategoryID,
		PictureID=@PictureID,
		PageSize=@PageSize,
		PriceRanges=@PriceRanges,
		ShowOnHomePage=@ShowOnHomePage,
		Published=@Published,
		Deleted=@Deleted,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		CategoryID = @CategoryID
END
GO

-- SP Nop_CategoryLoadAll
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CategoryLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CategoryLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_CategoryLoadAll]
	@ShowHidden bit = 0,
	@ParentCategoryID int = 0,
	@LanguageID int
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		c.CategoryID, 
		dbo.NOP_getnotnullnotempty(cl.Name,c.Name) as [Name],
		dbo.NOP_getnotnullnotempty(cl.Description,c.Description) as [Description],
		c.TemplateID, 
		dbo.NOP_getnotnullnotempty(cl.MetaKeywords,c.MetaKeywords) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(cl.MetaDescription,c.MetaDescription) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(cl.MetaTitle,c.MetaTitle) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(cl.SEName,c.SEName) as [SEName],
		c.ParentCategoryID, 
		c.PictureID, 
		c.PageSize, 
		c.PriceRanges,
		c.ShowOnHomePage, 
		c.Published,
		c.Deleted, 
		c.DisplayOrder, 
		c.CreatedOn, 
		c.UpdatedOn
	FROM [Nop_Category] c
		LEFT OUTER JOIN [Nop_CategoryLocalized] cl 
		ON c.CategoryID = cl.CategoryID AND cl.LanguageID = @LanguageID	
	WHERE 
		(c.Published = 1 or @ShowHidden = 1) AND 
		c.Deleted=0 AND 
		c.ParentCategoryID=@ParentCategoryID
	order by c.DisplayOrder
END
GO

-- SP Nop_CategoryLoadByPrimaryKey
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CategoryLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CategoryLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_CategoryLoadByPrimaryKey]
(
	@CategoryID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		c.CategoryID, 
		dbo.NOP_getnotnullnotempty(cl.Name,c.Name) as [Name],
		dbo.NOP_getnotnullnotempty(cl.Description,c.Description) as [Description],
		c.TemplateID, 
		dbo.NOP_getnotnullnotempty(cl.MetaKeywords,c.MetaKeywords) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(cl.MetaDescription,c.MetaDescription) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(cl.MetaTitle,c.MetaTitle) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(cl.SEName,c.SEName) as [SEName],
		c.ParentCategoryID, 
		c.PictureID, 
		c.PageSize, 
		c.PriceRanges,
		c.ShowOnHomePage, 
		c.Published,
		c.Deleted, 
		c.DisplayOrder,
		c.CreatedOn, 
		c.UpdatedOn
	FROM [Nop_Category] c
		LEFT OUTER JOIN [Nop_CategoryLocalized] cl 
		ON c.CategoryID = cl.CategoryID AND cl.LanguageID = @LanguageID	
	WHERE 
		(c.CategoryID = @CategoryID) 
END
GO

-- SP Nop_CategoryLoadDisplayedOnHomePage
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CategoryLoadDisplayedOnHomePage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CategoryLoadDisplayedOnHomePage]
GO
CREATE PROCEDURE [dbo].[Nop_CategoryLoadDisplayedOnHomePage]
(
	@ShowHidden		bit = 0,
	@LanguageID		int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		c.CategoryID, 
		dbo.NOP_getnotnullnotempty(cl.Name,c.Name) as [Name],
		dbo.NOP_getnotnullnotempty(cl.Description,c.Description) as [Description],
		c.TemplateID, 
		dbo.NOP_getnotnullnotempty(cl.MetaKeywords,c.MetaKeywords) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(cl.MetaDescription,c.MetaDescription) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(cl.MetaTitle,c.MetaTitle) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(cl.SEName,c.SEName) as [SEName],
		c.ParentCategoryID, 
		c.PictureID, 
		c.PageSize, 
		c.PriceRanges,
		c.ShowOnHomePage, 
		c.Published,
		c.Deleted, 
		c.DisplayOrder, 
		c.CreatedOn, 
		c.UpdatedOn
	FROM [Nop_Category] c
		LEFT OUTER JOIN [Nop_CategoryLocalized] cl 
		ON c.CategoryID = cl.CategoryID AND cl.LanguageID = @LanguageID	
	WHERE 
		(c.Published = 1 or @ShowHidden = 1) AND 
		c.Deleted=0 AND 
		c.ShowOnHomePage=1
	order by c.DisplayOrder
END
GO

-- remove Display.ShowCategoriesOnMainPage setting
IF EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Display.ShowCategoriesOnMainPage')
BEGIN
	DELETE FROM [dbo].[Nop_Setting] WHERE [Name] = N'Display.ShowCategoriesOnMainPage'
END
GO

-- add IPAddress field to Nop_NewsComment table
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_NewsComment]') and NAME='IPAddress')
BEGIN
	ALTER TABLE [dbo].[Nop_NewsComment] 
	ADD IPAddress nvarchar(100) NOT NULL CONSTRAINT [DF_Nop_NewsComment_IPAddress] DEFAULT ((''))
END
GO

-- SP Nop_NewsCommentInsert
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_NewsCommentInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_NewsCommentInsert]
GO
CREATE PROCEDURE [dbo].[Nop_NewsCommentInsert]
(
	@NewsCommentID int = NULL output,
	@NewsID int,
	@CustomerID int,
	@IPAddress nvarchar(100),
	@Title nvarchar(1000),
	@Comment nvarchar(max),
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_NewsComment]
	(
		NewsID,
		CustomerID,
		IPAddress,
		Title,
		Comment,
		CreatedOn
	)
	VALUES
	(
		@NewsID,
		@CustomerID,
		@IPAddress,
		@Title,
		@Comment,
		@CreatedOn
	)

	set @NewsCommentID=SCOPE_IDENTITY()
END
GO

-- SP Nop_NewsCommentUpdate
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_NewsCommentUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_NewsCommentUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_NewsCommentUpdate]
(
	@NewsCommentID int,
	@NewsID int,
	@CustomerID int,
	@IPAddress nvarchar(100),
	@Title nvarchar(1000),
	@Comment nvarchar(max),
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_NewsComment]
	SET
		NewsID=@NewsID,
		CustomerID=@CustomerID,
		IPAddress=@IPAddress,
		Title=@Title,
		Comment=@Comment,
		CreatedOn=@CreatedOn
	WHERE
		NewsCommentID = @NewsCommentID
END
GO

-- add IPAddress field to Nop_BlogComment table
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_BlogComment]') and NAME='IPAddress')
BEGIN
	ALTER TABLE [dbo].[Nop_BlogComment] 
	ADD IPAddress nvarchar(100) NOT NULL CONSTRAINT [DF_Nop_BlogComment_IPAddress] DEFAULT ((''))
END
GO

-- SP Nop_BlogCommentInsert
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_BlogCommentInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_BlogCommentInsert]
GO
CREATE PROCEDURE [dbo].[Nop_BlogCommentInsert]
(
	@BlogCommentID int = NULL output,
	@BlogPostID int,
	@CustomerID int,
	@IPAddress nvarchar(100),
	@CommentText nvarchar(MAX),
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_BlogComment]
	(
		BlogPostID,
		CustomerID,
		IPAddress,
		CommentText,
		CreatedOn
	)
	VALUES
	(
		@BlogPostID,
		@CustomerID,
		@IPAddress,
		@CommentText,
		@CreatedOn
	)

	set @BlogCommentID=SCOPE_IDENTITY()
END
GO

-- SP Nop_BlogCommentUpdate
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_BlogCommentUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_BlogCommentUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_BlogCommentUpdate]
(
	@BlogCommentID int,
	@BlogPostID int,
	@CustomerID int,
	@IPAddress nvarchar(100),
	@CommentText nvarchar(MAX),
	@CreatedOn datetime
)
AS
BEGIN

	UPDATE [Nop_BlogComment]
	SET
		BlogPostID=@BlogPostID,
		CustomerID=@CustomerID,
		IPAddress=@IPAddress,
		CommentText=@CommentText,
		CreatedOn=@CreatedOn
	WHERE
		BlogCommentID = @BlogCommentID
END
GO

-- add IPAddress field to Nop_ProductReview table
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ProductReview]') and NAME='IPAddress')
BEGIN
	ALTER TABLE [dbo].[Nop_ProductReview] 
	ADD IPAddress nvarchar(100) NOT NULL CONSTRAINT [DF_Nop_ProductReview_IPAddress] DEFAULT ((''))
END
GO

-- SP Nop_ProductReviewInsert
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductReviewInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductReviewInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ProductReviewInsert]
(
	@ProductReviewID int = NULL output,
	@ProductID int,
	@CustomerID int,
	@IPAddress nvarchar(100),
	@Title nvarchar(1000),
	@ReviewText nvarchar(max),
	@Rating int,
	@HelpfulYesTotal int,
	@HelpfulNoTotal int,
	@IsApproved bit,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductReview]
	(
		ProductID,
		CustomerID,
		IPAddress,
		Title,
		ReviewText,
		Rating,
		HelpfulYesTotal,
		HelpfulNoTotal,
		IsApproved,
		CreatedOn
	)
	VALUES
	(
		@ProductID,
		@CustomerID,
		@IPAddress,
		@Title,
		@ReviewText,
		@Rating,
		@HelpfulYesTotal,
		@HelpfulNoTotal,
		@IsApproved,
		@CreatedOn
	)

	set @ProductReviewID=SCOPE_IDENTITY()
END
GO

-- SP Nop_ProductReviewUpdate
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductReviewUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductReviewUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ProductReviewUpdate]
(
	@ProductReviewID int,
	@ProductID int,
	@CustomerID int,
	@IPAddress nvarchar(100),
	@Title nvarchar(1000),
	@ReviewText nvarchar(max),
	@Rating int,
	@HelpfulYesTotal int,
	@HelpfulNoTotal int,
	@IsApproved bit,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_ProductReview]
	SET
		ProductID=@ProductID,
		CustomerID=@CustomerID,
		IPAddress=@IPAddress,
		Title=@Title,
		ReviewText=@ReviewText,
		Rating=@Rating,
		HelpfulYesTotal=@HelpfulYesTotal,
		HelpfulNoTotal=@HelpfulNoTotal,
		IsApproved=@IsApproved,
		CreatedOn=@CreatedOn
	WHERE
		ProductReviewID = @ProductReviewID
END
GO

-- SP Nop_NewsLoadAll
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_NewsLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_NewsLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_NewsLoadAll]
(
	@LanguageID	int,
	@ShowHidden bit,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		NewsID int NOT NULL,
	)

	INSERT INTO #PageIndex (NewsID)
	SELECT
		n.NewsID
	FROM 
	    Nop_News n 
	WITH 
		(NOLOCK)
	WHERE
		(Published = 1 or @ShowHidden = 1)
		AND
		(@LanguageID IS NULL OR @LanguageID = 0 OR LanguageID = @LanguageID)
	ORDER BY 
		CreatedOn 
	DESC


	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		n.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_News n on n.NewsID = [pi].NewsID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0
END
GO

--order editing
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderProductVariantDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderProductVariantDelete]
GO
CREATE PROCEDURE [dbo].[Nop_OrderProductVariantDelete]
	@OrderProductVariantID int
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM Nop_OrderProductVariant
	WHERE OrderProductVariantID = @OrderProductVariantID
	
END
GO

-- picture loading optimization
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductPictureLoadAllByProductID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductPictureLoadAllByProductID]
GO
CREATE PROCEDURE [dbo].[Nop_ProductPictureLoadAllByProductID]
(
	@ProductID int,
	@PictureCount int
)
AS
BEGIN
	IF(@PictureCount > 0) SET ROWCOUNT @PictureCount
	
	SELECT
		*
	FROM 
		Nop_ProductPicture
	WHERE 
		ProductID = @ProductID
	ORDER BY 
		DisplayOrder
		
	SET ROWCOUNT 0
END
GO

--dynamic price update
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'ProductAttribute.EnableDynamicPriceUpdate')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'ProductAttribute.EnableDynamicPriceUpdate', N'False', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'ProductAttribute.PricePattern')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'ProductAttribute.PricePattern', N'(?<val>(\d+[\s\,\.]?)+)', N'')
END
GO

--forum subsription changes
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_SubscriptionLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_SubscriptionLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_SubscriptionLoadAll]
(
	@UserID				int,
	@ForumID			int,
	@TopicID			int,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		SubscriptionID int NOT NULL,
		CreatedOn datetime NOT NULL,
	)

	INSERT INTO #PageIndex (SubscriptionID, CreatedOn)
	SELECT DISTINCT
		fs.SubscriptionID, fs.CreatedOn
	FROM Nop_Forums_Subscription fs with (NOLOCK)
	INNER JOIN Nop_Customer c with (NOLOCK) ON fs.UserID=c.CustomerID
	WHERE   (
				@UserID IS NULL OR @UserID=0
				OR (fs.UserID=@UserID)
			)
		AND (
				@ForumID IS NULL OR @ForumID=0
				OR (fs.ForumID=@ForumID)
			)
		AND (
				@TopicID IS NULL OR @TopicID=0
				OR (fs.TopicID=@TopicID)
			)
		AND (
				c.Active=1 AND c.Deleted=0
			)
	ORDER BY fs.CreatedOn desc, fs.SubscriptionID desc

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		fs.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Forums_Subscription fs on fs.SubscriptionID = [pi].SubscriptionID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0
END
GO

--product sorting
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Common.AllowProductSorting')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Common.AllowProductSorting', N'True', N'')
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductLoadAllPaged]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductLoadAllPaged]
GO
CREATE PROCEDURE [dbo].[Nop_ProductLoadAllPaged]
(
	@CategoryID			int = 0,
	@ManufacturerID		int = 0,
	@FeaturedProducts	bit = null,	--0 featured only , 1 not featured only, null - load all products
	@PriceMin			money = null,
	@PriceMax			money = null,
	@Keywords			nvarchar(MAX),	
	@SearchDescriptions bit = 0,
	@ShowHidden			bit = 0,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@FilteredSpecs		nvarchar(300) = null,	--filter by attributes (comma-separated list). e.g. 14,15,16
	@LanguageID			int = 0,
	@OrderBy			int = 0, --0 position, 5 - Name, 10 - Price
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	--init
	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = '%' + rtrim(ltrim(@Keywords)) + '%'

	SET @PriceMin = isnull(@PriceMin, 0)
	SET @PriceMax = isnull(@PriceMax, 2147483644)
	
	--filter by attributes
	SET @FilteredSpecs = isnull(@FilteredSpecs, '')
	CREATE TABLE #FilteredSpecs
	(
		SpecificationAttributeOptionID int not null
	)
	INSERT INTO #FilteredSpecs (SpecificationAttributeOptionID)
	SELECT CAST(data as int) FROM dbo.[NOP_splitstring_to_table](@FilteredSpecs, ',');
	
	DECLARE @SpecAttributesCount int	
	SELECT @SpecAttributesCount = COUNT(1) FROM #FilteredSpecs

	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #DisplayOrderTmp 
	(
		[ID] int IDENTITY (1, 1) NOT NULL,
		[ProductID] int NOT NULL,
		[Name] nvarchar(400) not null,
		[Price] money not null,
		[DisplayOrder1] int,
		[DisplayOrder2] int,
	)

	INSERT INTO #DisplayOrderTmp ([ProductID], [Name], [Price], [DisplayOrder1], [DisplayOrder2])
	SELECT p.ProductID, p.Name, pv.Price, pcm.DisplayOrder, pmm.DisplayOrder 
	FROM Nop_Product p with (NOLOCK) 
	LEFT OUTER JOIN Nop_Product_Category_Mapping pcm with (NOLOCK) ON p.ProductID=pcm.ProductID
	LEFT OUTER JOIN Nop_Product_Manufacturer_Mapping pmm with (NOLOCK) ON p.ProductID=pmm.ProductID
	LEFT OUTER JOIN Nop_ProductVariant pv with (NOLOCK) ON p.ProductID = pv.ProductID
	LEFT OUTER JOIN Nop_ProductVariantLocalized pvl with (NOLOCK) ON pv.ProductVariantID = pvl.ProductVariantID AND pvl.LanguageID = @LanguageID
	LEFT OUTER JOIN Nop_ProductLocalized pl with (NOLOCK) ON p.ProductID = pl.ProductID AND pl.LanguageID = @LanguageID
	WHERE 
		(
			(
				@ShowHidden = 1 OR p.Published = 1
			)
		AND 
			(
				@ShowHidden = 1 OR pv.Published = 1
			)
		AND 
			(
				p.Deleted=0
			)
		AND (
				@CategoryID IS NULL OR @CategoryID=0
				OR (pcm.CategoryID=@CategoryID AND (@FeaturedProducts IS NULL OR pcm.IsFeaturedProduct=@FeaturedProducts))
			)
		AND (
				@ManufacturerID IS NULL OR @ManufacturerID=0
				OR (pmm.ManufacturerID=@ManufacturerID AND (@FeaturedProducts IS NULL OR pmm.IsFeaturedProduct=@FeaturedProducts))
			)
		AND (
				pv.Price BETWEEN @PriceMin AND @PriceMax
			)
		AND	(
				-- search standard content
				patindex(@Keywords, isnull(p.name, '')) > 0
				or patindex(@Keywords, isnull(pv.name, '')) > 0
				or patindex(@Keywords, isnull(pv.sku , '')) > 0
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(p.ShortDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(p.FullDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pv.Description, '')) > 0)					
				-- search language content
				or patindex(@Keywords, isnull(pl.name, '')) > 0
				or patindex(@Keywords, isnull(pvl.name, '')) > 0
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pl.ShortDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pl.FullDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pvl.Description, '')) > 0)
			)
		AND
			(
				@ShowHidden = 1
				OR
				(getutcdate() between isnull(pv.AvailableStartDateTime, '1/1/1900') and isnull(pv.AvailableEndDateTime, '1/1/2999'))
			)
		AND
			(
				--filter by specs
				@SpecAttributesCount = 0
				OR
				(
					NOT EXISTS(
						SELECT 1 
						FROM #FilteredSpecs [fs]
						WHERE [fs].SpecificationAttributeOptionID NOT IN (
							SELECT psam.SpecificationAttributeOptionID
							FROM dbo.Nop_Product_SpecificationAttribute_Mapping psam
							WHERE psam.AllowFiltering = 1 AND psam.ProductID = p.ProductID
							)
						)
					
				)
			)
		)
	ORDER BY 
		CASE WHEN @OrderBy = 0 AND @CategoryID IS NOT NULL AND @CategoryID > 0
		THEN pcm.DisplayOrder END,
		CASE WHEN @OrderBy = 0 AND @ManufacturerID IS NOT NULL AND @ManufacturerID > 0
		THEN pmm.DisplayOrder END,
		CASE WHEN @OrderBy = 0
		THEN dbo.NOP_getnotnullnotempty(pl.[Name],p.[Name]) END,
		CASE WHEN @OrderBy = 5
		THEN dbo.NOP_getnotnullnotempty(pl.[Name],p.[Name]) END,
		CASE WHEN @OrderBy = 10
		THEN pv.Price END

	CREATE TABLE #PageIndex 
	(
		[IndexID] int IDENTITY (1, 1) NOT NULL,
		[ProductID] int NOT NULL
	)

	INSERT INTO #PageIndex ([ProductID])
	SELECT ProductID
	FROM #DisplayOrderTmp with (NOLOCK)
	GROUP BY ProductID
	ORDER BY min([ID])

	--total records
	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	--return
	SELECT  
		p.[ProductId],
		dbo.NOP_getnotnullnotempty(pl.[Name],p.[Name]) as [Name],
		dbo.NOP_getnotnullnotempty(pl.[ShortDescription],p.[ShortDescription]) as [ShortDescription],
		dbo.NOP_getnotnullnotempty(pl.[FullDescription],p.[FullDescription]) as [FullDescription],
		p.[AdminComment], 
		p.[ProductTypeID], 
		p.[TemplateID], 
		p.[ShowOnHomePage], 
		dbo.NOP_getnotnullnotempty(pl.[MetaKeywords],p.[MetaKeywords]) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(pl.[MetaDescription],p.[MetaDescription]) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(pl.[MetaTitle],p.[MetaTitle]) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(pl.[SEName],p.[SEName]) as [SEName],
		p.[AllowCustomerReviews], 
		p.[AllowCustomerRatings], 
		p.[RatingSum], 
		p.[TotalRatingVotes], 
		p.[Published], 
		p.[Deleted], 
		p.[CreatedOn], 
		p.[UpdatedOn]
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Product p on p.ProductID = [pi].ProductID
		LEFT OUTER JOIN Nop_ProductLocalized pl on (pl.ProductID = p.ProductID AND pl.LanguageID = @LanguageID) 
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #FilteredSpecs
	DROP TABLE #DisplayOrderTmp
	DROP TABLE #PageIndex
END
GO



--product tags
if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_ProductTag]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Nop_ProductTag](
	[ProductTagID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[ProductCount] int NOT NULL,
 CONSTRAINT [PK_Nop_ProductTag] PRIMARY KEY CLUSTERED 
(
	[ProductTagID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
) ON [PRIMARY]
END
GO

if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_ProductTag_Product_Mapping]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[Nop_ProductTag_Product_Mapping](
	[ProductTagID] [int] NOT NULL,
	[ProductID] [int] NOT NULL
CONSTRAINT [Nop_ProductTag_Product_Mapping_PK] PRIMARY KEY CLUSTERED 
(
	[ProductTagID] ASC,
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_ProductTag_Product_Mapping_Nop_ProductTag'
           AND parent_obj = Object_id('Nop_ProductTag_Product_Mapping')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_ProductTag_Product_Mapping
DROP CONSTRAINT FK_Nop_ProductTag_Product_Mapping_Nop_ProductTag
GO
ALTER TABLE [dbo].[Nop_ProductTag_Product_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Nop_ProductTag_Product_Mapping_Nop_ProductTag] FOREIGN KEY([ProductTagID])
REFERENCES [dbo].[Nop_ProductTag] ([ProductTagID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_ProductTag_Product_Mapping_Nop_Product'
           AND parent_obj = Object_id('Nop_ProductTag_Product_Mapping')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_ProductTag_Product_Mapping
DROP CONSTRAINT FK_Nop_ProductTag_Product_Mapping_Nop_Product
GO
ALTER TABLE [dbo].[Nop_ProductTag_Product_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Nop_ProductTag_Product_Mapping_Nop_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Nop_Product] ([ProductID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductTagUpdateCounts]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductTagUpdateCounts]
GO
CREATE PROCEDURE [dbo].[Nop_ProductTagUpdateCounts]
(
	@ProductTagID int
)
AS
BEGIN

	DECLARE @NumRecords int

	SELECT 
		@NumRecords = COUNT(1)
	FROM
		[Nop_ProductTag_Product_Mapping] ptpm
	WHERE
		ptpm.ProductTagID = @ProductTagID

	SET @NumRecords = isnull(@NumRecords, 0)
	
	SET NOCOUNT ON
	UPDATE 
		[Nop_ProductTag]
	SET
		[ProductCount] = @NumRecords
	WHERE
		[ProductTagID] = @ProductTagID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductTagDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductTagDelete]
GO
CREATE PROCEDURE [dbo].[Nop_ProductTagDelete]
(
	@ProductTagID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ProductTag]
	WHERE
		ProductTagID = @ProductTagID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductTagInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductTagInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ProductTagInsert]
(
	@ProductTagID int = NULL output,
	@Name nvarchar(100),
	@ProductCount int
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductTag]
	(
		[Name],
		[ProductCount]
	)
	VALUES
	(
		@Name,
		@ProductCount
	)

	set @ProductTagID=SCOPE_IDENTITY()
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductTagLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductTagLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_ProductTagLoadAll]
(
	@ProductID int,
	@Name nvarchar(100)
)
AS
BEGIN	

	SET @Name = isnull(@Name, '')
	
	SELECT pt1.*
	FROM [Nop_ProductTag] pt1
	WHERE pt1.ProductTagID IN
	(
		SELECT DISTINCT pt2.ProductTagID
		FROM [Nop_ProductTag] pt2
		LEFT OUTER JOIN [Nop_ProductTag_Product_Mapping] ptpm ON pt2.ProductTagID=ptpm.ProductTagID
		WHERE 
			(
				@ProductID IS NULL OR @ProductID=0
				OR ptpm.ProductID=@ProductID
			)
			AND
			(
				@Name = '' OR pt2.Name=@Name
			)
	)
	ORDER BY pt1.ProductCount DESC
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductTagLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductTagLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_ProductTagLoadByPrimaryKey]
(
	@ProductTagID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ProductTag]
	WHERE
		ProductTagID = @ProductTagID
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductTagUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductTagUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ProductTagUpdate]
(
	@ProductTagID int,
	@Name nvarchar(100),
	@ProductCount int
)
AS
BEGIN

	UPDATE [Nop_ProductTag]
	SET
		[Name]=@Name,
		[ProductCount]=@ProductCount
	WHERE
		ProductTagID = @ProductTagID

END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductTag_Product_MappingDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductTag_Product_MappingDelete]
GO
CREATE PROCEDURE [dbo].[Nop_ProductTag_Product_MappingDelete]
(
	@ProductTagID int,
	@ProductID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ProductTag_Product_Mapping]
	WHERE
		[ProductTagID] = @ProductTagID and [ProductID]=@ProductID
	
	exec [dbo].[Nop_ProductTagUpdateCounts] @ProductTagID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductTag_Product_MappingInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductTag_Product_MappingInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ProductTag_Product_MappingInsert]
(
	@ProductTagID int,
	@ProductID int
)
AS
BEGIN
	IF NOT EXISTS (SELECT (1) FROM [Nop_ProductTag_Product_Mapping] WHERE [ProductTagID] = @ProductTagID and [ProductID]=@ProductID)
	INSERT
		INTO [Nop_ProductTag_Product_Mapping]
		(
			[ProductTagID],
			[ProductID]
		)
		VALUES
		(
			@ProductTagID,
			@ProductID
		)

	exec [dbo].[Nop_ProductTagUpdateCounts] @ProductTagID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductLoadAllPaged]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductLoadAllPaged]
GO
CREATE PROCEDURE [dbo].[Nop_ProductLoadAllPaged]
(
	@CategoryID			int = 0,
	@ManufacturerID		int = 0,
	@ProductTagID		int = 0,
	@FeaturedProducts	bit = null,	--0 featured only , 1 not featured only, null - load all products
	@PriceMin			money = null,
	@PriceMax			money = null,
	@Keywords			nvarchar(MAX),	
	@SearchDescriptions bit = 0,
	@ShowHidden			bit = 0,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@FilteredSpecs		nvarchar(300) = null,	--filter by attributes (comma-separated list). e.g. 14,15,16
	@LanguageID			int = 0,
	@OrderBy			int = 0, --0 position, 5 - Name, 10 - Price
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	--init
	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = '%' + rtrim(ltrim(@Keywords)) + '%'

	SET @PriceMin = isnull(@PriceMin, 0)
	SET @PriceMax = isnull(@PriceMax, 2147483644)
	
	--filter by attributes
	SET @FilteredSpecs = isnull(@FilteredSpecs, '')
	CREATE TABLE #FilteredSpecs
	(
		SpecificationAttributeOptionID int not null
	)
	INSERT INTO #FilteredSpecs (SpecificationAttributeOptionID)
	SELECT CAST(data as int) FROM dbo.[NOP_splitstring_to_table](@FilteredSpecs, ',');
	
	DECLARE @SpecAttributesCount int	
	SELECT @SpecAttributesCount = COUNT(1) FROM #FilteredSpecs

	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #DisplayOrderTmp 
	(
		[ID] int IDENTITY (1, 1) NOT NULL,
		[ProductID] int NOT NULL,
		[Name] nvarchar(400) not null,
		[Price] money not null,
		[DisplayOrder1] int,
		[DisplayOrder2] int,
	)

	INSERT INTO #DisplayOrderTmp ([ProductID], [Name], [Price], [DisplayOrder1], [DisplayOrder2])
	SELECT p.ProductID, p.Name, pv.Price, pcm.DisplayOrder, pmm.DisplayOrder 
	FROM Nop_Product p with (NOLOCK) 
	LEFT OUTER JOIN Nop_Product_Category_Mapping pcm with (NOLOCK) ON p.ProductID=pcm.ProductID
	LEFT OUTER JOIN Nop_Product_Manufacturer_Mapping pmm with (NOLOCK) ON p.ProductID=pmm.ProductID
	LEFT OUTER JOIN Nop_ProductTag_Product_Mapping ptpm with (NOLOCK) ON p.ProductID=ptpm.ProductID
	LEFT OUTER JOIN Nop_ProductVariant pv with (NOLOCK) ON p.ProductID = pv.ProductID
	LEFT OUTER JOIN Nop_ProductVariantLocalized pvl with (NOLOCK) ON pv.ProductVariantID = pvl.ProductVariantID AND pvl.LanguageID = @LanguageID
	LEFT OUTER JOIN Nop_ProductLocalized pl with (NOLOCK) ON p.ProductID = pl.ProductID AND pl.LanguageID = @LanguageID
	WHERE 
		(
			(
				@ShowHidden = 1 OR p.Published = 1
			)
		AND 
			(
				@ShowHidden = 1 OR pv.Published = 1
			)
		AND 
			(
				p.Deleted=0
			)
		AND (
				@CategoryID IS NULL OR @CategoryID=0
				OR (pcm.CategoryID=@CategoryID AND (@FeaturedProducts IS NULL OR pcm.IsFeaturedProduct=@FeaturedProducts))
			)
		AND (
				@ManufacturerID IS NULL OR @ManufacturerID=0
				OR (pmm.ManufacturerID=@ManufacturerID AND (@FeaturedProducts IS NULL OR pmm.IsFeaturedProduct=@FeaturedProducts))
			)
		AND (
				@ProductTagID IS NULL OR @ProductTagID=0
				OR ptpm.ProductTagID=@ProductTagID
			)
		AND (
				pv.Price BETWEEN @PriceMin AND @PriceMax
			)
		AND	(
				-- search standard content
				patindex(@Keywords, isnull(p.name, '')) > 0
				or patindex(@Keywords, isnull(pv.name, '')) > 0
				or patindex(@Keywords, isnull(pv.sku , '')) > 0
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(p.ShortDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(p.FullDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pv.Description, '')) > 0)					
				-- search language content
				or patindex(@Keywords, isnull(pl.name, '')) > 0
				or patindex(@Keywords, isnull(pvl.name, '')) > 0
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pl.ShortDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pl.FullDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pvl.Description, '')) > 0)
			)
		AND
			(
				@ShowHidden = 1
				OR
				(getutcdate() between isnull(pv.AvailableStartDateTime, '1/1/1900') and isnull(pv.AvailableEndDateTime, '1/1/2999'))
			)
		AND
			(
				--filter by specs
				@SpecAttributesCount = 0
				OR
				(
					NOT EXISTS(
						SELECT 1 
						FROM #FilteredSpecs [fs]
						WHERE [fs].SpecificationAttributeOptionID NOT IN (
							SELECT psam.SpecificationAttributeOptionID
							FROM dbo.Nop_Product_SpecificationAttribute_Mapping psam
							WHERE psam.AllowFiltering = 1 AND psam.ProductID = p.ProductID
							)
						)
					
				)
			)
		)
	ORDER BY 
		CASE WHEN @OrderBy = 0 AND @CategoryID IS NOT NULL AND @CategoryID > 0
		THEN pcm.DisplayOrder END,
		CASE WHEN @OrderBy = 0 AND @ManufacturerID IS NOT NULL AND @ManufacturerID > 0
		THEN pmm.DisplayOrder END,
		CASE WHEN @OrderBy = 0
		THEN dbo.NOP_getnotnullnotempty(pl.[Name],p.[Name]) END,
		CASE WHEN @OrderBy = 5
		THEN dbo.NOP_getnotnullnotempty(pl.[Name],p.[Name]) END,
		CASE WHEN @OrderBy = 10
		THEN pv.Price END

	CREATE TABLE #PageIndex 
	(
		[IndexID] int IDENTITY (1, 1) NOT NULL,
		[ProductID] int NOT NULL
	)

	INSERT INTO #PageIndex ([ProductID])
	SELECT ProductID
	FROM #DisplayOrderTmp with (NOLOCK)
	GROUP BY ProductID
	ORDER BY min([ID])

	--total records
	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	--return
	SELECT  
		p.[ProductId],
		dbo.NOP_getnotnullnotempty(pl.[Name],p.[Name]) as [Name],
		dbo.NOP_getnotnullnotempty(pl.[ShortDescription],p.[ShortDescription]) as [ShortDescription],
		dbo.NOP_getnotnullnotempty(pl.[FullDescription],p.[FullDescription]) as [FullDescription],
		p.[AdminComment], 
		p.[ProductTypeID], 
		p.[TemplateID], 
		p.[ShowOnHomePage], 
		dbo.NOP_getnotnullnotempty(pl.[MetaKeywords],p.[MetaKeywords]) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(pl.[MetaDescription],p.[MetaDescription]) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(pl.[MetaTitle],p.[MetaTitle]) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(pl.[SEName],p.[SEName]) as [SEName],
		p.[AllowCustomerReviews], 
		p.[AllowCustomerRatings], 
		p.[RatingSum], 
		p.[TotalRatingVotes], 
		p.[Published], 
		p.[Deleted], 
		p.[CreatedOn], 
		p.[UpdatedOn]
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Product p on p.ProductID = [pi].ProductID
		LEFT OUTER JOIN Nop_ProductLocalized pl on (pl.ProductID = p.ProductID AND pl.LanguageID = @LanguageID) 
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #FilteredSpecs
	DROP TABLE #DisplayOrderTmp
	DROP TABLE #PageIndex
END
GO

	
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Common.ShowAlertForProductAttributes')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Common.ShowAlertForProductAttributes', N'true', N'')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Common.ShowAlertForPM')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Common.ShowAlertForPM', N'true', N'')
END
GO


--Alipay payment module
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_PaymentMethod]
		WHERE [ClassName] = N'NopSolutions.NopCommerce.Payment.Methods.Alipay.AlipayPaymentProcessor, Nop.Payment.Alipay')
BEGIN
	INSERT [dbo].[Nop_PaymentMethod] ([Name], [VisibleName], [Description], [ConfigureTemplatePath], [UserTemplatePath], [ClassName], [SystemKeyword], [IsActive], [DisplayOrder]) 
	VALUES (N'Alipay', N'Alipay', N'', N'Payment\Alipay\HostedPaymentConfig.ascx', N'~\Templates\Payment\Alipay\HostedPayment.ascx', N'NopSolutions.NopCommerce.Payment.Methods.Alipay.AlipayPaymentProcessor, Nop.Payment.Alipay', N'ALIPAY', 0, 280)
END
GO