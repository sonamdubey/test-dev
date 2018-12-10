IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_FetchWebsitePages ]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_FetchWebsitePages ]
GO

	-- =============================================
-- Author:		Komal Manjare 
-- Create date: 01 August 2016
-- Description:	fetch website pages to be bind to dropdown
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_FetchWebsitePages ]
	
AS
BEGIN
	SELECT Id,PageName AS Value
	FROM Microsite_WebsitePages WITH(NOLOCK)
	WHERE IsActive=1
END

