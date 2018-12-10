IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AddCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AddCity]
GO

	-- =============================================        
-- Author:  Sushil Kumar on 13th July 2016 
-- Description: Insert new city into cities table     
-- =============================================        
CREATE PROCEDURE [dbo].[AddCity]
@CityName VARCHAR(50),
@StateId INT ,
@Lattitude DECIMAL(18,4),
@Longitude DECIMAL(18,4),
@PinCode VARCHAR(10),
@CityId INT OUTPUT
AS        
BEGIN        
 
 INSERT INTO Cities( Name, StateId, Lattitude, Longitude, DefaultPinCode, IsDeleted ) 
	VALUES ( @CityName, @StateId , @Lattitude,@Longitude,@PinCode, 0 );
 -- return inserted cityid
 SET @CityId = SCOPE_IDENTITY();
 -- Syncing with Mysql
	begin try
		exec SyncCityWithMysql @CityId, @CityName, @StateId , @Lattitude,@Longitude,@PinCode;
  	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','AddCity',ERROR_MESSAGE(),'SyncCityWithMysql',@CityId,GETDATE(),null)
	END CATCH			
END 

