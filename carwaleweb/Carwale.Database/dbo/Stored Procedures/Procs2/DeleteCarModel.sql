IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteCarModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeleteCarModel]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 17/10/2016
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
CREATE PROCEDURE [dbo].[DeleteCarModel] 
	-- Add the parameters for the stored procedure here
	@Id int,
	@MoUpdatedBy int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		UPDATE CarModels SET IsDeleted=1,MoUpdatedOn=GETDATE(),MoUpdatedBy=@MoUpdatedBy WHERE Id=@Id
		begin try
		exec SyncCarModelsWithMysqlUpdate null  ,
		null  ,
		null  ,
		null  ,
		null ,
		@MoUpdatedBy ,
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
		2
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCarModelsWithMysqlUpdate',ERROR_MESSAGE(),'DeleteCarModel',@Id,GETDATE(),2)
	END CATCH
END

