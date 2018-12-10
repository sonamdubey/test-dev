IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPqZones]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPqZones]
GO

	
-- =============================================        
-- Author:  Ashish Verma        
-- Create date: 9 july 2014      
-- Description: Fetching all cities of the Sates  
-- EXEC [GetPqZones]101,1
-- =============================================        
CREATE PROCEDURE [dbo].[GetPqZones] 
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
			,cz.ZoneName AS ZoneName
		FROM Cities ct with(nolock)
		INNER JOIN NewCarShowroomPrices NCP with(nolock) ON NCP.CityId = ct.ID
		INNER JOIN CarVersions CV with(nolock) ON NCP.CarVersionId = CV.ID
		INNER JOIN CityZones cz with(nolock) ON ct.ID = cz.CityId
		WHERE ct.IsDeleted = 0
			AND CV.New = 1
			AND NCP.IsActive = 1
			AND CV.CarModelId = @ModelId
			AND (ct.ID = @CityId OR ct.ID IN (13,40,395,6,8))
	ELSE IF @CityId = 10
		SELECT DISTINCT ct.ID AS CityId
			,ct.NAME AS CityName
			,cz.Id AS ZoneId
			,cz.ZoneName AS ZoneName
		FROM Cities ct with(nolock)
		INNER JOIN NewCarShowroomPrices NCP with(nolock) ON NCP.CityId = ct.ID
		INNER JOIN CarVersions CV with(nolock) ON NCP.CarVersionId = CV.ID
		INNER JOIN CityZones cz with(nolock) ON ct.ID = cz.CityId
		WHERE ct.IsDeleted = 0
			AND CV.New = 1
			AND NCP.IsActive = 1
			AND CV.CarModelId = @ModelId
			AND (ct.ID = @CityId OR ct.ID IN (246,224,225,273))
	ELSE
		SELECT DISTINCT ct.ID AS CityId
			,ct.NAME AS CityName
			,cz.Id AS ZoneId
			,cz.ZoneName AS ZoneName
		FROM Cities ct with(nolock)
		INNER JOIN NewCarShowroomPrices NCP with(nolock) ON NCP.CityId = ct.ID
		INNER JOIN CarVersions CV with(nolock) ON NCP.CarVersionId = CV.ID
		INNER JOIN CityZones cz with(nolock) ON ct.ID = cz.CityId
		WHERE ct.IsDeleted = 0
			AND CV.New = 1
			AND NCP.IsActive = 1
			AND CV.CarModelId = @ModelId
			AND ct.ID = @CityId
END

