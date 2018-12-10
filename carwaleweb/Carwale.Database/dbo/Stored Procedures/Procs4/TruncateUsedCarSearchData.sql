IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TruncateUsedCarSearchData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TruncateUsedCarSearchData]
GO

	
CREATE PROCEDURE [dbo].[TruncateUsedCarSearchData]
AS
BEGIN

	TRUNCATE TABLE UCS_SearchResult
	TRUNCATE TABLE UCS_SearchCriteria

END
