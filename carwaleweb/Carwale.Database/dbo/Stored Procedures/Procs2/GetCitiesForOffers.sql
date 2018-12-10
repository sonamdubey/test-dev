IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCitiesForOffers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCitiesForOffers]
GO

	CREATE PROCEDURE [dbo].[GetCitiesForOffers]

AS
BEGIN

	SET NOCOUNT ON;
	
	SELECT 
	Distinct 
	CT.ID CityId, CT.Name AS Name
	FROM DealerOffersDealers DOD WITH (NOLOCK)
	JOIN DealerOffers DO  WITH (NOLOCK) ON DOD.OfferId = DO.ID 
	JOIN Cities CT WITH (NOLOCK) ON DOD.CityId = CT.ID
	WHERE 
	DO.IsActive = 1
	AND DO.IsApproved = 1
	AND DO.OfferType IN (1,3)
	AND Convert(DATE, DO.StartDate) <= Convert(DATE, GETDATE())
	AND Convert(DATE, DO.EndDate) >= Convert(DATE, GETDATE())
	AND ((DO.OfferUnits - DO.ClaimedUnits) > 0)
	AND CT.IsDeleted = 0 AND DOD.CityId <> -1
	-- Commented by vikas as performance was not good
	--UNION
	--SELECT DISTINCT CT.ID CityId, CT.Name AS Name
	--FROM DealerOffersDealers DOD
	--JOIN DealerOffers DO ON DOD.OfferId = DO.ID
	--JOIN DealerOffersVersion DOV ON DO.ID = DOV.OfferId
	--JOIN vwMMV VW ON DOV.MakeId = VW.MakeId
	--JOIN CW_NewCarShowroomPrices NCSP ON VW.VersionId = NCSP.CarVersionId
	--JOIN Cities CT ON NCSP.CityId = CT.ID
	--WHERE DOD.CityId = -1 
	--AND DO.IsActive = 1
	--AND DO.IsApproved = 1
	--AND DO.OfferType IN (1,3)
	--AND Convert(DATE, DO.StartDate) <= Convert(DATE, GETDATE())
	--AND Convert(DATE, DO.EndDate) >= Convert(DATE, GETDATE())
	--AND ((DO.OfferUnits - DO.ClaimedUnits) > 0)
	--AND CT.IsDeleted = 0
 ORDER BY Name

END

