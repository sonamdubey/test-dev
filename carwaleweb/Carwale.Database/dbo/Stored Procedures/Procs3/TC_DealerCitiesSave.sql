IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerCitiesSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerCitiesSave]
GO

	-- =============================================    
-- Author:  Chetan Kane    
-- Create date: 11th Aug,2011  4/4/2012  
-- Description: view Content for aboutus    
-- Modified by Nilesh on 17-09-2013 for maintaining logs of Dealers masters for Mobile APP
-- =============================================    
CREATE PROCEDURE [dbo].[TC_DealerCitiesSave]     
(    
 @DealerId INT,
 @CityId INT
)    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 SET NOCOUNT ON;     
 IF NOT EXISTS (SELECT * FROM TC_DealerCities WHERE DealerId = @DealerId AND CityId = @CityId)
	BEGIN    
		INSERT INTO TC_DealerCities(DealerId,CityId)   
		VALUES(@DealerId,@CityId)  
		
		------------------------below code is added by Nilesh on 17-09-2013 for maintaining logs of Dealers masters for Mobile APP
				
						    INSERT INTO TC_DealerMastersLog( DealerId,TableName,CreatedOn)
						    Values                         (@DealerId,'TC_DealerCities',GETDATE())
				
	  ----------------------------------------------------------------------------------------------------------------------
					
		
	END   
 ELSE
	BEGIN
		UPDATE TC_DealerCities SET IsActive = 1 WHERE DealerId = @DealerId AND CityId = @CityId
		
		------------------------below code is added by Nilesh on 17-09-2013 for maintaining logs of Dealers masters for Mobile APP
				
						    INSERT INTO TC_DealerMastersLog( DealerId,TableName,CreatedOn)
						    Values                         (@DealerId,'TC_DealerCities',GETDATE())
				
	 ----------------------------------------------------------------------------------------------------------------------
					
		
	END		
END 







/****** Object:  StoredProcedure [dbo].[TC_DealerCityRemove]    Script Date: 09/17/2013 19:02:04 ******/
SET ANSI_NULLS ON
