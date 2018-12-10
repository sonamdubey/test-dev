IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsites_SaveBannerImage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsites_SaveBannerImage]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 2/03/2012
-- Description:	Delete Banner Images uploaded by user for microsites home page
-- =============================================
CREATE PROCEDURE [dbo].[Microsites_SaveBannerImage] 
	-- Add the parameters for the stored procedure here
	(
		@PhotoId int,
		@LandingURL varchar(200)
	)
AS
BEGIN
    -- Insert statements for procedure here
    
    update Microsite_Images set LandingURL=@LandingURL where id =@PhotoId
	
END