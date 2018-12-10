IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetFieldForPriceQuotes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetFieldForPriceQuotes]
GO
	
/*
	Author:Vishal Srivastava AE1830
	Date:04/10/2013
	Description: Bind field of repeater made
*/
CREATE PROCEDURE [dbo].[TC_GetFieldForPriceQuotes]
@modelId NUMERIC
AS
BEGIN

	SELECT TC_PQFieldMasterId, Field
	FROM TC_PQFieldMaster as m WITH(NOLOCK)
	WHERE M.IsCompulsory=1
	AND M.IsActive=1

	SELECT VersionId, [Version] FROM vwMMV WITH(NOLOCK) WHERE ModelId=@modelId AND IsVerionNew=1

	SELECT TC_PQFieldMasterId, Field 
	FROM TC_PQFieldMaster AS m WITH(NOLOCK)
	WHERE IsCompulsory=0
	and IsActive=1

END
