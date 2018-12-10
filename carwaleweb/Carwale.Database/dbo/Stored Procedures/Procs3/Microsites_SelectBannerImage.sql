IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsites_SelectBannerImage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsites_SelectBannerImage]
GO

	
-----------------------------------------------
-- =============================================
-- Author:		Umesh Ojha
-- Create date: 29/02/2012
-- Description:	Bind Banner Images uploaded by user for microsites home page
-- Modified by : Komal Manjare on 7th july 2015
-- OriginalImgPath fetched
-- =============================================
CREATE PROCEDURE [dbo].[Microsites_SelectBannerImage] 
	-- Add the parameters for the stored procedure here
	(
		@DealerId int
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id,HostURL+DirectoryPath+ThumbImage as ImagePath,LandingURL,HostURL,OriginalImgPath
    FROM Microsite_Images
	WHERE DealerId=@DealerId AND IsActive = 1 AND IsBanner =1  ORDER BY BannerImgSortingOrder asc
END