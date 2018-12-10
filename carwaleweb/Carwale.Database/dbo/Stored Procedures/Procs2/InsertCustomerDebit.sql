IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertCustomerDebit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertCustomerDebit]
GO
	



CREATE PROCEDURE [dbo].[InsertCustomerDebit]
	@Id			NUMERIC,		-- Id. Will be -1 if Its Insertion
	@AccountId		NUMERIC,
	@AmtPaid		NUMERIC,
	@EntryDate		DATETIME,
	@DebitCatId		NUMERIC,
	@Description		VARCHAR(300),
	@ModeId		NUMERIC,
	@BankName		VARCHAR(50),
	@ChequeNo		VARCHAR(25),
	@GivenBy		NUMERIC,
	@DebitID		INTEGER OUTPUT
		
	
 AS
	
BEGIN
	SET 	@DebitID =  0			
	IF @Id = -1 -- Insertion
		
		BEGIN 
			INSERT INTO CustomerDebits 
				( 	AccountId, 	Amount, 	EntryDate, 	DebitCategoryId  , Description ,	GivenBy ,  ModeId,  BankName ,  ChequeNo  )	
			VALUES
				(	@AccountId, 	@AmtPaid, 	@EntryDate, 	@DebitCatId,       @Description ,	@GivenBy,   @ModeId,  @BankName ,   @ChequeNo   )
	
			SET @DebitID = SCOPE_IDENTITY()
		END
	
	
END
