IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetVersions]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 22 Feb 2013 at 2pm
-- Description:	To get all versions and Cra details for API of Carnation 
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetVersions]
	
AS
BEGIN
		
	SELECT VersionId,Car 
	FROM vwMMV
	
END