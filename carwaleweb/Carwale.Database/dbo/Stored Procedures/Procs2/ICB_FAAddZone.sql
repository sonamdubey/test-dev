IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ICB_FAAddZone]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ICB_FAAddZone]
GO

	
  
CREATE PROCEDURE [dbo].[ICB_FAAddZone]  
 @Id     NUMERIC,  
 @FAId    NUMERIC,  
 @ZoneName   VARCHAR(100),  
 @Status    BIT OUTPUT  
 AS  
   
BEGIN  
 IF @Id = -1 --Insertion  
  BEGIN  
   SELECT ID FROM ICB_FAZones   
   WHERE FAId = @FAId AND ZoneName = @ZoneName  
  
   IF @@ROWCOUNT = 0  
  
    BEGIN  
  
     INSERT INTO ICB_FAZones  
     ( FAId, ZoneName, IsActive  
     )   
      
     Values  
     ( @FAId, @ZoneName, 1  
     )   
  
     SET @Status = 1  
  
    END   
  
   ELSE  
      
    SET @Status = 0  
  END  
  
 ELSE  
  BEGIN  
     
   UPDATE ICB_FAZones  
   SET ZoneName = @ZoneName  
   WHERE Id = @Id  
      
   SET @Status = 1  
  END   
END  
  
  
  
  
