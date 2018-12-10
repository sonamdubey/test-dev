IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncPGTransactionsWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncPGTransactionsWithMysql]
GO

	-- =============================================
-- Author:		<Author,,Prasad Gawde>
-- Create date: <Create Date,,02-11-2016>
-- Description:	<Description,,Synching PGTransactions with mysql>
-- =============================================
CREATE PROCEDURE  [dbo].[SyncPGTransactionsWithMysql] --2,1,1,1,1,1,'funny desc', '10-10-2016',1,1,'1.0.0.1','funny agent',1,1,1,2
	@Id				NUMERIC,	-- Id. Will be -1 if Its Insertion, otherwise for updation for specific fields only
	@ConsumerType		SMALLINT,	
	@ConsumerId			NUMERIC, 	
	@CarId				NUMERIC, 
	@PackageId			NUMERIC, 
	@Amount			NUMERIC, 
	@PackageDesc    varchar(1500),
	@EntryDateTime		DATETIME, 
	@ProcessCompleted		BIT, 
	@TransactionCompleted	BIT,	 
	@IPAddress			VARCHAR(25),
	@UserAgent			VARCHAR(MAX),	 --user agent
	@PGSource			SMALLINT,
	@PlatformId			SMALLINT = 1,         ---1 :Carwale, 43:Mobile, 74:Android, 83:IOS
	@ApplicationId		SMALLINT = 1,         ---1 :Carwale, 2:Bikewale
    @InsertType int =1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @Query varchar(max);
begin try
if @InsertType=1
begin
  /*INSERT INTO mysql_test...pgtransactions --mysql_test...pgtransactions 
			(
				Id,ConsumerType, 		ConsumerId, 		CarId, 			PackageId, 		
				Amount, 		PackageDesc, 		EntryDateTime, 		ProcessCompleted, 	
				TransactionCompleted, 	IPAddress,		UserAgent,		PGSource
			)	
		VALUES
			(
				@Id,@ConsumerType, 	@ConsumerId, 		@CarId, 		@PackageId, 		
				@Amount, 		@PackageDesc, 	@EntryDateTime, 	@ProcessCompleted, 	
				@TransactionCompleted, @IPAddress,		@UserAgent,		@PGSource
			)	*/	
			set @Query='call SyncPGTransactionsWithMysql('+
						cast(isnull(@Id,'null') as varchar)+','+
						cast(isnull(@ConsumerType,'null') as varchar)+','+
						cast(isnull(@ConsumerId,'null') as varchar)+','+
						cast(isnull(@CarId,'null') as varchar)+','+
						cast(isnull(@PackageId,'null') as varchar)+','+
						cast(isnull(@Amount,'null') as varchar)+','+
						cast('''' as varchar)+cast(isnull(@PackageDesc,'null') as varchar)+cast('''' as varchar)+','+
						cast('''' as varchar)+CONVERT(varchar,@EntryDateTime,121) +cast('''' as varchar)+','+
						cast(isnull(@ProcessCompleted,'null') as varchar)+','+
						cast(isnull(@TransactionCompleted,'null') as varchar)+','+
						cast('''' as varchar)+cast(isnull(@IPAddress,'null') as varchar)+cast('''' as varchar)+','+
						cast('''' as varchar)+cast(isnull(@UserAgent,'null') as varchar)+cast('''' as varchar)+','+
						cast(isnull(@PGSource,'null') as varchar)+',1,1,1)'		
			exec (@query) at [Mysql_test]
end
else if @InsertType=2
begin
/*
		INSERT INTO mysql_test...pgtransactions--mysql_test...pgtransactions  
			(
				Id,ConsumerType, 		ConsumerId, 		CarId, 			PackageId, 		
				Amount, 		PackageDesc, 		EntryDateTime, 		ProcessCompleted, 	
				TransactionCompleted, 	IPAddress,		UserAgent,		PGSource,	PlatformId,	ApplicationId
			)	
		VALUES
			(
				@Id,@ConsumerType, 	@ConsumerId, 		@CarId, 		@PackageId, 		
				@Amount, 		@PackageDesc, 	@EntryDateTime, 	@ProcessCompleted, 	
				@TransactionCompleted, @IPAddress,		@UserAgent,		@PGSource, @PlatformId, @ApplicationId
			)
			*/		
			set @Query='call SyncPGTransactionsWithMysql('+
						cast(isnull(@Id,'null') as varchar)+','+
						cast(isnull(@ConsumerType,'null') as varchar)+','+
						cast(isnull(@ConsumerId,'null') as varchar)+','+
						cast(isnull(@CarId,'null') as varchar)+','+
						cast(isnull(@PackageId,'null') as varchar)+','+
						cast(isnull(@Amount,'null') as varchar)+','+
						cast('''' as varchar)+cast(isnull(@PackageDesc,'null') as varchar)+cast('''' as varchar)+','+
						cast('''' as varchar)+CONVERT(varchar,@EntryDateTime,121) +cast('''' as varchar)+','+
						cast(isnull(@ProcessCompleted,'null') as varchar)+','+
						cast(isnull(@TransactionCompleted,'null') as varchar)+','+
						cast('''' as varchar)+cast(isnull(@IPAddress,'null') as varchar)+cast('''' as varchar)+','+
						cast('''' as varchar)+cast(isnull(@UserAgent,'null') as varchar)+cast('''' as varchar)+','+
						cast(isnull(@PGSource,'null') as varchar)+','+
						cast(isnull(@PlatformId,'null') as varchar)+','+
						cast(isnull(@ApplicationId,'null') as varchar)+','+
						'2)'
		exec (@query) at [Mysql_test]
end
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncPGTransactionsWithMysql',ERROR_MESSAGE(),'pgtransactions',@Id,GETDATE(),@InsertType)
	END CATCH
END

