IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncCarModelRootsWithMysqlDelete]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncCarModelRootsWithMysqlDelete]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 23/9/16
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
CREATE PROCEDURE  [dbo].[SyncCarModelRootsWithMysqlDelete]
	-- Add the parameters for the stored procedure here
	@RootId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
     DELETE FROM mysql_test...carmodelroots where RootId = @RootId
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCarModelRootsWithMysqlDelete',ERROR_MESSAGE(),'CarModelRoots',@RootId,GETDATE(),null)
	END CATCH
END

