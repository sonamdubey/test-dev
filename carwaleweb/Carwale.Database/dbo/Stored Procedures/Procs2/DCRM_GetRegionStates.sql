IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetRegionStates]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetRegionStates]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(12th Jan 2015)
-- Description	:	Get states based on region
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetRegionStates]
	
	@RegionId	SMALLINT = NULL
AS
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT 
		DISTINCT S.ID AS Value,
		S.Name AS Text 
	FROM 
		DCRM_ADM_RegionCities RC (NOLOCK) 
		INNER JOIN  Cities C(NOLOCK) ON C.ID = RC.CityId AND C.IsDeleted = 0
		INNER JOIN States S(NOLOCK) ON S.ID = C.StateId
	WHERE 
		RC.RegionId = @RegionId 
	ORDER BY 
		Text
END

