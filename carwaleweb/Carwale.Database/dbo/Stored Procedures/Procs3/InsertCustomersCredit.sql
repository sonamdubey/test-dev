IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertCustomersCredit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertCustomersCredit]
GO

	




--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR CustomersCredit TABLE
--ID, CustomerId, Amount, ReceiptNo, ReceiptDate, EntryDate, ModeId, Description, Encashed, IsActive

CREATE PROCEDURE [dbo].[InsertCustomersCredit]
	@Id			NUMERIC,		-- Id. Will be -1 if Its Insertion
	@AccountId		NUMERIC,		
	@Amount		NUMERIC,
	@ReceiptNo		NUMERIC,	
	@ReceiptDate		DATETIME,
	@EntryDate		DATETIME,
	@ModeId		NUMERIC,
	@Description		VARCHAR(200),
	@BankName		VARCHAR(50),
	@ChequeNo		VARCHAR(50),
	@EncashedDate	DATETIME,
	@ReceivedById		BIT,
	@CreditId		NUMERIC OUTPUT
	
	
 AS
	DECLARE @DefaultEncashed	AS BIT	
BEGIN
					
	IF @Id = -1 -- Insertion
		
		BEGIN
			SELECT @DefaultEncashed = DefaultEncashedValue FROM PaymentModes WHERE ID = @ModeId
				
			INSERT INTO CustomerCredits
				(
					 AccountId, 		Amount, 		ReceiptNo, 		ReceiptDate, 
					 EntryDate, 		ModeId, 		Description, 		BankName ,
					 ChequeNo,		Encashed, 		EncashedDate,		RecievedBy
				 )
			 VALUES 
				(
					 @AccountId, 		@Amount, 		@ReceiptNo, 		@ReceiptDate, 
					 @EntryDate, 		@ModeId, 		@Description, 		@BankName,
					@ChequeNo,		@DefaultEncashed,	@EncashedDate, 	@ReceivedById
					  	
				)
			
			SET @CreditId = SCOPE_IDENTITY()
		END
	ELSE
		BEGIN
			--update the details
			SET @CreditId = @ID			
		END
	
END
