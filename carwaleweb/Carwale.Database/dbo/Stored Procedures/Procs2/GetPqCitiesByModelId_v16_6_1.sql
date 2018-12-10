IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPqCitiesByModelId_v16_6_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPqCitiesByModelId_v16_6_1]
GO

	
-- =============================================        
-- Author:  Ashish Verms        
-- Create date: 9 july 2014      
-- Description: Fetching all cities of the Sates
--exec [GetPqCitiesByModelId] 458,0      
-- Modified By: Shalini Nair on 31/05/2016 to bring states as well
-- =============================================        
create PROCEDURE [dbo].[GetPqCitiesByModelId_v16_6_1] @ModelId SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from        
	-- interfering with SELECT statements.        
	SET NOCOUNT ON;

		SELECT DISTINCT ct.ID AS CityId
			,ct.NAME AS CityName
			,st.ID as StateId
			,st.Name as StateName
		FROM Cities ct with(nolock)
		INNER JOIN NewCarShowroomPrices NCP with(nolock) ON NCP.CityId = ct.ID
		INNER JOIN CarVersions CV with(nolock) ON NCP.CarVersionId = CV.ID
		INNER JOIN States st with(nolock) ON st.ID = ct.StateId
		WHERE ct.IsDeleted = 0
			AND CV.New = 1
			AND NCP.IsActive = 1
			AND CV.CarModelId = @ModelId
		ORDER BY ct.Name 
END

/****** Object:  StoredProcedure [dbo].[WA_NewsDetail_16.6.1]    Script Date: 6/6/2016 1:51:28 PM ******/
SET ANSI_NULLS ON
