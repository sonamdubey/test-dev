IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AddState]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AddState]
GO

	-- =============================================        
-- Author:  Sushil Kumar on 13th July 2016 
-- Description: Insert new state into states table     
-- =============================================        
CREATE PROCEDURE [dbo].[AddState]      
@StateName VARCHAR(30)
,@StateId INT OUTPUT
AS        
BEGIN        
 
 INSERT INTO States( Name, IsDeleted ) VALUES( @StateName , 0 );
 -- return inserted stateid
 SET @StateId = SCOPE_IDENTITY();
	begin try
		 exec SyncStateWithMysql @StateId, @StateName;
  	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','AddState',ERROR_MESSAGE(),'SyncStateWithMysql',@StateId,GETDATE(),null)
	END CATCH			
END 

