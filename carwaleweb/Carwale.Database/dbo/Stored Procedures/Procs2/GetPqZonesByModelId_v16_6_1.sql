IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPqZonesByModelId_v16_6_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPqZonesByModelId_v16_6_1]
GO

	
-- =============================================        
-- Author:  Shalini Nair      
-- Create date: 15/04/2016
-- Description: Fetching all pq zones of a model
-- exec [GetPqZonesByModelId] 458
-- Modified by: Shalini Nair on 31/05/2016 to fetch states also
-- =============================================        
create PROCEDURE [dbo].[GetPqZonesByModelId_v16_6_1]
 @ModelId SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from        
	-- interfering with SELECT statements.        
	SET NOCOUNT ON;

	SELECT DISTINCT cz.ZoneName
		,CZ.ID AS ZoneId
		,CZ.CityType as GroupMasterId
		,CZ.DisplayOrder
		,CZ.CityId
		,CT.Name as CityName
		,PCG.NAME AS GroupName
		,ST.ID AS StateId
		,ST.Name AS StateName
	FROM CityZones CZ WITH (NOLOCK)
	INNER JOIN NewCarShowroomPrices NCP WITH (NOLOCK) ON NCP.CityId = CZ.CityId
	INNER JOIN CarVersions CV WITH (NOLOCK) ON NCP.CarVersionId = CV.ID
	INNER JOIN PQCityGroupsMaster PCG WITH (NOLOCK) ON PCG.Id = CZ.CityType
	INNER JOIN Cities CT WITH (NOLOCK) ON CT.ID = CZ.CityId
	INNER JOIN States ST WITH(NOLOCK) ON ST.ID = CT.StateId
	WHERE CZ.IsActive = 1
		AND CV.New = 1
		AND NCP.IsActive = 1
		AND CV.CarModelId = @ModelId
		AND CZ.ActualCityId IS NULL
	ORDER BY CityType
		,DisplayOrder
END
/****** Object:  StoredProcedure [dbo].[GetPqCitiesByModelId_v16_6_1]    Script Date: 6/6/2016 1:50:51 PM ******/
SET ANSI_NULLS ON
