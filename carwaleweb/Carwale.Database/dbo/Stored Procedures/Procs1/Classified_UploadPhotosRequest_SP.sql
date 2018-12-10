IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_UploadPhotosRequest_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_UploadPhotosRequest_SP]
GO

	
-- =============================================
-- Author:		Satish Sharma
-- Create date: 23/8/2010 9:10PM Thu
-- Description:	SP to store buyer request to seller to upload photos
-- =============================================
CREATE PROCEDURE [dbo].[Classified_UploadPhotosRequest_SP]
	-- Add the parameters for the stored procedure here
	@SellInquiryId		NUMERIC,
	@BuyerId			NUMERIC,
	@ConsumerType		TINYINT,
	@BuyerMessage		VARCHAR(200)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT SellInquiryId FROM Classified_UploadPhotosRequest WHERE SellInquiryId = @SellInquiryId AND BuyerId = @BuyerId AND ConsumerType = @ConsumerType)
	BEGIN
		INSERT INTO Classified_UploadPhotosRequest(SellInquiryId, BuyerId, ConsumerType, BuyerMessage, RequestDateTime)
		VALUES(@SellInquiryId, @BuyerId, @ConsumerType, @BuyerMessage, GETDATE())
	END
END

