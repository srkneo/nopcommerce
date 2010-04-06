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
		([Description] IS NULL OR substring([Description],1,100) = '') AND
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
		([Description] IS NULL OR substring([Description],1,100) = '') AND
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
	DELETE FROM [dbo].[Nop_CustomerAction] WHERE [SystemKeyword] = N'ManageCoutriesStates'
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_CustomerAction]
		WHERE [SystemKeyword] = N'ManageCountriesStates')
BEGIN
	INSERT [dbo].[Nop_CustomerAction] ([Name], [SystemKeyword], [Comment], [DisplayOrder])
	VALUES (N'Manage Countries / States', N'ManageCountriesStates', N'',250)
END
GO
