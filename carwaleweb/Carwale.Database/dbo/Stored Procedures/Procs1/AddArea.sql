IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AddArea]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AddArea]
GO

	-- =============================================        
-- Author:  Sushil Kumar on 13th July 2016 
-- Description: Insert new area into areas table     
-- =============================================        
CREATE PROCEDURE [dbo].[AddArea]
@AreaName VARCHAR(50),
@CityId INT ,
@Lattitude DECIMAL(18,4),
@Longitude DECIMAL(18,4),
@PinCode VARCHAR(10),
@AreaId INT OUTPUT
AS        
BEGIN        
 
 INSERT INTO Areas( Name, CityId, Lattitude, Longitude, PinCode, IsDeleted ) 
	VALUES ( @AreaName, @CityId , @Lattitude,@Longitude,@PinCode, 0 );
 -- return inserted areaid
 SET @AreaId = SCOPE_IDENTITY();
-- Prasad: Folllowing is added to sync Carwale sqlserver with Mysql
	begin try
		exec SyncAreaWithMysql @AreaId, @AreaName, @CityId , @Lattitude,@Longitude,@PinCode ;
 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','AddArea',ERROR_MESSAGE(),'SyncAreaWithMysql',@AreaId,GETDATE(),null)
	END CATCH			
-- End of sync
END 

