IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncConsumerPackageRequestsWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncConsumerPackageRequestsWithMysql]
GO

	-- =============================================
-- Author:		<Author,,Prasad Gawde>
-- Create date: <Create Date,,29/09/2016>
-- Description:	<Description,,to sync sql server with mysql>
-- =============================================
CREATE PROCEDURE  [dbo].[SyncConsumerPackageRequestsWithMysql]
	@Id			NUMERIC,	
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
	@Comments		VARCHAR(max),
	@ContractId		INT,
	@InsertType int =1
AS
BEGIN
	
		SET NOCOUNT ON;
		BEGIN TRY
	if @InsertType=1
	INSERT INTO mysql_test...consumerpackagerequests (Id,ConsumerType, ConsumerId, PackageId, ActualValidity, ActualInquiryPoints,
								ActualAmount,  PaymentModeId, Chk_DD_Number, Chk_DD_Date, EntryDate, 
								EnteredBy,EnteredById, BankName, ItemId, Comments,ContractId)
		
			VALUES (@Id,@ConsumerType, @ConsumerId, @PackageId, @ActualValidity, @ActualInquiryPoints,
				@ActualAmount, @PaymentModeId, @Chk_DD_Number, @Chk_DD_Date, @EntryDate, @EnteredBy, 
				@EnteredById, @BankName, @ItemId, @Comments,@ContractId);
	else if @InsertType=2
	INSERT INTO mysql_test...consumerpackagerequests (Id,ConsumerType, ConsumerId, PackageId, ActualValidity, ActualInquiryPoints,
								ActualAmount,  PaymentModeId, EntryDate, 
								EnteredBy,EnteredById)
		
			VALUES (@Id,@ConsumerType, @ConsumerId, @PackageId, @ActualValidity, @ActualInquiryPoints,
				@ActualAmount, @PaymentModeId, @EntryDate, @EnteredBy, 
				@EnteredById);
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncConsumerPackageRequestsWithMysql',ERROR_MESSAGE(),'ConsumerPackageRequests',@Id,GETDATE(),@InsertType)
	END CATCH	
END

