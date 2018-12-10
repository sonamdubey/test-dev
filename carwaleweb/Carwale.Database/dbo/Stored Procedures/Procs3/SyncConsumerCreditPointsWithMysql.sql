IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncConsumerCreditPointsWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncConsumerCreditPointsWithMysql]
GO

	-- =============================================
-- Author:		<Author,,Prasad Gawde>
-- Create date: <Create Date,23/09/2016>
-- Description:	<Description,added to sync Carwale sqlserver with Mysql>
-- =============================================
CREATE PROCEDURE  [dbo].[SyncConsumerCreditPointsWithMysql]
	   @Id Numeric,
	   @ConsumerId   NUMERIC,  
	   @ConsumerType  NUMERIC,  
	   @ActualInquiryPoints   NUMERIC,   
	   @PackageType   SMALLINT ,
	   @NewDate DATETIME,
	   @PackageId   NUMERIC,
	   @InsertId int =1
	   
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
	if(@InsertId=1)
		INSERT INTO  mysql_test...consumercreditpoints(ID,ConsumerType, ConsumerId, Points, ExpiryDate, PackageType, CustomerPackageId) 
		VALUES(@Id,@ConsumerType, @ConsumerId, @ActualInquiryPoints, @NewDate, @PackageType, @PackageId); 
	else if @InsertId =2
		INSERT INTO mysql_test...consumercreditpoints (ID, ConsumerType, ConsumerId, ExpiryDate, Points)		
			VALUES (@Id,@ConsumerType, @ConsumerId, @NewDate, @ActualInquiryPoints)
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncConsumerCreditPointsWithMysql',ERROR_MESSAGE(),'ConsumerCreditPoints',@Id,GETDATE(),@InsertId)
	END CATCH
END

