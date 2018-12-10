IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPqCityGroupsByModelId_v16_6_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPqCityGroupsByModelId_v16_6_1]
GO

	
-- =============================================        
-- Author:  Shalini Nair      
-- Create date: 15/04/2016
-- Description: Fetching all pq city groups of a model
-- exec [GetPqCityGroupsByModelId] 28
-- Modified by Sanjay Soni 06-06-2016 : Stateid and StateName added in select query 
-- =============================================        
create PROCEDURE [dbo].[GetPqCityGroupsByModelId_v16_6_1] @ModelId SMALLINT
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
		,CT.StateId
		,ST.Name AS StateName
	FROM PQCityGroups PG WITH (NOLOCK)
	INNER JOIN NewCarShowroomPrices NCP WITH (NOLOCK) ON NCP.CityId = PG.CityId
	INNER JOIN CarVersions CV WITH (NOLOCK) ON NCP.CarVersionId = CV.ID
	INNER JOIN PQCityGroupsMaster PCG WITH (NOLOCK) ON PCG.Id = PG.GroupMasterId
	INNER JOIN Cities CT WITH (NOLOCK) ON CT.ID = PG.CityId 
	INNER JOIN States ST WITH (NOLOCK) ON CT.StateId = ST.ID -- Modified by Sanjay Soni 06-06-2016 : Stateid and StateName added in select query 
	WHERE PG.IsActive = 1
		AND CV.New = 1
		AND NCP.IsActive = 1
		AND CV.CarModelId = @ModelId
		AND ST.IsDeleted=0
		AND CT.IsDeleted=0
	ORDER BY GroupMasterId
		,DisplayOrder

END
