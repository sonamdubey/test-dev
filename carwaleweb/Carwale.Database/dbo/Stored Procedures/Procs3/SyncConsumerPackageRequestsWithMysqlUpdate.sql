IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncConsumerPackageRequestsWithMysqlUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncConsumerPackageRequestsWithMysqlUpdate]
GO

	-- =============================================
-- Author:		<Author,,Prasad Gawde>
-- Create date: <Create Date,23/09/2016>
-- Description:	<Description,added to sync Carwale sqlserver with Mysql>
-- =============================================
CREATE PROCEDURE  [dbo].[SyncConsumerPackageRequestsWithMysqlUpdate] 
	   @EnteredById   numeric,  
	   @NewExpiryDate  datetime,  
	   @ConsumerPkgReqId   numeric,
	   @ActualValidity INT ,
	   @ActualInquiryPoints INT,
	   @ActualAmount numeric ,
	   @PaymentModeId INT,
	   @Chk_DD_Number	VARCHAR(50),
		@BankName		VARCHAR(150),
		@Chk_DD_Date	DATETIME,
		@EnteredBy		SMALLINT,
		@ItemId			NUMERIC,
		@Id NUMERIC,
		@UpdateType Numeric
AS
BEGIN
		SET NOCOUNT ON;
		BEGIN TRY
		
		declare @strNewExpiryDate varchar (50)
		declare @strChk_DD_Date varchar(50),
		@strEnteredById   varchar(25),  	   
		   @strConsumerPkgReqId   varchar(25),
		   @strActualValidity varchar(25) ,
		   @strActualInquiryPoints varchar(25),
		   @strActualAmount varchar(25) ,
		   @strPaymentModeId varchar(25),		
		@strEnteredBy		varchar(25),
		@strItemId			varchar(25),
		@strId varchar(25),
		@strUpdateType varchar(25),
		@strBankName varchar(25)

		if @strEnteredById is null
			set @strEnteredById =null
		else
			set @strEnteredById =cast(@EnteredById as varchar)

		   if @ConsumerPkgReqId   is null 
			set @strConsumerPkgReqId=null
		   else
			set @strConsumerPkgReqId=cast(@ConsumerPkgReqId as varchar)
			
		   if @ActualValidity   is null 
			set @strActualValidity=null
		   else
			set @strActualValidity=cast(@ActualValidity as varchar)

		   if @ActualInquiryPoints   is null 
			set @strActualInquiryPoints=null
		   else
			set @strActualInquiryPoints=cast(@ActualInquiryPoints as varchar)

		   if @ActualAmount  is null 
			set @strActualAmount=null
		   else
			set @strActualAmount=cast(@ActualAmount as varchar)

		   if @PaymentModeId   is null 
			set @strPaymentModeId=null
		   else
			set @strPaymentModeId=cast(@PaymentModeId as varchar)

		   if @EnteredBy   is null 
			set @strEnteredBy=null
		   else
			set @strEnteredBy=cast(@EnteredBy as varchar)

		   if @ItemId   is null 
			set @strItemId=null
		   else
			set @strItemId=cast(@ItemId as varchar)


		   if @Id   is null 
			set @strId=null
		   else
			set @strId=cast(@Id as varchar)


		   if @UpdateType   is null 
			set @strUpdateType=null
		   else
			set @strUpdateType=cast(@UpdateType as varchar)

		if @Chk_DD_Date is null
			set @strChk_DD_Date=null
		else
			set @strChk_DD_Date = cast('''' as varchar)+CONVERT(varchar,@Chk_DD_Date,121) +cast('''' as varchar)
		 

		 if @NewExpiryDate is null
			set @strNewExpiryDate=null
		else
			set @strNewExpiryDate = cast('''' as varchar)+CONVERT(varchar,@NewExpiryDate,121) +cast('''' as varchar)


			declare @strChk_DD_Number varchar(25)

			if @Chk_DD_Number   is null 
			set @strChk_DD_Number=null
		   else
			set @strChk_DD_Number=cast(@Chk_DD_Number as varchar)

			if @BankName   is null 
			set @strBankName=null
		   else
			set @strBankName=cast(@BankName as varchar)

			

		declare @Query varchar(max);
	

		set @Query=
		'call SyncConsumerPackageRequestsWithMysqlUpdate('+
		cast(isnull(@strEnteredById,'null') as varchar)+','+ 
	    cast(isnull(@strNewExpiryDate,'null') as varchar)+','+ 
	    cast(isnull(@strConsumerPkgReqId,'null') as varchar)+','+ 
	    cast(isnull(@strActualValidity,'null') as varchar)+','+ 
	    cast(isnull(@strActualInquiryPoints,'null') as varchar)+','+ 
	    cast(isnull(@strActualAmount,'null') as varchar)+','+ 
	    cast(isnull(@strPaymentModeId,'null') as varchar)+','+ 
		cast(isnull(@strChk_DD_Number,'null') as varchar)+','+ 
		cast(isnull(@strBankName,'null') as varchar)+','+ 
		cast(isnull(@strChk_DD_Date,'null') as varchar)+','+ 
		cast(isnull(@strEnteredBy,'null') as varchar)+','+ 
		cast(isnull(@strItemId,'null') as varchar)+','+ 
		cast(isnull(@strId,'null') as varchar)+','+ 
		cast(isnull(@strUpdateType,'null') as varchar)+
		')'

	exec(@query) at mysql_test

	/*if @UpdateType=1 
		Update mysql_test...consumerpackagerequests Set IsApproved = 1,isActive = 1, ApprovedBy = @EnteredById, ApprovalDate = @NewExpiryDate  Where ID = @ConsumerPkgReqId

	else if @UpdateType=2 
		UPDATE mysql_test...consumerpackagerequests SET  ActualValidity=@ActualValidity, 
					ActualInquiryPoints=@ActualInquiryPoints, ActualAmount=@ActualAmount, 
					PaymentModeId = @PaymentModeId, Chk_DD_Number = @Chk_DD_Number,
					Chk_DD_Date = @Chk_DD_Date, EnteredBy = @EnteredBy, EnteredById = @EnteredById,
					BankName = @BankName, ItemId = @ItemId
			WHERE Id=@Id

	else if @UpdateType=3
		Update mysql_test...consumerpackagerequests Set IsApproved = 1 Where ID = @ConsumerPkgReqId

	else if @UpdateType=4
		UPDATE mysql_test...consumerpackagerequests SET  ActualValidity=@ActualValidity, 
					ActualInquiryPoints=@ActualInquiryPoints, ActualAmount=@ActualAmount					
			WHERE Id=@Id

	else if @UpdateType=5
	UPDATE mysql_test...consumerpackagerequests 
			SET EndDate=@NewExpiryDate,
			ContractStatus=4,
			isActive=0
			WHERE Id=@ConsumerPkgReqId		

	else if @UpdateType=6
		UPDATE mysql_test...consumerpackagerequests 
				SET 
				ContractStatus=4
				WHERE Id=@ConsumerPkgReqId		

	else if @UpdateType=7
		UPDATE mysql_test...consumerpackagerequests 
				SET EndDate=@NewExpiryDate, 
				ContractStatus=4
				WHERE Id=@ConsumerPkgReqId		

				--[DCRM_AutoPausePackageData] sp has direct changes for mysql_test...consumerpackagerequests 

	else if @UpdateType=8
		Update mysql_test...consumerpackagerequests  Set ItemId = @ItemId Where ID = @ID

	else if @UpdateType=9
		Update mysql_test...consumerpackagerequests  SET isActive = 0 
				 WHERE Id = @ID 

	else if @UpdateType=10
		Update mysql_test...consumerpackagerequests set ReceivedPayment = 1 Where Id = @ID 
		*/
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncConsumerPackageRequestsWithMysqlUpdate',ERROR_MESSAGE(),'ConsumerPackageRequests',@ConsumerPkgReqId,GETDATE(),@UpdateType)
	END CATCH	
END
