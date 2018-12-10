IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SMBE_ConfirmationDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SMBE_ConfirmationDetails]
GO

	-- =============================================
-- Author:	Mihir Chheda
-- Create date: 19-Dec-2013
-- Description:	Saves variant details 
-- =============================================
CREATE PROCEDURE OLM_SMBE_ConfirmationDetails @Id NUMERIC OUTPUT
	,@IsPaymentSuccessful BIT
	,@OutletName VARCHAR(50)
	,@SkodaBookingId VARCHAR(50)
	,@LeadTokenNo VARCHAR(50)
	,@TokenDateTime DATETIME
	,@PushSuccess BIT
	,@PushErrorMsg VARCHAR(2000)
AS
BEGIN
	UPDATE OLM_BookingData
	SET IsPaymentSuccessful = @IsPaymentSuccessful
		,OutletName = @OutletName
		,SkodaBookingId = @SkodaBookingId
		,LeadTokenNo = @LeadTokenNo
		,TokenDateTime = @TokenDateTime
		,PushSuccess = @PushSuccess
		,PushErrorMsg = @PushErrorMsg
	WHERE Id = @Id
END
