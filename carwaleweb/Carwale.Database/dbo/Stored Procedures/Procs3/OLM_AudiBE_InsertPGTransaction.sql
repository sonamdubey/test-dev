IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_InsertPGTransaction]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_InsertPGTransaction]
GO

	
-- =============================================
-- Author:		Vaibhav K
-- Create date: 9-8-2013
-- Description:	Insert a new entry for Payment Gateway transaction for Audi Booking transaction
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AudiBE_InsertPGTransaction]
	-- Add the parameters for the stored procedure here
	@TransactionId			numeric,
	@CustomerId				numeric,
	@ClientIP				varchar(150) = NULL,
	@UserAgent				varchar(500) = NULL,
	@Amount					numeric,
	@PaymentMode			int,
	@PaymentType			int,	
	@ResponseCode			numeric = NULL,
	@ResponseMsg			varchar(500) = NULL,
	@EPGTransactionId		varchar(100) = NULL,
	@AuthId					varchar(100) = NULL,
	@ProcessCompleted		bit = 0,
	@TransactionCompleted	bit = 0,
	@PGTransactionId		numeric output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @PGTransactionId = -1
	DECLARE @TransactionDate DATETIME 
	SET @TransactionDate = GETDATE()
    -- Insert statements for procedure here
	INSERT INTO OLM_AudiBE_PGTransactions
		( 
			TransactionId, CustomerId, ClientIP, UserAgent, Amount, PaymentMode, PaymentType,
			EntryDate, ResponseCode, ResponseMsg, EPGTransactionId, AuthId, 
			ProcessCompleted, TransactionCompleted
		)
	VALUES
		( 
			@TransactionId, @CustomerId, @ClientIP, @UserAgent, @Amount, @PaymentMode, @PaymentType,
			@TransactionDate, @ResponseCode, @ResponseMsg, @EPGTransactionId, @AuthId, 
			@ProcessCompleted, @TransactionCompleted
		)
		
	SET @PGTransactionId = SCOPE_IDENTITY()
	
	--update audi booking transaction for the pg transaction id
	UPDATE OLM_AudiBE_Transactions
	SET 
		PGTransactionId = @PGTransactionId,
		PaymentMode = @PaymentMode,
		PaymentType = @PaymentType,
		Amount = @Amount,
		TransactionDate = @TransactionDate
	WHERE Id = @TransactionId
END

