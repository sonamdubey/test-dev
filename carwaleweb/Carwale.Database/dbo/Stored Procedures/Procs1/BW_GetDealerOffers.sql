IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetDealerOffers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetDealerOffers]
GO

	-- =============================================
-- Author:		Suresh Prajapati
-- Created date: 22nd Oct 2014
-- Description:	To get offer details provided by dealer
-- BW_GetDealerOffers 11, 25 , 361
-- Modified By : Suresh Prajapati on 07th Jan, 2014
-- Summary     : Added OfferTypeId fields.
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetDealerOffers] @DealerId INT
	,@ModelId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT PQO.Id
		,PQO.DealerId
		,BM.NAME MakeName
		,BMO.NAME ModelName
		,C.Name [CityName]
		,PQO.ModelId
		,PQO.OfferText
		,OC.OfferType
		,OC.Id AS OfferTypeId
		,ISNULL(PQO.OfferValue, 0) OfferValue
		,PQO.EntryDate
		,PQO.OfferValidTill
		,PQO.IsActive
	FROM [dbo].[BW_PQ_Offers] PQO WITH (NOLOCK)
	INNER JOIN BW_PQ_OfferCategories OC WITH (NOLOCK) ON PQO.OfferCategoryId = OC.Id
		AND OC.IsActive = 1
	INNER JOIN BikeModels BMO WITH (NOLOCK) ON BMO.ID = PQO.ModelId
		AND BMO.IsDeleted = 0 AND BMO.New=1
	INNER JOIN BikeMakes BM WITH (NOLOCK) ON BM.ID = BMO.BikeMakeId
		AND BM.IsDeleted = 0 AND BM.New=1
	INNER JOIN Cities C WITH (NOLOCK) ON C.ID=PQO.CityId AND C.IsDeleted=0
	WHERE (
			@DealerId IS NULL
			OR PQO.DealerId = @DealerId
			)
		AND (
			@ModelId IS NULL
			OR PQO.ModelId = @ModelId
			)
		AND PQO.IsActive = 1
	ORDER BY PQO.OfferValidTill DESC
END 

