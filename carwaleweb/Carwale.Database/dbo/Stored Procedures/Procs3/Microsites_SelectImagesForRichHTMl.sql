IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsites_SelectImagesForRichHTMl]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsites_SelectImagesForRichHTMl]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 29/02/2012
-- Description:	Seelect Images for Dragging in RicH HTML text control  by user for microsites home page
-- =============================================
CREATE PROCEDURE [dbo].[Microsites_SelectImagesForRichHTMl] 
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
	SELECT Id,HostURL+DirectoryPath+ThumbImage as ImagePath,LandingURL,LargeImage FROM Microsite_Images
	WHERE DealerId=@DealerId AND IsActive = 1  and ThumbImage !=''
END


 