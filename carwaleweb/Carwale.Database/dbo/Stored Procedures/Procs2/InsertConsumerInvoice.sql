IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertConsumerInvoice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertConsumerInvoice]
GO
	CREATE PROCEDURE [dbo].[InsertConsumerInvoice]
	@CPRID		NUMERIC,	-- 
	@PGTransId		NUMERIC	--  -1 in case of other type of payment
	
AS

DECLARE
	@ConsumerType	SMALLINT,	
	@ConsumerId		NUMERIC,	--validaty in days
	@ConsumerName	VARCHAR(100),	
	@ConsumerEmail	VARCHAR(100),
	@ConsumerContactNo	VARCHAR(100),
	@ConsumerAddress	VARCHAR(300),	
	@PaymentMode		SMALLINT,
	@PaymentModeDetails	VARCHAR(200),
	@Amount		NUMERIC,
	@PackageName	VARCHAR(500),
	@PackageDetails	VARCHAR(2500),
	@ChqDate		VARCHAR(50),
	@ChqNo		VARCHAR(50),
	@BankName		VARCHAR(150),
	@EntryDateTime	DATETIME

BEGIN	

	--first check that no invoice has been generated for this CPRID
	Select ID From ConsumerInvoice WITH (NOLOCK) Where CPRID = @CPRID
	IF @@ROWCOUNT > 0
		RETURN

	--fetch the consumertype, package name, and description based on the CPRID
	Select 
		@ConsumerType 	= CP.ConsumerType, 
		@ConsumerId		= CP.ConsumerId, 
		@Amount		= CP.ActualAmount,
		@ChqDate		= Convert(Varchar, CP.Chk_DD_Date, 105),
		@ChqNo		= CP.Chk_DD_Number, 
		@BankName		= CP.BankName,
		@PackageName	= PK.Name,
		@PackageDetails	= PK.Description,
		@PaymentMode		= CP.PaymentModeId,
		@EntryDateTime	= CP.EntryDate
	From
		ConsumerPackageRequests AS CP WITH (NOLOCK), Packages AS PK WITH (NOLOCK)
	Where 
		CP.ID = @CPRID AND PK.ID = CP.PackageId AND CP.isApproved = 1	

	
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
				@ConsumerContactNo	= IsNull(CU.Mobile, '') + ', ' + IsNull(CU.Phone1, '') + ', ' + IsNull(CU.Phone2, ''),
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
			
			IF @PGTransId <> -1
				SET @PaymentModeDetails = @PaymentModeDetails + '-ORDER NUMBER : ' + CONVERT(VARCHAR(20), @PGTransId)
	
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
				@CPRID, 		@ConsumerType, 	@ConsumerId, 		@ConsumerName, 
				@ConsumerEmail, 	@ConsumerContactNo, 	@ConsumerAddress, 	@PaymentMode, 	
				@PaymentModeDetails, 	@Amount, 		@PackageName, 	@PackageDetails, 	
				@EntryDateTime
			)
			
	
	END


END
