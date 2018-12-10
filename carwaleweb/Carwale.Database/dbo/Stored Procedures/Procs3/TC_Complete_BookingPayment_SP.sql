IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Complete_BookingPayment_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Complete_BookingPayment_SP]
GO

	
-- =============================================
-- Author:		Binumon George
-- Create date: 12-09-2011
-- Description:	complete the booking payment
-- =============================================
CREATE PROCEDURE [dbo].[TC_Complete_BookingPayment_SP] 
	@StockId INT,
	@status INT OUTPUT
AS
BEGIN
	DECLARE @CarBooingId  NUMERIC
	DECLARE @AmountReceived NUMERIC
	SET @status=0
		IF(@StockId IS NOT NULL)
			BEGIN
				--below retriving carbooking_id from TC_CarBooking according to stock_id 
				SELECT @CarBooingId = TC_CarBookingId FROM TC_CarBooking WHERE StockId=@StockId
				IF(@CarBooingId IS NOT NULL)
					BEGIN
						-- taking sum of amount paid to carbooking
						SELECT @AmountReceived=SUM(AmountReceived) FROM  TC_PaymentReceived WHERE TC_CarBooking_Id=@CarBooingId
						--checking amount paid above or equal to net price of car 
						IF(@AmountReceived >=(SELECT NetPayment FROM TC_CarBooking WHERE TC_CarBookingId=@CarBooingId))
							BEGIN
								--if amount paid equal or above updating TC_CarBooking payment is finished
								UPDATE TC_CarBooking SET IsCompleted=1 WHERE TC_CarBookingId=@CarBooingId
								SET @status=1--Payment finished
								--select 'completed'
							END
						ELSE
							BEGIN
								SET @status=2--payment not completed
								--print @AmountReceived
							END
					END
			END
			
END