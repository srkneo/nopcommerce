CREATE PROCEDURE `nop_splitstring_to_table`
(
    IN string LONGTEXT,
    IN delim CHAR(1)
)
BEGIN
    DROP TEMPORARY TABLE IF EXISTS nop_splitstring_to_table_TempTable;
    CREATE TEMPORARY TABLE nop_splitstring_to_table_TempTable
    (
        `data` LONGTEXT
    );
    
    set @start = 1;    
    set @end = LOCATE(delim, string);

    WHILE @start < LENGTH(string) + 1 DO
        IF @end = 0 then
            SET @end = LENGTH(string) + 1;
        END IF;

        INSERT INTO nop_splitstring_to_table_TempTable (data) 
        VALUES(SUBSTRING(string, @start, @end - @start));
        SET @start = @end + 1;
        SET @end = LOCATE(delim, string, @start);
    END WHILE;
END
-- GO



CREATE PROCEDURE `nop_getnotnullnotempty`
(
    IN p1 LONGTEXT, 
    IN p2 LONGTEXT,
    OUT res LONGTEXT
)
BEGIN
    set res = p1;
    
    IF res IS NULL OR res = '' then
        set res = p2;
    END IF;
END
-- GO



CREATE PROCEDURE `nop_getprimarykey_indexname`
(
    IN table_name nvarchar(1000),
    OUT index_name nvarchar(1000)
)
BEGIN
    select CONSTRAINT_NAME into index_name from information_schema.table_constraints t
where t.TABLE_NAME = table_name AND CONSTRAINT_TYPE = 'PRIMARY KEY';
END
-- GO


CREATE PROCEDURE `ProductLoadAllPaged`(
	IN CategoryIds		LONGTEXT,	-- a list of category IDs (comma-separated list). e.g. 1,2,3
	IN ManufacturerId		int,
	IN ProductTagId		int,
	IN FeaturedProducts	bool,	-- 0 featured only , 1 not featured only, null - load all products
	IN PriceMin			decimal(18, 4),
	IN PriceMax			decimal(18, 4),
	IN Keywords			nvarchar(4000),
	IN SearchDescriptions bool,
	IN UseFullTextSearch  bool,
	IN FullTextMode		int, -- 0 using CONTAINS with <prefix_term>, 5 - using CONTAINS and OR with <prefix_term>, 10 - using CONTAINS and AND with <prefix_term>
	IN FilteredSpecs		longtext,	-- filter by attributes (comma-separated list). e.g. 14,15,16
	IN LanguageId			int,
	IN OrderBy			int, -- 0 position, 5 - Name: A to Z, 6 - Name: Z to A, 10 - Price: Low to High, 11 - Price: High to Low, 15 - creation date
	IN PageIndex			int, 
	IN PageSize			int,
	IN ShowHidden			bool,
	IN LoadFilterableSpecificationAttributeOptionIds bool, -- a value indicating whether we should load the specification attribute option identifiers applied to loaded products (all pages)
	OUT FilterableSpecificationAttributeOptionIds longtext, -- the specification attribute option identifiers applied to loaded products (all pages). returned as a comma separated list of identifiers
	OUT TotalRecords		int
)
BEGIN

