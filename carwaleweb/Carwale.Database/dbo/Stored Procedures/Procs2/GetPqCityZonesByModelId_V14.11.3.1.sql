IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPqCityZonesByModelId_V14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPqCityZonesByModelId_V14]
GO

	
-- =============================================        
-- Author:  Vikas J        
-- Create date: 27 nov 2014      
-- Description: Fetching all cities and zones of a model
--exec [GetPqCityZonesByModelId] 458
-- =============================================        
CREATE PROCEDURE [dbo].[GetPqCityZonesByModelId_V14.11.3.1] 
@ModelId SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from        
	-- interfering with SELECT statements.        
	SET NOCOUNT ON;

	SELECT DISTINCT  
                       cz.ZoneName
                       ,CZ.ID AS ZoneId
                       ,CZ.CityType
                       ,CZ.DisplayOrder
					   ,CZ.CityId
					   
			FROM CityZones CZ
               INNER JOIN NewCarShowroomPrices NCP with(nolock) ON NCP.CityId = CZ.CityId
               INNER JOIN CarVersions CV with(nolock) ON NCP.CarVersionId = CV.ID
			  
			   WHERE         
					CZ.IsActive =1
                    AND CV.New = 1
                    AND NCP.IsActive = 1
                    AND CV.CarModelId = @ModelId
     
   ORDER BY CityType,DisplayOrder

END

