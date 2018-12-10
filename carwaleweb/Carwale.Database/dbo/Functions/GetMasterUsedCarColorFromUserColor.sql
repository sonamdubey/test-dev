IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMasterUsedCarColorFromUserColor]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[GetMasterUsedCarColorFromUserColor]
GO

	---Created By: Manish Chourasiya on 22-09-2014 
---Desc : Will return the Used car master color id corresponding to user entered color value.
CREATE FUNCTION [dbo].[GetMasterUsedCarColorFromUserColor] ( @UserEnteredColor VARCHAR(75))
RETURNS  INT
AS
BEGIN
DECLARE @UsedCarMasterColorId INT
				
 	SELECT @UsedCarMasterColorId=UsedCarMasterColorsId   
	FROM UsedCarMasterColors
	 WHERE ColorName=( CASE 	
	       WHEN SOUNDEX(@UserEnteredColor) = SOUNDEX('White')	
				OR @UserEnteredColor LIKE '%Whi%'
				OR @UserEnteredColor LIKE 'Whi%'
				OR @UserEnteredColor LIKE  '%Ivo%'
				OR @UserEnteredColor LIKE '%Pearl%'
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 1)) = SOUNDEX('White')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 2)) = SOUNDEX('White')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 3)) = SOUNDEX('White')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 4)) = SOUNDEX('White')
				THEN 'White'	
			WHEN SOUNDEX(@UserEnteredColor) = SOUNDEX('Silver')	
				OR @UserEnteredColor LIKE '%Silv%'
				OR @UserEnteredColor LIKE '%Platinum%'
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 1)) = SOUNDEX('Silver')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 2)) = SOUNDEX('Silver')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 3)) = SOUNDEX('Silver')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 4)) = SOUNDEX('Silver')
				THEN 'Silver'
			WHEN SOUNDEX(@UserEnteredColor) = SOUNDEX('grey')	
				OR @UserEnteredColor LIKE '%Grey%'
				OR @UserEnteredColor LIKE '%Steel%'
				OR @UserEnteredColor LIKE  '%Gray%'
				OR @UserEnteredColor LIKE  '%Grey%'
				OR @UserEnteredColor LIKE   '%Charc%'
				OR @UserEnteredColor LIKE   '%Mica%' 
				OR @UserEnteredColor LIKE   '%Graphite%'
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 1)) = SOUNDEX('Grey')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 2)) = SOUNDEX('Grey')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 3)) = SOUNDEX('Grey')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 4)) = SOUNDEX('Grey')
				THEN 'Grey'	
			WHEN SOUNDEX(@UserEnteredColor) = SOUNDEX('Blue')	
				OR @UserEnteredColor LIKE '%Blue%'
				OR 	@UserEnteredColor LIKE  '%Blue%'
				OR 	@UserEnteredColor LIKE  '%Bluie%'
				OR 	@UserEnteredColor LIKE  '%Mist%'
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 1)) = SOUNDEX('Blue')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 2)) = SOUNDEX('Blue')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 3)) = SOUNDEX('Blue')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 4)) = SOUNDEX('Blue')
				THEN 'Blue'
			WHEN SOUNDEX(@UserEnteredColor) = SOUNDEX('Red')	
				OR @UserEnteredColor LIKE '%Red%'
				OR @UserEnteredColor LIKE '%cherry%'
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 1)) = SOUNDEX('Red')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 2)) = SOUNDEX('Red')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 3)) = SOUNDEX('Red')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 4)) = SOUNDEX('Red')
				THEN 'Red'
			
			WHEN SOUNDEX(@UserEnteredColor) = SOUNDEX('Black')	
				OR @UserEnteredColor LIKE '%Black%'
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 1)) = SOUNDEX('Black')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 2)) = SOUNDEX('Black')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 3)) = SOUNDEX('Black')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 4)) = SOUNDEX('Black')
				THEN 'Black'
			
				
			WHEN SOUNDEX(@UserEnteredColor) = SOUNDEX('Beige')	
				OR @UserEnteredColor LIKE '%Be%ge%'
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 1)) = SOUNDEX('Beige')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 2)) = SOUNDEX('Beige')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 3)) = SOUNDEX('Beige')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 4)) = SOUNDEX('Beige')
				THEN 'Beige'
			WHEN SOUNDEX(@UserEnteredColor) = SOUNDEX('Brown')	
				OR @UserEnteredColor LIKE '%Brown%'
				OR @UserEnteredColor LIKE '%Choc%'
				OR @UserEnteredColor LIKE  '%Choc%'
				OR @UserEnteredColor LIKE  '%Bronze%'
				OR @UserEnteredColor LIKE  '%Copper%'
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 1)) = SOUNDEX('Brown')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 2)) = SOUNDEX('Brown')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 3)) = SOUNDEX('Brown')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 4)) = SOUNDEX('Brown')
				THEN 'Brown'
			WHEN SOUNDEX(@UserEnteredColor) = SOUNDEX('Gold')	
				OR @UserEnteredColor LIKE '%Gold%'
				OR @UserEnteredColor LIKE '%champaigne%' 
				OR @UserEnteredColor LIKE  '%champagne%'
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 1)) = SOUNDEX('Gold')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 2)) = SOUNDEX('Gold')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 3)) = SOUNDEX('Gold')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 4)) = SOUNDEX('Gold')
				THEN 'Gold / Yellow'  --'Gold'
			WHEN SOUNDEX(@UserEnteredColor) = SOUNDEX('Yellow')	
				OR @UserEnteredColor LIKE  '%Yellow%' -- '%Yellow%'
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 1)) = SOUNDEX('Yellow')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 2)) = SOUNDEX('Yellow')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 3)) = SOUNDEX('Yellow')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 4)) = SOUNDEX('Yellow')
				THEN 'Gold / Yellow' 
			WHEN SOUNDEX(@UserEnteredColor) = SOUNDEX('Green')	
				OR @UserEnteredColor LIKE '%Green%'
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 1)) = SOUNDEX('Green')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 2)) = SOUNDEX('Green')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 3)) = SOUNDEX('Green')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 4)) = SOUNDEX('Green')
				THEN 'Green'
			WHEN SOUNDEX(@UserEnteredColor) = SOUNDEX('Purple')	
				OR @UserEnteredColor LIKE '%Purple%'
				OR @UserEnteredColor LIKE '%violet%' 
				OR @UserEnteredColor LIKE  '%lavender%'
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 1)) = SOUNDEX('Purple')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 2)) = SOUNDEX('Purple')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 3)) = SOUNDEX('Purple')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 4)) = SOUNDEX('Purple')
				THEN 'Purple'
			WHEN SOUNDEX(@UserEnteredColor) = SOUNDEX('Maroon')	
				OR @UserEnteredColor LIKE '%Maroon%'
				OR @UserEnteredColor LIKE  '%Burgundy%'
				OR @UserEnteredColor LIKE  '%Burgendy%' 
				OR @UserEnteredColor LIKE  '%Burgundi%' 
				OR @UserEnteredColor LIKE   '%Crimson%'
				OR @UserEnteredColor LIKE   '%wine%'
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 1)) = SOUNDEX('Maroon')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 2)) = SOUNDEX('Maroon')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 3)) = SOUNDEX('Maroon')
				OR SOUNDEX(PARSENAME(REPLACE(@UserEnteredColor, ' ', '.'), 4)) = SOUNDEX('Maroon')
				THEN 'Maroon'
			
			ELSE 'Others'	
			END )
			


RETURN @UsedCarMasterColorId;
END
