IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertCDTransactions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertCDTransactions]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR cheque dd transactions
CREATE PROCEDURE [dbo].[InsertCDTransactions]
	@ConsumerType		SMALLINT,	
	@ConsumerId			NUMERIC, 	
	@CarId				NUMERIC, 
	@PackageId			NUMERIC, 
	@Amount			NUMERIC, 
	@EntryDateTime		DATETIME, 
	@IPAddress			VARCHAR(150),
	@UserAgent			VARCHAR(500),	 --user agent
	@ChequeNumber		VARCHAR(100),	 
	@BankName			VARCHAR(500),	 
	@BranchName			VARCHAR(150),	 
	@BankCity			VARCHAR(150),	
	@RecordId			NUMERIC OUTPUT -- In case of Insertion, It will hold current Record Id.
	
 AS
	DECLARE	@PackageDesc AS VarChar(1500)	
			
		  	
BEGIN
		--get the package description
		Select @PackageDesc = Description From Packages WITH(NOLOCK) Where ID = @PackageId
		
		INSERT INTO CDTransactions 
			(
				ConsumerType, 		ConsumerId, 		CarId, 			PackageId, 		
				Amount, 		PackageDesc, 		EntryDateTime, 		IPAddress,		
				UserAgent,		IsActive, 		ChequeNumber, 	BankName, 
				BranchName, 		BankCity	
			)	
		VALUES
			(
				@ConsumerType, 	@ConsumerId, 		@CarId, 		@PackageId, 		
				@Amount, 		@PackageDesc, 	@EntryDateTime, 	@IPAddress,		
				@UserAgent,		1,			@ChequeNumber, 	@BankName, 
				@BranchName, 		@BankCity			
			)		
		SET @RecordId = SCOPE_IDENTITY()
		
		-- Modified by Navead on 13/10/2016 - removed since customersellinquiries will be migrated to mysql
		--UPDATE CustomerSellInquiries SET PaymentMode = 0 WHERE ID = @CarId --AND CustomerId = @ConsumerId
END

