IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncCarMakesWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncCarMakesWithMysql]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 23/9/16
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
CREATE PROCEDURE  [dbo].[SyncCarMakesWithMysql] 
	@ID numeric(18, 0),
	@Name varchar(30),
	@IsDeleted bit,	
	@MaCreatedOn datetime ,
	@MaUpdatedBy numeric(18, 0)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
	INSERT INTO mysql_test...carmakes(ID, Name,IsDeleted ,MaCreatedOn ,MaUpdatedBy)  
	VALUES (@ID, @Name,@IsDeleted ,@MaCreatedOn ,@MaUpdatedBy);
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCarMakesWithMysql',ERROR_MESSAGE(),'CarMakes',@ID,GETDATE(),null)
	END CATCH
END

