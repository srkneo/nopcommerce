﻿IF NOT EXISTS (
    SELECT 1
    FROM [dbo].[Nop_CustomerAction]
    WHERE [SystemKeyword] = N'ManageEmailSettings')
BEGIN
  INSERT [dbo].[Nop_CustomerAction] ([Name], [SystemKeyword], [Comment], [DisplayOrder])
  VALUES (N'Manage Email Settings', N'ManageEmailSettings', N'',20)
END
GO 

UPDATE [dbo].[Nop_Currency]
SET [CustomFormatting]=N'€0.00'
WHERE [CurrencyCode]=N'EUR'
GO