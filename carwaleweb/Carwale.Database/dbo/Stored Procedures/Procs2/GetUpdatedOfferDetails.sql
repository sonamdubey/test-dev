IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUpdatedOfferDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUpdatedOfferDetails]
GO

	
-- ==========================================================================================
-- Author: Chetan Thambad
-- Create date: 16/12/2015
-- Description:	Get Offer Details
-- exec [dbo].[GetUpdatedOfferDetails] 
-- ==========================================================================================
CREATE PROCEDURE [dbo].[GetUpdatedOfferDetails] @updateId INT
AS
BEGIN
SELECT DO.OfferType,DO.OfferDescription,DO.OfferTitle,DO.MaxOfferValue,DO.Conditions,DO.StartDate,DO.EndDate,
DO.SourceCategory,DO.SourceDescription,DO.IsCountryWide,DO.OfferUnits,DO.ClaimedUnits,DO.HostURL,DO.ImageName,
DO.ImagePath,DO.PreBookingEmailIds,DO.PreBookingMobile,DO.CouponEmailIds,DO.CouponMobile,DO.DispOnMobile,DO.DispOnDesk,
DO.DispOnOffersPgDesk, DO.DispOnOffersPgMob, DO.DispSnippetOnDesk, DO.DispSnippetOnMob, DO.ShortDescription as PQSnippet, 
DOD.DealerId AS DealerId,DOD.ZoneId AS ZoneId,
DOV.VersionId AS VersionId,DOV.ModelId AS ModelId,DOV.MakeId AS MakeId,
CASE WHEN DOD.CityId = -1 THEN 'All Cities' WHEN DOD.ZoneId IS NOT NULL THEN '' ELSE C.Name END AS CityName,
CASE DOD.ZoneId WHEN NULL THEN '-' WHEN 645 THEN 'Thane with Octroi' ELSE CZ.ZoneName END AS ZoneName,
DOD.CityId,S.id AS StateId
FROM DealerOffers DO WITH(NOLOCK)
JOIN DealerOffersDealers DOD WITH(NOLOCK) ON DOD.OfferId = DO.ID
LEFT JOIN Dealers D WITH(NOLOCK) ON D.ID = DOD.DealerId
LEFT JOIN Cities C WITH(NOLOCK) ON C.ID = DOD.CityId
LEFT JOIN States S WITH(NOLOCK) ON S.ID = C.StateId
LEFT JOIN CityZones CZ WITH(NOLOCK) ON CZ.ID = DOD.ZoneId
LEFT JOIN DealerOffersVersion DOV WITH(NOLOCK) ON DOV.OfferId = DO.ID
WHERE DO.ID = @updateId
END

