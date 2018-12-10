IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPqCitiesByModelIdAndStateId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPqCitiesByModelIdAndStateId]
GO

	
-- =============================================        
-- Author:  Ashish Verma        
-- Create date: 9 july 2014      
-- Description: Fetching all cities of the Sates  
-- EXEC [GetPqCitiesByModelIdAndStateId]458,1
-- =============================================        
CREATE PROCEDURE [dbo].[GetPqCitiesByModelIdAndStateId] @ModelId SMALLINT
	,@StateId SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from        
	-- interfering with SELECT statements.        
	SET NOCOUNT ON;

	SELECT DISTINCT ct.ID AS CityId
		,ct.NAME AS CityName
		,st.ID AS StateId
		,st.NAME AS StateName
	FROM Cities ct with(nolock)
	INNER JOIN NewCarShowroomPrices NCP with(nolock) ON NCP.CityId = ct.ID
	INNER JOIN CarVersions CV with(nolock) ON NCP.CarVersionId = CV.ID
	INNER JOIN States st with(nolock) ON st.ID = ct.StateId
	WHERE ct.IsDeleted = 0
		AND CV.New = 1
		AND NCP.IsActive = 1
		AND CV.CarModelId = @ModelId
		AND st.ID = @StateId
		ORDER BY ct.Name
END