if ManufacturerId is null then
        set ManufacturerId = 0;
    END IF;
    
    if ProductTagId is null then
        set ProductTagId = 0;
    END IF;
    
    if SearchDescriptions is null then
        set SearchDescriptions = 0;
    END IF;
    
    if UseFullTextSearch is null then
        set UseFullTextSearch = 0;
    END IF;
    
    if FullTextMode is null then
        set FullTextMode = 0;
    END IF;
    
    if LanguageId is null then
        set LanguageId = 0;
    END IF;
    
    if OrderBy is null then
        set OrderBy = 0;
    END IF;
    
    if PageIndex is null then
        set PageIndex = 0;
    END IF;
    
    if PageSize is null then
        set PageSize = 2147483644;
    END IF;
    
    if ShowHidden is null then
        set ShowHidden = 0;
    END IF;
    
    if LoadFilterableSpecificationAttributeOptionIds is null then
        set LoadFilterableSpecificationAttributeOptionIds = 0;
    END IF;
	
	/* Products that filtered by keywords */
	drop temporary table if exists KeywordProducts_TempTable;
	CREATE temporary TABLE KeywordProducts_TempTable
	(
		ProductId int NOT NULL
	) ENGINE = MEMORY;
	
	
	-- filter by keywords
	SET Keywords = coalesce(Keywords, '');
	SET Keywords = rtrim(ltrim(Keywords));
	IF Keywords != '' then
		SET @SearchKeywords = 1;
		
		IF UseFullTextSearch = 1 then        
			-- full-text search
			IF FullTextMode = 0 then
				-- 0 - using CONTAINS with <prefix_term>
				SET Keywords = concat(' "', Keywords, '*" ');
			ELSE
				-- 5 - using CONTAINS and OR with <prefix_term>
				-- 10 - using CONTAINS and AND with <prefix_term>

				-- remove wrong chars (' ")
				SET Keywords = REPLACE(Keywords, '''', '');
				SET Keywords = REPLACE(Keywords, '"', '');
				-- clean multiple spaces
				WHILE LOCATE('  ', Keywords) > 0 DO
					SET Keywords = REPLACE(Keywords, '  ', ' ');
        END WHILE;

				IF FullTextMode = 5 then -- 5 - using CONTAINS and OR with <prefix_term>
					SET @concat_term = 'OR';				
				ELSEIF FullTextMode = 10 then -- 10 - using CONTAINS and AND with <prefix_term>
					SET @concat_term = 'AND';
				END IF;

				-- now let's build search string
				set @fulltext_keywords = N'';
		
				set @index = LOCATE(' ', Keywords, 0);

				--  if index = 0, then only one field was passed
				IF @index = 0 then
					set @fulltext_keywords = CONCAT(' "', Keywords, '*" ');
				ELSE
					SET  @first = 1;
					WHILE @index > 0 DO
						IF @first = 0 then
							SET @fulltext_keywords = CONCAT(@fulltext_keywords, ' ', @concat_term, ' ');
						ELSE
							SET @first = 0;
            END IF;

						SET @fulltext_keywords = CONCAT(@fulltext_keywords, '"', SUBSTRING(Keywords, 1, @index - 1), '*"');
						SET Keywords = SUBSTRING(Keywords, @index + 1, LENGTH(Keywords) - @index);
						SET @index = LOCATE(' ', Keywords, 0);
					end while;
					
					--  add the last field
					IF LENGTH(@fulltext_keywords) > 0 THEN
						SET @fulltext_keywords = CONCAT(@fulltext_keywords, ' ', @concat_term, ' ', '"', SUBSTRING(Keywords, 1, LENGTH(Keywords)), '*"');
          END IF;
				END IF;
				SET Keywords = @fulltext_keywords;
			END IF;
		ELSE
			-- usual search by PATINDEX
			SET Keywords = concat('%', Keywords, '%');
		END IF;
		-- PRINT Keywords
        
		-- product name
		SET @sql = '
		INSERT INTO KeywordProducts_TempTable (`ProductId`)
		SELECT p.Id
		FROM Product p
		WHERE ';
		IF UseFullTextSearch = 1 THEN
			SET @sql = CONCAT(@sql, 'MATCH (p.`Name`) AGAINST (@Keywords IN BOOLEAN MODE) ');
		ELSE
			SET @sql = CONCAT(@sql, 'p.`Name` LIKE @Keywords ');
    END IF;


		-- product variant name
		SET @sql = CONCAT(@sql, '
		UNION
		SELECT pv.ProductId
		FROM ProductVariant pv
		WHERE ');
		IF UseFullTextSearch = 1 THEN
			SET @sql = CONCAT(@sql, 'MATCH (pv.`Name`) AGAINST (@Keywords IN BOOLEAN MODE) ');
		ELSE
			SET @sql = CONCAT(@sql, 'pv.`Name` LIKE @Keywords ');
    END IF;

		-- SKU
		SET @sql = CONCAT(@sql, '
		UNION
		SELECT pv.ProductId
		FROM ProductVariant pv
		WHERE ');
		IF UseFullTextSearch = 1 THEN
			SET @sql = CONCAT(@sql, 'MATCH (pv.`Sku`) AGAINST (@Keywords IN BOOLEAN MODE) ');
		ELSE
			SET @sql = CONCAT(@sql, 'pv.`Sku` LIKE @Keywords ');
    END IF;

		-- localized product name
		SET @sql = CONCAT(@sql, '
		UNION
		SELECT lp.EntityId
		FROM LocalizedProperty lp
		WHERE
			lp.LocaleKeyGroup = N''Product''
			AND lp.LanguageId = ', COALESCE(LanguageId, 0), '
			AND lp.LocaleKey = N''Name''');
		IF UseFullTextSearch = 1 THEN
			SET @sql = CONCAT(@sql, ' AND MATCH (lp.`LocaleValue`) AGAINST (@Keywords IN BOOLEAN MODE) ');
		ELSE
			SET @sql = CONCAT(@sql, ' AND lp.`LocaleValue` LIKE @Keywords ');
    END IF;	

		-- product short description
		IF SearchDescriptions = 1 THEN
			SET @sql = CONCAT(@sql, '
			UNION
			SELECT p.Id
			FROM Product p
			WHERE ');
			IF UseFullTextSearch = 1 THEN
				SET @sql = CONCAT(@sql, 'MATCH (p.`ShortDescription`) AGAINST (@Keywords IN BOOLEAN MODE) ');
			ELSE
				SET @sql = CONCAT(@sql, 'p.`ShortDescription` LIKE @Keywords ');
      END IF;


			-- product full description
			SET @sql = CONCAT(@sql, '
			UNION
			SELECT p.Id
			FROM Product p
			WHERE ');
			IF UseFullTextSearch = 1 THEN
				SET @sql = CONCAT(@sql, 'MATCH (p.`FullDescription`) AGAINST (@Keywords IN BOOLEAN MODE) ');
			ELSE
				SET @sql = CONCAT(@sql, 'p.`FullDescription` LIKE @Keywords ');
      END IF;

			-- product variant description
			SET @sql = CONCAT(@sql, '
			UNION
			SELECT pv.ProductId
			FROM ProductVariant pv
			WHERE ');
			IF UseFullTextSearch = 1 THEN
				SET @sql = CONCAT(@sql, 'MATCH (pv.`Description`) AGAINST (@Keywords IN BOOLEAN MODE) ');
			ELSE
				SET @sql = CONCAT(@sql, 'pv.`Description` LIKE @Keywords ');
      END IF;


			-- localized product short description
			SET @sql = CONCAT(@sql, '
			UNION
			SELECT lp.EntityId
			FROM LocalizedProperty lp
			WHERE
				lp.LocaleKeyGroup = N''Product''
				AND lp.LanguageId = ', COALESCE(LanguageId, 0), '
				AND lp.LocaleKey = N''ShortDescription''');
			IF UseFullTextSearch = 1 THEN
				SET @sql = CONCAT(@sql, ' AND MATCH (lp.`LocaleValue`) AGAINST (@Keywords IN BOOLEAN MODE) ');
			ELSE
				SET @sql = CONCAT(@sql, ' AND lp.`LocaleValue` LIKE @Keywords ');
      END IF;				

			-- localized product full description
			SET @sql = CONCAT(@sql, '
			UNION
			SELECT lp.EntityId
			FROM LocalizedProperty lp
			WHERE
				lp.LocaleKeyGroup = N''Product''
				AND lp.LanguageId = ', COALESCE(LanguageId, 0), '
				AND lp.LocaleKey = N''FullDescription''');
			IF UseFullTextSearch = 1 THEN
				SET @sql = CONCAT(@sql, ' AND MATCH (lp.`LocaleValue`) AGAINST (@Keywords IN BOOLEAN MODE) ');
			ELSE
				SET @sql = CONCAT(@sql, ' AND lp.`LocaleValue` LIKE @Keywords ');
      END IF;
		END IF;

    -- select @sql;
    
    -- set @TempKeywords = Keywords;
    -- SET @sql = '
--         Select @Keywords;';
        
		-- PRINT (@sql)
    PREPARE stmt1 FROM @sql; 
    SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    
    EXECUTE stmt1; -- USING @TempKeywords; 
    SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ;
    DEALLOCATE PREPARE stmt1;
    
    
    -- select * from KeywordProducts_TempTable;
		-- EXEC sp_executesql @sql, N'Keywords nvarchar(4000)', Keywords
	
	ELSE
		SET @SearchKeywords = 0;
	END IF;

	-- filter by category IDs
	SET CategoryIds = COALESCE(CategoryIds, '');
	Drop temporary table if exists FilteredCategoryIds_TempTable;
	CREATE temporary TABLE FilteredCategoryIds_TempTable
	(
		CategoryId int not null
	) ENGINE = MEMORY;
    
    
    
    call nop_splitstring_to_table(CategoryIds, ',');
	INSERT INTO FilteredCategoryIds_TempTable (CategoryId)
	SELECT (data + 0) FROM nop_splitstring_to_table_TempTable;
    
    SELECT COUNT(1) FROM FilteredCategoryIds_TempTable into @CategoryIdsCount;

	-- filter by attributes
	SET FilteredSpecs = COALESCE(FilteredSpecs, '');	
	Drop temporary table if exists FilteredSpecs_TempTable;
	CREATE temporary TABLE FilteredSpecs_TempTable
	(
		SpecificationAttributeOptionId int not null
	) ENGINE = MEMORY;
    
    call nop_splitstring_to_table(FilteredSpecs, ',');
	INSERT INTO FilteredSpecs_TempTable (SpecificationAttributeOptionId)
	SELECT (data + 0) FROM nop_splitstring_to_table_TempTable;	
    
    SELECT COUNT(1) FROM FilteredSpecs_TempTable into @SpecAttributesCount;

	-- paging
	SET @RowsToReturn = PageSize * (PageIndex + 1);
	SET @PageLowerBound = PageSize * PageIndex;
	SET @PageUpperBound = @PageLowerBound + PageSize + 1;
	
	Drop temporary table if exists DisplayOrder_TempTable;
	CREATE temporary TABLE DisplayOrder_TempTable 
	(
		Id int NOT NULL AUTO_INCREMENT,
		ProductId int NOT NULL,
    PRIMARY KEY (`Id`)
	) ENGINE = MEMORY;

	SET @sql = '
	INSERT INTO DisplayOrder_TempTable (`ProductId`)
	SELECT p.Id
	FROM
		Product p';
	
	IF @CategoryIdsCount > 0 THEN
		SET @sql = CONCAT(@sql, '
		LEFT JOIN Product_Category_Mapping pcm
			ON p.Id = pcm.ProductId');
	END IF;
	
	IF ManufacturerId > 0 THEN
		SET @sql = CONCAT(@sql, '
		LEFT JOIN Product_Manufacturer_Mapping pmm
			ON p.Id = pmm.ProductId');
	END IF;
	
	IF COALESCE(ProductTagId, 0) != 0 THEN
		SET @sql = CONCAT(@sql, '
		LEFT JOIN Product_ProductTag_Mapping pptm
			ON p.Id = pptm.Product_Id');
	END IF;
	
	IF ShowHidden = 0
	OR PriceMin > 0
	OR PriceMax > 0
	OR OrderBy = 10 /* Price: Low to High */
	OR OrderBy = 11 /* Price: High to Low */ THEN
		SET @sql = CONCAT(@sql, '
		LEFT JOIN ProductVariant pv
			ON p.Id = pv.ProductId');
	END IF;
	
	-- searching by keywords
	IF @SearchKeywords = 1 THEN
		SET @sql = CONCAT(@sql, '
		JOIN KeywordProducts_TempTable kp
			ON  p.Id = kp.ProductId');
	END IF;
	
	SET @sql = CONCAT(@sql, '
	WHERE
		p.Deleted = 0');
	
	-- filter by category
	IF @CategoryIdsCount > 0 THEN
		SET @sql = CONCAT(@sql, '
		AND pcm.CategoryId IN (SELECT CategoryId FROM FilteredCategoryIds_TempTable)');
		
		IF FeaturedProducts IS NOT NULL THEN
			SET @sql = CONCAT(@sql, '
		AND pcm.IsFeaturedProduct = ', FeaturedProducts);
		END IF;
	END IF;
	
	-- filter by manufacturer
	IF ManufacturerId > 0 THEN
		SET @sql = CONCAT(@sql, '
		AND pmm.ManufacturerId = ', ManufacturerId);
		
		IF FeaturedProducts IS NOT NULL THEN
			SET @sql = CONCAT(@sql, '
		AND pmm.IsFeaturedProduct = ', FeaturedProducts);
		END IF;
	END IF;
	
	-- filter by product tag
	IF COALESCE(ProductTagId, 0) != 0 THEN
		SET @sql = CONCAT(@sql, '
		AND pptm.ProductTag_Id = ', ProductTagId);
	END IF;
	
	IF ShowHidden = 0 THEN
		SET @sql = CONCAT(@sql, '
		AND p.Published = 1
		AND pv.Published = 1
		AND pv.Deleted = 0
		AND (utc_timestamp() BETWEEN COALESCE(pv.AvailableStartDateTimeUtc, STR_TO_DATE(''(1-1-1900)'', ''(%e-%c-%Y)'')) and COALESCE(pv.AvailableEndDateTimeUtc, STR_TO_DATE(''(1-1-2999)'', ''(%e-%c-%Y)'')))');
	END IF;
	
	-- min price
	IF PriceMin > 0 THEN
		SET @sql = CONCAT(@sql, '
		AND (
				(
					-- special price (specified price and valid date range)
					(pv.SpecialPrice IS NOT NULL AND (utc_timestamp() BETWEEN COALESCE(pv.SpecialPriceStartDateTimeUtc, STR_TO_DATE(''(1-1-1900)'', ''(%e-%c-%Y)'')) AND COALESCE(pv.SpecialPriceEndDateTimeUtc, STR_TO_DATE(''(1-1-2999)'', ''(%e-%c-%Y)''))))
					AND
					(pv.SpecialPrice >= ', PriceMin, ')
				)
				OR 
				(
					-- regular price (price isnt specified or date range isnt valid)
					(pv.SpecialPrice IS NULL OR (utc_timestamp() NOT BETWEEN COALESCE(pv.SpecialPriceStartDateTimeUtc, STR_TO_DATE(''(1-1-1900)'', ''(%e-%c-%Y)'')) AND COALESCE(pv.SpecialPriceEndDateTimeUtc, STR_TO_DATE(''(1-1-2999)'', ''(%e-%c-%Y)''))))
					AND
					(pv.Price >= ', PriceMin, ')
				)
			)');
	END IF;
	
	-- max price
	IF PriceMax > 0 THEN
		SET @sql = CONCAT(@sql, '
		AND (
				(
					-- special price (specified price and valid date range)
					(pv.SpecialPrice IS NOT NULL AND (utc_timestamp() BETWEEN COALESCE(pv.SpecialPriceStartDateTimeUtc, STR_TO_DATE(''(1-1-1900)'', ''(%e-%c-%Y)'')) AND COALESCE(pv.SpecialPriceEndDateTimeUtc, STR_TO_DATE(''(1-1-2999)'', ''(%e-%c-%Y)''))))
					AND
					(pv.SpecialPrice <= ', PriceMax, ')
				)
				OR 
				(
					-- regular price (price isnt specified or date range isnt valid)
					(pv.SpecialPrice IS NULL OR (utc_timestamp() NOT BETWEEN COALESCE(pv.SpecialPriceStartDateTimeUtc, STR_TO_DATE(''(1-1-1900)'', ''(%e-%c-%Y)'')) AND COALESCE(pv.SpecialPriceEndDateTimeUtc, STR_TO_DATE(''(1-1-2999)'', ''(%e-%c-%Y)''))))
					AND
					(pv.Price <= ', PriceMax, ')
				)
			)');
	END IF;
	
	-- filter by specs
	IF @SpecAttributesCount > 0 THEN
		SET @sql = CONCAT(@sql, '
		AND NOT EXISTS (
			SELECT 1 
			FROM
				FilteredSpecs_TempTable `fs`
			WHERE
				`fs`.SpecificationAttributeOptionId NOT IN (
					SELECT psam.SpecificationAttributeOptionId
					FROM Product_SpecificationAttribute_Mapping psam
					WHERE psam.AllowFiltering = 1 AND psam.ProductId = p.Id
				)
			)');
	END IF;
	
	-- sorting
	SET @sql_orderby = '';	
	IF OrderBy = 5 THEN /* Name: A to Z */
		SET @sql_orderby = ' p.`Name` ASC';
	ELSEIF OrderBy = 6 THEN /* Name: Z to A */
		SET @sql_orderby = ' p.`Name` DESC';
	ELSEIF OrderBy = 10 THEN /* Price: Low to High */
		SET @sql_orderby = ' pv.`Price` ASC';
	ELSEIF OrderBy = 11 THEN /* Price: High to Low */
		SET @sql_orderby = ' pv.`Price` DESC';
	ELSEIF OrderBy = 15 THEN /* creation date */
		SET @sql_orderby = ' p.`CreatedOnUtc` DESC';
	ELSE /* default sorting, 0 (position) */
		-- category position (display order)
		IF @CategoryIdsCount > 0 THEN
        SET @sql_orderby = ' pcm.DisplayOrder ASC';
    END IF;
		
		-- manufacturer position (display order)
		IF ManufacturerId > 0 THEN
			IF LENGTH(@sql_orderby) > 0 THEN
            SET @sql_orderby = CONCAT(@sql_orderby, ', ');
      END IF;
      
			SET @sql_orderby = CONCAT(@sql_orderby, ' pmm.DisplayOrder ASC');
		END IF;
		
		-- name
		IF LENGTH(@sql_orderby) > 0 THEN
        SET @sql_orderby = CONCAT(@sql_orderby, ', ');
    END IF;
    
		SET @sql_orderby = CONCAT(@sql_orderby, ' p.`Name` ASC');
	END IF;
	
	SET @sql = CONCAT(@sql, '
	ORDER BY', @sql_orderby);
    
    -- select @sql;
	        
	-- PRINT (@sql)
    PREPARE stmt2 FROM @sql; 
    SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    EXECUTE stmt2; 
    SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ;
    DEALLOCATE PREPARE stmt2;
     
	DROP Temporary TABLE FilteredCategoryIds_TempTable;
	DROP Temporary TABLE FilteredSpecs_TempTable;

DROP Temporary TABLE if exists PageIndex_TempTable;
	CREATE Temporary TABLE PageIndex_TempTable 
	(
		IndexId int NOT NULL AUTO_INCREMENT,
		ProductId int NOT NULL,
    PRIMARY KEY (`IndexId`)
	) ENGINE = MEMORY;
    
	INSERT INTO PageIndex_TempTable (`ProductId`)
	SELECT ProductId
	FROM DisplayOrder_TempTable
	GROUP BY ProductId
	ORDER BY min(`Id`);

	-- total records
	SET TotalRecords = ROW_COUNT();
	
	DROP temporary TABLE DisplayOrder_TempTable;

	-- prepare filterable specification attribute option identifier (if requested)
	IF LoadFilterableSpecificationAttributeOptionIds = 1 THEN
		DROP Temporary TABLE if exists FilterableSpecs_TempTable;
		CREATE Temporary TABLE FilterableSpecs_TempTable 
		(
			SpecificationAttributeOptionId int NOT NULL
		) ENGINE = MEMORY;
        
		INSERT INTO FilterableSpecs_TempTable (`SpecificationAttributeOptionId`)
		SELECT DISTINCT `psam`.SpecificationAttributeOptionId
		FROM `Product_SpecificationAttribute_Mapping` `psam`
		WHERE `psam`.`AllowFiltering` = 1
		AND `psam`.`ProductId` IN (SELECT `pi`.ProductId FROM PageIndex_TempTable `pi`);
    
		-- build comma separated list of filterable identifiers
    SELECT GROUP_CONCAT(COALESCE(concat(FilterableSpecificationAttributeOptionIds, ',') , ''), SpecificationAttributeOptionId) into FilterableSpecificationAttributeOptionIds
		FROM FilterableSpecs_TempTable;

    DROP Temporary TABLE FilterableSpecs_TempTable;
 	END IF;

set @sql = concat('
	SELECT
		p.*
	FROM
		PageIndex_TempTable pi
		INNER JOIN Product p on p.Id = pi.ProductId
	WHERE
		pi.IndexId > ', @PageLowerBound, ' AND 
		pi.IndexId < ', @PageUpperBound, '
	ORDER BY
		pi.IndexId
  limit ', @RowsToReturn, ';');
	
    PREPARE stmt3 FROM @sql;
    EXECUTE stmt3;
    DEALLOCATE PREPARE stmt3;
    
	DROP Temporary TABLE PageIndex_TempTable;
END
-- GO



CREATE PROCEDURE `FullText_IsSupported`()
BEGIN	
    -- Not sure how to test for this in MySql or if it's always on
    SELECT 1;
END
-- GO



CREATE PROCEDURE `FullText_Enable`()
BEGIN	
	
    -- These are remarked out because InnoDb doesn't support full text indexes with MySql 5.5 or earlier
    
    -- SELECT COUNT(1) INTO @IndexCount FROM information_schema.statistics 
--   WHERE table_name = 'Product' AND INDEX_NAME = 'IX_PRODUCT_FULLTEXT';
--   
--   if @IndexCount = 0 then
--     CREATE FULLTEXT INDEX `IX_PRODUCT_FULLTEXT` ON `product` ( `Name`, `ShortDescription`, `FullDescription`);
--   end if;	
--     
--     SELECT COUNT(1) INTO @IndexCount FROM information_schema.statistics 
--   WHERE table_name = 'ProductVariant' AND INDEX_NAME = 'IX_ProductVariant_FULLTEXT';
--   
--   if @IndexCount = 0 then
--     CREATE FULLTEXT INDEX `IX_ProductVariant_FULLTEXT` ON `ProductVariant` ( `Description`, `SKU`, `FullDescription`);
--   end if;
-- 
-- SELECT COUNT(1) INTO @IndexCount FROM information_schema.statistics 
--   WHERE table_name = 'LocalizedProperty' AND INDEX_NAME = 'IX_LocalizedProperty_FULLTEXT';
--   
--   if @IndexCount = 0 then
--     CREATE FULLTEXT INDEX `IX_LocalizedProperty_FULLTEXT` ON `LocalizedProperty` ( `LocaleValue` );
--   end if;
END
-- GO



CREATE PROCEDURE `FullText_Disable`()
BEGIN	
	
    -- These are remarked out because InnoDb doesn't currently support full text indexes with MySql 5.5 or earlier
    
    -- SELECT COUNT(1) INTO @IndexCount FROM information_schema.statistics 
--   WHERE table_name = 'Product' AND INDEX_NAME = 'IX_PRODUCT_FULLTEXT';
--   
--   if @IndexCount > 0 then
--     DROP INDEX `IX_PRODUCT_FULLTEXT` ON `product`;
--   end if;	
--     
--     SELECT COUNT(1) INTO @IndexCount FROM information_schema.statistics 
--   WHERE table_name = 'ProductVariant' AND INDEX_NAME = 'IX_ProductVariant_FULLTEXT';
--   
--   if @IndexCount > 0 then
--     DROP INDEX `IX_ProductVariant_FULLTEXT` ON `ProductVariant`;
--   end if;
-- 
-- SELECT COUNT(1) INTO @IndexCount FROM information_schema.statistics 
--   WHERE table_name = 'LocalizedProperty' AND INDEX_NAME = 'IX_LocalizedProperty_FULLTEXT';
--   
--   if @IndexCount > 0 then
--     DROP INDEX `IX_LocalizedProperty_FULLTEXT` ON `LocalizedProperty`;
--   end if;
END
-- GO


CREATE PROCEDURE `LanguagePackImport`(
	IN LanguageId int,
	IN XmlPackage LONGTEXT
)
BEGIN
	IF (EXISTS(SELECT 1 FROM `Language` WHERE `Id` = LanguageId)) THEN
        
    drop temporary table if exists LocaleStringResource_TempTable;
	CREATE temporary TABLE LocaleStringResource_TempTable
	(
		LanguageId int NOT NULL,
				ResourceName nvarchar(200) NOT NULL,
				ResourceValue LONGTEXT NOT NULL
	);

set @i = 1;
select ExtractValue(@xml, 'count(//Language/LocaleResource/@Name)') into @count;
WHILE @i <= @count DO
    insert into LocaleStringResource_TempTable
    SELECT LanguageId, ExtractValue(@xml, '//Language/LocaleResource[$@i]/@Name'), ExtractValue(@xml, '//Language/LocaleResource[$@i]/Value[1]');
    SET @i = @i+1;
END WHILE;		       
        
        BEGIN
        DECLARE done INT DEFAULT FALSE;
        DECLARE ResourceName nvarchar(200);
		DECLARE ResourceValue LONGTEXT;
		DECLARE cur_localeresource CURSOR FOR
		SELECT LanguageID, LocaleStringResource_TempTable.ResourceName, LocaleStringResource_TempTable.ResourceValue
		FROM LocaleStringResource_TempTable;
        DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
        
		OPEN cur_localeresource;
    
    read_loop: LOOP
    FETCH cur_localeresource INTO LanguageId, ResourceName, ResourceValue;
    IF done THEN
      LEAVE read_loop;
    END IF;
    
    -- select LanguageId, ResourceName, ResourceValue;
    
    IF (EXISTS (SELECT 1 FROM LocaleStringResource WHERE LocaleStringResource.LanguageID=LanguageId AND LocaleStringResource.ResourceName=ResourceName)) THEN
				UPDATE LocaleStringResource
				SET LocaleStringResource.ResourceValue=ResourceValue
				WHERE LocaleStringResource.LanguageID=LanguageId AND LocaleStringResource.ResourceName=ResourceName;
			ELSE 
				INSERT INTO LocaleStringResource
				(
					LocaleStringResource.LanguageId,
					LocaleStringResource.ResourceName,
					LocaleStringResource.ResourceValue
				)
				VALUES
				(
					LanguageId,
					ResourceName,
					ResourceValue
				);
			END IF;
  END LOOP;
  
  CLOSE cur_localeresource;
		-- DEALLOCATE cur_localeresource;
			END;		

		DROP temporary TABLE LocaleStringResource_TempTable;
	END IF;
END
-- GO