IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPopularPqCitiesByModelId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPopularPqCitiesByModelId]
GO

	
-- =============================================        
-- Author:  Ashish Verma        
-- Create date: 9 july 2014      
-- Description: Fetching all cities of the Sates  
-- EXEC [GetPqCitiesByModelIdAndStateId]458,1
-- =============================================        
CREATE PROCEDURE [dbo].[GetPopularPqCitiesByModelId] @ModelId SMALLINT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from        
	-- interfering with SELECT statements.        
	SET NOCOUNT ON;

	SELECT  ct.ID AS CityId
		,ct.NAME AS CityName
		,ct.CityImageUrl AS ImageUrl
		--,st.ID AS StateId
		--,st.NAME AS StateName
	FROM Cities ct with(nolock)
	INNER JOIN NewCarShowroomPrices NCP with(nolock) ON NCP.CityId = ct.ID
	INNER JOIN CarVersions CV with(nolock) ON NCP.CarVersionId = CV.ID
	--INNER JOIN States st with(nolock) ON st.ID = ct.StateId
	
	WHERE ct.IsDeleted = 0
		AND CV.New = 1
		AND NCP.IsActive = 1
		AND CV.CarModelId = @ModelId
		AND ct.IsPopular = 1
		Group By ct.ID 
		,ct.NAME
		,ct.CityImageUrl --modified by ashish verma on 16/09/2014(removed distinct and added Group By)
		ORDER BY 
		
			case ct.Id 
			when '1' then 1
			when '10' then 2
			when '2' then 3
			when '176' then 4
			when '12' then 5
			when '105' then 6 
			when '198' then 7
			when '128' then 8
			when '244' then 9        --modified by ashish verma on 16/09/2014  for ordering zones.
			end
END


