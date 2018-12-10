IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerCitiesView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerCitiesView]
GO

	-- =============================================        
-- Author:  Chetan Kane        
-- Create date: 4/5/2012      
-- Description: To view all the cities where dealer is operating  
-- Modified By: Nilesh Utture on 9th May, 2013 Added parameter page src
-- Modified By: Umesh on 28 June 2013 added order by
-- Modified BY: Tejashree Patil on 11 NOV 2014, Added DISTINCT Condition.
-- =============================================        
CREATE PROCEDURE [dbo].[TC_DealerCitiesView]    
(        
 @DealerId INT,  
 @PageSrc TINYINT = NULL 
)        
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
 SET NOCOUNT ON;    
 IF @PageSrc IS NULL
	BEGIN  
		 SELECT DISTINCT TD.CityId AS Value, C.Name AS Text 
		 FROM	TC_DealerCities AS TD WITH (NOLOCK) 
				INNER JOIN Cities AS C  WITH (NOLOCK)  on TD.CityID = C.ID       
		 WHERE	TD.DealerId = @DealerId AND TD.IsActive = 1  
		 Order By C.Name    
	END
ELSE IF(@PageSrc IS NOT NULL AND @PageSrc > 0)
	BEGIN
		 SELECT DISTINCT TD.CityId AS Id, C.Name AS Name 
		 FROM	TC_DealerCities AS TD WITH (NOLOCK) 
				INNER JOIN Cities AS C  WITH (NOLOCK)  on TD.CityID = C.ID       
		 WHERE	TD.DealerId = @DealerId AND TD.IsActive = 1 
		 Order By C.Name  
	END
ELSE IF (@PageSrc =0 AND @PageSrc IS NOT NULL)
	BEGIN  
		 SELECT DISTINCT C.Id AS Value, C.Name AS Text 
		 FROM	Cities AS C WITH (NOLOCK) 
		 WHERE	C.IsDeleted=0  
		 Order By C.Name    
	END
END


--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

