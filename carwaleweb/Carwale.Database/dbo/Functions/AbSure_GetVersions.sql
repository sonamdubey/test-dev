IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetVersions]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[AbSure_GetVersions]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: July 20,2015
-- Description:	To get all versions of given Model
-- =============================================
CREATE FUNCTION [dbo].[AbSure_GetVersions]
(
	@ModelId INT
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE	@VersionSet	VARCHAR(MAX) = NULL
	SELECT	@VersionSet = (COALESCE(@VersionSet + ', ', '') +  CAST(V.ID AS VARCHAR(500)) + '_' + CAST(V.Name AS VARCHAR(500)) ) 
	FROM	CarVersions V 
	WHERE	CarModelId = @ModelId
			AND Futuristic <> 1 AND (Used = 1 OR New = 1)
	ORDER BY V.Name 
	RETURN	@VersionSet
END



----------------------------------------------------------------------------------------------------------------------------------------------------

