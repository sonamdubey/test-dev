IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SaveCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SaveCities]
GO

	CREATE PROCEDURE [dbo].[DCRM_SaveCities]  
 @Id    NUMERIC,  
 @CityId   NUMERIC,  
 @RegionId  NUMERIC,  
 @Status   INT OUTPUT   
AS  
  
BEGIN  
 IF @ID = -1   
      BEGIN  
			 INSERT INTO DCRM_ADM_RegionCities  
			 (   
			  RegionId, CityId   
			 )   
			 VALUES  
			 (   
			  @RegionId, @CityId   
			 )  
		      
			 SET @Status = 1   
		END  
 ELSE	 
		BEGIN
			UPDATE DCRM_ADM_RegionCities SET RegionId = @RegionId 
			WHERE CityId = @Id
			
			 SET @Status = 1   
		END
END