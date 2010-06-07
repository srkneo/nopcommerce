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