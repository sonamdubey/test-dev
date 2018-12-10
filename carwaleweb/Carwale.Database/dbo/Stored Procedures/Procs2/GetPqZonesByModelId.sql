IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPqZonesByModelId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPqZonesByModelId]
GO

	


-- =============================================        
-- Author:  Shalini Nair      
-- Create date: 15/04/2016
-- Description: Fetching all pq zones of a model
-- exec [GetPqZonesByModelId] 458
-- =============================================        
CREATE PROCEDURE [dbo].[GetPqZonesByModelId] @ModelId SMALLINT
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
		,CT.NAME AS CityName
		,PCG.NAME AS GroupName
	FROM CityZones CZ WITH (NOLOCK)
	INNER JOIN NewCarShowroomPrices NCP WITH (NOLOCK) ON NCP.CityId = CZ.CityId
	INNER JOIN CarVersions CV WITH (NOLOCK) ON NCP.CarVersionId = CV.ID
	INNER JOIN PQCityGroupsMaster PCG WITH (NOLOCK) ON PCG.Id = CZ.CityType
	INNER JOIN Cities CT WITH (NOLOCK) ON CT.ID = CZ.CityId
	WHERE CZ.IsActive = 1
		AND CV.New = 1
		AND NCP.IsActive = 1
		AND CV.CarModelId = @ModelId
		AND CZ.ActualCityId IS NULL
	ORDER BY CityType
		,DisplayOrder
END


