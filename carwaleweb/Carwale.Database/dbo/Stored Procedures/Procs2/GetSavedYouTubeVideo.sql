IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetSavedYouTubeVideo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetSavedYouTubeVideo]
GO

	
-- =============================================
-- Author:		amit verma
-- Create date: 10/31/201.
-- Description:	Get saved youtube video key for used car listing
-- =============================================
CREATE PROCEDURE [dbo].[GetSavedYouTubeVideo]
	-- Add the parameters for the stored procedure here
	@CarId NUMERIC(18,0),
	@VideoKey VARCHAR(20) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @VideoKey = YoutubeVideo FROM CustomerSellInquiryDetails WITH(NOLOCK)
	WHERE InquiryId = @CarId
END


