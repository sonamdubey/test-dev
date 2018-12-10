IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPGTransactions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPGTransactions]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR paymentgateway transactions table
--ConsumerType, ConsumerId, CarId, PackageId, Amount, PackageDesc, EntryDateTime, ResponseCode, ResponseMessage, EPGTransactionId, 
--AuthId, ProcessCompleted, TransactionCompleted, IPAddress
-- Modified by Manish on 19-06-2014 sync parametre of the InsertConsumerPackageRequests sp before it was not matching
-- Modified by Manish on 19-06-2014 Added try block
---Modified by Manish on 20-06-2014 commented the execution of the sp  [dbo].[InsertConsumerInvoice]  and copied script of the same in this sp.
---Modified by Manish on 20-06-2014  change the position of if block for IsApproved update in ConsumerPackageRequest table.
-- Modified by navead on 05/10/2016 Shifted this logic to code since customersellinquiries was migrated to mysql
CREATE PROCEDURE [dbo].[InsertPGTransactions]
	@Id				NUMERIC,	-- Id. Will be -1 if Its Insertion, otherwise for updation for specific fields only
	@ConsumerType		SMALLINT,	
	@ConsumerId			NUMERIC, 	
	@CarId				NUMERIC, 
	@PackageId			NUMERIC, 
	@Amount			NUMERIC, 
	@EntryDateTime		DATETIME, 
	@ResponseCode		NUMERIC, 
	@ResponseMessage		VARCHAR(150), 
	@EPGTransactionId		VARCHAR(100),
	@AuthId			VARCHAR(100) = NULL, 
	@ProcessCompleted		BIT, 
	@TransactionCompleted		BIT,	 
	@IPAddress			VARCHAR(25),
	@EntryDate			DATETIME,	--just the date and no time
	@UserAgent			VARCHAR(MAX),	 --user agent
	@PGSource			SMALLINT,
	@RecordId			NUMERIC OUTPUT -- In case of Insertion, It will hold current Record Id.     
 AS
	DECLARE	@PckReqId AS Numeric,
			@TempConsumerType AS SmallInt, 	
			@TempConsumerId AS Numeric,
			@TempPackageId AS INT,	
			@TempValidity AS INT,
			@TempInquiryPoints AS INT,
			@TempAmount	AS NUMERIC,
			@TempStatus	AS NUMERIC,
			@PackageDesc AS VarChar(MAX),
			@TempProcessComplete AS BIT
			
		  	
