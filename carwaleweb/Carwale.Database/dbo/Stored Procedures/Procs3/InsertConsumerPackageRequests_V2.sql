IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertConsumerPackageRequests_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertConsumerPackageRequests_V2]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR Class PendingPackageRequests TABLE Packages

CREATE PROCEDURE [dbo].[InsertConsumerPackageRequests_V2]
	@Id			NUMERIC,	-- Id. Will be -1 if Its Insertion
	@ConsumerType	SMALLINT,	
	@ConsumerId		NUMERIC,	--validaty in days
	@PackageId		INT,	
	@ActualValidity		INT,
	@ActualInquiryPoints	INT,
	@ActualAmount		NUMERIC,	--return value, -1 for unsuccessfull attempt, and 0 for success
	@PaymentModeId	INT,
	@Chk_DD_Number	VARCHAR(50),
	@BankName		VARCHAR(150),
	@Chk_DD_Date	DATETIME,
	@EntryDate		DATETIME,
	@EnteredBy		SMALLINT,
	@EnteredById		NUMERIC,
	@ItemId			NUMERIC,
	@Status		INTEGER OUTPUT
	
 AS
	
BEGIN
	
	
	IF @Id = -1 

		BEGIN
		
			INSERT INTO ConsumerPackageRequests (ConsumerType, ConsumerId, PackageId, ActualValidity, ActualInquiryPoints,
								ActualAmount,  PaymentModeId, Chk_DD_Number, Chk_DD_Date, EntryDate, 
								EnteredBy,EnteredById, BankName, ItemId)
		
			VALUES (@ConsumerType, @ConsumerId, @PackageId, @ActualValidity, @ActualInquiryPoints,
				@ActualAmount, @PaymentModeId, @Chk_DD_Number, @Chk_DD_Date, @EntryDate, @EnteredBy, 
				@EnteredById, @BankName, @ItemId)
		
			SET @Status = SCOPE_IDENTITY()
		END
	ELSE

		BEGIN
			UPDATE ConsumerPackageRequests SET  ActualValidity=@ActualValidity, 
					ActualInquiryPoints=@ActualInquiryPoints, ActualAmount=@ActualAmount, 
					PaymentModeId = @PaymentModeId, Chk_DD_Number = @Chk_DD_Number,
					Chk_DD_Date = @Chk_DD_Date, EnteredBy = @EnteredBy, EnteredById = @EnteredById,
					BankName = @BankName,
					ItemId = @ItemId
			WHERE Id=@Id
			
			INSERT INTO ConsumerPackageRequestsLogs (ConsumerPkgReqId, ActualValidity, ActualInquiryPoints,
								ActualAmount,  PaymentModeId, Chk_DD_Number, Chk_DD_Date, EntryDate, EnteredBy,EnteredById)
		
			VALUES (@Id, @ActualValidity, @ActualInquiryPoints,
				@ActualAmount, @PaymentModeId, @Chk_DD_Number, @Chk_DD_Date, @EntryDate, @EnteredBy, @EnteredById)			
			
			SET @Status = 0
		END
END