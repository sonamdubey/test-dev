IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveYouTubeVideo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveYouTubeVideo]
GO

	-- =============================================
-- Author:		amit verma
-- Create date: 10/31/201.
-- Description:	Save youtube video key for used car listing
-- =============================================
CREATE PROCEDURE [dbo].[SaveYouTubeVideo]
	-- Add the parameters for the stored procedure here
	@VideoKey VARCHAR(20),
    @CarId NUMERIC(18,0)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CustomerSellInquiryDetails
	SET YoutubeVideo = (CASE @VideoKey WHEN '' THEN NULL ELSE @VideoKey END), IsYouTubeVideoApproved = 0
	WHERE InquiryId = @CarId AND (YoutubeVideo != @VideoKey OR YoutubeVideo IS NULL)
END

