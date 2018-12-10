IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPQPopularCitiesByModelId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPQPopularCitiesByModelId]
GO

	
-- =============================================
-- Author:		Rohan Sapkal
-- Create date: 20-03-2015
-- Description:	Popular Cities for PQ
-- =============================================
CREATE PROCEDURE [dbo].[GetPQPopularCitiesByModelId] @ModelId SMALLINT --exec [dbo].[GetPQPopularCitiesByModelId] 28
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from        
	-- interfering with SELECT statements.        
	SET NOCOUNT ON;

	SELECT DISTINCT PPQ.CityId AS CityId
		,PPQ.NAME AS CityName
		,PPQ.CityImageUrl
		,PPQ.DisplayText
		,PPQ.DisplayOrder
	FROM PopularCities PPQ WITH (NOLOCK)
	--INNER JOIN NewCarShowroomPrices NCP WITH (NOLOCK) ON NCP.CityId = PPQ.CityId
	INNER JOIN CW_NewCarShowroomPrices NCP WITH(NOLOCK) on NCP.CityId =PPQ.CityId
	INNER JOIN CarVersions CV WITH (NOLOCK) ON NCP.CarVersionId = CV.ID AND CV.New=1
	WHERE CV.CarModelId = @ModelId
		AND PPQ.isActive=1
		AND CV.IsDeleted=0
	ORDER BY PPQ.DisplayOrder
END



/****** Object:  StoredProcedure [dbo].[GetModelOnBranchId]    Script Date: 3/30/2015 4:43:26 PM ******/
SET ANSI_NULLS ON
