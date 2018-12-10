IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncAreaWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncAreaWithMysql]
GO

	-- =============================================
-- Author:		<Author,,Prasad Gawde>
-- Create date: <Create Date,22/09/2016>
-- Description:	<Description,added to sync Carwale sqlserver with Mysql>
-- =============================================
CREATE PROCEDURE  [dbo].[SyncAreaWithMysql] 
	@AreaId INT, 
	@AreaName VARCHAR(50),
	@CityId INT ,
	@Lattitude DECIMAL(18,4),
	@Longitude DECIMAL(18,4),
	@PinCode VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
	 INSERT INTO mysql_test...areas(ID, Name, CityId, Lattitude, Longitude, PinCode, IsDeleted ) 
	VALUES (@AreaId, @AreaName, @CityId , @Lattitude,@Longitude,@PinCode, 0 );
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncAreaWithMysql',ERROR_MESSAGE(),'Areas',@areaid,GETDATE(),null)
	END CATCH
END

