IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetVersionColors]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[AbSure_GetVersionColors]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: July 20,2015
-- Description:	To get all colors of given version
-- =============================================
CREATE FUNCTION [dbo].[AbSure_GetVersionColors]
(
	@VersionId INT
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE	@ColorArray	VARCHAR(MAX) = NULL
	SELECT	@ColorArray = (COALESCE(@ColorArray + ', ', '') +  CAST(V.VersionColorsId AS VARCHAR(500)) + '_' + CAST(V.VersionColor AS VARCHAR(500)) + '_' + CAST(V.VersionHexCode AS VARCHAR(500))) 
	FROM	vwAllVersionColors V 
	WHERE	V.VersionId = @VersionId
	ORDER BY V.VersionColor 
	RETURN	@ColorArray
END


--ruchira---------------------------------------------------------------------------------------------------------------------

------------------------






--------------------------------------------------------------------------------------------------------------------------


