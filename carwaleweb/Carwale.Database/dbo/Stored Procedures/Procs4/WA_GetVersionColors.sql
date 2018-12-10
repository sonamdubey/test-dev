IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_GetVersionColors]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_GetVersionColors]
GO
	
/*
Author:Rakesh Yadav
Date: 02/07/2013
Desc : Get Other all available colors of version
*/
CREATE  PROCEDURE [dbo].[WA_GetVersionColors]
@versionId INT
AS
BEGIN
	SELECT 
	Color,HexCode 
	FROM VersionColors 
	WHERE 
	CarVersionID=@versionId AND IsActive=1 
END
