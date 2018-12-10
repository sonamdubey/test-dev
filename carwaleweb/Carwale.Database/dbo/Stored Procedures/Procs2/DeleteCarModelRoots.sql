IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteCarModelRoots]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeleteCarModelRoots]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 17/10/16
-- Description:	added to delete car model root
-- =============================================
CREATE PROCEDURE [dbo].[DeleteCarModelRoots] 
	-- Add the parameters for the stored procedure here
	@RootId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Delete from CarModelRoots where RootId = @RootId
	begin try
		exec SyncCarModelRootsWithMysqlDelete @RootId
   end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','DeleteCarModelRoots',ERROR_MESSAGE(),'SyncCarModelRootsWithMysqlDelete',@RootId,GETDATE(),null)
	END CATCH
END

