IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPqCityGroupsByModelId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPqCityGroupsByModelId]
GO

	



-- =============================================        
-- Author:  Shalini Nair      
-- Create date: 15/04/2016
-- Description: Fetching all pq city groups of a model
-- exec [GetPqCityGroupsByModelId] 28
-- =============================================        
CREATE PROCEDURE [dbo].[GetPqCityGroupsByModelId] @ModelId SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from        
	-- interfering with SELECT statements.        
	SET NOCOUNT ON;

	SELECT DISTINCT PG.CityName
		,PG.GroupMasterId
		,PG.DisplayOrder
		,PG.CityId
		,PCG.NAME AS GroupName
	FROM PQCityGroups PG WITH (NOLOCK)
	INNER JOIN NewCarShowroomPrices NCP WITH (NOLOCK) ON NCP.CityId = PG.CityId
	INNER JOIN CarVersions CV WITH (NOLOCK) ON NCP.CarVersionId = CV.ID
	INNER JOIN PQCityGroupsMaster PCG WITH (NOLOCK) ON PCG.Id = PG.GroupMasterId
	INNER JOIN Cities CT WITH (NOLOCK) ON CT.ID = PG.CityId
	WHERE PG.IsActive = 1
		AND CV.New = 1
		AND NCP.IsActive = 1
		AND CV.CarModelId = @ModelId
	ORDER BY GroupMasterId
		,DisplayOrder
END


