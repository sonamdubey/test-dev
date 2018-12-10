IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_GetOnRoadPrice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_GetOnRoadPrice]
GO

	-- =============================================
-- Author:		Raghupathy					EXEC [dbo].[GetNewOnRoadPrice] 6865--,776,13
-- Create date: 14/10/2013
-- Description:	get new on-road price
-- Modified By : Raghu on <27/12/2013> Added WITH(NOLOCK) Conditions on table
-- =============================================
CREATE procedure [dbo].[CW_GetOnRoadPrice] 
	-- Add the parameters for the stored procedure here
	@PQId numeric(18,0)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @PQCarversionId INT
	DECLARE @PQCityId INT

	SELECT @PQCarversionId = NCP.CarVersionId, @PQCityId = NPC.CityId
	FROM NewCarPurchaseInquiries NCP WITH(NOLOCK)
	INNER JOIN NewPurchaseCities NPC WITH(NOLOCK) ON NCP.Id = NPC.InquiryId
	WHERE Id = @PQId

	PRINT @PQCarversionId
	PRINT @PQCityId

	IF(@PQCarversionId > 0 AND @PQCityId > 0)
	BEGIN
		DECLARE @PQCarModelId INT = (SELECT CarModelId FROM CarVersions WHERE ID = @PQCarversionId)

		SELECT PQC.CategoryId, Ci.Id AS CategoryItemId, CI.CategoryName, PQN.PQ_CategoryItemValue AS Value,PQLT.IsTaxOnTax
		FROM CW_NewCarShowroomPrices PQN WITH(NOLOCK)
		INNER JOIN PQ_CategoryItems CI WITH(NOLOCK) ON CI.Id = PQN.PQ_CategoryItem
		INNER JOIN PQ_Category PQC WITH(NOLOCK) ON PQC.CategoryId = CI.CategoryId
		LEFT JOIN PriceQuote_LocalTax PQLT WITH(NOLOCK) ON CI.Id = PQLT.CategoryItemid AND PQLT.CityId = @PQCityId
		--LEFT JOIN PriceQuote_LocalTax PQLT WITH(NOLOCK) ON PQN.CityId = PQLT.CityId AND PQLT.CityId = @PQCityId
		WHERE CarVersionId = @PQCarversionId AND PQN.CityId = @PQCityId
		ORDER BY PQC.SortOrder ASC
	END
END


/****** Object:  StoredProcedure [dbo].[GetDetailsofSimilarVersions]    Script Date: 12/30/2013 2:02:22 PM ******/
SET ANSI_NULLS ON

