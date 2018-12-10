IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ICB_AddStampDuty]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ICB_AddStampDuty]
GO

	
 
CREATE PROCEDURE [dbo].[ICB_AddStampDuty]  
 @Id     NUMERIC,  
 @FAID    NUMERIC,  
 @CityId    NUMERIC,  
 @Rate    DECIMAL(6,3),  
 @FixedFee   NUMERIC,  
 @ServiceTax   DECIMAL(5,2),  
 @StartPrice   DECIMAL(18,2),  
 @EndPrice   DECIMAL(18,2),  
 @LastUpdated  DATETIME,  
 @Status    BIT OUTPUT  
 AS  
   
BEGIN  
 SET @Status = 0  
 IF @Id = -1 --Insertion  
  BEGIN  
      
     INSERT INTO ICB_StampDuty  
     (	FAID, CityId, Rate, FixedFee, ServiceTax, StartPriceRange, EndPriceRange, LastUpdated )   
     Values ( @FAID, @CityId, @Rate, @FixedFee, @ServiceTax, @StartPrice, @EndPrice, @LastUpdated )   
  
     SET @Status = 1  
  END    
  
 ELSE   
    BEGIN  
     UPDATE ICB_StampDuty  
		 SET CityId = @CityId, Rate = @Rate, FixedFee = @FixedFee,   
		 ServiceTax = @ServiceTax, StartPriceRange = @StartPrice,  
		 EndPriceRange = @EndPrice, LastUpdated = @LastUpdated   
     WHERE ID = @Id  
  
     SET @Status = 1  
    END    
    
END  
  
  
  
  
  
  
  
  
  
