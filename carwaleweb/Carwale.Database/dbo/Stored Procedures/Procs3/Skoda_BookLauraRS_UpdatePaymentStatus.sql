IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Skoda_BookLauraRS_UpdatePaymentStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Skoda_BookLauraRS_UpdatePaymentStatus]
GO

	
-- =============================================
-- Author:		Satish Sharma
-- Create date: 29/8/2011 4:48 PM
-- Description:	To update payment status of Booking Laura RS. This SP will be called after successful transaction.
-- =============================================
CREATE PROCEDURE [dbo].[Skoda_BookLauraRS_UpdatePaymentStatus]
	-- Add the parameters for the stored procedure here
	@BookingId INT,
	@IsPaymentSuccessful Bit,
	@Status Bit = 0 Output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF(@BookingId IS NOT NULL AND @BookingId != -1 AND @IsPaymentSuccessful = 1)
	BEGIN
		UPDATE Skoda_LauraRSBooking SET IsPaymentSuccessful = @IsPaymentSuccessful WHERE ID = @BookingId
		SET @Status = 1
	END
END

