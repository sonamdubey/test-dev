IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncConsumerCreditPointsWithMysqlUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncConsumerCreditPointsWithMysqlUpdate]
GO

	
-- =============================================
-- Author:		<Author,,Prasad Gawde>
-- Create date: <Create Date,23/09/2016>
-- Description:	<Description,added to sync Carwale sqlserver with Mysql>
-- =============================================
CREATE PROCEDURE  [dbo].[SyncConsumerCreditPointsWithMysqlUpdate] --3838,1,12,12,'2017-10-10','2017-11-10',1,1,1,1
	   @ConsumerId   NUMERIC,  
	   @ConsumerType  NUMERIC,  
	   @ActualInquiryPoints   NUMERIC,   
	   @ActualValidity    NUMERIC,  
	   @NewExpiryDate   datetime,	
	   @PreviousExpiryDate DATETIME,     
	   @PackageType   SMALLINT ,
	   @PackageId   NUMERIC,
	   @updateType Numeric,
	   @Id Numeric = NULL
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY	

	declare @Query varchar(max)

	declare @strPreviousExpiryDate varchar (50)

	if @PreviousExpiryDate is null
		set @strPreviousExpiryDate=null
	else
		set @strPreviousExpiryDate = cast('''' as varchar)+CONVERT(varchar,@PreviousExpiryDate,121) +cast('''' as varchar)

	declare @strNewExpiryDate varchar (50)

	if @NewExpiryDate is null
		set @strNewExpiryDate=null
	else
		set @strNewExpiryDate = cast('''' as varchar)+CONVERT(varchar,@NewExpiryDate,121) +cast('''' as varchar)

		   declare
	   @strConsumerId   varchar(25),  
	   @strConsumerType  varchar(25),  
	   @strActualInquiryPoints   varchar(25),   
	   @strActualValidity    varchar(25),  
	   @strPackageType   varchar(25) ,
	   @strPackageId   varchar(25),
	   @strupdateType varchar(25),
	   @strId varchar(25) 


	   if @consumerID is null 
			set @strconsumerid='null'
		else
			set @strconsumerid=cast(@consumerid as varchar)
			
		if @ConsumerType is null 
			set @strConsumerType='null'
		else
			set @strConsumerType=cast(@ConsumerType  as varchar)

			if @ActualInquiryPoints is null 
			set @strActualInquiryPoints='null'
		else
			set @strActualInquiryPoints=cast(@ActualInquiryPoints as varchar)

			if @ActualValidity is null 
			set @strActualValidity='null'
		else
			set @strActualValidity=cast(@ActualValidity as varchar)

			if @PackageType is null 
			set @strPackageType='null'
		else
			set @strPackageType=cast(@PackageType as varchar)

			if @PackageId is null 
			set @strPackageId='null'
		else
			set @strPackageId=cast(@PackageId as varchar)


			if @updateType is null 
			set @strupdateType='null'
		else
			set @strupdateType=cast(@updateType as varchar)

			if @Id is null 
			set @strId='null'
		else
			set @strId=cast(@Id as varchar)

	set @Query	=
	' call SyncConsumerCreditPointsWithMysqlUpdate('+
	   cast(isnull(@strConsumerId,'null') as varchar)+','+   
	   cast(isnull(@strConsumerType,'null') as varchar)+','+   
	   cast(isnull(@strActualInquiryPoints,'null') as varchar)+','+   
	   cast(isnull(@strActualValidity,'null') as varchar)+','+   
	   cast(isnull(@strNewExpiryDate,'null') as varchar)+','+   
	   cast(isnull(@strPreviousExpiryDate,'null') as varchar)+','+   
	   cast(isnull(@strPackageType,'null') as varchar)+','+   
	   cast(isnull(@strPackageId,'null') as varchar)+','+   
	   cast(isnull(@strupdateType,'null') as varchar)+','+   
	   cast(isnull(@strId,'1') as varchar)+')';
	

	exec (@query) at mysql_test	
	
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncConsumerCreditPointsWithMysqlUpdate',ERROR_MESSAGE(),'ConsumerCreditPoints',isnull(@Id,'null'),GETDATE(),@updateType)
	END CATCH						
END