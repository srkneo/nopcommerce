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
		[Filename]=@Filename,
		[Extension]=@Extension,
		[IsNew]=@IsNew
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
