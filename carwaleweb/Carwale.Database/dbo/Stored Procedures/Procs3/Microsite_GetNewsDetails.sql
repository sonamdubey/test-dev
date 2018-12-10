IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetNewsDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetNewsDetails]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 4/7/2012
-- Description:	Fetching data for news details showing in the news details page
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_GetNewsDetails] 
	-- Add the parameters for the stored procedure here
	@BasicId BigInt
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select AuthorName, DisplayDate,Title, MainImageSet, HostURL, 
	(select Data from Con_EditCms_PageContent 
	where PageId = (select id from Con_EditCms_Pages where BasicId = @BasicId))As Description  
	from Con_EditCms_Basic CB where Id=@BasicId
	
END