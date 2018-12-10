IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncPGTransactionsWithMysqlUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncPGTransactionsWithMysqlUpdate]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[SyncPGTransactionsWithMysqlUpdate] --1,1,1,1,'funny response','funny id','auth id','transaction ref id',1
	@Id				NUMERIC,	-- Id. Will be -1 if Its Insertion, otherwise for updation for specific fields only
	@ProcessCompleted		BIT, 
	@TransactionCompleted	BIT,
	@ResponseCode		NUMERIC, 
	@ResponseMessage		VARCHAR(150), 
	@EPGTransactionId		VARCHAR(100),
	@AuthId			VARCHAR(100) = NULL, 	 
	@TransactionReferenceId VARCHAR(20) = NULL,
    @UpdateType int =1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @errorId varchar(20)=@Id
	declare @query varchar(max);
	begin try
	if @UpdateType=1
	begin
	/*
		UPDATE mysql_test...pgtransactions SET
				ResponseCode		= @ResponseCode, 
				ResponseMessage	= @ResponseMessage, 
				EPGTransactionId	= @EPGTransactionId, 
				AuthId			= @AuthId, 
				ProcessCompleted	= @ProcessCompleted, 
				TransactionCompleted	= @TransactionCompleted
			WHERE
				ID = @ID;
				*/
		set @query='call SyncPGTransactionsWithMysqlUpdate('+
		cast(isnull(@id,'null') as varchar)+','+
		cast(isnull(@ProcessCompleted,'null') as varchar)+','+
		cast(isnull(@TransactionCompleted,'null') as varchar)+','+
		cast(isnull(@ResponseCode,'null') as varchar)+','+
		cast('''' as varchar)+isnull(@ResponseMessage,'null')+cast('''' as varchar)+','+
		cast('''' as varchar)++isnull(@EPGTransactionId,'null')+cast('''' as varchar)+','+
		cast('''' as varchar)+isnull(@AuthId,'null')+cast('''' as varchar)+',null,1)'
		exec (@query) at [Mysql_test]
	end
	else if @UpdateType =2
	BEGIN
		declare @transactionRefId varchar(20)
		set @transactionRefId = CAST((ISNULL(@TransactionReferenceId, '') + CAST(@ID AS VARCHAR)) AS varchar)
		set @errorId=@transactionRefId
		--UPDATE mysql_test...pgtransactions SET TransactionReferenceId =@transactionRefId  WHERE ID = @ID
		set @query='call SyncPGTransactionsWithMysqlUpdate('+
		cast(isnull(@id,'null') as varchar)+
		',null,null,null,null,null,null,'+
		cast('''' as varchar)+
		isnull(@transactionRefId,'null')+
		cast('''' as varchar)+
		',2)'		
		exec (@query) at [Mysql_test]
	end
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncPGTransactionsWithMysqlUpdate',ERROR_MESSAGE(),'pgtransactions',@errorId,GETDATE(),@UpdateType)
	END CATCH
END

