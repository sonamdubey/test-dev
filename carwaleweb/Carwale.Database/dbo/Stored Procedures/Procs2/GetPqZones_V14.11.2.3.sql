IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPqZones_V14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPqZones_V14]
GO

	
-- =============================================        
-- Author:  Ashish Verma        
-- Create date: 9 july 2014      
-- Description: Fetching all cities of the Sates  
-- EXEC [GetPqZones]101,1
-- modified by ashish verma (for showing zone names in particular order) on 06/10/2014
-- modified by ashish verma (for showing only active zone names) on 06/10/2014
-- =============================================        
CREATE PROCEDURE [dbo].[GetPqZones_V14.11.2.3] 
@ModelId SMALLINT
,@CityId SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from        
	-- interfering with SELECT statements.        
	SET NOCOUNT ON;
	IF @CityId = 1
		SELECT DISTINCT ct.ID AS CityId
			,ct.NAME AS CityName
			,cz.Id AS ZoneId
			,cz.ZoneName AS ZoneName,cz.DisplayOrder
		FROM Cities ct with(nolock)
		INNER JOIN NewCarShowroomPrices NCP with(nolock) ON NCP.CityId = ct.ID
		INNER JOIN CarVersions CV with(nolock) ON NCP.CarVersionId = CV.ID
		INNER JOIN CityZones cz with(nolock) ON ct.ID = cz.CityId
		WHERE ct.IsDeleted = 0
			AND CV.New = 1
			AND NCP.IsActive = 1
			AND CV.CarModelId = @ModelId
			AND (ct.ID = @CityId OR ct.ID IN (13,40,395,6,8)) and cz.IsActive =1 -- modified by ashish verma (for showing only active zone names)
		ORDER BY cz.DisplayOrder asc -- modified by ashish verma (for showing zone names in particular order)

	ELSE IF @CityId = 10
		SELECT DISTINCT ct.ID AS CityId
			,ct.NAME AS CityName
			,cz.Id AS ZoneId
			,cz.ZoneName AS ZoneName,cz.DisplayOrder
		FROM Cities ct with(nolock)
		INNER JOIN NewCarShowroomPrices NCP with(nolock) ON NCP.CityId = ct.ID
		INNER JOIN CarVersions CV with(nolock) ON NCP.CarVersionId = CV.ID
		INNER JOIN CityZones cz with(nolock) ON ct.ID = cz.CityId
		WHERE ct.IsDeleted = 0
			AND CV.New = 1
			AND NCP.IsActive = 1
			AND CV.CarModelId = @ModelId
			AND (ct.ID = @CityId OR ct.ID IN (246,224,225,273)) and cz.IsActive =1 -- modified by ashish verma (for showing only active zone names)
		ORDER BY cz.DisplayOrder asc-- modified by ashish verma (for showing zone names in particular order)
END


