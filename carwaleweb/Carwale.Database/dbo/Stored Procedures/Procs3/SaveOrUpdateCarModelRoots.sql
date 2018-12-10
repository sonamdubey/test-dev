IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveOrUpdateCarModelRoots]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveOrUpdateCarModelRoots]
GO

	-- ============================================= 
-- Author: Prashant vishe    
-- Create date: <29 Jan 2013> 
-- Description:  for saving and updating Car Model roots 
-- Modified by Ashwini Todkar on 10 Oct 2015 retrieved @RootId 
-- ============================================= 
CREATE PROCEDURE [dbo].[SaveOrUpdateCarModelRoots] 
  -- exec SaveOrUpdateCarModelRoots 'A6',18,-1,0 
  -- Add the parameters for the stored procedure here 
  @RootName VARCHAR(50), 
  @MakeId   SMALLINT, 
  @Id       SMALLINT,
  @RootId  INT out --added by ashwini todkar
AS 
  BEGIN 
      -- SET NOCOUNT ON added to prevent extra result sets from 
      -- Insert statements for procedure here 
      IF  @Id is null or @Id = -1 -- modified from -1 to NULL
        BEGIN			
            IF NOT EXISTS (SELECT rootid 
                           FROM   carmodelroots  WITH(NOLOCK)
                           WHERE  rootname = @RootName 
                                  AND makeid = @MakeId) 
              BEGIN 			 
                  INSERT INTO carmodelroots 
                              (rootname, 
                               makeid) 
                  VALUES      ( @RootName, 
                                @MakeId ) 
				  SET @RootId = Scope_identity()
				  
				  begin try
					exec SyncCarModelRootsWithMysql @RootId,@RootName,@MakeId,1;
				  end try
				BEGIN CATCH
					INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
					VALUES('MysqlSync','SaveOrUpdateCarModelRoots',ERROR_MESSAGE(),'SyncCarModelRootsWithMysql',@RootId,GETDATE(),1)
				END CATCH		
              END 
        END 
      ELSE 
        BEGIN 
            UPDATE carmodelroots 
            SET    rootname = @RootName, 
                   makeid = @MakeId 
            WHERE  rootid = @Id 
			SET @RootId = @Id
			begin try
				exec SyncCarModelRootsWithMysql @RootId,@RootName,@MakeId,2;
			end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','SaveOrUpdateCarModelRoots',ERROR_MESSAGE(),'SyncCarModelRootsWithMysql',@RootId,GETDATE(),2)
			END CATCH		
        END 
  END 

