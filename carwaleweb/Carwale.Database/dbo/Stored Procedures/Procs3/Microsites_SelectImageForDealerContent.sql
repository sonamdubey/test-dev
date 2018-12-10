IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsites_SelectImageForDealerContent]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsites_SelectImageForDealerContent]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 12/03/2012
-- Description:	Select Large image for dragging into rich HTML input text by user for microsites home page
-- =============================================
CREATE PROCEDURE [dbo].[Microsites_SelectImageForDealerContent] 
	-- Add the parameters for the stored procedure here
	(
		@ImgId int
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id,HostURL+DirectoryPath+LargeImage as ImagePath FROM Microsite_Images
	WHERE id=@ImgId
END