BEGIN
	
	BEGIN TRY 
	If @Id = -1 -- 
	BEGIN
		--get the package description
		Select @PackageDesc = Description From Packages Where ID = @PackageId
		
		INSERT INTO PGTransactions 
			(
				ConsumerType, 		ConsumerId, 		CarId, 			PackageId, 		
				Amount, 		PackageDesc, 		EntryDateTime, 		ProcessCompleted, 	
				TransactionCompleted, 	IPAddress,		UserAgent,		PGSource
			)	
		VALUES
			(
				@ConsumerType, 	@ConsumerId, 		@CarId, 		@PackageId, 		
				@Amount, 		@PackageDesc, 	@EntryDateTime, 	@ProcessCompleted, 	
				@TransactionCompleted, @IPAddress,		@UserAgent,		@PGSource
			)		
		SET @RecordId = SCOPE_IDENTITY()
		
		declare @InsertType smallint = 1,
		@PlatformId smallint =1,
		@ApplicationId smallint=1
		begin try
		exec [dbo].[SyncPGTransactionsWithMysql]
			@RecordId,
			@ConsumerType,
			@ConsumerId,
			@CarId,
			@PackageId,
			@Amount,
			@PackageDesc,
			@EntryDateTime,
			@ProcessCompleted,
			@TransactionCompleted,
			@IPAddress,
			@UserAgent,
			@PGSource,
			@PlatformId,
			@ApplicationId,
			@InsertType
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','InsertPGTransactions',ERROR_MESSAGE(),'SyncPGTransactionsWithMysql',@Id,GETDATE(),@InsertType)
	END CATCH		
	END
	ELSE
	BEGIN
		
		SET @TempProcessComplete = 1
		--before updating the record check whether this process is incomplete or not
		--in case this process is incomplete then only go ahead els return
		SELECT @TempProcessComplete = ProcessCompleted From PGTransactions Where ID = @ID
		IF @TempProcessComplete = 0	--if process isnot completed then only 
		BEGIN
			
			--update the record
			UPDATE PGTransactions SET
				ResponseCode		= @ResponseCode, 
				ResponseMessage	= @ResponseMessage, 
				EPGTransactionId	= @EPGTransactionId, 
				AuthId			= @AuthId, 
				ProcessCompleted	= @ProcessCompleted, 
				TransactionCompleted	= @TransactionCompleted
			WHERE
				ID = @ID
			
			SET @RecordId = @ID
		
			declare @UpdateType int =1
			begin try
			exec [dbo].[SyncPGTransactionsWithMysqlUpdate] 
			@Id,
			@ProcessCompleted, 
			@TransactionCompleted,
			@ResponseCode, 
			@ResponseMessage, 
			@EPGTransactionId,
			@AuthId	, 	 
			null ,
			@UpdateType 
				end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','InsertPGTransactions',ERROR_MESSAGE(),'SyncPGTransactionsWithMysql',@Id,GETDATE(),@UpdateType)
	END CATCH	
			--if @TransactionCompleted is set to true then make an entry in the package request table and then approve that request
			IF @TransactionCompleted = 1 
			BEGIN
				--call the procedure to make entry into the package request table
				--get all the details form the database
				
					Select
						@TempConsumerType 	= PG.ConsumerType,
						@TempConsumerId	= PG.ConsumerId,
						@TempAmount		= PG.Amount,
						@TempPackageId	= PK.ID,
						@TempValidity		= PK.Validity,
						@TempInquiryPoints	= PK.InquiryPoints
					From
						Packages AS PK,
						PGTransactions AS PG
					Where
						PG.ID = @ID AND
						PK.ID = PG.PackageId
					
					--execute the procedure to insert the requests into the package request tab;e
					EXEC InsertConsumerPackageRequests
						-1,	-- -1 for insertion
						@TempConsumerType, 	
						@TempConsumerId,
						@TempPackageId,	
						@TempValidity,		--@ActualValidity		INT,
						@TempInquiryPoints,	--@ActualInquiryPoints	INT,
						@TempAmount,		--@ActualAmount		NUMERIC,
						4,			--@PaymentModeId	INT, 4 for online payment
						'',			--chk dd no
						'',			--bank name
						@EntryDateTime,	--@Chk_DD_Date	DATETIME,
						@EntryDateTime,	--@EntryDate		DATETIME,
						2,			--@EnteredBy		SMALLINT, 2 for consumer
						@TempConsumerId,	--@EnteredById		NUMERIC,
						@CarId,				--@ItemId
						@PckReqId OUTPUT,
						null,                 ----added by manish on 19-06-2014
						null,                 ----added by manish on 19-06-2014
						null,                  ----added by manish on 19-06-2014
						@PckReqId OUTPUT;       ----added by manish on 19-06-2014
    ---------------------if Block shifted from bottom  by Manish on 20-06-2014--------------------
	 if(@ResponseCode = 0)
	 BEGIN
		UPDATE ConsumerPackageRequests SET isApproved = 1 WHERE Id = @PckReqId 
		begin try
			exec SyncConsumerPackageRequestsWithMysqlUpdate null,null,@PckReqId,null,null,null,null,null,null,null,null,null,null,3
			end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','InsertPGTransactions',ERROR_MESSAGE(),'SyncConsumerPackageRequestsWithMysqlUpdate',@PckReqId,GETDATE(),3)
	END CATCH	
	 END
    --------------------------------------------------------------------------------------------------------------
	
				--now call the procedure to update the package request
				--Exec UpdateConsumerCreditPoints
				--	@PckReqId,
				--	@EntryDate, 	--@NewExpiryDate	DATETIME,		
				--	2,		--@EnteredBy		SMALLINT,		-- 1 for admin and 2 for consumer
				--	@TempConsumerId, --@EnteredById		NUMERIC,		
				--	@TempStatus		--NUMERIC OUTPUT
				--generate the invoice
				INSERT INTO InvoiceIds (CPRId, PGTransactions ) VALUES (@PckReqId,@ID);
				
			--	EXEC  [dbo].[InsertConsumerInvoice]  @CPRID=@PckReqId, @PGTransId=@ID;  -- Commented by Manish on 20-06-2014
--------------------------------------------------------------------------------------------------------------------------------------
   ----------copied the complete script of the sp [dbo].[InsertConsumerInvoice]  since it was not executing -----------
DECLARE
--	@ConsumerTypeIV	SMALLINT,	
--	@ConsumerIdIV		INT,	--validaty in days
	@ConsumerName	VARCHAR(100),	
	@ConsumerEmail	VARCHAR(100),
	@ConsumerContactNo	VARCHAR(50),
	@ConsumerAddress	VARCHAR(150),	
	@PaymentMode		SMALLINT,
	@PaymentModeDetails	VARCHAR(150),
--	@AmountIV		INT,
	@PackageName	VARCHAR(150),
--	@PackageDetails	VARCHAR(MAX),
	@ChqDate		VARCHAR(50),
	@ChqNo		VARCHAR(50),
	@BankName		VARCHAR(150)
