IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RevertDiscontinuedCarModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RevertDiscontinuedCarModels]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 17/10/2016
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
CREATE PROCEDURE [dbo].[RevertDiscontinuedCarModels] 
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    UPDATE CarModels SET New=1, Comment=NULL, Discontinuation_date=NULL, ReplacedByModelId=NULL, DiscontinuationId=NULL WHERE Id=@Id
		begin try
			exec SyncCarModelsWithMysqlUpdate null  ,
			null  ,
			null  ,
			null  ,
			null ,
			null ,
			null ,
			null ,
			null ,
			null ,
			null ,
			null ,
			null ,
			@Id ,
			null ,
			null ,
			null ,
			3
		end try
		BEGIN CATCH
			INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
			VALUES('MysqlSync','RevertDiscontinuedCarModels',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@Id,GETDATE(),3)
		END CATCH	
END

