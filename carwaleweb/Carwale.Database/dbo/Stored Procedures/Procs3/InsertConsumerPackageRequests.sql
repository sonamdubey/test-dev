IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertConsumerPackageRequests]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertConsumerPackageRequests]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR Class PendingPackageRequests TABLE Packages
--Modifier	:	Sachin Bharti(28th March 2013)
--Purpose	:	Assign @IsRenewal value null
-- modified by Manish on 19-06-2014 changed the datatype of the parameter Status to Numeric
-- Modified By : Sunil M. Yadav On 08 Jan 2016
-- Description : To save @ContractId.
CREATE PROCEDURE [dbo].[InsertConsumerPackageRequests]
	@Id			NUMERIC,	-- Id. Will be -1 if Its Insertion
	@ConsumerType	SMALLINT,	
	@ConsumerId		NUMERIC,	--validaty in days
	@PackageId		INT,	
	@ActualValidity	INT,
	@ActualInquiryPoints	INT,
	@ActualAmount	NUMERIC,	--return value, -1 for unsuccessfull attempt, and 0 for success
	@PaymentModeId	INT,
	@Chk_DD_Number	VARCHAR(50),
	@BankName		VARCHAR(150),
	@Chk_DD_Date	DATETIME,
	@EntryDate		DATETIME,
	@EnteredBy		SMALLINT,
	@EnteredById	NUMERIC,
	@ItemId			NUMERIC,
	@Status			NUMERIC OUTPUT,
	@IsRenewal		NUMERIC = NULL,
	@RenewalDate	DateTime = Null,
	@Comments		VARCHAR(max) = NULL,
	@NewId			NUMERIC = NULL OUTPUT ,
	@ContractId		INT = NULL
	
	
 AS
	
BEGIN
	
	SET @Status = 0
	IF @Id = -1 
		BEGIN
			INSERT INTO ConsumerPackageRequests (ConsumerType, ConsumerId, PackageId, ActualValidity, ActualInquiryPoints,
								ActualAmount,  PaymentModeId, Chk_DD_Number, Chk_DD_Date, EntryDate, 
								EnteredBy,EnteredById, BankName, ItemId, Comments,ContractId)
		
			VALUES (@ConsumerType, @ConsumerId, @PackageId, @ActualValidity, @ActualInquiryPoints,
				@ActualAmount, @PaymentModeId, @Chk_DD_Number, @Chk_DD_Date, @EntryDate, @EnteredBy, 
				@EnteredById, @BankName, @ItemId, @Comments,@ContractId)
		
			SET @NewId = SCOPE_IDENTITY()
			SET @Status = @NewId
			
			begin try
			exec SyncConsumerPackageRequestsWithMysql @NewId,@ConsumerType,@ConsumerId,@PackageId,@ActualValidity,@ActualInquiryPoints,					@ActualAmount,@PaymentModeId,@Chk_DD_Number,@BankName,@Chk_DD_Date,@EntryDate,@EnteredBy,@EnteredById,@ItemId,@Comments,@ContractId,1;
			 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','InsertConsumerPackageRequests',ERROR_MESSAGE(),'SyncConsumerPackageRequestsWithMysql',@NewId,GETDATE(),1)
	END CATCH		
			if @IsRenewal = 1
				BEGIN
					INSERT INTO ConsumerPackageRenewal(RequestId, RenewalDate) 
					VALUES(@Status, @RenewalDate)
				END
		END
	ELSE
		BEGIN
			UPDATE ConsumerPackageRequests SET  ActualValidity=@ActualValidity, 
					ActualInquiryPoints=@ActualInquiryPoints, ActualAmount=@ActualAmount, 
					PaymentModeId = @PaymentModeId, Chk_DD_Number = @Chk_DD_Number,
					Chk_DD_Date = @Chk_DD_Date, EnteredBy = @EnteredBy, EnteredById = @EnteredById,
					BankName = @BankName, ItemId = @ItemId
			WHERE Id=@Id
			
			begin try
			exec SyncConsumerPackageRequestsWithMysqlUpdate @EnteredById,null,null,@ActualValidity,@ActualInquiryPoints,@ActualAmount,@PaymentModeId,@Chk_DD_Number,@BankName,@Chk_DD_Date,@EnteredBy,@ItemId,@Id,2
			end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','InsertConsumerPackageRequests',ERROR_MESSAGE(),'SyncConsumerPackageRequestsWithMysqlUpdate',@Id,GETDATE(),2)
	END CATCH		
			INSERT INTO ConsumerPackageRequestsLogs (ConsumerPkgReqId, ActualValidity, ActualInquiryPoints,
								ActualAmount,  PaymentModeId, Chk_DD_Number, Chk_DD_Date, EntryDate, EnteredBy,EnteredById)
			VALUES (@Id, @ActualValidity, @ActualInquiryPoints,
				@ActualAmount, @PaymentModeId, @Chk_DD_Number, @Chk_DD_Date, @EntryDate, @EnteredBy, @EnteredById)			
			
			SET @Status = 0
			
			SET @NewId = @Id
		END
END
---------------------------------------------------------------------------------------------------------------------------------------------------------------