--	@EntryDateTimeIV	DATETIME
BEGIN	
	--first check that no invoice has been generated for this CPRID
	--Select ID From ConsumerInvoice WITH (NOLOCK) Where CPRID = @PckReqId
	IF NOT EXISTS (Select ID From ConsumerInvoice WITH (NOLOCK) Where CPRID = @PckReqId)
    BEGIN 
	--fetch the consumertype, package name, and description based on the CPRID
	Select 
		@ConsumerType 	= CP.ConsumerType, 
		@ConsumerId		= CP.ConsumerId, 
		@Amount		= CP.ActualAmount,
		@ChqDate		= Convert(Varchar, CP.Chk_DD_Date, 105),
		@ChqNo		= CP.Chk_DD_Number, 
		@BankName		= CP.BankName,
		@PackageName	= PK.Name,
		@PackageDesc	= PK.Description,
		@PaymentMode		= CP.PaymentModeId,
		@EntryDateTime	= CP.EntryDate
	From
		ConsumerPackageRequests AS CP WITH (NOLOCK), Packages AS PK WITH (NOLOCK)
	Where 
		CP.ID = @PckReqId AND PK.ID = CP.PackageId AND CP.isApproved = 1	
	
	IF @@ROWCOUNT > 0
	BEGIN
		--fetch the name and address of the consumer
		IF(@ConsumerType = 1)		-- dealer
		BEGIN
			--get the dealer name and address
			Select
				@ConsumerName 	= D.Organization,
				@ConsumerEmail 	= IsNull(D.EmailId, '') + ', ' + IsNull(D.ContactEmail, ''),
				@ConsumerContactNo	= IsNull(D.PhoneNo, '') + ', ' + IsNull(D.MobileNo, ''),
				@ConsumerAddress	= IsNull(C.Name, '') + ', ' + IsNull(S.Name, '')
			From
				((Dealers AS D LEFT JOIN Cities AS C ON C.ID = D.CityId)
				LEFT JOIN States AS S ON S.ID = D.StateId)
			Where 
				D.ID = @ConsumerId
		END
		ELSE
		BEGIN
			Select 
				@ConsumerName 	= CU.Name,
				@ConsumerEmail 	= CU.Email,
				@ConsumerContactNo	= IsNull(CU.Mobile, '') + ', ' + IsNull(CU.Phone1, ''),-- + ', ' + IsNull(CU.Phone2, ''),
				@ConsumerAddress	= IsNull(C.Name, '') + ', ' + IsNull(S.Name, '')
			From
				((Customers AS CU LEFT JOIN Cities AS C ON C.ID = CU.CityId)
				LEFT JOIN States AS S ON S.ID = CU.StateId)
			Where 
				CU.ID = @ConsumerId
		END
		
		--prepare the payment mode
		IF @PaymentMode = 4
		BEGIN 
			SET @PaymentModeDetails = 'CREDIT-CARD/ONLINE'			
			
			IF @ID <> -1
				SET @PaymentModeDetails = @PaymentModeDetails + '-ORDER NUMBER : ' + CONVERT(VARCHAR(20), @ID)
	
		END
		IF @PaymentMode = 1
		BEGIN 
			SET @PaymentModeDetails = 'CASH'			
		END
		ELSE IF @PaymentMode = 2
		BEGIN 
			SET @PaymentModeDetails = 'CHEQUE, ' + @ChqNo + ', ' + @ChqDate + ', ' + @BankName		
		END
		ELSE IF @PaymentMode = 3
		BEGIN 
			SET @PaymentModeDetails = 'DD, ' + @ChqNo + ', ' + @ChqDate + ', ' + @BankName		
		END		
		INSERT INTO ConsumerInvoice 
			(
				CPRID, 			ConsumerType, 		ConsumerId, 		ConsumerName, 
				ConsumerEmail, 		ConsumerContactNo, 	ConsumerAddress, 	PaymentMode, 		
				PaymentModeDetails, 	Amount, 		PackageName, 		PackageDetails, 	
				EntryDateTime
			)
		VALUES
			(
				@PckReqId, 		@ConsumerType, 	@ConsumerId, 		@ConsumerName, 
				@ConsumerEmail, 	@ConsumerContactNo, 	@ConsumerAddress, 	@PaymentMode, 	
				@PaymentModeDetails, 	@Amount, 		@PackageName, 	@PackageDesc, 	
				@EntryDateTime
			)
			
	
	END
	END
END
-------------------------------------------------------------------------------------------------------------------------------------				
				
				
		
			END
		END--if @TempProcessComplete
	END
	
	-- Modified by navead on 05/10/2016 Shifted this logic to code since customersellinquiries was migrated to mysql
	--if(@ResponseCode = 0)
	--BEGIN
	--	UPDATE CustomerSellInquiries SET PaymentMode = 1 WHERE ID = @CarId --AND CustomerId = @ConsumerId
	--	--UPDATE ConsumerPackageRequests SET isApproved = 1 WHERE Id = @PckReqId -- Commented by Manish on 20-06-2014
	--END
	END TRY
	BEGIN CATCH
	 
	 INSERT INTO CarWaleWebSiteExceptions (
			                                ModuleName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('Used Car Individual Payment',
									        'dbo.InsertPGTransactions',
											 ERROR_MESSAGE(),
											 'PGTransactions',
											 @id,
											 GETDATE()
                                            )
	END CATCH
END

