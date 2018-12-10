IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[sp_SyncCarModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[sp_SyncCarModel]
GO

	CREATE PROCEDURE [dbo].[sp_SyncCarModel]
       @modelId NUMERIC(18,0),                      
       @modelname VARCHAR(30),      
       @createdon DATETIME,                  
       @createdby NUMERIC(18,0),      
       @servername VARCHAR(30),     
       @isreplicated BIT                           
AS 
BEGIN 
     SET NOCOUNT ON 

     INSERT INTO Con_SyncModel
          ( 
            ModelId,
            ModelName,
            CreatedOn,
            CreatedBy,
            ServerName,
            IsReplicated                  
          ) 
     VALUES 
          ( 
            @modelId,
            @modelname,
            @createdon,
            @createdby,
            @servername,
            @isreplicated                
          ) 
END 


