IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingPremiumSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingPremiumSave]
GO

	-- =============================================
-- Author:		Binumon George	
-- Create date: 20 Oct 2011
-- Description:	save premium details for booking
-- =============================================
CREATE PROCEDURE [dbo].[TC_BookingPremiumSave]
(
@StockId INT ,
@DealerId INT,
@Amount INT=NULL,
@Status INT OUTPUT
)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET @Status=3
	SET NOCOUNT ON;
	IF EXISTS(SELECT Id	FROM TC_Stock WHERE Id=@StockId AND BranchId=@DealerId)--checking valid stock id and dealerid
		BEGIN
			DECLARE @TC_CarBooking_Id INT
			SELECT @TC_CarBooking_Id=TC_CarBookingId FROM TC_Carbooking WHERE stockId=@StockId--taking bookingid as per stockid
			IF(@TC_CarBooking_Id IS NOT NULL) --checking car stock booked or not. if yes continuing process
				BEGIN
					--checking here insurance appiled  to this booking id or not if yes, updating else inserting
					DECLARE @OldPremium INT=0-- this store old insurance paid to to stock
					SELECT @OldPremium=ISNULL(Amount,0) FROM TC_PaymentOtherCharges WHERE TC_CarBooking_Id=@TC_CarBooking_Id AND TC_PaymentVariables_Id=3
					IF EXISTS(SELECT TC_CarBooking_Id FROM TC_PaymentOtherCharges WHERE TC_CarBooking_Id=@TC_CarBooking_Id AND TC_PaymentVariables_Id=3 )
						BEGIN
							UPDATE TC_PaymentOtherCharges SET Amount=@Amount WHERE TC_CarBooking_Id=@TC_CarBooking_Id AND TC_PaymentVariables_Id=3
							--updating totalamt and stockamt with premium amt
							UPDATE TC_CarBooking set TotalAmount=((TotalAmount + @Amount)-@OldPremium), NetPayment=((NetPayment + @Amount)-@OldPremium) WHERE TC_CarBookingId=@TC_CarBooking_Id
							SET @Status=2--updated
						END
						ELSE
						BEGIN
							INSERT TC_PaymentOtherCharges(TC_CarBooking_Id,Amount,TC_PaymentVariables_Id)VALUES(@TC_CarBooking_Id,@Amount,3)
							--updating totalamt and stockamt with premium amt
							--UPDATE TC_CarBooking set TotalAmount=((TotalAmount + @Amount)-@OldPremium), NetPayment=((NetPayment + @Amount)-@OldPremium) WHERE TC_CarBookingId=@TC_CarBooking_Id
							UPDATE TC_CarBooking set TotalAmount=((TotalAmount + @Amount)-@OldPremium), NetPayment=((NetPayment + @Amount)-@OldPremium) WHERE TC_CarBookingId=@TC_CarBooking_Id
							SET @Status=1--saved
						END	
				END
		END
END

/****** Object:  StoredProcedure [dbo].[TC_BookingInsuranceView]    Script Date: 11/10/2011 17:51:08 ******/
SET ANSI_NULLS ON
