IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCharacters]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[GetCharacters]
GO

	
CREATE FUNCTION [dbo].[GetCharacters](@mainString varchar(500), @validChars varchar(100))
RETURNS varchar(500) AS
BEGIN
 
	While @mainString like '%[^' + @validChars + ']%'
		Select @mainString = replace(@mainString,substring(@mainString,patindex('%[^' + @validChars + ']%',@mainString),1),'')

	Return @mainString
END

