IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Booking_Payment_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Booking_Payment_SP]
GO

	  
-- =============================================
-- Author:		Binumon George
-- Create date: 02-09-2011
-- Description:	Insert and update booking payment details to TC_PaymentReceived table
-- =============================================
CREATE PROCEDURE [dbo].[TC_Booking_Payment_SP] 
	-- Add the parameters for the stored procedure here
	@TC_PaymentReceived_Id INT=NULL,
	@TC_CarBooking_Id INT ,
	@TC_PaymentOptions_Id INT,
	@UserId INT,
	@AmountReceived DECIMAL,
	@PaymentType TINYINT,
	@PayDate DATE,
	@ChequeNo VARCHAR(20),
	@BankName VARCHAR(50),
	@Remarks VARCHAR(100),
	@CreditCardNo VARBINARY(50),
	@CVV VARBINARY(50),
	@CardExpiry DATE,
	@CardBank VARCHAR(50),
	@Status INT OUTPUT 
AS
BEGIN
	SET @Status=0
	IF(@TC_PaymentReceived_Id IS NULL)
		BEGIN-- For Inserting new payment
			INSERT INTO TC_PaymentReceived(TC_CarBooking_Id, TC_PaymentOptions_Id, UserId, AmountReceived, PaymentType, 
			PayDate, ChequeNo, BankName, Remarks)
			
			VALUES(@TC_CarBooking_Id, @TC_PaymentOptions_Id, @UserId, @AmountReceived, @PaymentType,
			 @PayDate, @ChequeNo, @BankName, @Remarks)
			SET @Status=1
		END
	ELSE
		BEGIN--For udating payment
		 
				UPDATE TC_PaymentReceived SET TC_CarBooking_Id=@TC_CarBooking_Id,
				 TC_PaymentOptions_Id=@TC_PaymentOptions_Id,
				 UserId=@UserId,
				 AmountReceived=@AmountReceived,
				 PaymentType=@PaymentType,
				 PayDate=@PayDate,
				 ChequeNo=@ChequeNo,
				 BankName=@BankName,
				 Remarks=@Remarks
				WHERE TC_PaymentReceived_Id=@TC_PaymentReceived_Id
				SET @Status=2
		END
END