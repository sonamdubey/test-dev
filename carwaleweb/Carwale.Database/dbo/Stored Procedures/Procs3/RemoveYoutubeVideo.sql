IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RemoveYoutubeVideo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RemoveYoutubeVideo]
GO

	
-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 17 Dec 2013
-- Description:	Proc to remove the youtube video uploaded by customer
-- =============================================
CREATE PROCEDURE [dbo].[RemoveYoutubeVideo]
	-- Add the parameters for the stored procedure here
	@InquiryId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CustomerSellInquiryDetails
	SET YoutubeVideo = NULL, IsYouTubeVideoApproved = 0
	WHERE InquiryId = @InquiryId
END

