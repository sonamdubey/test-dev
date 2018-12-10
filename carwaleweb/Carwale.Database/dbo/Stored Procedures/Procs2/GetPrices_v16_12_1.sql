IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPrices_v16_12_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPrices_v16_12_1]
GO

--==========================================================
-- Created : Vicky Lund, 20/07/2016
-- EXEC [GetPrices_v16_12_1] 229, 160
-- EXEC [GetPrices_v16_12_1] 552, 1
-- EXEC [GetPrices_v16_12_1] 198, 625
-- Modified : Vicky Lund, 05/10/2016, Create temp table schema
-- Modified : Sanjay Soni, 23/11/2016, Use Pq_PriceAvailabilityStatus status Table
--==========================================================
CREATE PROCEDURE [dbo].[GetPrices_v16_12_1] @ModelId INT
	,@CityId INT
AS
BEGIN
	SELECT CV.ID VersionId
		,ISNULL(CNCNP.AvgPrice, 0) AveragePrice
		,ISNULL(CONVERT(INT, NCSP.Price), 0) ExShowroomPrice
		,@CityId CityId
		,C.[Name] CityName
		,IsNull(PAS.IsVersionBlocked,0) AS IsVersionBlocked
		,PAS.ReasonText				
	FROM CarVersions CV WITH (NOLOCK)
	LEFT OUTER JOIN NewCarShowroomPrices NCSP WITH (NOLOCK) ON CV.ID = NCSP.CarVersionId
		AND NCSP.IsActive = 1
		AND NCSP.CityId = @CityId
	LEFT OUTER JOIN Con_NewCarNationalPrices CNCNP WITH (NOLOCK) ON CV.ID = CNCNP.VersionId
	LEFT OUTER JOIN Pq_PriceAvailabilityStatus PAS WITH (NOLOCK) ON CV.ID = PAS.VersionId
		AND PAS.CityId = @CityId
	INNER JOIN Cities C WITH (NOLOCK) ON C.ID = @CityId
		AND C.IsDeleted = 0
	WHERE CV.CarModelId = @ModelId
		AND CV.IsDeleted = 0
		AND CV.New = 1
END
