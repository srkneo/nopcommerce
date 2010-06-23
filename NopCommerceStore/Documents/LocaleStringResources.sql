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


