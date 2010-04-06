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
