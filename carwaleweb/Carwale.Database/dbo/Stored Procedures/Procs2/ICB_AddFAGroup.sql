IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ICB_AddFAGroup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ICB_AddFAGroup]
GO

	
  
CREATE PROCEDURE [dbo].[ICB_AddFAGroup]  
 @Id     NUMERIC,  
 @FAId    NUMERIC,  
 @GroupName   VARCHAR(100),  
 @Status    BIT OUTPUT  
 AS  
   
BEGIN  
 IF @Id = -1 --Insertion  
  BEGIN  
   SELECT ID FROM ICB_FAGroups   
   WHERE FAId = @FAId AND GroupName = @GroupName  
  
   IF @@ROWCOUNT = 0  
  
    BEGIN  
  
     INSERT INTO ICB_FAGroups  
     ( FAId, GroupName, IsActive  
     )   
      
     Values  
     ( @FAId, @GroupName, 1  
     )   
  
     SET @Status = 1  
  
    END   
  
   ELSE  
      
    SET @Status = 0  
  END  
  
 ELSE  
  BEGIN  
     
   UPDATE ICB_FAGroups  
   SET GroupName = @GroupName  
   WHERE Id = @Id  
      
   SET @Status = 1  
  END   
END  
  
  
  
  
