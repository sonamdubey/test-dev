IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncAreasWithMysqlUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncAreasWithMysqlUpdate]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 17/10/2016
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
CREATE PROCEDURE  [dbo].[SyncAreasWithMysqlUpdate]
	-- Add the parameters for the stored procedure here
	@AreaId INT, 
	@AreaName VARCHAR(50),
	@Lattitude DECIMAL(18,4),
	@Longitude DECIMAL(18,4),
	@PinCode VARCHAR(10),
	@UpdateType INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Begin try
    if @updateType= 1 	
	UPDATE mysql_test...areas SET 
				 Name = @AreaName,
				 PinCode = @PinCode,
				 Lattitude = @Lattitude,
				 Longitude = @Longitude
				 WHERE ID = @AreaId;
	else if @UpdateType = 2
		 UPDATE mysql_test...areas SET 
				IsDeleted = 1 
				WHERE ID = @AreaId
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncAreasWithMysqlUpdate',ERROR_MESSAGE(),'Areas',@areaid,GETDATE(),@UpdateType)
	END CATCH
END

