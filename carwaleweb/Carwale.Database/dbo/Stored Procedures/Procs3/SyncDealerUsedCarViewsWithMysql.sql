IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncDealerUsedCarViewsWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncDealerUsedCarViewsWithMysql]
GO

	-- =============================================
-- Author:		<Author,,Prasad Gawde>
-- Create date: <Create Date,,14/10/2016>
-- Description:	<Description,,Syncing of SQL server and Mysql>
-- =============================================
CREATE PROCEDURE  [dbo].[SyncDealerUsedCarViewsWithMysql] 
				@InquiryID bigint
				,@Sellertype int
				,@Viewcount int 
				,@Impression int
				,@LastUpdated datetime
				,@IsInsert bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	begin try
	if(@IsInsert=1)
	begin
		INSERT into mysql_test...dealerusedcarviews (
				InquiryID 
				,Sellertype 
				,Viewcount 
				,Impression
				,LastUpdated
			)
	VALUES (
				@InquiryID
				,@Sellertype
				,@Viewcount
				,@Impression
				,@LastUpdated
			);
	end
	else
	begin
		UPDATE mysql_test...dealerusedcarviews 
		SET ViewCount=@Viewcount
			,LastUpdated= @LastUpdated
		    ,Impression=@Impression
		where InquiryID=@InquiryID and Sellertype=@Sellertype
	end
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncDealerUsedCarViewsWithMysql',ERROR_MESSAGE(),'dealerusedcarviews',@InquiryID,GETDATE(),@IsInsert)
	END CATCH
END

