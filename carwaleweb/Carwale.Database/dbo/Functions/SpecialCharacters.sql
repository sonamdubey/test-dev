IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SpecialCharacters]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[SpecialCharacters]
GO

	
-- ==========================================================================================
-- Author: Avishkar
-- Create date: 30/10/2012
-- Description: Removes any characters from @myString that do not meet the provided criteria.
-- ==========================================================================================
CREATE FUNCTION [dbo].[SpecialCharacters](@iString varchar(300), @validChars varchar(50))
RETURNS varchar(300) AS
BEGIN

While @iString like '%[^' + @validChars + ']%'
Select @iString = replace(@iString,substring(@iString,patindex('%[^' + @validChars + ']%',@iString),1),'')

Return @iString
END

