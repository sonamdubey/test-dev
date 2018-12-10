IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOnRoadPrice_API]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOnRoadPrice_API]
GO

	
-- =============================================
-- Author:		<Ashish VErma>
-- Create date: <19/8/2014>
-- Description:	<Description:For getting pricequote for Sponsored car based on cityId and versionId>
-- =============================================
CREATE PROCEDURE [dbo].[GetOnRoadPrice_API]
	-- Add the parameters for the stored procedure here
	-- Parameters For NewCarPurchaseInquiries
	@CarVersionId NUMERIC(18, 0)
	,@CityId NUMERIC(18, 0)
AS
BEGIN
	
		---Data Reader first
		SELECT PQC.CategoryId
			,Ci.Id AS CategoryItemId
			,CI.CategoryName AS categoryItem
			,PQN.PQ_CategoryItemValue AS Value
			,PQLT.IsTaxOnTax
		FROM CW_NewCarShowroomPrices PQN WITH (NOLOCK)
		INNER JOIN PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = PQN.PQ_CategoryItem
		INNER JOIN PQ_Category PQC WITH (NOLOCK) ON PQC.CategoryId = CI.CategoryId
		LEFT JOIN PriceQuote_LocalTax PQLT WITH (NOLOCK) ON CI.Id = PQLT.CategoryItemid
			AND PQLT.CityId = @CityId
		WHERE CarVersionId = @CarVersionId
			AND PQN.CityId = @CityId
		ORDER BY PQC.SortOrder ASC
END

