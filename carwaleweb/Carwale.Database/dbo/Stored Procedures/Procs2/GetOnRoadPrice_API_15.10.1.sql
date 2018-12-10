IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOnRoadPrice_API_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOnRoadPrice_API_15]
GO

	

-- =============================================
-- Author:		<Ashish VErma>
-- Create date: <19/8/2014>
-- Description:	<Description:For getting pricequote for Sponsored car based on cityId and versionId>
-- modified by Shalini Nair on 02/09/2015 added condition to not retrieve PQcategoryId 1 and 2 'Price before Local tax' and 'Local Tax'
-- modified by Sanjay Soni on 09/09/2015 filtered prices null values 
-- =============================================
CREATE  PROCEDURE [dbo].[GetOnRoadPrice_API_15.10.1]
	-- Add the parameters for the stored procedure here
	-- Parameters For NewCarPurchaseInquiries
	@CarVersionId INT
	,@CityId INT
AS
BEGIN
	
		---Data Reader first
		SELECT PQC.CategoryId
			,Ci.Id AS CategoryItemId
			,CI.CategoryName AS categoryItem
			,PQN.PQ_CategoryItemValue AS Value
			,PQLT.IsTaxOnTax
			,PQN.isMetallic
		FROM CW_NewCarShowroomPrices PQN WITH (NOLOCK)
		INNER JOIN PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = PQN.PQ_CategoryItem
		INNER JOIN PQ_Category PQC WITH (NOLOCK) ON PQC.CategoryId = CI.CategoryId
		LEFT JOIN PriceQuote_LocalTax PQLT WITH (NOLOCK) ON CI.Id = PQLT.CategoryItemid
			AND PQLT.CityId = @CityId
		WHERE CarVersionId = @CarVersionId
			AND PQN.CityId = @CityId
			AND PQN.PQ_CategoryItemValue IS NOT NULL -- filtered Null Values
			AND PQC.CategoryId NOT IN (1,2,7) -- not retrieving 'Price before local tax', 'Local Tax', 'Optional Charges'(categoryId= 1, 2, 7) respectively
					ORDER BY PQC.SortOrder ASC
END


