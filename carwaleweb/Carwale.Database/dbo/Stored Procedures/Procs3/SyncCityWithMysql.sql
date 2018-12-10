IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncCityWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncCityWithMysql]
GO

	-- =============================================
-- Author:		<Author,,Prasad Gawde>
-- Create date: <Create Date,22/09/2016>
-- Description:	<Description,added to sync Carwale sqlserver with Mysql>
-- =============================================
CREATE PROCEDURE  [dbo].[SyncCityWithMysql] --11212,'name',1,1,1,20002
	@CityId INT,
	@CityName VARCHAR(50),
	@StateId INT ,
	@Lattitude DECIMAL(18,4),
	@Longitude DECIMAL(18,4),
	@PinCode VARCHAR(10)
AS
BEGIN
	set nocount on;
	BEGIN TRY
	 INSERT INTO mysql_test...cities(ID, Name, StateId, Lattitude, Longitude, DefaultPinCode, IsDeleted ) 
	VALUES (@CityId, @CityName, @StateId , @Lattitude,@Longitude,@PinCode, 0 );
	
	 				end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCityWithMysql',ERROR_MESSAGE(),'Cities',@CityId,GETDATE(),null)
	END CATCH
END

