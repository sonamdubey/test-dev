IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AP_ForumSearchData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AP_ForumSearchData]
GO

	CREATE PROCEDURE [dbo].[AP_ForumSearchData]
	@DateFrom	DateTime
 AS
	
BEGIN
	DELETE FROM ForumSearchResults 
	WHERE SearchId <= (SELECT Max(Id) FROM ForumSearches WHERE SearchDateTime < @DateFrom)
END


