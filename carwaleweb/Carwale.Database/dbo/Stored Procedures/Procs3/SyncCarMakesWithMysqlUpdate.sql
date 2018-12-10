IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncCarMakesWithMysqlUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncCarMakesWithMysqlUpdate]
GO

	-- =============================================
-- Author:		Prasad 
-- Create date: 14/10/16
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
CREATE PROCEDURE  [dbo].[SyncCarMakesWithMysqlUpdate] --1,'fiat car',0,'10-10-2016','12-10-2016',13,1
	@ID numeric(18, 0),
	@Name varchar(30),
	@IsDeleted bit,	
	@MaCreatedOn datetime ,
	@MaUpdatedOn datetime ,
	@MaUpdatedBy numeric(18, 0),
	@UpdateType int =1
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
	IF(@UpdateType = 1)
		BEGIN
			UPDATE mysql_test...carmakes SET IsDeleted = @IsDeleted,MaUpdatedOn = @MaUpdatedOn, MaUpdatedBy = @MaUpdatedBy WHERE ID = @ID
		END
	ELSE
		begin
			UPDATE mysql_test...carmakes SET
				Name = @Name,
				IsDeleted = @IsDeleted,
				MaCreatedOn = @MaCreatedOn,
				MaUpdatedBy = @MaUpdatedBy
			WHERE ID = @ID
		end
		end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCarMakesWithMysqlUpdate',ERROR_MESSAGE(),'CarMakes',@ID,GETDATE(),@UpdateType)
	END CATCH
END

