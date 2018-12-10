IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPqCityZonesByModelId_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPqCityZonesByModelId_V16]
GO

	


-- =============================================        
-- Author:  Vikas J        
-- Create date: 27 nov 2014      
-- Description: Fetching all cities and zones of a model
--exec [GetPqCityZonesByModelId] 458
-- Modified By: Shalini Nair on 20/04/2016 to alias CityType as GroupMasterId
-- =============================================        
CREATE PROCEDURE [dbo].[GetPqCityZonesByModelId_V16.4.6] @ModelId SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from        
	-- interfering with SELECT statements.        
	SET NOCOUNT ON;

	SELECT DISTINCT cz.ZoneName
		,CZ.ID AS ZoneId
		,CZ.CityType AS GroupMasterId
		,CZ.DisplayOrder
		,CZ.CityId
	FROM CityZones CZ WITH (NOLOCK)
	INNER JOIN NewCarShowroomPrices NCP WITH (NOLOCK) ON NCP.CityId = CZ.CityId
	INNER JOIN CarVersions CV WITH (NOLOCK) ON NCP.CarVersionId = CV.ID
	WHERE CZ.IsActive = 1
		AND CV.New = 1
		AND NCP.IsActive = 1
		AND CV.CarModelId = @ModelId
	ORDER BY CityType
		,DisplayOrder
END


