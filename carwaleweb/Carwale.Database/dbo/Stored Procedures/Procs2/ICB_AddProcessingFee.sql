IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ICB_AddProcessingFee]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ICB_AddProcessingFee]
GO

	
--THIS PROCEDURE INSERTS THE VALUES FOR ICB-Processing Fee  
  
CREATE PROCEDURE [dbo].[ICB_AddProcessingFee]  
 @Id     NUMERIC,  
 @FAId    NUMERIC,  
 @StartPrice   DECIMAL(18,2),  
 @EndPrice   DECIMAL(18,2),  
 @Fee    DECIMAL(5,2),  
 @LastUpdated  DATETIME,  
 @Status    BIT OUTPUT  
 AS  
   
BEGIN  
 IF @Id = -1 --Insertion  
  BEGIN  
   SELECT ID FROM ICB_ProcessingFee   
   WHERE FAId = @FAId AND StartPrice = @StartPrice  
     AND EndPrice = @EndPrice  
  
   IF @@ROWCOUNT = 0  
  
    BEGIN  
  
     INSERT INTO ICB_ProcessingFee  
     ( FAId, StartPrice,   
      EndPrice, Fee, LastUpdated   
     )   
      
     Values  
     ( @FAId, @StartPrice,   
      @EndPrice, @Fee, @LastUpdated   
     )   
  
     SET @Status = 1  
  
    END   
  
   ELSE  
      
    SET @Status = 0  
  END  
  
 ELSE  
  BEGIN  
     
   UPDATE ICB_ProcessingFee  
   SET StartPrice = @StartPrice,   
    EndPrice = @EndPrice, Fee = @Fee,   
    LastUpdated = @LastUpdated  
   WHERE Id = @Id  
      
   SET @Status = 1  
  END   
END  
  
  
  
  
