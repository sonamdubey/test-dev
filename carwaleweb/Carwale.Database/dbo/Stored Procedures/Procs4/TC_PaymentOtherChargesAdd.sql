IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_PaymentOtherChargesAdd]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_PaymentOtherChargesAdd]
GO

	
-- =============================================
-- Author:		Binumon George
-- Create date: 25-08-2011
-- Description:	Insert and update TC_PaymentIncludes table
-- =============================================
CREATE PROCEDURE [dbo].[TC_PaymentOtherChargesAdd] 
	-- Add the parameters for the stored procedure here
	@TC_PaymentOtherCharges_Id INT =null,
	@TC_CarBooking_Id INT ,
	@Amount DECIMAL,
	@TC_PaymentVariables_Id INT,
	@comments VARCHAR(100),
	@Status INT OUTPUT 
AS
BEGIN
	SET @Status=-1
	IF(@TC_PaymentOtherCharges_Id IS NULL)
		BEGIN-- For Inserting new payment
		  --checking below under one car booking same charges repeating
			IF NOT EXISTS(SELECT TC_PaymentOtherCharges_Id FROM TC_PaymentOtherCharges WHERE TC_PaymentVariables_Id=@TC_PaymentVariables_Id AND TC_CarBooking_Id=@TC_CarBooking_Id)
				BEGIN
					INSERT INTO TC_PaymentOtherCharges(TC_CarBooking_Id, TC_PaymentVariables_Id, Amount,Comments)
					VALUES(@TC_CarBooking_Id,@TC_PaymentVariables_Id,@Amount,@comments)
					SET @Status=SCOPE_IDENTITY()
					-- Adding other charge to total amount of stock and netpayment
					UPDATE TC_CarBooking set TotalAmount=TotalAmount + @Amount, NetPayment=NetPayment+ @Amount WHERE TC_CarBookingId=@TC_CarBooking_Id					
				END
			ELSE
				BEGIN
					SET @Status=-2--under this carbooking this charge already applied
				END
		END
	ELSE
		BEGIN--For udating payment
		 --checking below under one car booking same charges repeating
			IF NOT EXISTS(SELECT TC_PaymentOtherCharges_Id FROM TC_PaymentOtherCharges WHERE TC_PaymentVariables_Id=@TC_PaymentVariables_Id AND TC_CarBooking_Id=@TC_CarBooking_Id AND TC_PaymentOtherCharges_Id<>@TC_PaymentOtherCharges_Id)
			BEGIN
				UPDATE TC_PaymentOtherCharges SET TC_CarBooking_Id=@TC_CarBooking_Id, TC_PaymentVariables_Id=@TC_PaymentVariables_Id, Amount=@Amount, Comments=@comments
				WHERE TC_PaymentOtherCharges_Id=@TC_PaymentOtherCharges_Id
				SET @Status=0
			END
		ELSE
			BEGIN
				SET @Status=-3--under this carbooking this charge already applied
			END
		END
END

