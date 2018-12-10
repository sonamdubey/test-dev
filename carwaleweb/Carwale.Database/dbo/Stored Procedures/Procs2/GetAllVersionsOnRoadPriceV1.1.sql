IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllVersionsOnRoadPriceV1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAllVersionsOnRoadPriceV1]
GO

	-- =============================================
-- Author:		Ashish Verma						EXEC GetAllVersionsOnRoadPrice 99,13
-- Create date: 14/7/2014
-- Description:	Get All Versions On RoadPrice
-- =============================================
CREATE PROCEDURE [dbo].[GetAllVersionsOnRoadPriceV1.1]
	-- Add the parameters for the stored procedure here
	@ModelId INT = 0,
	@CityId INT,
	@VersionId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @ModelId = CarModelId from CarVersions WITH(NOLOCK) where ID = @VersionId

	SELECT CV.Name VersionName, CV.ID VersionId ,SUM(PQN.PQ_CategoryItemValue) OnRoadPrice
	FROM CW_NewCarShowroomPrices PQN WITH(NOLOCK)
	INNER JOIN PQ_CategoryItems CI WITH(NOLOCK) ON CI.Id = PQN.PQ_CategoryItem
	INNER JOIN PQ_Category PQC WITH(NOLOCK) ON PQC.CategoryId = CI.CategoryId
	INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.ID = PQN.CarVersionId AND CV.New = 1 AND CV.IsDeleted = 0
	LEFT JOIN PriceQuote_LocalTax PQLT WITH(NOLOCK) ON CI.Id = PQLT.CategoryItemid AND PQLT.CityId = @CityId
	WHERE CV.CarModelId = @ModelId AND PQN.CityId = @CityId AND PQC.CategoryId > 1 AND (PQLT.IsTaxOnTax = 0 OR PQLT.IsTaxOnTax IS NULL OR PQLT.IsTaxOnTax = '')
	GROUP BY CV.ID,CV.Name ORDER BY OnRoadPrice ASC
	
END


